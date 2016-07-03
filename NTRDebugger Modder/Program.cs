using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NTRDebugger_Modder
{
    class Program
    {
        static void Main(string[] args)
        {
            String FilePath = @"mods\mh4x_jp.xml";
            String CodesPath = @"codes\mhx_jp.txt";
            String Author = @"imthe666st";
            String Version = @"1.0.0";
            String GameName = @"Festa_Ro";
            Regions Region = Regions.JPN;
            
            String[] Codes_raw = File.ReadAllLines(CodesPath);

            /* Parse the codes */
            foreach (String CodeLine in Codes_raw)
            {
                if (CodeLine.Length > 2)
                {
                    if (CodeLine.StartsWith(@"[") && CodeLine.EndsWith(@"]"))
                    {

                    }
                }
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            XmlWriter writer = XmlWriter.Create(FilePath, settings);

            /* Creating the Mod */

            int x = 20, y = 20;

            writer.WriteStartElement("NTRClientMod");

            writer.WriteElementString("Author", Author);
            writer.WriteElementString("Version", Version);
            writer.WriteElementString("GameName", GameName);
            writer.WriteElementString("Region", Region.ToString());
            writer.WriteElementString("Codes", (s1.Length - 1).ToString());
            writer.WriteStartElement("Codes");

            for (int i = 0; i < CodeNames.Length; i++)
            {
                writer.WriteAttributeString("X", x.ToString());
                writer.WriteAttributeString("Width", "200");
                writer.WriteAttributeString("Y", y.ToString());
                writer.WriteAttributeString("Height", "20");

                writer.WriteElementString("Name", CodeNames[i]);
                writer.WriteStartElement("CodeLines");
                foreach (String code in Codes[i])
                {
                    writer.WriteElementString("CodeLine", code);
                }
                writer.WriteEndElement(); // CodeLines

            }

            writer.WriteEndElement(); // Codes
            writer.WriteEndElement(); // NTRClientMod

            /* Finished creating the mod */

            writer.Flush();
            writer.Close();

            Console.ReadLine();
        }

        public enum Regions
        {
            EUR = 0,
            USA = 1,
            JPN = 2,
        }
    }
}
