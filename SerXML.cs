using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GCode2xml
{
    public class XML
    {
        [XmlElement] public Part[] PART;
    }

    public class Part
    {
        [XmlElement] public Ply[] PLY;
        [XmlAttribute] public string NAME;
        [XmlAttribute] public string STATUS;
        [XmlAttribute] public string DBVERSION;
        [XmlAttribute] public string FIELDDATE;
        [XmlAttribute] public string INVERTNORMALS;
        [XmlAttribute] public int HIGHESTPLY;
        [XmlAttribute] public string MODIFIED;
        [XmlAttribute] public int TOPLAYERS;
        [XmlAttribute] public int DEFAULTLASER;
        [XmlAttribute] public string USEASSIGNEDLASERS;
    }

    public class Ply
    {
        [XmlElement] public Point[] POINT;
        [XmlAttribute] public string NAME;
        [XmlElement] public string INSTRUCTIONS;
        [XmlElement] public string NOTES;
    }

    public class Point
    {
        [XmlAttribute] public int NUMBER;
        [XmlAttribute] public string NAME;
        [XmlAttribute] public decimal XVAL;
        [XmlAttribute] public decimal YVAL;
        [XmlAttribute] public decimal ZVAL;
        [XmlAttribute] public decimal IVAL;
        [XmlAttribute] public decimal JVAL;
        [XmlAttribute] public decimal KVAL;
    }

    class SerXML
    {
        public static void Export2XML(Point[] plyPoints, string partName, string exportPath)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(XML));
            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
            xns.Add("", "");

            if ( partName.StartsWith("L1-") == false) { partName = "L1-" + partName; } // add required name prefix, if missing
            if ( exportPath.EndsWith("\\") == false ) { exportPath += "\\"; }

            TextWriter writer = new StreamWriter(exportPath + partName + ".xml");
            
            // create instance of PLY class, assign values to ply attributes
            Ply ply1 = new Ply();
            ply1.NAME = partName;
            ply1.INSTRUCTIONS= "";
            ply1.NOTES = "";
            Ply[] plys = { ply1 };

            plys[0].POINT = plyPoints; // assign points to point elements in Plys class

            // create instance of Part class, assign values to part attributes
            Part part1 = new Part();
            part1.NAME = partName;
            part1.STATUS = "";
            part1.DBVERSION = "3.9";
            part1.HIGHESTPLY = 1;
            part1.TOPLAYERS = 1;
            part1.FIELDDATE = "";
            Part[] parts = { part1 };
            parts[0].PLY = plys; // assign plys to PLY element

            // create instance of XML class
            XML xm1 = new XML();
            xm1.PART = parts; // assign parts to PART element

            // write xml data to file
            serializer.Serialize(writer, xm1, xns);
            writer.Close();
        }
    }
}