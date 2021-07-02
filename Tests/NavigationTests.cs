namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading.Tasks;
    using XFI_MVVM.Core;
    using XFI_MVVM.Exceptions;

    [TestClass]
    public class NavigationTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Navigation.Register("Root", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page1));
            Navigation.Register("Page2", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page1));
        }

        [TestMethod]
        public void NavigateInit()
        {
            Navigation.Init("Root");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NavigateInit_NullUrl()
        {
            Navigation.Init(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public void NavigateInit_PageNotFound()
        {
            Navigation.Init("Page1");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void NavigateInit_CreateInstanceFail()
        {
            Navigation.Register("Root2", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page3));
            Navigation.Init("Root2");
        }

        [TestMethod]
        public void Navigate()
        {
            Navigation.Init("Root");
            Navigation.PushSync("Page2");
        }

        [TestMethod]
        public void Navigate_WithArgs()
        {
            Navigation.Init("Root");
            Navigation.PushSync("Page2", null, null, null, "Property1", "Property2");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Navigate_NullUrl()
        {
            Navigation.Init("Root");
            Navigation.PushSync(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public void Navigate_PageNotFound()
        {
            Navigation.Init("Root");
            Navigation.PushSync("Page1");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void Navigate_CreateInstanceFail()
        {
            Navigation.Register("Root2", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page3));
            Navigation.Init("Root");
            Navigation.PushSync("Root2");
        }

        [TestMethod]
        public async Task NavigateAsync()
        {
            Navigation.Init("Root");
            await Navigation.Push("Page2");
        }

        [TestMethod]
        public async Task NavigateAsync_SameInstance()
        {
            Navigation.Init("Root");
            await Navigation.Push("Page2");
            await Navigation.Push("Page2", null, false, false, "Property1", "Property2");

            // Check only the root page and second page are in stack.
            Assert.IsTrue(Navigation.Instance.Navigation.NavigationStack.Count == 2);
        }

        [TestMethod]
        public async Task NavigateAsync_Replace()
        {
            Navigation.SetDefaultAllowMultiple(false);

            Navigation.Init("Root");
            await Navigation.Push("Page2");
            await Navigation.Push("Page2", replace:true);

            // Check only the root page and second page are in stack.
            Assert.IsTrue(Navigation.Instance.Navigation.NavigationStack.Count == 2);
        }

        [TestMethod]
        public async Task NavigateAsync_Multiple()
        {
            Navigation.Init("Root");
            await Navigation.Push("Page2");
            await Navigation.Push("Page2", allowMultiple: true);

            // Check the root page 2 duplicate pages2 are in the stack.
            Assert.IsTrue(Navigation.Instance.Navigation.NavigationStack.Count == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task NavigateAsync_PopRootFail()
        {
            Navigation.SetDefaultAllowMultiple(false);

            Navigation.Init("Root");
            await Navigation.Push("Root");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task NavigateAsync_NullUrl()
        {
            Navigation.Init("Root");
            await Navigation.Push(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(PageNotFoundException))]
        public async Task NavigateAsync_PageNotFound()
        {
            Navigation.Init("Root");
            await Navigation.Push("Page1");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public async Task NavigateAsync_CreateInstanceFail()
        {
            Navigation.Register("Root2", typeof(RequiredObjects.Views.Page1), typeof(RequiredObjects.ViewModels.Page3));
            Navigation.Init("Root");
            await Navigation.Push("Root2");
        }
    }
}
