using MToolkit.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MToolkit
{
    public partial class Form1 : Form
    {
        private LoginHelper loginHelper = new LoginHelper();
        private ViewHelper viewHelper = new ViewHelper();
        private WebServer ws;
        public Form1()
        {
            InitializeComponent();

            LoadConfigs();
        }

        private void LoadConfigs()
        {
            txtPageLoad.Text = Helper.config.Page_Load.ToString();
            chbLogError.Checked = Helper.config.Log_Error != 0;
            txtEnterLoad.Text = Helper.config.Enter_Load.ToString();
            txtManualLoad.Text = Helper.config.Manual_Load.ToString();
            txtManageSiteUrl.Text = Helper.config.Manage_Site_Url;
            txtActionSleep.Text = Helper.config.Action_Sleep.ToString();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                btnStart.Text = "Stop";
                ws = new WebServer(Response, "http://127.0.0.1:8080/", "http://localhost:8080/");
                ws.Run();
            }
            else
            {
                btnStart.Text = "Start";
                ws.Stop();
                Console.WriteLine("Webserver stopped.");
            }
        }

        private string Response(HttpListenerRequest request)
        {
            switch (request.Url.AbsolutePath)
            {
                case "/login-gmail":
                    return loginHelper.Login(
                        request.QueryString["profile"],
                        request.QueryString["email"],
                        request.QueryString["password"],
                        request.QueryString["recovery_email"]
                    );
                case "/manual-login":
                    return loginHelper.ManualLogin(request.QueryString["profile"], request.QueryString["email"], request.QueryString["password"]);
                //case "/login-by-cookie":
                //    return loginHelper.LoginByCookie(request.QueryString["cookie"]);
                case "/bots/auto-view":
                    var accounts = JsonConvert.DeserializeObject<Account[]>(request.QueryString["accounts"]);
                    byte[] bytes = Encoding.Default.GetBytes(request.QueryString["title_video_ids"]);
                    var title_video_ids = Encoding.UTF8.GetString(bytes);
                    var data = new ViewData
                    {
                        Accounts = accounts,
                        FilterType = request.QueryString["filter_type"],
                        TitleVideoIds = title_video_ids,
                        DurationMin = Int32.Parse(request.QueryString["duration_min"]),
                        DurationMax = Int32.Parse(request.QueryString["duration_max"]),
                        Sub = request.QueryString["sub"] == "1" ? true : false,
                        SubRatio = Int32.Parse(request.QueryString["sub_ratio"]),
                        Like = request.QueryString["like"] == "1" ? true : false,
                        LikeRatio = Int32.Parse(request.QueryString["like_ratio"])
                    };
                    return viewHelper.AutoView(data);
                case "/bots/stop-auto-view":
                    return viewHelper.StopAutoView();
                default:
                    return string.Empty;
            }    
        }

        private void BtnCleanAllProcess_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("geckodriver"))
            {
                process.Kill();
            }
            
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
            
            MessageBox.Show("Đã dọn sạch hết các tiến trình chạy ngầm");
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/LuDucQuan83");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/nanyangbk");
        }

        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            Helper.config.Page_Load = Int32.Parse(txtPageLoad.Text);
            Helper.config.Enter_Load = Int32.Parse(txtPageLoad.Text);
            Helper.config.Log_Error = chbLogError.Checked ? 1 : 0;
            Helper.config.Manual_Load = Int32.Parse(txtManualLoad.Text);
            Helper.config.Manage_Site_Url = txtManageSiteUrl.Text.Trim();
            Helper.config.Action_Sleep = Int32.Parse(txtActionSleep.Text);
            var configJson = JsonConvert.SerializeObject(Helper.config);
            var jsonFormatted = JValue.Parse(configJson).ToString(Formatting.Indented);

            File.WriteAllText("Configs.json", jsonFormatted);

            MessageBox.Show("Đã lưu thành công", "Thành công");
        }
    }
}
