using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Procore.AlfaDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Procore.Data
{
    public class RestService
    {
        HttpClient _client;
        Uri _UrlBase;
        DBDatos _dbDatos;

        public RestService(string urlBase)
        {
            _UrlBase = new Uri(urlBase);
            _dbDatos = new DBDatos();
            _client = new HttpClient
            {
                BaseAddress = _UrlBase
            };
        }

        public RestService(string urlBase, string token) : this(urlBase)
            => 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



        /// <summary>
        /// Obtiene una lista de T a partir de una url
        /// </summary>
        /// <typeparam name="T">Tipo de objeto deseado para la lista</typeparam>
        /// <param name="url">url del servicio</param>
        /// <returns>Lista de "T"</returns>
        public async Task<string> GetDataAsync<T>(string url, Dictionary<string, string> parameters = null)
        {
 
            string jsonData = null;
            try
            {
                if (parameters != null && parameters.Any())
                {
                    string queryString = string.Join("&", parameters.Select(kv => $"{kv.Key}={kv.Value}"));
                    url += "?" + queryString;
                }
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    jsonData = await response.Content.ReadAsStringAsync();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized) // Comprueba si el código de estado es 401 (Unauthorized)
                {
                    jsonData = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
            }

            return jsonData;
        }

        public bool CheckToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var reponse = _client.GetAsync("auth/check").GetAwaiter().GetResult();

            return reponse.IsSuccessStatusCode;
        }

        public async Task<string> PostAsync<T>(T data, string url)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());

            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutDataAsync<T>(T data, string url, long id,string company_id)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // Agregar el encabezado Procore-Company-Id
            _client.DefaultRequestHeaders.Add("Procore-Company-Id", company_id );
            var response =
                await _client.PutAsync(string.Concat(url, "/", id.ToString()), content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());

            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task DeleteAsync(string url, int id)
        {
            var response = await _client.DeleteAsync(String.Concat(url, "/", id.ToString()));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());

            }
        }

        public String GetToken()
        {
            try
            {
                String json;

                using (DbAlfaContext db = new DbAlfaContext())
                {
                    List<OauthRefreshTokenProcore> objtokens = (from d in db.OauthRefreshTokenProcores
                                                                where d.Revoked == false
                                                                select d).ToList();

                    json = JsonConvert.SerializeObject(objtokens);
                }

                return json; // Devuelve una respuesta HTTP 200 OK con el JSON serializado.
            }
            catch (Exception ex)
            {
               return ex.Message; // Devuelve un error 500 en caso de error
            }
        }
    }
}
