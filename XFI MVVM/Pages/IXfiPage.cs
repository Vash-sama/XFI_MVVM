using XFI_MVVM.Models;

namespace XFI_MVVM.Pages
{
    internal interface IXfiPage
    {
        void SetBinding(XfiViewModel viewModel);

        void SetArgs(object[] args);
    }
}
