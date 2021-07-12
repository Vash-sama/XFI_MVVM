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

    public partial class Navigation : NavigationPage, IDisposable
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
            this.PopRequested += this.Navigation_PopRequested;
        }

        /// <summary>
        /// Gets the persistent user details.
        /// </summary>
        public static Navigation Instance { get; private set; }

        /// <summary>
        /// Initalize the Navigation page.
        /// </summary>
        /// <param name="root">Starting page of the navigation.</param>
        public static void Init(string pageUrl)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException($"'{nameof(pageUrl)}' cannot be null or whitespace.", nameof(pageUrl));
            }

            if (Instance != null)
            {
                Instance.Dispose();
            }

            var foundPage = ViewsStore.GetPage(pageUrl);

            var newPage = foundPage.CreateInstance();

            Instance = new(newPage);
            Instance.rootUrl = pageUrl;
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
        /// DeRegister a page from navigation, disposing of any open instances.
        /// </summary>
        /// <param name="url">The url / key for navigation</param>
        /// <param name="targetIdiom">The perfered idiom for this page.</param>
        /// <param name="targetOrientation">The prefered orientation for this page.</param>
        public static void DeRegister(string url, Idiom targetIdiom = null, Orientation targetOrientation = null)
        {
            var page = ViewsStore.FindPage(url, targetIdiom, targetOrientation);

            if (page == null)
                return;

            var openPages = IsPageOpen(url, false);

            var removePages = new List<Page>();
            foreach (IXfiPage openPage in openPages)
            {
                if (openPage.GetType() == page.PageView && openPage.ViewModel.GetType() == page.ViewModel)
                    removePages.Add((Page)openPage);
            }

            RemovePages(removePages);
            page.DeRegister();
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

            var eventArgs = new NavEventArgs()
            {
                EventType = nameof(Push),
                PageUrl = pageUrl,
            };
            Instance.StartedNavigation?.Invoke(eventArgs);

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
            eventArgs.PageView = foundPage.PageView;
            eventArgs.ViewModel = foundPage.ViewModel;

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

                // Inside InvoiceOnMain to force triggering only once the push has taken effect.
                Instance.FinishedNavigation?.Invoke(eventArgs);
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
            Instance.Navigation_PopToRootRequested();

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
        /// Dispose of all pages in the stacks and remove all existing handlers.
        /// </summary>
        public void Dispose()
        {
            this.Navigation_PopToRootRequested();

            var rootPage = (IXfiPage)this.RootPage;
            var rootViewModel = rootPage.ViewModel;

            rootViewModel.Dispose();
            rootPage.Dispose();

            this.Popped -= this.Navigation_Popped;
            this.PopRequested -= this.Navigation_PopRequested;
        }

        /// <summary>
        /// Change the view to a more relevent orientation version if available.
        /// </summary>
        internal static void OrientationChange()
        {
            Page newPage = null;

            // Find if existing page is modal or not.
            var isModal = IsModal();

            // Get current page open.
            var currentPage = GetCurrentPage(isModal);
            var pageUrl = ((IXfiPage)currentPage).PageUrl;

            var eventArgs = new NavEventArgs()
            {
                EventType = nameof(OrientationChange),
                PageUrl = pageUrl,
            };
            Instance.StartedNavigation?.Invoke(eventArgs);

            // Get new page.
            var foundPage = ViewsStore.GetPage(pageUrl);
            eventArgs.PageView = foundPage.PageView;
            eventArgs.ViewModel = foundPage.ViewModel;

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

                // Inside InvoiceOnMain to force triggering only once the push has taken effect.
                Instance.FinishedNavigation?.Invoke(eventArgs);
            });
        }

        /// <summary>
        /// Is the current page open a modal page.
        /// </summary>
        /// <returns>Is open view a modal.</returns>
        private static bool IsModal()
        {
            return Instance.Navigation.ModalStack.Any();
        }

        /// <summary>
        /// Get the current open page.
        /// </summary>
        /// <param name="isModal">Is the current page modal or not.</param>
        /// <returns>The found open page.</returns>
        private static Page GetCurrentPage(bool isModal)
        {
            var stack = Instance.Navigation.NavigationStack;
            if (isModal)
                stack = Instance.Navigation.ModalStack;

            return stack.LastOrDefault();
        }

        /// <summary>
        /// Get open instances of a page by its URL.
        /// </summary>
        /// <param name="url">The url being investigated.</param>
        /// <param name="isModal">If its expected to be modal or not.</param>
        /// <returns>A list of all instances of the open page.</returns>
        private static List<Page> IsPageOpen(string url, bool isModal)
        {
            var stack = Instance.Navigation.NavigationStack;
            if (isModal)
                stack = Instance.Navigation.ModalStack;

            // Find if the page already exists in the stack
            return (from b in stack where ((IXfiPage)b).PageUrl == url select b).ToList();
        }

        /// <summary>
        /// Remove all pages from the navigation stack provided.
        /// </summary>
        /// <param name="openPages">Open pages to remove from navigation stack.</param>
        private static void RemovePages(List<Page> openPages)
        {
            foreach (var thisPage in openPages)
            {
                if (Instance.Navigation.NavigationStack.Contains(thisPage))
                {
                    // Remove the page from the stack.
                    Instance.Navigation.RemovePage(thisPage);

                    // Dispose View and ViewModel of the page too.
                    var currentPage = (IXfiPage)thisPage;
                    var viewModel = currentPage.ViewModel;
                    viewModel.Dispose();
                    currentPage.Dispose();
                }
            }
        }

        /// <summary>
        /// A navigation pop was requested, trigger dispose of the View and ViewModel.
        /// </summary>
        /// <param name="sender">The pop sender.</param>
        /// <param name="e">The event args of the request.</param>
        private void Navigation_PopRequested(object sender, Xamarin.Forms.Internals.NavigationRequestedEventArgs e)
        {
            // Get current page open.
            var currentPage = (IXfiPage)e.Page;

            // Get current viewmodel.
            var viewModel = currentPage.ViewModel;

            viewModel.Dispose();
            currentPage.Dispose();
        }

        /// <summary>
        /// A navigation pop-to-root was requested, trigger dispose of all Views and ViewModels of all pages in the stack.
        /// </summary>
        private void Navigation_PopToRootRequested()
        {
            // If any open pages are modal.
            var currentPageModal = IsModal();
            if (currentPageModal)
            {
                foreach (IXfiPage page in Instance.Navigation.ModalStack)
                {
                    // Get current viewmodel.
                    var viewModel = page.ViewModel;

                    viewModel.Dispose();
                    page.Dispose();
                }
            }

            // For the rest of the stack excluding the root page.
            foreach (IXfiPage page in Instance.Navigation.NavigationStack)
            {
                if (page == RootPage)
                    continue;

                // Get current viewmodel.
                var viewModel = page.ViewModel;

                viewModel.Dispose();
                page.Dispose();
            }
        }

        /// <summary>
        /// A navigation page was popped, check if the next page has a better orientation registered.
        /// </summary>
        /// <param name="sender">The pop sender.</param>
        /// <param name="e">The event args of the request.</param>
        private void Navigation_Popped(object sender, NavigationEventArgs e)
        {
            if (!Defaults.HandleOrientationChange)
            {
                return;
            }

            // Get current page open.
            var currentPage = (IXfiPage)e.Page;

            // Get current viewmodel.
            var viewModel = currentPage.ViewModel;

            // If current viewmodel remembered orientation is different from current orientation - reload.
            if (viewModel.CurrentOrientation != Orientation.GetOrientation())
            {
                OrientationChange();
            }
        }
    }
}
