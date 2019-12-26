using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using OpenQA.Selenium.Interactions;
using System.IO.Compression;

namespace MToolkit.Helpers
{
    class ViewHelper
    {
        private bool stopThread = false;
        private static readonly HttpClient client = new HttpClient();

        public void Test()
        {
            var driver = Helper.CreateFirefoxDriver(@"C:\Users\tin\AppData\Roaming\Mozilla\Firefox\Profiles\huamp3p3.Profile1");
            driver.Navigate().GoToUrl("https://24h.com.vn");
            Thread.Sleep(5000);
            driver.Navigate().GoToUrl("https://vnexpress.net");
            Thread.Sleep(5000);
            Helper.CloseBrowser(driver);
        }

        public string AutoView(ViewData data)
        {
            var response = new AutoViewResponse
            {
                Status = false,
                Detail_Reason = ""
            };

            try
            {
                stopThread = false;
                var thread = new Thread(() => ProcessAutoView(data));
                thread.Start();
                response.Status = true;
            }
            catch (Exception e)
            {
                response.Detail_Reason = e.Message;
            }

            return JsonConvert.SerializeObject(response);
        }

        private void ProcessAutoView(ViewData data)
        {
            try
            {
                var titleVideoIds = data.TitleVideoIds.Split('\n');
                var i = 0;
                var random = new Random();
                var errorAccountIds = new List<int>();

                while (true)
                {
                    foreach (var account in data.Accounts)
                    {
                        if (errorAccountIds.Contains(account.Id)) continue;
                        try
                        {
                            if (i == titleVideoIds.Length) i = 0;
                            var titleVideoId = titleVideoIds[i].Split('|');
                            var title = titleVideoId[0];
                            var videoId = titleVideoId[1];
                            var recommendedVideoId = titleVideoId[2];
                            var sub = data.Sub == false ? false : random.Next(1, 101) <= data.SubRatio;
                            var like = data.Like == false ? false : random.Next(1, 101) <= data.LikeRatio;
                            var status = AutoViewAccount(account, data.FilterType, title, videoId, recommendedVideoId, data.DurationMin, data.DurationMax, sub, like);
                            if (!status)
                            {
                                errorAccountIds.Add(account.Id);
                            }
                            i += 1;
                        }
                        catch (Exception e)
                        {
                            if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
                        }

                        // check continue running or not
                        if (stopThread) break;
                    }
                    if (errorAccountIds.Count == data.Accounts.Length)
                    {
                        stopThread = true;
                    }
                    if (stopThread) break;
                }
                
                // Todo: send status stop to server
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
            }
        }

