using AppDatabase.Databases;
using AppDatabase.Models;
using AppInfrastructures.ConfigProfile;
using AppInfrastructures.DefaultServices.UserServices.Dto;
using AppInfrastructures.Helpers;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInfrastructures.DefaultServices.UserServices
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            Mapper.Configure();
        }

        public async Task Create(CreateUserDto model)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var users = TinyMapper.Map<Users>(model);
                await _appDbContext.Users.AddAsync(users);
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _appDbContext.Users.FindAsync(id);
                if (user == null)
                {
                    throw new Exception("Product not found");
                }

                _appDbContext.Users.Remove(user);
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PagedResult<UserListDto>> GetAllUsers(PageInfo pageinfo)
        {
            var pagedResult = new PagedResult<UserListDto>
            {
                Data = (from user in _appDbContext.Users
                        select new UserListDto
                        {
                            UserCode = user.UserCode,
                            UserName = user.UserName
                        })
                        .Skip(pageinfo.Skip)
                        .Take(pageinfo.PageSize)
                        .OrderBy(w => w.UserCode),
                Total = await _appDbContext.Users.CountAsync()
            };

            return pagedResult;
        }

        public async Task<UpdateUserDto> GetUserByCode(string code)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(w => w.UserCode == code);
            if (user == null)
            {
                return null;
            }

            var productDto = TinyMapper.Map<UpdateUserDto>(user);
            return productDto;
        }

        public async  Task<PagedResult<UserListDto>> SearchUser(string searchString, PageInfo pageInfo)
        {
            var users = _appDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString)
                                        || s.UserCode.Contains(searchString));
            }

            var query = from user in users
                        select new UserListDto
                        {
                            UserCode = user.UserCode,
                            UserName = user.UserName
                        };

            var pagedData = await query
                .Skip(pageInfo.Skip)
                .Take(pageInfo.PageSize)
                .OrderBy(w => w.UserCode)
                .ToListAsync();

            var total = await users.CountAsync();

            var pagedResult = new PagedResult<UserListDto>
            {
                Data = pagedData,
                Total = total
            };

            return pagedResult;
        }

        public async Task Update(UpdateUserDto model)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var users = TinyMapper.Map<Users>(model);
                _appDbContext.Entry(users).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
