using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Procore.AlfaDB;
using Procore.Models;
using System.Collections.Generic;

namespace Procore.Data
{
    public class Context
    {


        private RestService _RestService;
        private RestServiceLogin _RestServiceLogin;
        private OauthRefreshTokenProcore objToken;
        private DBDatos dbDatos;
        private string token;
        public Context()
        {
            Constants.InitializeFromDatabase();
            _RestService = new RestService(Constants.ServiceUrlBase);
            _RestServiceLogin = new RestServiceLogin(Constants.ServiceUrlBaseLogin);
            dbDatos = new DBDatos();
            objToken = dbDatos.GetToken();
            _RestServiceLogin = new RestServiceLogin(Constants.ServiceUrlBaseLogin, objToken.AccessTokenId);
           var tokenInfo= validateToken(objToken).GetAwaiter().GetResult();
            if(tokenInfo.expires_in < 500)
            {
                var handleRefresh = handlerRefresh().GetAwaiter().GetResult();
            }
            objToken = dbDatos.GetToken();
            _RestService = new RestService(Constants.ServiceUrlBase, objToken.AccessTokenId);

        }
      
        public async Task<string> GetInfo()
          => await _RestServiceLogin.GetDataAsync<string>("/oauth/token/info");

        public async Task<string> refreshToken()
          => await _RestServiceLogin.RefreshToken("/oauth/token");

        public async Task<string> WorkOrderContracts(WebHookProcore data)
            => await _RestService.GetDataAsync<string>("/rest/v1.0/work_order_contracts/" + data.resource_id + "?project_id=" + data.project_id);
        public async Task<string> CompanyVendor(long id, WebHookProcore data)
          => await _RestService.GetDataAsync<string>("/rest/v1.0/vendors/" + id + "?company_id=" + data.company_id);

        public async Task<string> ContractsPayments(WebHookProcore data)
           => await _RestService.GetDataAsync<string>("/rest/v1.1/requisitions/" + data.resource_id + "?project_id=" + data.project_id + "&company_id=" + data.company_id);
        public async Task<string> ContractsPaymentsDetail(WebHookProcore data)
          => await _RestService.GetDataAsync<string>("/rest/v1.0/requisitions/" + data.resource_id + "/detail?project_id=" + data.project_id + "&company_id=" + data.company_id);

        /// <summary>
        /// Show detail on a specified User.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> getUserCompany(WebHookProcore data)
         => await _RestService.GetDataAsync<string>("/rest/v1.3/companies/" + data.company_id + "/users/" + data.user_id);


        /// <summary>
        /// Funtion obtiene lista de codigos de procore
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> getListCodes(WebHookProcore data)
         => await _RestService.GetDataAsync<string>("/rest/v1.0/cost_codes?project_id=" + data.project_id + "&company_id=" + data.company_id);


        public async Task<string> addPaymentErpProcore(ContractPayment data)
       => await _RestService.PostAsync<ContractPayment>(data, "/rest/v1.0/contract_payments?company_id=" + data.company_id);

        /// <summary>
        /// Update License number vendor procore
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> updateVendorProcore(Dictionary<object, object> data,long id,string company_id)
             => await _RestService.PutDataAsync<Dictionary<object, object>>(data, "/rest/v1.0/vendors", id,company_id);


        /// <summary>
        /// Update License number vendor procore
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> updateStatusContract(Dictionary<object, object> data, long id, string company_id)
             => await _RestService.PutDataAsync<Dictionary<object, object>>(data, "/rest/v1.0/work_order_contracts", id, company_id);

        /// <summary>
        /// Asignacion de Token a nuestro de Context Data
        /// </summary>
        /// <param name="token"></param>
        public async Task<OauthTokenJson> validateToken(OauthRefreshTokenProcore objToken)
        {
            try
            {
                token = await GetInfo();
                OauthTokenJson json = JsonConvert.DeserializeObject<OauthTokenJson>(token);

                return json;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// handle Refresh Token
        /// </summary>
        /// <param name=""></param>
        public async Task<string> handlerRefresh()
        {
            try
            {
                token = await refreshToken();

            }
            catch (Exception ex)
            {
                return null;
            }
            return token;
        }


    }
}
