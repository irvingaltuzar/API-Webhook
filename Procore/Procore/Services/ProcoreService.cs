using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Procore.Data;
using Procore.Models;
using Azure;
using System.Net;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Procore.Models.Emails;
using Procore.AlfaDB;

namespace Procore.Services
{
    public class ProcoreService 
    {
        private Dictionary<string, Func<WebHookProcore, Task<string>>> opciones = new Dictionary<string, Func<WebHookProcore, Task<string>>>();
        //private Dictionary<string, int> actions = new Dictionary<string, int>
        //{
        //    { "create", 1 },
        //    { "update", 2 },
        //    { "delete", 3 }

        //};

        // Dictionarios de acciones específicos para cada tipo de recurso
        private static readonly Dictionary<string, int> workOrderContractsActions = new Dictionary<string, int>
    {
        { "create", 1 },
        { "update", 1 }
    };

        private static readonly Dictionary<string, int> drawRequestsActions = new Dictionary<string, int>
    {
        { "create", 3 },
        { "update", 3 },
        { "delete", 4 },
    };

        private static readonly Dictionary<string, int> purchaseOrderContractsActions = new Dictionary<string, int>
    {
        { "create", 5 }
    };

        private Context _Context;
        private DBDatos _dbDatos;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMailService _emailService;
        public ProcoreService(IWebHostEnvironment hostingEnvironment, IMailService emailService)
        {
            _Context = new Context();
            opciones["Work Order Contracts"] = WorkOrderContracts;
            opciones["Draw Requests"] = ContractPayment;
            opciones["Purchase Order Contracts"] = PurchaseContract;
            _hostingEnvironment = hostingEnvironment;
            _emailService = emailService;
          
        }
        public async Task<string> ProcesarOpcion(string opcion,WebHookProcore data)
        {
            if (opciones.ContainsKey(opcion))
            {
                 return await opciones[opcion](data);
            }
            else
            {
                // Manejar opción no válida
                return "Opción no válida";
            }
        }
        //public int ProcesarAccion(string action)
        //{
        //    if (actions.ContainsKey(action))
        //    {
        //        int valor = actions[action];
        //        return valor;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        public int ProcesarAccion(string eventType, string resourceName)
        {
            switch (resourceName)
            {
                case "Work Order Contracts":
                    return workOrderContractsActions.ContainsKey(eventType) ? workOrderContractsActions[eventType] : 0;
                case "Draw Requests":
                    return drawRequestsActions.ContainsKey(eventType) ? drawRequestsActions[eventType] : 0;
                case "Purchase Order Contracts":
                    return purchaseOrderContractsActions.ContainsKey(eventType) ? purchaseOrderContractsActions[eventType] : 0;
                default:
                    return 0; // 0 podría indicar una acción desconocida o no soportada
            }
        }
        private async Task<string> WorkOrderContracts(WebHookProcore data)
        {
            string response = "";
            string eventType = data.event_type;
            string resource_name = data.resource_name;
            var jsonContract = await _Context.WorkOrderContracts(data);

            if (jsonContract == null)
            {
             
              response = "No se encontró informacion relacionada del contrato.";
                
            }
            else
            {

                Contract contract = JsonConvert.DeserializeObject<Contract>(jsonContract);
                string status = contract.status;
                var idVendor = contract.vendor.id;
                string json = "";
                if (status == "Approved")
                {
                    string jsonVendor = await _Context.CompanyVendor(idVendor, data);
                    CompanyVendor lvendor = JsonConvert.DeserializeObject<CompanyVendor>(jsonVendor);
                    var diccionario = new Dictionary<object, object>{
                            { "contract", contract },
                            { "vendor", lvendor }
                        };
                    json = JsonConvert.SerializeObject(diccionario, Formatting.Indented);
                    //get option {int} to (create,delete or update)
                    int option = ProcesarAccion(eventType, resource_name);
                    //execute spProcore PrincipalDMI
                    _dbDatos = new DBDatos();
                    response = _dbDatos.spProcore(json, option);


                    var jsonUser = await _Context.getUserCompany(data);

                    UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonUser);

                  
                    // Analizar la respuesta JSON
                    JArray jsonResponse = JArray.Parse(response);
                    JObject firstObject = (JObject)jsonResponse.First;
                    string st = firstObject["status"].ToString();
                    string msg = firstObject["msg"].ToString();
                    string subject = firstObject["subject"].ToString();
                    string Prov = firstObject.ContainsKey("vendor") ? firstObject["vendor"].ToString() : null;
                    string ProvRFC = firstObject.ContainsKey("vendorRFC") ? firstObject["vendorRFC"].ToString() : null;
                    string Contract = firstObject.ContainsKey("contract") ? firstObject["contract"].ToString() : null;
                    string type = "";

                    EmailParameter objEmail = new EmailParameter();
                    objEmail.Subject = subject;
                    objEmail.Message = msg;
                    objEmail.Template = st;
                    objEmail.Email = new List<string>();
                    if (st == "validation")
                    {
                        if (Contract == null || string.IsNullOrEmpty(Contract.ToString()))
                        {

                            //create data json for update license number

                            var dataContract = new Dictionary<object, object>{
                            { "project_id", data.project_id },
                            { "work_order_contract", new Dictionary<string, object>
                                        {
                                            { "status", "Draft" }
                                        }
                            }
                        };

                            //await _Context.updateStatusContract(dataContract, data.resource_id, data.company_id.ToString());
                            //create data for send email
                            List<string> emailUser = new List<string>();
                            emailUser.Add(userInfo.email_address);
                            objEmail.Email.AddRange(emailUser);
                        }
                    
                    }
                    else if (st == "success")
                    {
                        if (Prov != "" || Prov!= null)
                        {
                            //create data json for update license number

                            var dataVendor = new Dictionary<object, object>{
                            { "company_id", data.company_id },
                            { "vendor", new Dictionary<string, object>
                                        {
                                            { "license_number", Prov }
                                        }
                            }
                        };
                            //serialize data json vendor 
                            //put update vendor in procore
                            await _Context.updateVendorProcore(dataVendor, lvendor.id, data.company_id.ToString());
                            _dbDatos.updateVendorAlfa(ProvRFC, Prov);
                            //get Information catEmailNotification
                            List<CatSupplierNotification> listEmails = _dbDatos.GetMailSupplierNotification();

                            List<string> emailList = new List<string>();

                            foreach (var notification in listEmails)
                            {
                                // Agregas el correo electrónico de cada notificación a la lista de correos electrónicos
                                emailList.Add(notification.Mail);
                            }
                            objEmail.Email.AddRange(emailList);

                            //success to user 
                            List<string> emailUser = new List<string>();
                            emailUser.Add(userInfo.email_address);
                            objEmail.Email.AddRange(emailUser);
                        }
                    }
                    if (objEmail.Email.Count > 0)
                    {
                        var resEmail = SendEmail(objEmail);
                    }
                }
                else
                {
                    response = "Contrato no se encuentra aprobado.";
                }
            }

