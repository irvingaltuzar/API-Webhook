using Newtonsoft.Json;
using Procore.AlfaDB;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Procore.Data
{
    public class RestServiceLogin
    {
        HttpClient _client;
        Uri _UrlBaseLogin;
        DBDatos _dbDatos;

        public RestServiceLogin(string urlBaseLogin)
        {
            _UrlBaseLogin = new Uri(urlBaseLogin);
            _dbDatos = new DBDatos();
            _client = new HttpClient
            {
                BaseAddress = _UrlBaseLogin
            };

        }

        public RestServiceLogin(string urlBaseLogin, string token) : this(urlBaseLogin)
            => _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



        /// <summary>
        /// Obtiene una lista de T a partir de una url
        /// </summary>
        /// <typeparam name="T">Tipo de objeto deseado para la lista</typeparam>
        /// <param name="url">url del servicio</param>
        /// <returns>Lista de "T"</returns>
        public async Task<string> GetDataAsync<T>(string url)
        {

            string jsonData = null;
            try
            {
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    jsonData = await response.Content.ReadAsStringAsync();

                    OauthTokenJson myObject = JsonConvert.DeserializeObject<OauthTokenJson>(jsonData);

                    _dbDatos.updateAccessToken(myObject);




                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized) // Comprueba si el código de estado es 401 (Unauthorized)
                {
                    jsonData= await RefreshToken("/oauth/token");
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

        public async Task<string> RefreshToken(string url)
        {
            //_client.DefaultRequestHeaders.Clear();
            DBDatos dbDatos = new DBDatos();
            OauthRefreshTokenProcore objToken= dbDatos.GetToken();

            string jsonData = null;
            try
            {
                var data = new
                {
                    grant_type = "refresh_token",
                    refresh_token = objToken.RefreshTokenId,
                    client_id = Constants.CLIENT_ID_PROCORE, // Reemplaza esto con el valor correcto
                    client_secret = Constants.CLIENT_SECRET_PROCORE, // Reemplaza esto con el valor correcto
                    redirect_uri = "http://localhost:8000/redirect" // Reemplaza esto con la URL correcta
                };
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    jsonData = await response.Content.ReadAsStringAsync();
                    OauthTokenJson myObject = JsonConvert.DeserializeObject<OauthTokenJson>(jsonData);
                    _dbDatos.AddRefreshToken(myObject);
                }
                //else if (response.StatusCode == HttpStatusCode.Unauthorized) // Comprueba si el código de estado es 401 (Unauthorized)
                //{

                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
            }

            return jsonData;
        }

    }
}
