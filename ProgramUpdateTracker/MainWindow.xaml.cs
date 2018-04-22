using ProgramUpdateTracker.src;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ProgramUpdateTracker {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ProgramList pl;
        Dictionary<string, ProgramObject> listedPrograms;

        public MainWindow() {
            this.InitializeComponent();
            pl = new ProgramList();
            refreshProgramList(pl);
        }

        private void refreshProgramList(ProgramList list) {
            listedPrograms = list.getTrackedProgramObjects();

            foreach(KeyValuePair<string, ProgramObject> pair in listedPrograms) {
                if(!lstBox_trackedPrograms.Items.Contains(pair.Key)) {
                    lstBox_trackedPrograms.Items.Add(pair.Value.getProgramName());
                }
            }
        }

        private void updateTextFields(ProgramObject po) {
            txt_programName.Text = po.getProgramName();
            txt_programCurrentVersion.Text = po.getProgramVersion();
            txt_programInstallDate.Text = po.getInstallDate();
            txt_programPublisher.Text = po.getPublisher();
        }

        private void lstBox_trackedPrograms_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox box = (ListBox) sender;
            updateTextFields(listedPrograms[box.SelectedItem.ToString()]);
        }

        private void btn_addPrograms_Click(object sender, RoutedEventArgs e) {
            AddNewPrograms anp = new AddNewPrograms();
            anp.ShowDialog();
            addProgramsToTrackedList(anp.programs);
            anp.Close();
        }

        public void addProgramsToTrackedList(List<ProgramObject> programObjects) {
            pl.addItemsToTracked(programObjects);
            refreshProgramList(pl);
        }
    }
}
