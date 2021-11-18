using System;

namespace DM2BD.Europa.UIComponents.Code
{
    public interface IClientDivEventListenerState
    {
        event EventHandler<OnDivResizeDto> OnDivResizeEventHandler;
        void OnDivResizeEventInvoke(object sender, OnDivResizeDto dimensions);
    }
    public interface IClientEventListenerState : IClientDivEventListenerState
    {
        event EventHandler<OnKeyDownDto> OnKeyDownEventHandler;
        event EventHandler<OnMouseMoveDto> OnMouseMoveEventHandler;
        event EventHandler<OnBrowserResizeDto> OnBrowserResizeEventHandler;
        event EventHandler OnMouseUpEventHandler;
        event EventHandler OnMouseDownEventHandler;

        void OnKeyDownEventInvoke(object sender, OnKeyDownDto key);
        void OnMouseMoveEventInvoke(object sender, OnMouseMoveDto key);
        void OnBrowserResizeEventInvoke(object sender, OnBrowserResizeDto dimensions);
        void OnMouseUpEventInvoke(object sender, EventArgs e);
        void OnMouseDownEventInvoke(object sender, EventArgs e);

    }
}
