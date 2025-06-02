using AccesoDatos.Contratos;
using AccesoDatos.Entidades;
using AccesoDatos.Repositorios;
using Dominio.ObjetosDeValores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    public class UsuarioModelo
    {
        private int idUsuario;
        private string Nombre;
        private string ApellidoPater;
        private string ApellidoMater;
        private string LoginUser;
        private string Password;
        private string email;
        private string idRol;

        private IUsuarioRepositorio UsuarioRepositorio;
        public EstadoEntidad Estado;
        private List<UsuarioModelo> ListaUsuarios;

        [Required]
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        [Required]
        public string Nombre1 { get => Nombre; set => Nombre = value; }
        [Required]
        public string ApellidoPater1 { get => ApellidoPater; set => ApellidoPater = value; }
        [Required]
        public string ApellidoMater1 { get => ApellidoMater; set => ApellidoMater = value; }
        [Required]
        public string LoginUser1 { get => LoginUser; set => LoginUser = value; }
        [Required]
        public string Password1 { get => Password; set => Password = value; }
        [Required]
        public string Email { get => email; set => email = value; }
        [Required]
        public string IdRol { get => idRol; set => idRol = value; }



        public UsuarioModelo()
        {
            UsuarioRepositorio = new RepositorioUsuario();
        }

        public string GuardarCambios()
        {
            string mensaje = null;

            try
            {

                var DatosUsuarioModelo = new Usuarios();

                DatosUsuarioModelo.IdUsuario = IdUsuario;
                DatosUsuarioModelo.Nombre = Nombre1;
                DatosUsuarioModelo.ApellidoPater = ApellidoPater1;
                DatosUsuarioModelo.ApellidoMater = ApellidoMater1;
                DatosUsuarioModelo.LoginUser = LoginUser1;
                DatosUsuarioModelo.Password = Password1;
                DatosUsuarioModelo.Email = Email;
                DatosUsuarioModelo.IdRol = IdRol;

                switch (Estado)
                {
                    case EstadoEntidad.Agregado:
                        UsuarioRepositorio.Agregar(DatosUsuarioModelo);
                        mensaje = "Usuario Agregado sastifactoriamente";
                        break;
                    case EstadoEntidad.Eliminado:
                        UsuarioRepositorio.Borrar(IdUsuario);
                        mensaje = "Usuario Eliminado de la base de datos";
                        break;
                    case EstadoEntidad.Modificado:
                        UsuarioRepositorio.Editar(DatosUsuarioModelo);
                        mensaje = "El usuario a sido modificado sastifactoriamente";
                        break;

                }

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    mensaje = "Ficha del usuario duplicada\n Ya existe un usuario con ese numero de Ficha";
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;

        }

        public List<UsuarioModelo> GetAll()
        {
            var UsuarioModelo = UsuarioRepositorio.GetAll();
            ListaUsuarios = new List<UsuarioModelo>();
            foreach (Usuarios item in UsuarioModelo)
            {
                ListaUsuarios.Add(new UsuarioModelo
                {
                    idUsuario = item.IdUsuario,
                    Nombre = item.Nombre,
                    ApellidoPater = item.ApellidoPater,
                    ApellidoMater = item.ApellidoMater,
                    LoginUser = item.LoginUser,
                    Password = item.Password,
                    email = item.Email,
                    idRol = item.IdRol,
                });
            }
            return ListaUsuarios;
        }

        public string logIn(string user, string pass)
        {
            try
            {
                if (UsuarioRepositorio.Login(user, pass))
                {
                    return "concedido";
                }

                return "falso";

            }
            catch (Exception ex)
            {
                //throw new ArgumentException( "A ocurrido un error..." + ex.Message, "SICC...  ADVERTENCIA");
                // MessageBox.Show(ex.Message, "S I C C...",
                //  MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return ex.Message.ToString();
            }
        }
        public IEnumerable<UsuarioModelo> BuscarId(string filter)
        {
            return ListaUsuarios.FindAll(e => e.idUsuario.ToString().Contains(filter) || e.Nombre.Contains(filter) || e.ApellidoPater.Contains(filter));
        }
    }
}
