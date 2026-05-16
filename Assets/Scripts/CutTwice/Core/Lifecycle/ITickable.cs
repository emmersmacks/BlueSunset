namespace CutTwice.Common
{
    /// <summary>
    /// Simple interface for components that require update every frame.
    /// </summary>
    public interface ITickable
    {
        void Tick();
    }
}