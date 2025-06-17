﻿using Phone_mvc.Common;
using Phone_mvc.Dtos;

namespace Phone_mvc.Services
{
    /// <summary>
    /// Interface cho Brand Service
    /// </summary>
    public interface IBrandService
    {
        Task<PagedResult<BrandDto>> GetPagedAsync(BaseQuery query);
        Task<BrandDto> GetByIdAsync(Guid id);
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<bool> AddAsync(BrandDto dto);
        Task<bool> UpdateAsync(BrandDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> Approve(Guid id);
        Task<bool> Reject(Guid id);
    }
}
