using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GamesSoft.EditorTools
{
    public static class LogCleaner
    {
        [MenuItem("GamesSoft/Clear Logs", false, 100)]
        public static void ClearLogs()
        {
            var logsDirectory = Path.GetFullPath("Logs");
            var deletedFiles = 0;
            var skippedFiles = 0;
            long freedBytes = 0;

            if (Directory.Exists(logsDirectory))
            {
                foreach (var path in Directory.GetFiles(logsDirectory, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var info = new FileInfo(path);
                        var size = info.Exists ? info.Length : 0L;
                        info.IsReadOnly = false;
                        File.Delete(path);
                        deletedFiles++;
                        freedBytes += size;
                    }
                    catch (Exception)
                    {
                        skippedFiles++;
                    }
                }
            }

            deletedFiles += DeleteMatchingFiles(Directory.GetCurrentDirectory(), "TestResults*.txt", ref freedBytes, ref skippedFiles);
            deletedFiles += DeleteMatchingFiles(Directory.GetCurrentDirectory(), "TestResults*.xml", ref freedBytes, ref skippedFiles);

            ClearUnityConsole();

            var freedMb = freedBytes / (1024f * 1024f);
            Debug.Log(
                $"GamesSoft logs cleared: deleted {deletedFiles} file(s), " +
                $"skipped {skippedFiles}, freed ~{freedMb:0.00} MB.");
        }

        private static int DeleteMatchingFiles(
            string directory,
            string searchPattern,
            ref long freedBytes,
            ref int skippedFiles)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }

            var deleted = 0;
            foreach (var path in Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var info = new FileInfo(path);
                    var size = info.Exists ? info.Length : 0L;
                    info.IsReadOnly = false;
                    File.Delete(path);
                    deleted++;
                    freedBytes += size;
                }
                catch (Exception)
                {
                    skippedFiles++;
                }
            }

            return deleted;
        }

        private static void ClearUnityConsole()
        {
            var logEntriesType = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntriesType?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod?.Invoke(null, null);
        }
    }
}
