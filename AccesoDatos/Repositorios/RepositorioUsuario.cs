using AccesoDatos.Contratos;
using AccesoDatos.Entidades;
using Comun.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repositorios
{
    public class RepositorioUsuario : RepositorioMaestro, IUsuarioRepositorio
    {

        private string SelectAll;
        private string Insert;
        private string Actualizar;
        private string Eliminar;
        
        public RepositorioUsuario()
        {
            SelectAll = "Select * from Usuarios";
            Insert = "insert into Usuarios values( @idUsuario, @Nombre, @ApellidoPater, @ApellidoMater, @LoginUser, @Password, @email, @idRol)";
            Actualizar = "update Usuarios set Nombre=@Nombre, ApellidoPater=@ApellidoPater, ApellidoMater=@ApellidoMater, LoginUser=@LoginUser, Password=@Password, email=@email, idRol=@idRol where idUsuario = @idUsuario ";
            Eliminar = "delete from Usuarios where idUsuario = @idUsuario";
        }
        public int Agregar(Usuarios entidad)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@idUsuario", entidad.IdUsuario));
            Parametros.Add(new SqlParameter("@Nombre", entidad.Nombre));
            Parametros.Add(new SqlParameter("@ApellidoPater", entidad.ApellidoPater));
            Parametros.Add(new SqlParameter("@ApellidoMater", entidad.ApellidoMater));
            Parametros.Add(new SqlParameter("@LoginUser", entidad.LoginUser));
            Parametros.Add(new SqlParameter("@Password", entidad.Password));
            Parametros.Add(new SqlParameter("@email", entidad.Email));
            Parametros.Add(new SqlParameter("@idRol", entidad.IdRol));

            return ExecuteNonQuery(Insert);

        }

        public int Borrar(int Id)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@idUsuario", Id));

            return ExecuteNonQuery(Eliminar);
        }

        public int Editar(Usuarios entidad)
        {
            Parametros = new List<SqlParameter>();
            Parametros.Add(new SqlParameter("@Nombre", entidad.Nombre));
            Parametros.Add(new SqlParameter("@ApellidoPater", entidad.ApellidoPater));
            Parametros.Add(new SqlParameter("@ApellidoMater", entidad.ApellidoMater));
            Parametros.Add(new SqlParameter("@LoginUser", entidad.LoginUser));
            Parametros.Add(new SqlParameter("@Password", entidad.Password));
            Parametros.Add(new SqlParameter("@email", entidad.Email));
            Parametros.Add(new SqlParameter("@idRol", entidad.IdRol));
            Parametros.Add(new SqlParameter("@idUsuario", entidad.IdUsuario));

            return ExecuteNonQuery(Actualizar);

        }

        public IEnumerable<Usuarios> GetAll()
        {
            var TableResultado = ExecuteReader(SelectAll);
            var ListaUsuarios = new List<Usuarios>();

            foreach(DataRow item in TableResultado.Rows)
            {
                ListaUsuarios.Add(new Usuarios
                {
                    IdUsuario = Convert.ToInt32(item[0]),
                    Nombre = item[1].ToString(),
                    ApellidoPater = item[2].ToString(),
                    ApellidoMater = item[3].ToString(),
                    LoginUser = item[4].ToString(),
                    Password = item[5].ToString(),
                    Email = item[6].ToString(),
                    IdRol = item[7].ToString()

                });               
            }

            return ListaUsuarios;
        }

        public bool Login(string UserName, string Pass)
        {
            var TableResultado = ExecuteReader("select * from Usuarios where LoginUser COLLATE Latin1_General_CS_AS = " + "'" + UserName + "'" + "and Password COLLATE Latin1_General_CS_AS =" + "'" + Pass + "'");

            if (TableResultado.Rows.Count != 0)
            {
                foreach(DataRow item in TableResultado.Rows)
                {
                    UsuarioActivo.C_idUsuario = Convert.ToInt32(item[0]);
                    UsuarioActivo.C_Nombre = item[1].ToString();
                    UsuarioActivo.C_ApellidoPaterno = item[2].ToString();
                    UsuarioActivo.C_ApellidoMaterno = item[3].ToString();
                    UsuarioActivo.C_LoginUser = item[4].ToString();
                    UsuarioActivo.C_Contraseña = item[5].ToString();
                    UsuarioActivo.C_Email = item[6].ToString();
                    UsuarioActivo.C_IdRol = Convert.ToInt32(item[7]);
                }

                return true;
            }
            else
            {
                return false;
            }        

        }
    }
}
