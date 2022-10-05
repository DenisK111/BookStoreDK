namespace BookStoreDK.DL.Intefraces
{
    public interface IBaseRepository<T,TId>
    {
        Task<T?> Add(T model);
        Task<T?> Delete(TId modelId);
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(TId id);
        Task<T?> Update(T model);
    }
}
