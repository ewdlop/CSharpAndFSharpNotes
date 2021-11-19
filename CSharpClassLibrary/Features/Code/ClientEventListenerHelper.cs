//using Microsoft.JSInterop;

//namespace DM2BD.Europa.UIComponents.Code
//{
//    public interface IClientDivEventListenrHelper
//    {
//        void OnDivResize(OnDivResizeDto dto);
//    }
//    public partial class ClientEventListenerHelper : IClientDivEventListenrHelper
//    {
//        private readonly IClientEventListenerState _eventListenerState;

//        public ClientEventListenerHelper(IClientEventListenerState eventListenerState)
//        {
//            _eventListenerState = eventListenerState;
//        }

//        [JSInvokable("OnBrowserResize")]
//        public void OnBrowserResize(OnBrowserResizeDto dto)
//        {
//            _eventListenerState.OnBrowserResizeEventInvoke(this, dto);
//        }

//        [JSInvokable("OnDivResizeDto")]
//        public void OnDivResize(OnDivResizeDto dto)
//        {
//            _eventListenerState.OnDivResizeEventInvoke(this, dto);
//        }
//    }
//}
