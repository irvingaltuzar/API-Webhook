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
        public WebhookProcoreController(DbAlfaContext dbContext, IMailService emailService)
        {
            _dbContext = dbContext;
            _Context = new Context();
            _dbDatos = new DBDatos();
            _emailService = emailService;
        }

        [HttpPost("WebhookCallback")]
        public async Task<IActionResult> WebhookCallback([FromBody] WebHookProcore webhookData)
        {
            try
            {
                string eventType = webhookData.event_type;

                var handler = new ProcoreService();
                string response = await handler.ProcesarOpcion(webhookData.resource_name, webhookData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Maneja las excepciones según tus necesidades
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("MailTest")]
        public async Task<IActionResult> SendEmail()
        {
            WelcomeMail welcomeMail = new WelcomeMail();
            welcomeMail.Name = "Irving"; ;
            welcomeMail.Email = "irving.altuzar@grupodmi.com.mx";
            // Create MailData object
            MailData mailData = new MailData(
                new List<string> { welcomeMail.Email, "lesly.galicia@grupodmi.com.mx" },
            "Validación correo procore",
                _emailService.GetEmailTemplate("validation", welcomeMail));


            bool sendResult = await _emailService.SendAsync(mailData, new CancellationToken());

            if (sendResult)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent using template.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }


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

        [HttpPost("Payments_ERP_Procore")]
        public async Task<IActionResult> Payments_ERP_Procore()
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

                DataTable res = DBDatos.Listar("spDMI_PROCOREEstimaciones", parametros);
                json = JsonConvert.SerializeObject(res);
                return Ok(json);
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
