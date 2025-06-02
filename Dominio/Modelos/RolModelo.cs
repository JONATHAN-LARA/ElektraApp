using AccesoDatos.Contratos;
using AccesoDatos.Entidades;
using AccesoDatos.Repositorios;
using Dominio.ObjetosDeValores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos
{
    public class RolModelo
    {
        private int idRol;
        private string NombreRol;

        private IRolRepositorio RolRepositorio;
        public EstadoEntidad Estado;
        private List<RolModelo> ListRoles;

        public int IdRol { get => idRol; set => idRol = value; }
        public string NombreRol1 { get => NombreRol; set => NombreRol = value; }

        public RolModelo()
        {
            RolRepositorio = new RepositorioRol();
        }

        public string GuardarCambios()
        {
            string Mensaje = null;

            try
            {
                var RolDataModel = new Rol();
                RolDataModel.IdRol = idRol;
                RolDataModel.NombreRol = NombreRol;

                switch (Estado)
                {
                    case EstadoEntidad.Agregado:
                        RolRepositorio.Agregar(RolDataModel);
                        Mensaje = "Rol agregado sastifactoriamente en la base de datos";
                        break;
                    case EstadoEntidad.Eliminado:
                        RolRepositorio.Borrar(idRol);
                        Mensaje = "Rol eliminado de la base de datos";
                        break;
                    case EstadoEntidad.Modificado:
                        RolRepositorio.Editar(RolDataModel);
                        Mensaje = "Rol Modificado";
                        break;
                }
            }
            catch(Exception ex)
            {
                Mensaje = ex.ToString();
            }
            return Mensaje;
        }

        public List<RolModelo> GetAll()
        {
            var RolModelo = RolRepositorio.GetAll();
            ListRoles = new List<RolModelo>();
            foreach (Rol item in RolModelo)
            {
                ListRoles.Add(new RolModelo
                {
                    idRol = item.IdRol,
                    NombreRol = item.NombreRol
                });
            }
            return ListRoles;
        }
    }
}
