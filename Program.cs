using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
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
      parser.StoreDataFile();
    }
  }
  class GeoEntry
  {
    // Tag:natural=peak
    // Tag:waterway=waterfall
    // Tag:historic=castle
    public static string[] tagLookups = new string[] { "peak", "waterfall", "castle" };
    public string OpenStreetMapId {get; set;}
    public string Name { get; set; }
    public string NameNative { get; set; }
    public string NameEnglish { get; set; }
    public string Lat { get; set; }
    public string Lon { get; set; }
    public string Category { get; set; }
    public GeoEntry(string openStreetMapId, string name, string nameNative = null, string nameEnglish = null)
    {
      this.OpenStreetMapId = openStreetMapId;
      this.Name = name;
      this.NameNative = String.IsNullOrEmpty(nameNative) ? this.Name : nameNative;
      this.NameEnglish = String.IsNullOrEmpty(nameEnglish) ? this.Name : nameEnglish;
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
      IEnumerable<XElement> tags = nodeElement.Descendants().Where(item => item.Name == "tag");
      if (tags.Count() == 0)
      {
        return;
      }

      XElement tagMatchingCategory =
        tags.Where(tag =>
          GeoEntry.tagLookups.Contains(tag.Attribute("v").Value)
        ).FirstOrDefault();

      if (tagMatchingCategory == null)
      {
        return;
      }

      CategorizeAndStoreNode(nodeElement, tagMatchingCategory, tags);
    }
    void CategorizeAndStoreNode(XElement nodeElement, XElement tagMatchingCategory, IEnumerable<XElement> tags)
    {
      System.Console.WriteLine(nodeElement);

      var id = nodeElement.Attribute("id").Value;
      var category = tagMatchingCategory.Attribute("v").Value;

      var name = tags.Where(tag => tag.Attribute("k").Value == "name").Select(tag => tag.Attribute("v")).FirstOrDefault()?.Value;
      var nameNative = tags.Where(tag => tag.Attribute("k").Value == "name:si").Select(tag => tag.Attribute("v")).FirstOrDefault()?.Value;
      var nameEnglish = tags.Where(tag => tag.Attribute("k").Value == "name:en").Select(tag => tag.Attribute("v")).FirstOrDefault()?.Value;

      string lat = nodeElement.Attribute("lat").Value;
      string lon = nodeElement.Attribute("lon").Value;

      GeoEntry geoEntry = new GeoEntry(
        openStreetMapId: id,
        name: name,
        nameNative: nameNative,
        nameEnglish: nameEnglish
      );
      geoEntry.Category = category;
      geoEntry.Lat = lat;
      geoEntry.Lon = lon;

      GeoEntries.Add(geoEntry);
    }
    public void ProcessOSMFile()
    {
      try
      {
        using (XmlReader reader = XmlReader.Create(osmFilePath))
        {
          reader.MoveToContent();
          while (!reader.EOF)
          {
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
      }
      catch (IOException e)
      {
        System.Console.WriteLine("Failed to open file");
        System.Console.WriteLine(e.Message);
      }
    }
    public void StoreDataFile()
    {
      try
      {
        string json = JsonConvert.SerializeObject(GeoEntries);
        File.WriteAllText($"./data_files/{countryName}.json", json);
      }
      catch (JsonException e)
      {
        System.Console.WriteLine(e.Message);
      }
    }
  }
}
