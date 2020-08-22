using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Visual
{
    class GemFactory : APieceFactory
    {
        [SerializeField]
        private Gem[] m_gemPrefabs;

        [SerializeField]
        private Transform m_spawnParent;

        [SerializeField]
        private bool m_centerBoard;

        private Stack<IPiece>[] m_pools;

        public override IPiece[] Prefabs
        {
            get { return m_gemPrefabs; }
        }

        public override IPiece CreatePiece(int index)
        {
            Stack<IPiece> pool = m_pools[index];

            IPiece piece;
            if (pool.Count == 0)
            {
                piece = Instantiate(m_gemPrefabs[index], m_spawnParent, false);
                piece.PoolId = index;
                piece.OnDisposed.AddListener(OnPieceDisposed);
                piece.Create();
            }
            else
            {
                piece = pool.Pop();
            }

            return piece;
        }

        private void OnPieceDisposed(IPiece piece)
        {
            m_pools[piece.PoolId].Push(piece);
        }

        public override void Init(BoardState boardState)
        {
            m_pools = new Stack<IPiece>[m_gemPrefabs.Length];

            for (int i = 0; i < m_pools.Length; i++)
            {
                m_pools[i] = new Stack<IPiece>();
            }

            //center the board
            if (m_centerBoard)
            {
                m_spawnParent.position = new Vector3((boardState.BoardSize.x - 1) * Constants.GridSize, (boardState.BoardSize.y - 1) * Constants.GridSize) * -.5f;
            }
        }
    }
}
