using XFI_MVVM.Enums;

namespace XFI_MVVM.Models
{
    /// <summary>
    /// Inherit to identify XFI ViewModels.
    /// </summary>
    public abstract class XfiViewModel
    {
        /// <summary>
        /// Gets the arguments passed through to this page.
        /// </summary>
        public object[] Args { get; private set; }

        /// <summary>
        /// Gets or sets the current oriention of this viewmodel.
        /// </summary>
        internal Orientation CurrentOrientation { get; set; } = Orientation.GetOrientation();

        internal void SetArgs(object[] args)
        {
            this.Args = args;
        }
    }
}
