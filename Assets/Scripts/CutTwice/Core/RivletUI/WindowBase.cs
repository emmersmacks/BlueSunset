using System.Collections.Generic;
using CutTwice.Core.RivletUI;
using UnityEngine;

namespace CutTwice.UI
{
    public class WindowBase : IWindow
    {
        private readonly GameObject _windowObject;
        protected List<IWindowController> Controllers = new();
        
        public WindowBase(GameObject windowObject)
        {
            _windowObject = windowObject;
            _windowObject.SetActive(false);
        }
        
        protected void Register(IWindowController controller)
        {
            Controllers.Add(controller);
        }
        
        public void Show(object payload = null)
        {
            _windowObject.SetActive(true);
            foreach (var controller in Controllers)
            {
                controller.Show(payload);
            }
        }

        public void Hide()
        {
            foreach (var controller in Controllers)
            {
                controller.Hide();
            }
            _windowObject.SetActive(false);
        }
    }
}