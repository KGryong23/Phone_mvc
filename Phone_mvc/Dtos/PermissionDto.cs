namespace Phone_mvc.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string? Endpoint { get; set; }
        public string? Method { get; set; }
    }
}
