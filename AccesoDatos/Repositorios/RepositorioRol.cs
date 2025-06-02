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
    public class RepositorioRol : RepositorioMaestro, IRolRepositorio
    {
        private string SelectAll;
        private string Insert;
        private string Actualizar;
        private string Eliminar;

        public RepositorioRol()
        {
            SelectAll = "Select * from Roles";
            Insert = "insert into Roles values( @NombreRol)";
            Actualizar = "update Roles set nomRol=@NombreRol where idRol = @idRol ";
            Eliminar = "delete from Roles where idRol = @idRol";
        }
        public int Agregar(Rol entidad)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@NombreRol", entidad.NombreRol));
            return ExecuteNonQuery(Insert);
            
        }

        public int Borrar(int Id)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@idRol", Id));
            return ExecuteNonQuery(Eliminar);
        }

        public int Editar(Rol entidad)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@idRol", entidad.IdRol));
            Parametros.Add(new SqlParameter("@NombreRol", entidad.NombreRol));
            return ExecuteNonQuery(Actualizar);

        }

        public IEnumerable<Rol> GetAll()
        {
            var TableResult = ExecuteReader(SelectAll);
            var ListRoles = new List<Rol>();
            foreach(DataRow Item in TableResult.Rows)
            {
                ListRoles.Add(new Rol
                {
                    IdRol = Convert.ToInt32(Item[0]),
                    NombreRol = Item[1].ToString(),


                });
            }

            return ListRoles;

            
        }
    }
}
