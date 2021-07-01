namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using XFI_MVVM.Core;
    using XFI_MVVM.Exceptions;

    [TestClass]
    public class RegistrationTests
    {
        [TestMethod]
        public void RegisterPage()
        {
            Navigation.Register("Root", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidViewModelTypeException))]
        public void RegisterPage_InvalidViewModel()
        {
            Navigation.Register("Root", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page2));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPageTypeException))]
        public void RegisterPage_InvalidView()
        {
            Navigation.Register("Root", typeof(RequiredObjects.Views.Page2), typeof(RequiredObjects.ViewModels.Page1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterPage_NullUrl()
        {
            Navigation.Register(null, typeof(RequiredObjects.Views.Page2), typeof(RequiredObjects.ViewModels.Page1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterPage_EmptyUrl()
        {
            Navigation.Register(string.Empty, typeof(RequiredObjects.Views.Page2), typeof(RequiredObjects.ViewModels.Page1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPage_NullView()
        {
            Navigation.Register("Root", null, typeof(RequiredObjects.ViewModels.Page1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterPage_NullViewModel()
        {
            Navigation.Register("Root", typeof(RequiredObjects.Views.Page2), null);
        }
    }
}