            return response;
        }

        private async Task<string> ContractPayment(WebHookProcore data)
        {
            string eventType = data.event_type;
            string response = "";
            string json = "";
            _dbDatos = new DBDatos();

            if (eventType == "delete")
            {
                json = JsonConvert.SerializeObject(data, Formatting.Indented);
                response = _dbDatos.spProcore(json, 4);
            }
            else
            {
                var estimate = await _Context.ContractsPayments(data);

                if (estimate != null)
                {

                    Estimate contractPayment = JsonConvert.DeserializeObject<Estimate>(estimate);

                    if (contractPayment.status == "approved")
                    {


                        foreach (Attachment attachment in contractPayment.attachments)
                        {
                            if (attachment.content_type == "text/xml")
                            {
                                var stat = FileDownload(attachment.url, attachment.filename);
                            }
                        }

                        var items = await _Context.ContractsPaymentsDetail(data);
                        List<EstimateDetail> details = JsonConvert.DeserializeObject<List<EstimateDetail>>(items);
                        var codes = await _Context.getListCodes(data);
                        List<CostCode> listCodes = JsonConvert.DeserializeObject<List<CostCode>>(codes);
                        var diccionario = new Dictionary<object, object>
                {
                                { "estimacion", contractPayment },
                                { "detalle", details },
                                { "costos", listCodes },
                };

                        json = JsonConvert.SerializeObject(diccionario, Formatting.Indented);
                        response = _dbDatos.spProcore(json, 3);
                    }
                    else
                    {

                    }

                }
                else
                {
                    response = "No se encontró informacion relacionada desde webhook.";
                }
            }
            return response;

        }

