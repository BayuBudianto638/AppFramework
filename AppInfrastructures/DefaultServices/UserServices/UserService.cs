using AppDatabase.Databases;
using AppDatabase.Models;
using AppInfrastructures.ConfigProfile;
using AppInfrastructures.DefaultServices.UserServices.Dto;
using AppInfrastructures.Helpers;
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

        public PagedResult<UserListDto> GetAllUsers(PageInfo pageinfo)
        {
            throw new NotImplementedException();
        }

        public UpdateUserDto GetUserByCode(string code)
        {
            throw new NotImplementedException();
        }

        public PagedResult<UserListDto> SearchUser(string searchString, PageInfo pageinfo)
        {
            throw new NotImplementedException();
        }

        public void Update(UpdateUserDto model)
        {
            throw new NotImplementedException();
        }
    }
}
