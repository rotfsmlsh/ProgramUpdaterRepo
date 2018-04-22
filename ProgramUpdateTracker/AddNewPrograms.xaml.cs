using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Reflection;
using ProgramUpdateTracker.src;
using System.ComponentModel;

namespace ProgramUpdateTracker {
    public partial class AddNewPrograms : Window {
        Dictionary<string, ProgramObject> allPrograms;
        BackgroundWorker bw;
        Dictionary<string, ProgramObject> list;
        ProgressWindow pw;

        public AddNewPrograms() {
            InitializeComponent();

            bw = new BackgroundWorker();
            list = new Dictionary<string, ProgramObject>();
            bw.DoWork += new DoWorkEventHandler(bw_doWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_runWorkerCompleted);
        }

        public List<ProgramObject> programs {
            get;
            private set;
        }

        public Dictionary<string, ProgramObject> getAllPrograms() {
            allPrograms = new Dictionary<string, ProgramObject>();

            string uninstallScript = "Get-ItemProperty HKLM:\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\* |  Select-Object DisplayName, DisplayVersion, Publisher, InstallDate";
            string[] uninstallProp = new string[] { "DisplayName", "DisplayVersion", "Publisher", "InstallDate" };
            runPSScript(uninstallScript, uninstallProp);

            string wmiObjectScript = "Get-WmiObject -Class Win32_Product | Select-Object Name, Version, Vendor, InstallDate";
            string[] wmiObjectProp = new string[] { "Name", "Version", "Vendor", "InstallDate" };
            runPSScript(wmiObjectScript, wmiObjectProp);

            return allPrograms;
        }

        private void runPSScript(string theScript, string[] theProperties) {
            using(PowerShell psInstance = PowerShell.Create()) {
                psInstance.AddScript(theScript);
                string[] properties = theProperties;
                Collection<PSObject> PSOutput = psInstance.Invoke();
                foreach(PSObject pso in PSOutput) {
                    if(pso != null) {
                        ProgramObject po = createProgramObject(pso, properties);
                        if(po != null) {
                            if(!allPrograms.ContainsKey(po.getProgramName())) {
                                allPrograms.Add(po.getProgramName(), po);
                            }
                        }
                    }
                }
            }
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e) {
            //open file chooser and set textbox text with chosen path if it is an executable 
        }

        private void rad_addFromList_Checked(object sender, RoutedEventArgs e) {
            pw = new ProgressWindow();
            pw.Show();
            bw.RunWorkerAsync();
        }

        private ProgramObject createProgramObject(PSObject psobject, string[] props) {
            string name = psobject.Properties[props[0]].ToString();
            name = name.Substring(name.LastIndexOf('=') + 1).Trim();

            if(name.Equals("null", StringComparison.InvariantCultureIgnoreCase)) {
                return null;
            }

            string version = psobject.Properties[props[1]].ToString();
            version = version.Substring(version.LastIndexOf('=') + 1).Trim();

            string publisher = psobject.Properties[props[2]].ToString();
            publisher = publisher.Substring(publisher.LastIndexOf('=') + 1).Trim();

            string installDate = psobject.Properties[props[3]].ToString();
            installDate = installDate.Substring(installDate.LastIndexOf('=') + 1).Trim();

            return new ProgramObject(name, version, publisher, "", installDate);
        }

        private void btn_addPrograms_Click(object sender, RoutedEventArgs e) {
            programs = new List<ProgramObject>();
            //if from list
            foreach(string selectedItem in lstBox_AllPrograms.SelectedItems) {
                //add to returnable
                if(allPrograms.TryGetValue(selectedItem, out ProgramObject tempObject)) {
                    programs.Add(tempObject);
                }
            }

            //if from path
            this.Hide();
        }



        private void bw_doWork(object sender, DoWorkEventArgs e) {
            list = getAllPrograms();
        }

        private void bg_runWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            foreach(KeyValuePair<string, ProgramObject> po in list) {
                lstBox_AllPrograms.Items.Add(po.Key);
            }
            pw.Close();
        }
    }
}
