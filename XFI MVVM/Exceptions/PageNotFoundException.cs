namespace XFI_MVVM.Exceptions
{
    // Thown when attempting to navigate to a page that isnt registered.
    public class PageNotFoundException : System.Exception
    {
        public PageNotFoundException(string message)
            : base(message)
        {
        }
    }
}
