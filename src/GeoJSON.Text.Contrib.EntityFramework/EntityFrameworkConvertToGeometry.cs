using System;
using GeoJSON.Text.Geometry;
using GeoJSON.Text.Contrib.Wkb.Conversions;

namespace GeoJSON.Text.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        [Obsolete("This method will be removed in future releases, consider migrating now to the newest signature.", false)]
        public static NetTopologySuite.Geometries.Geometry ToGeometry(this IGeometryObject geometryObject)
        {
            return new NetTopologySuite.IO.WKBReader().Read(WkbEncode.Encode(geometryObject));
        }

        public static NetTopologySuite.Geometries.Geometry ToGeometry(this IGeometryObject geometryObject, int coordinateSystemId = 4326)
        {
            return new NetTopologySuite.IO.WKBReader().Read(WkbEncode.Encode(geometryObject));
        }
    }
}
