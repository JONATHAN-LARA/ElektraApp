using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositorios
{
    public class RepositorioMaestro : Repositorio
    {
        protected List<SqlParameter> Parametros;

        protected int ExecuteNonQuery(string Transactsql)
        {
            using (var Conexion = ObtenerConexion())
            {
                Conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = Conexion;
                    comando.CommandText = Transactsql;
                    comando.CommandType = CommandType.Text;
                    foreach (SqlParameter item in Parametros)
                    {
                        comando.Parameters.Add(item);
                    }
                    int resultado = comando.ExecuteNonQuery();
                    Parametros.Clear();
                    return resultado;
                }

            }

        }

        protected DataTable ExecuteReader(string Transactsql)
        {
            using (var conexion = ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = Transactsql;
                    comando.CommandType = CommandType.Text;
                    SqlDataReader Reader = comando.ExecuteReader();
                    using (var table = new DataTable())
                    {
                        table.Load(Reader);
                        Reader.Dispose();

                        return table;
                    }

                }
            }

        }

    }
}
