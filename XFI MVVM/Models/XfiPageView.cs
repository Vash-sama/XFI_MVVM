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
        public XfiPageView(string pageUrl, Type pageView, Type viewModel, Idiom targetIdiom = null, Orientation targetOrientation = null)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException($"'{nameof(pageUrl)}' cannot be null or whitespace.", nameof(pageUrl));
            }

            this.PageURL = pageUrl;
            this.PageView = pageView ?? throw new ArgumentNullException(nameof(pageView));
            this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.TargetIdiom = targetIdiom ?? Idiom.Phone;
            this.TargetOrientation = targetOrientation ?? Orientation.Portrait;

            if (!typeof(XfiViewModel).IsAssignableFrom(viewModel))
            {
                throw new InvalidViewModelTypeException($"ViewModel type {viewModel} is not a valid XFI ViewModel");
            }

            if (!typeof(IXfiPage).IsAssignableFrom(pageView))
            {
                throw new InvalidPageTypeException($"Page type {pageView} is not a valid XFI View");
            }
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
            ViewsStore.Instance.PageViews.Remove(this);
        }

        internal Page CreateInstance(params object[] args)
        {
            var viewModelInstance = (XfiViewModel)Activator.CreateInstance(this.ViewModel);
            viewModelInstance.SetArgs(args);

            var pageViewInstance = (Page)Activator.CreateInstance(this.PageView);
            var xfiPage = (IXfiPage)pageViewInstance;
            xfiPage.SetArgs(args);
            xfiPage.SetBinding(viewModelInstance);
            xfiPage.PageUrl = this.PageURL;

            return pageViewInstance;
        }
    }
}
