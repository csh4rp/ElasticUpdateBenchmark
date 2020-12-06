using System;

namespace ElasticUpdateBenchmark.Model
{
    public class Product
    {
        [Nest.Keyword(Name = "id")]
        public int Id { get; set; }
        
        [Nest.Keyword(Name = "guid")]
        public Guid Guid { get; set; }
        
        [Nest.Keyword(Name = "name")]
        public string Name { get; set; }
        
        [Nest.Keyword(Name = "date")]
        public DateTime Date { get; set; }
        
    }
}