namespace cmclean.Domain.Repositories
{
    public interface IRepositoryWrapper
    {
        IContactRepository ContactRepository { get; }
        Task Save();
    }
}