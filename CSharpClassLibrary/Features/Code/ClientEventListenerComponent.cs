//using Microsoft.AspNetCore.Components;
//using Microsoft.JSInterop;

//namespace DM2BD.Europa.UIComponents.Code
//{
//    public class ClientEventListenerComponent : ComponentBase
//    {
//        [Inject]
//        private IJSRuntime JsRuntime { get; set; }
//        [Inject]
//        private IClientEventListenerState EventListenerState { get; set; }

//        private ClientEventListenerHelper _eventListenerHelper;

//        protected override void OnAfterRender(bool firstRender)
//        {
//            if (firstRender)
//            {
//                _eventListenerHelper = new ClientEventListenerHelper(EventListenerState);

//                _ = JsRuntime.InvokeVoidAsync("clientEventListener.browserResize",
//                    DotNetObjectReference.Create(_eventListenerHelper));
//            }

//            base.OnAfterRender(firstRender);
//        }
//    }
//}
