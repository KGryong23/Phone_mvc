namespace Phone_mvc.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; } // Đổi tên từ Items thành Data để khớp với DataTable
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public int Draw { get; set; }

        public PagedResult(IEnumerable<T> data, int recordsTotal, int recordsFiltered, int draw)
        {
            Data = data;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
            Draw = draw;
        }
    }
}
