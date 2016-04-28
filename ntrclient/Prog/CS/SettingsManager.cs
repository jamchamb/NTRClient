using System;
using System.IO;
using System.Windows.Forms;

namespace ntrclient.Prog.CS
{
    public class SettingsManager
    {
        public string[] QuickCmds { set; get; }
        public string IpAddress { set; get; }
        public int GsUsed { set; get; }

        public void Init()
        {
            if (QuickCmds == null)
            {
                QuickCmds = new string[10];
                for (int i = 0; i < QuickCmds.Length; i++)
                {
                    QuickCmds[i] = "";
                }
            }
            if (IpAddress == null)
            {
                IpAddress = "Nintendo 3DS IP";
            }
            if (GsUsed < 0)
            {
                GsUsed = 0;
            }
        }

        public static void SaveToXml(string filePath, SettingsManager sourceObj)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(sourceObj.GetType());
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static SettingsManager LoadFromXml(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof (SettingsManager));
                    return (SettingsManager) xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"Ignore this message if you just " + Environment.NewLine +
                    @"Downloaded or Updated this tool..." + Environment.NewLine + Environment.NewLine +
                    ex.Message
                    );
            }
            return new SettingsManager();
        }
    }
}