using System;

namespace DM2BD.Europa.UIComponents.Code
{
    public class ClientEventListenerState : IClientEventListenerState
    {
        public event EventHandler<OnKeyDownDto> OnKeyDownEventHandler;
        public event EventHandler<OnMouseMoveDto> OnMouseMoveEventHandler;
        public event EventHandler<OnBrowserResizeDto> OnBrowserResizeEventHandler;
        public event EventHandler<OnDivResizeDto> OnDivResizeEventHandler;
        public event EventHandler OnMouseUpEventHandler;
        public event EventHandler OnMouseDownEventHandler;

        public void OnBrowserResizeEventInvoke(object sender, OnBrowserResizeDto dimensions)
        {
            EventHandler<OnBrowserResizeDto> handler = OnBrowserResizeEventHandler;

            handler?.Invoke(sender, dimensions);
        }
        public void OnMouseMoveEventInvoke(object sender, OnMouseMoveDto key)
        {
            EventHandler<OnMouseMoveDto> handler = OnMouseMoveEventHandler;

            handler?.Invoke(sender, key);
        }

        public void OnKeyDownEventInvoke(object sender, OnKeyDownDto key)
        {
            EventHandler<OnKeyDownDto> handler = OnKeyDownEventHandler;

            handler?.Invoke(sender, key);
        }
        public void OnDivResizeEventInvoke(object sender, OnDivResizeDto dimensions)
        {
            EventHandler<OnDivResizeDto> handler = OnDivResizeEventHandler;

            handler?.Invoke(sender, dimensions);
        }

        public void OnMouseUpEventInvoke(object sender, EventArgs e)
        {
            EventHandler handler = OnMouseUpEventHandler;

            handler?.Invoke(sender, e);
        }

        public void OnMouseDownEventInvoke(object sender, EventArgs e)
        {
            EventHandler handler = OnMouseDownEventHandler;

            handler?.Invoke(sender, e);
        }
    }
}
