using System;

using System.Net;
using System.IO;
using System.IO;
using System.Windows.Forms;

namespace FTPManager
{
    public class FTPManagerClass
    {
        private static string userName = "";
        private static string password = "";
        private static string _host = "";

        private FtpWebRequest ftpWebRequest = null;
        private FtpWebResponse ftpWebResponse = null;
        private Stream ftpStream = null;

        // When info FTP download into your computer, your data needs some time to 
        // process the incoming information.
        // The amount of time allotted for processing is called the Buffer Size.
        private int bufferSize = 2048;

        public FTPManagerClass(string user, string pass, string host)
        {
            userName = user;
            password = pass;
            _host     = host;
        }

        public void downloadFile(string remoteFile, string localFile)
        {
            try
            {
                ftpWebRequest = (FtpWebRequest) FtpWebRequest.Create(_host + "/" + remoteFile);
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpStream = ftpWebResponse.GetResponseStream();

                FileStream fs = new FileStream(localFile, FileMode.OpenOrCreate);
                byte[] byteBuffer = new byte[Convert.ToInt32(getFileSize(remoteFile))];
                int bytesRead = ftpStream.Read(byteBuffer, 0, Convert.ToInt32(getFileSize(remoteFile)));

                try
                {
                    while(bytesRead > 0)
                    {
                        fs.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0,Convert.ToInt32(getFileSize(remoteFile)) );

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                fs.Close();
                ftpStream.Close();
                ftpWebResponse.Close();
                ftpWebRequest = null;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void uploadFile(string remoteFile, string localFile)
        {
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + remoteFile);
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream lfs = new FileStream(localFile, FileMode.Open);
                byte[] byteBuffer = new byte[(int)lfs.Length];
                int bytesSend = lfs.Read(byteBuffer, 0, (int)lfs.Length);

                try
                {
                    while (bytesSend != -1)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSend);
                        bytesSend = lfs.Read(byteBuffer, 0, (int)lfs.Length);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                lfs.Close();
                ftpStream.Close();
                ftpWebResponse.Close();
                ftpWebRequest = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void rename(string oldName, string newName)
        {
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + oldName);
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpWebRequest.RenameTo = newName;

                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpWebResponse.Close();
                ftpWebRequest = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string[] getFileOnServer(String dir)
        {
            string[] fileInDir = new string[50]; ;
            try
            {
                
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + dir);
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;

                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpStream = ftpWebResponse.GetResponseStream();

                StreamReader streamReader = new StreamReader(ftpStream);
                string dirRaw = null;

                try
                {
                    while(streamReader.Peek() != -1)
                    {
                        dirRaw += streamReader.ReadLine() + "|";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                streamReader.Dispose();
                ftpStream.Close();
                ftpWebResponse.Close();
                ftpWebRequest = null;

                try
                {
                    fileInDir = dirRaw.Split("|".ToCharArray());
                    return fileInDir;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return fileInDir;
        }

        public void Delete(string fileName)
        {
            try
            {

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + fileName);
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse.Close();
                ftpWebRequest = null;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void creatDir(string fileName)
        {
            try
            {

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + fileName);
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = true;
                ftpWebRequest.KeepAlive = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpWebResponse.Close();
                ftpWebRequest = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public long getFileSize(string remoteFile)
        {
            long size;

            FtpWebRequest sizeRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + remoteFile);
            sizeRequest.Credentials = new NetworkCredential(userName, password);
            sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            sizeRequest.UseBinary = true;

            FtpWebResponse serverResponse = (FtpWebResponse)sizeRequest.GetResponse();
            FtpWebResponse sizeResponse = (FtpWebResponse)sizeRequest.GetResponse();

            size = sizeResponse.ContentLength;
            return size;
        }
    }
}
