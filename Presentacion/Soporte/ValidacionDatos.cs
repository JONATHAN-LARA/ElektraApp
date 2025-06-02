using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presentacion.Soporte
{
    public class ValidacionDatos
    {
        private ValidationContext context;
        private List<ValidationResult> Results;
        private bool valido;
        private string mensaje;

        public ValidacionDatos(object instancia)
        {
            context = new ValidationContext(instancia);
            Results = new List<ValidationResult>();
            valido = Validator.TryValidateObject(instancia, context, Results, true);
        }

        public bool validar()
        {
            if (valido == false)
            {
                foreach (ValidationResult item in Results)
                {
                    mensaje += item.ErrorMessage + "\n";
                }
                System.Windows.Forms.MessageBox.Show(mensaje, "SICC... ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return valido;
        }
    }
}
