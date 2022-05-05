using GeoJSON.Text.Contrib.Wkb.Conversions;
using System.Linq;
using Xunit;

#pragma warning disable 618

namespace GeoJSON.Text.Contrib.EntityFramework.Test
{
    public partial class EntityFrameworkConvertTests
    {
        [Fact]
        public void ToDbGeometryValidPointTest()
        {
            var dbPoint = point.ToGeometry();

            Assert.NotNull(point);
            Assert.NotNull(dbPoint);
            Assert.Equal(WkbGeometryType.Point.ToString(), dbPoint.GeometryType);
            Assert.Single(dbPoint.Coordinates);
        }

        [Fact]
        public void ToDbGeometryValidMultiPointTest()
        {
            var dbMultiPoint = multiPoint.ToGeometry();

            Assert.NotNull(multiPoint);
            Assert.NotNull(dbMultiPoint);
            Assert.Equal(WkbGeometryType.MultiPoint.ToString(), dbMultiPoint.GeometryType);
            Assert.Equal(multiPoint.Coordinates.Count, dbMultiPoint.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidLineStringTest()
        {
            var dbLineString = lineString.ToGeometry();

            Assert.NotNull(lineString);
            Assert.NotNull(dbLineString);
            Assert.Equal(WkbGeometryType.LineString.ToString(), dbLineString.GeometryType);
            Assert.Equal(lineString.Coordinates.Count, dbLineString.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidMultiLineStringTest()
        {
            var dbMultiLineString = multiLineString.ToGeometry();

            Assert.NotNull(multiLineString);
            Assert.NotNull(dbMultiLineString);
            Assert.Equal(WkbGeometryType.MultiLineString.ToString(), dbMultiLineString.GeometryType);
            Assert.Equal(dbMultiLineString.Coordinates.Count(), multiLineString.Coordinates.Count);
            Assert.Equal(multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbMultiLineString.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidPolygonTest()
        {
            var dbPolygon = polygon.ToGeometry();

            Assert.NotNull(polygon);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.GeometryType);
            Assert.Equal(polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidPolygonWithHoleTest()
        {
            var dbPolygon = polygonWithHole.ToGeometry();

            Assert.NotNull(polygonWithHole);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.GeometryType);
            Assert.Equal(polygonWithHole.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidPolygonWithHoleReverseWindingTest()
        {
            var dbPolygon = polygonWithHoleReverseWinding.ToGeometry();

            Assert.NotNull(polygonWithHoleReverseWinding);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.GeometryType);
            Assert.Equal(polygonWithHoleReverseWinding.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidMultiPolygonTest()
        {
            var dbMultiPolygon = multiPolygon.ToGeometry();

            Assert.NotNull(multiPolygon);
            Assert.NotNull(dbMultiPolygon);
            Assert.Equal(WkbGeometryType.MultiPolygon.ToString(), dbMultiPolygon.GeometryType);
            Assert.Equal(multiPolygon.Coordinates.Count, dbMultiPolygon.Coordinates.Length);
            Assert.Equal(multiPolygon.Coordinates.SelectMany(p => p.Coordinates).SelectMany(ls => ls.Coordinates).Count(), dbMultiPolygon.Coordinates.Count());
        }

        [Fact]
        public void ToDbGeometryValidGeometryCollectionTest()
        {
            var dbGeomCol = geomCollection.ToGeometry();

            Assert.NotNull(geomCollection);
            Assert.NotNull(dbGeomCol);
            Assert.Equal(WkbGeometryType.GeometryCollection.ToString(), dbGeomCol.GeometryType);
            Assert.Equal(geomCollection.Geometries.Count, dbGeomCol.Coordinates.Count());
        }
    }
}
