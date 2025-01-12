using System.Diagnostics;

namespace Api.Template.Integration.Tests.Tests
{
    public class BaseFunctionTestFixture : IDisposable
    {
        private readonly Process _funcHostProcess;

        public int Port => 7001;

        public readonly HttpClient Client = new();

        public BaseFunctionTestFixture()
        {
            var functionApplicationPath = "..\\..\\..\\..\\Api.Template.App";
            var mode = "Release";
#if DEBUG
            mode = "Debug";
#endif
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), functionApplicationPath);
            var directoryInfo = new DirectoryInfo(functionAppFolder);
            var appInfo = new ProcessStartInfo("func", $"start --port {Port} --prefix bin/{mode}/net8.0")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = directoryInfo.FullName
            };
            _funcHostProcess = new Process { StartInfo = appInfo };
            var success = _funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }

            Client.BaseAddress = new Uri($"http://localhost:{Port}");
        }

        public virtual void Dispose()
        {
            if (!_funcHostProcess.HasExited)
            {
                _funcHostProcess.Kill();
            }

            _funcHostProcess.Dispose();
        }
    }
}
