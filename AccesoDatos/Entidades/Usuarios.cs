using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Entidades
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPater { get; set; }
        public string ApellidoMater { get; set; }
        public string LoginUser { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string IdRol { get; set; }
    }
}
