using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PORECT.Helper;
using System.Security.Cryptography;
using Tes.Domain;

namespace PORECT.Controllers
{
    public class LoginController : ParentController
    {
        public LoginController(IHttpContextAccessor contextAccessor) : base(null, contextAccessor)
        {

        }

        public IActionResult Register()
        {
            try
            {
                return View("_Register");
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Login", "Form Register");
                throw;
            }
        }
        [HttpPost]
        public IActionResult NewUser(string? data)
        {
            try
            {
                TransactionResponse response = new TransactionResponse();
                if (string.IsNullOrEmpty(data))
                {
                    response.IsSuccess = false;
                    response.Message = "Parameter is empty";
                    return Json(response);
                }

                Tes.Domain.MsUserRequest model = JsonConvert.DeserializeObject<Tes.Domain.MsUserRequest>(data);
                if (!ModelState.IsValid)
                {
                    //throw new Exception("Please check your input");
                    response.IsSuccess = false;
                    response.Message = "Please check your input";
                    return Json(response);
                }
                string json = JsonConvert.SerializeObject(model);
                string result = _api.PostString(json, AppConfig.Config.ConfigAPI.User.Submit.BaseUrl, AppConfig.Config.ConfigAPI.User.Submit.Endpoint, default, false,
                    AppConfig.Config.ConfigAPI.User.Submit.BaseUrl.Split('/')[0] == "https:");
                if (!string.IsNullOrEmpty(result))
                    response = JsonConvert.DeserializeObject<TransactionResponse>(result);
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Transaction success but no response from api";
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "AppUsers", "Submit");
                throw;
            }
        }

        public IActionResult Login()
        {
            ViewBag.message = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AppUserRequest appUser)
        {
            try
            {
                var username = appUser.Username;
                if(string.IsNullOrEmpty(username))
                    ViewBag.message = "Username is empty!";
                var password = appUser.Password;
                if (string.IsNullOrEmpty(password))
                    ViewBag.message = "Password is empty!";

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    //Check username
                    //var isfoundUsername = _repository.sp_list_user().FirstOrDefault(x =>
                    //                        !string.IsNullOrEmpty(x.Username) &&
                    //                        x.Username.ToLower() == username.ToLower());
                    var param = new List<ParamTaskViewModel>
                    {
                        new ParamTaskViewModel
                        {
                            colName = "username",
                            value = username
                        }
                    };
                    var isfoundUsername = _api.Get<List<Tes.Domain.MsUserResponse>>(param, AppConfig.Config.ConfigAPI.User.List.BaseUrl, AppConfig.Config.ConfigAPI.User.List.Endpoint,
                        AppConfig.Config.ConfigAPI.User.List.BaseUrl.Split('/')[0] == "https:").FirstOrDefault();
                    if (isfoundUsername != null)
                    {
                        if (isfoundUsername.IsActive == true)
                        {
                            if (isfoundUsername.Password == password)
                            {
                                #region Detail User
                                _contextAccessor.HttpContext.Session.SetInt32("Id", isfoundUsername.ID);
                                _contextAccessor.HttpContext.Session.SetString("Fullname", string.Format("{0}{1}",
                                                                                    !string.IsNullOrEmpty(isfoundUsername.FirstName) ?
                                                                                        isfoundUsername.FirstName : string.Empty,
                                                                                    !string.IsNullOrEmpty(isfoundUsername.LastName) ?
                                                                                        string.Concat(" ", isfoundUsername.LastName) : string.Empty));
                                _contextAccessor.HttpContext.Session.SetString("Username", !string.IsNullOrEmpty(isfoundUsername.Username) ? 
                                                                                isfoundUsername.Username : string.Empty);
                                #endregion Detail User
                                return RedirectToAction("Index", "LandingPage");
                            }
                            else
                            {
                                ViewBag.message = "Incorrect username or password!";
                            }

                        }
                        else
                        {
                            ViewBag.message = "Username is Inactive!";
                        }
                    }
                    else
                    {
                        ViewBag.message = "Incorrect username or password!";
                    }
                }
            }
            catch (Exception e)
            {
                logger.WriteErrorToLog(e, "Login", "Login");
                ViewBag.message = e.Message;
            }
            return View();
        }

        public IActionResult Logout()
        {
            _contextAccessor.HttpContext.Session.SetInt32("Id", -1);
            _contextAccessor.HttpContext.Session.SetString("Fullname", string.Empty);
            _contextAccessor.HttpContext.Session.SetString("Username", string.Empty);

            return RedirectToAction("Login", "Login");
        }
    }
}
