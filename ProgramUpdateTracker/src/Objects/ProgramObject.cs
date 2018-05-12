using System;

namespace ProgramUpdateTracker{
    public class ProgramObject {
        public string programName { get; set; }
        public string programVersion { get; set; }
        public string availableVersion { get; set; }
        public string publisher { get; set; }
        public string installDate { get; set; }
        public string updateURL { get; set; }

        public ProgramObject(string name, string version, string pub, string avail, string time) {
            programName = name;
            programVersion = version;
            publisher = pub;
            availableVersion = avail;
            installDate = formatDate(time);
        }

        private string formatDate(string time) {
            string newTime = time;
            try {
                newTime = DateTime.Parse(time).ToLongDateString();
            } catch {
                //could not format the time
                return time;
            } 
            return newTime;
        }
    }
}