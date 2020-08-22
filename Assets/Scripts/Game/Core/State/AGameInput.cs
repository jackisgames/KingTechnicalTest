using Core.GameEvent;
using UnityEngine;

namespace Core.State
{
    public abstract class AGameInput:MonoBehaviour,IGameState
    {
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

        /// <summary>
        /// When the swap is confirmed
        /// </summary>
        public readonly SwapEvent OnSwapEvent = new SwapEvent();

        /// <summary>
        /// When input is over but not selected
        /// </summary>
        public readonly PieceEvent OnHoverEvent = new PieceEvent();

        /// <summary>
        /// When origin gem is selected
        /// </summary>
        public readonly PieceEvent OnOriginSelectedEvent = new PieceEvent();

        /// <summary>
        /// When destination gem is selected
        /// </summary>
        public readonly PieceEvent OnDestinationSelectedEvent = new PieceEvent();

        public abstract void Init(BoardState boardState, APieceFactory pieceFactory);

        public abstract void StartGame();

        public abstract void Begin();

        public abstract void Tick();
    }
}
