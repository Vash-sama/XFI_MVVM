# XFI_MVVM
Xamarin Forms Idiom based MVVM Navigation

# Contents
 1. [Concept](#concept)
 1. [Basic usage](#basic-usage)
    1. [Inheritance](#inheritance)
    1. [Defaults](#defaults)
    1. [Register Pages](#register-pages)
    1. [Navigation](#navigation)
    1. [Initalize](#initalize)
 1. [Events](#events)
 1. [De-Register Pages](#de-register-pages)
 1. [Disposing](#disposing)
 1. [Custom Idioms](#custom-idioms)
 1. [Roadmap](#Roadmap)

## Concept
This is a package is being created to allow Xamarin Forms navigation to implement MVVM architecture, but focused on Idiom / Orientation specific views.

The idea is register a page addressable by a url / key with different views / viewmodels for desired idiom and orientations. 

This package works by using types instead of instances to save having all views and viewmodels loaded in memory, and only created the instances when shown.

The package will select the page to load by prioritsing the idiom, then filtering by orientation, and using the most appropriate page registered, so not all combinations must be registered for it to operate.

## Basic usage
### Inheritance
Views you wish to use must at some level inherit one of the XFI page types currently supported (more to come):

```csharp
XFI_MVVM.Pages.XfiContentPage
```

ViewModels must at some level inherit the base ViewModel of the package:

```csharp
XFI_MVVM.Models.XfiViewModel
```

### Defaults
Defaults can be set through methods exposed in Navigation which allow you to set how you want the package to work if no parameters are passed through

```csharp
// If pages should be opened as modal by default.
Navigation.SetDefaultIsModal(value);

// If multiple instances of the same page are allowd.
Navigation.SetDefaultAllowMultiple(value);

// If exsting open pages of the same type get replaced with a new instance.
Navigation.SetDefaultReplaceInstance(value);

// What the perfered idiom should be.
Navigation.SetDefaultIdiom(value);

// What the prefered orientation should be.
Navigation.SetDefaultOrientation(value);

// If the package should handle device orientation change and try to reload a more appropriate view.
Navigation.SetHandleOrientationChange(value);

// When package is set to handle orientation change, setting this to true will cause the package to try to keep the viewmodel instance for an orientation change if the viewmodel types are the same for both registered views.
Navigation.SetTryToKeepViewModelOnOrientationChange(value);

// Forces the package to use the supplied idiom regardless. Required if custom idioms are used to allow the package to know which custom idiom is expected.
Navigation.SetIdiomOverride(value);
```

### Register Pages
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

### Navigation
Navigation by the shared url / key allows the package to automatically choose which view and viewmodel is most suitable to load by simply using:

```csharp
// Navigate to the registered page by the url provided asynchronously.
await Navigation.Push("YourPageUrl");

// Navigate to the registered page by the url provided synchronously.
Navigation.PushSync("YourPageUrl");
```

### Initalize
To initalise and set the root of the Navigation Page simply use

```csharp
Navigation.Init("YourRootPageUrl");
```

## Events
This package will trigger events that can be handled throughout the navigation process, 
All events will pass the NavEvent arguments through which contains the current processing data.

Currently available events are and vaguely inorder:

| Event Name | Description |
|------------|-------------|
| `StartedNavigation`       |       This is triggered whenever a navigation is started either through Pushing a new screen or an orientation change handling a new view load. Ideal for starting any loading animations / overlays. |
| `InitalizingViewModel`    |       This is triggered just before the Instance of a ViewModel is created during navigation. |
| `InitalisedViewModel`     |       This is triggered after the creation of the ViewModel instance, i.e. after all processing in the ViewModels contructor. |
| `InitalizingView`         |       This is triggered just before the Instance of the View is created and the ViewModel is bound to it. |
| `InitalizedView`          |       This is triggered after the creation of the View instance, i.e. after processing the contructor and binding the ViewModel. |
| `FinishedNavigation`      |       This is triggered after everything has finished processing, the new view should have been pushed to the screen at this point. |

## De-Register Pages
Use this to remove pages that have been registered and are not longer going to be navigated to.

```csharp
Navigation.DeRegister("Page1");
Navigation.DeRegister("Page1", Idiom.Desktop, Orientation.Landscape);
```

This will remove the page Page1 with default idiom and orientation from the list and Page1 with specific idiom of Desktop and specific orientation of Landscape.

## Disposing
All Pages and ViewModels inheriting the requried types of this package will have access to override the `Dispose` method.
This method will be called for both the View and ViewModel when the page is removed from the stack in anyway.

```csharp
public override void Dispose()
{
    base.Dispose();
    // Remove handlers or subscriptions here.
}
```

## Custom Idioms
In order to allow extensibility of the package, I have allowed the creation of the Idiom enum, these should start from 5 to keep sequence with existing Idioms but its not vital.

This functionality can be used to create idioms for specific screen sizes that you want e.g. small phone, large phone, folding phone, etc,

This can also be used to AB test views and flows e.g.:

```csharp
var ABTest1 = new Idiom(5, "ABTest1");
var ABTest2 = new Idiom(6, "ABTest2");
```

To use these custom idioms you must handle the idiom selection in your own app, and specify to the package which idiom you plan to use as an override.

In this example i have 2 idioms and the application can decide if the user is group 1 or not and specify the idiom override at any point, this can be returned to Null to reset.

```csharp
if (userInGroup1)
    Navigation.SetIdiomOverride(ABTest1);
else
    Navigation.SetIdiomOverride(ABTest2);
```
If no screen layout is found that matches the override it will use the same logic to find the most relevent view to display.

To use them for your own screen simply register a screen with your custom idiom.

```csharp
Navigation.Register("Root", typeof(Views.ABTesting.Root1), typeof(ViewModels.Root), ABTest1);
Navigation.Register("Root", typeof(Views.ABTesting.Root2), typeof(ViewModels.Root), ABTest2);
```

Where the above will use root1 for users in group 1 but root2 for everyone else.

## Roadmap
Currently a work in progress and not available to use.
Keep track of progress and roadmap on my trello board : https://trello.com/b/PNUTzHg7