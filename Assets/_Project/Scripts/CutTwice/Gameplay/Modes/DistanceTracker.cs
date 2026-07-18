using CutTwice.Core.EventBus;
using CutTwice.Core.Lifecycle;
using CutTwice.Gameplay.Runtime.Road.Components;
using UnityEngine;

namespace CutTwice.Gameplay.Modes
{
    public class DistanceTracker : ITickable
    {
        private readonly InfiniteRoadController _roadController;
        private readonly GameSession _gameSession;
        private readonly GameModeContext _gameModeContext;
        private readonly IEventBus _eventBus;

        private bool _goalReached;

        public float DistanceMeters { get; private set; }

        public DistanceTracker(InfiniteRoadController roadController, GameSession gameSession, GameModeContext gameModeContext, IEventBus eventBus)
        {
            _roadController = roadController;
            _gameSession = gameSession;
            _gameModeContext = gameModeContext;
            _eventBus = eventBus;
        }

        public void Tick()
        {
            if (!_gameSession.IsRunning || _goalReached)
                return;

            DistanceMeters += _roadController.MovementSpeed * Time.deltaTime;

            if (_gameModeContext.CurrentMode is DistanceModeConfig distanceMode &&
                DistanceMeters >= distanceMode.TargetMeters)
            {
                _goalReached = true;
                _eventBus.Publish(new RunCompletedEvent());
            }
        }
    }
}
