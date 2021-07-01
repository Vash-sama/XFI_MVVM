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

        internal void SetArgs(object[] args)
        {
            this.Args = args;
        }
    }
}
