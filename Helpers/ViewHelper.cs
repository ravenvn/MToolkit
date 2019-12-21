using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        public Configs config = JsonConvert.DeserializeObject<Configs>(File.ReadAllText("Configs.json"));
        private bool stopThread = false;
        private static readonly HttpClient client = new HttpClient();

        public void Test()
        {
            var driver = CreateFirefoxDriver(@"C:\Users\Mickey\AppData\Roaming\Mozilla\Firefox\Profiles\0iheofyh.Profile1", "193.27.10.153:80|wrletwth-25|er0kx7ty42vw1", "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) FxiOS/8.1.2 Mobile/16A366 Safari/605.1.15");
            if (driver != null)
            {
                driver.Navigate().GoToUrl("https://www.google.com/search?q=my+user+agent&oq=my+user+agent&aqs=chrome..69i57j69i59j0l5j69i60.4180j0j4&sourceid=chrome&ie=UTF-8");
                Thread.Sleep(5);
                driver.Navigate().GoToUrl("https://www.google.com/search?q=my+ip&oq=my+ip&aqs=chrome..69i57j0l4j69i60l3.8205j0j9&sourceid=chrome&ie=UTF-8");
                Thread.Sleep(5);
                driver.Close();
                driver.Quit();
            }

            driver = CreateFirefoxDriver(@"C:\Users\Mickey\AppData\Roaming\Mozilla\Firefox\Profiles\okxw9gv6.Profile2", "84.21.191.1933:80|wrletwth-23|er0kx7ty42vw", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0");
            if (driver != null)
            {
                driver.Navigate().GoToUrl("https://www.google.com/search?q=my+user+agent&oq=my+user+agent&aqs=chrome..69i57j69i59j0l5j69i60.4180j0j4&sourceid=chrome&ie=UTF-8");
                Thread.Sleep(5);
                driver.Navigate().GoToUrl("https://www.google.com/search?q=my+ip&oq=my+ip&aqs=chrome..69i57j0l4j69i60l3.8205j0j9&sourceid=chrome&ie=UTF-8");
                Thread.Sleep(5);
                driver.Close();
                driver.Quit();
            }
        }

        private FirefoxDriver CreateFirefoxDriver(string profile, string proxy, string userAgent)
        {
            FirefoxDriver driver = null;
            try
            {
                var proxyAuth = proxy.Split('|');
                var proxyAddress = proxyAuth[0];
                var proxyUsername = proxyAuth[1];
                var proxyPassword = proxyAuth[2];

                var firefoxProfile = new FirefoxProfile(profile);
                firefoxProfile.SetPreference("general.useragent.override", userAgent);
                var service = FirefoxDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var firefoxProxy = new Proxy();
                firefoxProxy.HttpProxy = firefoxProxy.FtpProxy = firefoxProxy.SslProxy = proxyAddress;
                firefoxProxy.Kind = ProxyKind.Manual;

                var options = new FirefoxOptions();
                options.Profile = firefoxProfile;
                options.Proxy = firefoxProxy;

                driver = new FirefoxDriver(service, options);
                driver.Navigate().GoToUrl("https://google.com");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                wait.Until(ExpectedConditions.AlertIsPresent());
                var alert = driver.SwitchTo().Alert();
                alert.SendKeys(proxyUsername + Keys.Tab + proxyPassword);
                alert.Accept();
                Thread.Sleep(1);
                try
                {
                    // if alert is still present, it means the authentication fail, we quit the browser
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                    wait.Until(ExpectedConditions.AlertIsPresent());
                    // we don't use driver.Close() here because it is not working for proxy alert
                    driver.Quit();
                    driver = null;
                }
                catch (Exception)
                {
                    driver.Manage().Window.Maximize();
                }
            }
            catch (Exception e)
            {
                LogError(e.Message);
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                }
            }

            return driver;
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
                while (true)
                {
                    foreach (var account in data.Accounts)
                    {
                        try
                        {
                            if (i == titleVideoIds.Length) i = 0;
                            var titleVideoId = titleVideoIds[i].Split('|');
                            var title = titleVideoId[0];
                            var videoId = titleVideoId[1];
                            var recommendedVideoId = titleVideoId[2];
                            var sub = data.Sub == false ? false : random.Next(1, 101) <= data.SubRatio;
                            var like = data.Like == false ? false : random.Next(1, 101) <= data.LikeRatio;
                            AutoViewAccount(account, data.FilterType, title, videoId, recommendedVideoId, data.DurationMin, data.DurationMax, sub, like);
                            i += 1;
                        }
                        catch (Exception e)
                        {
                            LogError(e.Message);
                        }

                        // check continue running or not
                        if (stopThread) break;
                    }

                    if (stopThread) break;
                }
                
                // Todo: send status stop to server
            }
            catch (Exception e)
            {
                LogError(e.Message);
            }
        }

        private void AutoViewAccount(Account account, string filterType, string title, string videoId, string recommendedVideoId, int durationMin, int durationMax, bool sub, bool like)
        {
            FirefoxDriver driver = null;
            try
            {
                var random = new Random();
                driver = CreateFirefoxDriver(account.Profile, account.Proxy, account.UserAgent);
                if (driver != null)
                {
                    driver.Navigate().GoToUrl("https://youtube.com");
                    
                    var searchElement = driver.FindElementByXPath("//input[@id='search']");
                    searchElement.SendKeys(title);
                    Thread.Sleep(config.Action_Sleep);
                    searchElement.SendKeys(Keys.Enter);
                    Thread.Sleep(config.Action_Sleep);
                    driver.Navigate().GoToUrl(driver.Url + "&sp=" + filterType);
                    Thread.Sleep(config.Action_Sleep);

                    var videoElement = driver.FindElementsById("video-title").Where(x => x.Displayed && x.GetAttribute("href") != null && x.GetAttribute("href").Contains(videoId)).First();
                    if (videoElement != null)
                    {
                        videoElement.Click();
                        var duration = random.Next(durationMin, durationMax + 1);
                        Thread.Sleep(duration);
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
                                    Thread.Sleep(config.Action_Sleep);
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
                                LogError(e.Message);
                            }
                        }

                        if (like)
                        {
                            var likeButton = driver.FindElementsByXPath("//button[@id='button']/yt-icon[@class='style-scope ytd-toggle-button-renderer']").Where(x => x.Displayed).First();
                            if (likeButton != null)
                            {
                                likeButton.Click();
                                Thread.Sleep(config.Action_Sleep);
                            }
                        }

                        // recommended video
                        //var js = (IJavaScriptExecutor)driver;
                        //js.ExecuteScript("document.getElementsById('thumbnail').setAttribute('href', 'abcdef')");
                        //var firstRightVideo = driver.FindElementsById("thumbnail").Where(x => x.Displayed && x.GetAttribute("href") != null).First();
                        //firstRightVideo.Click();
                    }

                }
            }
            catch (Exception e)
            {
                ReportAccountError(account.Id, e.Message);
            }
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
                client.PostAsync(config.Manage_Site_Url + "/api/videos/update-duration", content);
            }
            catch (Exception e)
            {
                LogError(e.Message);
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
                client.PostAsync(config.Manage_Site_Url + "/api/channels/increase-sub", content);
            }
            catch (Exception e)
            {
                LogError(e.Message);
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
                client.PostAsync(config.Manage_Site_Url + "/api/account/report-error", content);
            }
            catch (Exception e)
            {
                LogError(e.Message);
            }
        }

        private void CopyFromTempToProfileDirectory(string temp, string profile)
        {
            var dataFiles = Directory.GetFiles(temp, "*.sqlite*");
            foreach (var dataFile in dataFiles)
            {
                File.Copy(dataFile, profile + @"\" + Path.GetFileName(dataFile), true);
            }

            #region Copy all files and sub directory. It's caused error when reload profile
            // Copy all files and sub directory. It's caused error  
            ////Now Create all of the directories
            //foreach (string dirPath in Directory.GetDirectories(temp, "*",
            //    SearchOption.AllDirectories))
            //    Directory.CreateDirectory(dirPath.Replace(temp, profile));

            ////Copy all the files & Replaces any files with the same name
            //foreach (string newPath in Directory.GetFiles(temp, "*.*",
            //    SearchOption.AllDirectories).Where(x => !x.EndsWith(".lock")))
            //    File.Copy(newPath, newPath.Replace(temp, profile), true);
            #endregion
        }

        private void LogError(string error)
        {
            File.AppendAllText("View_Errors.txt", error + Environment.NewLine);
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
                LogError(e.Message);
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
                LogError(e.Message);
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
