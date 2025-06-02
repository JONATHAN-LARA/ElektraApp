using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comun.Cache
{
    public static class UsuarioActivo
    {
         public static int C_idUsuario { get; set; }
             public static string C_Nombre {get; set;}
             public static string C_ApellidoPaterno {get; set;}
             public static string C_ApellidoMaterno {get; set;}
             public static string C_LoginUser {get; set;}
             public static string C_Contraseña {get; set;}
             public static string C_Email {get; set;}
             public static int C_IdRol {get; set;}
    }
}
