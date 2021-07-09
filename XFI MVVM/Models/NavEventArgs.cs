namespace XFI_MVVM.Models
{
    using System;

    public class NavEventArgs
    {
        public string EventType { get; set; }

        public string PageUrl { get; set; }

        public Type PageView { get; set; }

        public Type ViewModel { get; set; }
    }
}
