namespace XFI_MVVM.Exceptions
{
    /// <summary>
    /// Thrown when a view without the <see cref="Models.XfiViewModel"/> type inherited is registered.
    /// </summary>
    public class InvalidViewModelTypeException : System.Exception
    {
        public InvalidViewModelTypeException(string message)
            : base(message)
        {
        }
    }
}
