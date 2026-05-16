namespace CutTwice.Core.RivletUI
{
    public class OpenWindowRequest<TWindow> where TWindow : IWindow
    {
        public readonly object Payload;
        public OpenWindowRequest(object payload = null) { Payload = payload; }
    }

    public class CloseWindowRequest<TWindow> where TWindow : IWindow
    {
        public CloseWindowRequest() { }
    }

    public class PushWindowRequest<TWindow> where TWindow : IWindow
    {
        public readonly object Payload;
        public PushWindowRequest(object payload = null) { Payload = payload; }
    }

    public class PopWindowRequest
    {
        public PopWindowRequest() { }
    }

    public class PopToWindowRequest<TWindow> where TWindow : IWindow
    {
        public PopToWindowRequest() { }
    }
}


