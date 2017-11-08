namespace ForeignExchange
{
    using System.Threading.Tasks;
    using Plugin.Connectivity;
    using System.Net.Http;
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ApiService
    {
        
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = "Check your Internet settings."
                };
            }


            //var response = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            //if (!response)
            //{
            //    return new Response
            //    {
            //        IsSucess = false,
            //        Message = "Check your Internet connection."
            //    };
            //}
            return new Response
            {
                IsSucess = true,

            };
        }
    

        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result,

                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSucess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = ex.Message,
                };
                
            }
        }
    }
}
