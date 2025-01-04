namespace Dash.Services;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(int id, T entity);
    Task DeleteAsync(int id);
    /*Task<PaginationViewModel<T>> GetPaginatedAsync(int pageNumber, int pageSize);*/
}