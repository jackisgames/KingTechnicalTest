using UnityEngine.Events;

namespace Core.GameEvent
{
    /// <summary>
    /// Num matches, current execution stack count, target execution stack count
    /// </summary>
    class VisualStackEndEvent:UnityEvent<int,int,int>
    {
    }
}
