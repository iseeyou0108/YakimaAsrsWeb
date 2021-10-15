using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YakimaAsrsWeb.Filters;

namespace YakimaAsrsWeb.Controllers
{
    [CustomAuthenticationFilter]
    public class WCSController : Controller
    {
        Service.WcsCrnService CrnService = new Service.WcsCrnService();
        Service.WcsTrkService TrkService = new Service.WcsTrkService();
        Service.WcsDeviceService DeviceService = new Service.WcsDeviceService();

        // GET: WCS_CRN
        public ActionResult VwCrn()
        {

            return View();
        }

        // GET: WCS_TRK
        public ActionResult VwTrk()
        {

            return View();
        }

        public ActionResult VwDevice()
        {

            return View();
        }

        public JsonResult GetWcsTrkData(string SearchText, int? AsrsID, int? Page, int? Limit)
        {
            Models.LayUITableData result = new Models.LayUITableData();
            var DataResult = TrkService.GetData();
            List<Models.WcsTrk> Trks = new List<Models.WcsTrk>();
            if (DataResult.IsCompleted)
            {
                if (DataResult.Result.Successed)
                {
                    foreach (System.Data.DataRow dr in DataResult.Result.Data.Rows)
                    {
                        Trks.Add(new Models.WcsTrk()
                        {
                            ASRS_ID = Convert.ToInt16(dr["ASRS_ID"]),
                            USE_CRN_ID = Convert.ToInt16(dr["USE_CRN_ID"]),
                            SER_NO = Convert.ToInt16(dr["SER_NO"]),
                            STEP = Convert.ToInt16(dr["STEP"]),
                            STATUS = Convert.ToInt16(dr["STATUS"]),
                            OPN = Convert.ToInt16(dr["OPN"]),
                            EMERGE = Convert.ToInt16(dr["EMERGE"]),
                            OPN_DESC = Convert.ToString(dr["OPN_DESC"]),
                            STATUS_DESC = Convert.ToString(dr["STATUS_DESC"]),
                            STEP_DESC = Convert.ToString(dr["STEP_DESC"]),
                            BIN_NO = Convert.ToString(dr["BIN_NO"]),
                            DEV_NO = Convert.ToString(dr["DEV_NO"]),
                            IO = Convert.ToString(dr["IO"]),
                            CREATE_BY = Convert.ToString(dr["CREATE_BY"]),
                            USER_NAME = Convert.ToString(dr["USER_NAME"]),
                            CUR_DEV_NO = Convert.ToString(dr["CUR_DEV_NO"]),
                            NEXT_DEV_NO = Convert.ToString(dr["NEXT_DEV_NO"]),
                            SIZE_CHK = Convert.ToInt16(dr["SIZE_CHK"]),
                            CREATE_DATE = Convert.ToDateTime(dr["CREATE_DATE"]).ToString("yyyy/MM/dd HH:mm:ss"),
                            IO_DESC = TrkService.GetIODesc(dr["IO"].ToString())
                        });
                    }

                    if (AsrsID.HasValue)
                    {
                        if (AsrsID > 0)
                            Trks = Trks.Where(o => o.ASRS_ID == AsrsID).ToList();
                    }
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        Trks = Trks.Where(o => o.IO.Contains(SearchText)
                        || o.IO_DESC.Contains(SearchText)
                        || o.STEP_DESC.Contains(SearchText)
                        || o.STATUS_DESC.Contains(SearchText)
                        || o.STEP.ToString().Contains(SearchText)
                        || o.STATUS.ToString().Contains(SearchText)
                        || o.USE_CRN_ID.ToString().Contains(SearchText)
                        || o.SER_NO.ToString().Contains(SearchText)
                        || o.USER_NAME.Contains(SearchText)
                        || o.ASRS_ID.ToString().Contains(SearchText)
                        ).ToList();
                    }
                    result.count = Trks.Count;

                    result.data = Trks;
                    result.code = 0;
                }
                else
                {
                    result.count = 0;

                    result.data = Trks;
                    result.code = Convert.ToInt16(HttpStatusCode.InternalServerError);
                }
            }
            result.data = Trks.Skip((Page.Value - 1) * Limit.Value).Take(Limit.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWcsDeviceData(string SearchText, int? AsrsID, int? Page, int? Limit)
        {
            Models.LayUITableData result = new Models.LayUITableData();
            var DataResult = DeviceService.GetData();
            List<Models.WcsDevice> Devices = new List<Models.WcsDevice>();
            if (DataResult.IsCompleted)
            {
                if (DataResult.Result.Successed)
                {
                    foreach (System.Data.DataRow dr in DataResult.Result.Data.Rows)
                    {
                        Devices.Add(new Models.WcsDevice()
                        {
                            ASRS_ID = Convert.ToInt16(dr["ASRS_ID"]),
                            DEST = Convert.ToString(dr["DEST"]),
                            DEV_NO = Convert.ToString(dr["DEV_NO"]),
                            SER_NO = Convert.ToInt16(dr["SER_NO"]),
                            MANU = Convert.ToInt16(dr["MANU"]) > 0 ? true : false,
                            AUTO = Convert.ToInt16(dr["AUTO"]) > 0 ? true : false,
                            AUTO_START = Convert.ToInt16(dr["AUTO_START"]) > 0 ? true : false,
                            ERR = Convert.ToInt16(dr["ERR"]) > 0 ? true : false,
                            IDLE = Convert.ToInt16(dr["IDLE"]) > 0 ? true : false,
                            LOAD = Convert.ToInt16(dr["LOAD"]) > 0 ? true : false,
                            STKIN_REQ = Convert.ToInt16(dr["STKIN_REQ"]) > 0 ? true : false,
                            IN_ENABLE = Convert.ToString(dr["IN_ENABLE"]),
                            OUT_ENABLE = Convert.ToString(dr["OUT_ENABLE"]),
                            IO_MODE = Convert.ToInt16(dr["IO_MODE"]),
                            WEIGHT = Convert.ToDecimal(dr["WEIGHT"]),
                            SIZE_CHK = Convert.ToDecimal(dr["SIZE_CHK"]),
                            OP_MODE = Convert.ToDecimal(dr["OP_MODE"]),
                            DISPLAY_ITEM = Convert.ToDecimal(dr["DISPLAY_ITEM"])
                        });
                    }

                    if (AsrsID.HasValue)
                    {
                        if (AsrsID > 0)
                            Devices = Devices.Where(o => o.ASRS_ID == AsrsID).ToList();
                    }
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        Devices = Devices.Where(o => o.DEV_NO.Contains(SearchText)
                        ).ToList();
                    }
                    result.count = Devices.Count;

                    result.data = Devices;
                    result.code = 0;
                }
                else
                {
                    result.count = 0;

                    result.data = Devices;
                    result.code = Convert.ToInt16(HttpStatusCode.InternalServerError);
                }
            }

            result.data= Devices.Skip((Page.Value - 1) * Limit.Value).Take(Limit.Value);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWcsCrnData(string SearchText, int? Page, int? Limit)
        {
            Models.LayUITableData result = new Models.LayUITableData();
            var DataResult = CrnService.GetData();
            List<Models.WcsCrn> Crns = new List<Models.WcsCrn>();
            if (DataResult.IsCompleted)
            {
                if (DataResult.Result.Successed)
                {
                    foreach (System.Data.DataRow dr in DataResult.Result.Data.Rows)
                    {
                        Crns.Add(new Models.WcsCrn()
                        {
                            ASRS_ID = Convert.ToInt16(dr["ASRS_ID"]),
                            CRN_ID = Convert.ToInt16(dr["CRN_ID"]),
                            SER_NO = Convert.ToInt16(dr["SER_NO"]),
                            CRN_AUTO = Convert.ToBoolean(dr["CRN_AUTO"]),
                            CRN_ERROR = Convert.ToBoolean(dr["CRN_ERROR"]),
                            CRN_INITAL = Convert.ToBoolean(dr["CRN_INITAL"]),
                            CRN_READY = Convert.ToBoolean(dr["CRN_READY"]),
                            CRN_NO = Convert.ToString(dr["CRN_NO"]),
                            ERR_CODE = Convert.ToString(dr["ERR_CODE"]),
                            LOADING = Convert.ToBoolean(dr["LOADING"])
                        });
                    }
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        Crns = Crns.Where(o => o.CRN_NO.Contains(SearchText)).ToList();
                    }
                    result.count = Crns.Count;

                    result.data = Crns;
                    result.code = 0;
                }
                else
                {
                    result.count = 0;

                    result.data = Crns;
                    result.code = Convert.ToInt16(HttpStatusCode.InternalServerError);
                }
            }
            result.data = Crns.Skip((Page.Value - 1) * Limit.Value).Take(Limit.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CraneReset(Models.WcsCrn Crn)
        {
           var result = CrnService.ResetCrn(Crn, Session["UserNo"].ToString());
            if (result.IsCompleted)
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = result.Result.Successed, Message = result.Result.Message });
            }
            return Json("OK");
        }

