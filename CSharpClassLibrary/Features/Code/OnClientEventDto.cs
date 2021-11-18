namespace DM2BD.Europa.UIComponents.Code
{
    public interface IOnEventDto
    {

    }
    public class OnClientEventDto : IOnEventDto
    {
    }

    public class OnDivResizeDto : OnClientEventDto
    {
        public double ClientX { get; set; }
        public double ClientY { get; set; }
    }

    public class OnBrowserResizeDto : OnClientEventDto
    {
        public double ClientX { get; set; }
        public double ClientY { get; set; }
    }

    public class OnKeyDownDto : OnClientEventDto
    {
        public string Key { get; set; }
        public bool CtrlWasPressed { get; set; }
        public bool ShiftWasPressed { get; set; }
        public bool AltWasPressed { get; set; }
    }

    public class OnMouseMoveDto : OnClientEventDto
    {
        public double ClientY { get; set; }
        public double ClientX { get; set; }
    }
}
