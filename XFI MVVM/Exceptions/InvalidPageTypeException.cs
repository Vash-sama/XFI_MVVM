namespace XFI_MVVM.Exceptions
{
    /// <summary>
    /// Thrown when a view without a valid XFIPage type inherited is registered.
    /// </summary>
    public class InvalidPageTypeException : System.Exception
    {
        public InvalidPageTypeException(string message) 
            : base(message)
        {
        }
    }
}
