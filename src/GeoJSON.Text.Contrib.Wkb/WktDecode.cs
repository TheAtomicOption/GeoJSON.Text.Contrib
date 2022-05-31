using System;
using System.Linq;
using System.Collections.Generic;
using GeoJSON.Text.Geometry;
using System.Text.RegularExpressions;

namespace GeoJSON.Text.Contrib.Wkb.Conversions
{
    /// <summary>
    /// Wkb to GeoJson decoder.
    /// Built from https://github.com/gksource/GeoJSON.Text.Contrib.EF
    /// </summary>
    public static class WktDecode
    {
        private static char[] doubleChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.' };
        private static char[] firstChars = { 'M', 'P', 'L', 'G' };
        public static IGeometryObject Decode(string wkt)
        {
            var v_pos = 0;
            return ParseShape(wkt, ref v_pos);
        }

        private static Point ParsePoint(string wkt, ref int wktPosition, bool hasAltitude)
        {
            ZipToNextDouble(wkt, ref wktPosition);
            Position geographicalPosition = GetGeographicPosition(wkt, ref wktPosition, hasAltitude);
            wktPosition++;
            return new Point(geographicalPosition);
        }

        private static LineString ParseLineString(string wkt, ref int wktPosition, bool hasAltitude)
        {
            Position[] positions = ParsePositions(wkt, ref wktPosition, hasAltitude);
            return new LineString(positions);
        }

        private static Polygon ParsePolygon(string wkt, ref int wktPosition, bool hasAltitude)
        {
            var lines = new List<LineString>();
            while (wktPosition < wkt.Length - 1)
            {
                ZipToNextDouble(wkt, ref wktPosition);
                Position[] positions = ParsePositions(wkt, ref wktPosition, hasAltitude);
                lines.Add(new LineString(positions));
                wktPosition++;
                if (wkt.IndexOfAny(doubleChars, wktPosition) == -1
                    || wkt.IndexOf('(', wktPosition) == -1
                    || wkt.IndexOf(')', wktPosition) < wkt.IndexOf('(', wktPosition))
                {
                    break;
                }
            }
            return new Polygon(lines);
        }

        private static MultiPoint ParseMultiPoint(string wkt, ref int wktPosition, bool hasAltitude)
        {
            var points = new List<Point>();
            while (wktPosition < wkt.Length - 1)
            {
                ZipToNextDouble(wkt, ref wktPosition);
                points.Add(ParsePoint(wkt, ref wktPosition, hasAltitude));
                if (wkt.IndexOfAny(doubleChars, wktPosition) == -1
                    || wkt.IndexOf(')', wktPosition) < wkt.IndexOfAny(doubleChars, wktPosition)
                    )
                {
                    break;
                }
                wktPosition++;
            }
            wktPosition++;
            return new MultiPoint(points);
        }

        private static MultiLineString ParseMultiLineString(string wkt, ref int wktPosition, bool hasAltitude)
        {
            var lines = new List<LineString>();
            wktPosition++;
            while (true)
            {
                lines.Add(ParseLineString(wkt, ref wktPosition, hasAltitude));
                wktPosition++;
                if (wktPosition >= wkt.Length - 1
                    || wkt.IndexOf('(', wktPosition) == -1
                    || wkt.IndexOfAny(doubleChars, wktPosition) == -1
                    || (wkt.IndexOf(')', wktPosition) < wkt.IndexOfAny(doubleChars, wktPosition))
                    )
                {
                    break;
                }
            }
            wktPosition++;
            return new MultiLineString(lines);
        }

        private static MultiPolygon ParseMultiPolygon(string wkt, ref int wktPosition, bool hasAltitude)
        {
            var polygons = new List<Polygon>();

            while (true)
            {
                ZipToNextDouble(wkt, ref wktPosition);
                polygons.Add(ParsePolygon(wkt, ref wktPosition, hasAltitude));
                wktPosition++;
                if (wktPosition >= wkt.Length - 1
                || wkt.IndexOf('(', wktPosition) == -1
                || wkt.IndexOfAny(doubleChars, wktPosition) == -1
                || wkt.IndexOf(')', wktPosition) < wkt.IndexOfAny(doubleChars, wktPosition))
                {
                    break;
                }
            }

            return new MultiPolygon(polygons);
        }

