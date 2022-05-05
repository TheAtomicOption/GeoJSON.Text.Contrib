# GeoJSON.Text.Contrib.MsSqlSpatial
Allows conversion from / to Microsoft Sql Server geometry and geography data types.

## Usage:

```csharp
using GeoJSON.Text.Geometry;
using Microsoft.SqlServer.Types;
using GeoJSON.Text.Contrib.MsSqlSpatial;

// SqlGeometry sample point
SqlGeometry simplePoint = SqlGeometry.Point(1, 47, 4326);

// SqlGeometry -> GeoJSON example
Point point = simplePoint.ToGeoJSONObject<Point>();

// GeoJSON -> SqlGeometry example
SqlGeometry sqlPoint = point.ToSqlGeometry(4326);
```