# XFI_MVVM
Xamarin Forms Idiom based MVVM Navigation

## Concept
This is a package is being created to allow Xamarin Forms navigation to implement MVVM architecture, but focused on Idiom / Orientation specific views.

The idea is register a page addressable by a url / key with different views / viewmodels for desired idiom and orientations. 

This package works by using types instead of instances to save having all views and viewmodels loaded in memory, and only created the instances when shown.

The package will select the page to load by prioritsing the idiom, then filtering by orientation, and using the most appropriate page registered, so not all combinations must be registered for it to operate.

## Basic usage
Simply register the views and viewmodels to a key and specify the desired idiom and orientation combo then let the package handle the navigation. 

```csharp
new XfiPageView("Root", typeof(Views.Phone.Root), typeof(ViewModels.Root), Idiom.Phone, Orientation.Portrait).Register();
new XfiPageView("Root", typeof(Views.Desktop.Root), typeof(ViewModels.Root), Idiom.Desktop, Orientation.Landscape).Register();

new XfiPageView("Page1", typeof(Views.Phone.Page1), typeof(ViewModels.Page1), Idiom.Phone, Orientation.Portrait).Register();
new XfiPageView("Page1", typeof(Views.Desktop.Page1), typeof(ViewModels.Page1), Idiom.Desktop, Orientation.Landscape).Register();

new XfiPageView("Page2", typeof(Views.Shared.Page2), typeof(ViewModels.Page2), Idiom.Phone, Orientation.Portrait).Register();
```

In the above example there are 2 registered views for Root and Page1 both sharing the same viewmodel, but a single view and viewmodel for Page2.  
This will allow navigation by the shared url / key and the package will automatically choose which view and viewmodel is most suitable to load by simply using:

```csharp
Navigation.PushSync("Page1");
```

Currently a work in progress and not available to use.
Keep track of progress and roadmap on my trello board : https://trello.com/b/PNUTzHg7