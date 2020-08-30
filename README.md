# OSM extractor

Reads the OSM file from osm_files directory for a given country and creates a JSON file that contains peaks, waterfalls, castles for it.

## Dependency

Uses Newtonsoft.JSON for serializing to JSON.
`dotnet add package Newtonsoft.Json --version 12.0.3`

## OSM file

All the data is taken from an OMS file. OSM - [OpenStreetMap](https://www.openstreetmap.org/) data file.
OSM file for Slovenia was downloaded at: https://download.geofabrik.de/europe/slovenia.html

## Licence

https://www.openstreetmap.org/copyright