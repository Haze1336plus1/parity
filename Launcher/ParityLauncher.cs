using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Awesomium;
using Awesomium.Core;
using System.Runtime.InteropServices;

namespace Launcher
{

    public partial class ParityLauncher : Form
    {

        public class Data
        {

            public static readonly string UpdatePath;
            public static readonly int Version;
            public static readonly JSValue News;

            public static bool IsUpdating;
            public static bool IsApplyingUpdate;

            static Data()
            {
                UpdatePath = "http://itemman.local.turtledev.net/update/";
                string NewsAddress = "http://itemman.local.turtledev.net/parity.php";
                System.Xml.XmlDocument newsList = new System.Xml.XmlDocument();
                try
                {

                    newsList.Load(NewsAddress);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not contact Parity Server - " + ex.Message, "Parity Launcher", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                }

                Version = int.Parse(newsList["parity"]["game"]["version"].InnerText);
                News = new JSValue(
                    (from System.Xml.XmlElement newsEntry in newsList["parity"]["news"] select new JSValue(newsEntry["type"].InnerText, newsEntry["title"].InnerText)).ToArray()
                );

            }

        }

        #region DLLImports

        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr ptr);

        [DllImport("credui.dll", CharSet = CharSet.Unicode)]
        private static extern uint CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere, int authError, ref uint authPackage, IntPtr InAuthBuffer,
          uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, ref bool fSave, PromptForWindowsCredentialsFlags flags);

