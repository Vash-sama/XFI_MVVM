namespace XFI_MVVM.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using XFI_MVVM.Exceptions;
    using XFI_MVVM.Models;

    internal class ViewsStore
    {
        private static ViewsStore _instance;

        private ViewsStore()
        { }

        /// <summary>
        /// Gets the persistent instance.
        /// </summary>
        public static ViewsStore Instance => _instance ??= new ViewsStore();

        /// <summary>
        /// Gets the list of pages registered.
        /// </summary>
        internal List<XfiPageView> PageViews { get; } = new List<XfiPageView>();

        internal static XfiPageView GetPage(string url)
        {
            // Get current Orientiation and Idiom.
            var orientiation = Enums.Orientation.GetOrientation();
            var idiom = Enums.Idiom.GetIdiom();

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
    }
}
