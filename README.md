# XFI_MVVM
Xamarin Forms Idiom based MVVM Navigation

## Concept
This is a package is being created to allow Xamarin Forms navigation to implement MVVM architecture, but focused on Idiom / Orientation specific views.

The idea is register a page addressable by a url / key with different views / viewmodels for desired idiom and orientations. 

This package works by using types instead of instances to save having all views and viewmodels loaded in memory, and only created the instances when shown.

The package will select the page to load by prioritsing the idiom, then filtering by orientation, and using the most appropriate page registered, so not all combinations must be registered for it to operate.

## Basic usage
Views you wish to use must at some level inherit one of the XFI page types currently supported (more to come):

```csharp
XFI_MVVM.Pages.XfiContePage
```

ViewModels must at some level inherit the base ViewModel of the package:

```csharp
XFI_MVVM.Models.XfiViewModel
```

Defaults can be set through methods exposed in Navigation which allow you to set how you want the package to work if no parameters are passed through

```csharp
// If pages should be opened as modal by default.
Navigation.SetDefaultIsModal(value)

// If multiple instances of the same page are allowd.
Navigation.SetDefaultAllowMultiple(value)

// If exsting open pages of the same type get replaced with a new instance.
Navigation.SetDefaultReplaceInstance(value)

// What the perfered idiom should be.
Navigation.SetDefaultIdiom(value)

// What the prefered orientation should be.
Navigation.SetDefaultOrientation(value)
```

Simply register the views and viewmodels to a key and specify the desired idiom and orientation combo then let the package handle the navigation. 

```csharp
// Register 2 pages for root, one for phone and another for desktop using different views but the same viewmodel.
Navigation.Register("Root", typeof(Views.Phone.Root), typeof(ViewModels.Root), Idiom.Phone, Orientation.Portrait);
Navigation.Register("Root", typeof(Views.Desktop.Root), typeof(ViewModels.Root), Idiom.Desktop, Orientation.Landscape);

// Register 2 pages for page 1, one for phone and another for desktop using differet views and different viewmodels.
Navigation.Register("Page1", typeof(Views.Phone.Page1), typeof(ViewModels.Page1), Idiom.Phone, Orientation.Portrait);
Navigation.Register("Page1", typeof(Views.Desktop.Page1), typeof(ViewModels.Page1Destop), Idiom.Desktop, Orientation.Landscape);

// Register 1 page for page 2, will always be the page displayed regardless of idiom / orientation.
Navigation.Register("Page2", typeof(Views.Shared.Page2), typeof(ViewModels.Page2));
```

In the above example it shows how view models can be shared between different views or also seperated with idiom or orientation.
It also shows that not all targeted platforms / orientations need seperate views or viewmodels.

Navigation by the shared url / key allows the package to automatically choose which view and viewmodel is most suitable to load by simply using:

```csharp
// Navigate to the registered page by the url provided asynchronously.
await Navigation.Push("YourPageUrl");

// Navigate to the registered page by the url provided synchronously.
Navigation.PushSync("YourPageUrl");
```

To initalise and set the root of the Navigation Page simply use

```csharp
Navigation.Init("YourRootPageUrl");
```

Currently a work in progress and not available to use.
Keep track of progress and roadmap on my trello board : https://trello.com/b/PNUTzHg7