using UnityEngine;

namespace Core
{
    public abstract class APieceFactory:MonoBehaviour
    {
        public abstract IPiece[] Prefabs { get; }

        public abstract void Init(BoardState boardState);

        public abstract IPiece CreatePiece(int index);

    }
}
