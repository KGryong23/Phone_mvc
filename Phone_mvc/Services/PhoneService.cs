using Phone_mvc.Common;
using Phone_mvc.Dtos;
using Phone_mvc.Enums;
using Phone_mvc.Extensions;
using Phone_mvc.Repositories;

namespace Phone_mvc.Services
{
    /// <summary>
    /// Service xử lý logic nghiệp vụ cho Phone
    /// </summary>
    public class PhoneService : IPhoneService
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public PhoneService(IPhoneRepository phoneRepository, IBrandRepository brandRepository, IUserService userService, IRoleService roleService)
        {
            _phoneRepository = phoneRepository;
            _brandRepository = brandRepository;
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Lấy danh sách Phone phân trang, tìm kiếm theo Model, sắp xếp theo Price
        /// </summary>
        public async Task<PagedResult<PhoneDto>> GetPagedAsync(BaseQuery query, Guid? userId)
        {
            if (userId is null)
                throw new UnauthorizedAccessException("Không có quyền thực hiện điều này!");
            bool isAuthorized = await IsUserAuthorizedAsync(userId.Value);
            if (!isAuthorized)
            {
                query.CreatedBy = userId.Value; // Chỉ lấy dữ liệu của người dùng hiện tại
            }
            var result = await _phoneRepository.GetPagedAsync(query, "Model");
            var phones = result.Data.Select(p => new PhoneDto
            {
                Id = p.Id,
                Model = p.Model,
                Price = p.Price,
                Stock = p.Stock,
                Created = p.Created,
                LastModified = p.LastModified,
                ModerationStatus = p.ModerationStatus,
                ModerationStatusTxt = p.ModerationStatus switch
                {
                    ModerationStatus.Approved => AppConstants.Approved,
                    ModerationStatus.Rejected => AppConstants.Rejected,
                    _ => AppConstants.NotDetermined
                },
                BrandId = p.BrandId,
                BrandName = p.BrandId.HasValue ? (_brandRepository.GetById(p.BrandId.Value))?.Name ?? "N/A" : "N/A"
            }).ToList();
            return new PagedResult<PhoneDto>(phones, result.RecordsTotal, result.RecordsFiltered, result.Draw);
        }

        /// <summary>
        /// Lấy Phone theo ID
        /// </summary>
        public async Task<PhoneDto> GetByIdAsync(Guid id, Guid? userId)
        {
            if (userId is null)
                throw new UnauthorizedAccessException("Không có quyền thực hiện điều này!");
            bool isAuthorized = await IsUserAuthorizedAsync(userId.Value);
            var phone = await _phoneRepository.GetByIdAsync(id);
            if (isAuthorized || phone.CreatedBy == userId.Value) return phone.ToDto(); // Trả về thông tin đầy đủ nếu người dùng có quyền
            return new PhoneDto();
        }

        /// <summary>
        /// Thêm Phone mới với kiểm tra đầu vào
        /// </summary>
        public async Task<bool> AddAsync(CreatePhoneRequest request)
        {
            if (request.BrandId.HasValue)
            {
                var brand = await _brandRepository.GetByIdAsync(request.BrandId.Value);
                if (brand is null)
                    throw new ArgumentException(AppResources.InvalidBrand, nameof(request.BrandId));
            }

            var existingPhones = await _phoneRepository.FindAllAsync(x => x.Model == request.Model);
            if (existingPhones.Any())
                throw new ArgumentException(AppResources.DuplicateModel, nameof(request.Model));

            await _phoneRepository.AddAsync(request.ToEntity());
            return await _phoneRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật Phone với kiểm tra đầu vào
        /// </summary>
        public async Task<bool> UpdateAsync(Guid id, UpdatePhoneRequest request)
        {
            if (request.BrandId.HasValue)
            {
                var brand = await _brandRepository.GetByIdAsync(request.BrandId.Value);
                if (brand is null)
                    throw new ArgumentException(AppResources.InvalidBrand, nameof(request.BrandId));
            }

            var existingPhones = await _phoneRepository.FindAllAsync(x => x.Model == request.Model && x.Id != id);
            if (existingPhones.Any())
                throw new ArgumentException(AppResources.DuplicateModel, nameof(request.Model));

            var phone = await _phoneRepository.GetByIdAsync(id);
            if (phone is null)
                throw new ArgumentException(AppResources.PhoneNotFound, nameof(id));

            phone.Model = request.Model;
            phone.Price = request.Price;
            phone.Stock = request.Stock;
            phone.BrandId = request.BrandId;

            _phoneRepository.Update(phone);
            return await _phoneRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Xóa Phone
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var phone = await _phoneRepository.GetByIdAsync(id);
            _phoneRepository.Delete(phone);
            return await _phoneRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Duyệt Phone
        /// </summary>
        public async Task<bool> Approve(Guid id)
        {
            var phone = await _phoneRepository.GetByIdAsync(id);
            if (phone is null)
                return false;

            phone.ModerationStatus = ModerationStatus.Approved;

            _phoneRepository.Update(phone);
            return await _phoneRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Từ chối Phone
        /// </summary>
        public async Task<bool> Reject(Guid id)
        {
            var phone = await _phoneRepository.GetByIdAsync(id);
            if (phone is null)
                return false;

            phone.ModerationStatus = ModerationStatus.Rejected;
            _phoneRepository.Update(phone);

            return await _phoneRepository.SaveChangesAsync();
        }
        private async Task<bool> IsUserAuthorizedAsync(Guid userId)
        {
            var userIds = await _userService.GetUserRoleIdsAsync(userId);
            var roleIds = await _roleService.GetRoleIdsWithData();
            foreach (var userRoleId in userIds)
            {
                if (roleIds.Contains(userRoleId))
                    return true;
            }
            return false;
        }
    }
}
