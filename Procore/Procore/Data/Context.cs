using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Procore.AlfaDB;
using Procore.Models;

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
            _RestService = new RestService(Constants.ServiceUrlBase);
            _RestServiceLogin = new RestServiceLogin(Constants.ServiceUrlBaseLogin);
            dbDatos = new DBDatos();
            objToken = dbDatos.GetToken();
            _RestServiceLogin = new RestServiceLogin(Constants.ServiceUrlBaseLogin, objToken.AccessTokenId);
            validateToken(objToken).GetAwaiter().GetResult();
            objToken = dbDatos.GetToken();
            _RestService = new RestService(Constants.ServiceUrlBase, objToken.AccessTokenId);


        }
        public async Task<bool> validateToken(OauthRefreshTokenProcore objToken)
        {
            try
            {
                token = await GetInfo();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Asignacion de Token a nuestro de Context Data
        /// </summary>
        /// <param name="token"></param>
        //public Context()
        //{
           

        //}

        public async Task<string> GetInfo()
          => await _RestServiceLogin.GetDataAsync<string>("/oauth/token/info");

        public async Task<string> refreshToken()
          => await _RestServiceLogin.RefreshToken("/oauth/token");

        public async Task<string> WorkOrderContracts(WebHookProcore data)
            => await _RestService.GetDataAsync<string>("/rest/v1.0/work_order_contracts/"+ data.resource_id+"?project_id="+data.project_id);
        public async Task<string> CompanyVendor(long id,WebHookProcore data)
          => await _RestService.GetDataAsync<string>("/rest/v1.0/vendors/" + id + "?company_id=" + data.company_id);

        public async Task<string> ContractsPayments(WebHookProcore data)
           => await _RestService.GetDataAsync<string>("/rest/v1.1/requisitions/" + data.resource_id + "?project_id=" + data.project_id+"&company_id="+ data.company_id);
        public async Task<string> ContractsPaymentsDetail(WebHookProcore data)
          => await _RestService.GetDataAsync<string>("/rest/v1.0/requisitions/" + data.resource_id + "/detail?project_id=" + data.project_id + "&company_id=" + data.company_id);
        public async Task<string> getListCodes(WebHookProcore data)
         => await _RestService.GetDataAsync<string>("/rest/v1.0/cost_codes?project_id=" + data.project_id + "&company_id=" + data.company_id);
        /// <summary>
        /// Validacion si el token aun sigue vigente
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> CheckToken() 
            => await _RestServiceLogin.GetDataAsync<string>("/oauth/token/info");

        /// <summary>
        /// Obtener ultimo token insertado en base datos
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> TokenActive()
        {
           string res= await _RestServiceLogin.GetDataAsync<string>("/oauth/token/info");

            return res;
        }

        //    public async Task<List<PersonalIntelisis>> GetContactPersonal()
        //      => await _RestService.GetDataAsync<PersonalIntelisis>("auth/extension");
        //    public async Task<List<Permiso>> GetMisPermisos()
        //    => await _RestService.GetDataAsync<Permiso>("auth/getPermission");
        //    public async Task<List<SolicitudesFirma>> GetSolicitudesFirma()
        //     => await _RestService.GetDataAsync<SolicitudesFirma>("auth/requestsToSing");

        //    public async Task<List<Permiso>> GetMisPermisosFiltroSolicitada()
        //   => await _RestService.GetDataAsync<Permiso>("auth/permissionRequested");
        //    public async Task<List<Permiso>> GetMisPermisosFiltroCancelado()
        // => await _RestService.GetDataAsync<Permiso>("auth/permissionCancel");
        //    public async Task<List<Permiso>> GetMisPermisosFiltroCompletado()
        // => await _RestService.GetDataAsync<Permiso>("auth/permissionCompleted");
        //    public async Task<List<SolicitudesFirma>> GetPermisosFirmados()
        //        => await _RestService.GetDataAsync<SolicitudesFirma>("auth/workerRequests");
        //    public async Task<List<PermisoConcepto>> GetPermisoConceptoGoce()
        //    => await _RestService.GetDataAsync<PermisoConcepto>("auth/permissionConcept");
        //    public async Task<List<PermisoConcepto>> GetPermisoConceptoGoceOtro()
        //    => await _RestService.GetDataAsync<PermisoConcepto>("auth/anotherConceptPermission");
        //    public async Task<List<Beneficios>> GetBeneficios()
        //=> await _RestService.GetDataAsync<Beneficios>("auth/benefits");
        //    /// <summary>
        //    /// Meotodo para autenticacion PostAsync a Laravel 8
        //    /// </summary>
        //    /// <param name="user"></param>
        //    /// <returns></returns>
        //    public async Task<string> Login(SegUsuario user)
        //          => await _RestService.PostAsync<SegUsuario>(user, "auth/login");

        //    /// <summary>
        //    /// Metodo para cerrar sesion 
        //    /// </summary>
        //    /// <param name="user"></param>
        //    /// <returns></returns>
        //    public async Task<string> Logout(RootUser user)
        //          => await _RestService.PostAsync<RootUser>(user, "auth/logout");


        //    /// <summary>
        //    /// MEtodo agregar permiso
        //    /// </summary>
        //    /// <param name="objPermiso"></param>
        //    /// <returns></returns>
        //    public async Task AddPermiso(Permiso objPermiso)
        //      => await _RestService.PostAsync(objPermiso, "auth/storePermission");


        //    /// <summary>
        //    /// Metodo agregar firma a solicitud permiso
        //    /// </summary>
        //    /// <param name="objPermiso"></param>
        //    /// <returns></returns>
        //    public async Task AddFirmaPermiso(Permiso objPermiso)
        //      => await _RestService.PostAsync(objPermiso, "auth/addFirm");

        //    /// <summary>
        //    /// Metodo cancelar una solicitud permiso
        //    /// </summary>
        //    /// <param name="objPermiso"></param>
        //    /// <returns></returns>
        //    public async Task CancelarPermiso(Permiso objPermiso)
        //      => await _RestService.PostAsync(objPermiso, "auth/cancelPermission");

        //    public async Task RechazarSolicitud(SolicitudesFirma objPermiso)
        //      => await _RestService.PostAsync(objPermiso, "auth/rejectRequest");
    }
}
