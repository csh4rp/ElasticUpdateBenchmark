using System;
using System.Collections.Generic;
using System.Linq;
using ElasticUpdateBenchmark.Model;

namespace ElasticUpdateBenchmark
{
    public class DataGenerator
    {
        private const string Characters = "abcdefghijklmnopqrstuwxyz";
        private static readonly Random Random = new Random(50);
        private static readonly DateTime BaseDate = DateTime.Today.AddYears(-1);
        private int _currentId = 1;
        private int _currentAddressId = 1;
        private int _currentProductId = 1;
        private  int _currentTeamId = 1;
        
        public IEnumerable<Person> CreateBatch(int batchSize)
        {
            for (var i = 0; i < batchSize; i++)
            {
                yield return new Person
                {
                    Id = _currentId++,
                    Guid = Guid.NewGuid(),
                    FirstName = GetRandomString(5, 15),
                    MiddleName = GetRandomString(3, 5),
                    LastName = GetRandomString(5, 15),
                    Address = GetRandomAddress(),
                    Products = GetRandomProducts().ToList(),
                    Teams = GetRandomTeams().ToList()
                };
            }
        }

        private Address GetRandomAddress()
        {
            return new Address
            {
                Id = _currentAddressId++,
                Guid = Guid.NewGuid(),
                City = GetRandomString(5, 10),
                HouseNumber = GetRandomNumber(1, 100),
                HousePostfix = GetRandomString(1, 1),
                StreetName = GetRandomString(5, 20),
                ZipCode = GetRandomZipCode()
            };
        }

        private IEnumerable<Product> GetRandomProducts()
        {
            var number = GetRandomNumber(1, 10);
            for (var i = 0; i < number; i++)
            {
                yield return new Product
                {
                    Id = _currentProductId++,
                    Guid = Guid.NewGuid(),
                    Name = GetProductName(),
                    Date = BaseDate.AddMinutes(GetRandomNumber(0, 1000000))
                };
            }
        }
        
        private IEnumerable<Team> GetRandomTeams()
        {
            var number = GetRandomNumber(1, 10);
            for (var i = 0; i < number; i++)
            {
                yield return new Team
                {
                    Id = _currentTeamId++,
                    Guid = Guid.NewGuid(),
                    Name = GetTeamName(),
                };
            }
        }
        
        private static string GetRandomString(int minLength, int maxLength)
        {
            var length = Random.Next(minLength, maxLength);
            return new string(Enumerable.Repeat(Characters, length)
                .Select((s, i) => i == 0
                    ? char.ToUpper(s[Random.Next(s.Length)])
                    : s[Random.Next(s.Length)]).ToArray());
        }

        private static string GetRandomZipCode()
        {
            return $"{GetRandomNumber(10, 99)}-{GetRandomNumber(100, 999)}";
        }

        private static int GetRandomNumber(int min, int max)
        {
            return Random.Next(min, max);
        }

        private static string GetProductName()
        {
            var names = new []
            {
                "Product_1",
                "Product_2",
                "Product_3",
                "Product_4",
                "Product_5",
                "Product_6",
                "Product_7",
                "Product_8",
                "Product_9",
                "Product_10",
            };

            return names[GetRandomNumber(0, 9)];
        }
        
        private static string GetTeamName()
        {
            var names = new []
            {
                "Team_1",
                "Team_2",
                "Team_3",
                "Team_4",
                "Team_5",
                "Team_6",
                "Team_7",
                "Team_8",
                "Team_9",
                "Team_10",
                "Team_11",
                "Team_12",
                "Team_13",
                "Team_14",
                "Team_15",
                "Team_16",
                "Team_17",
                "Team_18",
                "Team_19",
                "Team_20",

            };

            return names[GetRandomNumber(0, 19)];
        }
    }
}