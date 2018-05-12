using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ProgramUpdateTracker.src {
    public class ProgramList {
        
        private Dictionary<string, ProgramObject> trackedPrograms;
        string fileName = "listedprograms.xml";

        public ProgramList() {
            //get the list of programs
            trackedPrograms = new Dictionary<string, ProgramObject>();
            //trackedPrograms = getTrackedProgramObjects();
            //check for updates
        }

        public Dictionary<string, ProgramObject> getTrackedProgramObjects() {
            Boolean hasObjects = false;
            
            ProgramObject po;
            if(File.Exists(fileName)) {
                foreach(XElement element in XElement.Load(fileName).Elements("program")) {
                    hasObjects = true;
                    string name = element.Attribute("name").ToString();
                    string currentVersion = element.Attribute("currVersion").ToString();
                    string availVersion = element.Attribute("availVersion").ToString();
                    string publisher = element.Attribute("publisher").ToString();
                    string installDate = element.Attribute("installDate").ToString();
                    string updateUrl = element.Attribute("updateUrl").ToString();
                    po = new ProgramObject(name, currentVersion, publisher, availVersion, installDate) {
                        updateURL = updateUrl
                    };

                    trackedPrograms.Add(po.programName, po);
                }
            }
            if(hasObjects) {
                return trackedPrograms;
            }
            else {
                return trackedPrograms;
            }
            
        }

        public void addItemsToTracked(List<ProgramObject> allPrograms) {
            if(allPrograms != null) {
                foreach(ProgramObject po in allPrograms) {
                    if(!trackedPrograms.ContainsKey(po.programName)) {
                        //doesnt have it, add it
                        trackedPrograms.Add(po.programName, po);
                    }
                }
            }
        }

        public void createXMLFile() {
            XmlWriterSettings settings = new XmlWriterSettings {
                Indent = true,
                IndentChars = "  ",
                ConformanceLevel = ConformanceLevel.Auto
            };

            using(XmlWriter writer = XmlWriter.Create(outputFileName: fileName, settings: settings)) {
                foreach(ProgramObject po in trackedPrograms.Values) {
                    writer.WriteStartElement("program");
                    writer.WriteElementString("name", po.programName);
                    writer.WriteElementString("currVersion", po.programVersion);
                    writer.WriteElementString("availVersion", po.availableVersion);
                    writer.WriteElementString("publisher", po.publisher);
                    writer.WriteElementString("installDate", po.installDate);
                    writer.WriteElementString("updateUrl", po.updateURL);
                    writer.WriteEndElement();
                }
                writer.Flush();
            }
        }
    }
}