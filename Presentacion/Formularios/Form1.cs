using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblNewVersion.Hide();
            labelNewVersion.Hide();
            bgw_UpdateChecker.RunWorkerAsync();
            lblCurrentVersion.Text = Application.ProductVersion.ToString();
        }

        private void CheckUpdate()
        {
            var urlVersion = "https://www.dropbox.com/scl/fi/a9i1p5dksq6oqwi52u0ub/version.txt?rlkey=kgwnvh340or6tlsrdxl76qzvc&st=g76ahn1x&dl=1";
            var newVersion = (new WebClient().DownloadString(urlVersion));
            var currentVersion = Application.ProductVersion.ToString();

            newVersion = newVersion.Replace(".", "");
            currentVersion = currentVersion.Replace(".", "");

            this.Invoke(new Action(() =>
            {
                if (Convert.ToInt32(newVersion) > Convert.ToInt32(currentVersion))
                {
                    lblHeader.Text = "Hay una nueva versión disponible, \r\n¿Desea Actualizarla?\r\n";
                    lblNewVersion.Text = (new WebClient().DownloadString(urlVersion));
                    btnActualizar.Enabled = true;
                    lblNewVersion.Show();
                    labelNewVersion.Show();

                }
                else
                {
                    lblHeader.Text = "Su sistema esta actualizado a la versión mas reciente";
                    btnActualizar.Enabled = false;
                    lblNewVersion.Hide();
                    labelNewVersion.Hide();
                }
            }));          

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            WebClient web = new WebClient();
            web.DownloadFileCompleted += web_DownloadFileCompleted;
            web.DownloadFileAsync(new Uri("https://www.dropbox.com/scl/fi/55iqi4fnexc99ibbbnkty/update.msi?rlkey=st04ycsuir7uoberpb2m5ghwy&st=3941odua&dl=1"), "C:\\Users\\Jonathan Lara Corzo\\Desktop\\AutoUpdate\\Update\\update.msi");

        }

        private void web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            initScript();
        }

        private void initScript()
        {
            try
            {
                string path = Application.StartupPath + @"\bat.bat";

                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.Arguments = "";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.Verb = "runas";
                p.Start();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void bgw_UpdateChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckUpdate();
        }

        private void bgw_UpdateChecker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgw_UpdateChecker.RunWorkerAsync();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

