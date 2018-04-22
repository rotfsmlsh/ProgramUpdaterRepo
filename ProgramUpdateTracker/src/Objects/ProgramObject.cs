using System;

namespace ProgramUpdateTracker{
    public class ProgramObject {
        private string programName;
        private string programVersion;
        private string availableVersion;
        private string publisher;
        private string installDate;
        private string updateURL;

        public ProgramObject(string name, string version, string pub, string avail, string time) {
            programName = name;
            programVersion = version;
            publisher = pub;
            availableVersion = avail;
            installDate = time;
        }

        public string getProgramName() {
            return programName;
        }

        public string getProgramVersion() {
            return programVersion;
        }

        public string getAvailableVersion() {
            return availableVersion;
        }

        public string getPublisher() {
            return publisher;
        }

        public string getInstallDate() {
            return installDate;
        }
    }
}