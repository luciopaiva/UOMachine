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
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using UOMachine.Utility;

namespace UOMachine
{
    internal static class ScriptCompiler
    {
        internal delegate void dScriptFinished();
        /// <summary>
        /// Event fires when script is finished running.
        /// </summary>
        internal static event dScriptFinished ScriptFinishedEvent;
        private static Thread myThread, myWaitThread;
        private static IScriptInterface myScriptInterface;
        private static Assembly myScriptAssembly;
        private static int myScriptTimeout = 10;

        public static void Initialize(int stopScriptTimeout)
        {
            myScriptTimeout = stopScriptTimeout;
        }

        private static void FilterSource(string sourcecode)
        {
            //TODO filter out unsafe code
        }

        private static void PreserveStackTrace(Exception exception)
        {
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
            preserveStackTrace.Invoke(exception, null);
        }

        private static void OnScriptFinished()
        {
            Events.General.ClearEvents();
            Events.IncomingPackets.ClearEvents();
            Events.LowLevel.ClearEvents();
            Events.OutgoingPackets.ClearEvents();

            dScriptFinished handler = ScriptFinishedEvent;
            if (handler != null) handler();
        }

        public static void ForceStopScript()
        {
            if (myThread != null && myThread.IsAlive)
            {
                myThread.Abort();
                myThread.Join();
            }
        }

        private static void WaitForStop()
        {
            if (myThread == null) return;
            DateTime endTime = DateTime.Now + TimeSpan.FromSeconds(myScriptTimeout);
            while (DateTime.Now < endTime)
            {
                if (myThread != null && !myThread.IsAlive) return;
                Thread.Sleep(100);
            }
            ForceStopScript();
        }

        /// <summary>
        /// Stop currently running script.
        /// </summary>
        public static void StopScript()
        {
            if (myScriptInterface != null)
            {
                myWaitThread.Start();
                try { myScriptInterface.Stop(); }
                catch (Exception e)
                {
                    if (e.InnerException != null) PreserveStackTrace(e.InnerException);
                    else PreserveStackTrace(e);
                    Log.LogMessage(e);
                }
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
                if (myWaitThread.IsAlive) myWaitThread.Join();
                OnScriptFinished();
            }
            myScriptAssembly = null;
            myScriptInterface = null;
            myThread = null;
            myWaitThread = null;
        }

        private static void RunScript()
        {
            if (myScriptInterface != null)
            {
                try { myScriptInterface.Start(); }
                catch (Exception e)
                {
                    if (e.InnerException != null) PreserveStackTrace(e.InnerException);
                    else PreserveStackTrace(e);
                    Log.LogMessage(e);
                }
            }
            else if (myScriptAssembly != null)
            {
                try { myScriptAssembly.EntryPoint.Invoke(null, null); }
                catch (Exception e)//(TargetInvocationException e)
                {
                    if (e.InnerException != null) PreserveStackTrace(e.InnerException);
                    else PreserveStackTrace(e);
                    Log.LogMessage(e);
                }
            }
            OnScriptFinished();
        }

        private static void Execute(Assembly scriptAssembly)
        {
            if (scriptAssembly == null) return;

            foreach (Type t in scriptAssembly.GetTypes())
            {
                foreach (Type i in t.GetInterfaces())
                {
                    if (i == typeof(IScriptInterface))
                    {
                        myScriptInterface = (IScriptInterface)scriptAssembly.CreateInstance(t.FullName);
                        myThread.Start();
                        return;
                    }
                }
            }
            myScriptAssembly = scriptAssembly;
            myThread.Start();
            return;
        }

        private static void AddReferences(string sourcecode, CompilerParameters cp)
        {
            cp.ReferencedAssemblies.Add("system.dll");
            string usingPattern = @"using\s+([\w\.]+)[\s;{]";
            string referencePattern = "/\\*\\s*<\\s*[aA][rR][eE][fF]\\s*=\\s*?\"(.+?)\"\\s*>\\s*\\*/";
            string[] assemblies;

            foreach (Match m in Regex.Matches(sourcecode, usingPattern))
            {
                if (NamespaceToAssembly.Resolve(m.Groups[1].Value, out assemblies))
                {
                    foreach (string assembly in assemblies)
                    {
                        if (!cp.ReferencedAssemblies.Contains(assembly.ToLower()))
                            cp.ReferencedAssemblies.Add(assembly.ToLower());
                    }
                }
            }

            foreach (Match m in Regex.Matches(sourcecode, referencePattern))
            {
                if (!cp.ReferencedAssemblies.Contains(m.Groups[1].Value.ToLower()))
                    cp.ReferencedAssemblies.Add(m.Groups[1].Value.ToLower());
            }
        }

        public static bool Compile(string exePath, string sourcecode)
        {
            myScriptInterface = null;
            myScriptAssembly = null;
            myThread = new Thread(new ThreadStart(RunScript));
            myWaitThread = new Thread(new ThreadStart(WaitForStop));
            Environment.CurrentDirectory = exePath.Substring(0, exePath.LastIndexOf(@"\"));
            CodeDomProvider cdp = CodeDomProvider.CreateProvider("C#");
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add(exePath);
            AddReferences(sourcecode, cp);
            cp.GenerateExecutable = false;
            cp.CompilerOptions = "/optimize+";
            cp.GenerateInMemory = true;
            cp.IncludeDebugInformation = false;
            CompilerResults CR = cdp.CompileAssemblyFromSource(cp, sourcecode);
            if (CR.Errors.HasErrors)
            {
                StringBuilder ErrorText = new StringBuilder(128);
                //mainForm.SelectText(CR.Errors[0].Line);
                foreach (CompilerError CE in CR.Errors)
                    ErrorText.Append("Error " + CE.ErrorNumber + " on line " + CE.Line.ToString() + ": " + CE.ErrorText + "\r\n");
                MessageBox.Show(ErrorText.ToString(), "Compiler error!");
                return false;
            }
            Execute(CR.CompiledAssembly);
            return true;
        }
    }
}