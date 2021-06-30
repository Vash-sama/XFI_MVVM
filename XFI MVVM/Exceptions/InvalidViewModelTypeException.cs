
namespace XFI_MVVM.Exceptions
{
    using System;

    public class InvalidViewModelTypeException : Exception
    {
        public InvalidViewModelTypeException(string message)
            : base(message)
        {
        }
    }
}
