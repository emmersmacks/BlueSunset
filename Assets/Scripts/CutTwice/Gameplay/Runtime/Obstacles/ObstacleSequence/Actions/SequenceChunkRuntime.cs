using System.Collections.Generic;

namespace CutTwice.ObstacleSequence.Actions
{
    public class SequenceChunkRuntime
    {
        public string Name;
        public List<ISequenceActionRuntime> Actions = new();
    }
}