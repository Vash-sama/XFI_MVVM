
namespace XFI_MVVM.Exceptions
{
    using System;

    public class IdiomNotSupportedException : Exception
    {
        public IdiomNotSupportedException(string message) 
            : base(message)
        {
        }
    }
}
