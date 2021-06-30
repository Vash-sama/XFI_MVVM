
namespace XFI_MVVM.Exceptions
{
    using System;

    public class InvalidPageTypeException : Exception
    {
        public InvalidPageTypeException(string message) 
            : base(message)
        {
        }
    }
}
