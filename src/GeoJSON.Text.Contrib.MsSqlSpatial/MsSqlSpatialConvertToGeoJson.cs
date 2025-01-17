using System;
using GeoJSON.Text.Geometry;
using GeoJSON.Text.Contrib.MsSqlSpatial.Sinks;
using Microsoft.SqlServer.Types;

namespace GeoJSON.Text.Contrib.MsSqlSpatial
{
    /*
        MsSqlSpatialConvert
        Partial class. Only methods from Sql spatial types to GeoJSON are here
        For Sql spatial types to GeoJSON, see MsSqlSpatialConvertToGeoJson.cs file
        For GeoJSON to Sql Server geometry, see MsSqlSpatialConvertToSqlGeometry.cs file
        For GeoJSON to Sql Server geography, see MsSqlSpatialConvertToSqlGeography.cs file
    */
    /// <summary>
    /// GeoJSON.Text / MS Sql Server Spatial data types converter.
    /// All methods here are static and extensions to GeoJSON.Text types and Sql Server types.
    /// </summary>
    public static partial class MsSqlSpatialConvert
    {
        #region SqlGeometry to GeoJSON

        /// <summary>
        /// Converts a native Sql Server geometry (lat/lon) to GeoJSON geometry
        /// </summary>
        /// <param name="sqlGeometry">SQL Server geometry to convert</param>
        /// <param name="withBoundingBox">Value indicating whether the feature's BoundingBox should be set.</param>
        /// <returns>GeoJSON geometry</returns>
        public static IGeometryObject ToGeoJSONGeometry(this SqlGeometry sqlGeometry, bool withBoundingBox = true)
        {
            if (sqlGeometry == null || sqlGeometry.IsNull)
            {
                return null;
            }

            // Make valid if necessary
            sqlGeometry = sqlGeometry.MakeValidIfInvalid();
            if (sqlGeometry.STIsValid().IsFalse)
            {
                throw new Exception("Invalid geometry : " + sqlGeometry.IsValidDetailed());
            }

            // Conversion using geometry sink
            SqlGeometryGeoJsonSink sink = new SqlGeometryGeoJsonSink(withBoundingBox);
            sqlGeometry.Populate(sink);
            return sink.ConstructedGeometry;
        }

        /// <summary>
        /// Converts a native Sql Server geometry (lat/lon) to GeoJSON geometry
        /// </summary>
        /// <param name="sqlGeometry">SQL Server geometry to convert</param>
        /// <param name="withBoundingBox">Value indicating whether the feature's BoundingBox should be set.</param>
        /// <returns>GeoJSON geometry</returns>
        public static T ToGeoJSONObject<T>(this SqlGeometry sqlGeometry, bool withBoundingBox = true) where T : GeoJSONObject
        {
            if (sqlGeometry == null || sqlGeometry.IsNull)
            {
                return null;
            }

            // Make valid if necessary
            sqlGeometry = sqlGeometry.MakeValidIfInvalid();
            if (sqlGeometry.STIsValid().IsFalse)
            {
                throw new Exception("Invalid geometry : " + sqlGeometry.IsValidDetailed());
            }

            // Conversion using geometry sink
            T geoJSONobj = null;
            SqlGeometryGeoJsonSink sink = new SqlGeometryGeoJsonSink();
            sqlGeometry.Populate(sink);
            geoJSONobj = sink.ConstructedGeometry as T;

            geoJSONobj.BoundingBoxes = withBoundingBox ? sink.BoundingBox : null;

            return geoJSONobj;
        }

        #endregion

        #region SqlGeography to GeoJSON

        /// <summary>
        /// Converts a native Sql Server geography to GeoJSON geometry
        /// </summary>
        /// <param name="sqlGeography">SQL Server geography to convert</param>
        /// <returns>GeoJSON geometry</returns>
        public static IGeometryObject ToGeoJSONGeometry(this SqlGeography sqlGeography)
        {
            if (sqlGeography == null || sqlGeography.IsNull)
            {
                return null;
            }

            // Make valid if necessary
            sqlGeography = sqlGeography.MakeValidIfInvalid();
            if (sqlGeography.STIsValid().IsFalse)
            {
                throw new Exception("Invalid geometry : " + sqlGeography.IsValidDetailed());
            }

            // Conversion using geography sink
            SqlGeographyGeoJsonSink sink = new SqlGeographyGeoJsonSink();
            sqlGeography.Populate(sink);
            return sink.ConstructedGeography;
        }

        /// <summary>
        /// Converts a native Sql Server geography to GeoJSON geometry
        /// </summary>
        /// <param name="sqlGeography">SQL Server geography to convert</param>
        /// <param name="withBoundingBox">Value indicating whether the feature's BoundingBox should be set.</param>
        /// <returns>GeoJSON geometry</returns>
        public static T ToGeoJSONObject<T>(this SqlGeography sqlGeography, bool withBoundingBox = true) where T : GeoJSONObject
        {
            if (sqlGeography == null || sqlGeography.IsNull)
            {
                return null;
            }

            // Make valid if necessary
            sqlGeography = sqlGeography.MakeValidIfInvalid();
            if (sqlGeography.STIsValid().IsFalse)
            {
                throw new Exception("Invalid geometry : " + sqlGeography.IsValidDetailed());
            }

            // Conversion using geography sink
            T geoJSONobj = null;
            SqlGeographyGeoJsonSink sink = new SqlGeographyGeoJsonSink();
            sqlGeography.Populate(sink);
            geoJSONobj = sink.ConstructedGeography as T;
            geoJSONobj.BoundingBoxes = withBoundingBox ? sink.BoundingBox : null;

            return geoJSONobj;
        }

        #endregion
    }
}
