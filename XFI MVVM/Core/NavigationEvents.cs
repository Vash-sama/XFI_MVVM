using XFI_MVVM.Models;

namespace XFI_MVVM.Core
{
    public partial class Navigation
    {
        public delegate void NavEvent(NavEventArgs e);

        /// <summary>
        /// Triggered at the inital point of navigating.
        /// Called by : Push, PushSync, OrientationChange.
        /// </summary>
        public event NavEvent StartedNavigation;

        /// <summary>
        /// Triggered prior to Initalizing the ViewModel.
        /// </summary>
        public event NavEvent InitalizingViewModel;

        /// <summary>
        /// Triggered after the ViewModel has finished Initalizing.
        /// </summary>
        public event NavEvent InitalizedViewModel;

        /// <summary>
        /// Triggered prior to Initalizing the View.
        /// </summary>
        public event NavEvent InitalizingView;

        /// <summary>
        /// Triggered after the View has finished Initalized.
        /// </summary>
        public event NavEvent InitalizedView;

        /// <summary>
        /// Triggered at the very end of navigating when all processing is finished.
        /// </summary>
        public event NavEvent FinishedNavigation;

        /// <summary>
        /// Invoke the navigation event for InitalizedView.
        /// </summary>
        /// <param name="e">The nav event args.</param>
        internal static void FoundPage_InitalizedView(NavEventArgs e)
        {
            Instance?.InitalizedView?.Invoke(e);
        }

        /// <summary>
        /// Invoke the navigation event for InitalizingViewModel.
        /// </summary>
        /// <param name="e">The nav event args.</param>
        internal static void FoundPage_InitalizingViewModel(NavEventArgs e)
        {
            Instance?.InitalizingViewModel?.Invoke(e);
        }

        /// <summary>
        /// Invoke the navigation event for InitalizedViewModel.
        /// </summary>
        /// <param name="e">The nav event args.</param>
        internal static void FoundPage_InitalizedViewModel(NavEventArgs e)
        {
            Instance?.InitalizedViewModel?.Invoke(e);
        }

        /// <summary>
        /// Invoke the navigation event for InitalizingView.
        /// </summary>
        /// <param name="e">The nav event args.</param>
        internal static void FoundPage_InitalizingView(NavEventArgs e)
        {
            Instance?.InitalizingView?.Invoke(e);
        }
    }
}