        [HttpPost]
        public JsonResult CancelTrk(Models.WcsTrk Trk)
        {
            var result = TrkService.CancelOrComplteTrk(Trk, Session["UserNo"].ToString(), true);
            if (result.IsCompleted)
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = result.Result.Successed, Message = result.Result.Message });
            }
            return Json("OK");
        }

        [HttpPost]
        public JsonResult CompleteTrk(Models.WcsTrk Trk)
        {
            var result = TrkService.CancelOrComplteTrk(Trk, Session["UserNo"].ToString(), false);
            if (result.IsCompleted)
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = result.Result.Successed, Message = result.Result.Message });
            }
            return Json("OK");
        }

        [HttpPost]
        public JsonResult ChangeTrkStep(Models.WcsTrk Trk, string STEP)
        {
            if (string.IsNullOrWhiteSpace(STEP))
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = false, Message = "請輸入STEP" });
            }

            int intStep = 0;
            var ConvertResult = int.TryParse(STEP, out intStep);
            if(!ConvertResult)
                return Json(new Service.WcsCrnService.PostResponse() { Successed = false, Message = "STEP格式錯誤，請輸入0~99" });

            var result = TrkService.ChangeTrkStep(Trk, Session["UserNo"].ToString(), intStep);
            if (result.IsCompleted)
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = result.Result.Successed, Message = result.Result.Message });
            }
            return Json("OK");
        }
    }
}
