using UnityEngine;

namespace Core.GameEvent
{
    public struct MatchData
    {
        public Vector2Int[] Matches;

        public MatchData(params Vector2Int[] matches)
        {
            Matches = matches;
        }
    }
}
