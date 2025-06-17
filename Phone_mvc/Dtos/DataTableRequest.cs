namespace Phone_mvc.Dtos
{
    /// <summary>
    /// Model để nhận dữ liệu từ DataTable
    /// </summary>
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataTableSearch Search { get; set; } = new();
    }
}
