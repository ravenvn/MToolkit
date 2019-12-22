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

namespace MToolkit.Helpers
{
    class Helper
    {
        public static Configs config = JsonConvert.DeserializeObject<Configs>(File.ReadAllText("Configs.json"));
        public static string tempDir = "";
        public static string profileDir = "";
        public static FirefoxDriver CreateFirefoxDriver(string profile, string proxy = "", string userAgent = "")
        {
            proxy = "";
            userAgent = "";
            FirefoxDriver driver = null;
            try
            {
                

                var firefoxProfile = new FirefoxProfile(profile);
                if (userAgent != "")
                {
                    firefoxProfile.SetPreference("general.useragent.override", userAgent);
                }
                var service = FirefoxDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                var options = new FirefoxOptions();
                options.Profile = firefoxProfile;

                var proxyUsername = "";
                var proxyPassword = "";
                if (proxy != "")
                {
                    var proxyAuth = proxy.Split('|');
                    var proxyAddress = proxyAuth[0];
                    proxyUsername = proxyAuth[1];
                    proxyPassword = proxyAuth[2];
                    var firefoxProxy = new Proxy();
                    firefoxProxy.HttpProxy = firefoxProxy.FtpProxy = firefoxProxy.SslProxy = proxyAddress;
                    firefoxProxy.Kind = ProxyKind.Manual;
                    options.Proxy = firefoxProxy;
                }

                driver = new FirefoxDriver(service, options);
                if (proxy != "")
                {
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
                    }
                }

                driver.Manage().Window.Maximize();
                tempDir = driver.Capabilities["moz:profile"].ToString();
                profileDir = profile;
            }
            catch (Exception e)
            {
                LogError("Errors.txt", e.Message);
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                }
            }

            return driver;
        }

        public static void CloseBrowser(FirefoxDriver driver, bool saveHistory = false)
        {
            if (driver != null)
            {
                if (saveHistory) CopyFromTempToProfileDirectory(tempDir, profileDir);
                driver.Close();
                driver.Quit();
            }
        }

        private static void CopyFromTempToProfileDirectory(string temp, string profile)
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

        public static void LogError(string filePath, string error)
        {
            File.AppendAllText(filePath, error + Environment.NewLine);
        }
    }
}
