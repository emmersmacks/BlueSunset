using System;
using CutTwice.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CutTwice.Controllers
{
    public class ObjectMoverController : ITickable
    {
        private readonly ObjectMoverPresenter _presenter;

        private float _elapsedTime;
        private Vector3 _initialPosition;

        public Action OnFinished;
        
        private bool _finished;

        public ObjectMoverController(ObjectMoverPresenter presenter)
        {
            _presenter = presenter;
            _initialPosition = _presenter.transform.position;
        }

        public void Tick()
        {
            if (_finished) return;

            _elapsedTime += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(_elapsedTime / _presenter.Duration);
            float movementFactor = _presenter.MovementCurve.Evaluate(normalizedTime);

            Vector3 newPosition = _initialPosition + _presenter.MoveDirection * movementFactor;
            _presenter.transform.position = newPosition;

            if (normalizedTime >= 1f)
            {
                _finished = true;
                OnFinished?.Invoke();
            }
        }
    }
}


