using System;

namespace ElasticUpdateBenchmark.Model
{
    public class Team
    {
        [Nest.Keyword(Name = "id")]
        public int Id { get; set; }
        
        [Nest.Keyword(Name = "guid")]
        public Guid Guid { get; set; }
        
        [Nest.Keyword(Name = "name")]
        public string Name { get; set; }
    }
}