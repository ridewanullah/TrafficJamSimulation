using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TerminalCommandRunner : MonoBehaviour
{
    public string command = "mlagents-learn config/dqn.yaml --run-id=TestRun --force";
    private Process powerShellProcess;

    public void RunCommand(string command)
    {
        ProcessStartInfo start = new ProcessStartInfo();

        // Target PowerShell
        start.FileName = "powershell.exe";

        // Command to execute in PowerShell (note: no -File or -Command)
        start.Arguments = $"-NoExit -Command \"{command}\"";

        // Launch PowerShell visibly
        start.UseShellExecute = true;
        start.CreateNoWindow = false;
        start.WindowStyle = ProcessWindowStyle.Normal;

        try
        {
            Process.Start(start);
            UnityEngine.Debug.Log("PowerShell launched.");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Failed to launch PowerShell: " + ex.Message);
        }
    }

    public void ExitCommand()
    {
        if (powerShellProcess != null && !powerShellProcess.HasExited)
        {
            powerShellProcess.Kill(); // Force exit
            powerShellProcess.Dispose();
            UnityEngine.Debug.Log("PowerShell process terminated.");
        }
    }
}
