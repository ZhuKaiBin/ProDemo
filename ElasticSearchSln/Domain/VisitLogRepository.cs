using Elastic.Clients.Elasticsearch.Nodes;
using ElasticSearchSln.Models;
using ElasticSearchSln.Services;

namespace ElasticSearchSln.Domain
{
    public class VisitLogRepository : ElasticsearchRepositoryBase, IVisitLogRepository
    {
        public VisitLogRepository(IElasticsearchProvider elasticsearchProvider) : base(elasticsearchProvider)
        {
        }

        protected override string IndexName => "visitlogs";

        public async Task InsertAsync(VisitLog visitLog)
        {
            await Client.IndexAsync(visitLog, x => x.Index(IndexName));
        }

        public async Task DeleteAsync(string id)
        {
            await Client.DeleteAsync<VisitLog>(id, x => x.Index(IndexName));
        }

        public async Task UpdateAsync(VisitLog visitLog)
        {
            await Client.UpdateAsync<VisitLog>(visitLog.Id, x => x.Index(IndexName).Doc(visitLog));
        }

        public async Task<Tuple<int, IList<VisitLog>>> QueryAsync(int page, int limit)
        {
            var query = await Client.SearchAsync<VisitLog>(x => x.Index(IndexName)
                                    .From((page - 1) * limit)
                                    .Size(limit)
                                    .Sort(x => x.Descending(v => v.CreatedAt)));
            return new Tuple<int, IList<VisitLog>>(Convert.ToInt32(query.Total), query.Documents.ToList());
        }
    }
}
