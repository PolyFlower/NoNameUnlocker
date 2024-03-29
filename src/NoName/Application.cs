﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

internal class Application
{
    private static void Main()
    {
        Console.Title = Crypto.RandomString;

        Class6 class6 = new Class6();

        if (CheckLicenseFile() == true)
        {
            licenseKey = File.ReadAllText(licenseKeyFileName);

            foreach (string processName in class6.windowNames)
            {
                class6.processList.AddRange(Process.GetProcessesByName(processName));
            }

            class6.maxProcessWaitTimestamp = DateTime.Now.AddSeconds(20.0);
            class6.server = new Server(new Action(ConnectionLossRoutine));

            NetworkStreamWriter networkStreamWriter = new NetworkStreamWriter(class6.server);
            if (networkStreamWriter.SendKeyMessage())
            {
                networkStreamWriter.Action_3 = new Action<object>(class6.method_0);
                class6.hasResumed = true;
                networkStreamWriter.SendAuthMessage(licenseKey, Crypto.MachineGUID);
                Thread.Sleep(-1);
                return;
            }
            Logger.Error("Failed to connect!");
            Console.ReadLine();
        }
        else
        {
            Thread.Sleep(2000);
            Environment.Exit(Environment.ExitCode);
        }
    }

    private static bool CheckLicenseFile()
    {
        if (!IsLicenseFileValid())
        {
            Logger.Error("Missing or empty license file");
            return false;
        }
        return true;
    }

    private static bool IsLicenseFileValid()
    {
        return File.Exists(licenseKeyFileName) &&
               File.ReadAllText(licenseKeyFileName) != string.Empty;
    }

    private static void KillProcesses()
    {
        foreach (int processID in pidList)
        {
                Process process = Process.GetProcessById(processID);
                if (!process.HasExited)
                {
                    Logger.Info("Force killing process: PID " + processID);
                    process.Kill();
                }
        }
    }

    private static void ConnectionLossRoutine()
    {
        Logger.Error("Connection to the server was lost!");
        KillProcesses();
        Process.GetCurrentProcess().Kill();
    }

    public const ushort ushort_0 = 105;

    private static List<int> pidList = new List<int>();

    private static string licenseKey = "";

    private static readonly string licenseKeyFileName = "license.txt";

    [CompilerGenerated]
    private sealed class Class6
    {
        internal void method_0(object ushort_0)
        {
            UnknownStruct_0 unknownStruct_0;
            unknownStruct_0.ushort_0 = (ushort)ushort_0;
            if (unknownStruct_0.ushort_0 == 0)
            {
                Thread.Sleep(2000);
                return;
            }
            Logger.Info("Waiting for World of Warcraft...");
            bool flag = true;
            while (flag)
            {
                List<Process> list = new List<Process>();
                foreach (string text in windowNames)
                {
                    list.AddRange(Process.GetProcessesByName(text));
                }
                using (List<Process>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ProcessIdComparer processComparer = new ProcessIdComparer();
                        processComparer.targetProcess = enumerator.Current;
                        if (processList.FindIndex(new Predicate<Process>(processComparer.IsSameProcess)) == -1)
                        {
                            processList.Add(processComparer.targetProcess);
                            if (!pidList.Contains(processComparer.targetProcess.Id) && (processComparer.targetProcess.HandleCount <= 0 || !processComparer.targetProcess.HasExited))
                            {
                                pidList.Add(processComparer.targetProcess.Id);
                                if (wowProcess == null)
                                {
                                    wowProcess = processComparer.targetProcess;
                                }
                                method_1(processComparer.targetProcess, ref unknownStruct_0);
                                flag = false;
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
                if (maxProcessWaitTimestamp < DateTime.Now || wowProcess != null)
                {
                    Thread.Sleep(12500);
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        internal void method_1(Process processInfo, ref UnknownStruct_0 unkownStruct_0)
        {
            InjectionProcess class7 = new InjectionProcess
            {
                class6 = this,
                process = processInfo
            };
            try
            {
                // Check for authFlag == 2, bad one is 0
                class7.bool_0 = unkownStruct_0.ushort_0 == 2;
                class7.wardenController = new WardenController(new ProcessMemoryHandler(class7.process), server);
                if (class7.bool_0)
                {
                    class7.wardenController.SuspendProcess();
                }
                class7.wardenController.method_0(new Action(class7.Inject));
            }
            catch (Exception)
            {
                Logger.Error("Failed to inject " + class7.process.ProcessName + " " + class7.process.Id.ToString("X"));
            }
        }

        public Server server;

        public bool IsInjected = false;

        public bool hasResumed;

        public readonly string[] windowNames = { "Wow", "WowT", "WowClassic" };

        public List<Process> processList = new List<Process>();

        public Process wowProcess = null;

        public DateTime maxProcessWaitTimestamp;
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct UnknownStruct_0
    {
        public ushort ushort_0;
    }

    [CompilerGenerated]
    private sealed class InjectionProcess
    {
        internal void Inject()
        {
            if (!bool_0)
            {
                Thread.Sleep(5000);
                wardenController.method_4();
                class6.IsInjected = true;
                Logger.Info($"Injected into {process.ProcessName} {process.Id.ToString("X")}");
                wardenController.StartWardenScanningThread();
                return;
            }
            wardenController.method_1("Punani", new Action(InjectWithResume));
        }

        internal void InjectWithResume()
        {
            wardenController.method_5();
            class6.IsInjected = true;
            class6.hasResumed = true;
            Logger.Info($"Injected into {process.ProcessName} {process.Id.ToString("X")}");
            wardenController.StartWardenScanningThread();
            if (bool_0)
            {
                wardenController.ResumeProcess();
            }
        }

        public Process process { get; set; }

        public bool bool_0 { get; set; }

        public WardenController wardenController { get; set; }

        public Class6 class6 { get; set; }
    }

    [CompilerGenerated]
    private sealed class ProcessIdComparer
    {
        internal bool IsSameProcess(Process process)
        {
            return process.Id == targetProcess.Id;
        }
        public Process targetProcess;
    }
}
