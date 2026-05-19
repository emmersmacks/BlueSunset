using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CutTwice.Gameplay.Runtime.Scenario.Stages
{
    public class InitialStage : ScenarioStage
    {
        private Transform _player;
        private Transform _initialPosition;

        public InitialStage(Transform player, Transform initialPosition)
        {
            _player = player;
            _initialPosition = initialPosition;
        }
        
        public override void StageStart()
        {
            var carPos = _initialPosition.position;
            carPos.y = _player.position.y;
            _player.position = carPos;
        }

        public override UniTask InitAsync(CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        public override bool StageComplete()
        {
            return true;
        }
    }
}