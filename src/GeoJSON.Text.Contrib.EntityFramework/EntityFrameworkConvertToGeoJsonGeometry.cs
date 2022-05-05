using GeoJSON.Text.Geometry;
using GeoJSON.Text.Contrib.Wkb.Conversions;

namespace GeoJSON.Text.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        //public static IGeometryObject ToGeoJSONGeometry(this DbGeography dbGeography)
        //{
        //    return WkbDecode.Decode(dbGeography.AsBinary());
        //}

        public static IGeometryObject ToGeoJSONGeometry(this NetTopologySuite.Geometries.Geometry dbGeometry)
        {
            return WkbDecode.Decode(dbGeometry.AsBinary());
        }
    }
}
