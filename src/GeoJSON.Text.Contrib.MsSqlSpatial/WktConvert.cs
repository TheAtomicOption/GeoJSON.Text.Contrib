using GeoJSON.Text.Geometry;
using Microsoft.SqlServer.Types;
using System;
using System.Data.SqlTypes;

namespace GeoJSON.Text.Contrib.MsSqlSpatial
{
    /// <summary>
    /// Well Known Text (WKT) helper class. Allow to generate GeoJSON objects from WKT.
    /// </summary>
    public static class WktConvert
    {
        /// <summary>
        /// IGeometryObject representing WKT
        /// </summary>
        /// <param name="wkt">WKT string</param>
        /// <param name="spatialReferenceId">SRID (defaults to WGS84)</param>
        /// <returns></returns>
        public static IGeometryObject GeoJSONGeometry(string wkt, int spatialReferenceId = 4326)
        {
            if (string.IsNullOrWhiteSpace(wkt))
                return null;

            SqlGeometry geom;
            try
            {
                geom = SqlGeometry.STGeomFromText(new SqlChars(new SqlString(wkt)), spatialReferenceId);
            }
            catch (Exception)
            {
                throw;
            }

            return MsSqlSpatialConvert.ToGeoJSONGeometry(geom);
        }

        /// <summary>
        /// GeoJSON typed object representing WKT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wkt">WKT string</param>
        /// <param name="spatialReferenceId">SRID (defaults to WGS84)</param>
        /// <returns></returns>
        public static T GeoJSONObject<T>(string wkt, int spatialReferenceId = 4326) where T : GeoJSONObject
        {
            if (string.IsNullOrWhiteSpace(wkt))
                return null;

            SqlGeometry geom;
            try
            {
                geom = SqlGeometry.STGeomFromText(new SqlChars(new SqlString(wkt)), spatialReferenceId);
            }
            catch (Exception)
            {
                throw;
            }

            return geom.ToGeoJSONObject<T>();
        }
    }
}
