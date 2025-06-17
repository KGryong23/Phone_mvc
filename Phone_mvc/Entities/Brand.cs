namespace Phone_mvc.Entities
{
    public class Brand : BaseDomainEntity
    {
        private string _name = null!;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or empty.", nameof(value));
                }
                _name = value;
            }
        }
    }
}
