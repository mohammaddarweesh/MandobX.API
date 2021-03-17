﻿using MandobX.API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandobX.API.Authentication
{
    /// <summary>
    /// Response
    /// </summary>
    public class Response
    {
        /// <summary>
        /// status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// Data from the call
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Response Code
        /// </summary>
        public string Code { get; set; }
    }
}
