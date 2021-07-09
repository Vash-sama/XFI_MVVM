namespace XFI_MVVM.Models
{
    using System;
    using XFI_MVVM.Enums;
    using XFI_MVVM.Exceptions;
    using XFI_MVVM.Core;
    using Xamarin.Forms;
    using XFI_MVVM.Pages;

    internal class XfiPageView
    {
        /// <summary>
        /// New XFI PageView
        /// </summary>
        /// <param name="pageUrl">The url and id for the page, used for navigation.</param>
        /// <param name="pageView">The type of view for this page (TypeOf XfiPage).</param>
        /// <param name="viewModel">The type viewmodel for this page (TypeOf XfiViewModel).</param>
        /// <param name="targetIdiom">The idiom for this specific view & view model combo.</param>
        /// <param name="targetOrientation">The orientation for this specific view & view model combo.</param>
        public XfiPageView(string pageUrl, Type pageView, Type viewModel, Idiom targetIdiom = null, Orientation targetOrientation = null, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException($"'{nameof(pageUrl)}' cannot be null or whitespace.", nameof(pageUrl));
            }

            this.PageURL = pageUrl;
            this.PageView = pageView ?? throw new ArgumentNullException(nameof(pageView));
            this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.TargetIdiom = targetIdiom ?? Defaults.Idiom;
            this.TargetOrientation = targetOrientation ?? Defaults.Orientation;
            this.DefaultArgs = args;

            if (!typeof(XfiViewModel).IsAssignableFrom(viewModel))
            {
                throw new InvalidViewModelTypeException($"ViewModel type {viewModel} is not a valid XFI ViewModel");
            }

            if (!typeof(IXfiPage).IsAssignableFrom(pageView))
            {
                throw new InvalidPageTypeException($"Page type {pageView} is not a valid XFI View");
            }

            this.eventArgs = new NavEventArgs()
            {
                EventType = nameof(CreateInstance),
                PageUrl = this.PageURL,
                PageView = this.PageView,
                ViewModel = this.ViewModel,
            };

            this.InitalizingViewModel += Navigation.FoundPage_InitalizingViewModel;
            this.InitalizedViewModel += Navigation.FoundPage_InitalizedViewModel;
            this.InitalizingView += Navigation.FoundPage_InitalizingView;
            this.InitalizedView += Navigation.FoundPage_InitalizedView;
        }

        /// <summary>
        /// The url / id of the page for navigation.
        /// </summary>
        public string PageURL { get; private set; }

        /// <summary>
        /// The view for this page.
        /// </summary>
        public Type PageView { get; private set; }

        /// <summary>
        /// The viewmodel for this page.
        /// </summary>
        public Type ViewModel { get; private set; }

        /// <summary>
        /// The idiom for this specific view & view model combo.
        /// </summary>
        public Idiom TargetIdiom { get; private set; }

        /// <summary>
        /// The orientation for this specific view & view model combo.
        /// </summary>
        public Orientation TargetOrientation { get; private set; }

        /// <summary>
        /// Default arguments to be be used none are set at navigation.
        /// </summary>
        public object[] DefaultArgs { get; private set; }

        /// <summary>
        /// Register this XFI PageView for navigation.
        /// </summary>
        public void Register()
        {
            ViewsStore.Instance.PageViews.Add(this);
        }

        /// <summary>
        /// De-Register this XFI PageView from navigation.
        /// </summary>
        public void DeRegister()
        {
            _ = ViewsStore.Instance.PageViews.Remove(this);
        }

        /// <summary>
        /// Triggered before viewmodel instance is created.
        /// </summary>
        public event Navigation.NavEvent InitalizingViewModel;

        /// <summary>
        /// Triggered after viewmodel is created.
        /// </summary>
        public event Navigation.NavEvent InitalizedViewModel;

        /// <summary>
        /// Triggered before view instance is created.
        /// </summary>
        public event Navigation.NavEvent InitalizingView;

        /// <summary>
        /// Triggered after view is created.
        /// </summary>
        public event Navigation.NavEvent InitalizedView;

        private readonly NavEventArgs eventArgs;

        internal Page CreateInstance(params object[] args)
        {
            args ??= this.DefaultArgs;

            InitalizingViewModel?.Invoke(this.eventArgs);

            var viewModelInstance = (XfiViewModel)Activator.CreateInstance(this.ViewModel);
            viewModelInstance.SetArgs(args);

            InitalizedViewModel?.Invoke(this.eventArgs);
            InitalizingView?.Invoke(this.eventArgs);

            var pageViewInstance = (Page)Activator.CreateInstance(this.PageView);
            var xfiPage = (IXfiPage)pageViewInstance;
            xfiPage.SetArgs(args);
            xfiPage.ViewModel = viewModelInstance;
            xfiPage.SetBinding(xfiPage.ViewModel);
            xfiPage.PageUrl = this.PageURL;

            InitalizedView?.Invoke(this.eventArgs);

            return pageViewInstance;
        }

        internal Page CreateInstance(XfiViewModel viewModelInstance)
        {
            var args = viewModelInstance.Args ?? this.DefaultArgs;

            InitalizingView?.Invoke(this.eventArgs);

            var pageViewInstance = (Page)Activator.CreateInstance(this.PageView);
            var xfiPage = (IXfiPage)pageViewInstance;
            xfiPage.SetArgs(args);
            xfiPage.ViewModel = viewModelInstance;
            xfiPage.SetBinding(xfiPage.ViewModel);
            xfiPage.PageUrl = this.PageURL;

            InitalizedView?.Invoke(this.eventArgs);

            return pageViewInstance;
        }

        /// <summary>
        /// Remove handlers on finalize.
        /// </summary>
        ~XfiPageView()
        {
            this.InitalizingViewModel -= Navigation.FoundPage_InitalizingViewModel;
            this.InitalizedViewModel -= Navigation.FoundPage_InitalizedViewModel;
            this.InitalizingView -= Navigation.FoundPage_InitalizingView;
            this.InitalizedView -= Navigation.FoundPage_InitalizedView;
        }
    }
}
