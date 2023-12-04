using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Procore.AlfaDB;
using Procore.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace Procore.Data
{
    public class DBDatos
    {
        public static string cadenaConexion = "Data Source=192.168.3.240;Initial Catalog=DMI_Calidad;Persist Security Info=True;Trusted_Connection=True;TrustServerCertificate=True;Encrypt = True";

        public DataTable EjecutarConsultaSQL(string consulta)
        {
            try
            {
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(cadenaConexion))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(consulta, connection))
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                // Manejar la excepción o lanzarla nuevamente según tus necesidades
                throw ex;
            }
        }
            public static DataSet ListarTablas(string nombreProcedimiento, List<Parametro> parametros = null)
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);

            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                    }
                }
                DataSet tabla = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);


                return tabla;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conexion.Close();
            }
        }

        public static DataTable Listar(string nombreProcedimiento, List<Parametro> parametros = null)
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);

            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                    }
                }
                DataTable tabla = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);


                return tabla;
            }
            catch (Exception )
            {
                return null;
            }
            finally
            {
                conexion.Close();
            }
        }

        public static bool Ejecutar(string nombreProcedimiento, List<Parametro> parametros = null)
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);

            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                    }
                }

                int i = cmd.ExecuteNonQuery();

                return (i > 0) ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conexion.Close();
            }
        }
        public OauthRefreshTokenProcore GetToken()
        {
            try
            {
                using (DbAlfaContext db = new DbAlfaContext())
                {
                    OauthRefreshTokenProcore token = (from d in db.OauthRefreshTokenProcores
                                                      where d.Revoked == false
                                                      select d).FirstOrDefault();
                    return token; // Devuelve el objeto OauthRefreshTokenProcore si se encuentra uno válido.
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public bool updateAccessToken(OauthTokenJson objToken)
        {
            try
            {
                var dbContext = new DbAlfaContext(); // Reemplaza "YourDbContext" con el nombre real de tu DbContext

                // Actualización en la entidad OAuthTokenProcore
                var oauthTokensToUpdate = dbContext.OauthTokensProcores
                    .Where(o => o.Revoked == false) // Filtra los registros que cumplan con el criterio
                    .ToList();

                foreach (var oauthTokenToUpdate in oauthTokensToUpdate)
                {
                    oauthTokenToUpdate.ResourceOwnerId = objToken.resource_owner_id;
                }

                // Guarda los cambios para todos los registros de OAuthTokenProcore
                dbContext.SaveChanges();

                // Actualización en la entidad OAuthRefreshTokenProcore
                var oauthRefreshTokensToUpdate = dbContext.OauthRefreshTokenProcores
                    .Where(o => o.Revoked == false) // Filtra los registros que cumplan con el criterio
                    .ToList();

                foreach (var oauthRefreshTokenToUpdate in oauthRefreshTokensToUpdate)
                {
                    oauthRefreshTokenToUpdate.ExpiresAt = objToken.expires_in_seconds;
                }

                // Guarda los cambios para todos los registros de OAuthRefreshTokenProcore
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool AddRefreshToken(OauthTokenJson objToken)
        {
            try
            {
                var dbContext = new DbAlfaContext(); 

                var registros = dbContext.OauthRefreshTokenProcores.ToList(); 

                foreach (var registro in registros)
                {
                    registro.Revoked = true;
                }

                dbContext.SaveChanges(); // Guarda los cambios en la base de datos

                using (var context = new DbAlfaContext())
                {
                    var token = new OauthRefreshTokenProcore()
                    {
                        AccessTokenId = objToken.access_token,
                        RefreshTokenId = objToken.refresh_token,
                        TokenType = objToken.token_type,
                        ExpiresAt= objToken.expires_in,
                        Revoked = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    context.OauthRefreshTokenProcores.Add(token);

                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
           
        }
        public bool addCodeAccessToken(string code)
        {
            try
            {
                var dbContext = new DbAlfaContext(); 

                var registros = dbContext.OauthTokensProcores.ToList(); 

                foreach (var registro in registros)
                {
                    registro.Revoked = true; 
                }

                dbContext.SaveChanges(); // Guarda los cambios en la base de datos

                using (var context = new DbAlfaContext())
                {
                    var token = new OauthTokensProcore()
                    {
                        Id = code,
                        Revoked = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    context.OauthTokensProcores.Add(token);

                    context.SaveChanges();
                }

             
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public string spProcore(string webhookData,int option)
        {
            try
            {
                String json;

                List<Parametro> parametros = new List<Parametro>
                {
                    new Parametro("@DatosJSON",webhookData.ToString()),
                new Parametro("@Opcion", option.ToString())
                };

                DataTable res = DBDatos.Listar("spDMI_PROCOREPrincipal", parametros);

                json = JsonConvert.SerializeObject(res);

                return json;
            }
            catch (Exception ex)
            {
                return ex.Message; // Devuelve un error 500 en caso de error
            }
        }

        //public string spProcoreEstimate(string webhookData, int option)
        //{
        //    try
        //    {
        //        String json;

        //        List<Parametro> parametros = new List<Parametro>
        //        {
        //            new Parametro("@DatosJSON",webhookData),
        //        new Parametro("@Opcion","3")
        //        };

        //        DataTable res = DBDatos.Listar("spDMI_PROCOREPrincipal", parametros);
        //        json = JsonConvert.SerializeObject(res);

        //        return json;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message; // Devuelve un error 500 en caso de error
        //    }
        //}
    }
}
