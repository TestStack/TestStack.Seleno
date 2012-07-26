### What is Seleno?
Seleno helps you to write automated UI tests in the right way by implementing Page Objects and Page Components and by reading and writing web page data using strongly typed view models. It uses Selenium for browser automation.

### What are Page Objects?
The Page Object design pattern is a way of encapsulating the interaction with an individual page in your application in a single object. It makes heavy use of OO principles to enable code reuse and improve maintenance. Rather than having tests that are a series of Selenium commands that are sent to the server, your tests become a series of interactions with objects that represent a page (or part of one).

### How does it work?
The usage of the Page Object design pattern creates a strong separation of concerns between  tests and Page Objects. The tests specify what should happen and the Page Objects encapsulate how it happens. 
* Tests are very procedural. They only interact with the Page Objects and make assertions. They should not have any implementation details, such as Selenium calls, whatsoever. 
* Page Objects encapsulate all of the interaction with Selenium, and all of the logic of that interaction. There are no test assertions in the Page Objects but they can throw exceptions.

### What are the benefits of using Page Objects?
* Separating test specification from test implementation makes tests more robust. If you change how a feature works, you just have to change the test, rather than every test that uses the feature.
* Maintenance is easier because you only have to change things in one place.
* Tests are more readable as they just work with Page Objects and make assertions. They do not have any Selenium code as this is hidden away in the Page Object. 

### Authors and Contributors
Mehdi Khalili (@MehdiK), Michael Whelan (@mwhelan)