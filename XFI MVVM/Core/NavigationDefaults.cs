namespace XFI_MVVM.Core
{
    using XFI_MVVM.Enums;

    /// <summary>
    /// Partial of the navigation class to seperate the code for setting defaults, but keeping them in 'Navigation' for the package.
    /// </summary>
    public partial class Navigation
    {
        /// <summary>
        /// Set the default value for opening a page as modal.
        /// </summary>
        /// <param name="value">If pages should be opened as modal by default.</param>
        public static void SetDefaultIsModal(bool value)
        {
            Defaults.IsModal = value;
        }

        /// <summary>
        /// Set the default value for allowing multiple of the same page.
        /// </summary>
        /// <param name="value">If multiple instances of the same pages are allowd.</param>
        public static void SetDefaultAllowMultiple(bool value)
        {
            Defaults.AllowMultiple = value;
        }

        /// <summary>
        /// Set the default value for replacing an existing open page with a new instance.
        /// </summary>
        /// <param name="value">If exsting open pages of the same type get replaced with a new instance.</param>
        public static void SetDefaultReplaceInstance(bool value)
        {
            Defaults.ReplaceInstance = value;
        }

        /// <summary>
        /// Set the default value for prefered idiom.
        /// </summary>
        /// <param name="value">What the perfered idiom should be.</param>
        public static void SetDefaultIdiom(Idiom value)
        {
            Defaults.Idiom = value;
        }

        /// <summary>
        /// Set the default value for the prefered orientation.
        /// </summary>
        /// <param name="value">What the prefered orientation should be.</param>
        public static void SetDefaultOrientation(Orientation value)
        {
            Defaults.Orientation = value;
        }

        /// <summary>
        /// Set if the package should handle the orientation change using internal events to change to a different view automatically.
        /// </summary>
        /// <param name="value">Have the package handle orientation change or not.</param>
        public static void SetHandleOrientationChange(bool value)
        {
            Defaults.HandleOrientationChange = value;
        }

        /// <summary>
        /// Set if the package should try to keep the existing instance of viewModel when changing view to a new orientation.
        /// </summary>
        /// <param name="value">If the viewmodel should be reused when orientation changes or not.</param>
        public static void SetTryToKeepViewModelOnOrientationChange(bool value)
        {
            Defaults.TryToKeepViewModelOnOrientationChange = value;
        }

        /// <summary>
        /// Set an override idiom if you want your application to figure the idiom out itself, or to use custom ones.
        /// </summary>
        /// <param name="value">The idiom to use throughout regardless of what the package sees the device as.</param>
        public static void SetIdiomOverride(Idiom value)
        {
            Defaults.IdiomOverride = value;
        }
    }
}
