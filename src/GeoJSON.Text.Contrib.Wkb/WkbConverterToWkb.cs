using GeoJSON.Text.Contrib.Wkb.Conversions;
using GeoJSON.Text.Geometry;

namespace GeoJSON.Text.Contrib.Wkb
{
    /// <summary>
    /// GeoJSON.Text / Wkb converter.
    /// All methods here are static and extensions to GeoJSON.Text types and Db geography and geometry types.
    /// </summary>
    public static partial class WkbConverter
    {
        public static byte[] ToWkb(this IGeometryObject geometryObject)
        {
            return WkbEncode.Encode(geometryObject);
        }

        public static string ToWkt(this IGeometryObject geometryObject)
        {
            return WktEncode.Encode(geometryObject);
        }
    }
}
