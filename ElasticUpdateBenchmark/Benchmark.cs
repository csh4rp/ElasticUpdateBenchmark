using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ElasticUpdateBenchmark.Model;
using Nest;

namespace ElasticUpdateBenchmark
{
    public class Benchmark
    {
        private const int BatchSize = 10000;
        private const int Iterations = 1000;
        private const int BaseId = 1234;
        private readonly IElasticClient _elasticClient = new ElasticClient(new Uri("http://localhost:9200"));
        private readonly DataGenerator _dataGenerator = new DataGenerator();
        
        [GlobalSetup]
        public async Task Init()
        {
            var initializer = new EsInitializer(_elasticClient);

            while (!await initializer.IsEsReadyAsync())
            {
                await Task.Delay(1000);
            }

            Console.WriteLine("Already initialized.");
            if (await initializer.IsInitializedAsync())
            {
                return;
            }

            Console.WriteLine("Creating indices...");
            await initializer.CreateIndicesAsync();

            Console.WriteLine("Indexing data");
            for (var i = 0; i < 100; i++)
            {
                var batch = _dataGenerator.CreateBatch(BatchSize).ToList();
                await initializer.IndexAsync(batch);
                Console.WriteLine($"{i}% indexing done...");
            }

            await initializer.ChangeRefreshTime();
            Console.WriteLine("Indexing finished.");
        }
        
        [Benchmark(Description = "Optimized model")]
        public async Task UpdateOptimizedModel()
        {
            var tasks = new List<Task>(Iterations);
            for (var i = 0; i < Iterations; i++)
            {
                var id = BaseId + i;
                var parameters = new Dictionary<string, object> {{"house_number", i}};
                var responseTask = _elasticClient.UpdateByQueryAsync<Person>(s => s.Index(Constants.OptimizedIndexName)
                    .Query(q =>
                        q.Bool(b =>
                            b.Filter(fi =>
                                fi.Term(t =>
                                    t.Field(f => f.Address.Id).Value(id))
                            )
                        )
                    ).Script(script => script.Source("ctx._source.house_number = house_number;").Params(parameters))
                );
                
                tasks.Add(responseTask);
            }

            await Task.WhenAll(tasks);
        }
        
        [Benchmark(Description = "Not optimized model")]
        public async Task UpdateNotOptimizedModel()
        {
            var tasks = new List<Task>(Iterations);
            for (var i = 0; i < Iterations; i++)
            {
                var id = BaseId + i;
                var parameters = new Dictionary<string, object> {{"house_number", i}};
                var responseTask = _elasticClient.UpdateByQueryAsync<Person>(s => s.Index(Constants.NotOptimizedIndexName)
                    .Query(q =>
                        q.Bool(b =>
                            b.Filter(fi =>
                                fi.Term(t =>
                                    t.Field(f => f.Address.Id).Value(id))
                            )
                        )
                    ).Script(script => script.Source("ctx._source.house_number = house_number;").Params(parameters))
                    .Refresh(true)
                );
                
                tasks.Add(responseTask);
            }

            await Task.WhenAll(tasks);
        }
        
        [Benchmark(Description = "Not optimized model - no refresh")]
        public async Task UpdateNotOptimizedModelNoRefresh()
        {
            var tasks = new List<Task>(Iterations);
            for (var i = 0; i < Iterations; i++)
            {
                var id = BaseId + i;
                var parameters = new Dictionary<string, object> {{"house_number", i}};
                var responseTask = _elasticClient.UpdateByQueryAsync<Person>(s => s.Index(Constants.NotOptimizedIndexName)
                    .Query(q =>
                        q.Bool(b =>
                            b.Filter(fi =>
                                fi.Term(t =>
                                    t.Field(f => f.Address.Id).Value(id))
                            )
                        )
                    ).Script(script => script.Source("ctx._source.house_number = house_number;").Params(parameters))
                );
                
                tasks.Add(responseTask);
            }

            await Task.WhenAll(tasks);
        }
        
    }
}