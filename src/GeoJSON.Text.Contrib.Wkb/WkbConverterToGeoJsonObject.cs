namespace GeoJSON.Text.Contrib.Wkb
{
    /// <summary>
    /// GeoJSON.Text / Wkb converter.
    /// All methods here are static and extensions to GeoJSON.Text types and Db geography and geometry types.
    /// </summary>
    public static partial class WkbConverter
    {
        public static T ToGeoJSONObject<T>(this byte[] wkb) where T : GeoJSONObject
        {
            return wkb.ToGeoJSONGeometry() as T;
        }

        public static T ToGeoJSONObject<T>(this string wkt) where T : GeoJSONObject
        {
            return wkt.ToGeoJSONGeometry() as T;
        }
    }
}
