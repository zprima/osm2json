# OSM extractor

Reads the OSM file from osm_files directory for a given country and creates a JSON file that contains peaks, waterfalls, castles for it.

## Dependency

Uses Newtonsoft.JSON for serializing to JSON.
`dotnet add package Newtonsoft.Json --version 12.0.3`

## OSM file

Contains information from [slovenia-latest.osm](https://download.geofabrik.de/europe/slovenia.html), which is made available here under the [Open Database License (ODbL)](https://opendatacommons.org/licenses/odbl/1-0/).
All the data is taken from an OMS file. OSM - [OpenStreetMap](https://www.openstreetmap.org/) data file.
OSM file for Slovenia was downloaded at: https://download.geofabrik.de/europe/slovenia.html

## Licence

Data gathered from OSM and the data files are under [Open Database Licence](https://www.openstreetmap.org/copyright)
Code is under MIT.

## Credit
[OpenStreetMap and its contributors](https://www.openstreetmap.org)