using System;
using System.Collections.Generic;

namespace ProgramUpdateTracker.src {
    public class ProgramList {
        
        private Dictionary<string, ProgramObject> trackedPrograms;

        public ProgramList() {
            //get the list of programs
            trackedPrograms = new Dictionary<string, ProgramObject>();
            trackedPrograms = getTrackedProgramObjects();
            //check for updates
        }

        public Dictionary<string, ProgramObject> getTrackedProgramObjects() {
            return trackedPrograms;

            //update this to grab list of programs from xml
        }

        public void addItemsToTracked(List<ProgramObject> allPrograms) {
            if(allPrograms != null) {
                foreach(ProgramObject po in allPrograms) {
                    if(!trackedPrograms.ContainsKey(po.getProgramName())) {
                        //doesnt have it, add it
                        trackedPrograms.Add(po.getProgramName(), po);
                    }
                }
            }
        }
    }
}