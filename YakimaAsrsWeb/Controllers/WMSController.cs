using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YakimaAsrsWeb.Extensions;
using YakimaAsrsWeb.Filters;

namespace YakimaAsrsWeb.Controllers
{
    [CustomAuthenticationFilter]
    public class WMSController : Controller
    {
        Service.WmsStkService StkService = new Service.WmsStkService();
        Service.WmsBinStaService BinStaService = new Service.WmsBinStaService();

        class UIDropDown
        {
            public string Text { get; set; }
            public object Value { get; set; }
        }

        // GET: STK View
        public ActionResult VwStk()
        {
            return View();
        }

        // GET: BinSta View
        public ActionResult VwBinSta()
        {
            return View();
        }

        public ActionResult VwAddWmsStk()
        {
            //var ProdList = StkService.GetProdListData();
            //if (ProdList.IsCompleted)
            //{
            //    List<Models.WmsProd> Prods = new List<Models.WmsProd>();
            //    foreach (DataRow dr in ProdList.Result.Data.Rows)
            //    {
            //        Prods.Add(new Models.WmsProd() { ProdNo = dr["PROD_NO"].ToString(), ProdName = dr["PROD_NAME"].ToString() });
            //    }
            //    ViewBag.ProdList = new SelectList(
            //        items: Prods,
            //        dataTextField: "ProdName",
            //        dataValueField: "ProdNo"
            //        );
                
            //}
            return View();
        }

        public ActionResult VwProdList()
        {
            return View();
        }

