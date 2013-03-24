# Version 0.4.X

## Web Element Extensions
Removed the following extension methods from IWebElement:

* `void ClearAndSendKeys(this IWebElement element, string value, bool clearFirst = true)`
* `void SetAttribute(this IWebElement element, string attributeName, string value)`
* `void ReplaceInputValueWith(this IWebDriver driver, string inputSelector, string value)`
* `T GetAttributeAsType<T>(this IWebElement element, string attributeName)`
* `T GetValueFromTextBox<T>(this IWebElement element)`
* `T TextAsType<T>(this IWebElement element)`

### Reason
We are trying to move away from exposing Selenium interfaces and classes in our public API so that we can provide a driver agnostic API for you to use. This gives us flexibility in the future to support other drivers e.g. Phantom etc.

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

## Web driver specification

The configuration method for specifying a different web driver has been renamed to `WithRemoteWebDriver` (from `WithWebDriver`) and now takes a `Func<RemoteWebDriver>` rather than a `Func<IWebDriver>`.

### Reason

We need to register an `IJavaScriptExecutor` within our dependency injection container so that our API wrappers for executing JavaScript can function correctly. Previously, the `IWebDriver` that was passed into the configuration was simply type-casted to an `IJavaScriptExecutor`. This is kind of nasty and makes it a bit harder to unit test (and also isn't very discoverable in our public API. Hence, we decided to take the dependency on `RemoteWebDriver` instead, which implements both `IWebDriver` and `IJavaScriptExecutor`.

### Fix

IF you were using the Seleno `BrowserFactory` class then it's already been modified to return the correct factories for this change. If you implemented a custom browser implementation it will now need to extend `RemoteWebDriver`.
