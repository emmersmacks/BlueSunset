namespace CutTwice.Core.Lifecycle
{
    public interface IFixedTickable : ILifecycleObject
    {
        void FixedTick();
    }
}