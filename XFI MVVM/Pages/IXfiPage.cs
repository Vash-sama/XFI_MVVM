namespace XFI_MVVM.Pages
{
    using System;
    using XFI_MVVM.Models;

    internal interface IXfiPage : IDisposable
    {
        /// <summary>
        /// Bind the ViewModel to the views Context.
        /// </summary>
        /// <param name="viewModel"></param>
        internal void SetBinding(XfiViewModel viewModel);

        /// <summary>
        /// Set the args of this viewModel to the provided parameters.
        /// </summary>
        /// <param name="args">The args to set.</param>
        internal void SetArgs(object[] args);

        /// <summary>
        /// Gets or sets the PageURL of this page.
        /// </summary>
        internal string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the ViewModel instance.
        /// </summary>
        internal XfiViewModel ViewModel { get; set; }
    }
}
