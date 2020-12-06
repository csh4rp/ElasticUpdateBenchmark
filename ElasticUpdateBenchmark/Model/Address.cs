using System;

namespace ElasticUpdateBenchmark.Model
{
    public class Address
    {
        [Nest.Keyword(Name = "id")]
        public int Id { get; set; }
        
        [Nest.Keyword(Name = "guid")]
        public Guid Guid { get; set; }
        
        [Nest.Keyword(Name = "city")]
        public string City { get; set; }
        
        [Nest.Keyword(Name = "street_name")]
        public string StreetName { get; set; }
        
        [Nest.Keyword(Name = "zip_code")]
        public string ZipCode { get; set; }
        
        [Nest.Keyword(Name = "house_number")]
        public int HouseNumber { get; set; }
        
        [Nest.Keyword(Name = "house_postfix")]
        public string HousePostfix { get; set; }
        
    }
}