using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Comun.Cache;
using Dominio.Modelos;

namespace Presentacion.Formularios
{
    public partial class FormLogin : Form
    {

        public FormLogin()
        {
            InitializeComponent();
        }       

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text == "USUARIO")
            {
                Text = "";
                txtUser.Text = Text;
                txtUser.ForeColor = Color.White;
                lblErrorMessage.Visible = false;
            }
        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                txtUser.Text = "USUARIO";
                txtUser.ForeColor = Color.Silver;
                lblErrorMessage.Visible = false;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "CONTRASEÑA")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.White;
                lblErrorMessage.Visible = false;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "CONTRASEÑA";
                txtPassword.ForeColor = Color.Silver;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToShortTimeString();
            lblFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        
        private void mensaje_Error(string msjError)
        {
            lblErrorMessage.Text = "    " + msjError;
            lblErrorMessage.Visible = true;
        }

        private void login()
        {
            if (txtUser.Text != "USUARIO" && txtUser.TextLength > 2)
            {
                if (txtPassword.Text != "CONTRASEÑA")
                {
                    UsuarioModelo userlogin = new UsuarioModelo();
                    var validar = userlogin.logIn(txtUser.Text, txtPassword.Text);
                    if (validar == "concedido")
                    {
                        this.Hide();
                        FormBienvenida bienvenido = new FormBienvenida();
                        bienvenido.ShowDialog();
                        FormMenuPrincipal menuPrincipal = new FormMenuPrincipal();
                        menuPrincipal.Show();
                        menuPrincipal.FormClosed += Logout;
                        
                    }
                    if (validar == "falso")
                    {
                        mensaje_Error("Usuario o contraseña Incorrectos.\n " + "   Por favor intentalo nuevamente.");
                        txtPassword.Text = "CONTRASEÑA";
                        txtPassword.ForeColor = Color.Silver;
                        txtPassword.UseSystemPasswordChar = false;
                        txtUser.Focus();
                    }
                    if(validar != "falso" && validar != "concedido") 
                    {
                        MessageBox.Show(validar, "SICC...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    mensaje_Error("Ingrese su contraseña");
                }
            }
            else
            {
                mensaje_Error("Ingrese su Usuario");
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void Logout(object sender, FormClosedEventArgs e)
        {
            //reiniciar formulario login

            txtPassword.Text = "CONTRASEÑA";
            txtPassword.UseSystemPasswordChar = false;
            txtUser.Text = "USUARIO";
            lblErrorMessage.Visible = false;
            //limpiar cache de usuario
            UsuarioActivo.C_idUsuario = 0;
            UsuarioActivo.C_Nombre = null;
            UsuarioActivo.C_ApellidoPaterno= null;
            UsuarioActivo.C_ApellidoMaterno = null;
            UsuarioActivo.C_LoginUser = null;
            UsuarioActivo.C_Contraseña = null;
            UsuarioActivo.C_Email= null;
            UsuarioActivo.C_IdRol= 0 ;
           
            //mostrar formulario
            this.Show();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            pictureBox2.Visible = false;
        }       
    }
}
