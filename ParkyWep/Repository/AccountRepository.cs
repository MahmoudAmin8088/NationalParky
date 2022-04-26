using Newtonsoft.Json;
using ParkyWep.Models;
using ParkyWep.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWep.Repository
{
    public class AccountRepository:Repository<AppUser>,IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<AppUser> LoginAsync(string ur1, AppUser objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ur1);

            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return new AppUser();
            }

            var cleint = _clientFactory.CreateClient();
            HttpResponseMessage response = await cleint.SendAsync(request);

            if (response.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AppUser>(jsonString);
            }
            return new AppUser();

        }

        public async Task<AppUser> RegisterAsync(string ur1, AppUser objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ur1);
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return new AppUser ();
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AppUser>(jsonString);
            }
            return new AppUser();
        }
    }
}
