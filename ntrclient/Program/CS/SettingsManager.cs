using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntrclient {
	public class SettingsManager {
		public string[] quickCmds {set; get;}
        public string ip_address { set; get; }
        public int gs_used { set; get; }

		public void init() {
			if (quickCmds == null ) {
				quickCmds = new string[10];
				for (int i = 0; i < quickCmds.Length; i++) {
					quickCmds[i] = "";
				}
			}
            if (ip_address == null)
            {
                ip_address = "Nintendo 3DS IP";
            }
            if (gs_used < 0)
            {
                gs_used = 0;
            }
		}

		public static void SaveToXml(string filePath, SettingsManager sourceObj) {

				try {
					using (StreamWriter writer = new StreamWriter(filePath)) {
						System.Xml.Serialization.XmlSerializer xmlSerializer = 
							new System.Xml.Serialization.XmlSerializer(sourceObj.GetType()); 
						xmlSerializer.Serialize(writer, sourceObj);
				}
				
				} catch(Exception ex) {
					MessageBox.Show(ex.Message);
				}

		}

		public static SettingsManager LoadFromXml(string filePath) {
			try {
				using (StreamReader reader = new StreamReader(filePath)) {
					System.Xml.Serialization.XmlSerializer xmlSerializer = 
						new System.Xml.Serialization.XmlSerializer(typeof(SettingsManager));
					return (SettingsManager)xmlSerializer.Deserialize(reader);
				}
			}
			catch (Exception ex) {
				MessageBox.Show(
                    "Ignore this message if you just \r\n" +
                    "Downloaded or Updated this tool...\r\n\r\n" + 
                    ex.Message
                );
			}
			return new SettingsManager();
		}
	}
}
