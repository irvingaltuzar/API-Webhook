using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Procore.Data;
using Procore.Models;

namespace Procore.Services
{
    public class JobsService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory; // Necesario si se van a usar servicios inyectados
        private readonly ILogger<JobsService> _logger; // Necesario si se va a utilizar registro de logs
        private Context _Context;
        public JobsService(IServiceScopeFactory serviceScopeFactory, ILogger<JobsService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _Context = new Context();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    String json;
                    String result;

                    List<Parametro> parametros = new List<Parametro>
                {
                    new Parametro("@DatosJSON","0")

                };

                    DataTable res = DBDatos.Listar("spDMI_PROCOREPagos", parametros);

                    json = res.Rows[0][0].ToString();
                    if (json != null && json != "")
                    {

                        List<ContractPayment> payment = JsonConvert.DeserializeObject<List<ContractPayment>>(json);
                        foreach (ContractPayment paymentItem in payment)
                        {
                            paymentItem.company_id = 4266708; 
                            //paymentItem.company_id = 598134325511619; 
                            result = await _Context.addPaymentErpProcore(paymentItem);

                            List<Parametro> param = new List<Parametro>
                      {
                    new Parametro("@DatosJSON",result)
                       };

                            DataTable erpRes = DBDatos.Listar("spDMI_PROCOREPagos", param);

                        }
                    }
                    else
                    {

                        _logger.LogInformation("No hay informacion de pagos ERP a ejecutar");
                    }


                    _logger.LogInformation("Tarea ejecutada correctamente."); // Ejemplo de registro de un mensaje informativo
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al ejecutar la tarea."); // Ejemplo de registro de un error
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Espera 1 hora antes de la próxima ejecución
            }
        }
    }

}
