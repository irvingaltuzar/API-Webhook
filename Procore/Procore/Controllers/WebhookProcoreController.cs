using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Procore.Data;
using System.Collections.ObjectModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Procore.AlfaDB;
using System.Text.Json;
using Procore.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using Procore.Services;
using Procore.Models.Emails;
using System.Net;
using Microsoft.AspNetCore.Hosting.Server;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Procore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookProcoreController : ControllerBase 
    {
        private Context _Context;
        private readonly DbAlfaContext _dbContext;
        private DBDatos _dbDatos;
        private readonly IMailService _emailService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WebhookProcoreController(DbAlfaContext dbContext, IMailService emailService, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _Context = new Context();
            _dbDatos = new DBDatos();
            _emailService = emailService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("WebhookCallback")]
        public async Task<IActionResult> WebhookCallback([FromBody] WebHookProcore webhookData)
        {
            try
            {
                string eventType = webhookData.event_type;

                var handler = new ProcoreService(_hostingEnvironment, _emailService);
                string response = await handler.ProcesarOpcion(webhookData.resource_name, webhookData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Maneja las excepciones según tus necesidades
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("MailTest")]
        //public async Task<IActionResult> SendEmail()
        //{
        //    EmailParameter welcomeMail = new EmailParameter();
        //    welcomeMail.Name = "Irving"; ;
        //    welcomeMail.Email = "irving.altuzar@grupodmi.com.mx";
        //    welcomeMail.Message = "El proyecto no esta correctamente configurado (campo DIVISION GENERAL) - Favor de revisar con Finanzas";
        //    // Create MailData object
        //    MailData mailData = new MailData(
        //        new List<string> { welcomeMail.Email, "lesly.galicia@grupodmi.com.mx" },
        //    "Notificación Contrato Procore",
        //        _emailService.GetEmailTemplate("validation", welcomeMail));


        //    bool sendResult = await _emailService.SendAsync(mailData, new CancellationToken());

        //    if (sendResult)
        //    {
        //        return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
        //    }
        //}


        [HttpPost("ObtenerDatos")]
        public IActionResult ObtenerDatos([FromBody] JsonObject webhookData)
        {
            try
            {
                String json;
                string tes = webhookData.ToString();
                List<Parametro> parametros = new List<Parametro>
                {
                    new Parametro("@DatosJSON",tes),
                new Parametro("@opc","6")
                };

                DataTable res= DBDatos.Listar("spDMI_PROCOREEstimaciones", parametros);
                json = JsonConvert.SerializeObject(res);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Devuelve un error 500 en caso de error
            }
        }

        [HttpGet("Payments_ERP_Procore")]
        public async Task<IActionResult> Payments_ERP_Procore()
        {
            try
            {
                String json;
                string result="";
            
                List<Parametro> parametros = new List<Parametro>
                {
                    new Parametro("@DatosJSON","0")
                
                };

                DataTable res = DBDatos.Listar("spDMI_PROCOREPagos", parametros);
          
                json = res.Rows[0][0].ToString();
                if(json != null && json !="")
                {

                List<ContractPayment> payment = JsonConvert.DeserializeObject<List<ContractPayment>>(json);
                    foreach (ContractPayment paymentItem in payment)
                    {
                        paymentItem.company_id = 598134325511619;
                        result = await _Context.addPaymentErpProcore(paymentItem);
                        List<Parametro> param = new List<Parametro>
                      {
                    new Parametro("@DatosJSON",result)
                       };

                        DataTable erpRes = DBDatos.Listar("spDMI_PROCOREPagos", param);
                     json = JsonConvert.SerializeObject(erpRes);

                 }

                }
                else
                {

                    json = "no hay informacion de pagos ERP";
                }

                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Devuelve un error 500 en caso de error
            }
        }
        [HttpGet("filedownload")]
        public async Task<IActionResult> filedownload(string url, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(name))
                {
                    return BadRequest("La URL o el nombre del archivo no pueden estar vacíos.");
                }
                string response = "";
                string carpetaDescargas = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Files");
                //string rutaCompletaArchivo = Path.Combine(carpetaDescargas, name);
                string rutaArchivoTemporal = Path.Combine(carpetaDescargas, name);
                string carpetaCompartida = @"\\192.168.3.124\AdmonDocs\No Validados\Egresos\Gastos\FilesProcore";

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


                        response = "Descarga completa.";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al descargar: {ex.Message}");
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Devuelve un error 500 en caso de error
            }
        }

        [HttpGet("GetInfo")]
        public async Task<string> Metodo()
        {
            try
            {
                var response = await _Context.GetInfo();

                return response;
            } 
            catch (Exception ex)
            {

                return  ex.Message;
            }
           

        }

        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken()
        {
            try
            {
                var response = await _Context.refreshToken();

                return response;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }

    }
}
