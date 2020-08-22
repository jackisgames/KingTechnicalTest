using System.Collections.Generic;

namespace Core.GameEvent
{
    public struct BoardUpdateData
    {
        public List<PieceMoveData> Moves;
        public MatchData[] Matches;

        public BoardUpdateData(List<PieceMoveData> moves, MatchData[] matches)
        {
            Moves = moves;
            Matches = matches;
        }
    }
}
