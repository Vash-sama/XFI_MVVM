namespace XFI_MVVM.Pages
{
    using XFI_MVVM.Models;

    internal interface IXfiPage
    {
        internal void SetBinding(XfiViewModel viewModel);

        internal void SetArgs(object[] args);

        internal string PageUrl { get; set; }
    }
}
