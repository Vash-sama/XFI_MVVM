namespace XFI_MVVM.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Essentials;
    using XFI_MVVM.Enums;
    using XFI_MVVM.Exceptions;
    using XFI_MVVM.Models;

    internal class ViewsStore
    {
        private static ViewsStore _instance;

        private ViewsStore()
        {
            DeviceDisplay.MainDisplayInfoChanged += Orientation.DeviceDisplay_MainDisplayInfoChanged;
        }

        /// <summary>
        /// Gets the persistent instance.
        /// </summary>
        public static ViewsStore Instance => _instance ??= new ViewsStore();

        /// <summary>
        /// Gets the list of pages registered.
        /// </summary>
        internal List<XfiPageView> PageViews { get; } = new List<XfiPageView>();

        /// <summary>
        /// Get the most relevant page for the provided URL.
        /// </summary>
        /// <param name="url">The url to get the page of.</param>
        /// <returns>The most relevant page.</returns>
        internal static XfiPageView GetPage(string url)
        {
            // Get current Orientiation and Idiom.
            var orientiation = Orientation.GetOrientation();
            var idiom = Idiom.GetIdiom();

            // Get all registered pages with the url provided.
            var pages = Instance.PageViews.Where(b => b.PageURL.ToLower() == url.ToLower());

            // Filter pages with the current Idiom.
            var foundIdiom = pages.Where(b => b.TargetIdiom == idiom);

            // If none found revert to previous list.
            if (!foundIdiom.Any())
                foundIdiom = pages;

            // Filter pages with current Orientation.
            var foundOrientation = foundIdiom.Where(b => b.TargetOrientation == orientiation);

            // If none found revert to previous list.
            if (!foundOrientation.Any())
                foundOrientation = foundIdiom;

            // first from remaining list.
            var foundPage = foundOrientation.FirstOrDefault();

            return foundPage ?? throw new PageNotFoundException($"Could not find suitable page for url:'{url}', idiom:'{idiom}', and orientation:'{orientiation}'");
        }

        /// <summary>
        /// Find if an open instance of the page and provided idiom and orientation.
        /// </summary>
        /// <param name="url">The url of the page being searched for.</param>
        /// <param name="targetIdiom">The idiom of the page being searched for.</param>
        /// <param name="targetOrientation">The orientation of the page being searched for.</param>
        /// <returns>Any found page matching provided details.</returns>
        internal static XfiPageView FindPage(string url, Idiom targetIdiom = null, Orientation targetOrientation = null)
        {
            var idiom = targetIdiom ?? Defaults.Idiom;
            var orientation = targetOrientation ?? Defaults.Orientation;

            // Get all registered pages with the url provided.
            return Instance.PageViews.Where(b => b.PageURL.ToLower() == url.ToLower() && b.TargetIdiom == idiom && b.TargetOrientation == orientation).FirstOrDefault();
        }
    }
}
