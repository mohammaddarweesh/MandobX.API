using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Services.IService
{
    public interface IDriverService
    {
        bool BlockDriver(string Id);
        bool UnBlockDriver(string Id);
    }
}
