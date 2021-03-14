using MandobX.API.Authentication;
using MandobX.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Services.IService
{
    public interface IIdentityService
    {
        Task<Response> Register(RegisterModel registerModel, string role);
    }
}
