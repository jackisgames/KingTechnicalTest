using UnityEngine;

namespace Core
{
    public class BoardState
    {
        public readonly Vector2Int BoardSize;

        public readonly int[,] State;

        public BoardState(Vector2Int boardSize)
        {
            BoardSize = boardSize;
            State = new int[boardSize.x, boardSize.y];
        }

        public bool IsOutside(Vector2Int position)
        {
            return position.x < 0 || position.y < 0 || position.x >= BoardSize.x || position.y >= BoardSize.y;
        }

        public bool IsInside(Vector2Int position)
        {
            return !IsOutside(position);
        }

        /// <summary>
        /// Copy current board state to target state
        /// </summary>
        public void CopyState(BoardState target)
        {
            System.Buffer.BlockCopy(State, 0, target.State, 0, State.Length * 4);
            //System.Array.Copy(State, 0, target.State, 0, State.Length);
        }
    }
}
