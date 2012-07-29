namespace TestStack.Seleno.PageObjects.Locators
{
    public class By : OpenQA.Selenium.By
    {
        /// <summary>
        /// jQuery selector
        /// </summary>
        public static jQueryBy jQuery(string selector)
        {
            return new jQueryBy("(\"" + selector + "\")");
        }

        /// <summary>
        /// Specialized "ActionOnLocator" class for jQuery selector
        /// </summary>
        public class jQueryBy
        {
            public string Selector
            {
                get;
                set;
            }

            public jQueryBy(string selector)
            {
                this.Selector = selector;
            }

            public jQueryBy Html(string selector = "")
            {
                return Function("html", selector);
            }


            #region ----Tree Traversal----

            public jQueryBy Children(string selector = "")
            {
                return Function("children", selector);
            }

            public jQueryBy Closest(string selector = "")
            {
                return Function("closest", selector);
            }

            public jQueryBy Find(string selector = "")
            {
                return Function("find", selector);
            }

            public jQueryBy Next(string selector = "")
            {
                return Function("next", selector);
            }

            public jQueryBy NextAll(string selector = "")
            {
                return Function("nextAll", selector);
            }

            public jQueryBy NextUntil(string selector = "", string filter = "")
            {
                return Function("nextUntil", selector, filter);
            }

            public jQueryBy OffsetParent()
            {
                return Function("offsetParent");
            }

            public jQueryBy Parent(string selector = "")
            {
                return Function("parent", selector);
            }

            public jQueryBy Parents(string selector = "")
            {
                return Function("parents", selector);
            }

            public jQueryBy ParentsUntil(string selector = "", string filter = "")
            {
                return Function("parentsUntil", selector, filter);
            }

            public jQueryBy Prev(string selector = "")
            {
                return Function("prev", selector);
            }

            public jQueryBy PrevAll(string selector = "")
            {
                return Function("prevAll", selector);
            }

            public jQueryBy PrevUntil(string selector = "", string filter = "")
            {
                return Function("prevUntil", selector, filter);
            }

            public jQueryBy Siblings(string selector = "")
            {
                return Function("siblings", selector);
            }

            #endregion

            #region -----Filtering----

            public jQueryBy Eq(int index)
            {
                return Function("eq", index.ToString());
            }

            public jQueryBy First()
            {
                return Function("first");
            }

            public jQueryBy Has(string selector)
            {
                return Function("has", selector);
            }

            public jQueryBy Last()
            {
                return Function("last");
            }

            public jQueryBy Not(string selector)
            {
                return Function("not", selector);
            }

            #endregion

            private jQueryBy Function(string func, string selector = "", string additionalArg = "")
            {
                //Add quotes to selector
                if (selector != "")
                    selector = "\"" + selector + "\"";

                //Add additional paramater
                if (additionalArg != "")
                    selector += ",\"" + additionalArg + "\"";

                //Add either: .func() or .func("selector") to original selector
                return new jQueryBy(this.Selector + "." + func + "(" + selector + ")");
            }
        }
    }
}
