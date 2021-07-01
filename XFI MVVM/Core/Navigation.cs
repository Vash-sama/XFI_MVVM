namespace XFI_MVVM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using XFI_MVVM.Enums;
    using XFI_MVVM.Models;
    using XFI_MVVM.Pages;

    public class Navigation : NavigationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyNav"/> class.
        /// </summary>
        /// <param name="root">Page to use as the root of the navigation.</param>
        private Navigation(Page root)
            : base(root)
        {
        }

        /// <summary>
        /// Gets the persistent user details.
        /// </summary>
        public static Navigation Instance { get; private set; }

        /// <summary>
        /// Initalise the Navigation page.
        /// </summary>
        /// <param name="root">Starting page of the navigation.</param>
        public static void Init(string pageUrl)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException($"'{nameof(pageUrl)}' cannot be null or whitespace.", nameof(pageUrl));
            }

            var foundPage = ViewsStore.GetPage(pageUrl);

            var newPage = foundPage.CreateInstance();

            Instance = new Navigation(newPage);
        }

        /// <summary>
        /// Push a new page into the navigation stack.
        /// </summary>
        /// <param name="pageUrl">Page to push.</param>
        /// <param name="isModal">If the page to display should be a modal.</param>
        /// <param name="allowMultiple">Allow multiple instances of this page type.</param>
        /// <param name="replace">Whether or not to replace an existing page in the stack with the new page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Push(string pageUrl, bool isModal = false, bool allowMultiple = false, bool replace = false)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException($"'{nameof(pageUrl)}' cannot be null or whitespace.", nameof(pageUrl));
            }

            var foundPage = ViewsStore.GetPage(pageUrl);

            var openPages = IsPageOpen(pageUrl, isModal);

            Page newPage;

            if (!allowMultiple && replace)
            {
                newPage = foundPage.CreateInstance();
                RemovePages(openPages);
            }
            else if (!allowMultiple && !replace && openPages.Count > 0)
            {
                newPage = openPages.FirstOrDefault();
                RemovePages(openPages);
            }
            else
            {
                newPage = foundPage.CreateInstance();
            }

            if (isModal)
                await Instance.Navigation.PushModalAsync(newPage, true);
            else
                await Instance.Navigation.PushAsync(newPage, true);
        }

        /// <summary>
        /// Push a new page into the navigation stack.
        /// </summary>
        /// <param name="pageUrl">Page to push.</param>
        /// <param name="isModal">If the page to display should be a modal.</param>
        /// <param name="allowMultiple">Allow multiple instances of this page.</param>
        /// <param name="replace">Whether or not to replace an existing page in the stack with the new page.</param>
        public static void PushSync(string pageUrl, bool isModal = false, bool allowMultiple = false, bool replace = false)
        {
            try
            {
                var task = Push(pageUrl, isModal, allowMultiple, replace);
                task.Wait();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Pop the current page from the navigation stack.
        /// </summary>
        /// <param name="isModal">If the screen to display should be a modal.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Pop(bool isModal = false)
        {
            if (isModal)
                await Instance.Navigation.PopModalAsync(true);
            else
                await Instance.Navigation.PopAsync(true);
        }

        /// <summary>
        /// Pop the current page from the navigation stack.
        /// </summary>
        /// <param name="isModal">If the screen to display should be a modal.</param>
        public static void PopSync(bool isModal = false)
        {
            try
            {
                var task = Pop(isModal);
                task.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Register a new page for navigation.
        /// </summary>
        /// <param name="url">The url / key for navigation</param>
        /// <param name="view">The type of view to use for this combination.</param>
        /// <param name="viewModel">The type of viewmodel to use for this combination.</param>
        /// <param name="targetIdiom">The perfered idiom for this page.</param>
        /// <param name="targetOrientation">The prefered orientation for this page.</param>
        public static void Register(string url, Type view, Type viewModel, Idiom targetIdiom = null, Orientation targetOrientation = null)
        {
            new XfiPageView(url, view, viewModel, targetIdiom, targetOrientation).Register();
        }

        private static List<Page> IsPageOpen(string url, bool isModal)
        {
            var stack = Instance.Navigation.NavigationStack;
            if (isModal)
                stack = Instance.Navigation.ModalStack;

            // Find if the page already exists in the stack
            return (from b in stack where ((IXfiPage)b).PageUrl == url select b).ToList();
        }

        private static void RemovePages(List<Page> openPages)
        {
            foreach (var thisPage in openPages)
                Instance.Navigation.RemovePage(thisPage);
        }
    }
}
