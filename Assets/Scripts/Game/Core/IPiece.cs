using Core.GameEvent;
using UnityEngine;

namespace Core
{
    public interface IPiece
    {
        Transform Transform { get; }

        Vector3 GetPosition(int x, int y);

        int PoolId { get; set; }

        void Create();

        void Init(int x, int y);

        void Hint(float delay,float amount);

        void Selected();

        void StartDisappear();

        void Move(Vector2Int destination);

        void Dispose();

        PieceDisposedEvent OnDisposed { get; }
    }
}
