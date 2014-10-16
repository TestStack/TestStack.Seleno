using System;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.Interceptors
{
    class CameraProxyInterceptor : IInterceptor
    {
        private readonly ICamera _camera;
        private readonly string _filename;
        private readonly ILogger _logger;

        public CameraProxyInterceptor(ICamera camera, string filename, ILogger logger)
        {
            _camera = camera;
            _filename = filename;
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.ErrorFormat(e, "Error invoking {0}.{1}", invocation.TargetType.Name, invocation.Method.Name);
                var filename = _filename + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

                if (_camera.ScreenshotTaker == null)
                    _logger.WarnFormat("ITakesScreenshot isn't supported by the web driver {0} so taking a screenshot probably won't work", _camera.Browser.GetType().Name);
                else
                    _logger.DebugFormat("Taking screenshot with filename: {0}", filename);

                try
                {
                    _camera.TakeScreenshot(filename);
                }
                catch (Exception ex)
                {
                    _logger.Error("Error saving a screenshot", ex);
                }

                if (e is SelenoReceivedException)
                    throw;

                throw new SelenoReceivedException(e);
            }
        }
    }
}
