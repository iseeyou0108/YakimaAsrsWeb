using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YakimaAsrsWeb.Controllers
{
    public class AccountController : Controller
    {
        //Get Login page
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Models.Account LoginUser)
        {
            //驗證帳號密碼
            if (ModelState.IsValid)
            {
                var VerrifyResult = GetUserData(LoginUser);
                if (VerrifyResult.Successed)
                {
                    if (VerrifyResult.Data.Rows.Count > 0)
                    {
                        Session["UserName"] = VerrifyResult.Data.Rows[0]["USER_NAME"].ToString();
                        Session["UserNo"] = LoginUser.UserNo;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "帳號或密碼不正確");
                        return View(LoginUser);
                    }
                }
                else
                {
                    ModelState.AddModelError("", VerrifyResult.Message);
                    return View(LoginUser);
                }
            }
            else
            {
                return View(LoginUser);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = string.Empty;
            Session["UserNo"] = string.Empty;
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 驗證登入資料
        /// </summary>
        /// <param name="LoginUser"></param>
        /// <returns></returns>
        private Service.DBService.DBResult GetUserData(Models.Account LoginUser)
        {
            var service = new Service.DBService();

            var parameters = new List<Service.DBService.DBParameter>();
            string SqlCmd = "select a.* from HRS_USER a where 1 = 1 and a.USER_NO = @USER_NO ";
            parameters.Add(new Service.DBService.DBParameter() { ParameterName = "USER_NO", Value = LoginUser.UserNo });

            var result = service.GetDbData(SqlCmd, parameters);
            if (result.IsCompleted)
            {
                return result.Result;
            }

            return new Service.DBService.DBResult();
        }
    }
}
