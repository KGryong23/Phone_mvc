using Microsoft.EntityFrameworkCore;
using Phone_mvc.Common;
using Phone_mvc.Data;
using Phone_mvc.Exceptions;
using System.Linq.Expressions;

namespace Phone_mvc.Repositories
{
    /// <summary>
    /// Generic Repository triển khai các thao tác CRUD và phân trang
    /// </summary>
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly PhoneContext _context;
        private readonly DbSet<T> _dbSet;

        protected Repository(PhoneContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        public T GetById(Guid id) => _dbSet.Find(id) ?? throw new NotFoundException("Bản ghi không tồn tại");

        /// <summary>
        /// Lấy bản ghi theo ID (async)
        /// </summary>
        public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id) ?? throw new NotFoundException("Bản ghi không tồn tại");

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        /// <summary>
        /// Thêm bản ghi mới
        /// </summary>
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Lấy danh sách phân trang, tìm kiếm theo tên, sắp xếp theo trường chỉ định
        /// </summary>
        public async Task<PagedResult<T>> GetPagedAsync(BaseQuery query, string searchField)
        {
            var dataQuery = _dbSet.AsQueryable();

            // Đếm tổng số bản ghi trước khi lọc
            int recordsTotal = await dataQuery.CountAsync();

            // Tìm kiếm theo tên (Model hoặc Name)
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                dataQuery = dataQuery.Where(x => EF.Property<string>(x, searchField).ToLower().Contains(query.Keyword.ToLower()));
            }

            // Lọc theo CreatedBy nếu có giá trị
            if (query.CreatedBy.HasValue)
            {
                dataQuery = dataQuery.Where(x => EF.Property<Guid?>(x, "CreatedBy") == query.CreatedBy.Value);
            }

            // Đếm số bản ghi sau khi lọc
            int recordsFiltered = await dataQuery.CountAsync();

            // Sắp xếp
            if (string.IsNullOrEmpty(query.SortField))
            {
                query.SortField = "Id"; // Mặc định nếu SortField rỗng
            }
            if (query.SortDirection?.ToLower() == "asc")
            {
                dataQuery = dataQuery.OrderBy(x => EF.Property<object>(x, query.SortField));
            }
            else
            {
                dataQuery = dataQuery.OrderByDescending(x => EF.Property<object>(x, query.SortField));
            }

            // Phân trang
            dataQuery = dataQuery.Skip(query.Skip).Take(query.Take);

            // Lấy dữ liệu
            var data = await dataQuery.ToListAsync();

            return new PagedResult<T>(data, recordsTotal, recordsFiltered, query.Draw);
        }

        /// <summary>
        /// Hàm lưu các thay đổi vào cơ sở dữ liệu
        /// </summary>
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        /// <summary>
        /// Lấy các bản ghi theo điều kiện
        /// </summary>
        public async Task<IEnumerable<T>> FindAllAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            // Áp dụng các Include
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(predicate).ToListAsync();
        }
        /// <summary>
        /// Lấy bản ghi đầu tiên theo điều kiện.
        /// </summary>
        public async Task<T?> FindFirstAsync(Expression<Func<T, bool>> exp) => await _dbSet.Where(exp).FirstOrDefaultAsync();
    }
}
