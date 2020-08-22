using System.Collections.Generic;
using Core.GameEvent;
using UnityEngine;

namespace Core.State
{
    public class GameLogic: IGameState
    {
        private static readonly Stack<Vector2Int> MatchCheckBuffer = new Stack<Vector2Int>();

        private const int Blank = -1;

        private BoardState m_boardState;

        private BoardState m_boardStateTemp;

        private APieceFactory m_pieceFactory;

        private readonly List<MatchData> m_currentStateMatchData = new List<MatchData>();

        public readonly SwapResultEvent OnSwapResult = new SwapResultEvent();

        public List<MatchData> CurrentStateMatchData
        {
            get { return m_currentStateMatchData; }
        }

        private readonly StateEvent m_onStateBegin = new StateEvent();

        public StateEvent OnStateBegin
        {
            get { return m_onStateBegin; }
        }

        private readonly StateEvent m_onStateEnd = new StateEvent();

        public StateEvent OnStateEnd
        {
            get { return m_onStateEnd; }
        }

        public void Init(BoardState boardState,APieceFactory pieceFactory)
        {
            m_boardState = boardState;

            m_boardStateTemp = new BoardState(m_boardState.BoardSize);

            m_pieceFactory = pieceFactory;
            
        }

        /// <summary>
        /// This will fill the space with random gem
        /// </summary>
        public void StartGame()
        {
            RandomFill();
        }

        public void Begin()
        {
            m_onStateBegin.Invoke(this);

        }

        public void Tick()
        {
        }

        /// <summary>
        /// This will also check if swap possible
        /// </summary>
        public void DoSwap(PieceMoveData pieceMoveData)
        {
            m_boardState.CopyState(m_boardStateTemp);

            Queue<BoardUpdateData> boardUpdateData = new Queue<BoardUpdateData>();
            //swap
            m_boardStateTemp.State[pieceMoveData.Origin.x, pieceMoveData.Origin.y] = m_boardState.State[pieceMoveData.Destination.x, pieceMoveData.Destination.y];
            m_boardStateTemp.State[pieceMoveData.Destination.x, pieceMoveData.Destination.y] = m_boardState.State[pieceMoveData.Origin.x, pieceMoveData.Origin.y];
            //
            GetAllMatches(m_boardStateTemp, m_currentStateMatchData);


            boardUpdateData.Enqueue(new BoardUpdateData(new List<PieceMoveData>() { pieceMoveData, pieceMoveData.Inverse }, CopyMatchData(m_currentStateMatchData)));//move pieces towards move

            if (m_currentStateMatchData.Count > 0)
            {
                //grab lowest position that can be filled for each column
                int[] columnLowest = new int[m_boardStateTemp.BoardSize.x];

                for (int i = 0; i < m_boardStateTemp.BoardSize.x; i++)
                {
                    columnLowest[i] = m_boardStateTemp.BoardSize.y - 1;
                }

                while (m_currentStateMatchData.Count > 0)
                {
                    for (int i = 0; i < m_currentStateMatchData.Count; i++)
                    {
                        MatchData matchData = m_currentStateMatchData[i];
                        for (int j = 0; j < matchData.Matches.Length; j++)
                        {
                            Vector2Int matchPosition = matchData.Matches[j];
                            //mark empty space
                            m_boardStateTemp.State[matchPosition.x, matchPosition.y] = Blank;

                            //mark column that can be filled
                            columnLowest[matchPosition.x] = Mathf.Min(columnLowest[matchPosition.x], matchPosition.y);
                        }
                    }

                    //drop all pieces to bottom
                    List<PieceMoveData> currentPieceDropDatas = new List<PieceMoveData>();

                    for (int x = 0; x < m_boardStateTemp.BoardSize.x; x++)
                    {
                        int lowestColumn = columnLowest[x];

                        //only do piece adjusting on column that have a match
                        //move pieces downwards and register matches
                        for (int y = lowestColumn; y < m_boardStateTemp.BoardSize.y; y++)
                        {
                            int value = m_boardStateTemp.State[x, y];

                            if (value != Blank)
                            {
                                m_boardStateTemp.State[x, lowestColumn] = value;

                                currentPieceDropDatas.Add(new PieceMoveData(-1, new Vector2Int(x, y), new Vector2Int(x, lowestColumn)));

                                lowestColumn++;
                                columnLowest[x] = lowestColumn;
                            }
                        }

                        //drop new pieces to fill the blank spaces
                        for (int y = lowestColumn; y < m_boardStateTemp.BoardSize.y; y++)
                        {
                            int value = Random.Range(0, m_pieceFactory.Prefabs.Length);
                            m_boardStateTemp.State[x, y] = value;

                            currentPieceDropDatas.Add(new PieceMoveData(value, new Vector2Int(x, m_boardStateTemp.BoardSize.y + (y - lowestColumn)), new Vector2Int(x, y)));
                        }

                    }

                    GetAllMatches(m_boardStateTemp, m_currentStateMatchData);

                    boardUpdateData.Enqueue(new BoardUpdateData(currentPieceDropDatas, CopyMatchData(m_currentStateMatchData)));
                }
                //drop gems also handle matches if there's any

                m_boardStateTemp.CopyState(m_boardState);
            }
            else
            {
                //no matches
                boardUpdateData.Enqueue(new BoardUpdateData(new List<PieceMoveData>() { pieceMoveData, pieceMoveData.Inverse }, new MatchData[0]));//move piece back
            }

            OnSwapResult.Invoke(boardUpdateData);
        }

