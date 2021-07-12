using System;
using XFI_MVVM.Enums;

namespace XFI_MVVM.Models
{
    /// <summary>
    /// Inherit to identify XFI ViewModels.
    /// </summary>
    public abstract class XfiViewModel : IDisposable
    {
        /// <summary>
        /// Gets the arguments passed through to this page.
        /// </summary>
        public object[] Args { get; private set; }

        /// <summary>
        /// Gets or sets the current oriention of this viewmodel.
        /// </summary>
        internal Orientation CurrentOrientation { get; set; } = Orientation.GetOrientation();

        /// <summary>
        /// Set the args of this viewModel to the provided parameters.
        /// </summary>
        /// <param name="args">The args to set.</param>
        internal void SetArgs(object[] args)
        {
            this.Args = args;
        }

        /// <summary>
        /// Override to remove handers, unsubscribe to listeners, etc.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
