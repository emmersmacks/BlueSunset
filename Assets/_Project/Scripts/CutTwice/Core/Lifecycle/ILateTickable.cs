namespace CutTwice.Core.Lifecycle
{
    public interface ILateTickable : ILifecycleObject
    {
        void LateTick();
    }
}