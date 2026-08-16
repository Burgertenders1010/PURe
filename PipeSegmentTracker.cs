using System.Collections.Generic;

namespace PURepipeconnectorsys
{
    public static class PipeSegmentTracker
    {
        public static Dictionary<ushort, ushort> RoadToPipeSegment = new Dictionary<ushort, ushort>();
        public static Dictionary<ushort, ushort> RoadToPipeNode = new Dictionary<ushort, ushort>();
    }
}
