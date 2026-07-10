namespace CutTwice.Core.Lifecycle
{
    /// <summary>
    /// Simple interface for components that require update every frame.
    /// </summary>
    public interface ITickable : ILifecycleObject
    {
        void Tick();
    }
}