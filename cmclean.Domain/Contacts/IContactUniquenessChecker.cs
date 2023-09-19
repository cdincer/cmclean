namespace cmclean.Domain.Contacts
{
    public interface IContactUniquenessChecker
    {
        Task<int> IsUnique(string contactemail);
    }
}