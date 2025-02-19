using ElasticSearchSln.Services;
using Nest;

namespace ElasticSearchSln.Models
{
    public abstract class ElasticsearchRepositoryBase
    {
        private readonly IElasticsearchProvider _elasticsearchProvider;

        public ElasticsearchRepositoryBase(IElasticsearchProvider elasticsearchProvider)
        {
            _elasticsearchProvider = elasticsearchProvider;
        }

        protected IElasticClient Client => _elasticsearchProvider.GetClient();

        protected abstract string IndexName { get; }
    }
}
