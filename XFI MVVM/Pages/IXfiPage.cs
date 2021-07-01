namespace XFI_MVVM.Pages
{
    using XFI_MVVM.Models;

    internal interface IXfiPage
    {
        void SetBinding(XfiViewModel viewModel);

        void SetArgs(object[] args);
    }
}
