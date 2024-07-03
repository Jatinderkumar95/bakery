using bakery.Models;
using Cassandra.Mapping;

namespace bakery.CassandraMappings;

public class CassandraMapping : Mappings{
    public CassandraMapping()
    {
        For<ColorSwatch>().TableName("colorswatch")
        .PartitionKey(x => x.PageKey);
    }
}