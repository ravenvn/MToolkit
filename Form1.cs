using MToolkit.Helpers;
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

namespace MToolkit
{
    public partial class Form1 : Form
    {
        private WebServer ws;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                btnStart.Text = "Stop";
                ws = new WebServer(Login, "http://localhost:8080/login-gmail/");
                ws.Run();
            }
            else
            {
                btnStart.Text = "Start";
                ws.Stop();
                Console.WriteLine("Webserver stopped.");
            }
        }

        private static string Login(HttpListenerRequest request)
        {
            return LoginHelper.Login(request.QueryString["email"], request.QueryString["password"], request.QueryString["recovery_email"]); 
        }

        private void BtnCleanAllProcess_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
        }
    }
}
