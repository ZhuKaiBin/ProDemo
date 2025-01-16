using Nest;


namespace ElasticSearchSln.Services
{

    public interface IElasticsearchProvider
    {
        IElasticClient GetClient();
    }


    /// <summary>
    /// 连接ElasticSearch本地的客户端
    /// </summary>
    public class ElasticsearchProvider : IElasticsearchProvider
    {
        public IElasticClient GetClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"));

            return new ElasticClient(connectionSettings);
        }
    }
}
