using AppInfrastructures.DefaultServices.UserServices.Dto;
using AppInfrastructures.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInfrastructures.DefaultServices.UserServices
{
    public interface IUserService
    {
        Task Create(CreateUserDto model);
        void Update(UpdateUserDto model);
        void Delete(int id);
        PagedResult<UserListDto> GetAllUsers(PageInfo pageinfo);
        UpdateUserDto GetUserByCode(string code);
        PagedResult<UserListDto> SearchUser(string searchString, PageInfo pageinfo);
    }
}
