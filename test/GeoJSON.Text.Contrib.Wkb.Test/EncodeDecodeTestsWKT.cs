using GeoJSON.Text.Geometry;
using Xunit;

namespace GeoJSON.Text.Contrib.Wkb.Test
{
    public partial class WktConversionsTests
    {
        [Fact]
        public void EncodeDecodePointTest()
        {
            var processedPoint = point.ToWkt().ToGeoJSONObject<Point>();

            Assert.Equal(point, processedPoint);
        }

        [Fact]
        public void EncodeDecodePointZTest()
        {
            var processedPointZ = pointZ.ToWkt().ToGeoJSONObject<Point>();

            Assert.Equal(pointZ, processedPointZ);
        }

        [Fact]
        public void EncodeDecodeMultiPointTest()
        {
            var processedMultiPoint = multiPoint.ToWkt().ToGeoJSONObject<MultiPoint>();

            var test1 = "MULTIPOINT (10 40, 40 30, 20 20, 30 10)".ToGeoJSONObject<MultiPoint>();
            var test2 = "MULTIPOINT ((10 40), (40 30), (20 20), (30 10))".ToGeoJSONObject<MultiPoint>();
            Assert.Equal(multiPoint, processedMultiPoint);
        }

        [Fact]
        public void EncodeDecodeMultiPointZTest()
        {
            var processedMultiPointZ = multiPointZ.ToWkt().ToGeoJSONObject<MultiPoint>();

            Assert.Equal(multiPointZ, processedMultiPointZ);
        }

        [Fact]
        public void EncodeDecodeLineStringTest()
        {
            var processedLineString = lineString.ToWkt().ToGeoJSONObject<LineString>();

            Assert.Equal(lineString, processedLineString);
        }

        [Fact]
        public void EncodeDecodeLineStringZTest()
        {
            var processedLineStringZ = lineStringZ.ToWkt().ToGeoJSONObject<LineString>();

            Assert.Equal(lineStringZ, processedLineStringZ);
        }

        [Fact]
        public void EncodeDecodeMultiLineStringTest()
        {
            var processedMultiLineString = multiLineString.ToWkt().ToGeoJSONObject<MultiLineString>();

            Assert.Equal(multiLineString, processedMultiLineString);
        }

        [Fact]
        public void EncodeDecodeMultiLineStringZTest()
        {
            var processedMultiLineStringZ = multiLineStringZ.ToWkt().ToGeoJSONObject<MultiLineString>();

            Assert.Equal(multiLineStringZ, processedMultiLineStringZ);
        }

        [Fact]
        public void EncodeDecodePolygonTest()
        {
            var processedPolygon = polygon.ToWkt().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygon, processedPolygon);
        }

        [Fact]
        public void EncodeDecodePolygonZTest()
        {
            var processedPolygonZ = polygonZ.ToWkt().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygonZ, processedPolygonZ);
        }

        [Fact]
        public void EncodeDecodePolygonWithHoleTest()
        {
            var processedPolygon = polygonWithHole.ToWkt().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygonWithHole, processedPolygon);
        }

        [Fact]
        public void EncodeDecodePolygonWithHoleReverseWindingTest()
        {
            var processedPolygon = polygonWithHoleReverseWinding.ToWkt().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygonWithHoleReverseWinding, processedPolygon);
        }

        [Fact]
        public void EncodeDecodeMultiPolygonTest()
        {
            var processedMultiPolygon = multiPolygon.ToWkt().ToGeoJSONObject<MultiPolygon>();

            Assert.Equal(multiPolygon, processedMultiPolygon);
        }

        [Fact]
        public void EncodeDecodeMultiPolygonZTest()
        {
            var processedMultiPolygonZ = multiPolygonZ.ToWkt().ToGeoJSONObject<MultiPolygon>();

            Assert.Equal(multiPolygonZ, processedMultiPolygonZ);
        }

        [Fact]
        public void EncodeDecodeGeometryCollectionTest()
        {
            var processedGeomCol = geomCollection.ToWkt().ToGeoJSONObject<GeometryCollection>();

            Assert.Equal(geomCollection, processedGeomCol);
        }

        [Fact]
        public void EncodeDecodeGeometryCollectionZTest()
        {
            var processedGeomColZ = geomCollectionZ.ToWkt().ToGeoJSONObject<GeometryCollection>();

            Assert.Equal(geomCollectionZ, processedGeomColZ);
        }
    }
}
