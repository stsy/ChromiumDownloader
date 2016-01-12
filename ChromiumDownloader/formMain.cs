using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using ChromiumDownloader.Classes;

namespace ChromiumDownloader
{
    public partial class formMain : Form
    {

        private string fileURI, fileURL, lastChangeURL, currentPath, fileName, lastDownloaded;

        public formMain()
        {
            InitializeComponent();


            fileURL = "http://commondatastorage.googleapis.com/chromium-browser-snapshots/Win/";
            lastChangeURL = "http://commondatastorage.googleapis.com/chromium-browser-snapshots/Win/LAST_CHANGE";
            currentPath = Directory.GetCurrentDirectory() + @"\";
            fileName = @"mini_installer.exe";
            lastDownloaded = Line.Read(currentPath + "LAST_CHANGE");
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            lastChangeURL = Download.sourceCode(lastChangeURL);
            buttonDownload.Text = "Download";
            toolStripStatusLabel1.Text = toolStripStatusLabel1.Text + lastChangeURL;
            toolStripStatusLabel2.Text = toolStripStatusLabel2.Text + fileName;

            if (lastChangeURL != lastDownloaded)
            {
                toolStripStatusLabel3.ForeColor = Color.Green;
                toolStripStatusLabel3.Text = "[new!]";
            }
                
            fileURI = fileURL + lastChangeURL + "/" + fileName;
            textBox1.Text = fileURI;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (lastChangeURL != null)
                {
                    if (buttonDownload.Text == "Download")
                    {
                        buttonDownload.Text = "Working..";
                        buttonDownload.Enabled = false;
                        downloadFile(fileURI, currentPath + fileName);
                    }
                    else if (buttonDownload.Text == "Close")
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    textBox1.Text = "Not found";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        public void downloadFile(string urlAddress, string location)
        {
            using (var client = new WebClient())
            {

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                try
                {
                    var URL = new Uri(urlAddress);
                    client.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }  

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                if (progressBar1.Value != e.ProgressPercentage && textBox1.Text != e.ProgressPercentage.ToString())
                {
                    progressBar1.Value = e.ProgressPercentage;
                    textBox1.Text =  "Downloading: " + e.ProgressPercentage.ToString() + "%";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                //File.Delete(currentPath + fileName);
                //MessageBox.Show("Canceled");
            }
            else
            {
                buttonDownload.Enabled = true;
                buttonDownload.Text = "Close";
                Line.Write(lastChangeURL, currentPath, "LAST_CHANGE");

            }
        }
    }
}
