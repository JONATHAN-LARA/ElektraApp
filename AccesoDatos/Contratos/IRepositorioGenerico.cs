using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Contratos
{
    public interface IRepositorioGenerico<Entidad> where Entidad:class
    {
        int Agregar(Entidad entidad);
        int Editar(Entidad entidad);
        int Borrar(int Id);
        IEnumerable<Entidad> GetAll();

    }
}
