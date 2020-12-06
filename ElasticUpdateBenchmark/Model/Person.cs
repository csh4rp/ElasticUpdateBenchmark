using System;
using System.Collections.Generic;

namespace ElasticUpdateBenchmark.Model
{
    public class Person
    {
        [Nest.Keyword(Name = "id")]
        public int Id { get; set; }
        
        [Nest.Keyword(Name = "guid")]
        public Guid Guid { get; set; }
        
        [Nest.Keyword(Name = "first_name")]
        public string FirstName { get; set; }
        
        [Nest.Keyword(Name = "middle_name")]
        public string MiddleName { get; set; }
        
        [Nest.Keyword(Name = "last_name")]
        public string LastName { get; set; }
        
        [Nest.Object(Name = "address")]
        public Address Address { get; set; }
        
        [Nest.Object(Name = "products")]
        public List<Product> Products { get; set; }
        
        [Nest.Object(Name = "teams")]
        public List<Team> Teams { get; set; }

        public Person()
        {
            Products = new List<Product>();
            Teams = new List<Team>();
        }
        
    }
}