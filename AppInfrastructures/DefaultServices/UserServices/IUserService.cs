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
        Task<(bool, string)> Create(CreateUserDto model);
        Task<(bool, string)> Update(UpdateUserDto model);
        Task<(bool, string)> Delete(int id);
        Task<PagedResult<UserListDto>> GetAllUsers(PageInfo pageinfo);
        Task<UpdateUserDto> GetUserByCode(string code);
        Task<PagedResult<UserListDto>> SearchUser(string searchString, PageInfo pageinfo);
    }
}
