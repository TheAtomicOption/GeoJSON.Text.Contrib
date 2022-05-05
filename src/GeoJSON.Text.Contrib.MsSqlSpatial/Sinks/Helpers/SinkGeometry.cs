using System.Collections.Generic;

namespace GeoJSON.Text.Contrib.MsSqlSpatial.Sinks
{
    internal class SinkGeometry<T> : List<SinkLineRing>
    {
        public T GeometryType { get; set; }

        public SinkGeometry(T geomType)
        {
            GeometryType = geomType;
        }
    }
}
