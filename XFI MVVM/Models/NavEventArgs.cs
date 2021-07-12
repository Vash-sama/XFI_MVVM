namespace XFI_MVVM.Models
{
    using System;

    /// <summary>
    /// The event arguments that are provided with all navigation events.
    /// </summary>
    public class NavEventArgs
    {
        /// <summary>
        /// The event type e.g. Push, OrientationChange.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// The page url the event relates to.
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// The view type the event relates to.
        /// </summary>
        public Type PageView { get; set; }

        /// <summary>
        /// The viewmodel type the event relates to.
        /// </summary>
        public Type ViewModel { get; set; }
    }
}
