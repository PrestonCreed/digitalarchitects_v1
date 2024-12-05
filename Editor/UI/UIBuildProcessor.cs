using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace DigitalArchitects.Editor
{
    public class UIBuildProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Rebuild UI when UI assets change
            foreach (string str in importedAssets)
            {
                if (str.Contains("UI/src"))
                {
                    BuildUI();
                    break;
                }
            }
        }

        static void BuildUI()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = Application.dataPath + "/../Packages/digitalarchitects_v1/Editor/UI";
            startInfo.FileName = "npm";
            startInfo.Arguments = "run build";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            var process = Process.Start(startInfo);
            process.WaitForExit();
            AssetDatabase.Refresh();
        }
    }
}