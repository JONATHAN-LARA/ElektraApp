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

    public partial class FormUsuarioNuevo : Form
    {
        RolModelo Rol = new RolModelo();
        UsuarioModelo Usuario = new UsuarioModelo();

        public FormUsuarioNuevo()
        {
            InitializeComponent();
        }

        private void FormUsuarioNuevo_Load(object sender, EventArgs e)
        {
          

        }      

        private void txtIdEvento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario.IdUsuario = Convert.ToInt32(txtIdUsuario.Text.ToString());
                Usuario.Nombre1 = txtNombre.Text.ToString();
                Usuario.ApellidoPater1 = txtApellidoPaterno.Text.ToString();
                Usuario.ApellidoMater1 = txtApellidoMaterno.Text.ToString();
                Usuario.LoginUser1 = txtLoginUser.Text.ToString();
                Usuario.Password1 = txtPassword.Text.ToString();
                Usuario.Email = txtCorreo.Text.ToString();
                Usuario.IdRol = cmbRol.SelectedValue.ToString();


                bool validar = new Soporte.ValidacionDatos(Usuario).validar();
                if (validar == true)
                {
                    Usuario.Estado = Dominio.ObjetosDeValores.EstadoEntidad.Modificado;
                    string result = Usuario.GuardarCambios();
                    MessageBox.Show(result, "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Usuario.GetAll();
                    resetForm();
                    this.Close();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Favor de llenar los campos obligatorios.", "SICC...  ADVERTENCIA",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario.IdUsuario = Convert.ToInt32(txtIdUsuario.Text.ToString());
                Usuario.Nombre1 = txtNombre.Text.ToString();
                Usuario.ApellidoPater1 = txtApellidoPaterno.Text.ToString();
                Usuario.ApellidoMater1= txtApellidoMaterno.Text.ToString();
                Usuario.LoginUser1 = txtLoginUser.Text.ToString();
                Usuario.Password1 = txtPassword.Text.ToString();
                Usuario.Email = txtCorreo.Text.ToString();
                Usuario.IdRol = cmbRol.SelectedValue.ToString();


                bool validar = new Soporte.ValidacionDatos(Usuario).validar();
                if (validar == true)
                {
                    string result = Usuario.GuardarCambios();
                    MessageBox.Show(result, "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Usuario.GetAll();
                    resetForm();
                    this.Close();
                }


            }
            catch (FormatException)
            {
                MessageBox.Show("Favor de llenar los campos obligatorios.", "SICC...  ADVERTENCIA",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resetForm()
        {
            txtIdUsuario.Text = "";
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtLoginUser.Text = "";
            txtPassword.Text = "";
            txtCorreo.Text = "";


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
