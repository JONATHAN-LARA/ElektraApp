using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpdate;
using Comun.Cache;
using Dominio.Modelos;
using Dominio.ObjetosDeValores;

namespace Presentacion.Formularios
{
    public partial class FormMenuPrincipal : Form
    {

        UsuarioModelo Usuario = new UsuarioModelo();
        ClienteModelo ClienteElektra = new ClienteModelo();


        public FormMenuPrincipal()
        {
            InitializeComponent();

            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;

        }
        //metodo para arrastrar el formulario

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        //RESIZE METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO EN TIEMPO DE EJECUCION ----------------------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private System.Drawing.Rectangle sizeGripRectangle;

        public void AddDataGridUsuarios()
        {
            try
            {
                var Lista = Usuario.GetAll().OrderBy(x => x.Nombre1).ToList();               
                DataGVUsuarios.DataSource = Lista;
                



            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddDataGridClientesElektra()
        {
            try
            {             
                var lista = ClienteElektra.GetAll().OrderBy(x => x.Nombre1).ToList();
                dataGVClientesElektra.DataSource = lista;

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new System.Drawing.Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new System.Drawing.Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        //capturar posicion y tamaño antes de restaurar

        int lx, ly;
        int sw, sh;

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void lblLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            AddDataGridUsuarios();
            AddDataGridClientesElektra();
            OcultarColumna();
            timer1.Enabled = true;
            ManagePermissions();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToShortTimeString();
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;

            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;

            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
        }

        private void BtnVerUsuarios_Click(object sender, EventArgs e)
        {
            DataGVUsuarios.Visible = true;
            dataGVClientesElektra.Visible = false;
            BtnVerClientes.Visible = true;
            BtnVerUsuarios.Visible = false;

        }

        private void BtnClientes_Click(object sender, EventArgs e)
        {
            DataGVUsuarios.Visible = false;
            dataGVClientesElektra.Visible = true;
            BtnVerClientes.Visible = false;
            BtnVerUsuarios.Visible = true;
        }

        private void btnClienteNuevo_Click(object sender, EventArgs e)
        {

            FormNuevoCliente frm = new FormNuevoCliente();            
            frm.ShowDialog();
            
            frm.BringToFront();
            AddDataGridClientesElektra();

        }


        private void FormClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnUsuarioNuevo_Click(object sender, EventArgs e)
        {
            FormUsuarioNuevo frm = new FormUsuarioNuevo();
            RolModelo Rol = new RolModelo();
           

            var listaroles = Rol.GetAll();

            frm.cmbRol.DisplayMember = "NombreRol1";
            frm.cmbRol.ValueMember = "IdRol";
            frm.cmbRol.DataSource = listaroles;
            frm.ShowDialog();
            frm.BringToFront();
            AddDataGridUsuarios();
    

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            dataGVClientesElektra.DataSource = ClienteElektra.BuscarId(txtUser.Text);
            DataGVUsuarios.DataSource = Usuario.BuscarId(txtUser.Text);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGVClientesElektra.SelectedRows.Count > 0)
            {
                var Nombre = dataGVClientesElektra.CurrentRow.Cells[2].Value.ToString();
                var IdClienteUnico = dataGVClientesElektra.CurrentRow.Cells[1].Value.ToString();
               
                DialogResult resultado = MessageBox.Show("¿Estas seguro de querer eliminar al Cliente: " + Nombre + "\n con número de Cliente Unico : " + IdClienteUnico + "?.", "Mensaje de SISTEMA...  ADVERTENCIA",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    ClienteElektra.Id1 = Convert.ToInt32(dataGVClientesElektra.CurrentRow.Cells[0].Value);
                    ClienteElektra.Estado = EstadoEntidad.Eliminado;
                    var mensaje = ClienteElektra.GuardarCambios();
                    MessageBox.Show(mensaje, "SICC...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddDataGridClientesElektra();
                }
            }
            else if (DataGVUsuarios.SelectedRows.Count > 0)
            {
                var Nombre = DataGVUsuarios.CurrentRow.Cells[1].Value.ToString();
                var ApePater = DataGVUsuarios.CurrentRow.Cells[2].Value.ToString();
                var ApeMater = DataGVUsuarios.CurrentRow.Cells[3].Value.ToString();
                var IdUsuario = DataGVUsuarios.CurrentRow.Cells[0].Value.ToString();
               

                DialogResult resultado = MessageBox.Show("¿Estas seguro de querer eliminar al Usuario: " + Nombre + " " + ApePater + " " + ApeMater + "\n con número de Cliente Unico : " + IdUsuario + "?.", "Mensaje de SISTEMA...  ADVERTENCIA",
               MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    Usuario.IdUsuario = Convert.ToInt32(DataGVUsuarios.CurrentRow.Cells[0].Value.ToString());
                    Usuario.Estado = EstadoEntidad.Eliminado;
                    var mensaje = Usuario.GuardarCambios();
                    MessageBox.Show(mensaje, "SICC...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddDataGridUsuarios();
                }
            }
            else
            {
                var mensaje = "Seleccione una fila";
                MessageBox.Show(mensaje, "S I C C...",
                   MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;

            this.Location = new Point(lx, ly);
            this.Size = new Size(sw, sh);
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {           
            if (dataGVClientesElektra.SelectedRows.Count > 0)
            {
                FormNuevoCliente form = new FormNuevoCliente();
                form.txtClienteUnico.Text = dataGVClientesElektra.CurrentRow.Cells[1].Value.ToString();
                form.txtNombre.Text = dataGVClientesElektra.CurrentRow.Cells[2].Value.ToString();
                form.txtApellidoPaterno.Text = dataGVClientesElektra.CurrentRow.Cells[3].Value.ToString();
                form.txtApellidoMaterno.Text = dataGVClientesElektra.CurrentRow.Cells[4].Value.ToString();
                form.txtId.Text = dataGVClientesElektra.CurrentRow.Cells[0].Value.ToString();
                form.btnGuardar.Visible = false;
                form.btnGuardarCambios.Visible = true;
                form.ShowDialog();
               
                form.BringToFront();
                AddDataGridClientesElektra();

            }
            else if (DataGVUsuarios.SelectedRows.Count > 0)
            {
                RolModelo Rol = new RolModelo();
                FormUsuarioNuevo FormUser = new FormUsuarioNuevo();


                var listaroles = Rol.GetAll();

                FormUser.cmbRol.DisplayMember = "NombreRol1";
                FormUser.cmbRol.ValueMember = "IdRol";
                FormUser.cmbRol.DataSource = listaroles;


                FormUser.txtIdUsuario.Text = DataGVUsuarios.CurrentRow.Cells[0].Value.ToString();
                FormUser.txtNombre.Text = DataGVUsuarios.CurrentRow.Cells[1].Value.ToString();
                FormUser.txtApellidoPaterno.Text = DataGVUsuarios.CurrentRow.Cells[2].Value.ToString();
                FormUser.txtApellidoMaterno.Text = DataGVUsuarios.CurrentRow.Cells[3].Value.ToString();
                FormUser.txtLoginUser.Text = DataGVUsuarios.CurrentRow.Cells[4].Value.ToString();
                FormUser.txtPassword.Text = DataGVUsuarios.CurrentRow.Cells[5].Value.ToString();
                FormUser.txtCorreo.Text = DataGVUsuarios.CurrentRow.Cells[6].Value.ToString();
                FormUser.cmbRol.SelectedValue = Convert.ToInt32(DataGVUsuarios.CurrentRow.Cells[7].Value.ToString());
                FormUser.btnGuardarCambios.Visible = true;
                FormUser.ShowDialog();
                
                FormUser.BringToFront();
                AddDataGridUsuarios();
            }
            else
            {
                var mensaje = "Seleccione una fila";
                MessageBox.Show(mensaje, "ADVERTENCIA...",
                   MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            CambiarVisivilidadPanelSubmenu();
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            CambiarVisivilidadPanelSubmenu();
        }

        private void ManagePermissions()
        {

            //Manage Permissions
            if (UsuarioActivo.C_IdRol == Rol.Empleado)
            {
                BtnUsuarioNuevo.Visible = false;
                BtnVerClientes.Visible = false;
                BtnVerUsuarios.Visible = false;
                DataGVUsuarios.Visible = false;
                dataGVClientesElektra.Visible = true;
                lblLogin.Text = UsuarioActivo.C_Nombre.ToUpper() + " " + UsuarioActivo.C_ApellidoPaterno.ToUpper() + " " + UsuarioActivo.C_ApellidoMaterno.ToUpper() + '\n' + UsuarioActivo.C_Email.ToLower();
                lblRolPosition.Text = "Empleado";
                lblUserName.Text = UsuarioActivo.C_LoginUser;
                lblEmail.Text = UsuarioActivo.C_Email.ToLower();


            }
            else
            {
                lblLogin.Text = UsuarioActivo.C_Nombre.ToUpper() + " " + UsuarioActivo.C_ApellidoPaterno.ToUpper() + " " + UsuarioActivo.C_ApellidoMaterno.ToUpper() + '\n' + UsuarioActivo.C_Email.ToLower(); ;
                lblRolPosition.Text = "Administrador";
                lblUserName.Text = UsuarioActivo.C_LoginUser;
                lblEmail.Text = UsuarioActivo.C_Email.ToLower();
            }
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
        }      

        private void dataGVClientesElektra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //var valor = (dataGVClientesElektra.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                var valor = dataGVClientesElektra.CurrentCell.Value.ToString();
                Clipboard.SetText(valor);
                notifyIcon1.BalloonTipText = "ver detalles";
                notifyIcon1.BalloonTipTitle = "Cliente Unico: " + valor + " copiado";                
                notifyIcon1.ShowBalloonTip(100);
                //System.Diagnostics.Process.Start("https://app.prontipagos.mx/pdv/venta/servicio/categoria/referencia");



            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                //MessageBox.Show($"Error: {argumentOutOfRangeException.Message}");
            }
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text.Length >= 1)
            {
                Text = "";
                txtUser.Text = Text;
                txtUser.ForeColor = Color.Black;

            }
           
        }

        private void btnActualizaciones_Click(object sender, EventArgs e)
        {
            Form1 FormActualizar = new Form1();
            FormActualizar.ShowDialog();
        }

        private void linkpaginaweb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jheviservices.com/");
        }

        public void OcultarColumna()
        {
            dataGVClientesElektra.Columns[0].Visible = false;
            
        }

        private void CambiarVisivilidadPanelSubmenu()
        {
            if (panelSubMenu.Visible == false)
            {
                panelSubMenu.Visible = true;

                //lblNomUserLogin.Text = UsuarioActivo.C_loginUser;
                //lblRolPosition.Text = UsuarioActivo.C_Rol_idRol == 1 ? "ADMINISTRADOR" : "OFICINISTA";
                //lblEmail.Text = UsuarioActivo.C_email_2;

                panelSubMenu.BringToFront();
            }
            else
            {
                panelSubMenu.Visible = false;
            }

        }
    }
}
