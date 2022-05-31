using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON.Text.Geometry;

namespace GeoJSON.Text.Contrib.Wkb.Conversions
{
    /// <summary>
    /// Wkb to GeoJson encoder.
    /// Built from https://github.com/gksource/GeoJSON.Text.Contrib.EF
    /// </summary>
    public static class WktEncode
    {
        public static string Encode(IGeometryObject geometryObject)
        {
            var v_bw = new StringBuilder();
            Encode(v_bw, geometryObject);
            return v_bw.ToString();
        }

        public static string Encode(Feature.Feature feature)
        {
            return Encode(feature.Geometry);
        }

        private static void Point(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            var v_Point = geometryObject as Point;
            var hasAltitude = HasAltitude(v_Point);
            
            var position = v_Point.Coordinates as Position;
            StringBuilder.Append(position.Longitude);
            StringBuilder.Append(' ');
            StringBuilder.Append(position.Latitude);
            if (hasAltitude)
            {
                StringBuilder.Append(' ');
                StringBuilder.Append((double)position.Altitude);
            }
        }

        private static void MultiPoint(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            var multiPoint = geometryObject as MultiPoint;
            StringBuilder.Append('(');
            foreach (Point point in multiPoint.Coordinates)
            {
                Point(StringBuilder, point);
                StringBuilder.Append(", ");
            }
            if (multiPoint.Coordinates.Count > 0)
                StringBuilder.Remove(StringBuilder.Length - 2, 2);
            StringBuilder.Append(')');

        }

