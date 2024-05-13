using Procore.AlfaDB;
using System;
using System.Collections.Generic;
using System.Text;
namespace Procore.Data
{
    public static class Constants

    {

        // Variables estáticas para almacenar los valores
        public static string ServiceUrlBase { get; private set; }
        public static string ServiceUrlBaseLogin { get; private set; }
        public static string CLIENT_ID_PROCORE { get; private set; }
        public static string CLIENT_SECRET_PROCORE { get; private set; }

       public static void InitializeFromDatabase()
        {
            try
            {
                using (DbAlfaContext db = new DbAlfaContext())
                {
                    ProcoreConfiguration procore = db.ProcoreConfigurations
                                                  .FirstOrDefault();
                    if (procore != null)
                    {
                        ServiceUrlBase = procore.ServiceUrl;
                        ServiceUrlBaseLogin = procore.ServiceUrlLogin;
                        CLIENT_ID_PROCORE = procore.ClientId;
                        CLIENT_SECRET_PROCORE = procore.ClientSecret;
                    }
                }
            }
            catch (Exception ex)
            {

                // Manejar cualquier excepción que ocurra durante la inicialización desde la base de datos
                Console.WriteLine($"Error al inicializar desde la base de datos: {ex.Message}");
            }
        }
    }
}
