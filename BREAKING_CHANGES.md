# Dev

## Removed Android support

### Reason
Selenium WebDriver has deprecated Android and iOS support. See [here](http://seleniumhq.wordpress.com/2013/12/24/android-and-ios-support/) for details.

## Made methods and properties on UiComponent and UiComponent<> protected

### Reason
When methods and properties on UiComponent and UiComponent<> are exposed publicly it is possible to just skip encapsulating the page interaction logics in custom page objects and use the methods and properties from test script, which breaks the whole idea and mindset behind Seleno and page objects. Making them protected requires  encapsulating the logic in custom page objects. 

# Version 0.8

## Exceptions from within Seleno
Exceptions such as `SelenoException` and `NoSuchElementException` that you would previously have received from Seleno will generally now be presented as a `SelenoReceivedException`.

### Reason
Seleno now wraps all internal calls in an interceptor that automatically takes screenshots and performs logging, but in order to signal that a particular exception deep within Seleno should be ignored by the layers higher up the root exception needed to be wrapped in another exception (`SelenoReceivedException`).

### Fix
If you are relying on a particular type of exception to be thrown then simply change your logic to look for a `SelenoReceivedException` and then check the `InnerException` property for the original exception you were previously dealing with.

# Version 0.7

## WaitForAjaxCallsToComplete moved
WaitForAjaxCallsToComplete method has been removed from Executor

### Reason
A new `WaitFor` property has been added to `UiComponent` to support all waiting needs. Also `Execute.WaitForAjaxCallsToComplete` didn't make much sense from fluency point of view.

### Fix
Replace `Execute.WaitForAjaxCallsToComplete` with `WaitFor.AjaxCallsToComplete` in your code.

# Version 0.6

## ExecuteScript method renamed to Script on Executor
On Executor and IExecutor `ExecuteScript` was renamed to `Script`.

### Reason
The API wasn't very fluent and wouldn't read well; e.g. before it was `Execute().ExecuteScript` - now it is changed to `Execute.Script`

### Fix
Rename your ExecuteScript calls on Executor to Script

## UiComponent and Page methods changed to properties
Most methods on UiComponent and Page classes are changed to properties

### Reason
It was very hard to extend UiComponent and Page classes (see #41). Also to assert anything from your tests you would have to first wrap it in a method/property on your page class.

### Fix
Change calls to `Input()` to `Input` and `Execute()` to `Execute` and `Navigate()` to `Navigate`.

## `AssertThatElements` method signature on `UiComponent` has changed to not require a `By` selector
The selector has been pushed down into the `IElementAssert` methods instead of `AssertThatElements` method.

### Reason
The selector has been pushed down to firstly make the `IElementAssert` and `AssertThatElements` methods more consistent with the rest of the API and secondly allow for assertions to be done using By and jQueryBy.

### Fix
Move your selector from the `AssertThatElements` call to the `IElementAssert` method you are calling.

## `IElementAssert` and `ElementAssert` API changed
`IElementAssert` and `ElementAssert` API were not consistent with the other API and were changed. So the API changed to accept a `By` and a `jQueryBy` instead (by removing the selector from AssertThatElements method).
All methods also accept a maxWait to allow for the developer to specify how long the methods should wait for the assertion before they throw.
Internally the methods have changed to use `IElementFinder` to avoid code duplication.

###Reason
Inconsistency with IElementFinder API

###Fix
Remove the selector from finder methods and put it on the ElementAssert methods instead.

# Version 0.5.1

## InternetExplorer32 and InternetExplorer64 methods on BrowserFactory deprecated
For simplicity you now need to include the `IEDriverServer.exe` file with that name embedded in your assembly rather than a name specific to 64-bit or 32-bit. This does mean that you will need to use multiple assemblies if you are using both at the same time, but it's unlikely that would have worked previously anyway.

### Reason
It was overly complex before.

### Fix
Use the `BrowserFactory.InternetExplorer` method and embed the driver server file as `IEDriverServer.exe`.

# Version 0.5

## Host property on SelenoHost is renamed
Host property on SelenoHost is renamed to Application

### Reason
Host was a confusing name given it wasn't actually the host; but an instance of ISelenoApplication

### Fix
Normally you shouldn't be using Host property on SelenoHost but if you are then just rename the call on the instance of SelenoHost from Host to Application.

## SelenoHost is no longer static
SelenoHost class and it's methods are no longer static. 

### Reason
When SelenoHost was static you could only have one instance of the Seleno running in each AppDomain. 
This way it was impractical to have Seleno run on multiple websites or domains at the same time.
This also opens the door for adding features like running your tests on multiple browsers or with different settings.

### Fix
You should instantiate a single instance of SelenoHost and use it in all your tests:
* Add a new static class, <YourHostClass>, that instantiates and holds a reference to SelenoHost and exposes it via a static getter property, <NameOfInstanceProperty>.
* Change all the instances of `SelenoHost` with `<YourHostClass>.<NameOfInstanceProperty>`

# Version 0.4.55

## Web Element Extensions
Removed the following extension methods from IWebElement:

* `void ClearAndSendKeys(this IWebElement element, string value, bool clearFirst = true)`
* `void SetAttribute(this IWebElement element, string attributeName, string value)`
* `void ReplaceInputValueWith(this IWebDriver driver, string inputSelector, string value)`
* `T GetAttributeAsType<T>(this IWebElement element, string attributeName)`
* `T GetValueFromTextBox<T>(this IWebElement element)`
* `T TextAsType<T>(this IWebElement element)`

### Reason
We are trying to move away from exposing Selenium interfaces and classes in our public API so that we can provide a driver agnostic API for you to use. 

### Fix
* ClearAndSendKeys method
	* If you are inside a typed page object (e.g. extending Page<TModel>) then replace with Input().ClearAndSendKeys(...) [there are two overloads]
	* Otherwise simply replace with element.Clear(); element.SendKeys(...);
	* Alternatively, you can use the new HtmlControl support (e.g. HtmlControlFor<TextBox>()...) or the Input().ReplaceInputValueWith(...)
* SetAttribute method
	* If you are inside a typed page object (e.g. extending Page<TModel>) then replace with Input().SetAttribute(...)
* ReplaceInputValueWith method
	* If you are inside a typed page object (e.g. extending Page<TModel>) then replace with Input().ReplaceInputValueWith(...)
* GetAttributeAsType method
	* If you are inside a typed page object (e.g. extending Page<TModel>) then replace with Read().GetAttributeAsType(...)
* GetValueFromTextBox method
	* If you are inside a typed page object (e.g. extending Page<TModel>) then replace with Read().GetValueFromTextBox(...)
* TextAsType method
	* If you are inside a typed page object (e.g. extending Page<TModel>) then replace with Read().TextAsType(...)

## Execute() changes

A number of changes have been made to the object returned from the Execute() method on `UiComponent` / `Page`:

* `IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20)` has been deprecated
* `TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func)` has been deprecated
* The interface has been changed from `IScriptExecutor` to `IExecutor`

### Reason

The two deprecated methods had obvious equivalents and the interface was changed because it encapsulated more than just executing JavaScript.

### Fix
* WithPatience method
	* `ActionOnLocator` now contains a `maxWait` parameter so is an equivalent for the `WithPatience` method
* ActionOnLocator functor overload
	* The deprecated `ActionOnLocator` method can be replaced by Find().Element() and then the stuff that was in the functor to map the returned `IWebElement` to the desired value
* IScriptExecutor interface
	* Change any references you had to `IScriptExecutor` to `IExecutor`

## Find() changes

A number of changes have been made to the object returned from the Find() method on `UiComponent` / `Page`:

* `IWebElement ElementWithWait(By findElement, int waitInSeconds = 20)` has been renamed to `Element`
* `IWebElement TryFindElement(By by)` has been renamed to `OptionalElement`
* `IWebElement TryFindElementByjQuery(Locators.By.jQueryBy by)` has been renamed to `OptionalElement`

### Reason

The DSL reads better as `Find().Element()` and `Find().OptionalElement()` and having overloads for the By.jQuery finders rather than separate methods makes the API more consistent.

### Fix

* `ElementWithWait` method
	* Change any references you had to `ElementWithWait` to `Element`
* `TryFindElement` method
	* Change any references you had to `TryFindelement` to `OptionalElement`
* `TryFindElementByjQuery`
	* Change any references you had to `TryFindElementByjQuery` to `OptionalElement`

## Navigate() changes

The `void To(By clickDestination)` method has been deprecated.

### Reason

There is no point in navigating to a page if you aren't returning another page object.

### Fix

If you were navigating to the page to cause a HTTP request then use a `HttpClient` instead. If you were navigating in order to click an element then instead use the `Click()` method after getting the `IWebElement` via `Find().Element(...)`.

## SelenoApplicationRunner

`SelenoApplicationRunner` has been renamed to `SelenoHost`

### Reason

It was less verbose and seemed like a better fit.

### Fix

Change any references you had to `SelenoApplicationRunner` to `SelenoHost`

## Maximum wait time

The maximum wait parameter for all methods that accept a maximum wait has been changed from taking an integer (in seconds) to a `TimeSpan`. Also, the default value is now 5 seconds rather than 20 seconds.

### Reason

Having a `TimeSpan` rather than an integer in seconds is a lot more flexible and having a default maximum wait of 20 seconds is an abnormally long time to wait.

### Fix

Change all instances where you specified an integer for maximum wait to `TimeSpan.FromSeconds(integer_you_had_before)`. Any tests that now fail because the 5s maximum wait time is too small should be changed to specify an explicit maximum wait time that works.

## Starting automated tests (replacing newing up page objects)

Previously, the automated tests would be started by newing up a page object. Now if page objects are newed up then you will receive an exception. Also, previously the page that the web browser was on for the previous test was still shown when newing up the page object, but now when a test is initialised you choose which page the test should start on (the root/homepage by default).

### Reason

Seleno should be managing the lifetime / instantiation / initialisation of all page objects and each tests should be independant of other tests that are run.

### Fix

Change all instances where you newed up page objects with a call to one of the `SelenoHost.NavigateToInitialPage` overloads.

## MVC Route config

Rather than using the global route config `RouteTable.Routes`, Seleno now has a self-contained route collection that can be configured by the fluent configurator.

### Reason

Rather than using a global object that might affect other code Seleno has a self-contained `RouteCollection`. Furthermore, providing a configuration method to specify the routes is more in keeping with how the rest of Seleno is globally configured.

### Fix

Instead of using `RouteConfig.RegisterRoutes(RouteTable.Routes)` when setting up Seleno, hook into the new `WithRouteConfig` method on the `IAppConfigurator`, e.g.:

    SelenoHost.Run("Your.Web.Project.Name", PortNumber, c => c
        .WithRouteConfig(RouteConfig.RegisterRoutes)
    );

## Internal accessibility

A bunch of internal implementation classes / properties have been marked internal:

* `AppConfigurator`
* `SelenoApplication`
* `IisExpressWebServer`
* `WebDriverExtensions`
	* `void WaitForSeconds(this IWebDriver driver, int seconds)`
	* `void WaitForMilliseconds(this IWebDriver driver, int milliseconds)`
	* `TReturn ExecuteScriptAndReturn<TReturn>(this IWebDriver driver, string javascriptToBeExecuted)`
	* `IWebElement FindElement(this IWebDriver driver, By by, Func<IWebElement, bool> predicate)`
	* `IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, Func<IWebElement, bool> predicate)`
	* `int CountNumberOfElements(this IWebDriver browser, By by, Func<IWebElement, Boolean> predicate = null)`
	* `IWebElement WaitForElement(this IWebDriver driver, OpenQA.Selenium.By by, Func<IWebElement, bool> predicate = null, int seconds = DefaultSecondTimeout)`
	* `IWebElement WaitForElement(this IWebDriver driver, By.jQueryBy by, Func<IWebElement, bool> predicate = null, int seconds = DefaultSecondTimeout)`
	* `IEnumerable<IWebElement> WaitForElements(this IWebDriver driver, By.jQueryBy by, Func<IWebElement, bool> predicate = null, int seconds = DefaultSecondTimeout)`
	* `IEnumerable<IWebElement> WaitForElements(this IWebDriver driver, OpenQA.Selenium.By by, Func<IWebElement, bool> predicate = null, int seconds = DefaultSecondTimeout)`
	* `IJavaScriptExecutor GetJavaScriptExecutor(this IWebDriver driver)`
	* `string GetText(this IWebDriver driver)`
	* `bool HasElement(this IWebDriver driver, OpenQA.Selenium.By by)`
	* `bool HasElement(this IWebDriver driver, By.jQueryBy byJQuery)`
	* `bool HasElement(this IWebDriver driver, Func<IWebDriver, IWebElement> elementFinder)`
* `ElementFinder`
* `ScriptExecutor` (also renamed to Executor)
* `PageNavigator`
* `PageReader`
* `PageWriter`
* Protected properties on `UiComponent`
	* `ElementFinder` (use Find() instead)
	* `ScriptExecutor` (use Execute() instead)

### Reason

In order to hide the internal implementation of Seleno and provide a discoverable API these classes have been marked internal.

### Fix

If you are using these classes directly then you need to work out a different way of achieving your goal. If you think that any of these classes are useful in the public API then please [lodge an issue on the GitHub site](https://github.com/TestStack/TestStack.Seleno/issues).

## Web driver configuration

The configuration method for specifying a different web driver has been renamed to `WithRemoteWebDriver` (from `WithWebDriver`) and now takes a `Func<RemoteWebDriver>` rather than a `Func<IWebDriver>`.

### Reason

We need to register an `IJavaScriptExecutor` within our dependency injection container so that our API wrappers for executing JavaScript can function correctly. Previously, the `IWebDriver` that was passed into the configuration was simply type-casted to an `IJavaScriptExecutor`. This is kind of nasty and makes it a bit harder to unit test (and also isn't very discoverable in our public API. Hence, we decided to take the dependency on `RemoteWebDriver` instead, which implements both `IWebDriver` and `IJavaScriptExecutor`.

### Fix

IF you were using the Seleno `BrowserFactory` class then it's already been modified to return the correct factories for this change. If you implemented a custom browser implementation it will now need to extend `RemoteWebDriver`.

## Logging configuration

The custom logging API has been replaced with the [Castle.Core logging API](http://docs.castleproject.org/Windsor.logging-facility.ashx).

### Reason

Hand-rolling a logging library is like reinventing the wheel. Castle.Core is a widely used library so we are happy in taking it as a dependency (and it might come in handy in the future since we might use some aspect-oriented programming). It also handily has a bunch of integrations with common logging libraries.

### Fix

Replace any calls to `UsingLogger` in the configuration with a call to `UsingLoggerFactory` and passing in the relevant Castle.Core logger (or your own implementation of the Castle.Core `ILoggerFactory` interface to bridge the gap with your logging library.
