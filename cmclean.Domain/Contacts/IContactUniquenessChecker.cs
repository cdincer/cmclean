namespace cmclean.Domain.Contacts
{
    public interface IContactUniquenessChecker
    {
        Task<bool> IsUnique(string contactemail);
    }
}