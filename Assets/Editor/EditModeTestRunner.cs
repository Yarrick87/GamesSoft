using System.IO;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace GamesSoft.EditorTools
{
    public static class EditModeTestRunner
    {
        private const string ResultsPath = "TestResults-EditMode.txt";

        [MenuItem("GamesSoft/Run EditMode Tests")]
        public static void Run()
        {
            AssetDatabase.Refresh();
            var absolutePath = Path.GetFullPath(ResultsPath);
            File.WriteAllText(absolutePath, "started\n");

            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            api.RegisterCallbacks(new ResultsWriter(absolutePath));
            api.Execute(new ExecutionSettings(new Filter { testMode = TestMode.EditMode }));
            Debug.Log($"EditMode tests started. Results -> {absolutePath}");
        }

        private sealed class ResultsWriter : ICallbacks
        {
            private readonly string _path;

            public ResultsWriter(string path)
            {
                _path = path;
            }

            public void RunStarted(ITestAdaptor testsToRun)
            {
                File.AppendAllText(_path, "run_started\n");
            }

            public void TestStarted(ITestAdaptor test)
            {
            }

            public void TestFinished(ITestResultAdaptor result)
            {
                if (result.HasChildren)
                {
                    return;
                }

                File.AppendAllText(_path, $"{result.TestStatus}\t{result.Test.FullName}\t{result.Message}\n");
            }

            public void RunFinished(ITestResultAdaptor result)
            {
                File.AppendAllText(
                    _path,
                    $"FINISHED\t{result.TestStatus}\tpass={result.PassCount}\tfail={result.FailCount}\tskip={result.SkipCount}\n");
                Debug.Log(
                    $"EditMode tests finished: {result.TestStatus} pass={result.PassCount} fail={result.FailCount}");
            }
        }
    }
}
