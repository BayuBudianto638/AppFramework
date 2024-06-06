using AppInfrastructures.DefaultServices.UserServices;
using AppInfrastructures.DefaultServices.UserServices.Dto;
using AppInfrastructures.Helpers;
using AppInfrastructures.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUser")]
        [Produces("application/json")]
        [Authorize] // diperlukan otorisasi ketika ingin mengakses si API
        public async Task<IActionResult> GetAllUser([FromQuery] PageInfo pageinfo)
        {
            //// FromBody tidak bisa di gunakan untuk method HttpGet
            //// Ada 2 cara untuk bisa mengirim parameter ke HttpGet
            //// 1. Deklarasi variable 1 per 1
            //// 2. Gunakan FormQuery
            try
            {
                var productList = await _userService.GetAllUsers(pageinfo);
                if (productList.Data.Count() < 1)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Not Found");
                }
                return Requests.Response(this, new ApiStatus(200), productList, "");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message); // not found
            }
        }

        [HttpPost("SaveUser")]
        [Authorize]
        public async Task<IActionResult> SaveUser([FromBody] CreateUserDto model)
        {
            try
            {
                var (isAdded, isMessage) = await _userService.Create(model);
                if (!isAdded)
                {
                    return Requests.Response(this, new ApiStatus(406), isMessage, "Error");
                }

                return Requests.Response(this, new ApiStatus(200), isMessage, "Success");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpGet("GetUserByCode")]
        [Authorize]
        public async Task<IActionResult> GetUserByCode(string code)
        {
            try
            {
                var data = await _userService.GetUserByCode(code);
                if (data == null)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Not Found");
                }

                return Requests.Response(this, new ApiStatus(200), data, "");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(404), null, ex.Message); // not found
            }
        }

        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                var (isDeleted, isMessage) = await _userService.Delete(Id);
                if (!isDeleted)
                {
                    return Requests.Response(this, new ApiStatus(406), isMessage, "Error");
                }

                return Requests.Response(this, new ApiStatus(200), isMessage, "Success");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpPatch("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto model)
        {
            try
            {
                var (isUpdated, isMessage) = await _userService.Update(model);
                if (!isUpdated)
                {
                    return Requests.Response(this, new ApiStatus(406), isMessage, "Error");
                }

                return Requests.Response(this, new ApiStatus(200), isMessage, "Success");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(500), null, ex.Message);
            }
        }

        [HttpGet("SearchUser")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchUser(string searchString, [FromQuery] PageInfo pageInfo)
        {
            try
            {
                var data = await _userService.SearchUser(searchString, pageInfo);
                if (data.Data.Count() < 1)
                {
                    return Requests.Response(this, new ApiStatus(404), null, "Not Found");
                }

                return Requests.Response(this, new ApiStatus(200), data, "");
            }
            catch (Exception ex)
            {
                return Requests.Response(this, new ApiStatus(404), null, ex.Message); // not found
            }
        }
    }
}
