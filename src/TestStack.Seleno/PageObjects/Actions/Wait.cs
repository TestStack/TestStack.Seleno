using System;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class Wait : IWait
    {
        private readonly IExecutor _executor;

        public Wait(IExecutor executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Wait until ajax calls are complete
        /// </summary>
        /// <param name="maxWait">Maximum amount of time to wait for ajax calls to be completed (default is 5 seconds)</param>
        /// <exception cref="TimeoutException">When the ajax calls took more than maxWait to execute</exception>
        public void AjaxCallsToComplete(TimeSpan maxWait = new TimeSpan())
        {
            _executor.PredicateScriptAndWaitToComplete("$.active == 0", maxWait);
        }
    }
}