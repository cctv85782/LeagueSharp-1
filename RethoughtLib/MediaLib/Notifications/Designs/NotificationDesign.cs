namespace RethoughtLib.Notifications.Designs
{
    using RethoughtLib.Transitions;

    public abstract class NotificationDesign
    {
        public abstract int Height { get; set; }

        public abstract int Width { get; set; }
        
        public abstract Transition Transition { get; set; }
    }
}
