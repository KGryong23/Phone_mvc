namespace Phone_mvc.Entities
{
    public class Phone : BaseDomainEntity
    {
        private string _model = null!;
        public string Model
        {
            get => _model;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Model cannot be null or empty.", nameof(value));
                }
                _model = value;
            }
        }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be negative.");
                }
                _price = value;
            }
        }
        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Stock cannot be negative.");
                }
                _stock = value;
            }
        }
        public Guid? BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}
