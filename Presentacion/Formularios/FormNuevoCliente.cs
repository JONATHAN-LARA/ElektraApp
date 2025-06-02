using Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion.Formularios
{

    public partial class FormNuevoCliente : Form
    {

        ClienteModelo ClienteElektra = new ClienteModelo();
      

        public FormNuevoCliente()
        {
            InitializeComponent();
        }
      
        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ClienteElektra.ClienteUnico1 = txtClienteUnico.Text.ToString();
                ClienteElektra.Nombre1 = txtNombre.Text.ToString();
                ClienteElektra.ApellidoPaterno1 = txtApellidoPaterno.Text.ToString();
                ClienteElektra.ApellidoMaterno1 = txtApellidoMaterno.Text.ToString();

                bool validar = new Soporte.ValidacionDatos(ClienteElektra).validar();
                if (validar == true)
                {
                    string result = ClienteElektra.GuardarCambios();
                    MessageBox.Show(result, "SICC...  ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClienteElektra.GetAll();                    
                    resetForm();
                    this.Close();
                }


            }
            catch(FormatException)
            {
                MessageBox.Show("Favor de llenar los campos obligatorios.", "SICC...  ADVERTENCIA",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void resetForm()
        {
            txtClienteUnico.Text = "";
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";

        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {               
                ClienteElektra.ClienteUnico1 = txtClienteUnico.Text.ToString();
                ClienteElektra.Nombre1 = txtNombre.Text.ToString();
                ClienteElektra.ApellidoPaterno1 = txtApellidoPaterno.Text.ToString();
                ClienteElektra.ApellidoMaterno1 = txtApellidoMaterno.Text.ToString();
                ClienteElektra.Id1 =Convert.ToInt32(txtId.Text.ToString());

                bool validar = new Soporte.ValidacionDatos(ClienteElektra).validar();
                if (validar == true)
                {
                    ClienteElektra.Estado = Dominio.ObjetosDeValores.EstadoEntidad.Modificado;
                    string result = ClienteElektra.GuardarCambios();
                    MessageBox.Show(result, "SICC...  ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClienteElektra.GetAll();
                    resetForm();
                    this.Close();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Favor de llenar los campos obligatorios.", "...  ADVERTENCIA",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtClienteUnico_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
