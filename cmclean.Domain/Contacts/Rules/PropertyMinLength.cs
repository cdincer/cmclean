using cmclean.Domain.SeedWork;

namespace cmclean.Contacts.Rules
{
    public class PropertyMinLength : IBusinessRule
    {
        private string _property;

        public PropertyMinLength(string Property)
        {
            _property = Property;
        }
        public string Message => "This property length must be longer than 2 characters";

        public bool IsBroken()
        {
            return _property.Length <= 2;
        }
    }
}