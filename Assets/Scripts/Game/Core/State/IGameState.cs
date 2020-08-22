using Core.GameEvent;

namespace Core.State
{
    public interface IGameState
    {
        StateEvent OnStateEnd { get; }

        StateEvent OnStateBegin { get; }

        //initialization
        void Init(BoardState boardState, APieceFactory pieceFactory);

        //start game
        void StartGame();

        //initialize state begin
        void Begin();

        //Update call
        void Tick();
    }
}
