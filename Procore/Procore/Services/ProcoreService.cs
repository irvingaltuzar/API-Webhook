using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Procore.Data;
using Procore.Models;
using Azure;

namespace Procore.Services
{
    public class ProcoreService
    {
        private Dictionary<string, Func<WebHookProcore, Task<string>>> opciones = new Dictionary<string, Func<WebHookProcore, Task<string>>>();
       private  Dictionary<string, int> actions = new Dictionary<string, int>
        {
            { "update", 1 },
            { "delete", 2 },
            { "create", 3 }
        };

        private Context _Context;
        private DBDatos _dbDatos;

        public ProcoreService()
        {
            _Context = new Context();
            opciones["Work Order Contracts"] = WorkOrderContracts;
            opciones["Draw Requests"] = ContractPayment;
            opciones["opcion3"] = CompanyUsers;
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
        public int ProcesarAccion(string action)
        {
            if (actions.ContainsKey(action))
            {
                int valor = actions[action];
                return valor;
            }
            else
            {
                return 0;
            }
        }
        private async Task<string> WorkOrderContracts(WebHookProcore data)
        {
            string response = "";
            string eventType = data.event_type;
            var jsonContract = await _Context.WorkOrderContracts(data);

            if (jsonContract == null)
            {
              response=  await ContractPayment(data);
            }
            else
            {

                Contract contract = JsonConvert.DeserializeObject<Contract>(jsonContract);
                //Contract contract = listcontract.FirstOrDefault(e => e.id == data.resource_id);
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
                    int option = ProcesarAccion(eventType);
                    //execute spProcore PricipalDMI
                    _dbDatos = new DBDatos();
                    response = _dbDatos.spProcore(json, option);
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

            var estimate = await _Context.ContractsPayments(data);

            if(estimate != null)
            {

            Estimate contractPayment = JsonConvert.DeserializeObject<Estimate>(estimate);

            if (contractPayment.status == "approved")
            {
                string json = "";
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
                response = _dbDatos.spProcore(json, 3);
            }
            else{
                response = "Contrato no se encuentra aprobado.";
            }

            }
            else
            {
                response = "No se encontró informacion relacionada desde webhook.";
            }
            return response;

        }

        private async Task<string> CompanyUsers(WebHookProcore data)
        {
            return  "Procesando Opción 3";
            // Implementa la lógica de Opción 3 aquí
        }
    }
}
