namespace XFI_MVVM.Pages
{
    using Xamarin.Forms;
    using XFI_MVVM.Models;

    /// <summary>
    /// Inherit to use as a Flyout Page view.
    /// </summary>
    public abstract class XfiFlyoutPage : FlyoutPage, IXfiPage
    {
        /// <summary>
        /// Gets the arguments passed through to this page.
        /// </summary>
        public object[] Args { get; private set; }

        /// <inheritdoc/>
        string IXfiPage.PageUrl { get; set; }

        /// <inheritdoc/>
        XfiViewModel IXfiPage.ViewModel { get; set; }

        /// <inheritdoc/>
        void IXfiPage.SetBinding(XfiViewModel viewModel)
        {
            this.BindingContext = viewModel;
        }

        /// <inheritdoc/>
        void IXfiPage.SetArgs(object[] args)
        {
            this.Args = args;
        }

        /// <summary>
        /// Override to remove handers, unsubscribe to listeners, etc.
        /// </summary>
        public virtual void Dispose()
        {
            // Remove link to ViewModel.
            this.BindingContext = null;
        }
    }
}
