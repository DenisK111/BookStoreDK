namespace BookStoreDK.DL.Intefraces
{
    public interface IBaseRepository<T,TId>
    {
        T? Add(T model);
        T? Delete(TId modelId);
        IEnumerable<T> GetAll();
        T? GetById(TId id);
        T? Update(T model);
    }
}
