using CutTwice.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CutTwice.Controllers
{
    public class PlayerInputController : ITickable
    {
        private PlayerCarController _playerCarController;
        public bool Enabled;

        public PlayerInputController(PlayerCarController playerCarController)
        {
            _playerCarController = playerCarController;
        }

        public void Tick()
        {
            if (!Enabled)
            {
                return;
            }
            
            // --- Mobile ---
            if (Input.touchCount > 0)
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject())
                    return;
                
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current != null &&
                        EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        return;

                    HandleInput(touch.position);
                }

                return; 
            }

            // --- PC ---
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject())
                    return;

                HandleInput(Input.mousePosition);
            }
        }

        void HandleInput(Vector2 position)
        {
            if (position.x < Screen.width / 2f)
            {
                OnLeftSideClick();
            }
            else
            {
                OnRightSideClick();
            }
        }

        void OnLeftSideClick()
        {
            _playerCarController.MoveLeft();
        }

        void OnRightSideClick()
        {
            _playerCarController.MoveRight();
        }
    }
}