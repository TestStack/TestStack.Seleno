using System;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IWait
    {
        /// <summary>
        /// Wait until ajax calls are complete
        /// </summary>
        /// <param name="maxWait">Maximum amount of time to wait for ajax calls to be completed (default is 5 seconds)</param>
        /// <exception cref="TimeoutException">When the ajax calls took more than maxWait to execute</exception>
        void AjaxCallsToComplete(TimeSpan maxWait = default(TimeSpan));
    }
}
