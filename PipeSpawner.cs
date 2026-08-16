using ColossalFramework;
using UnityEngine;

namespace PURepipeconnectorsys
{
    public static class PipeSpawner
    {
        private const float BurialOffset = -6f;

        public static NetInfo PipePrefab;

        public static void SpawnPipeForSegment(ushort roadSegmentID, ref NetSegment roadData)
        {
            if (PipePrefab == null)
                PipePrefab = FindDefaultPipePrefab();

            if (PipePrefab == null) return;

            NetManager netManager = Singleton<NetManager>.instance;

            ushort startNodeID = roadData.m_startNode;
            ushort endNodeID = roadData.m_endNode;

            Vector3 startNodePos = netManager.m_nodes.m_buffer[startNodeID].m_position + new Vector3(0, BurialOffset, 0);
            Vector3 endNodePos = netManager.m_nodes.m_buffer[endNodeID].m_position + new Vector3(0, BurialOffset, 0);

            ushort pipeStartNode = GetOrCreatePipeNode(startNodeID, startNodePos);
            ushort pipeEndNode = GetOrCreatePipeNode(endNodeID, endNodePos);

            ushort newSegmentID;
            bool success = netManager.CreateSegment(out newSegmentID, ref Singleton<SimulationManager>.instance.m_randomizer,
                PipePrefab, pipeStartNode, pipeEndNode,
                roadData.m_startDirection, roadData.m_endDirection,
                Singleton<SimulationManager>.instance.m_currentBuildIndex,
                Singleton<SimulationManager>.instance.m_currentBuildIndex, false);

            if (success)
                PipeSegmentTracker.RoadToPipeSegment[roadSegmentID] = newSegmentID;
        }

        public static void RemovePipeForSegment(ushort roadSegmentID)
        {
            if (!PipeSegmentTracker.RoadToPipeSegment.TryGetValue(roadSegmentID, out ushort pipeSegmentID))
                return;

            NetManager netManager = Singleton<NetManager>.instance;
            netManager.ReleaseSegment(pipeSegmentID, false);
            PipeSegmentTracker.RoadToPipeSegment.Remove(roadSegmentID);
        }

        public static void UpdatePipeForSegment(ushort roadSegmentID, ref NetSegment roadData)
        {
            if (PipeSegmentTracker.RoadToPipeSegment.ContainsKey(roadSegmentID))
                RemovePipeForSegment(roadSegmentID);

            SpawnPipeForSegment(roadSegmentID, ref roadData);
        }

        private static ushort GetOrCreatePipeNode(ushort roadNodeID, Vector3 position)
        {
            if (PipeSegmentTracker.RoadToPipeNode.TryGetValue(roadNodeID, out ushort existing))
                return existing;

            NetManager netManager = Singleton<NetManager>.instance;
            ushort newNodeID;
            netManager.CreateNode(out newNodeID, ref Singleton<SimulationManager>.instance.m_randomizer,
                PipePrefab, position, Singleton<SimulationManager>.instance.m_currentBuildIndex);

            PipeSegmentTracker.RoadToPipeNode[roadNodeID] = newNodeID;
            return newNodeID;
        }

        private static NetInfo FindDefaultPipePrefab()
        {
            return PrefabCollection<NetInfo>.FindLoaded("Water Pipe");
        }
    }
}
