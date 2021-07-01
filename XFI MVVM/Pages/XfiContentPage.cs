namespace XFI_MVVM.Pages
{
    using Xamarin.Forms;
    using XFI_MVVM.Models;

    /// <summary>
    /// Inherit to use as a content page view.
    /// </summary>
    public abstract class XfiContentPage : ContentPage, IXfiPage
    {
        /// <summary>
        /// Gets the arguments passed through to this page.
        /// </summary>
        public object[] Args { get; private set; }

        string IXfiPage.PageUrl { get ; set; }

        void IXfiPage.SetBinding(XfiViewModel viewModel)
        {
            this.BindingContext = viewModel;
        }

        void IXfiPage.SetArgs(object[] args)
        {
            this.Args = args;
        }
    }
}