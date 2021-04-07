﻿using MandobX.API.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MandobX.API.Services.Service
{
    /// <summary>
    /// Message service
    /// </summary>
    public class MessageService : IMessageService
    {
        static string UserName = "asimapi";
        static string Password = "as12345sim";
        static Uri BaseUri = new Uri("https://api-server3.com/api/send.aspx");
        /// <summary>
        /// Send message to user using sms
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(string PhoneNumber, string Msg)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = BaseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync($"?username={UserName}&password={Password}&language=2&sender=MOLATCOM&mobile={PhoneNumber}&message={Msg}");
            var a = response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }
    }
}