        private bool AutoViewAccount(Account account, string filterType, string title, string videoId, string recommendedVideoId, int durationMin, int durationMax, bool sub, bool like)
        {
            var status = false;
            FirefoxDriver driver = null;
            try
            {
                var random = new Random();
                driver = Helper.CreateFirefoxDriver(account.Profile, account.Proxy, account.UserAgent);
                if (driver != null)
                {
                    driver.Navigate().GoToUrl("https://youtube.com");
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    var searchElement = driver.FindElementByXPath("//input[@id='search']");
                    searchElement.SendKeys(title);
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    searchElement.SendKeys(Keys.Enter);
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    driver.Navigate().GoToUrl(driver.Url + "&sp=" + filterType);
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));

                    var videoElement = driver.FindElementsById("video-title").Where(x => x.Displayed && x.GetAttribute("href") != null && x.GetAttribute("href").Contains(videoId)).First();
                    if (videoElement != null)
                    {
                        videoElement.Click();
                        var duration = random.Next(durationMin, durationMax + 1);
                        Thread.Sleep(TimeSpan.FromSeconds(duration));
                        UpdateDuration(videoId, duration);
                        // sub
                        if (sub)
                        {
                            try
                            {
                                var subscribeButton = driver.FindElementsById("subscribe-button").Where(x => x.Displayed).First();
                                if (subscribeButton != null)
                                {
                                    subscribeButton.Click();
                                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                                    var unsubModal = driver.FindElementsByXPath("//div[@id='scrollable']/yt-formatted-string[@class='line-text style-scope yt-confirm-dialog-renderer']").Where(x => x.Displayed).First();
                                    if (unsubModal != null)
                                    {
                                        // already sub
                                        var actions = new Actions(driver);
                                        actions.SendKeys(Keys.Escape).Perform();
                                    }
                                    else
                                    {
                                        // not sub
                                        var channelElement = driver.FindElementsByXPath("//div[@id='container']/div[@id='text-container']/yt-formatted-string[@id='text']/a[@class='yt-simple-endpoint style-scope yt-formatted-string']").Where(x => x.Displayed).First();
                                        if (channelElement != null)
                                        {
                                            var channelId = channelElement.GetAttribute("href").Split('/').Last();
                                            var channelName = channelElement.Text;
                                            IncreaseSub(channelId, channelName, videoId);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors", e.Message);
                            }
                        }

                        if (like)
                        {
                            var likeButton = driver.FindElementsByXPath("//button[@id='button']/yt-icon[@class='style-scope ytd-toggle-button-renderer']").Where(x => x.Displayed).First();
                            if (likeButton != null)
                            {
                                likeButton.Click();
                                Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                            }
                        }

                        try
                        {
                            // recommended video
                            var js = (IJavaScriptExecutor)driver;
                            var query = string.Format("document.querySelectorAll('[id=\"thumbnail\"]').forEach(function(e) {{ if (e.getAttribute('href') != null) e.setAttribute('href', '/watch?v={0}') }});", recommendedVideoId);
                            js.ExecuteScript(query);
                            var firstRightVideo = driver.FindElementsById("thumbnail").Where(x => x.Displayed && x.GetAttribute("href") != null).First();
                            var actions = new Actions(driver);
                            actions.KeyDown(Keys.Control)
                                    .Click(firstRightVideo)
                                    .KeyUp(Keys.Control)
                                    .Perform();
                            Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                            driver.Close();
                            driver.SwitchTo().Window(driver.WindowHandles.Last());
                            duration = random.Next(durationMin, durationMax + 1);
                            Thread.Sleep(TimeSpan.FromSeconds(duration));
                            UpdateDuration(recommendedVideoId, duration);
                        }
                        catch (Exception e)
                        {
                            if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
                        }


                        status = true;
                    }

                }
                else
                {
                    ReportAccountError(account.Id, "Proxy bị lỗi");
                }
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
                ReportAccountError(account.Id, e.Message);
            }

            Helper.CloseBrowser(driver, true);

            return status;
        }

        public string StopAutoView()
        {
            var response = new AutoViewResponse
            {
                Status = true,
                Detail_Reason = ""
            };

            stopThread = true;
            
            return JsonConvert.SerializeObject(response);
        }

        private void UpdateDuration(string videoId, int duration)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "video_id", videoId },
                    { "duration", duration.ToString() }
                };
                var content = new FormUrlEncodedContent(values);
                client.PostAsync(Helper.config.Manage_Site_Url + "/api/videos/update-duration", content);
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
            }
        }

        private void IncreaseSub(string channelId, string channelName, string videoId)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "channel_id", channelId },
                    { "channel_name", channelName },
                    { "video_id", videoId }
                };
                var content = new FormUrlEncodedContent(values);
                client.PostAsync(Helper.config.Manage_Site_Url + "/api/channels/increase-sub", content);
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
            }
        }
        
        private void ReportAccountError(int accountId, string error)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "account_id", accountId.ToString() },
                    { "error", error }
                };
                var content = new FormUrlEncodedContent(values);
                client.PostAsync(Helper.config.Manage_Site_Url + "/api/account/report-error", content);
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors.txt", e.Message);
            }
        }

        #region Draft
        private void CloseBrowser(IWebDriver driver, string profile = null)
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }
            if (profile != null)
            {
                ClearProxy(profile);
            }
        }

        private void ClearProxy(string profile)
        {
            try
            {
                var userPath = profile + @"\user.js";
                if (File.Exists(userPath))
                {
                    var oldLines = File.ReadAllLines(userPath);
                    var newLines = oldLines.Where(x => !x.Contains("network.proxy") && x.Trim() != "");
                    File.WriteAllLines(userPath, newLines);
                }
                else
                {
                    File.WriteAllText(userPath, "");
                }
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors", e.Message);
            }

        }

        private void AddProxy(string profile, string proxyAddress, string proxyPort)
        {
            try
            {
                var userPath = profile + @"\user.js";
                var proxyLines = new string[]
                {
                    string.Format("user_pref(\"network.proxy.backup.ftp\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.backup.ftp_port\", \"{0}\");", proxyPort),
                    string.Format("user_pref(\"network.proxy.backup.socks\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.backup.socks_port\", \"{0}\");", proxyPort),
                    string.Format("user_pref(\"network.proxy.backup.ssl\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.backup.ssl_port\", \"{0}\");", proxyPort),
                    string.Format("user_pref(\"network.proxy.backup.ftp\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.ftp\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.ftp_port\", \"{0}\");", proxyPort),
                    string.Format("user_pref(\"network.proxy.http\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.http_port\", \"{0}\");", proxyPort),
                    "user_pref(\"network.proxy.share_proxy_settings\", true);",
                    string.Format("user_pref(\"network.proxy.socks\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.socks_port\", \"{0}\");", proxyPort),
                    string.Format("user_pref(\"network.proxy.ssl\", \"{0}\");", proxyAddress),
                    string.Format("user_pref(\"network.proxy.ssl_port\", \"{0}\");", proxyPort),
                    "user_pref(\"network.proxy.type\", 1);"
                };

                File.AppendAllLines(userPath, proxyLines);
            }
            catch (Exception e)
            {
                if (Helper.config.Log_Error == 1) Helper.LogError("View_Errors", e.Message);
            }
        }
        private void CreateProxyExtension(string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            var background = File.ReadAllText(@"proxy\background.js");
            File.WriteAllText(@"proxy\background.js", background.Replace("PROXY_ADDRESS", proxyAddress)
                                                            .Replace("PROXY_PORT", proxyPort)
                                                            .Replace("PROXY_USERNAME", proxyUsername)
                                                            .Replace("PROXY_PASSWORD", proxyPassword));
            if (File.Exists("proxy.crx"))
            {
                File.Delete("proxy.crx");
            }
            ZipFile.CreateFromDirectory("proxy", "proxy.crx");
            File.WriteAllText(@"proxy\background.js", background);
        }
        #endregion
    }
}
