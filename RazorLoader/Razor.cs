/* Copyright (C) 2009 Matthew Geyer
 * 
 * This file is part of UO Machine.
 * 
 * UO Machine is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * UO Machine is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with UO Machine.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Microsoft.Win32;

namespace RazorLoader
{
    internal static class Razor
    {
        /*public enum Loader_Error
        {
            NO_ALLOC_MEM = 5,
            NO_MAP_EXE = 2,
            NO_OPEN_EXE = 1,
            NO_READ = 8,
            NO_READ_EXE_DATA = 3,
            NO_RUN_EXE = 4,
            NO_VPROTECT = 7,
            NO_WRITE = 6,
            SUCCESS = 0,
            UNKNOWN_ERROR = 0x63
        }*/

        private static Assembly myRazorAssembly;

        private static string GetRazorVersion()
        {
            Version version = myRazorAssembly.GetName().Version;
            return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        private static void InitializeRazor()
        {
            foreach (Type t in myRazorAssembly.GetTypes())
            {
                MethodInfo method = t.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static);
                if (method != null) method.Invoke(null, null);
            }
        }

        private static string GetRegString(RegistryKey hkey, string vname)
        {
            try
            {
                RegistryKey key = hkey.OpenSubKey(@"SOFTWARE\Razor");
                if (key == null)
                {
                    key = hkey.CreateSubKey(@"SOFTWARE\Razor");
                    if (key == null)
                    {
                        return null;
                    }
                }
                string str = key.GetValue(vname) as string;
                if (str == null)
                {
                    return null;
                }
                return str.Trim();
            }
            catch
            {
                return null;
            }
        }

        public static IPAddress Resolve(string address)
        {
            IPAddress ip = IPAddress.None;
            if (!string.IsNullOrEmpty(address))
            {
                if (!IPAddress.TryParse(address, out ip))
                {
                    try
                    {
                        IPHostEntry entry = Dns.GetHostEntry(address);
                        if (entry.AddressList.Length > 0)
                        {
                            ip = entry.AddressList[entry.AddressList.Length - 1];
                        }
                    }
                    catch { }
                }
            }
            return ip;
        }

        public static void Launch(string razorFolder, bool patchClientEncryption, bool serverEncrypted, string server, int port, int pid)
        {
            Environment.CurrentDirectory = razorFolder;
            string razorPath = Path.Combine(razorFolder, "razor.exe");
            string cryptPath = Path.Combine(razorFolder, "crypt.dll");
            if (!File.Exists(razorPath) || !File.Exists(cryptPath))
                return;
            myRazorAssembly = Assembly.LoadFile(razorPath);
            IntPtr hModule = Win32.LoadLibrary(cryptPath);
            Win32.GainMemoryAccess(hModule, 0x16000);
            if (!CryptPatcher.Patch(hModule))
            {
                MessageBox.Show("Error patching crypt.dll, unknown Razor version.");
                return;
            }
            Thread.CurrentThread.Name = "Razor Main Thread";
            Type engineType = myRazorAssembly.GetType("Assistant.Engine");
            Type clientCommunicationType = myRazorAssembly.GetType("Assistant.ClientCommunication");
            Type configType = myRazorAssembly.GetType("Assistant.Config");
            Type languageType = myRazorAssembly.GetType("Assistant.Language");
            //Type welcomeFormType = myRazorAssembly.GetType("Assistant.WelcomeForm");
            Type mainFormType = myRazorAssembly.GetType("Assistant.MainForm");
            Type counterType = myRazorAssembly.GetType("Assistant.Counter");
            Type macroManagerType = myRazorAssembly.GetType("Assistant.Macros.MacroManager");
            Type packetPlayerType = myRazorAssembly.GetType("Assistant.PacketPlayer");
            Type aviRecType = myRazorAssembly.GetType("Assistant.AVIRec");

            object engine = Activator.CreateInstance(engineType);
            object config = Activator.CreateInstance(configType);
            object clientCommunication = Activator.CreateInstance(clientCommunicationType);
            object language = Activator.CreateInstance(languageType);
            //object welcomeForm = Activator.CreateInstance(welcomeFormType);
            object mainForm = Activator.CreateInstance(mainFormType);

            //Loader_Error error = Loader_Error.UNKNOWN_ERROR;

            FieldInfo ActiveWnd = engineType.GetField("m_ActiveWnd", BindingFlags.Static | BindingFlags.NonPublic);
//            FieldInfo m_BaseDir = engineType.GetField("m_BaseDir", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo m_MainWnd = engineType.GetField("m_MainWnd", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo m_Running = engineType.GetField("m_Running", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo m_ClientEnc = clientCommunicationType.GetField("m_ClientEnc", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo m_ServerEnc = clientCommunicationType.GetField("m_ServerEnc", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo ClientProc = clientCommunicationType.GetField("ClientProc", BindingFlags.Static | BindingFlags.NonPublic);
            /*FieldInfo WFPatchEncryption = welcomeFormType.GetField("m_PatchEncy", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo WFClientLaunch = welcomeFormType.GetField("m_Launch", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo WFDataDirectory = welcomeFormType.GetField("m_DataDir", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo WFClientPath = welcomeFormType.GetField("m_ClientPath", BindingFlags.NonPublic | BindingFlags.Instance);*/

            MethodInfo miInitialize = engineType.GetMethod("Initialize", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo miResolve = engineType.GetMethod("Resolve", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo miSetConnectionInfo = clientCommunicationType.GetMethod("SetConnectionInfo", BindingFlags.Static | BindingFlags.NonPublic);
            MethodInfo miInitializeLibrary = clientCommunicationType.GetMethod("InitializeLibrary");
            MethodInfo miLoadCliloc = languageType.GetMethod("LoadCliLoc", BindingFlags.Static | BindingFlags.Public);
            MethodInfo miLoadCharList = configType.GetMethod("LoadCharList", BindingFlags.Static | BindingFlags.Public);
            //MethodInfo miAttach = clientCommunicationType.GetMethod("Attach");
            MethodInfo miLanguageLoad = languageType.GetMethod("Load");
            //MethodInfo miShowDialog = welcomeFormType.GetMethod("ShowDialog", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, Type.EmptyTypes, null);
            //MethodInfo miLaunchClient = clientCommunicationType.GetMethod("LaunchClient");
            //MethodInfo miGetRegString = configType.GetMethod("GetRegString");
            MethodInfo miLoadLastProfile = configType.GetMethod("LoadLastProfile");
            MethodInfo miClientCommunicationClose = clientCommunicationType.GetMethod("Close");
            MethodInfo miCounterSave = counterType.GetMethod("Save", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, Type.EmptyTypes, null);
            MethodInfo miMacroManagerSave = macroManagerType.GetMethod("Save");
            MethodInfo miConfigSave = configType.GetMethod("Save");
            MethodInfo miAviRecStop = aviRecType.GetMethod("Stop");
            MethodInfo miPacketPlayerStop = packetPlayerType.GetMethod("Stop");

            m_Running.SetValue(engine, true);

            m_ClientEnc.SetValue(clientCommunication, patchClientEncryption);
            m_ServerEnc.SetValue(clientCommunication, serverEncrypted);
//            m_BaseDir.SetValue(engine, razorFolder);
            miInitializeLibrary.Invoke(clientCommunication, new object[] { GetRazorVersion() });

            if (!(bool)miLanguageLoad.Invoke(null, new object[] { "ENU" }))
            {
                MessageBox.Show("Fatal Error: Unable to load required file Language/Razor_lang.enu\nRazor cannot continue.", "No Language Pack", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            string lang = GetRegString(Registry.CurrentUser, "DefaultLanguage");
            if (lang != null && !(bool)miLanguageLoad.Invoke(null, new object[] { lang }))
            {
                MessageBox.Show(string.Format("WARNING: Razor was unable to load the file Language/Razor_lang.{0}\nENU will be used instead.", lang), "Language Load Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            miLoadCliloc.Invoke(language, null);
            InitializeRazor();
            miLoadCharList.Invoke(config, null);
            if (!(bool)miLoadLastProfile.Invoke(config, null))
            {
                MessageBox.Show("Last used profile could not be loaded, using default instead.", "Profile Load Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            /*error = (Loader_Error)miLaunchClient.Invoke(clientCommunication, new object[] { razorFolder });
            if (error != Loader_Error.SUCCESS)
            {
                MessageBox.Show("Razor encountered error while launching client: " + error.ToString(), "Loader error");
                return;
            }*/

            Process clientProcess = Process.GetProcessById(pid);
            ClientProc.SetValue(clientCommunication, clientProcess);
            IPAddress addr = Resolve(server);
            miSetConnectionInfo.Invoke(clientCommunication, new object[] { addr, port });
            m_MainWnd.SetValue(engine, mainForm);
            Application.Run((Form)mainForm);
            m_Running.SetValue(engine, false);
            try { miPacketPlayerStop.Invoke(null, null); }
            catch { }
            try { miAviRecStop.Invoke(null, null); }
            catch { }
            miClientCommunicationClose.Invoke(null, null);
            miCounterSave.Invoke(null, null);
            miMacroManagerSave.Invoke(null, null);
            miConfigSave.Invoke(null, null);

        }
    }
}