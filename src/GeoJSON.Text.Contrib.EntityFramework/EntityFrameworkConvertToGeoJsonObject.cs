
namespace GeoJSON.Text.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        //public static T ToGeoJSONObject<T>(this DbGeography dbGeography) where T : GeoJSONObject
        //{
        //    return dbGeography.ToGeoJSONGeometry() as T;
        //}

        public static T ToGeoJSONObject<T>(this NetTopologySuite.Geometries.Geometry dbGeometry) where T : GeoJSONObject
        {
            return dbGeometry.ToGeoJSONGeometry() as T;
        }
    }
}
