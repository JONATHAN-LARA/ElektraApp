﻿using AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Contratos
{
    public interface IUsuarioRepositorio : IRepositorioGenerico<Usuarios>
    {
       bool Login(string user, string pass);
    }
}
