using System.Collections.Generic;
using UnityEngine.Events;

namespace Core.GameEvent
{
    public class SwapResultEvent : UnityEvent<Queue<BoardUpdateData>>
    {
    }
}
