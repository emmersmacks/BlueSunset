namespace CutTwice.Gameplay.Modes
{
    public class GameModeContext
    {
        public GameModeConfig CurrentMode { get; private set; } = new EndlessModeConfig();

        public void SetMode(GameModeConfig mode)
        {
            CurrentMode = mode ?? new EndlessModeConfig();
        }
    }
}
