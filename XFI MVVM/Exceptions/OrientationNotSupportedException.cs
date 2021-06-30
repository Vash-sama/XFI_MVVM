

namespace XFI_MVVM.Exceptions
{
    using System;

    public class OrientationNotSupportedException : Exception
    {
        public OrientationNotSupportedException(string message) 
            : base(message)
        {
        }
    }
}
