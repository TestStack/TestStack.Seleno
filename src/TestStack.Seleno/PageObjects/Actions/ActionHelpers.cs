using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Locators;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    public static class ActionHelpers
    {
        public static IWebElement PerformActionOn(this IWebDriver browser, By by, Action<IWebElement> actionToPerform)
        {
            var element = browser.FindElement(@by);

            actionToPerform(element);

            return element;
        }

        public static IWebElement PerformActionOn(this IWebDriver browser, Locators.By.jQueryBy by, Action<IWebElement> actionToPerform)
        {
            var element = browser.FindElementByjQuery(@by);

            actionToPerform(element);

            return element;
        }

        public static void SendKeyBoardShortCut(this IWebDriver browser, string commandKey, char key)
        {
            browser.SendKeyBoardShortCut(new[] { commandKey }, key);
        }

        public static void SendKeyBoardShortCut(this IWebDriver browser, string[] commandKeys, char key)
        {

            var remoteDriver = (RemoteWebDriver)browser;

            var javaScriptBuilder = new StringBuilder("var keypress = $.Event(\"keydown\");");

            foreach (var commandKey in commandKeys)
            {
                var keyCombination = String.Empty;

                if (commandKey == Keys.Control || commandKey == Keys.LeftControl)
                {
                    keyCombination = "ctrlKey";
                }

                if (commandKey == Keys.Alt || commandKey == Keys.LeftAlt)
                {
                    keyCombination = "altKey";
                }

                if (commandKey == Keys.Shift || commandKey == Keys.LeftShift)
                {
                    keyCombination = "shiftKey";
                }


                if (!String.IsNullOrEmpty(keyCombination))
                {
                    javaScriptBuilder.AppendFormat("keypress.{0} = true;", keyCombination);
                }
            }


            javaScriptBuilder.AppendFormat("keypress.which = {0};", (int)key);

            javaScriptBuilder.Append("$(document).trigger(keypress);");

            remoteDriver.ExecuteScript(javaScriptBuilder.ToString());
        }

        public static long GetCursorPosition(this IWebDriver browser)
        {
            return browser.ExecuteScriptAndReturn<long>("window.getSelection().getRangeAt(0).startOffset");
        }

        public static long GetSelectionLength(this IWebDriver browser, string jquerySelector)
        {
            return browser.ExecuteScriptAndReturn<long>($"$('{jquerySelector}').getSelection().length");
        }

        public static void SetCursorFocusOnAt(this IWebDriver browser, string jquerySelector, int caretPosition = 0)
        {
            var remoteDriver = (RemoteWebDriver)browser;

            var javaScriptBuilder =
                new StringBuilder(@"var getSelectedRange = function() {

                                    var selectedRange;

                                    //non IE Browsers
                                    if (window.getSelection) {
                                        selectedRange = window.getSelection().getRangeAt(0);
                                    }
                                    //IE
                                    else if (document.selection) {
                                        selectedRange = document.selection.createRange();
                                    }

                                    return selectedRange;
                                };


                                var setSelection = function(container, offset) {
                                    var range = document.createRange();
                                    range.setStart(container, offset);
                                    range.setEnd(container, offset);

                                    var sel = window.getSelection();
                                    sel.removeAllRanges();
                                    sel.addRange(range);
                                };");

            javaScriptBuilder.AppendFormat("var event = $(\"{0}\");", jquerySelector);
            javaScriptBuilder.Append(@"event.focus();
                                       var selectedRange = getSelectedRange();
                                       event.click();");


            javaScriptBuilder.AppendFormat("setSelection(selectedRange.commonAncestorContainer, {0});", caretPosition);

            remoteDriver.ExecuteScript(javaScriptBuilder.ToString());
        }
    }
}
