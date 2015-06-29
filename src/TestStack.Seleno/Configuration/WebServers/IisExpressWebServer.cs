using System;
using System.Diagnostics;
using Holf.AllForOne;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.WebServers
{
    internal class IisExpressWebServer : IWebServer
    {
        private static WebApplication _application;
        private static Process _webHostProcess;

        public IisExpressWebServer(WebApplication application)
        {
            if (application == null)
                throw new AppConfigurationException("The web application must be set.");
            _application = application;
        }

        public void Start()
        {
            var webHostStartInfo = ProcessStartInfo(_application);
            _webHostProcess = Process.Start(webHostStartInfo);
            _webHostProcess.TieLifecycleToParentProcess();
        }

        public void Stop()
        {
            if (_webHostProcess == null)
                return;
            if (!_webHostProcess.HasExited)
                _webHostProcess.Kill();
            _webHostProcess.Dispose();
        }

        public string BaseUrl
        {
            get { return string.Format("http://localhost:{0}", _application.PortNumber); }
        }

		private static bool Is64BitProcess()
		{
			if (IntPtr.Size == 4)
			{
				return false;
			}
			else if (IntPtr.Size == 8)
			{
				return true;
			}
			return false;
		}

		private static string GetProgramFilesPath()
		{
			var key = Environment.Is64BitOperatingSystem && !Is64BitProcess() ? "programfiles(x86)" : "programfiles";
			var programfiles = Environment.GetEnvironmentVariable(key);
			return programfiles;
		}
        
        private static ProcessStartInfo ProcessStartInfo(WebApplication application)
        {
            // todo: grab stdout and/or stderr for logging purposes?
			var programfiles = GetProgramFilesPath();

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments = String.Format("/path:\"{0}\" /port:{1}", application.Location.FullPath, application.PortNumber),
                FileName = string.Format("{0}\\IIS Express\\iisexpress.exe", programfiles)
            };

            foreach (var variable in application.EnvironmentVariables)
                startInfo.EnvironmentVariables.Add(variable.Key, variable.Value);

            return startInfo;
        }
    }
}
