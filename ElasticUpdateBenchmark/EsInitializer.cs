using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ElasticUpdateBenchmark.Model;
using Nest;

namespace ElasticUpdateBenchmark
{
    public class EsInitializer
    {
        private readonly IElasticClient _elasticClient;

        public EsInitializer(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<bool> IsEsReadyAsync()
        {
            try
            {
                bool optimizedExists, notOptimizedExists;
                var response = await _elasticClient.Indices.ExistsAsync(Constants.OptimizedIndexName);
                optimizedExists = response.IsValid && response.Exists;
                
                response = await _elasticClient.Indices.ExistsAsync(Constants.NotOptimizedIndexName);
                notOptimizedExists = response.IsValid && response.Exists;

                return notOptimizedExists && optimizedExists;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public async Task<bool> IsInitializedAsync()
        {
            var response = await _elasticClient.Indices.ExistsAsync(Constants.OptimizedIndexName);
            return response.Exists;
        }
        
        public async Task CreateIndicesAsync()
        {
            var response = await _elasticClient.Indices.CreateAsync(Constants.NotOptimizedIndexName, f => 
                f.Settings(se => se.RefreshInterval(new Time(TimeSpan.FromSeconds(30))))
                .Map<Person>(m => 
                m.Properties(p =>
                    p.Keyword(s => s.Name(n => n.Id))
                        .Keyword(s => s.Name(n => n.Guid))
                        .Keyword(s => s.Name(n => n.FirstName))
                        .Keyword(s => s.Name(n => n.MiddleName))
                        .Keyword(s => s.Name(n => n.LastName))
                        .Object<Address>(s =>
                            s.Name(n => n.Address)
                                .Properties(ap =>
                                    ap.Keyword(w => w.Name(ns => ns.Id))
                                        .Keyword(w => w.Name(ns => ns.Guid))
                                        .Keyword(w => w.Name(ns => ns.City))
                                        .Keyword(w => w.Name(ns => ns.ZipCode))
                                        .Keyword(w => w.Name(ns => ns.HouseNumber))
                                        .Keyword(w => w.Name(ns => ns.HousePostfix))
                                ))
                        .Nested<Product>(s =>
                            s.Name(n => n.Products)
                                .Properties(ap =>
                                    ap.Keyword(w => w.Name(ns => ns.Id))
                                        .Keyword(w => w.Name(ns => ns.Guid))
                                        .Keyword(w => w.Name(ns => ns.Name))
                                        .Date(w => w.Name(ns => ns.Date))
                                ))
                        .Nested<Team>(s =>
                            s.Name(n => n.Teams)
                                .Properties(ap =>
                                    ap.Keyword(w => w.Name(ns => ns.Id))
                                        .Keyword(w => w.Name(ns => ns.Guid))
                                        .Keyword(w => w.Name(ns => ns.Name))
                                ))
            )));

            response = await _elasticClient.Indices.CreateAsync(Constants.OptimizedIndexName, f =>
                f.Settings(se => se.RefreshInterval(new Time(TimeSpan.FromSeconds(30))))
                .Map<Person>(m =>
                m.Properties(p =>
                    p.Keyword(s => s.Name(n => n.Id))
                        .Keyword(s => s.Name(n => n.Guid).Index(false).DocValues(false))
                        .Keyword(s => s.Name(n => n.FirstName).Index(false).DocValues(false))
                        .Keyword(s => s.Name(n => n.MiddleName).Index(false).DocValues(false))
                        .Keyword(s => s.Name(n => n.LastName).Index(false).DocValues(false))
                        .Object<Address>(s =>
                            s.Name(n => n.Address)
                                .Properties(ap =>
                                    ap.Keyword(w => w.Name(ns => ns.Id))
                                        .Keyword(w => w.Name(ns => ns.Guid).Index(false).DocValues(false))
                                        .Keyword(w => w.Name(ns => ns.City).Index(false).DocValues(false))
                                        .Keyword(w => w.Name(ns => ns.ZipCode).Index(false).DocValues(false))
                                        .Keyword(w => w.Name(ns => ns.HouseNumber).Index(false).DocValues(false))
                                        .Keyword(w => w.Name(ns => ns.HousePostfix).Index(false).DocValues(false))
                                ))
                        .Nested<Product>(s =>
                            s.Name(n => n.Products)
                                .Properties(ap =>
                                    ap.Keyword(w => w.Name(ns => ns.Id))
                                        .Keyword(w => w.Name(ns => ns.Guid).Index(false).DocValues(false))
                                        .Keyword(w => w.Name(ns => ns.Name).Index(false).DocValues(false))
                                        .Date(w => w.Name(ns => ns.Date).Index(false).DocValues(false))
                                ))
                        .Nested<Team>(s =>
                            s.Name(n => n.Teams)
                                .Properties(ap =>
                                    ap.Keyword(w => w.Name(ns => ns.Id))
                                        .Keyword(w => w.Name(ns => ns.Guid).Index(false).DocValues(false))
                                        .Keyword(w => w.Name(ns => ns.Name).Index(false).DocValues(false))
                                ))
            )));
            
        }

        public async Task IndexAsync(IEnumerable<Person> batch)
        {
            var response = await _elasticClient.BulkAsync(b =>
                b.Index(Constants.OptimizedIndexName).IndexMany(batch));

            response = await _elasticClient.BulkAsync(b =>
                b.Index(Constants.NotOptimizedIndexName).IndexMany(batch));
        }

        public async Task ChangeRefreshTime()
        {
            await _elasticClient.Indices.UpdateSettingsAsync(Constants.NotOptimizedIndexName,
                f => f.IndexSettings(s => s.RefreshInterval(new Time(1000))));
            
            await _elasticClient.Indices.UpdateSettingsAsync(Constants.OptimizedIndexName,
                f => f.IndexSettings(s => s.RefreshInterval(new Time(250))));
        }
    }
}