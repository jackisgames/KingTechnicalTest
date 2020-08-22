using UnityEngine;

namespace Core.GameEvent
{
    public struct PieceMoveData
    {
        public readonly Vector2Int Origin;
        public readonly Vector2Int Destination;
        public readonly int NewPieceID;//new piece if more than zero

        public PieceMoveData(int newPieceId,Vector2Int origin,Vector2Int destination)
        {
            NewPieceID = newPieceId;
            Origin = origin;
            Destination = destination;
        }

        public PieceMoveData Inverse { get { return new PieceMoveData(NewPieceID,Destination, Origin);} }
    }
}