        private async Task<string> PurchaseContract(WebHookProcore data)
        {
            string eventType = data.event_type;
            string response = "";

            var estimate = await _Context.ContractsPayments(data);

            if (estimate != null)
            {

                Estimate contractPayment = JsonConvert.DeserializeObject<Estimate>(estimate);

                if (contractPayment.status == "approved")
                {
                    string json = "";

                    foreach (Attachment attachment in contractPayment.attachments)
                    {
                        if (attachment.content_type == "text/xml")
                        {
                            var stat = FileDownload(attachment.url, attachment.filename);
                        }
                    }

                    var items = await _Context.ContractsPaymentsDetail(data);
                    List<EstimateDetail> details = JsonConvert.DeserializeObject<List<EstimateDetail>>(items);
                    var codes = await _Context.getListCodes(data);
                    List<CostCode> listCodes = JsonConvert.DeserializeObject<List<CostCode>>(codes);
                    var diccionario = new Dictionary<object, object>
                {
                                { "estimacion", contractPayment },
                                { "detalle", details },
                                { "costos", listCodes },
                };

                    json = JsonConvert.SerializeObject(diccionario, Formatting.Indented);
                    _dbDatos = new DBDatos();
                    response = _dbDatos.spProcore(json, 1);
                }
                else
                {
                    response = "Contrato no se encuentra aprobado.";

                }

            }
            else
            {
                response = "No se encontró informacion relacionada desde webhook.";
            }
            return response;

        }
        private string FileDownload(string url, string name)
        {
            try
            {
                //string response = "";
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(name))
                {
                    return "La URL o el nombre del archivo no pueden estar vacíos.";
                }
                string carpetaDescargas = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Files");
                //string rutaCompletaArchivo = Path.Combine(carpetaDescargas, name);
                string rutaArchivoTemporal = Path.Combine(carpetaDescargas, name);
                string carpetaCompartida = @"\\192.168.3.240\xml";

                string rutaCompletaArchivo = Path.Combine(carpetaCompartida, name);

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        if (!Directory.Exists(carpetaDescargas))
                        {
                            Directory.CreateDirectory(carpetaDescargas);
                        }

                        client.DownloadFile(url, rutaArchivoTemporal);

                        // Copiar el archivo descargado a la carpeta compartida de red
                        System.IO.File.Copy(rutaArchivoTemporal, rutaCompletaArchivo, true);

                        System.IO.File.Delete(rutaArchivoTemporal);

                       return "Descarga completa.";

                    }
                    catch (Exception ex)
                    {
                        return $"Error al descargar: {ex.Message}";
                    }
                }

            }
            catch (Exception ex)
                {
                    return ex.Message; // Devuelve un error 500 en caso de error
                }
            }

        private async Task<bool> SendEmail(EmailParameter objEmail)
        {
            try {

                MailData mailData = new MailData(
                    objEmail.Email,
                     objEmail.Subject,
                    _emailService.GetEmailTemplate(objEmail.Template, objEmail));


                bool sendResult = await _emailService.SendAsync(mailData, new CancellationToken());

                return sendResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<string> CompanyUsers(WebHookProcore data)
        {
            return  "Procesando Opción 3";
            // Implementa la lógica de Opción 3 aquí
        }
    }
}
