namespace CutTwice.Gameplay.Modes
{
    public sealed class DistanceModeConfig : GameModeConfig
    {
        public float TargetMeters { get; }

        public DistanceModeConfig(float targetMeters)
        {
            TargetMeters = targetMeters;
        }
    }
}
