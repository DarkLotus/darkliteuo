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
using System.Threading;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DarkLiteUO
{
    public interface IScriptInterface
    {
        void Start(ref UOLite2.LiteClient Client, Form1 GUI);
        void Stop();
    }
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
        private static UOLite2.LiteClient Client;
        private static Form1 GUI;
        public static void Initialize(int stopScriptTimeout, ref UOLite2.LiteClient _client, Form1 _gui)
        {
            Client = _client;
            GUI = _gui;
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
                    
                    //UpdateLog.LogMessage(e);
                }
                //Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
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
                try { myScriptInterface.Start(ref Client, GUI); }
                catch (Exception e)
                {
                    if (e.InnerException != null) PreserveStackTrace(e.InnerException);
                    else PreserveStackTrace(e);
                    //Log.LogMessage(e);
                }
            }
            else if (myScriptAssembly != null)
            {
                try { myScriptAssembly.EntryPoint.Invoke(null, null); }
                catch (Exception e)//(TargetInvocationException e)
                {
                    if (e.InnerException != null) PreserveStackTrace(e.InnerException);
                    else PreserveStackTrace(e);
                    //Log.LogMessage(e);
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

            myThread = new Thread(new System.Threading.ThreadStart(RunScript));
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

    internal static class NamespaceToAssembly
    {
        private static Dictionary<string, string[]> myHashTable;

        public static void Initialize()
        {
            myHashTable = new Dictionary<string, string[]>();
            FillTable();
        }

        public static bool Resolve(string namespaceString, out string[] assemblies)
        {
            return myHashTable.TryGetValue(namespaceString, out assemblies);
        }

        private static void FillTable()
        {
            myHashTable.Add("Accessibility", new string[] { "Accessibility.dll" });
            myHashTable.Add("EnvDTE", new string[] { "envdte.dll" });
            myHashTable.Add("IEHost.Execute", new string[] { "IEExecRemote.dll" });
            myHashTable.Add("Microsoft.CLRAdmin", new string[] { "mscorcfg.dll" });
            myHashTable.Add("Microsoft.CSharp", new string[] { "cscompmgd.dll" });
            myHashTable.Add("Microsoft.IE", new string[] { "IEHost.dll", "IIEHost.dll" });
            myHashTable.Add("Microsoft.JScript", new string[] { "Microsoft.JScript.dll" });
            myHashTable.Add("Microsoft.JScript.Vsa", new string[] { "Microsoft.JScript.dll" });
            myHashTable.Add("Microsoft.Office.Core", new string[] { "office.dll" });
            myHashTable.Add("Microsoft.VisualBasic", new string[] { "Microsoft.VisualBasic.dll" });
            myHashTable.Add("Microsoft.VisualBasic.Compatibility.VB6", new string[] { "Microsoft.VisualBasic.Compatibility.dll", "Microsoft.VisualBasic.Compatibility.Data.dll" });
            myHashTable.Add("Microsoft.VisualBasic.CompilerServices", new string[] { "Microsoft.VisualBasic.dll" });
            myHashTable.Add("Microsoft.VisualBasic.Vsa", new string[] { "Microsoft.VisualBasic.Vsa.dll" });
            myHashTable.Add("Microsoft.VisualC", new string[] { "Microsoft.VisualC.dll", "Microsoft.JScript.dll" });
            myHashTable.Add("Microsoft.Vsa", new string[] { "Microsoft.Vsa.dll" });
            myHashTable.Add("Microsoft.Vsa.Vb.CodeDOM", new string[] { "Microsoft.Vsa.Vb.CodeDOMProcessor.dll" });
            //myHashTable.Add("Microsoft.Win32", new string[] { });
            myHashTable.Add("Microsoft_VsaVb", new string[] { "Microsoft_VsaVb.dll" });
            myHashTable.Add("RegCode", new string[] { "RegCode.dll" });
            //myHashTable.Add("System", new string[] { });
            //myHashTable.Add("System.CodeDom", new string[] { });
            //myHashTable.Add("System.CodeDom.Compiler", new string[] { });
            //myHashTable.Add("System.Collections", new string[] { });
            //myHashTable.Add("System.Collections.Specialized", new string[] { });
            //myHashTable.Add("System.ComponentModel", new string[] { });
            myHashTable.Add("System.ComponentModel.Design", new string[] { "System.Design.dll" });
            myHashTable.Add("System.ComponentModel.Design.Serialization", new string[] { "System.Design.dll" });
            //myHashTable.Add("System.Configuration", new string[] { });
            //myHashTable.Add("System.Configuration.Assemblies", new string[] { });
            myHashTable.Add("System.Configuration.Install", new string[] { "System.Configuration.Install.dll" });
            myHashTable.Add("System.Data", new string[] { "System.Data.dll" });
            myHashTable.Add("System.Data.Common", new string[] { "System.Data.dll" });
            myHashTable.Add("System.Data.OleDb", new string[] { "System.Data.dll" });
            myHashTable.Add("System.Data.SqlClient", new string[] { "System.Data.dll" });
            myHashTable.Add("System.Data.SqlTypes", new string[] { "System.Data.dll" });
            myHashTable.Add("System.Diagnostics", new string[] { "System.Configuration.Install.dll" });
            myHashTable.Add("System.Diagnostics.Design", new string[] { "System.Design.dll" });
            myHashTable.Add("System.Diagnostics.SymbolStore", new string[] { "ISymWrapper.dll" });
            myHashTable.Add("System.DirectoryServices", new string[] { "System.DirectoryServices.dll" });
            myHashTable.Add("System.Drawing", new string[] { "System.Drawing.dll" });
            myHashTable.Add("System.Drawing.Design", new string[] { "System.Drawing.dll", "System.Drawing.Design.dll" });
            myHashTable.Add("System.Drawing.Drawing2D", new string[] { "System.Drawing.dll" });
            myHashTable.Add("System.Drawing.Imaging", new string[] { "System.Drawing.dll" });
            myHashTable.Add("System.Drawing.Printing", new string[] { "System.Drawing.dll" });
            myHashTable.Add("System.Drawing.Text", new string[] { "System.Drawing.dll" });
            myHashTable.Add("System.EnterpriseServices", new string[] { "System.EnterpriseServices.dll" });
            myHashTable.Add("System.EnterpriseServices.CompensatingResourceManagerSystem.EnterpriseServices.dll", new string[] { "System.EnterpriseServices.dll" });
            myHashTable.Add("System.EnterpriseServices.Internal", new string[] { "System.EnterpriseServices.dll" });
            //myHashTable.Add("System.Globalization", new string[] { });
            //myHashTable.Add("System.IO", new string[] { });
            //myHashTable.Add("System.IO.IsolatedStorage", new string[] { });
            myHashTable.Add("System.Management", new string[] { "System.Management.dll" });
            myHashTable.Add("System.Management.Instrumentation", new string[] { "System.Management.dll" });
            myHashTable.Add("System.Messaging", new string[] { "System.Messaging.dll" });
            myHashTable.Add("System.Messaging.Design", new string[] { "System.Messaging.dll", "System.Design.dll" });
            //myHashTable.Add("System.Net", new string[] { });
            //myHashTable.Add("System.Net.Sockets", new string[] { });
            //myHashTable.Add("System.Reflection", new string[] { });
            //myHashTable.Add("System.Reflection.Emit", new string[] { });
            myHashTable.Add("System.Resources", new string[] { "System.Windows.Forms.dll" });
            //myHashTable.Add("System.Runtime.CompilerServices", new string[] { });
            //myHashTable.Add("System.Runtime.InteropServices", new string[] { });
            myHashTable.Add("System.Runtime.InteropServices.CustomMarshalers", new string[] { "CustomMarshalers.dll" });
            //myHashTable.Add("System.Runtime.InteropServices.Expando", new string[] { });
            //myHashTable.Add("System.Runtime.Remoting", new string[] { });
            //myHashTable.Add("System.Runtime.Remoting.Activation", new string[] { });
            myHashTable.Add("System.Runtime.Remoting.Channels", new string[] { "System.Runtime.Remoting.dll" });
            myHashTable.Add("System.Runtime.Remoting.Channels.Http", new string[] { "System.Runtime.Remoting.dll" });
            myHashTable.Add("System.Runtime.Remoting.Channels.Tcp", new string[] { "System.Runtime.Remoting.dll" });
            //myHashTable.Add("System.Runtime.Remoting.Contexts", new string[] { });
            //myHashTable.Add("System.Runtime.Remoting.Lifetime", new string[] { });
            //myHashTable.Add("System.Runtime.Remoting.Messaging", new string[] { });
            //myHashTable.Add("System.Runtime.Remoting.Metadata", new string[] { });
            //myHashTable.Add("System.Runtime.Remoting.Metadata.W3cXsd2001", new string[] { });
            myHashTable.Add("System.Runtime.Remoting.MetadataServices", new string[] { "System.Runtime.Remoting.dll" });
            //myHashTable.Add("System.Runtime.Remoting.Proxies", new string[] { });
            myHashTable.Add("System.Runtime.Remoting.Services", new string[] { "System.Runtime.Remoting.dll" });
            //myHashTable.Add("System.Runtime.Serialization", new string[] { });
            //myHashTable.Add("System.Runtime.Serialization.Formatters", new string[] { });
            //myHashTable.Add("System.Runtime.Serialization.Formatters.Binary", new string[] { });
            myHashTable.Add("System.Runtime.Serialization.Formatters.Soap", new string[] { "System.Runtime.Serialization.Formatters.Soap.dll" });
            //myHashTable.Add("System.Security", new string[] { });
            //myHashTable.Add("System.Security.Cryptography", new string[] { });
            //myHashTable.Add("System.Security.Cryptography.X509Certificates", new string[] { });
            myHashTable.Add("System.Security.Cryptography.Xml", new string[] { "System.Security.dll" });
            //myHashTable.Add("System.Security.Permissions", new string[] { });
            //myHashTable.Add("System.Security.Policy", new string[] { });
            //myHashTable.Add("System.Security.Principal", new string[] { });
            myHashTable.Add("System.ServiceProcess", new string[] { "System.ServiceProcess.dll" });
            myHashTable.Add("System.ServiceProcess.Design", new string[] { "System.ServiceProcess.dll", "System.Design.dll" });
            //myHashTable.Add("System.Text", new string[] { });
            //myHashTable.Add("System.Text.RegularExpressions", new string[] { });
            //myHashTable.Add("System.Threading", new string[] { });
            //myHashTable.Add("System.Timers", new string[] { });
            myHashTable.Add("System.Web", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Caching", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Configuration", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Handlers", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Hosting", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Mail", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.RegularExpressions", new string[] { "System.Web.RegularExpressions.dll" });
            myHashTable.Add("System.Web.Security", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Services", new string[] { "System.Web.Services.dll" });
            myHashTable.Add("System.Web.Services.Configuration", new string[] { "System.Web.Services.dll" });
            myHashTable.Add("System.Web.Services.Description", new string[] { "System.Web.Services.dll" });
            myHashTable.Add("System.Web.Services.Discovery", new string[] { "System.Web.Services.dll" });
            myHashTable.Add("System.Web.Services.Protocols", new string[] { "System.Web.Services.dll" });
            myHashTable.Add("System.Web.SessionState", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.UI", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.UI.Design", new string[] { "System.Design.dll" });
            myHashTable.Add("System.Web.UI.Design.WebControls", new string[] { "System.Design.dll" });
            myHashTable.Add("System.Web.UI.HtmlControls", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.UI.WebControls", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Web.Util", new string[] { "System.Web.dll" });
            myHashTable.Add("System.Windows.Forms", new string[] { "System.Windows.Forms.dll" });
            myHashTable.Add("System.Windows.Forms.ComponentModel.Com2Interop", new string[] { "System.Windows.Forms.dll" });
            myHashTable.Add("System.Windows.Forms.Design", new string[] { "System.Windows.Forms.dll", "System.Design.dll" });
            myHashTable.Add("System.Windows.Forms.PropertyGridInternal", new string[] { "System.Windows.Forms.dll" });
            myHashTable.Add("System.Xml", new string[] { "System.XML.dll", "System.Data.dll" });
            myHashTable.Add("System.Xml.Schema", new string[] { "System.XML.dll" });
            myHashTable.Add("System.Xml.Serialization", new string[] { "System.XML.dll" });
            myHashTable.Add("System.Xml.XPath", new string[] { "System.XML.dll" });
            myHashTable.Add("System.Xml.Xsl", new string[] { "System.XML.dll" });
            myHashTable.Add("System.Xml.Linq", new string[] { "System.XML.dll" });
            myHashTable.Add("UOLite2", new string[] { "UOLite2.dll" });

        }
    }
}