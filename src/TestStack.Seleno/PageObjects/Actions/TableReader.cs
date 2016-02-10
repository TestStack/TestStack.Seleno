using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Locators;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class TableReader<TViewModel> : IEnumerable<TViewModel>
         where TViewModel : class, new()
    {
        public const string PropertyNameAttribute = "data-property-name";

        protected internal IWebDriver Browser;
   
        readonly string _gridId;
        private IEnumerable<PropertyInfo> _displayedPropertiesInGrids;
        private IList<TViewModel> _rows;
        private long? _numberOfRows;

        public TableReader(string gridId)
        {
            _gridId = gridId;
        }

        private IEnumerable<PropertyInfo> DisplayedProperties
        {
            get
            {
                if (_displayedPropertiesInGrids == null)
                {

                    var propertyNames = GetColumnNames();

                    _displayedPropertiesInGrids =
                        typeof(TViewModel)
                            .GetProperties().Where(p => propertyNames.Contains(p.Name));
                }

                return _displayedPropertiesInGrids;
            }
        }

        private IEnumerable<TViewModel> Rows
        {
            get
            {
                if (_rows == null)
                {
                    _rows = new List<TViewModel>();
                    for (var rowIndex = 1; rowIndex <= NumberOfRows; rowIndex++)
                    {
                        _rows.Add(GetRowAt(rowIndex));
                    }
                }
                return _rows;
            }
        }

        public IEnumerable<string> DisplayedColumNames { get { return DisplayedProperties.Select(x => x.Name); } }

        /// <summary>
        /// Return a row In the Grid reading data of the HTML page and mapping it back to the ViewModel used to generate it
        /// </summary>
        /// <param name="rowNumber">number of the row 1 based</param>
        /// <returns></returns>
        public TViewModel this[int rowNumber] => GetRowAt(rowNumber);

        public long NumberOfRows
        {
            get
            {
                if (!_numberOfRows.HasValue)
                {
                    _numberOfRows = Browser.ExecuteScriptAndReturn<long>($"$('#{_gridId} tbody tr').size()");
                }

                return _numberOfRows.Value;
            }
        }

        /// <summary>
        /// Retrieve a cell value in grid for specified row number
        /// </summary>
        /// <param name="rowIndex">number of the row in the grid to click on (1 based)</param>
        /// <param name="propertySelector">An expression that indicates which property to get the value for in the given row</param>
        /// <returns>The value of the requested cell</returns>
        public TProperty GetCellValueFor<TProperty>(int rowIndex, Expression<Func<TViewModel, TProperty>> propertySelector)
        {
            var property = propertySelector.GetPropertyFromLambda();

            return (TProperty)GetCellValue(rowIndex, property);
        }

        public IEnumerator<TViewModel> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private TViewModel GetRowAt(int rowIndex = 1)
        {
            var item = new TViewModel();

            foreach (var property in DisplayedProperties)
            {
                var cellValue = GetCellValue(rowIndex, property);

                if (property.CanWriteToProperty(cellValue))
                {
                    property.SetValue(item, cellValue, null);
                }
            }

            return item;
        }

        private IEnumerable<String> GetColumnNames()
        {
            var selector = $"#{_gridId} thead th[{PropertyNameAttribute}]";

            return
                Browser
                    .FindElements(By.jQuery(selector))
                    .Select(e => e.GetAttribute(PropertyNameAttribute)
                                  .Split('_')
                                  .Last())
                    .ToList();
        }


        private IWebElement GetCellFor(int rowIndex, PropertyInfo property)
        {
            var selector = GetCellSelector(rowIndex, property.Name);

            return Browser.FindElementByjQuery(By.jQuery(selector));
        }

        string GetCellSelector(int rowIndex, string propertyName)
        {
            var selector = $"#{_gridId} tr:eq({rowIndex}) td[{PropertyNameAttribute}$='{propertyName}']";

            return selector;
        }

        private object GetCellValue(int rowIndex, PropertyInfo property)
        {
            if (property.PropertyType == typeof(bool))
            {
                var javaScriptCheckBoxStateRetriever =
                    $"$(\"{GetCellSelector(rowIndex, property.Name)} input[type=checkbox]\").is(':checked')";

                return Browser.ExecuteScriptAndReturn<bool>(javaScriptCheckBoxStateRetriever);

            }

            var cellText = GetCellFor(rowIndex, property).Text;

            var typedValue = !String.IsNullOrWhiteSpace(cellText) ? cellText.TryConvertTo<string>(property.PropertyType) : null;
            return typedValue;
        }

    }
}
