namespace BookStoreDK.DL.Intefraces
{
    public interface IBaseRepository<T>
    {
        T? Add(T model);
        T? Delete(int modelId);
        IEnumerable<T> GetAll();
        T? GetById(int id);
        T? Update(T model);
    }
}
