using System;
using System.Linq;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.Interceptors
{
    class DomCaptureProxyInterceptor : IInterceptor
    {
        private readonly IDomCapture _domCapture;
        private readonly string _filename;
        private readonly ILogger _logger;

        public DomCaptureProxyInterceptor(IDomCapture domCapture, string filename, ILogger logger)
        {
            _domCapture = domCapture;
            _filename = filename;
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (SelenoReceivedException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.ErrorFormat(e, "Error invoking {0}.{1}", invocation.TargetType.Name, invocation.Method.Name);
                var filename = _filename + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") + ".html";

                try
                {
                    _domCapture.CaptureDom(filename);
                }
                catch (Exception ex)
                {
                    _logger.Error("Error saving a DOM", ex);
                }

                throw new SelenoReceivedException(e);
            }
        }
    }
}
