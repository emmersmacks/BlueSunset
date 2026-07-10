using System;

namespace CutTwice.Core.RivletUI
{
    public interface IWindowController
    {
        void Show(object payload = null);

        void Hide();
    }
}


