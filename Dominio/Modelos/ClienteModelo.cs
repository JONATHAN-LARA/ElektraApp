using AccesoDatos.Contratos;
using Dominio.ObjetosDeValores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AccesoDatos.Repositorios;
using AccesoDatos.Entidades;
using System.Data.SqlClient;

namespace Dominio.Modelos
{
    public class ClienteModelo
    {
        private int Id;
        private string ClienteUnico;
        private string Nombre;
        private string ApellidoPaterno;
        private string ApellidoMaterno;

        private IClienteElektraRepositorio ClienteRepositorio;
        public EstadoEntidad Estado { private get; set; }
        private List<ClienteModelo> ListaClientes;

       
        public int Id1 { get => Id; set => Id = value; }

        [Required]
        [RegularExpression("^[0-9]{10,20}$", ErrorMessage = "El Cliente Unico solo debe contener numeros. MAXIMO: 20 DIGITOS; MINIMO: 12 DIGITOS")]
        public string ClienteUnico1 { get => ClienteUnico; set => ClienteUnico = value; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre1 { get => Nombre; set => Nombre = value; }

        [Required(ErrorMessage = "El campo Apellido Paterno es obligatorio")]
        public string ApellidoPaterno1 { get => ApellidoPaterno; set => ApellidoPaterno = value; }

        [Required(ErrorMessage = "El campo Apellido Materno es obligatorio")]
        public string ApellidoMaterno1 { get => ApellidoMaterno; set => ApellidoMaterno = value; }

        public ClienteModelo()
        {
            ClienteRepositorio = new RepositorioClientesElektra();
        }

        public string GuardarCambios()
        {
            string mensaje = null;

            try
            {

                var DatosClienteModelo = new ClientesElektra();

                DatosClienteModelo.Id = Id1;
                DatosClienteModelo.ClienteUnico = ClienteUnico1;
                DatosClienteModelo.Nombre = Nombre1;
                DatosClienteModelo.ApPaterno = ApellidoPaterno1;
                DatosClienteModelo.ApMaterno = ApellidoMaterno1;

               

                switch (Estado)
                {
                    case EstadoEntidad.Agregado:
                        ClienteRepositorio.Agregar(DatosClienteModelo);
                        mensaje = "Usuario Agregado sastifactoriamente";
                        break;
                    case EstadoEntidad.Eliminado:
                        ClienteRepositorio.Borrar(Id);
                        mensaje = "Usuario Eliminado de la base de datos";
                        break;
                    case EstadoEntidad.Modificado:
                        ClienteRepositorio.Editar(DatosClienteModelo);
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

        public List<ClienteModelo> GetAll()
        {
            var ClienteModelo = ClienteRepositorio.GetAll();
            ListaClientes = new List<ClienteModelo>();
            foreach (ClientesElektra item in ClienteModelo)
            {
                ListaClientes.Add(new ClienteModelo
                {
                    Id = item.Id,
                    ClienteUnico = item.ClienteUnico,
                    Nombre = item.Nombre,
                    ApellidoPaterno = item.ApPaterno,
                    ApellidoMaterno = item.ApMaterno
                    
                });
                
            }
            return ListaClientes;
        }

        public IEnumerable<ClienteModelo> BuscarId(string filter)
        {
            return ListaClientes.FindAll(e => e.ClienteUnico.ToString().Contains(filter) || e.Nombre.Contains(filter) || e.ApellidoPaterno.Contains(filter));
        }
    }
}
