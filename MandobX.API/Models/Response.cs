using MandobX.API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Authentication
{
    public class Response
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
        public string Code { get; set; }
    }
}
