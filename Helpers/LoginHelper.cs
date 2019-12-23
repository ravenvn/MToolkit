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
    class LoginHelper
    {
        public string Login(string profile, string proxy, string userAgent, string email, string password, string recoveryEmail, bool getCookieNChannel)
        {
            var response = new LoginResponse
            {
                Status = false,
                Detail_Reason = string.Empty,
                Cookie = null,
                Channel_Name = string.Empty,
                Channel_Link = string.Empty
            };

            var driver = Helper.CreateFirefoxDriver(profile, proxy, userAgent);
            if (driver == null)
            {
                response.Detail_Reason = "Không tải đc profile hoặc proxy chết";
                return JsonConvert.SerializeObject(response);
            }

            try
            {
                driver.Navigate().GoToUrl("https://accounts.google.com/signin/v2/identifier");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Page_Load));
                try
                {
                    var mailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("identifierId")));
                    mailInput.SendKeys(email);
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    mailInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Mạng lỗi";
                    Helper.CloseBrowser(driver);
                    
                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                    var passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));
                    passwordInput.SendKeys(password);
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    passwordInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Tài khoản không tồn tại";
                    Helper.CloseBrowser(driver);

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Name("password")));
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Sai mật khẩu";
                    Helper.CloseBrowser(driver);

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                    wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("vxx8jf")));
                    var confirmOptionButtons = driver.FindElementsByClassName("vxx8jf").Where(x => x.Displayed).ToArray();
                    var numOptions = confirmOptionButtons.Length;
                    if (numOptions > 0)
                    {
                        var confirmRecoveryEmailOptionButton = numOptions <= 3 ? confirmOptionButtons[0] : confirmOptionButtons[numOptions - 2];
                        confirmRecoveryEmailOptionButton.Click();

                        try
                        {
                            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                            var recoveryMailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("knowledgePreregisteredEmailResponse")));
                            recoveryMailInput.SendKeys(recoveryEmail);
                            Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                            recoveryMailInput.SendKeys(Keys.Enter);
                            try
                            {
                                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Name("knowledgePreregisteredEmailResponse")));
                                response.Status = true;
                            }
                            catch (Exception)
                            {
                                response.Detail_Reason = "Sai email khôi phục";
                                Helper.CloseBrowser(driver);

                                return JsonConvert.SerializeObject(response);
                            }
                        }
                        catch (Exception)
                        {
                            response.Detail_Reason = "Không hỗ trợ xác nhận bằng email khôi phục";
                            Helper.CloseBrowser(driver);

                            return JsonConvert.SerializeObject(response);
                        }
                    }
                }
                catch (Exception)
                {
                    response.Status = true;
                }
            }
            catch (Exception e)
            {
                response.Detail_Reason = e.Message;
            }

            if (response.Status && getCookieNChannel)
            {
                GetCookie(driver, ref response);
            }
            Helper.CloseBrowser(driver, true);


            return JsonConvert.SerializeObject(response);
        }

        public string ManualLogin(string profile, string email, string password)
        {
            var response = new LoginResponse
            {
                Status = false,
                Detail_Reason = string.Empty,
                Cookie = null,
                Channel_Name = string.Empty,
                Channel_Link = string.Empty
            };

            var driver = Helper.CreateFirefoxDriver(profile);
            if (driver == null)
            {
                response.Detail_Reason = "Không load được profile";
                return JsonConvert.SerializeObject(response);
            }

            try
            {
                driver.Navigate().GoToUrl("https://accounts.google.com/signin/v2/identifier");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Page_Load));
                try
                {
                    var mailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("identifierId")));
                    mailInput.SendKeys(email);
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    mailInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Mạng lỗi";
                    Helper.CloseBrowser(driver);

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                    var passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));
                    passwordInput.SendKeys(password);
                    passwordInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Tài khoản không tồn tại";
                    Helper.CloseBrowser(driver);

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Enter_Load));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Name("password")));
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Sai mật khẩu";
                    Helper.CloseBrowser(driver);

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    // Manual tasks
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Manual_Load));
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("avatar-btn")));
                    var loginCookies = driver.Manage().Cookies.AllCookies;
                    var cookie = string.Empty;
                    foreach (var loginCookie in loginCookies)
                    {
                        cookie += loginCookie.Name + "=" + loginCookie.Value + ";";
                    }
                    response.Cookie = cookie;
                    response.Status = true;
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Đăng nhập thủ công lỗi";
                }
            }
            catch (Exception e)
            {
                response.Detail_Reason = e.Message;
            }

            if (response.Status == true)
            {
                GetCookie(driver, ref response);
            }

            Helper.CloseBrowser(driver, true);

            return JsonConvert.SerializeObject(response);
        }

        //public string LoginByCookie(string cookieString)
        //{
        //    var response = new LoginResponse
        //    {
        //        Status = false,
        //        Detail_Reason = string.Empty,
        //        Cookie = cookieString
        //    };

        //    ChromeDriver driver = null;
        //    try
        //    {
        //        var service = ChromeDriverService.CreateDefaultService();
        //        service.HideCommandPromptWindow = true;
        //        var options = new ChromeOptions();
        //        options.BinaryLocation = config.Chrome_Path;
        //        options.AddArguments("disable-infobars");
        //        options.AddArguments("start-maximized");
        //        options.AddArguments("--incognito");


        //        driver = new ChromeDriver(service, options);
        //        driver.Navigate().GoToUrl("https://www.youtube.com");
        //        var cookies = cookieString.Split('\n');
        //        foreach (var cookie in cookies)
        //        {
        //            var item = cookie.Split(';');
        //            DateTime? expiry = null;
        //            try
        //            {
        //                expiry = DateTime.Parse(item[4]);
        //            }
        //            catch (Exception) { }
        //            var c = new Cookie(item[0], item[1], item[2], item[3], expiry);
        //            driver.Manage().Cookies.AddCookie(c);
        //        }
        //        driver.Navigate().Refresh();

        //        try
        //        {
        //            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Page_Load));
        //            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("avatar-btn")));
        //            response.Status = true;
        //        }
        //        catch (Exception)
        //        {
        //            response.Detail_Reason = "Cookie hết hạn";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Detail_Reason = ex.Message;
        //    }

        //    if (response.Status == false && driver != null)
        //    {
        //        driver.Close();
        //        driver.Quit();
        //    }

        //    return JsonConvert.SerializeObject(response);
        //}

        private void GetCookie(FirefoxDriver driver, ref LoginResponse response)
        {
            try
            {
                var cookieString = string.Empty;
                driver.Navigate().GoToUrl("https://studio.youtube.com/channel/");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Helper.config.Page_Load));
                var avatarBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("avatar-btn")));
                if (driver.Url.Contains("https://studio.youtube.com/channel/"))
                {
                    response.Channel_Link = "https://www.youtube.com/channel/" + driver.Url.Replace("https://studio.youtube.com/channel/", "");
                    response.Channel_Name = driver.FindElementById("entity-name").Text;

                    var cookies = driver.Manage().Cookies.AllCookies;
                    foreach (var cookie in cookies)
                    {
                        cookieString += string.Format("{0};{1};{2};{3};{4}\n", cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expiry);
                    }

                    driver.Navigate().GoToUrl("https://mail.google.com/mail/u/0/#inbox");
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    cookies = driver.Manage().Cookies.AllCookies;
                    foreach (var cookie in cookies)
                    {
                        cookieString += string.Format("{0};{1};{2};{3};{4}\n", cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expiry);
                    }

                    driver.Navigate().GoToUrl("https://docs.google.com/document/u/0/");
                    Thread.Sleep(TimeSpan.FromSeconds(Helper.config.Action_Sleep));
                    cookies = driver.Manage().Cookies.AllCookies;
                    foreach (var cookie in cookies)
                    {
                        cookieString += string.Format("{0};{1};{2};{3};{4}\n", cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expiry);
                    }

                    response.Cookie = cookieString;
                }
                else
                {
                    response.Status = false;
                    response.Detail_Reason = "Kênh không tồn tại";
                }
            }
            catch (Exception)
            {
                response.Status = false;
                response.Detail_Reason = "Không lấy được cookie";
            }
        }
    }
}