        private static GeometryCollection ParseGeometryCollection(string wkt, ref int wktPosition)
        {
            var geometries = new List<IGeometryObject>();

            while (wktPosition < wkt.Length - 1)
            {
                if (wkt.Substring(wktPosition).ToUpper().IndexOfAny(firstChars) < 0)
                    break;
                wktPosition = wkt.IndexOfAny(firstChars, wktPosition);
                geometries.Add(ParseShape(wkt, ref wktPosition));
            }

            return new GeometryCollection(geometries);
        }

        private static IGeometryObject ParseShape(string wkt, ref int wktPosition)
        {
            wkt = wkt.Trim();
            int positionStart = wktPosition;
            wktPosition = wkt.IndexOfAny(new char[] { ' ', '(' }, positionStart);
            WkbGeometryType v_type = (WkbGeometryType)Enum.Parse(typeof(WkbGeometryType), wkt.Substring(positionStart, wktPosition - positionStart), true);

            while (wkt[wktPosition] != '(')
            {
                wktPosition++;
            }

            if (Regex.Match(wkt.Substring(positionStart, wktPosition - positionStart), "( M )|( ZM )|( M\\()|( ZM\\()").Success)
            {
                throw new ArgumentOutOfRangeException("WKT data with an M value is currently not supported.");
            }
            bool hasAltitude = wkt.Substring(positionStart, wktPosition - positionStart).Contains('Z');

            switch (v_type)
            {
                case WkbGeometryType.Point:
                    return ParsePoint(wkt, ref wktPosition, hasAltitude);

                case WkbGeometryType.LineString:
                    return ParseLineString(wkt, ref wktPosition, hasAltitude);

                case WkbGeometryType.Polygon:
                    return ParsePolygon(wkt, ref wktPosition, hasAltitude);

                case WkbGeometryType.MultiPoint:
                    return ParseMultiPoint(wkt, ref wktPosition, hasAltitude);

                case WkbGeometryType.MultiLineString:
                    return ParseMultiLineString(wkt, ref wktPosition, hasAltitude);

                case WkbGeometryType.MultiPolygon:
                    return ParseMultiPolygon(wkt, ref wktPosition, hasAltitude);

                case WkbGeometryType.GeometryCollection:
                    return ParseGeometryCollection(wkt, ref wktPosition);

                default:
                    throw new Exception("Unsupported type");
            }
        }

        private static double GetDouble(string wkt, ref int wktPosition)
        {
            int startPos = wktPosition;
            while (doubleChars.Contains(wkt[wktPosition]))
            {
                wktPosition++;
            }
            var intConversion = wkt.Substring(startPos, wktPosition - startPos);
            return double.Parse(intConversion);
        }

        private static Position[] ParsePositions(string wkt, ref int wktPosition, bool hasAltitude)
        {
            List<Position> positions = new();
            int parenSemiphore = 1;
            while (parenSemiphore > 0)
            {
                ZipToNextDouble(wkt, ref wktPosition);
                positions.Add(GetGeographicPosition(wkt, ref wktPosition, hasAltitude));
                if (wkt.IndexOfAny(doubleChars, wktPosition) == -1
                    || wkt.IndexOf(')', wktPosition) < wkt.IndexOfAny(doubleChars, wktPosition))
                {
                    break;
                }
            }
            return positions.ToArray();
        }

        private static Position GetGeographicPosition(string wkt, ref int wktPosition, bool hasAltitude)
        {
            ZipToNextDouble(wkt, ref wktPosition);
            var longitude = GetDouble(wkt, ref wktPosition);
            ZipToNextDouble(wkt, ref wktPosition);
            var latitude = GetDouble(wkt, ref wktPosition);
            if (hasAltitude)
                ZipToNextDouble(wkt, ref wktPosition);
            var altitude = hasAltitude ? GetDouble(wkt, ref wktPosition) : (double?)null;
            return new Position(latitude, longitude, altitude);
        }

        private static void ZipToNextDouble(string wkt, ref int wktPosition)
        {
            while (!doubleChars.Contains(wkt[wktPosition]) && wktPosition < wkt.Length - 1)
            {
                wktPosition++;
            }
        }
    }

}
