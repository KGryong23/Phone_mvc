﻿using Phone_mvc.Common;
using Phone_mvc.Dtos;
using Phone_mvc.Entities;
using Phone_mvc.Enums;

namespace Phone_mvc.Extensions
{
    public static class MappingExtensions
    {
        /// <summary>
        /// Ánh xạ Phone thành PhoneDto
        /// </summary>
        public static PhoneDto ToDto(this Phone phone)
        {
            return new PhoneDto
            {
                Id = phone.Id,
                Model = phone.Model,
                Price = phone.Price,
                Stock = phone.Stock,
                Created = phone.Created,
                LastModified = phone.LastModified,
                ModerationStatus = phone.ModerationStatus,
                ModerationStatusTxt = phone.ModerationStatus switch
                {
                    ModerationStatus.Approved => "Đã duyệt",
                    ModerationStatus.Rejected => "Chưa duyệt",
                    _ => "Không xác định"
                },
                BrandId = phone.BrandId,
                BrandName = phone.Brand?.Name ?? "N/A"
            };
        }

        /// <summary>
        /// Ánh xạ PhoneDto thành Phone
        /// </summary>
        public static Phone ToEntity(this PhoneDto dto)
        {
            return new Phone
            {
                Model = dto.Model,
                Price = dto.Price,
                Stock = dto.Stock,
                BrandId = dto.BrandId
            };
        }

        /// <summary>
        /// Ánh xạ CreatePhoneRequest thành Phone
        /// </summary>
        public static Phone ToEntity(this CreatePhoneRequest request)
        {
            return new Phone
            {
                Model = request.Model,
                Price = request.Price,
                Stock = request.Stock,
                BrandId = request.BrandId,
                CreatedBy = request.CreatedBy
            };
        }

        /// <summary>
        /// Ánh xạ UpdatePhoneRequest thành Phone
        /// </summary>
        public static Phone ToEntity(this UpdatePhoneRequest request)
        {
            return new Phone
            {
                Model = request.Model,
                Price = request.Price,
                Stock = request.Stock,
                BrandId = request.BrandId
            };
        }

        /// <summary>
        /// Ánh xạ Brand thành BrandDto
        /// </summary>
        public static BrandDto ToDto(this Brand brand)
        {
            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Created = brand.Created,
                LastModified = brand.LastModified,
                ModerationStatus = brand.ModerationStatus
            };
        }

        /// <summary>
        /// Ánh xạ BrandDto thành Brand
        /// </summary>
        public static Brand ToEntity(this BrandDto dto)
        {
            return new Brand
            {
                Name = dto.Name,
                ModerationStatus = dto.ModerationStatus
            };
        }

        /// <summary>
        /// Ánh xạ User thành User
        /// </summary>
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ModerationStatusTxt = user.ModerationStatus switch
                {
                    ModerationStatus.Approved => AppConstants.Approved,
                    ModerationStatus.Rejected => AppConstants.Rejected,
                    _ => AppConstants.NotDetermined
                },
                Created = user.Created,
                LastModified = user.LastModified,
                ModerationStatus = user.ModerationStatus
            };
        }
    }
}
