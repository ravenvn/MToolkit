using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        public Configs config = JsonConvert.DeserializeObject<Configs>(File.ReadAllText("Configs.json"));

        public string Login(string email, string password, string recoveryEmail)
        {
            var response = new LoginResponse
            {
                Status = false,
                Detail_Reason = string.Empty,
                Cookie = null
            };

            ChromeDriver driver = null;
            try
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                var options = new ChromeOptions();
                options.BinaryLocation = @"C:\chrome-win32\chrome.exe";
                options.AddArguments("disable-infobars");
                options.AddArguments("start-maximized");
                options.AddArguments("--incognito");


                driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl("https://accounts.google.com/signin/v2/identifier");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Page_Load));
                try
                {
                    var mailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("identifierId")));
                    mailInput.SendKeys(email);
                    mailInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Mạng lỗi";

                    if (driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                    }

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                    var passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));
                    passwordInput.SendKeys(password);
                    passwordInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Tài khoản không tồn tại";

                    if (driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                    }

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Name("password")));
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Sai mật khẩu";

                    if (driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                    }

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                    wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("vxx8jf")));
                    var confirmOptionButtons = driver.FindElementsByClassName("vxx8jf").Where(x => x.Displayed).ToArray();
                    var numOptions = confirmOptionButtons.Length;
                    if (numOptions > 0)
                    {
                        var confirmRecoveryEmailOptionButton = numOptions <= 3 ? confirmOptionButtons[0] : confirmOptionButtons[numOptions - 2];
                        confirmRecoveryEmailOptionButton.Click();

                        try
                        {
                            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                            var recoveryMailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("knowledgePreregisteredEmailResponse")));
                            recoveryMailInput.SendKeys(recoveryEmail);
                            recoveryMailInput.SendKeys(Keys.Enter);
                            try
                            {
                                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Name("knowledgePreregisteredEmailResponse")));
                                response.Status = true;
                            }
                            catch (Exception)
                            {
                                response.Detail_Reason = "Sai email khôi phục";

                                if (driver != null)
                                {
                                    driver.Close();
                                    driver.Quit();
                                }

                                return JsonConvert.SerializeObject(response);
                            }
                        }
                        catch (Exception)
                        {
                            response.Detail_Reason = "Không hỗ trợ xác nhận bằng email khôi phục";

                            if (driver != null)
                            {
                                driver.Close();
                                driver.Quit();
                            }

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

            if (response.Status == true)
            {
                try
                {
                    driver.Navigate().GoToUrl("https://www.youtube.com/");
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Page_Load));
                    var avatarBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("avatar-btn")));
                    var loginCookies = driver.Manage().Cookies.AllCookies;
                    var cookie = string.Empty;
                    foreach (var loginCookie in loginCookies)
                    {
                        cookie += loginCookie.Name + "=" + loginCookie.Value + ";";
                    }
                    response.Cookie = cookie;
                }
                catch (Exception)
                {
                    response.Status = false;
                    response.Detail_Reason = "Tài khoản Youtube bị chết";
                }
                
            }

            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }

            return JsonConvert.SerializeObject(response);
        }

        public string ManualLogin(string email, string password)
        {
            var response = new LoginResponse
            {
                Status = false,
                Detail_Reason = string.Empty,
                Cookie = null
            };

            ChromeDriver driver = null;
            try
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                var options = new ChromeOptions();
                options.BinaryLocation = @"C:\chrome-win32\chrome.exe";
                options.AddArguments("disable-infobars");
                options.AddArguments("start-maximized");
                options.AddArguments("--incognito");


                driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl("https://accounts.google.com/signin/v2/identifier");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Page_Load));
                try
                {
                    var mailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("identifierId")));
                    mailInput.SendKeys(email);
                    mailInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Mạng lỗi";

                    if (driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                    }

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                    var passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));
                    passwordInput.SendKeys(password);
                    passwordInput.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Tài khoản không tồn tại";

                    if (driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                    }

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Enter_Load));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Name("password")));
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Sai mật khẩu";

                    if (driver != null)
                    {
                        driver.Close();
                        driver.Quit();
                    }

                    return JsonConvert.SerializeObject(response);
                }

                try
                {
                    // Manual tasks
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Manual_Load));
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
                try
                {
                    driver.Navigate().GoToUrl("https://www.youtube.com/");
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Page_Load));
                    var avatarBtn = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("avatar-btn")));
                    var loginCookies = driver.Manage().Cookies.AllCookies;
                    var cookie = string.Empty;
                    foreach (var loginCookie in loginCookies)
                    {
                        cookie += loginCookie.Name + "=" + loginCookie.Value + ";";
                    }
                    response.Cookie = cookie;
                }
                catch (Exception)
                {
                    response.Status = false;
                    response.Detail_Reason = "Tài khoản Youtube bị chết";
                }

            }

            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }

            return JsonConvert.SerializeObject(response);
        }

        public string LoginByCookie(string cookie)
        {
            var response = new LoginResponse
            {
                Status = false,
                Detail_Reason = string.Empty,
                Cookie = cookie
            };

            ChromeDriver driver = null;
            try
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                var options = new ChromeOptions();
                options.BinaryLocation = @"C:\chrome-win32\chrome.exe";
                options.AddArguments("disable-infobars");
                options.AddArguments("start-maximized");
                options.AddArguments("--incognito");


                driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl("https://www.youtube.com");
                var cookies = cookie.Split(';');
                foreach (var c in cookies)
                {
                    var pair = c.Split('=');
                    if (pair.Count() == 2)
                    {
                        driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(pair[0].Trim(), pair[1].Trim()));
                    }
                }
                driver.Navigate().Refresh();

                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(config.Page_Load));
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("avatar-btn")));
                    response.Status = true;
                }
                catch (Exception)
                {
                    response.Detail_Reason = "Cookie hết hạn";
                }
            }
            catch (Exception ex)
            {
                response.Detail_Reason = ex.Message;
            }
            
            if (response.Status == false && driver != null)
            {
                driver.Close();
                driver.Quit();
            }

            return JsonConvert.SerializeObject(response);
        }
    }
}
