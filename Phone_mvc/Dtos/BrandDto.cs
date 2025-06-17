using Phone_mvc.Enums;

namespace Phone_mvc.Dtos
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public ModerationStatus ModerationStatus { get; set; }
    }
    public class BrandView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
