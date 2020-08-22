using System.Collections;
using System.Collections.Generic;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.Events;
using Visual;

namespace Core.State
{
    class GameVisual : MonoBehaviour,IGameState
    {
        private BoardState m_boardState;

        private APieceFactory m_pieceFactory;

        private IPiece[,] m_visualState;

        public readonly UnityEvent OnBoardUpdateCompleted = new UnityEvent();

        private readonly StateEvent m_stateEndEvent = new StateEvent();

        public StateEvent OnStateEnd
        {
            get { return m_stateEndEvent; }
        }

        private readonly StateEvent m_stateBeginEvent = new StateEvent();
        
        public StateEvent OnStateBegin
        {
            get { return m_stateBeginEvent; }
        }

        public readonly VisualMatchEvent OnVisualMatchEvent = new VisualMatchEvent();

        public readonly VisualStackEndEvent OnVisualStackEndedEvent = new VisualStackEndEvent();

        [SerializeField]
        private Transform m_gemMaskPosition;

        public void Init(BoardState boardState, APieceFactory pieceFactory)
        {
            m_boardState = boardState;

            m_pieceFactory = pieceFactory;

            m_visualState = new IPiece[m_boardState.BoardSize.x, m_boardState.BoardSize.y];

            Shader.SetGlobalFloat("_YClip", m_gemMaskPosition.position.y);
        }

        public void StartGame()
        {

            for (int x = 0; x < m_boardState.BoardSize.x; x++)
            {
                for (int y = 0; y < m_boardState.BoardSize.y; y++)
                {
                    IPiece oldPiece = m_visualState[x, y];
                    if (oldPiece != null)
                    {
                        oldPiece.Dispose();
                    }

                    IPiece piece = m_pieceFactory.CreatePiece(m_boardState.State[x, y]);
                    piece.Init(x, m_boardState.BoardSize.y + y);
                    piece.Move(new Vector2Int(x,y));
                    m_visualState[x, y] = piece;
                }
            }
        }

        public void Begin()
        {
            m_stateBeginEvent.Invoke(this);
        }
        
        public void Tick()
        {
            
        }

        public void Select(params Vector2Int[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2Int pos = positions[i];
                if (m_boardState.IsInside(pos))
                {
                    m_visualState[pos.x, pos.y].Selected();
                }
            }
        }

        public void Hint(float amount, params Vector2Int[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2Int pos = positions[i];
                m_visualState[pos.x, pos.y].Hint(0, amount);
            }
        }

        public void UpdateVisualState(Queue<BoardUpdateData> boardUpdateDatas)
        {
            StartCoroutine(UpdateVisuals(boardUpdateDatas));
        }

        private IEnumerator UpdateVisuals(Queue<BoardUpdateData> boardUpdateDatas)
        {
            int numMatches = 0;
            int numVisualUpdateTarget = boardUpdateDatas.Count;
            int numVisualUpdate = 0;
            while (boardUpdateDatas.Count>0)
            {
                BoardUpdateData boardUpdateData = boardUpdateDatas.Dequeue();
                //do move pieces
                if (boardUpdateData.Moves.Count > 0)
                {
                    IPiece[] movedPieces = new IPiece[boardUpdateData.Moves.Count]; //Todo use buffering instead

                    for (int i = 0; i < boardUpdateData.Moves.Count; i++)
                    {
                        PieceMoveData moveData = boardUpdateData.Moves[i];
                        IPiece movedPiece;
                        if (moveData.NewPieceID>=0)
                        {
                            movedPiece = m_pieceFactory.CreatePiece(moveData.NewPieceID);
                            movedPiece.Init(moveData.Origin.x, moveData.Origin.y);

                        }
                        else
                        {
                            movedPiece = m_visualState[moveData.Origin.x, moveData.Origin.y];
                        }

                        movedPiece.Move(moveData.Destination);
                        movedPieces[i] = movedPiece;
                    }

                    //update pieces position
                    for (int i = 0; i < boardUpdateData.Moves.Count; i++)
                    {
                        PieceMoveData moveData = boardUpdateData.Moves[i];
                        m_visualState[moveData.Destination.x, moveData.Destination.y] = movedPieces[i];
                    }

                    yield return new WaitForSeconds(Constants.PieceFallDuration);
                }

                //handle matches
                numMatches += boardUpdateData.Matches.Length;
                numVisualUpdate++;
                
                if (boardUpdateData.Matches.Length > 0)
                {
                    for (int i = 0; i < boardUpdateData.Matches.Length; i++)
                    {
                        MatchData matchData = boardUpdateData.Matches[i];

                        for (int j = 0; j < matchData.Matches.Length; j++)
                        {
                            Vector2Int matchPosition = matchData.Matches[j];
                            IPiece movedPiece = m_visualState[matchPosition.x, matchPosition.y];
                            movedPiece.StartDisappear();
                        }

                        OnVisualMatchEvent.Invoke(matchData);
                    }

                    OnVisualStackEndedEvent.Invoke(numMatches, numVisualUpdate, numVisualUpdateTarget);
                    yield return new WaitForSeconds(Constants.PieceMatchDuration);
                }
                else
                {
                    OnVisualStackEndedEvent.Invoke(numMatches, numVisualUpdate, numVisualUpdateTarget);
                }
            }

           
            m_stateEndEvent.Invoke(this);
            OnBoardUpdateCompleted.Invoke();

            //visual candy if there's combo
            if (numMatches > 1)
            {
                float multiplier = 1f / m_boardState.BoardSize.magnitude;
                for (int x = 0; x < m_boardState.BoardSize.x; x++)
                {
                    for (int y = 0; y < m_boardState.BoardSize.y; y++)
                    {
                        m_visualState[x, y].Hint((x + y) * multiplier,.2f);
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            Shader.SetGlobalFloat("_YClip", m_gemMaskPosition.position.y);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(m_gemMaskPosition.position, m_gemMaskPosition.position + m_gemMaskPosition.right * 5f);
            UnityEditor.Handles.Label(m_gemMaskPosition.position, "Y Mask");

            if (!Application.isPlaying)
            {
                return;
            }

            for (int x = 0; x < m_boardState.BoardSize.x; x++)
            {
                for (int y = 0; y < m_boardState.BoardSize.y; y++)
                {
                    UnityEditor.Handles.Label(m_visualState[x, y].Transform.position, m_boardState.State[x, y].ToString());
                }
            }
        }
#endif
    }
}
