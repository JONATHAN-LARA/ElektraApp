using AccesoDatos.Contratos;
using AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositorios
{
    public class RepositorioClientesElektra : RepositorioMaestro, IClienteElektraRepositorio
    {

        private string selectAll;
        private string insert;
        private string update;
        private string delete;

        public RepositorioClientesElektra()
        {
            selectAll = "Select * from ClientesElektra";
            insert = "insert into ClientesElektra values(@ClienteUnico, @Nombre, @ApPaterno, @ApMaterno)";
            update = "update ClientesElektra set ClienteUnico=@ClienteUnico, Nombre=@Nombre, ApPaterno=@ApPaterno, ApMaterno=@ApMaterno where Id=@Id ";
            delete = "delete from ClientesElektra where Id = @Id";
        }


        public int Agregar(ClientesElektra entidad)
        {
            Parametros = new List<SqlParameter>();            
            Parametros.Add(new SqlParameter("@ClienteUnico", entidad.ClienteUnico));
            Parametros.Add(new SqlParameter("@Nombre", entidad.Nombre));
            Parametros.Add(new SqlParameter("@ApPaterno", entidad.ApPaterno));
            Parametros.Add(new SqlParameter("@ApMaterno", entidad.ApMaterno));

            return ExecuteNonQuery(insert);
        }

        public int Borrar(int Id)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@Id", Id));

            return ExecuteNonQuery(delete);
        }

        public int Editar(ClientesElektra entidad)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@Id", entidad.Id));
            Parametros.Add(new SqlParameter("@ClienteUnico", entidad.ClienteUnico));
            Parametros.Add(new SqlParameter("@Nombre", entidad.Nombre));
            Parametros.Add(new SqlParameter("@ApPaterno", entidad.ApPaterno));
            Parametros.Add(new SqlParameter("@ApMaterno", entidad.ApMaterno));

            return ExecuteNonQuery(update);

        }       

        public IEnumerable<ClientesElektra> GetAll()
        {
            var TableClientes = ExecuteReader(selectAll);
            var ListaClientes = new List<ClientesElektra>();

            foreach(DataRow item in TableClientes.Rows)
            {
                ListaClientes.Add(new ClientesElektra
                {
                    Id = Convert.ToInt32(item[0].ToString()),
                    ClienteUnico = item[1].ToString(),
                    Nombre = item[2].ToString(),
                    ApPaterno = item[3].ToString(),
                    ApMaterno = item[4].ToString()
                }); ; ;
            }

            return ListaClientes;
        }
    }
}
