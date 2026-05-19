using System.Collections.Generic;

namespace CutTwice.Gameplay.Runtime.Obstacles.ObstacleSequence.Actions
{
    public class SequenceChunkRuntime
    {
        public string Name;
        public List<ISequenceActionRuntime> Actions = new();
    }
}