using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using ClientSideApp.Models;
using ClientSideApp.ViewModels;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;

namespace ClientSideApp.Services
{
    public interface IPropertyService
    {
        string GetProperties();

        string GetPropertyById(int id);
        void CreateProperty(List<KeyValuePair<string, string>> keyValuePairs);
        void DeletePropertyById(int id);
        void UpdateProperty(List<KeyValuePair<string, string>> keyValuePairs);
    }

    public class PropertyService : IPropertyService
    {
        private readonly HttpClient _httpClient;

        private readonly string _remoteServiceBaseUrl;
        private readonly string _acessToken;

        public PropertyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _remoteServiceBaseUrl = ConfigurationManager.AppSettings["ApiServiceBase"];

            _acessToken = HttpContext.Current.GetOwinContext()
                .Authentication.User.Claims
                .FirstOrDefault(token => token.Type == "AcessToken")?.Value.Substring(7);
        }

        public string GetProperties()
        {
            string response = String.Empty;
            try
            {
                using (_httpClient)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _acessToken);
                    var result = _httpClient.GetAsync(ConfigurationManager.AppSettings["ApiServiceBase"]);

                    result.Wait(TimeSpan.FromSeconds(10));

                    if (result.IsCompleted)
                    {
                        if (result.Result.StatusCode == HttpStatusCode.Unauthorized)
                            response = HttpStatusCode.Unauthorized.ToString();
                        else
                            response = result.Result.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }
            return response;
        }

        public void CreateProperty(List<KeyValuePair<string, string>> postContents)
        {
            string response = String.Empty;
            try
            {
                using (_httpClient)
                {
                    FormUrlEncodedContent contents = new FormUrlEncodedContent(postContents);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _acessToken);
                    var result = _httpClient.PostAsync(ConfigurationManager.AppSettings["ApiServiceBase"], contents);

                    result.Wait(TimeSpan.FromSeconds(10));

                    if (!result.IsCompleted) return;

                    if (result.Result.StatusCode == HttpStatusCode.Unauthorized)
                        response = HttpStatusCode.Unauthorized.ToString();
                    else
                        response = result.Result.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }
        }

        public string GetPropertyById(int id)
        {
            string response = String.Empty;
            try
            {
                using (_httpClient)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _acessToken);
                    var result = _httpClient.GetAsync(ConfigurationManager.AppSettings["ApiServiceBase"] + $"/{id}");

                    result.Wait(TimeSpan.FromSeconds(10));

                    if (!result.IsCompleted) return response ;

                    response = result.Result.StatusCode == HttpStatusCode.Unauthorized 
                        ? HttpStatusCode.Unauthorized.ToString() 
                        : result.Result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }

            return response;
        }

        public void DeletePropertyById(int id)
        {
            string response = String.Empty;
            try
            {
                using (_httpClient)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _acessToken);
                    var result = _httpClient.DeleteAsync(ConfigurationManager.AppSettings["ApiServiceBase"] + $"/{id}");

                    result.Wait(TimeSpan.FromSeconds(10));

                    if (!result.IsCompleted) return;

                    response = result.Result.StatusCode == HttpStatusCode.Unauthorized
                        ? HttpStatusCode.Unauthorized.ToString()
                        : result.Result.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }
        }

        public void UpdateProperty(List<KeyValuePair<string, string>> keyValuePairs)
        {
            string response = String.Empty;
            try
            {
                using (_httpClient)
                {
                    FormUrlEncodedContent contents = new FormUrlEncodedContent(keyValuePairs);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _acessToken);
                    var result = _httpClient.PutAsync(ConfigurationManager.AppSettings["ApiServiceBase"], contents);

                    result.Wait(TimeSpan.FromSeconds(10));

                    if (!result.IsCompleted) return;

                    if (result.Result.StatusCode == HttpStatusCode.Unauthorized)
                        response = HttpStatusCode.Unauthorized.ToString();
                    else
                        response = result.Result.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }
        }
    }
}