        public JsonResult GetWmsProdData(string SearchText, int? Page, int? Limit)
        {
            List<Models.WmsProd> Prods = new List<Models.WmsProd>();
            Models.LayUITableData result = new Models.LayUITableData();
            var DataResult = StkService.GetProdListData(SearchText);
           
            if (DataResult.IsCompleted)
            {
                if (DataResult.Result.Successed)
                {
                    foreach (System.Data.DataRow dr in DataResult.Result.Data.Rows)
                    {
                        Prods.Add(new Models.WmsProd()
                        {
                            ProdNo = Convert.ToString(dr["PROD_NO"]),
                            ProdName = Convert.ToString(dr["PROD_NAME"])

                        });
                    }

                    //if (!string.IsNullOrEmpty(SearchText))
                    //{
                    //    Prods = Prods.Where(o => o.ProdNo.Contains(SearchText)
                    //    || o.ProdName.Contains(SearchText)
                    //    ).ToList();
                    //}
                    result.count = Prods.Count;

                    result.data = Prods;
                    result.code = 0;
                }
                else
                {
                    result.count = 0;

                    result.data = Prods;
                    result.code = Convert.ToInt16(HttpStatusCode.InternalServerError);
                }
            }

            result.data = Prods.Skip((Page.Value - 1) * Limit.Value).Take(Limit.Value);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWmsStkData(string SearchText, int? AsrsID, int? ProdType, int? StusCtr, int? Page, int? Limit)
        {
            Models.LayUITableData result = new Models.LayUITableData();
            var DataResult = StkService.GetData();
            List<Models.WmsStk> Stks = new List<Models.WmsStk>();
            if (DataResult.IsCompleted)
            {
                if (DataResult.Result.Successed)
                {
                    foreach (System.Data.DataRow dr in DataResult.Result.Data.Rows)
                    {
                        Stks.Add(new Models.WmsStk()
                        {
                            ASRS_ID = Convert.ToInt16(dr["ASRS_ID"]),
                            AREA = Convert.ToString(dr["AREA"]),
                            AREA_NO = Convert.ToString(dr["AREA_NO"]),
                            WH_NO = Convert.ToString(dr["WH_NO"]),
                            FIFO_NO = Convert.ToString(dr["FIFO_NO"]),
                            BIN_NO = Convert.ToString(dr["BIN_NO"]),
                            STUS_CTR_DESC = Convert.ToString(dr["STUS_CTR_DESC"]),
                            PROD_NO = Convert.ToString(dr["PROD_NO"]),
                            PROD_NAME = Convert.ToString(dr["PROD_NAME"]),
                            PROD_TYPE_DESC = Convert.ToString(dr["PROD_TYPE_DESC"]),
                            LOT_NO = Convert.ToString(dr["LOT_NO"]),
                            QC_LOT = Convert.ToString(dr["QC_LOT"]),
                            SCHEDULE_REMARK = Convert.ToString(dr["SCHEDULE_REMARK"]),
                            SRC_BIN_NO = Convert.ToString(dr["SRC_BIN_NO"]),
                            SUPPLIER = Convert.ToString(dr["SUPPLIER"]),
                            UNITS = Convert.ToString(dr["UNITS"]),
                            AUFNR = Convert.ToString(dr["AUFNR"]),
                            VORNR = Convert.ToString(dr["VORNR"]),
                            VORNR_NEXT = Convert.ToString(dr["VORNR_NEXT"]),
                            MOLD_NO = Convert.ToString(dr["MOLD_NO"]),
                            MOLD_GROUP = Convert.ToString(dr["MOLD_GROUP"]),
                            MOLD_STATUS = Convert.ToString(dr["MOLD_STATUS"]),
                            EBPM_NO = Convert.ToString(dr["EBPM_NO"]),
                            LIST_NO = Convert.ToString(dr["LIST_NO"]),
                            REMARK = Convert.ToString(dr["REMARK"]),
                            M_WIP_NO = Convert.ToString(dr["M_WIP_NO"]),
                            QTY = Convert.ToDecimal(dr["QTY"]),
                            LINE_ID = Convert.IsDBNull(dr["LINE_ID"]) ? null : (decimal?)dr["LINE_ID"],
                            PROD_TYPE = Convert.ToDecimal(dr["PROD_TYPE"]),
                            MOVE_OPN = dr["MOVE_OPN"].ToString(),
                            RES_QTY = Convert.IsDBNull(dr["RES_QTY"]) ? null : (decimal?)dr["RES_QTY"],
                            STUS_CTR = Convert.ToDecimal(dr["STUS_CTR"]),
                            STOREIN_DATE = Convert.ToDateTime(dr["STOREIN_DATE"]).ToString("yyyy/MM/dd"),
                            PDATE = Convert.IsDBNull(dr["PDATE"]) ? null : ((DateTime)dr["PDATE"]).ToString("yyyy/MM/dd")

                        });
                    }

                    if (AsrsID.HasValue)
                    {
                        if (AsrsID > 0)
                            Stks = Stks.Where(o => o.ASRS_ID == AsrsID).ToList();
                    }
                    
                    if (ProdType.HasValue)
                    {
                        if (ProdType >= 0)
                            Stks = Stks.Where(o => o.PROD_TYPE == ProdType).ToList();
                    }

                    if (StusCtr.HasValue)
                    {
                        if (StusCtr >= 0)
                            Stks = Stks.Where(o => o.STUS_CTR == StusCtr).ToList();
                    }

                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        Stks = Stks.Where(o => o.PROD_NO.Contains(SearchText)
                        || o.PROD_NAME.Contains(SearchText)
                        || o.BIN_NO.Contains(SearchText)
                        || o.LOT_NO.Contains(SearchText)
                        || o.QC_LOT.Contains(SearchText)
                        || o.REMARK.Contains(SearchText)
                        || o.STUS_CTR_DESC.Contains(SearchText)
                        || o.PROD_TYPE_DESC.Contains(SearchText)
                        || o.REMARK.Contains(SearchText)
                        || o.MOLD_NO.Contains(SearchText)
                        ).ToList();
                    }
                    result.count = Stks.Count;

                    result.data = Stks;
                    result.code = 0;
                }
                else
                {
                    result.count = 0;

                    result.data = Stks;
                    result.code = Convert.ToInt16(HttpStatusCode.InternalServerError);
                }
            }

            result.data = Stks.Skip((Page.Value - 1) * Limit.Value).Take(Limit.Value);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWmsBinstaData(string SearchText, int? AsrsID, string BinSta, int? Page, int? Limit)
        {
            Models.LayUITableData result = new Models.LayUITableData();
            var DataResult = BinStaService.GetData();
            List<Models.WmsBinsta> BinStas = new List<Models.WmsBinsta>();
            if (DataResult.IsCompleted)
            {
                if (DataResult.Result.Successed)
                {
                    foreach (System.Data.DataRow dr in DataResult.Result.Data.Rows)
                    {
                        BinStas.Add(new Models.WmsBinsta()
                        {
                            ASRS_ID = Convert.ToInt16(dr["ASRS_ID"]),
                            AREA = Convert.ToString(dr["AREA"]),
                            AREA_NO = Convert.ToString(dr["AREA_NO"]),
                            WH_NO = Convert.ToString(dr["WH_NO"]),
                            BIN_NO = Convert.ToString(dr["BIN_NO"]),
                            BIN_STA = Convert.ToString(dr["BIN_STA"]),
                            BIN_STA_DESC = Convert.ToString(dr["BIN_STA_DESC"]),
                            INHIBIT_IN_FLG = Convert.ToString(dr["INHIBIT_IN_FLG"]),
                            INHIBIT_OUT_FLG = Convert.ToString(dr["INHIBIT_OUT_FLG"]),
                            SIZE_CHK = Convert.ToString(dr["SIZE_CHK"])

                        });
                    }

                    if (AsrsID.HasValue)
                    {
                        if (AsrsID > 0)
                            BinStas = BinStas.Where(o => o.ASRS_ID == AsrsID).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(BinSta))
                    {
                        
                            BinStas = BinStas.Where(o => o.BIN_STA == BinSta).ToList();
                    }

                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        BinStas = BinStas.Where(o => o.BIN_NO.Contains(SearchText)
                        ).ToList();
                    }
                    result.count = BinStas.Count;

                    result.data = BinStas;
                    result.code = 0;
                }
                else
                {
                    result.count = 0;

                    result.data = BinStas;
                    result.code = Convert.ToInt16(HttpStatusCode.InternalServerError);
                }
            }

            result.data = BinStas.Skip((Page.Value - 1) * Limit.Value).Take(Limit.Value);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddWmsStk(Models.WmsStk Item)
        {
            var result = StkService.AddWmsStks(new List<Models.WmsStk>() { Item }, Session["UserNo"].ToString());
            if (result.IsCompleted)
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = result.Result.Successed, Message = result.Result.Message });
            }
            return Json("OK");
        }

        [HttpPost]
        public JsonResult DeleteWmsStk(List<Models.WmsStk> Items)
        {
            var result = StkService.DeleteWmsStks(Items, Session["UserNo"].ToString());
            if (result.IsCompleted)
            {
                return Json(new Service.WcsCrnService.PostResponse() { Successed = result.Result.Successed, Message = result.Result.Message });
            }
            return Json("OK");
        }
    }
}
