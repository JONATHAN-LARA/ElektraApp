using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AccesoDatos.Repositorios
{
    public abstract class Repositorio
    {
        private readonly string CadenaConexion;

       public Repositorio()
        {
            CadenaConexion = ConfigurationManager.ConnectionStrings["ConnClientesElektra"].ToString();
        } 

        protected SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        } 

    }
}