        private static void Points(StringBuilder StringBuilder, List<IPosition> positions, bool hasAltitude)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                IPosition v_Point = positions[i];
                var position = v_Point as Position;
                StringBuilder.Append(position.Longitude);
                StringBuilder.Append(' ');
                StringBuilder.Append(position.Latitude);
                if (hasAltitude)
                {
                    StringBuilder.Append(' ');
                    StringBuilder.Append((double)position.Altitude);
                }
                StringBuilder.Append(", ");
            }
            if(positions.Count > 0)
                StringBuilder.Remove(StringBuilder.Length - 2, 2);
        }

        private static void Polyline(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            var v_Polyline = geometryObject as LineString;
            var hasAltitude = HasAltitude(v_Polyline);
            Points(StringBuilder, v_Polyline.Coordinates.ToList(), hasAltitude);
        }

        private static void MultiPolyline(StringBuilder StringBuilder, IGeometryObject GeometryObject)
        {
            var multiLineString = GeometryObject as MultiLineString;
            foreach (LineString lineString in multiLineString.Coordinates)
            {
                StringBuilder.Append('(');
                Polyline(StringBuilder, lineString);
                StringBuilder.Append(')');
                StringBuilder.Append(", ");
            }
            if (multiLineString.Coordinates.Count > 0)
                StringBuilder.Remove(StringBuilder.Length - 2, 2);
        }

        private static void Polygon(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            var polygon = geometryObject as Polygon;
            var hasAltitude = HasAltitude(polygon);
            foreach (LineString ring in polygon.Coordinates)
            {
                StringBuilder.Append('(');
                Points(StringBuilder, ring.Coordinates.ToList(), hasAltitude);
                StringBuilder.Append(')');
                StringBuilder.Append(", ");
            }
            if (polygon.Coordinates.Count > 0)
                StringBuilder.Remove(StringBuilder.Length - 2, 2);
        }

        private static void MultiPolygon(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            var multiPolygon = geometryObject as MultiPolygon;
            foreach (Polygon polygon in multiPolygon.Coordinates)
            {
                StringBuilder.Append('(');
                Polygon(StringBuilder, polygon);
                StringBuilder.Append(')');
                StringBuilder.Append(", ");
            }
            if (multiPolygon.Coordinates.Count > 0)
                StringBuilder.Remove(StringBuilder.Length - 2, 2);
        }

        private static void GeometryCollection(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            var geometryCollection = geometryObject as GeometryCollection;
            foreach (IGeometryObject geometry in geometryCollection.Geometries)
            {
                Encode(StringBuilder, geometry);
                StringBuilder.Append(", ");
            }
            if (geometryCollection.Geometries.Count > 0)
                StringBuilder.Remove(StringBuilder.Length - 2, 2);
        }

        private static void Encode(StringBuilder StringBuilder, IGeometryObject geometryObject)
        {
            StringBuilder.Append(geometryObject.Type.ToString().ToUpper());
            bool hasAltitude = geometryObject.Type switch
            {
                GeoJSONObjectType.Point => HasAltitude(geometryObject as Point),
                GeoJSONObjectType.MultiPoint => HasAltitude(geometryObject as MultiPoint),
                GeoJSONObjectType.Polygon => HasAltitude(geometryObject as Polygon),
                GeoJSONObjectType.MultiPolygon => HasAltitude(geometryObject as MultiPolygon),
                GeoJSONObjectType.LineString => HasAltitude(geometryObject as LineString),
                GeoJSONObjectType.MultiLineString => HasAltitude(geometryObject as MultiLineString),
                GeoJSONObjectType.GeometryCollection => HasAltitude(geometryObject as GeometryCollection),
                _ => throw new Exception("Geometry contains unsupported type.")
            };

            if (hasAltitude)
                StringBuilder.Append(" Z");

            StringBuilder.Append(" (");
            switch (geometryObject.Type)
            {
                case GeoJSONObjectType.Point:
                    Point(StringBuilder, geometryObject);
                    break;

                case GeoJSONObjectType.MultiPoint:
                    MultiPoint(StringBuilder, geometryObject);
                    break;

                case GeoJSONObjectType.Polygon:
                    Polygon(StringBuilder, geometryObject);
                    break;

                case GeoJSONObjectType.MultiPolygon:
                    MultiPolygon(StringBuilder, geometryObject);
                    break;

                case GeoJSONObjectType.LineString:
                    Polyline(StringBuilder, geometryObject);
                    break;

                case GeoJSONObjectType.MultiLineString:
                    MultiPolyline(StringBuilder, geometryObject);
                    break;

                case GeoJSONObjectType.GeometryCollection:
                    GeometryCollection(StringBuilder, geometryObject);
                    break;
            }
            StringBuilder.Append(')');
        }

        private static bool HasAltitude(Point point)
        {
            return point.Coordinates.Altitude != null;
        }

        private static bool HasAltitude(MultiPoint multiPoint)
        {
            return multiPoint.Coordinates.FirstOrDefault()?.Coordinates.Altitude != null;
        }

        private static bool HasAltitude(Polygon polygon)
        {
            return polygon.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(MultiPolygon multiPolygon)
        {
            return multiPolygon.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(LineString lineString)
        {
            return lineString.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(MultiLineString multiLineString)
        {
            return multiLineString.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(GeometryCollection geometryCollection)
        {
            if (geometryCollection.Geometries.Count == 0)
            {
                return false;
            }

            var firstGeometry = geometryCollection.Geometries.First();
            switch (firstGeometry.Type)
            {
                case GeoJSONObjectType.Point:
                    return HasAltitude(firstGeometry as Point);

                case GeoJSONObjectType.MultiPoint:
                    return HasAltitude(firstGeometry as MultiPoint);

                case GeoJSONObjectType.Polygon:
                    return HasAltitude(firstGeometry as Polygon);

                case GeoJSONObjectType.MultiPolygon:
                    return HasAltitude(firstGeometry as MultiPolygon);

                case GeoJSONObjectType.LineString:
                    return HasAltitude(firstGeometry as LineString);

                case GeoJSONObjectType.MultiLineString:
                    return HasAltitude(firstGeometry as MultiLineString);

                default:
                    throw new Exception("Unsupported type");
            }
        }
    }
}