        /// <summary>
        /// Update the match data buffer from board state
        /// </summary>
        private static void GetAllMatches(BoardState boardState,List<MatchData> matchDatas,int maxMatch=int.MaxValue)
        {

            matchDatas.Clear();

            //horizontal matches
            for (int y = 0; y < boardState.BoardSize.y; y++)
            {
                int pointer = boardState.State[0, y];
                MatchCheckBuffer.Clear();
                MatchCheckBuffer.Push(new Vector2Int(0, y));

                for (int x = 1; x < boardState.BoardSize.x; x++)
                {
                    int currentValue = boardState.State[x, y];
                    if (pointer != currentValue || currentValue == Blank)
                    {
                        if (MatchCheckBuffer.Count >= 3)
                        {
                            matchDatas.Add(new MatchData(MatchCheckBuffer.ToArray()));
                            if (matchDatas.Count == maxMatch)
                            {
                                return;
                            }
                        }

                        MatchCheckBuffer.Clear();
                        pointer = currentValue;
                    }

                    MatchCheckBuffer.Push(new Vector2Int(x, y));
                }

                if (MatchCheckBuffer.Count >= 3)
                {
                    matchDatas.Add(new MatchData(MatchCheckBuffer.ToArray()));
                    if (matchDatas.Count == maxMatch)
                    {
                        return;
                    }
                }
            }



            //vertical matches
            for (int x = 0; x < boardState.BoardSize.x; x++)
            {
                int pointer = boardState.State[x,0];
                MatchCheckBuffer.Clear();
                MatchCheckBuffer.Push(new Vector2Int(x, 0));
                for (int y = 1; y < boardState.BoardSize.y; y++)
                {
                    int currentValue = boardState.State[x, y];
                    if (pointer != currentValue || currentValue == Blank)
                    {
                        if (MatchCheckBuffer.Count >= 3)
                        {
                            matchDatas.Add(new MatchData(MatchCheckBuffer.ToArray()));
                            if (matchDatas.Count == maxMatch)
                            {
                                return;
                            }
                        }
                        MatchCheckBuffer.Clear();
                        pointer = currentValue;
                    }
                    MatchCheckBuffer.Push(new Vector2Int(x, y));
                }
                if (MatchCheckBuffer.Count >= 3)
                {
                    matchDatas.Add(new MatchData(MatchCheckBuffer.ToArray()));
                    if (matchDatas.Count == maxMatch)
                    {
                        return;
                    }
                }
            }
        }

        private void RandomFill()
        {
            for (int x = 0; x < m_boardState.BoardSize.x; x++)
            {
                for (int y = 0; y < m_boardState.BoardSize.y; y++)
                {
                    m_boardState.State[x, y] = Random.Range(0, m_pieceFactory.Prefabs.Length);
                }
            }


            GetAllMatches(m_boardState,m_currentStateMatchData);

            //replace only matched pieces
            while (m_currentStateMatchData.Count > 0)
            {
                for (int i = 0; i < m_currentStateMatchData.Count; i++)
                {
                    MatchData matchData = m_currentStateMatchData[i];
                    for (int j = 0; j < matchData.Matches.Length; j++)
                    {
                        Vector2Int matchPosition = matchData.Matches[j];

                        m_boardState.State[matchPosition.x, matchPosition.y] = Random.Range(0, m_pieceFactory.Prefabs.Length);
                    }
                }

                GetAllMatches(m_boardState,m_currentStateMatchData);
            }
        }

        public bool GetHint(out PieceMoveData moveData)
        {
            m_boardState.CopyState(m_boardStateTemp);
            //check horizontal swipe
            for (int x = 0; x < m_boardStateTemp.BoardSize.x-1; x++)
            {
                for (int y = 0; y< m_boardStateTemp.BoardSize.y; y++)
                {
                    Vector2Int positionA = new Vector2Int(x, y);
                    Vector2Int positionB = new Vector2Int(x + 1, y);

                    if (GetHint(positionA, positionB, m_boardStateTemp))
                    {
                        moveData = new PieceMoveData(-1, positionA, positionB);
                        return true;
                    }
                }
            }

            for (int y = 0; y < m_boardStateTemp.BoardSize.y - 1; y++)
            {
                for (int x = 0; x < m_boardStateTemp.BoardSize.x; x++)
                {
                    Vector2Int positionA = new Vector2Int(x, x);
                    Vector2Int positionB = new Vector2Int(x, y + 1);

                    if (GetHint(positionA,positionB,m_boardStateTemp))
                    {
                        moveData = new PieceMoveData(-1, positionA, positionB);
                        return true;
                    }
                }
            }

            moveData = new PieceMoveData();
            return false;
        }

        private bool GetHint(Vector2Int positionA, Vector2Int positionB,BoardState boardState)
        {
            int valueA = boardState.State[positionA.x, positionA.y];
            int valueB = boardState.State[positionB.x, positionB.y];

            //swap
            boardState.State[positionA.x, positionA.y] = valueB;
            boardState.State[positionB.x, positionB.y] = valueA;

            GetAllMatches(boardState, m_currentStateMatchData, 1);

            //set value back
            boardState.State[positionA.x, positionA.y] = valueA;
            boardState.State[positionB.x, positionB.y] = valueB;

            return m_currentStateMatchData.Count > 0;
        }

        private MatchData[] CopyMatchData(List<MatchData> source)
        {
            MatchData[] result = new MatchData[source.Count];
            source.CopyTo(result);
            return result;
        }
    }
}
