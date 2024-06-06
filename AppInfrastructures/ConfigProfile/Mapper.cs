using AppDatabase.Models;
using AppInfrastructures.DefaultServices.UserServices.Dto;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInfrastructures.ConfigProfile
{
    public static class Mapper
    {
        public static void Configure()
        {
            TinyMapper.Bind<CreateUserDto, Users>();
        }
    }
}
