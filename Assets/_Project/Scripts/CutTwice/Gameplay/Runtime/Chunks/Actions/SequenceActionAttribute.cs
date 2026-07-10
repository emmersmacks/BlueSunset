using System;
using CutTwice.Gameplay.Runtime.Chunks.ModuleLoader.Dto;

namespace CutTwice.Gameplay.Runtime.Chunks.Actions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SequenceActionAttribute : Attribute
    {
        public ActionType ActionType { get; }

        public SequenceActionAttribute(ActionType actionType)
        {
            ActionType = actionType;
        }
    }
}