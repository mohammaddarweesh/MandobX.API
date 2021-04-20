using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Services.IService
{
    public interface IMessageService
    {
        Task<int> SendMessage(string PhoneNumber, string Msg);
    }
}
