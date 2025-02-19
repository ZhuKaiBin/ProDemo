namespace ElasticSearchSln.Domain
{
    public interface IVisitLogRepository
    {
        Task InsertAsync(VisitLog visitLog);

        Task DeleteAsync(string id);

        Task UpdateAsync(VisitLog visitLog);

        Task<Tuple<int, IList<VisitLog>>> QueryAsync(int page, int limit);
    }
}
