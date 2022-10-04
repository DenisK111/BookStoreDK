namespace BookStoreDK.Models
{
    public abstract record BaseIdModel<T>
    {
        public T Id { get; set; }
    }
}
