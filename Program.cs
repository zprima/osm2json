using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace osm2json
{
  class Program
  {
    static void Main(string[] args)
    {
      string countryName = "slovenia";
      string countryAlpha2Code = "si";

      Parser parser = new Parser(countryName, countryAlpha2Code);
      parser.ProcessOSMFile();
    }
  }

  class GeoEntry
  {
    // Tag:natural=peak
    // Tag:waterway=waterfall
    // Tag:historic=castle

    public static Dictionary<string, string> tagLookup = new Dictionary<string, string>(){
      {"natural", "peak"},
      {"waterway", "waterfall"},
      {"historic", "castle"}
    };

    public enum Categories { Peak, Waterfall, Castle }
    public string Name { get; set; }
    public string NameNative { get; set; }
    public string Lat { get; set; }
    public string Lon { get; set; }
    public int Category { get; set; }

    public GeoEntry(string name, string name_native, string lat, string lon, int category)
    {
      this.Name = name;
      this.NameNative = name_native;
      this.Lat = lat;
      this.Lon = lon;
      this.Category = category;
    }
  }

  class Parser
  {
    public const string NODE_NAME = "node";
    public string countryName { get; set; }
    public string countryAlpha2Code { get; set; }
    public string osmFilePath { get; set; }

    public List<GeoEntry> GeoEntries { get; set; } = new List<GeoEntry>();

    public Parser(string countryName, string countryAlpha2Code)
    {
      this.countryName = countryName;
      this.osmFilePath = $"./osm_files/{countryName}.osm";
      this.countryAlpha2Code = countryAlpha2Code;
    }

    void ProcessGeoNode(XElement nodeElement)
    {


        System.Console.WriteLine(nodeElement);
    }

    public void ProcessOSMFile()
    {
      try
      {
        using (XmlReader reader = XmlReader.Create(osmFilePath))
        {
          reader.MoveToContent();
          if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(NODE_NAME))
          {
            XElement nodeElement = XNode.ReadFrom(reader) as XElement;
            ProcessGeoNode(nodeElement);
          }
          else
          {
            reader.Read();
          }
        }
      }
      catch (IOException e)
      {
        System.Console.WriteLine("Failed to open file");
        System.Console.WriteLine(e.Message);
      }
    }
  }
}
