namespace XFI_MVVM.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using XFI_MVVM.Enums;
    using XFI_MVVM.Models;
    using XFI_MVVM.Pages;

    public class Navigation : NavigationPage
    {
        private string rootUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyNav"/> class.
        /// </summary>
        /// <param name="root">Page to use as the root of the navigation.</param>
        private Navigation(Page root)
            : base(root)
        {
            this.Popped += this.Navigation_Popped;
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
            Instance.rootUrl = pageUrl;
        }

        /// <summary>
        /// Push a new page into the navigation stack.
        /// </summary>
        /// <param name="pageUrl">Page to push.</param>
        /// <param name="isModal">If the page to display should be a modal.</param>
        /// <param name="allowMultiple">Allow multiple instances of this page type.</param>
        /// <param name="replace">Whether or not to replace an existing page in the stack with the new page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Push(string pageUrl, bool? isModal = null, bool? allowMultiple = null, bool? replace = null, params object[] args)
        {
            // If no url is provided exit.
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException($"'{nameof(pageUrl)}' cannot be null or whitespace.", nameof(pageUrl));
            }

            Page newPage;

            // Setup the values to use from either defaults or passed value.
            var isModalValue = isModal ?? Defaults.IsModal;
            var allowMultipleValue = allowMultiple ?? Defaults.AllowMultiple;
            var replaceValue = replace ?? Defaults.ReplaceInstance;

            // If trying to push a new instance of root but dont have allow multiple, pop back to root.
            if (pageUrl == Instance.rootUrl && !allowMultipleValue)
            {
                await PopToRoot();
                return;
            }
                
            // Get the page to nav to.
            var foundPage = ViewsStore.GetPage(pageUrl);

            // If the page is already open as an instance.
            var openPages = IsPageOpen(pageUrl, isModalValue);

            // Based of values, create new instances and / remove pages or move existing instance.
            if (!allowMultipleValue && replaceValue)
            {
                newPage = foundPage.CreateInstance(args);
                RemovePages(openPages);
            }
            else if (!allowMultipleValue && !replaceValue && openPages.Count > 0)
            {
                newPage = openPages.FirstOrDefault();
                RemovePages(openPages);
            }
            else
            {
                newPage = foundPage.CreateInstance(args);
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (isModalValue)
                    await Instance.Navigation.PushModalAsync(newPage, true);
                else
                    await Instance.Navigation.PushAsync(newPage, true);
            });

        }

        /// <summary>
        /// Push a new page into the navigation stack.
        /// </summary>
        /// <param name="pageUrl">Page to push.</param>
        /// <param name="isModal">If the page to display should be a modal.</param>
        /// <param name="allowMultiple">Allow multiple instances of this page.</param>
        /// <param name="replace">Whether or not to replace an existing page in the stack with the new page.</param>
        public static void PushSync(string pageUrl, bool? isModal = null, bool? allowMultiple = null, bool? replace = null, params object[] args)
        {
            try
            {
                var task = Push(pageUrl, isModal, allowMultiple, replace, args);
                task.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Pop the current page from the navigation stack.
        /// </summary>
        /// <param name="isModal">If the screen to display should be a modal.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Pop(bool? isModal = null)
        {
            var isModalValue = isModal ?? Defaults.IsModal;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (isModalValue)
                    await Instance.Navigation.PopModalAsync(true);
                else
                    await Instance.Navigation.PopAsync(true);
            });
        }

        /// <summary>
        /// Pop the current page from the navigation stack.
        /// </summary>
        /// <param name="isModal">If the screen to display should be a modal.</param>
        public static void PopSync(bool? isModal = null)
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
        /// Pop all the way back to root.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task PopToRoot()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Instance.Navigation.PopToRootAsync(true);
            });
        }

        /// <summary>
        /// Pop all the way back to root.
        /// </summary>
        public static void PopToRootSync()
        {
            try
            {
                var task = PopToRoot();
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
        public static void Register(string url, Type view, Type viewModel, Idiom targetIdiom = null, Orientation targetOrientation = null, params object[] args)
        {
            new XfiPageView(url, view, viewModel, targetIdiom, targetOrientation, args).Register();
        }

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

        internal static void OrientationChange()
        {
            Page newPage = null;

            // Find if existing page is modal or not.
            var isModal = IsModal();

            // Get current page open.
            var currentPage = GetCurrentPage(isModal);
            var pageUrl = ((IXfiPage)currentPage).PageUrl;

            // Get new page.
            var foundPage = ViewsStore.GetPage(pageUrl);

            // Dont re-load the same page.
            if (foundPage.PageView == currentPage.GetType())
            {
                return;
            }

            // If the page found isn't specifically targeting the new orientation return.
            if (foundPage.TargetOrientation != Orientation.GetOrientation())
            {
                return;
            }

            // If the system is setup to keep view model.
            if (Defaults.TryToKeepViewModelOnOrientationChange)
            {
                // Get current pages viewmodel.
                var viewModel = ((IXfiPage)currentPage).ViewModel;

                // Only if the viewmodel type matches original page, create a new view with existing viewmodel.
                if (foundPage.ViewModel == viewModel.GetType())
                {
                    newPage = foundPage.CreateInstance(viewModel);

                    // Reset viewmodels known orientation.
                    viewModel.CurrentOrientation = Orientation.GetOrientation();
                }
            }

            // If the above hasnt setup the page, just create it as normally.
            if (newPage == null)
                newPage = foundPage.CreateInstance();

            // Navigation to the new page before removing to prevent flashing.
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (isModal)
                    await Instance.Navigation.PushModalAsync(newPage, true);
                else
                    await Instance.Navigation.PushAsync(newPage, true);

                // Remove the original apge before re-loading new one.
                RemovePages(new List<Page>() { currentPage });
            });
        }

        private static bool IsModal()
        {
            return Instance.Navigation.ModalStack.Any();
        }

        private static Page GetCurrentPage(bool isModal)
        {
            var stack = Instance.Navigation.NavigationStack;
            if (isModal)
                stack = Instance.Navigation.ModalStack;

            return stack.LastOrDefault();
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
            {
                if (Instance.Navigation.NavigationStack.Contains(thisPage))
                    Instance.Navigation.RemovePage(thisPage);
            }
        }

        private void Navigation_Popped(object sender, NavigationEventArgs e)
        {
            if (!Defaults.HandleOrientationChange)
            {
                return;
            }

            // Find if existing page is modal or not.
            var isModal = IsModal();

            // Get current page open.
            var currentPage = GetCurrentPage(isModal);

            // Get current viewmodel.
            var viewModel = ((IXfiPage)currentPage).ViewModel;

            // If current viewmodel remembered orientation is different from current orientation - reload.
            if (viewModel.CurrentOrientation != Orientation.GetOrientation())
            {
                OrientationChange();
            }
        }
    }
}
