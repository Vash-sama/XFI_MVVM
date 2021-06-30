using Xamarin.Forms;
using XFI_MVVM.Models;

namespace XFI_MVVM.Pages
{
    public abstract class XfiContentPage : ContentPage, IXfiPage
    {
        public object[] Args { get; private set; }

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