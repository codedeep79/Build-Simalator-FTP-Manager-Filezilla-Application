using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTPManager;

namespace FTPManagerApp
{
    public partial class Form1 : Form
    {
        FTPManagerClass client;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtAdmin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnGetFile_Click(object sender, EventArgs e)
        {
            string[] files = client.getFileOnServer("");
            foreach(string fileName in files)
            {
                dataGridView.Rows.Add(fileName);
            }
        }

        private void txtLogin_Click(object sender, EventArgs e)
        {
            client = new FTPManagerClass(txtAdmin.Text, txtPassword.Text, txtHost.Text);
        }


        /// <summary>
        /// Download file or folder from host
        /// </summary>
        /// <param name="txtBrowse1">Input file name will save info download 
        /// before press browse 1 button
        /// </param>
        /// <param name="btnBrowse1">press Save and choose file which will save Info Download</param>
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string ext = txtBrowse1.Text.Substring(txtBrowse1.Text.IndexOf("."));

            sfd.Filter = "Download file extension(" + ext + ")|*" + ext;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDownload.Text = sfd.FileName;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            client.downloadFile(txtBrowse1.Text, txtDownload.Text);
        }

        /// <summary>
        /// Upload file of folder to host
        /// </summary>
        /// <param name="txtBrowse2">ofd.FileName include directory</param>
        /// <param name="txtUpload">ofd.SafeFileName only file name</param>
        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtBrowse2.Text = ofd.FileName;
                txtUpload.Text = ofd.SafeFileName;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            client.uploadFile(txtBrowse2.Text, txtUpload.Text);
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            client.rename(txtOldName.Text, txtNewName.Text);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            client.creatDir(txtCreateDirectory.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            client.Delete(txtDelete.Text);
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToString(dataGridView.Rows[e.RowIndex].Cells[0].Value).Contains("."))
            {
                return;
            }
            else
            {
                string[] files = client.getFileOnServer(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                dataGridView.Rows.Clear();
                foreach (string file in files)
                {
                    dataGridView.Rows.Add(file);
                }
            }
        }
    }
}
