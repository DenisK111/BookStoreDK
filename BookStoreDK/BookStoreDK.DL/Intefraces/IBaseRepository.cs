namespace BookStoreDK.DL.Intefraces
{
    public interface IBaseRepository<T,TInt>
    {
        T? Add(T model);
        T? Delete(TInt modelId);
        IEnumerable<T> GetAll();
        T? GetById(TInt id);
        T? Update(T model);
    }
}