        [DllImport("credui.dll", CharSet = CharSet.Unicode)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, uint cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        #endregion

        #region Structs and Enums
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        private enum PromptForWindowsCredentialsFlags
        {
            /// <summary>
            /// The caller is requesting that the credential provider return the user name and password in plain text.
            /// This value cannot be combined with SECURE_PROMPT.
            /// </summary>
            CREDUIWIN_GENERIC = 0x1,
            /// <summary>
            /// The Save check box is displayed in the dialog box.
            /// </summary>
            CREDUIWIN_CHECKBOX = 0x2,
            /// <summary>
            /// Only credential providers that support the authentication package specified by the authPackage parameter should be enumerated.
            /// This value cannot be combined with CREDUIWIN_IN_CRED_ONLY.
            /// </summary>
            CREDUIWIN_AUTHPACKAGE_ONLY = 0x10,
            /// <summary>
            /// Only the credentials specified by the InAuthBuffer parameter for the authentication package specified by the authPackage parameter should be enumerated.
            /// If this flag is set, and the InAuthBuffer parameter is NULL, the function fails.
            /// This value cannot be combined with CREDUIWIN_AUTHPACKAGE_ONLY.
            /// </summary>
            CREDUIWIN_IN_CRED_ONLY = 0x20,
            /// <summary>
            /// Credential providers should enumerate only administrators. This value is intended for User Account Control (UAC) purposes only. We recommend that external callers not set this flag.
            /// </summary>
            CREDUIWIN_ENUMERATE_ADMINS = 0x100,
            /// <summary>
            /// Only the incoming credentials for the authentication package specified by the authPackage parameter should be enumerated.
            /// </summary>
            CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200,
            /// <summary>
            /// The credential dialog box should be displayed on the secure desktop. This value cannot be combined with CREDUIWIN_GENERIC.
            /// Windows Vista: This value is not supported until Windows Vista with SP1.
            /// </summary>
            CREDUIWIN_SECURE_PROMPT = 0x1000,
            /// <summary>
            /// The credential provider should align the credential BLOB pointed to by the refOutAuthBuffer parameter to a 32-bit boundary, even if the provider is running on a 64-bit system.
            /// </summary>
            CREDUIWIN_PACK_32_WOW = 0x10000000
        }
        #endregion

        private System.Net.WebClient webClient;
        private int updateProcess;

        public ParityLauncher()
        {
            InitializeComponent();
            this.Shown += (s, e) =>
            {

                string iniFile = Environment.CurrentDirectory + "\\warrock.ini";

                {
                    string language = string.Empty;
                    StringBuilder tempValue = new StringBuilder(255);
                    GetPrivateProfileString("Parity", "Language", language, tempValue, tempValue.Capacity, iniFile);
                    language = tempValue.ToString();
                    if (language == string.Empty)
                        WritePrivateProfileString("Parity", "Language", "ENG", iniFile);
                }

                string username = string.Empty;
                string password = string.Empty;
                {
                    StringBuilder tempValue = new StringBuilder(255);
                    GetPrivateProfileString("Parity", "Username", username, tempValue, tempValue.Capacity, iniFile);
                    username = tempValue.ToString();
                }
                {
                    StringBuilder tempValue = new StringBuilder(255);
                    GetPrivateProfileString("Parity", "Password", password, tempValue, tempValue.Capacity, iniFile);
                    password = tempValue.ToString();
                }
                if (username == string.Empty ||
                    password == string.Empty)
                {

                    bool doRepeat = false;

                    do
                    {

                        string[] result = AuthPrompt.ShowDialog(this, "Enter your Parity Network login details", "Login to Parity Network");

                        username = result[0];
                        password = result[1];

                        if (username.Length == 0 || password.Length == 0)
                        {
                            Application.Exit();
                            return;
                        }

                        doRepeat = username.Length < 6 || password.Length < 6;
                        if (doRepeat)
                            MessageBox.Show(this, "Username and Password must be longer than 6 characters", "Parity Launcher", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    while (doRepeat);

                    WritePrivateProfileString("Parity", "Username", username, iniFile);
                    WritePrivateProfileString("Parity", "Password", password, iniFile);

                }
            };
        }

        private void ParityLauncher_Load(object sender, EventArgs e)
        {

            this.SuspendLayout();

            // 
            // webControl1
            // 
            this.webControl1 = new Awesomium.Windows.Forms.WebControl(this.components);
            this.webControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webControl1.Location = new System.Drawing.Point(0, 0);
            this.webControl1.Size = new System.Drawing.Size(600, 480);
            this.webControl1.TabIndex = 0;
            this.Controls.Add(this.webControl1);

            this.ResumeLayout();

            string iniFile = Environment.CurrentDirectory + "\\warrock.ini";

            webClient = new System.Net.WebClient();
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;

            using (JSObject jsobject = this.webControl1.CreateGlobalJavascriptObject("parity"))
            {
                jsobject.Bind("startWarRock", true, (jsS, jsE) =>
                {
                    string language = "ENG";
                    {
                        StringBuilder tempValue = new StringBuilder(255);
                        GetPrivateProfileString("Parity", "Language", language, tempValue, tempValue.Capacity, iniFile);
                        language = tempValue.ToString();
                    }
                    if (language == "ENG" ||
                        language == "GER")
                    {
                        string warrockExecutable = Environment.CurrentDirectory + "\\system\\WarRock.exe";
                        if (System.IO.File.Exists(warrockExecutable))
                        {
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                            startInfo.FileName = warrockExecutable;
                            startInfo.WorkingDirectory = Environment.CurrentDirectory + "\\system";
                            startInfo.Arguments = "0 0 " + language;
                            System.Diagnostics.Process.Start(startInfo);
                            Application.Exit();
                            jsE.Result = true;
                        }
                        else
                            MessageBox.Show(this, "Could not locate WarRock.exe. Ensure its located at \\system\\WarRock.exe (case sensitive?)", "Parity Launcher", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                        MessageBox.Show(this, "Invalid Language '" + language + "'. Use ENG or GER instead!", "Parity Launcher", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    jsE.Result = false;
                });
                jsobject.Bind("pollUpdate", true, (jsS, jsE) =>
                {
                    string strCurrentVersion = "0";
                    {
                        StringBuilder tempValue = new StringBuilder(255);
                        GetPrivateProfileString("Parity", "Version", strCurrentVersion, tempValue, tempValue.Capacity, iniFile);
                        strCurrentVersion = tempValue.ToString();
                    }
                    int currentVersion = int.Parse(strCurrentVersion);
                    if (Data.IsApplyingUpdate)
                    {
                        jsE.Result = new JSValue("update #" + currentVersion.ToString() + " installing...", 100, false);
                    }
                    else
                    {
                        if (currentVersion < Data.Version)
                        {
                            if (Data.IsUpdating)
                            {
                                if (updateProcess >= 100)
                                {
                                    jsE.Result = new JSValue("update to #" + (currentVersion + 1).ToString() + " finished", 100, false);
                                    var unpackUpdate = new System.Threading.Thread(() =>
                                    {
                                        ICSharpCode.SharpZipLib.Zip.FastZip mate = new ICSharpCode.SharpZipLib.Zip.FastZip();
                                        mate.ExtractZip("parity\\up" + (currentVersion + 1).ToString() + ".dat", Environment.CurrentDirectory, null);
                                        currentVersion += 1;
                                        WritePrivateProfileString("Parity", "Version", currentVersion.ToString(), iniFile);
                                        Data.IsApplyingUpdate = false;
                                    });
                                    Data.IsUpdating = false;
                                    Data.IsApplyingUpdate = true;
                                    unpackUpdate.Start();
                                }
                                else
                                    jsE.Result = new JSValue("updating to #" + (currentVersion + 1).ToString() + " - " + updateProcess.ToString() + "%", updateProcess, false);
                            }
                            else
                            {
                                webClient.DownloadFileAsync(new Uri(Data.UpdatePath + (currentVersion + 1).ToString()), Environment.CurrentDirectory + "\\parity\\up" + (currentVersion + 1).ToString() + ".dat");
                                jsE.Result = new JSValue("update to #" + (currentVersion + 1).ToString() + " starting", 0, false);
                                Data.IsUpdating = true;
                            }
                        }
                        else
                            jsE.Result = new JSValue("ready to play", 100, true);
                        // (string) message, (int) percentage, (bool) canStart
                    }
                });
                jsobject.Bind("pollNews", true, (jsS, jsE) =>
                {
                    jsE.Result = Data.News;
                });
            }

            this.webControl1.ShowJavascriptDialog += (jsdS, jsdE) =>
            {
                if (jsdE.DialogFlags.HasFlag(Awesomium.Core.JSDialogFlags.HasPromptField) ||
                    jsdE.DialogFlags.HasFlag(Awesomium.Core.JSDialogFlags.HasCancelButton))
                {
                    MessageBox.Show(this, jsdE.Message, "JS Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    jsdE.Handled = true;
                }
            };

            this.webControl1.Source = new Uri(String.Format("file:///{0}/launcher/index.html", Environment.CurrentDirectory.Replace("\\", "/")));

        }

        void webClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            updateProcess = e.ProgressPercentage;
        }
    }
}
