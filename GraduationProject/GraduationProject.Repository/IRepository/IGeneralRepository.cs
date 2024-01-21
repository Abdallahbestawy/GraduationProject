namespace GraduationProject.Repository.IRepository
{
    public interface IGeneralRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
    }
}
