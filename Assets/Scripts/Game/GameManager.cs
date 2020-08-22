using System.Collections.Generic;
using Core;
using Core.GameEvent;
using Core.State;
using GameAudio;
using GameUI;
using UnityEngine;
using Visual;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_gameDuration = 60;

    [SerializeField]
    private Vector2Int m_boardSize = new Vector2Int(8, 8);

    [SerializeField]
    private APieceFactory m_pieceFactory;
    
    [SerializeField]
    private GameVisual m_visual;

    [SerializeField]
    private AGameInput m_input;

    [SerializeField]
    private UIHandler m_uiHandler;

    [SerializeField]
    private AudioManager m_audioManager;

    private readonly GameLogic m_logic = new GameLogic();

    private Dictionary<Vector2Int, int> m_scoreTextGroup = new Dictionary<Vector2Int, int>();

    private IGameState m_currentState;

    private BoardState m_boardState;

    private bool m_startCountDown;

    private float m_timer;

    private bool m_hintEnabled;

    private float m_hintTimer;

    private int m_score;

    private PieceMoveData m_hintMove;

    private void Start()
    {
        m_boardState = new BoardState(m_boardSize);

        m_pieceFactory.Init(m_boardState);

        m_logic.Init(m_boardState, m_pieceFactory);
        m_logic.OnStateBegin.AddListener(OnStateStarted);
        m_logic.OnStateEnd.AddListener(OnStateEnded);
        m_logic.OnSwapResult.AddListener(OnSwapResult);

        m_visual.Init(m_boardState, m_pieceFactory);
        m_visual.OnStateBegin.AddListener(OnStateStarted);
        m_visual.OnStateEnd.AddListener(OnStateEnded);
        m_visual.OnVisualMatchEvent.AddListener(OnVisualMatch);
        m_visual.OnVisualStackEndedEvent.AddListener(OnVisualMatchEnded);
        m_visual.OnBoardUpdateCompleted.AddListener(OnBoardVisualCompleted);

        m_input.Init(m_boardState, m_pieceFactory);
        m_input.OnStateBegin.AddListener(OnStateStarted);
        m_input.OnStateEnd.AddListener(OnStateEnded);
        m_input.OnHoverEvent.AddListener(OnPieceHover);
        m_input.OnSwapEvent.AddListener(OnPieceSwap);

        m_uiHandler.Init();
        m_uiHandler.OnPlayAgainButtonClicked.AddListener(StartGame);

        StartGame();

        
    }
    
    private void StartGame()
    {
        m_input.Begin();
        m_timer = m_gameDuration;
        m_score = 0;
        m_hintTimer = 0;
        m_hintEnabled = true;
        m_uiHandler.UpdateTimer(m_timer);
        m_uiHandler.UpdateScore(m_score);

        m_uiHandler.StartGame();
        m_logic.StartGame();
        m_visual.StartGame();
        m_input.StartGame();

        if (!m_logic.GetHint(out m_hintMove))
        {
            //no solution restart game
            StartGame();
        }

    }

    private void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.Tick();
        }

        if (m_startCountDown)
        {
            if (m_timer > 0)
            {
                m_timer -= Time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = 0;
                    if (m_currentState == m_input)
                    {
                        //only end game when in input state
                        EndGame();
                        m_currentState = null;
                    }
                }
                m_uiHandler.UpdateTimer(m_timer);
            }

            if (m_hintEnabled)
            {
                if (m_hintTimer >= Constants.HintDuration)
                {
                    m_visual.Hint(1f, m_hintMove.Origin, m_hintMove.Destination);
                    m_hintTimer = 0;
                }
                else
                {
                    m_hintTimer += Time.deltaTime;
                }
            }
        }
    }

    private void EndGame()
    {
        m_startCountDown = false;
        m_audioManager.PlayAudio(ESfxType.Gameover);
        m_uiHandler.EndGame(m_score);
    }

    private void OnStateStarted(IGameState state)
    {
        m_currentState = state;
    }

    private void OnStateEnded(IGameState state)
    {
        if (m_currentState == state)
        {
            m_currentState = null;
        }
    }

    private void OnPieceHover(Vector2Int position)
    {
        m_visual.Select(position);
    }

    /// <summary>
    /// Getting input feed that input to logic
    /// </summary>
    private void OnPieceSwap(PieceMoveData pieceMoveData)
    {
        m_startCountDown = true;
        m_hintTimer = 0;
        //check if origin and destination inside bound
        if (m_boardState.IsOutside(pieceMoveData.Origin) || m_boardState.IsOutside(pieceMoveData.Destination))
        {
            m_input.Begin();
            return;
        }

        m_hintEnabled = false;
        m_logic.Begin();
        m_logic.DoSwap(pieceMoveData);
    }

    /// <summary>
    /// Logic update board state, update visual to match current board state
    /// </summary>
    private void OnSwapResult(Queue<BoardUpdateData> swapResult)
    {
        m_visual.Begin();
        m_visual.UpdateVisualState(swapResult);
    }

    /// <summary>
    /// Visual done updating, here we should decide to end the game or give state back to input if there's possible move
    /// </summary>
    private void OnBoardVisualCompleted()
    {
        if (m_timer > 0 && m_logic.GetHint(out m_hintMove))
        {
            m_hintEnabled = true;
            m_hintTimer = 0;
            m_input.Begin();
        }
        else
        {
            EndGame();
        }
    }

    /// <summary>
    /// visual display the match, use this to sync vfx etc.
    /// </summary>
    private void OnVisualMatch(MatchData matchData)
    {
        int score = Constants.ScorePerMatch + matchData.Matches.Length * Constants.ScorePerGem;
        m_score += score;

        Vector2Int center = Vector2Int.zero;

        for (int i = 0; i < matchData.Matches.Length; i++)
        {
            Vector2Int pos = matchData.Matches[i];
            center += pos;
        }

        center.x = center.x / matchData.Matches.Length;
        center.y = center.y / matchData.Matches.Length;

        int groupScore;
        if (!m_scoreTextGroup.TryGetValue(center, out groupScore))
        {
            groupScore = 0;
        }

        m_scoreTextGroup[center] = groupScore + score;

    }

    /// <summary>
    /// Visual match of current execution stack just ended, play audio or display progressive vfx here
    /// </summary>
    private void OnVisualMatchEnded(int matchesCount, int currentStack, int targetStack)
    {
        if (currentStack == 1)
        {
            m_audioManager.PlayAudio(ESfxType.SwapStart);
        }
        else if (targetStack == 2 && matchesCount == 0)
        {
            m_audioManager.PlayAudio(ESfxType.SwapFail);
        }
        else
        {
            m_audioManager.PlayAudio(ESfxType.Match);
        }

        if (m_scoreTextGroup.Count > 0)
        {
            Dictionary<Vector2Int, int>.Enumerator numerator = m_scoreTextGroup.GetEnumerator();
            while (numerator.MoveNext())
            {
                Vector2Int center = numerator.Current.Key;
                m_uiHandler.UpdateScore(new Vector3(center.x * Constants.GridSize, center.y * Constants.GridSize), numerator.Current.Value, m_score);
            }

            numerator.Dispose();

            m_scoreTextGroup.Clear();
        }
    }
}
