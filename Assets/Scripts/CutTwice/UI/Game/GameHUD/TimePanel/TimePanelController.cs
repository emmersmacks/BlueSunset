using System;
using CutTwice.Common;
using CutTwice.Core.RivletUI;
using CutTwice.Game;

namespace CutTwice.UI
{
    public class TimePanelController : WindowControllerBase<TimePanelView>, ITickable
    {
        private readonly GameSession _gameSession;

        public TimePanelController(TimePanelView view, GameSession gameSession) : base(view)
        {
            _gameSession = gameSession;
        }

        public void Tick()
        {
            TimeSpan t = TimeSpan.FromSeconds(_gameSession.SessionTime);
            View.TimeText.text = $"{(int)t.TotalHours:00}:{t.Minutes:00}:{t.Seconds:00}";
        }
    }
}