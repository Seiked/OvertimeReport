using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Contracts.Identity;
using Core.DBContext;
using Core.Models.Identity;

namespace Core.Services
{
    public class UserService : IUserService
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserId { get => _contextAccessor.HttpContext?.User?.FindFirstValue("uid"); }
        public string SolId { get => _contextAccessor.HttpContext?.User?.FindFirstValue("solId"); }
        public string Name { get => _contextAccessor.HttpContext?.User?.FindFirstValue("Name"); }


    }
}