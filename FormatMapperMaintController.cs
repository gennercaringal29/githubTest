using DARS.Web.Business;
using DARS.Web.Controllers;
using DARS.Web.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using VREMIT.ModelMVC;

namespace DARS.Web.Areas.Accounts.Controllers
{
    public class FormatMapperMaintController : BaseController
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(FormatMapperMaintController));
        private string g_global_region_code = Common.WebConfig("g_global_region_code");
        private readonly DAL dal = new DAL();

        private String title = "";
        private String text = "";
        private String icon = "";

        public FormatMapperMaintController() : base()
        {
            //
        }

        // GET: SysMaint/FormatMapperMaint
        public ActionResult Index()
        {

            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_VIEW))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }

            DARS.Web.Models.FormatMapperFormViewModel formViewModel = new DARS.Web.Models.FormatMapperFormViewModel();
            formViewModel.Action = 1;

            var tieupView = new DARS.Web.Models.FormatMapperFormViewModel();
            tieupView.TieupViewDetailInputs = new TieupViewDetailInputs();

            try
            {
                GetFileType();

                List<Tieup_Codes> Tieup_Codes = new List<Tieup_Codes>();
                sqlCompositeModel customsqlCode4 = new sqlCompositeModel();
                customsqlCode4.model = new VREMIT.ModelMVC.Tieup_Codes();
                customsqlCode4.sql = "SELECT * FROM tbl_tieup_codes where status = 0 AND exists (SELECT 1 FROM tbl_tieup A where tbl_tieup_codes.tieup_codes = A.tieup_code AND A.tieup_status = 0) order by tieup_codes ASC";
                Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode4, "Tieup_Codes", g_global_region_code));
                tieupView.TieupCodesMaintParam = Tieup_Codes;
                ViewBag.TieupMot = Tieup_Codes;

                tieupView.Formatmapper_Fieldname_Details = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Formatmapper_Fieldname_Details>>(dal.GenericViewAll(new VREMIT.ModelMVC.Formatmapper_Fieldname_Details(), "Formatmapper_Fieldname_Details", g_global_region_code));

                //Search
                List<Tieup> TieupCode = new List<Tieup>();
                sqlCompositeModel customsqlCode = new sqlCompositeModel();
                customsqlCode.model = new VREMIT.ModelMVC.Tieup();
                customsqlCode.sql = "SELECT t.*, p.* FROM tbl_tieup t INNER JOIN tbl_partner_category p ON t.tieup_type = p.id where t.tieup_name <> '' and t.tieup_status = 0 and p.partner_type_desc like '%Source%' order by t.tieup_code ";
                TieupCode = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCode, "Tieup", g_global_region_code));
                tieupView.TieupCode = TieupCode;
                ViewBag.TieupCode = TieupCode;
            }
            catch (Exception ex)
            {
                LOGGER.Fatal(ex);
            }

            ViewBag.stitle = TempData["title"];
            ViewBag.stext = TempData["text"];
            ViewBag.sicon = TempData["icon"];
            ViewBag.req = TempData["req"];
            ViewBag.app_status = TempData["app_status"];

            return View(tieupView);
        }


        //save record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNew(DARS.Web.Models.FormatMapperFormViewModel tieupView)
        {

            // FOR TESTING PURPOSES ONLY / DELETE AFTER TESTING -------------------
            // 1 = Maker , 2 = Approver
            TempData["permission"] = Request.Form["permission"];
            // End ----------------------------------------------------------------
            //var tieupView = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GenericViewAll(new VREMIT.ModelMVC.Tieup_Codes(), "Tieup_Codes", g_global_region_code));
            ViewBag.App_Codes = App_Codes();
            try
            {
                List<Settlement_Currency> settlementCur = new List<Settlement_Currency>();
                sqlCompositeModel customsqlFunCur = new sqlCompositeModel();
                customsqlFunCur.model = new VREMIT.ModelMVC.Settlement_Currency();
                customsqlFunCur.sql = "SELECT CASE WHEN code_val = 'FunCur' THEN 'Funding Currency' ELSE code_val END AS code_val\r\nFROM [tbl_settlement_currency]\r\nWHERE code_val IN ('PHP', 'USD', 'FunCur'); ";
                settlementCur = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Settlement_Currency>>(dal.GetCustomSQL(customsqlFunCur, "Settlement_Currency", g_global_region_code));
                tieupView.SettlementCurrency1 = settlementCur;

                List<Tieup> TieupMot = new List<Tieup>();
                sqlCompositeModel customsqlCodeMot = new sqlCompositeModel();
                customsqlCodeMot.model = new VREMIT.ModelMVC.Tieup();
                customsqlCodeMot.sql = "select * from tbl_tieup_mot where mode_of_transmission='BATCH FILE UPLOAD (SFTP/PORTAL)' ";
                TieupMot = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCodeMot, "Tieup", g_global_region_code));
                tieupView.TieupMot = TieupMot;

                List<Tieup_Codes> Tieup_Codes = new List<Tieup_Codes>();
                sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
                customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes();
                customsqlCode3.sql = "SELECT * FROM tbl_tieup_codes where status = 0 and tieup_codes <> '' ";
                Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes", g_global_region_code));
                tieupView.TieupCodesMaintParam = Tieup_Codes;

                tieupView.Formatmapper_Fieldname_Details = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Formatmapper_Fieldname_Details>>(dal.GenericViewAll(new VREMIT.ModelMVC.Formatmapper_Fieldname_Details(), "Formatmapper_Fieldname_Details", g_global_region_code));

                ViewBag.Tieup_Codes = Tieup_Codes;

                string tranID = Common.GetgenerateID(1, Convert.ToString(ModelViewer.TIEUP_FILE_MAPPER), g_global_region_code); //Bank Code TIEUP_FILE_MAPPER

                sqlCompositeModel customsqlCode1 = new sqlCompositeModel
                {
                    model = new VREMIT.ModelMVC.Tieup_Codes_Request(),
                    sql = String.Format("SELECT * FROM tbl_tieup_codes_request WHERE tieup_codes = '" + tieupView.TieupViewMaintInputs.tieup_codes + "' AND req_status=0 AND status=0")
                };
                IList<VREMIT.ModelMVC.Tieup_Codes_Request> dupTieup = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Request>>(dal.GetCustomSQL(customsqlCode1, "Tieup_Codes_Request", g_global_region_code));

                sqlCompositeModel customsqlCode2 = new sqlCompositeModel
                {
                    model = new VREMIT.ModelMVC.Tieup_Codes(),
                    sql = String.Format("SELECT * FROM tbl_tieup_codes WHERE tieup_codes = '" + tieupView.TieupViewMaintInputs.tieup_codes + "' AND status=0 ")
                };
                IList<VREMIT.ModelMVC.Tieup_Codes> dupTieup1 = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes", g_global_region_code));


                if (dupTieup.Count > 0)
                {
                    TempData["title"] = "Unable to Save!";
                    TempData["text"] = "Record currently pending for approval";
                    TempData["icon"] = "error";
                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                }
                if (dupTieup1.Count > 0)
                {
                    TempData["title"] = "Unable to Save!";
                    TempData["text"] = "Record already exists";
                    TempData["icon"] = "error";
                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                }

                //for checking value
                var tfile = "";
                var tfoot = "";

                var funCur = "";

                if (tieupView.isFilenameValidation == "On")
                {
                    tfile = "On";
                }
                else if (tieupView.isFilenameValidation == "Off" || tieupView.isFilenameValidation == null)
                {
                    tfile = "Off";
                }

                if (tieupView.isFooterValidation == "On")
                {
                    tfoot = "On";
                }
                else if (tieupView.isFooterValidation == "Off" || tieupView.isFooterValidation == null)
                {
                    tfoot = "Off";
                }

                if (tieupView.TieupViewMaintInputs.settlement_currency == "Funding Currency")
                {
                    funCur = "FunCur";
                }
                else
                {
                    funCur = tieupView.TieupViewMaintInputs.settlement_currency;
                }

                if (!ModelState.IsValid)
                {
                    var tieupCodeDetail = new VREMIT.ModelMVC.Tieup_Codes_Request
                    {
                        pk_tieup_code_id = tranID,
                        provided = tieupView.TieupViewDetailInputs.Provided,
                        tieup_codes = tieupView.TieupViewMaintInputs.tieup_codes,
                        file_type = tieupView.TieupViewMaintInputs.file_type,
                        start_record = tieupView.TieupViewDetailInputs.detail_start_record,
                        settlement_currency = funCur,
                        remarks = tieupView.TieupViewMaintInputs.remarks,
                        filename_validation = tfile,
                        footer_validation = tfoot,
                        delimiter_val = tieupView.TieupViewDetailInputs.delimiter_val,

                        d_prefix = tieupView.TieupViewDetailInputs.d_prefix,
                        delimiter_type = tieupView.TieupViewDetailInputs.delimiter_type,
                        delimiter = tieupView.TieupViewDetailInputs.delimiter,
                        detail_start_record = tieupView.TieupViewDetailInputs.detail_start_record,
                        detail_transdate_start_column = tieupView.TieupViewDetailInputs.detail_transdate_start_column,
                        detail_transdate_length = tieupView.TieupViewDetailInputs.detail_transdate_length,
                        detail_application_start_column = tieupView.TieupViewDetailInputs.detail_application_start_column,
                        detail_application_length = tieupView.TieupViewDetailInputs.detail_application_length,
                        detail_remitid_start_column = tieupView.TieupViewDetailInputs.detail_remitid_start_column,
                        detail_remitid_length = tieupView.TieupViewDetailInputs.detail_remitid_length,
                        detail_remitfname_start_column = tieupView.TieupViewDetailInputs.detail_remitfname_start_column,
                        detail_remitfname_length = tieupView.TieupViewDetailInputs.detail_remitfname_length,
                        detail_remitmidname_start_column = tieupView.TieupViewDetailInputs.detail_remitmidname_start_column,
                        detail_remitmidname_length = tieupView.TieupViewDetailInputs.detail_remitmidname_length,
                        detail_remitlastname_start_column = tieupView.TieupViewDetailInputs.detail_remitlastname_start_column,
                        detail_remitlastname_length = tieupView.TieupViewDetailInputs.detail_remitlastname_length,
                        d_remitCusType_start_column = tieupView.TieupViewDetailInputs.d_remitCusType_start_column,
                        d_remitCusType_length = tieupView.TieupViewDetailInputs.d_remitCusType_length,
                        d_remitNationality_start_column = tieupView.TieupViewDetailInputs.d_remitNationality_start_column,
                        d_remitNationality_length = tieupView.TieupViewDetailInputs.d_remitNationality_length,
                        d_remitAdd1_start_column = tieupView.TieupViewDetailInputs.d_remitAdd1_start_column,
                        d_remitAdd1_length = tieupView.TieupViewDetailInputs.d_remitAdd1_length,
                        d_remitAdd2_start_column = tieupView.TieupViewDetailInputs.d_remitAdd2_start_column,
                        d_remitAdd2_length = tieupView.TieupViewDetailInputs.d_remitAdd2_length,
                        d_remitAdd3_start_column = tieupView.TieupViewDetailInputs.d_remitAdd3_start_column,
                        d_remitAdd3_length = tieupView.TieupViewDetailInputs.d_remitAdd3_length,
                        d_remitAdd4_start_column = tieupView.TieupViewDetailInputs.d_remitAdd4_start_column,
                        d_remitAdd4_length = tieupView.TieupViewDetailInputs.d_remitAdd4_length,
                        d_remitContinent_start_column = tieupView.TieupViewDetailInputs.d_remitContinent_start_column,
                        d_remitContinent_length = tieupView.TieupViewDetailInputs.d_remitContinent_length,
                        d_remitZipCode_start_column = tieupView.TieupViewDetailInputs.d_remitZipCode_start_column,
                        d_remitZipCode_length = tieupView.TieupViewDetailInputs.d_remitZipCode_length,
                        d_remitBDate_start_column = tieupView.TieupViewDetailInputs.d_remitBDate_start_column,
                        d_remitBDate_length = tieupView.TieupViewDetailInputs.d_remitBDate_length,
                        d_remitProf_start_column = tieupView.TieupViewDetailInputs.d_remitProf_start_column,
                        d_remitProf_length = tieupView.TieupViewDetailInputs.d_remitProf_length,
                        d_remitGender_start_column = tieupView.TieupViewDetailInputs.d_remitGender_start_column,
                        d_remitGender_length = tieupView.TieupViewDetailInputs.d_remitGender_length,
                        d_remitCivilStat_start_column = tieupView.TieupViewDetailInputs.d_remitCivilStat_start_column,
                        d_remitCivilStat_length = tieupView.TieupViewDetailInputs.d_remitCivilStat_length,
                        d_remitPOBox_start_column = tieupView.TieupViewDetailInputs.d_remitPOBox_start_column,
                        d_remitPOBox_length = tieupView.TieupViewDetailInputs.d_remitPOBox_length,
                        d_remitMobPhoneNum_start_column = tieupView.TieupViewDetailInputs.d_remitMobPhoneNum_start_column,
                        d_remitMobPhoneNum_length = tieupView.TieupViewDetailInputs.d_remitMobPhoneNum_length,
                        d_remitOfficePhoneNo_start_column = tieupView.TieupViewDetailInputs.d_remitOfficePhoneNo_start_column,
                        d_remitOfficePhoneNo_length = tieupView.TieupViewDetailInputs.d_remitOfficePhoneNo_length,
                        d_remitEmailAdd_start_column = tieupView.TieupViewDetailInputs.d_remitEmailAdd_start_column,
                        d_remitEmailAdd_length = tieupView.TieupViewDetailInputs.d_remitEmailAdd_length,
                        d_remitTaxIDNo_start_column = tieupView.TieupViewDetailInputs.d_remitTaxIDNo_start_column,
                        d_remitTaxIDNo_length = tieupView.TieupViewDetailInputs.d_remitTaxIDNo_length,
                        d_remitIDType1_start_column = tieupView.TieupViewDetailInputs.d_remitIDType1_start_column,
                        d_remitIDType1_length = tieupView.TieupViewDetailInputs.d_remitIDType1_length,
                        d_remitIDNum1_start_column = tieupView.TieupViewDetailInputs.d_remitIDNum1_start_column,
                        d_remitIDNum1_length = tieupView.TieupViewDetailInputs.d_remitIDNum1_length,
                        d_remitIDIssueAt1_start_column = tieupView.TieupViewDetailInputs.d_remitIDIssueAt1_start_column,
                        d_remitIDIssueAt1_length = tieupView.TieupViewDetailInputs.d_remitIDIssueAt1_length,
                        d_remitIDExpDate1_start_column = tieupView.TieupViewDetailInputs.d_remitIDExpDate1_start_column,
                        d_remitIDExpDate1_length = tieupView.TieupViewDetailInputs.d_remitIDExpDate1_length,
                        d_remitIDType2_start_column = tieupView.TieupViewDetailInputs.d_remitIDType2_start_column,
                        d_remitIDType2_length = tieupView.TieupViewDetailInputs.d_remitIDType2_length,
                        d_remitIDNo2_start_column = tieupView.TieupViewDetailInputs.d_remitIDNo2_start_column,
                        d_remitIDNo2_length = tieupView.TieupViewDetailInputs.d_remitIDNo2_length,
                        d_remitIDIssueAt2_start_column = tieupView.TieupViewDetailInputs.d_remitIDIssueAt2_start_column,
                        d_remitIDIssueAt2_length = tieupView.TieupViewDetailInputs.d_remitIDIssueAt2_length,
                        d_remitIDExpDate2_start_column = tieupView.TieupViewDetailInputs.d_remitIDExpDate2_start_column,
                        d_remitIDExpDate2_length = tieupView.TieupViewDetailInputs.d_remitIDExpDate2_length,
                        d_remitNotifType_start_column = tieupView.TieupViewDetailInputs.d_remitNotifType_start_column,
                        d_remitNotifType_length = tieupView.TieupViewDetailInputs.d_remitNotifType_length,
                        d_beneficiaryID_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryID_start_column,
                        d_beneficiaryID_length = tieupView.TieupViewDetailInputs.d_beneficiaryID_length,
                        d_beneficiaryFName_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryFName_start_column,
                        d_beneficiaryFName_length = tieupView.TieupViewDetailInputs.d_beneficiaryFName_length,
                        d_beneficiaryMName_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryMName_start_column,
                        d_beneficiaryMName_length = tieupView.TieupViewDetailInputs.d_beneficiaryMName_length,
                        d_beneficiaryLName_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryLName_start_column,
                        d_beneficiaryLName_length = tieupView.TieupViewDetailInputs.d_beneficiaryLName_length,
                        d_beneficiaryCusType_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryCusType_start_column,
                        d_beneficiaryCusType_length = tieupView.TieupViewDetailInputs.d_beneficiaryCusType_length,
                        d_beneficiaryAdd1_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryAdd1_start_column,
                        d_beneficiaryAdd1_length = tieupView.TieupViewDetailInputs.d_beneficiaryAdd1_length,
                        d_beneficiaryAdd2_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryAdd2_start_column,
                        d_beneficiaryAdd2_length = tieupView.TieupViewDetailInputs.d_beneficiaryAdd2_length,
                        d_beneficiaryAdd3_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryAdd3_start_column,
                        d_beneficiaryAdd3_length = tieupView.TieupViewDetailInputs.d_beneficiaryAdd3_length,
                        d_beneficiaryCountry_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryCountry_start_column,
                        d_beneficiaryCountry_length = tieupView.TieupViewDetailInputs.d_beneficiaryCountry_length,
                        d_zipCode_start_column = tieupView.TieupViewDetailInputs.d_zipCode_start_column,
                        d_zipCode_length = tieupView.TieupViewDetailInputs.d_zipCode_length,
                        d_landMark_start_column = tieupView.TieupViewDetailInputs.d_landMark_start_column,
                        d_landMark_length = tieupView.TieupViewDetailInputs.d_landMark_length,
                        d_beneficiaryNationality_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryNationality_start_column,
                        d_beneficiaryNationality_length = tieupView.TieupViewDetailInputs.d_beneficiaryNationality_length,
                        d_beneficiaryBDate_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryBDate_start_column,
                        d_beneficiaryBDate_length = tieupView.TieupViewDetailInputs.d_beneficiaryBDate_length,
                        d_beneficiaryProf_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryProf_start_column,
                        d_beneficiaryProf_length = tieupView.TieupViewDetailInputs.d_beneficiaryProf_length,
                        d_beneficiaryGender_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryGender_start_column,
                        d_beneficiaryGender_length = tieupView.TieupViewDetailInputs.d_beneficiaryGender_length,
                        d_beneficiaryCivilStat_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryCivilStat_start_column,
                        d_beneficiaryCivilStat_length = tieupView.TieupViewDetailInputs.d_beneficiaryCivilStat_length,
                        d_beneficiaryPOBox_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryPOBox_start_column,
                        d_beneficiaryPOBox_length = tieupView.TieupViewDetailInputs.d_beneficiaryPOBox_length,
                        d_beneficiaryRel_tothe_remit_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryRel_tothe_remit_start_column,
                        d_beneficiaryRel_tothe_remit_length = tieupView.TieupViewDetailInputs.d_beneficiaryRel_tothe_remit_length,
                        d_alterRecipient_reltobeneficiary_start_column = tieupView.TieupViewDetailInputs.d_alterRecipient_reltobeneficiary_start_column,
                        d_alterRecipient_reltobeneficiary_length = tieupView.TieupViewDetailInputs.d_alterRecipient_reltobeneficiary_length,
                        d_beneficiaryMobPhoneNo_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryMobPhoneNo_start_column,
                        d_beneficiaryMobPhoneNo_length = tieupView.TieupViewDetailInputs.d_beneficiaryMobPhoneNo_length,
                        d_beneficiaryOfficePhoneNo_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryOfficePhoneNo_start_column,
                        d_beneficiaryOfficePhoneNo_length = tieupView.TieupViewDetailInputs.d_beneficiaryOfficePhoneNo_length,
                        d_beneficiaryEmailAdd_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryEmailAdd_start_column,
                        d_beneficiaryEmailAdd_length = tieupView.TieupViewDetailInputs.d_beneficiaryEmailAdd_length,
                        d_beneficiaryTaxIDNo_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryTaxIDNo_start_column,
                        d_beneficiaryTaxIDNo_length = tieupView.TieupViewDetailInputs.d_beneficiaryTaxIDNo_length,
                        d_beneficiaryNotifType_start_column = tieupView.TieupViewDetailInputs.d_beneficiaryNotifType_start_column,
                        d_beneficiaryNotifType_length = tieupView.TieupViewDetailInputs.d_beneficiaryNotifType_length,
                        d_fundCurrency_start_column = tieupView.TieupViewDetailInputs.d_fundCurrency_start_column,
                        d_fundCurrency_length = tieupView.TieupViewDetailInputs.d_fundCurrency_length,
                        d_fundAmount_start_column = tieupView.TieupViewDetailInputs.d_fundAmount_start_column,
                        d_fundAmount_length = tieupView.TieupViewDetailInputs.d_fundAmount_length,
                        d_buyingRate_start_column = tieupView.TieupViewDetailInputs.d_buyingRate_start_column,
                        d_buyingRate_length = tieupView.TieupViewDetailInputs.d_buyingRate_length,
                        d_settlementCurrency_start_column = tieupView.TieupViewDetailInputs.d_settlementCurrency_start_column,
                        d_settlementCurrency_length = tieupView.TieupViewDetailInputs.d_settlementCurrency_length,
                        d_settlementAmount_start_column = tieupView.TieupViewDetailInputs.d_settlementAmount_start_column,
                        d_settlementAmount_length = tieupView.TieupViewDetailInputs.d_settlementAmount_length,
                        d_settlementMode_start_column = tieupView.TieupViewDetailInputs.d_settlementMode_start_column,
                        d_settlementMode_length = tieupView.TieupViewDetailInputs.d_settlementMode_length,
                        d_bankCode_start_column = tieupView.TieupViewDetailInputs.d_bankCode_start_column,
                        d_bankCode_length = tieupView.TieupViewDetailInputs.d_bankCode_length,
                        d_bankName_start_column = tieupView.TieupViewDetailInputs.d_bankName_start_column,
                        d_bankName_length = tieupView.TieupViewDetailInputs.d_bankName_length,
                        d_branchCode_start_column = tieupView.TieupViewDetailInputs.d_branchCode_start_column,
                        d_branchCode_length = tieupView.TieupViewDetailInputs.d_branchCode_length,
                        d_branchName_start_column = tieupView.TieupViewDetailInputs.d_branchName_start_column,
                        d_branchName_length = tieupView.TieupViewDetailInputs.d_branchName_length,
                        d_accountType_start_column = tieupView.TieupViewDetailInputs.d_accountType_start_column,
                        d_accountType_length = tieupView.TieupViewDetailInputs.d_accountType_length,
                        d_accountNo_start_column = tieupView.TieupViewDetailInputs.d_accountNo_start_column,
                        d_accountNo_length = tieupView.TieupViewDetailInputs.d_accountNo_length,
                        d_goldCardNo_start_column = tieupView.TieupViewDetailInputs.d_goldCardNo_start_column,
                        d_goldCardNo_length = tieupView.TieupViewDetailInputs.d_goldCardNo_length,
                        d_billsPayField1_start_column = tieupView.TieupViewDetailInputs.d_billsPayField1_start_column,
                        d_billsPayField1_length = tieupView.TieupViewDetailInputs.d_billsPayField1_length,
                        d_billsPayField2_start_column = tieupView.TieupViewDetailInputs.d_billsPayField2_start_column,
                        d_billsPayField2_length = tieupView.TieupViewDetailInputs.d_billsPayField2_length,
                        d_billsPayField3_start_column = tieupView.TieupViewDetailInputs.d_billsPayField3_start_column,
                        d_billsPayField3_length = tieupView.TieupViewDetailInputs.d_billsPayField3_length,
                        d_billsPayField4_start_column = tieupView.TieupViewDetailInputs.d_billsPayField4_start_column,
                        d_billsPayField4_length = tieupView.TieupViewDetailInputs.d_billsPayField4_length,
                        d_billsPayField5_start_column = tieupView.TieupViewDetailInputs.d_billsPayField5_start_column,
                        d_billsPayField5_length = tieupView.TieupViewDetailInputs.d_billsPayField5_length,
                        d_outletCode_start_column = tieupView.TieupViewDetailInputs.d_outletCode_start_column,
                        d_outletCode_length = tieupView.TieupViewDetailInputs.d_outletCode_length,
                        d_outletBranchCode_start_column = tieupView.TieupViewDetailInputs.d_outletBranchCode_start_column,
                        d_outletBranchCode_length = tieupView.TieupViewDetailInputs.d_outletBranchCode_length,
                        d_alterBeneficiaryName_start_column = tieupView.TieupViewDetailInputs.d_alterBeneficiaryName_start_column,
                        d_alterBeneficiaryName_length = tieupView.TieupViewDetailInputs.d_alterBeneficiaryName_length,
                        d_alterBeneficiary_relto_beneficiary_start_column = tieupView.TieupViewDetailInputs.d_alterBeneficiary_relto_beneficiary_start_column,
                        d_alterBeneficiary_relto_beneficiary_length = tieupView.TieupViewDetailInputs.d_alterBeneficiary_relto_beneficiary_length,
                        d_messageToBeneficiary_start_column = tieupView.TieupViewDetailInputs.d_messageToBeneficiary_start_column,
                        d_messageToBeneficiary_length = tieupView.TieupViewDetailInputs.d_messageToBeneficiary_length,
                        d_receiverCorresBank_start_column = tieupView.TieupViewDetailInputs.d_receiverCorresBank_start_column,
                        d_receiverCorresBank_length = tieupView.TieupViewDetailInputs.d_receiverCorresBank_length,
                        d_senderCorresBank_start_column = tieupView.TieupViewDetailInputs.d_senderCorresBank_start_column,
                        d_senderCorresBank_length = tieupView.TieupViewDetailInputs.d_senderCorresBank_length,
                        d_sendingBank_start_column = tieupView.TieupViewDetailInputs.d_sendingBank_start_column,
                        d_sendingBank_length = tieupView.TieupViewDetailInputs.d_sendingBank_length,
                        d_receivingBank_start_column = tieupView.TieupViewDetailInputs.d_receivingBank_start_column,
                        d_receivingBank_length = tieupView.TieupViewDetailInputs.d_receivingBank_length,
                        d_modeOfChange_start_column = tieupView.TieupViewDetailInputs.d_modeOfChange_start_column,
                        d_modeOfChange_length = tieupView.TieupViewDetailInputs.d_modeOfChange_length,
                        d_purposeCode_start_column = tieupView.TieupViewDetailInputs.d_purposeCode_start_column,
                        d_purposeCode_length = tieupView.TieupViewDetailInputs.d_purposeCode_length,
                        d_indvCode_start_column = tieupView.TieupViewDetailInputs.d_indvCode_start_column,
                        d_indvCode_length = tieupView.TieupViewDetailInputs.d_indvCode_length,

                        //---- Default Data ----- 
                        applicationNumber = tieupView.TieupViewDetailInputs.applicationNumber,
                        settlementMode = tieupView.TieupViewDetailInputs.settlementMode,
                        fundingAmount = tieupView.TieupViewDetailInputs.fundingAmount,
                        fundingCurrency = tieupView.TieupViewDetailInputs.fundingCurrency,
                        remitterFirstName = tieupView.TieupViewDetailInputs.remitterFirstName,
                        beneficiaryFirstName = tieupView.TieupViewDetailInputs.beneficiaryFirstName,
                        accountNumber = tieupView.TieupViewDetailInputs.accountNumber,
                        bankCode = tieupView.TieupViewDetailInputs.bankCode,
                        beneficiaryAddress1 = tieupView.TieupViewDetailInputs.beneficiaryAddress1,
                        remitterNationality = tieupView.TieupViewDetailInputs.remitterNationality,
                        remitterAddress1 = tieupView.TieupViewDetailInputs.remitterAddress1,
                        //remitterCountry = tieupView.TieupViewDetailInputs.remitterCountry,
                        //remitterZipCode = tieupView.TieupViewDetailInputs.remitterZipCode,
                        remitterBirthDate = tieupView.TieupViewDetailInputs.remitterBirthDate,
                        remitterMobileNumber = tieupView.TieupViewDetailInputs.remitterMobileNumber,
                        remitterIdType1 = tieupView.TieupViewDetailInputs.remitterIdType1,
                        remitterIdNumber1 = tieupView.TieupViewDetailInputs.remitterIdNumber1,
                        remitterIdIssuedAt1 = tieupView.TieupViewDetailInputs.remitterIdIssuedAt1,
                        remitterIdExpiry1 = tieupView.TieupViewDetailInputs.remitterIdExpiry1,
                        beneficiaryMiddleName = tieupView.TieupViewDetailInputs.beneficiaryMiddleName,
                        beneficiaryLastName = tieupView.TieupViewDetailInputs.beneficiaryLastName,
                        beneficiaryCustomerType = tieupView.TieupViewDetailInputs.beneficiaryCustomerType,
                        beneficiaryZipCode = tieupView.TieupViewDetailInputs.beneficiaryZipCode,
                        beneficiaryNationality = tieupView.TieupViewDetailInputs.beneficiaryNationality,
                        beneficiaryBirthDate = tieupView.TieupViewDetailInputs.beneficiaryBirthDate,
                        beneficiaryRelationToRemitter = tieupView.TieupViewDetailInputs.beneficiaryRelationToRemitter,
                        beneficiaryMobileNumber = tieupView.TieupViewDetailInputs.beneficiaryMobileNumber,
                        buyingRate = tieupView.TieupViewDetailInputs.buyingRate,
                        settlementCurrency = tieupView.TieupViewDetailInputs.settlementCurrency == "Funding Currency" ? "FunCur" : tieupView.TieupViewDetailInputs.settlementCurrency,
                        paymentField1 = tieupView.TieupViewDetailInputs.paymentField1,
                        paymentField2 = tieupView.TieupViewDetailInputs.paymentField2,
                        paymentField3 = tieupView.TieupViewDetailInputs.paymentField3,
                        outletCode = tieupView.TieupViewDetailInputs.outletCode,
                        //senderCorrespondentBank = tieupView.TieupViewDetailInputs.senderCorrespondentBank,
                        //sendingBank = tieupView.TieupViewDetailInputs.sendingBank,
                        //receivingBank = tieupView.TieupViewDetailInputs.receivingBank,
                        //modeOfCharge = tieupView.TieupViewDetailInputs.modeOfCharge,
                        //purposeCode = tieupView.TieupViewDetailInputs.purposeCode,
                        //individualCode = tieupView.TieupViewDetailInputs.individualCode,
                        //natureOfBusiness = tieupView.TieupViewDetailInputs.natureOfBusiness,
                        remitterId = tieupView.TieupViewDetailInputs.remitterId,
                        remitterMiddleName = tieupView.TieupViewDetailInputs.remitterMiddleName,
                        remitterLastName = tieupView.TieupViewDetailInputs.remitterLastName,
                        remitterCustomerType = tieupView.TieupViewDetailInputs.remitterCustomerType,
                        settlementAmount = tieupView.TieupViewDetailInputs.settlementAmount,


                        remitterAddress2 = tieupView.TieupViewDetailInputs.remitterAddress2,
                        remitterAddress3 = tieupView.TieupViewDetailInputs.remitterAddress3,
                        remitterAddress4 = tieupView.TieupViewDetailInputs.remitterAddress4,
                        //remitterContinent = tieupView.TieupViewDetailInputs.remitterContinent,
                        //remitterProfession = tieupView.TieupViewDetailInputs.remitterProfession,
                        //remitterGender = tieupView.TieupViewDetailInputs.remitterGender,
                        //remitterCivilStatus = tieupView.TieupViewDetailInputs.remitterCivilStatus,
                        //remitterPoBox = tieupView.TieupViewDetailInputs.remitterPoBox,
                        //remitterOfficeNumber = tieupView.TieupViewDetailInputs.remitterOfficeNumber,
                        //remitterEmail = tieupView.TieupViewDetailInputs.remitterEmail,
                        //remitterTin = tieupView.TieupViewDetailInputs.remitterTin,
                        //remitterIdType2 = tieupView.TieupViewDetailInputs.remitterIdType2,
                        //remitterIdNumber2 = tieupView.TieupViewDetailInputs.remitterIdNumber2,
                        //remitterIdIssuedAt2 = tieupView.TieupViewDetailInputs.remitterIdIssuedAt2,
                        //remitterIdExpiry2 = tieupView.TieupViewDetailInputs.remitterIdExpiry2,
                        //remitterNotificationType = tieupView.TieupViewDetailInputs.remitterNotificationType,
                        //beneficiaryId = tieupView.TieupViewDetailInputs.beneficiaryId,
                        beneficiaryAddress2 = tieupView.TieupViewDetailInputs.beneficiaryAddress2,
                        beneficiaryAddress3 = tieupView.TieupViewDetailInputs.beneficiaryAddress3,
                        beneficiaryCountry = tieupView.TieupViewDetailInputs.beneficiaryCountry,
                        //beneficiaryLandmark = tieupView.TieupViewDetailInputs.beneficiaryLandmark,
                        //beneficiaryProfession = tieupView.TieupViewDetailInputs.beneficiaryProfession,
                        //beneficiaryGender = tieupView.TieupViewDetailInputs.beneficiaryGender,
                        //beneficiaryCivilStatus = tieupView.TieupViewDetailInputs.beneficiaryCivilStatus,
                        //beneficiaryPoBox = tieupView.TieupViewDetailInputs.beneficiaryPoBox,
                        //alternateRecipientRelationToBeneficiary = tieupView.TieupViewDetailInputs.alternateRecipientRelationToBeneficiary,
                        //beneficiaryOfficeNumber = tieupView.TieupViewDetailInputs.beneficiaryOfficeNumber,
                        beneficiaryEmail = tieupView.TieupViewDetailInputs.beneficiaryEmail,
                        //beneficiaryTin = tieupView.TieupViewDetailInputs.beneficiaryTin,
                        //beneficiaryNotificationType = tieupView.TieupViewDetailInputs.beneficiaryNotificationType,
                        transactionDate = tieupView.TieupViewDetailInputs.transactionDate,
                        bankName = tieupView.TieupViewDetailInputs.bankName,
                        //branchName = tieupView.TieupViewDetailInputs.branchName,
                        //accountType = tieupView.TieupViewDetailInputs.accountType,
                        //goldCardNumber = tieupView.TieupViewDetailInputs.goldCardNumber,
                        paymentField4 = tieupView.TieupViewDetailInputs.paymentField4,
                        paymentField5 = tieupView.TieupViewDetailInputs.paymentField5,
                        outletBranchCode = tieupView.TieupViewDetailInputs.outletBranchCode,
                        //alternateBeneficiary = tieupView.TieupViewDetailInputs.alternateBeneficiary,
                        //alternateBeneficiaryRelationToBeneficiary = tieupView.TieupViewDetailInputs.alternateBeneficiaryRelationToBeneficiary,
                        //messageToBeneficiary = tieupView.TieupViewDetailInputs.messageToBeneficiary,
                        //receiverCorrespondentBank = tieupView.TieupViewDetailInputs.receiverCorrespondentBank,
                        branchCode = tieupView.TieupViewDetailInputs.branchCode,

                        //New Format Mapper 2.0 added
                        //partnerCode_start_column = tieupView.TieupViewDetailInputs.PartnerCodeStartColumn,
                        //partnerCode_length = tieupView.TieupViewDetailInputs.PartnerCodeLength,
                        //partnerCode_default = tieupView.TieupViewDetailInputs.PartnerCodeDefault,
                        acctName_start_column = tieupView.TieupViewDetailInputs.AcctNameStartColumn,
                        acctName_length = tieupView.TieupViewDetailInputs.AcctNameLength,
                        acctName_default = tieupView.TieupViewDetailInputs.AcctNameDefault,
                        remCountry_start_column = tieupView.TieupViewDetailInputs.RemCountryStartColumn,
                        remCountry_length = tieupView.TieupViewDetailInputs.RemCountryLength,
                        remCountry_default = tieupView.TieupViewDetailInputs.RemCountryDefault,
                        remFourthNme_start_column = tieupView.TieupViewDetailInputs.RemCountryStartColumn,
                        remFourthNme_length = tieupView.TieupViewDetailInputs.RemFourthNmeLength,
                        remFourthNme_default = tieupView.TieupViewDetailInputs.RemFourthNmeDefault,
                        remPlaceBrth_start_column = tieupView.TieupViewDetailInputs.RemPlaceBrthStartColumn,
                        remPlaceBrth_length = tieupView.TieupViewDetailInputs.RemPlaceBrthLength,
                        remPlaceBrth_default = tieupView.TieupViewDetailInputs.RemPlaceBrthDefault,
                        remAcctNum_start_column = tieupView.TieupViewDetailInputs.RemAcctNumStartColumn,
                        remAcctNum_length = tieupView.TieupViewDetailInputs.RemAcctNumLength,
                        remAcctNum_default = tieupView.TieupViewDetailInputs.RemAcctNumDefault,
                        remIBAN_start_column = tieupView.TieupViewDetailInputs.RemIBANStartColumn,
                        remIBAN_length = tieupView.TieupViewDetailInputs.RemIBANLength,
                        remIBAN_default = tieupView.TieupViewDetailInputs.RemIBANDefault,
                        res1_start_column = tieupView.TieupViewDetailInputs.Res1StartColumn,
                        res1_length = tieupView.TieupViewDetailInputs.Res1Length,
                        res1_default = tieupView.TieupViewDetailInputs.Res1Default,
                        res2_start_column = tieupView.TieupViewDetailInputs.Res2StartColumn,
                        res2_length = tieupView.TieupViewDetailInputs.Res2Length,
                        res2_default = tieupView.TieupViewDetailInputs.Res2Default,
                        res3_start_column = tieupView.TieupViewDetailInputs.Res3StartColumn,
                        res3_length = tieupView.TieupViewDetailInputs.Res3Length,
                        res3_default = tieupView.TieupViewDetailInputs.Res3Default,
                        res4_start_column = tieupView.TieupViewDetailInputs.Res4StartColumn,
                        res4_length = tieupView.TieupViewDetailInputs.Res4Length,
                        res4_default = tieupView.TieupViewDetailInputs.Res4Default,
                        res5_start_column = tieupView.TieupViewDetailInputs.Res5StartColumn,
                        res5_length = tieupView.TieupViewDetailInputs.Res5Length,
                        res5_default = tieupView.TieupViewDetailInputs.Res5Default,
                        res6_start_column = tieupView.TieupViewDetailInputs.Res6StartColumn,
                        res6_length = tieupView.TieupViewDetailInputs.Res6Length,
                        res6_default = tieupView.TieupViewDetailInputs.Res6Default,
                        res7_start_column = tieupView.TieupViewDetailInputs.Res7StartColumn,
                        res7_length = tieupView.TieupViewDetailInputs.Res7Length,
                        res7_default = tieupView.TieupViewDetailInputs.Res7Default,
                        res8_start_column = tieupView.TieupViewDetailInputs.Res8StartColumn,
                        res8_length = tieupView.TieupViewDetailInputs.Res8Length,
                        res8_default = tieupView.TieupViewDetailInputs.Res8Default,
                        res9_start_column = tieupView.TieupViewDetailInputs.Res9StartColumn,
                        res9_length = tieupView.TieupViewDetailInputs.Res9Length,
                        res9_default = tieupView.TieupViewDetailInputs.Res9Default,
                        res10_start_column = tieupView.TieupViewDetailInputs.Res10StartColumn,
                        res10_length = tieupView.TieupViewDetailInputs.Res10Length,
                        res10_default = tieupView.TieupViewDetailInputs.Res10Default,
                        sourceOfRem_start_column = tieupView.TieupViewDetailInputs.SourceOfRemStartColumn,
                        sourceOfRem_length = tieupView.TieupViewDetailInputs.SourceOfRemLength,
                        sourceOfRem_default = tieupView.TieupViewDetailInputs.SourceOfRemDefault,
                        natrueOfBuss_start_column = tieupView.TieupViewDetailInputs.NatrueOfBussStartColumn,
                        natrueOfBuss_length = tieupView.TieupViewDetailInputs.NatrueOfBussLength,
                        natrueOfBuss_default = tieupView.TieupViewDetailInputs.NatrueOfBussDefault,
                        purposeOfRem_start_column = tieupView.TieupViewDetailInputs.PurposeOfRemStartColumn,
                        purposeOfRem_length = tieupView.TieupViewDetailInputs.PurposeOfRemLength,
                        purposeOfRem_default = tieupView.TieupViewDetailInputs.PurposeOfRemDefault,

                        status = "0",
                        username = _currentUser.Username,
                        dt_last_chg = DateTime.Now,
                        req_type = ModelViewer.REQ_TYPE_CREATE,
                        req_status = ModelViewer.REQ_STATUS_PENDING
                    };
                    ModelResponse modelResponseDetail = dal.GenericCreate(tieupCodeDetail, "Tieup_Codes_Request", g_global_region_code);
                    System.Diagnostics.Debug.WriteLine("model response detail " + modelResponseDetail.code);

                    if (modelResponseDetail.code == 0)
                    {
                        TempData["title"] = "Add New";
                        TempData["text"] = "New request has been successfully submitted for approval";
                        TempData["icon"] = "success";
                        return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["title"] = "Format Mapper Failed!";
                        TempData["text"] = "Tie-up already exists";
                        TempData["icon"] = "error";
                        return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.Fatal(ex);
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckDuplicateRequest(DARS.Web.Models.FormatMapperFormViewModel formViewModel)
        {
            formViewModel.Action = 2;

            try
            {
                VREMIT.ModelMVC.Tieup_Codes m = new VREMIT.ModelMVC.Tieup_Codes();
                m.pk_tieup_code_id = formViewModel.tieup_code_id;
                m = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes>(dal.GetGenericDetails(m, "Tieup_Codes", g_global_region_code));

                sqlCompositeModel customsqlCode1 = new sqlCompositeModel
                {
                    model = new VREMIT.ModelMVC.Tieup_Codes_Request(),
                    sql = String.Format("SELECT * FROM tbl_tieup_codes_request WHERE tieup_codes = '{0}' AND ((req_status=0 AND status=0) OR (req_status=0 AND status=1))", m.tieup_codes)
                };
                IList<VREMIT.ModelMVC.Tieup_Codes_Request> dupTieup = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Request>>(dal.GetCustomSQL(customsqlCode1, "Tieup_Codes_Request", g_global_region_code));

                if (dupTieup.Count > 0)
                {
                    TempData["title"] = "";
                    TempData["text"] = "Cannot proceed, this record has pending request for approval";
                    TempData["icon"] = "error";
                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.ToString(), ex);

                ModelState.AddModelError("", "An error encountered while processing request");

                return Json(new { success = false, errorMessage = "An error encountered while processing request" }, JsonRequestBehavior.AllowGet);
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        // Edit Record & View
        public ActionResult MaintEditFormatMapper(string i)
        {

            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_MODIFY))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }

            DARS.Web.Models.FormatMapperFormViewModel formViewModel = new DARS.Web.Models.FormatMapperFormViewModel();
            formViewModel.Action = 2;
            ViewBag.App_Codes = App_Codes();

            VREMIT.ModelMVC.Tieup_Codes tieupView = new VREMIT.ModelMVC.Tieup_Codes();
            VREMIT.ModelMVC.Tieup_Codes_Detail tieupViewDetail = new VREMIT.ModelMVC.Tieup_Codes_Detail();

            try
            {
                List<Tieup> TieupMot = new List<Tieup>();
                sqlCompositeModel customsqlCodeMot = new sqlCompositeModel();
                customsqlCodeMot.model = new VREMIT.ModelMVC.Tieup();
                customsqlCodeMot.sql = "select * from tbl_tieup";
                TieupMot = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCodeMot, "Tieup", g_global_region_code));
                formViewModel.TieupMot = TieupMot;
                ViewBag.TieupCodeMot = TieupMot;

                List<Settlement_Currency> settlementCur = new List<Settlement_Currency>();
                sqlCompositeModel customsqlFunCur = new sqlCompositeModel();
                customsqlFunCur.model = new VREMIT.ModelMVC.Settlement_Currency();
                customsqlFunCur.sql = "SELECT CASE WHEN code_val = 'FunCur' THEN 'Funding Currency' ELSE code_val END AS code_val FROM [tbl_settlement_currency] WHERE code_val IN ('PHP', 'USD', 'FunCur'); ";
                settlementCur = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Settlement_Currency>>(dal.GetCustomSQL(customsqlFunCur, "Settlement_Currency", g_global_region_code));
                formViewModel.SettlementCurrency1 = settlementCur;

                List<Tieup> TieupCode = new List<Tieup>();
                sqlCompositeModel customsqlCode = new sqlCompositeModel();
                customsqlCode.model = new VREMIT.ModelMVC.Tieup();
                customsqlCode.sql = "select * from tbl_tieup where tieup_name <> '' ";
                TieupCode = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCode, "Tieup", g_global_region_code));
                formViewModel.TieupCode = TieupCode;

                sqlCompositeModel customsqlCode1 = new sqlCompositeModel();
                customsqlCode1.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
                customsqlCode1.sql = "SELECT * FROM [tbl_tieup_codes_fileType] where delimiterType in ('Delimiter','Fixed Length') ";
                formViewModel.TieupCodesFileTypes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode1, "Tieup_Codes_FileType", g_global_region_code));

                sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
                customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
                customsqlCode2.sql = "SELECT file_type FROM [tbl_tieup_codes_fileType] where file_type <> '' ";
                formViewModel.TieupCodesFileTypes1 = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_FileType", g_global_region_code));

                sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
                customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
                customsqlCode3.sql = "SELECT delimiters from [tbl_tieup_codes_fileType] where delimiters <> '' ";
                formViewModel.TieupCodesFileTypes2 = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes_FileType", g_global_region_code));


                formViewModel.Formatmapper_Fieldname_Details = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Formatmapper_Fieldname_Details>>(dal.GenericViewAll(new VREMIT.ModelMVC.Formatmapper_Fieldname_Details(), "Formatmapper_Fieldname_Details", g_global_region_code));

                tieupView.pk_tieup_code_id = i;
                tieupView = JsonConvert.DeserializeObject<Tieup_Codes>(dal.GetGenericDetails(tieupView, "Tieup_Codes", g_global_region_code));

                tieupViewDetail.pk_detail_tieup_id = i;
                tieupViewDetail = JsonConvert.DeserializeObject<Tieup_Codes_Detail>(dal.GetGenericDetails(tieupViewDetail, "Tieup_Codes_Detail", g_global_region_code));

                formViewModel.TieupCodesFileTypes1 = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GenericViewAll(new VREMIT.ModelMVC.Tieup_Codes_FileType(), "Tieup_Codes_FileType", g_global_region_code));

                var childDetail = new DARS.Web.Models.TieupViewMaintInputs
                {
                    tieup_code_id = tieupView.pk_tieup_code_id,
                    tieup_codes = tieupView.tieup_codes,
                    status = Convert.ToByte(tieupView.status),
                    file_type = tieupView.file_type,
                    start_record = tieupView.start_record,
                    filenameV = tieupView.filename_validation,
                    footerV = tieupView.footer_validation,
                    //remarks = tieupView.remarks,
                };

                var tieupDetail = new DARS.Web.Models.TieupViewDetailInputs
                {
                    settlement_currency = tieupViewDetail.settlementCurrency == "FunCur" ? "Funding Currency" : tieupViewDetail.settlementCurrency, //Handling data only
                    tieup_code_id = tieupView.tieup_codes,
                    Provided = tieupViewDetail.provided,
                    file_type = tieupView.file_type,
                    delimiter_val = tieupViewDetail.delimiter_val,
                    detail_tieup_id = tieupView.pk_tieup_code_id,
                    detail_start_record = tieupViewDetail.detail_start_record,
                    detail_transdate_start_column = tieupViewDetail.detail_transdate_start_column,
                    detail_transdate_length = tieupViewDetail.detail_transdate_length,
                    detail_application_start_column = tieupViewDetail.detail_application_start_column,
                    detail_application_length = tieupViewDetail.detail_application_length,
                    detail_remitid_start_column = tieupViewDetail.detail_remitid_start_column,
                    detail_remitid_length = tieupViewDetail.detail_remitid_length,
                    detail_remitfname_start_column = tieupViewDetail.detail_remitfname_start_column,
                    detail_remitfname_length = tieupViewDetail.detail_remitfname_length,
                    detail_remitmidname_start_column = tieupViewDetail.detail_remitmidname_start_column,
                    detail_remitmidname_length = tieupViewDetail.detail_remitmidname_length,
                    detail_remitlastname_start_column = tieupViewDetail.detail_remitlastname_start_column,
                    detail_remitlastname_length = tieupViewDetail.detail_remitlastname_length,
                    delimiter_type = tieupViewDetail.delimiter_type,
                    delimiter = tieupViewDetail.delimiter,
                    d_prefix = tieupViewDetail.d_prefix,
                    d_remitCusType_start_column = tieupViewDetail.d_remitCusType_start_column,
                    d_remitCusType_length = tieupViewDetail.d_remitCusType_length,
                    d_remitNationality_start_column = tieupViewDetail.d_remitNationality_start_column,
                    d_remitNationality_length = tieupViewDetail.d_remitNationality_length,
                    d_remitAdd1_start_column = tieupViewDetail.d_remitAdd1_start_column,
                    d_remitAdd1_length = tieupViewDetail.d_remitAdd1_length,
                    d_remitAdd2_start_column = tieupViewDetail.d_remitAdd2_start_column,
                    d_remitAdd2_length = tieupViewDetail.d_remitAdd2_length,
                    d_remitAdd3_start_column = tieupViewDetail.d_remitAdd3_start_column,
                    d_remitAdd3_length = tieupViewDetail.d_remitAdd3_length,
                    d_remitAdd4_start_column = tieupViewDetail.d_remitAdd4_start_column,
                    d_remitAdd4_length = tieupViewDetail.d_remitAdd4_length,
                    d_remitContinent_start_column = tieupViewDetail.d_remitContinent_start_column,
                    d_remitContinent_length = tieupViewDetail.d_remitContinent_length,
                    d_remitZipCode_start_column = tieupViewDetail.d_remitZipCode_start_column,
                    d_remitZipCode_length = tieupViewDetail.d_remitZipCode_length,
                    d_remitBDate_start_column = tieupViewDetail.d_remitBDate_start_column,
                    d_remitBDate_length = tieupViewDetail.d_remitBDate_length,
                    d_remitProf_start_column = tieupViewDetail.d_remitProf_start_column,
                    d_remitProf_length = tieupViewDetail.d_remitProf_length,
                    d_remitGender_start_column = tieupViewDetail.d_remitGender_start_column,
                    d_remitGender_length = tieupViewDetail.d_remitGender_length,
                    d_remitCivilStat_start_column = tieupViewDetail.d_remitCivilStat_start_column,
                    d_remitCivilStat_length = tieupViewDetail.d_remitCivilStat_length,
                    d_remitPOBox_start_column = tieupViewDetail.d_remitPOBox_start_column,
                    d_remitPOBox_length = tieupViewDetail.d_remitPOBox_length,
                    d_remitMobPhoneNum_start_column = tieupViewDetail.d_remitMobPhoneNum_start_column,
                    d_remitMobPhoneNum_length = tieupViewDetail.d_remitMobPhoneNum_length,
                    d_remitOfficePhoneNo_start_column = tieupViewDetail.d_remitOfficePhoneNo_start_column,
                    d_remitOfficePhoneNo_length = tieupViewDetail.d_remitOfficePhoneNo_length,
                    d_remitEmailAdd_start_column = tieupViewDetail.d_remitEmailAdd_start_column,
                    d_remitEmailAdd_length = tieupViewDetail.d_remitEmailAdd_length,
                    d_remitTaxIDNo_start_column = tieupViewDetail.d_remitTaxIDNo_start_column,
                    d_remitTaxIDNo_length = tieupViewDetail.d_remitTaxIDNo_length,
                    d_remitIDType1_start_column = tieupViewDetail.d_remitIDType1_start_column,
                    d_remitIDType1_length = tieupViewDetail.d_remitIDType1_length,
                    d_remitIDNum1_start_column = tieupViewDetail.d_remitIDNum1_start_column,
                    d_remitIDNum1_length = tieupViewDetail.d_remitIDNum1_length,
                    d_remitIDIssueAt1_start_column = tieupViewDetail.d_remitIDIssueAt1_start_column,
                    d_remitIDIssueAt1_length = tieupViewDetail.d_remitIDIssueAt1_length,
                    d_remitIDExpDate1_start_column = tieupViewDetail.d_remitIDExpDate1_start_column,
                    d_remitIDExpDate1_length = tieupViewDetail.d_remitIDExpDate1_length,
                    d_remitIDType2_start_column = tieupViewDetail.d_remitIDType2_start_column,
                    d_remitIDType2_length = tieupViewDetail.d_remitIDType2_length,
                    d_remitIDNo2_start_column = tieupViewDetail.d_remitIDNo2_start_column,
                    d_remitIDNo2_length = tieupViewDetail.d_remitIDNo2_length,
                    d_remitIDIssueAt2_start_column = tieupViewDetail.d_remitIDIssueAt2_start_column,
                    d_remitIDIssueAt2_length = tieupViewDetail.d_remitIDIssueAt2_length,
                    d_remitIDExpDate2_start_column = tieupViewDetail.d_remitIDExpDate2_start_column,
                    d_remitIDExpDate2_length = tieupViewDetail.d_remitIDExpDate2_length,
                    d_remitNotifType_start_column = tieupViewDetail.d_remitNotifType_start_column,
                    d_remitNotifType_length = tieupViewDetail.d_remitNotifType_length,
                    d_beneficiaryID_start_column = tieupViewDetail.d_beneficiaryID_start_column,
                    d_beneficiaryID_length = tieupViewDetail.d_beneficiaryID_length,
                    d_beneficiaryFName_start_column = tieupViewDetail.d_beneficiaryFName_start_column,
                    d_beneficiaryFName_length = tieupViewDetail.d_beneficiaryFName_length,
                    d_beneficiaryMName_start_column = tieupViewDetail.d_beneficiaryMName_start_column,
                    d_beneficiaryMName_length = tieupViewDetail.d_beneficiaryMName_length,
                    d_beneficiaryLName_start_column = tieupViewDetail.d_beneficiaryLName_start_column,
                    d_beneficiaryLName_length = tieupViewDetail.d_beneficiaryLName_length,
                    d_beneficiaryCusType_start_column = tieupViewDetail.d_beneficiaryCusType_start_column,
                    d_beneficiaryCusType_length = tieupViewDetail.d_beneficiaryCusType_length,
                    d_beneficiaryAdd1_start_column = tieupViewDetail.d_beneficiaryAdd1_start_column,
                    d_beneficiaryAdd1_length = tieupViewDetail.d_beneficiaryAdd1_length,
                    d_beneficiaryAdd2_start_column = tieupViewDetail.d_beneficiaryAdd2_start_column,
                    d_beneficiaryAdd2_length = tieupViewDetail.d_beneficiaryAdd2_length,
                    d_beneficiaryAdd3_start_column = tieupViewDetail.d_beneficiaryAdd3_start_column,
                    d_beneficiaryAdd3_length = tieupViewDetail.d_beneficiaryAdd3_length,
                    d_beneficiaryCountry_start_column = tieupViewDetail.d_beneficiaryCountry_start_column,
                    d_beneficiaryCountry_length = tieupViewDetail.d_beneficiaryCountry_length,
                    d_zipCode_start_column = tieupViewDetail.d_zipCode_start_column,
                    d_zipCode_length = tieupViewDetail.d_zipCode_length,
                    d_landMark_start_column = tieupViewDetail.d_landMark_start_column,
                    d_landMark_length = tieupViewDetail.d_landMark_length,
                    d_beneficiaryNationality_start_column = tieupViewDetail.d_beneficiaryNationality_start_column,
                    d_beneficiaryNationality_length = tieupViewDetail.d_beneficiaryNationality_length,
                    d_beneficiaryBDate_start_column = tieupViewDetail.d_beneficiaryBDate_start_column,
                    d_beneficiaryBDate_length = tieupViewDetail.d_beneficiaryBDate_length,
                    d_beneficiaryProf_start_column = tieupViewDetail.d_beneficiaryProf_start_column,
                    d_beneficiaryProf_length = tieupViewDetail.d_beneficiaryProf_length,
                    d_beneficiaryGender_start_column = tieupViewDetail.d_beneficiaryGender_start_column,
                    d_beneficiaryGender_length = tieupViewDetail.d_beneficiaryGender_length,
                    d_beneficiaryCivilStat_start_column = tieupViewDetail.d_beneficiaryCivilStat_start_column,
                    d_beneficiaryCivilStat_length = tieupViewDetail.d_beneficiaryCivilStat_length,
                    d_beneficiaryPOBox_start_column = tieupViewDetail.d_beneficiaryPOBox_start_column,
                    d_beneficiaryPOBox_length = tieupViewDetail.d_beneficiaryPOBox_length,
                    d_beneficiaryRel_tothe_remit_start_column = tieupViewDetail.d_beneficiaryRel_tothe_remit_start_column,
                    d_beneficiaryRel_tothe_remit_length = tieupViewDetail.d_beneficiaryRel_tothe_remit_length,
                    d_alterRecipient_reltobeneficiary_start_column = tieupViewDetail.d_alterRecipient_reltobeneficiary_start_column,
                    d_alterRecipient_reltobeneficiary_length = tieupViewDetail.d_alterRecipient_reltobeneficiary_length,
                    d_beneficiaryMobPhoneNo_start_column = tieupViewDetail.d_beneficiaryMobPhoneNo_start_column,
                    d_beneficiaryMobPhoneNo_length = tieupViewDetail.d_beneficiaryMobPhoneNo_length,
                    d_beneficiaryOfficePhoneNo_start_column = tieupViewDetail.d_beneficiaryOfficePhoneNo_start_column,
                    d_beneficiaryOfficePhoneNo_length = tieupViewDetail.d_beneficiaryOfficePhoneNo_length,
                    d_beneficiaryEmailAdd_start_column = tieupViewDetail.d_beneficiaryEmailAdd_start_column,
                    d_beneficiaryEmailAdd_length = tieupViewDetail.d_beneficiaryEmailAdd_length,
                    d_beneficiaryTaxIDNo_start_column = tieupViewDetail.d_beneficiaryTaxIDNo_start_column,
                    d_beneficiaryTaxIDNo_length = tieupViewDetail.d_beneficiaryTaxIDNo_length,
                    d_beneficiaryNotifType_start_column = tieupViewDetail.d_beneficiaryNotifType_start_column,
                    d_beneficiaryNotifType_length = tieupViewDetail.d_beneficiaryNotifType_length,
                    d_fundCurrency_start_column = tieupViewDetail.d_fundCurrency_start_column,
                    d_fundCurrency_length = tieupViewDetail.d_fundCurrency_length,
                    d_fundAmount_start_column = tieupViewDetail.d_fundAmount_start_column,
                    d_fundAmount_length = tieupViewDetail.d_fundAmount_length,
                    d_buyingRate_start_column = tieupViewDetail.d_buyingRate_start_column,
                    d_buyingRate_length = tieupViewDetail.d_buyingRate_length,
                    d_settlementCurrency_start_column = tieupViewDetail.d_settlementCurrency_start_column,
                    d_settlementCurrency_length = tieupViewDetail.d_settlementCurrency_length,
                    d_settlementAmount_start_column = tieupViewDetail.d_settlementAmount_start_column,
                    d_settlementAmount_length = tieupViewDetail.d_settlementAmount_length,
                    d_settlementMode_start_column = tieupViewDetail.d_settlementMode_start_column,
                    d_settlementMode_length = tieupViewDetail.d_settlementMode_length,
                    d_bankCode_start_column = tieupViewDetail.d_bankCode_start_column,
                    d_bankCode_length = tieupViewDetail.d_bankCode_length,
                    d_bankName_start_column = tieupViewDetail.d_bankName_start_column,
                    d_bankName_length = tieupViewDetail.d_bankName_length,
                    d_branchCode_start_column = tieupViewDetail.d_branchCode_start_column,
                    d_branchCode_length = tieupViewDetail.d_branchCode_length,
                    d_branchName_start_column = tieupViewDetail.d_branchName_start_column,
                    d_branchName_length = tieupViewDetail.d_branchName_length,
                    d_accountType_start_column = tieupViewDetail.d_accountType_start_column,
                    d_accountType_length = tieupViewDetail.d_accountType_length,
                    d_accountNo_start_column = tieupViewDetail.d_accountNo_start_column,
                    d_accountNo_length = tieupViewDetail.d_accountNo_length,
                    d_goldCardNo_start_column = tieupViewDetail.d_goldCardNo_start_column,
                    d_goldCardNo_length = tieupViewDetail.d_goldCardNo_length,
                    d_billsPayField1_start_column = tieupViewDetail.d_billsPayField1_start_column,
                    d_billsPayField1_length = tieupViewDetail.d_billsPayField1_length,
                    d_billsPayField2_start_column = tieupViewDetail.d_billsPayField2_start_column,
                    d_billsPayField2_length = tieupViewDetail.d_billsPayField2_length,
                    d_billsPayField3_start_column = tieupViewDetail.d_billsPayField3_start_column,
                    d_billsPayField3_length = tieupViewDetail.d_billsPayField3_length,
                    d_billsPayField4_start_column = tieupViewDetail.d_billsPayField4_start_column,
                    d_billsPayField4_length = tieupViewDetail.d_billsPayField4_length,
                    d_billsPayField5_start_column = tieupViewDetail.d_billsPayField5_start_column,
                    d_billsPayField5_length = tieupViewDetail.d_billsPayField5_length,
                    d_outletCode_start_column = tieupViewDetail.d_outletCode_start_column,
                    d_outletCode_length = tieupViewDetail.d_outletCode_length,
                    d_outletBranchCode_start_column = tieupViewDetail.d_outletBranchCode_start_column,
                    d_outletBranchCode_length = tieupViewDetail.d_outletBranchCode_length,
                    d_alterBeneficiaryName_start_column = tieupViewDetail.d_alterBeneficiaryName_start_column,
                    d_alterBeneficiaryName_length = tieupViewDetail.d_alterBeneficiaryName_length,
                    d_alterBeneficiary_relto_beneficiary_start_column = tieupViewDetail.d_alterBeneficiary_relto_beneficiary_start_column,
                    d_alterBeneficiary_relto_beneficiary_length = tieupViewDetail.d_alterBeneficiary_relto_beneficiary_length,
                    d_messageToBeneficiary_start_column = tieupViewDetail.d_messageToBeneficiary_start_column,
                    d_messageToBeneficiary_length = tieupViewDetail.d_messageToBeneficiary_length,
                    d_receiverCorresBank_start_column = tieupViewDetail.d_receiverCorresBank_start_column,
                    d_receiverCorresBank_length = tieupViewDetail.d_receiverCorresBank_length,
                    d_senderCorresBank_start_column = tieupViewDetail.d_senderCorresBank_start_column,
                    d_senderCorresBank_length = tieupViewDetail.d_senderCorresBank_length,
                    d_sendingBank_start_column = tieupViewDetail.d_sendingBank_start_column,
                    d_sendingBank_length = tieupViewDetail.d_sendingBank_length,
                    d_receivingBank_start_column = tieupViewDetail.d_receivingBank_start_column,
                    d_receivingBank_length = tieupViewDetail.d_receivingBank_length,
                    d_modeOfChange_start_column = tieupViewDetail.d_modeOfChange_start_column,
                    d_modeOfChange_length = tieupViewDetail.d_modeOfChange_length,
                    d_purposeCode_start_column = tieupViewDetail.d_purposeCode_start_column,
                    d_purposeCode_length = tieupViewDetail.d_purposeCode_length,
                    d_indvCode_start_column = tieupViewDetail.d_indvCode_start_column,
                    d_indvCode_length = tieupViewDetail.d_indvCode_length,
                    //d_detailHash_start_column = tieupViewDetail.d_detailHash_start_column,
                    //d_detailHash_length = tieupViewDetail.d_detailHash_length,

                    //---- Default Data ----- 
                    applicationNumber = tieupViewDetail.applicationNumber,
                    settlementMode = tieupViewDetail.settlementMode,
                    fundingAmount = tieupViewDetail.fundingAmount,
                    fundingCurrency = tieupViewDetail.fundingCurrency,
                    remitterFirstName = tieupViewDetail.remitterFirstName,
                    beneficiaryFirstName = tieupViewDetail.beneficiaryFirstName,
                    accountNumber = tieupViewDetail.accountNumber,
                    bankCode = tieupViewDetail.bankCode,
                    beneficiaryAddress1 = tieupViewDetail.beneficiaryAddress1,
                    remitterNationality = tieupViewDetail.remitterNationality,
                    remitterAddress1 = tieupViewDetail.remitterAddress1,
                    //remitterCountry = tieupViewDetail.remitterCountry,
                    //remitterZipCode = tieupViewDetail.remitterZipCode,
                    remitterBirthDate = tieupViewDetail.remitterBirthDate,
                    remitterMobileNumber = tieupViewDetail.remitterMobileNumber,
                    remitterIdType1 = tieupViewDetail.remitterIdType1,
                    remitterIdNumber1 = tieupViewDetail.remitterIdNumber1,
                    remitterIdIssuedAt1 = tieupViewDetail.remitterIdIssuedAt1,
                    remitterIdExpiry1 = tieupViewDetail.remitterIdExpiry1,
                    beneficiaryMiddleName = tieupViewDetail.beneficiaryMiddleName,
                    beneficiaryLastName = tieupViewDetail.beneficiaryLastName,
                    beneficiaryCustomerType = tieupViewDetail.beneficiaryCustomerType,
                    beneficiaryZipCode = tieupViewDetail.beneficiaryZipCode,
                    beneficiaryNationality = tieupViewDetail.beneficiaryNationality,
                    beneficiaryBirthDate = tieupViewDetail.beneficiaryBirthDate,
                    beneficiaryRelationToRemitter = tieupViewDetail.beneficiaryRelationToRemitter,
                    beneficiaryMobileNumber = tieupViewDetail.beneficiaryMobileNumber,
                    buyingRate = tieupViewDetail.buyingRate,
                    settlementCurrency = tieupViewDetail.settlementCurrency == "FunCur" ? "Funding Currency" : tieupViewDetail.settlementCurrency,
                    paymentField1 = tieupViewDetail.paymentField1,
                    paymentField2 = tieupViewDetail.paymentField2,
                    paymentField3 = tieupViewDetail.paymentField3,
                    outletCode = tieupViewDetail.outletCode,
                    //senderCorrespondentBank = tieupViewDetail.senderCorrespondentBank,
                    //sendingBank = tieupViewDetail.sendingBank,
                    //receivingBank = tieupViewDetail.receivingBank,
                    //modeOfCharge = tieupViewDetail.modeOfCharge,
                    //purposeCode = tieupViewDetail.purposeCode,
                    //individualCode = tieupViewDetail.individualCode,
                    //natureOfBusiness = tieupViewDetail.natureOfBusiness,
                    remitterId = tieupViewDetail.remitterId,
                    remitterMiddleName = tieupViewDetail.remitterMiddleName,
                    remitterLastName = tieupViewDetail.remitterLastName,
                    remitterCustomerType = tieupViewDetail.remitterCustomerType,
                    settlementAmount = tieupViewDetail.settlementAmount,
                    remitterAddress2 = tieupViewDetail.remitterAddress2,
                    remitterAddress3 = tieupViewDetail.remitterAddress3,
                    remitterAddress4 = tieupViewDetail.remitterAddress4,
                    //remitterContinent = tieupViewDetail.remitterContinent,
                    //remitterProfession = tieupViewDetail.remitterProfession,
                    //remitterGender = tieupViewDetail.remitterGender,
                    //remitterCivilStatus = tieupViewDetail.remitterCivilStatus,
                    //remitterPoBox = tieupViewDetail.remitterPoBox,
                    //remitterOfficeNumber = tieupViewDetail.remitterOfficeNumber,
                    //remitterEmail = tieupViewDetail.remitterEmail,
                    //remitterTin = tieupViewDetail.remitterTin,
                    //remitterIdType2 = tieupViewDetail.remitterIdType2,
                    //remitterIdNumber2 = tieupViewDetail.remitterIdNumber2,
                    //remitterIdIssuedAt2 = tieupViewDetail.remitterIdIssuedAt2,
                    //remitterIdExpiry2 = tieupViewDetail.remitterIdExpiry2,
                    //remitterNotificationType = tieupViewDetail.remitterNotificationType,
                    //beneficiaryId = tieupViewDetail.beneficiaryId,
                    beneficiaryAddress2 = tieupViewDetail.beneficiaryAddress2,
                    beneficiaryAddress3 = tieupViewDetail.beneficiaryAddress3,
                    beneficiaryCountry = tieupViewDetail.beneficiaryCountry,
                    //beneficiaryLandmark = tieupViewDetail.beneficiaryLandmark,
                    //beneficiaryProfession = tieupViewDetail.beneficiaryProfession,
                    //beneficiaryGender = tieupViewDetail.beneficiaryGender,
                    //beneficiaryCivilStatus = tieupViewDetail.beneficiaryCivilStatus,
                    //beneficiaryPoBox = tieupViewDetail.beneficiaryPoBox,
                    //alternateRecipientRelationToBeneficiary = tieupViewDetail.alternateRecipientRelationToBeneficiary,
                    //beneficiaryOfficeNumber = tieupViewDetail.beneficiaryOfficeNumber,
                    beneficiaryEmail = tieupViewDetail.beneficiaryEmail,
                    //beneficiaryTin = tieupViewDetail.beneficiaryTin,
                    //beneficiaryNotificationType = tieupViewDetail.beneficiaryNotificationType,
                    transactionDate = tieupViewDetail.transactionDate,
                    bankName = tieupViewDetail.bankName,
                    //branchName = tieupViewDetail.branchName,
                    //accountType = tieupViewDetail.accountType,
                    //goldCardNumber = tieupViewDetail.goldCardNumber,
                    paymentField4 = tieupViewDetail.paymentField4,
                    paymentField5 = tieupViewDetail.paymentField5,
                    outletBranchCode = tieupViewDetail.outletBranchCode,
                    //alternateBeneficiary = tieupViewDetail.alternateBeneficiary,
                    //alternateBeneficiaryRelationToBeneficiary = tieupViewDetail.alternateBeneficiaryRelationToBeneficiary,
                    //messageToBeneficiary = tieupViewDetail.messageToBeneficiary,
                    //receiverCorrespondentBank = tieupViewDetail.receiverCorrespondentBank,
                    branchCode = tieupViewDetail.branchCode,

                    //New Format Mapper 2.0 added
                    //PartnerCodeStartColumn = tieupViewDetail.partnerCode_start_column,
                    //PartnerCodeLength = tieupViewDetail.partnerCode_length,
                    //PartnerCodeDefault = tieupViewDetail.partnerCode_default,
                    AcctNameStartColumn = tieupViewDetail.acctName_start_column,
                    AcctNameLength = tieupViewDetail.acctName_length,
                    AcctNameDefault = tieupViewDetail.acctName_default,
                    RemCountryStartColumn = tieupViewDetail.remCountry_start_column,
                    RemCountryLength = tieupViewDetail.remCountry_length,
                    RemCountryDefault = tieupViewDetail.remCountry_default,
                    RemFourthNmeStartColumn = tieupViewDetail.remFourthNme_start_column,
                    RemFourthNmeLength = tieupViewDetail.remFourthNme_length,
                    RemFourthNmeDefault = tieupViewDetail.remFourthNme_default,
                    RemPlaceBrthStartColumn = tieupViewDetail.remPlaceBrth_start_column,
                    RemPlaceBrthLength = tieupViewDetail.remPlaceBrth_length,
                    RemPlaceBrthDefault = tieupViewDetail.remPlaceBrth_default,
                    RemAcctNumStartColumn = tieupViewDetail.remAcctNum_start_column,
                    RemAcctNumLength = tieupViewDetail.remAcctNum_length,
                    RemAcctNumDefault = tieupViewDetail.remAcctNum_default,
                    RemIBANStartColumn = tieupViewDetail.remIBAN_start_column,
                    RemIBANLength = tieupViewDetail.remIBAN_length,
                    RemIBANDefault = tieupViewDetail.remIBAN_default,
                    Res1StartColumn = tieupViewDetail.res1_start_column,
                    Res1Length = tieupViewDetail.res1_length,
                    Res1Default = tieupViewDetail.res1_default,
                    Res2StartColumn = tieupViewDetail.res2_start_column,
                    Res2Length = tieupViewDetail.res2_length,
                    Res2Default = tieupViewDetail.res2_default,
                    Res3StartColumn = tieupViewDetail.res3_start_column,
                    Res3Length = tieupViewDetail.res3_length,
                    Res3Default = tieupViewDetail.res3_default,
                    Res4StartColumn = tieupViewDetail.res4_start_column,
                    Res4Length = tieupViewDetail.res4_length,
                    Res4Default = tieupViewDetail.res4_default,
                    Res5StartColumn = tieupViewDetail.res5_start_column,
                    Res5Length = tieupViewDetail.res5_length,
                    Res5Default = tieupViewDetail.res5_default,
                    Res6StartColumn = tieupViewDetail.res6_start_column,
                    Res6Length = tieupViewDetail.res6_length,
                    Res6Default = tieupViewDetail.res6_default,
                    Res7StartColumn = tieupViewDetail.res7_start_column,
                    Res7Length = tieupViewDetail.res7_length,
                    Res7Default = tieupViewDetail.res7_default,
                    Res8StartColumn = tieupViewDetail.res8_start_column,
                    Res8Length = tieupViewDetail.res8_length,
                    Res8Default = tieupViewDetail.res8_default,
                    Res9StartColumn = tieupViewDetail.res9_start_column,
                    Res9Length = tieupViewDetail.res9_length,
                    Res9Default = tieupViewDetail.res9_default,
                    Res10StartColumn = tieupViewDetail.res10_start_column,
                    Res10Length = tieupViewDetail.res10_length,
                    Res10Default = tieupViewDetail.res10_default,
                    SourceOfRemStartColumn = tieupViewDetail.sourceOfRem_start_column,
                    SourceOfRemLength = tieupViewDetail.sourceOfRem_length,
                    SourceOfRemDefault = tieupViewDetail.sourceOfRem_default,
                    NatrueOfBussStartColumn = tieupViewDetail.natrueOfBuss_start_column,
                    NatrueOfBussLength = tieupViewDetail.natrueOfBuss_length,
                    NatrueOfBussDefault = tieupViewDetail.natrueOfBuss_default,
                    PurposeOfRemStartColumn = tieupViewDetail.purposeOfRem_start_column,
                    PurposeOfRemLength = tieupViewDetail.purposeOfRem_length,
                    PurposeOfRemDefault = tieupViewDetail.purposeOfRem_default,

                };

                formViewModel.TieupViewDetailInputs = tieupDetail;
                formViewModel.TieupViewMaintInputs = childDetail;
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.ToString(), ex);

                ViewBag.Status = -1;
                ViewBag.ErrorMessage = "An error encountered while processing request.";

                return View();
            }

            return View(formViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MaintUpdateFormatMapper(FormCollection fc, string id)
        {

            var valid = 0;
            var newStat = "";

            try
            {
                if (ModelState.IsValid)
                {
                    if (fc["req"] == "Save")
                    {
                        if (fc["status"] == "0")
                        {
                            newStat = "1"; //edit
                            valid = 1; // create record to request tbl
                        }
                    }
                    else if (fc["req"] == "Approve")
                    {
                        if (fc["status"] != "0")
                        {
                            newStat = "0"; //edit
                            valid = 2; // create record to request tbl
                        }
                    }
                    else if (fc["req"] == "Reject")
                    {
                        if (fc["status"] != "0")
                        {
                            valid = 3; // create record to request tbl
                        }
                    }

                    DARS.Web.Models.FormatMapperFormViewModel formViewModel1 = new DARS.Web.Models.FormatMapperFormViewModel();
                    formViewModel1.Action = 2;


                    var tprequestApprove = new VREMIT.ModelMVC.Tieup_Codes();
                    tprequestApprove.pk_tieup_code_id = id;

                    var tprequest = new VREMIT.ModelMVC.Tieup_Codes_Request();
                    tprequest.pk_tieup_code_id = fc["TieupViewMaintInputs.tieup_code_id"];

                    // valid 1 -  create new to tbl_User_Request from Edit Request
                    if (valid == 1)
                    {
                        VREMIT.ModelMVC.Tieup_Codes_Detail m = new VREMIT.ModelMVC.Tieup_Codes_Detail();
                        m.pk_detail_tieup_id = id;
                        m = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes_Detail>(dal.GetGenericDetails(m, "Tieup_Codes_Detail", g_global_region_code));
                        VREMIT.ModelMVC.Tieup_Codes n = new VREMIT.ModelMVC.Tieup_Codes();
                        n.pk_tieup_code_id = id;
                        n = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes>(dal.GetGenericDetails(n, "Tieup_Codes", g_global_region_code));


                        //ViewBag.HasHCode_tieup_code = TempData["HasHCode_tieup_code"];
                        //ViewBag.HasHCode_detail = TempData["HasHCode_detail"];

                        //TempData.Keep("HasHCode_tieup_code");
                        //TempData.Keep("HasHCode_detail");

                        //Debug.WriteLine("HasHCode_tieup_code: " + TempData["HasHCode_tieup_code"]);
                        //Debug.WriteLine("HasHCode_detail: " + TempData["HasHCode_detail"]);

                        var noChanges = fc["TieupViewMaintInputs.Action"];
                        if (noChanges == "0")
                        {
                            TempData["title"] = "Unable to Update!";
                            TempData["text"] = "No changes were made.";
                            TempData["icon"] = "error";
                            return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                        }

                        string tranID = Common.GetgenerateID(1, Convert.ToString(ModelViewer.TIEUP_FILE_MAPPER), g_global_region_code); //Bank Code TIEUP_FILE_MAPPER

                        var countryReq = new VREMIT.ModelMVC.Tieup_Codes_Request
                        {
                            pk_tieup_code_id = tranID,
                            tieup_codes = fc["TieupViewMaintInputs.tieup_codes"],
                            remarks = fc["TieupViewMaintInputs.remarks"],
                            status = newStat,
                            filename_validation = fc["TieupViewMaintInputs.filenameV"],
                            footer_validation = fc["TieupViewMaintInputs.footerV"],
                            //settlement_currency = fc["TieupViewMaintInputs.settlement_currency"] == "Funding Currency" ? "FunCur" : fc["TieupViewMaintInputs.settlement_currency"],
                            delimiter_val = fc["TieupViewDetailInputs.delimiter_val"],
                            username = _currentUser.Username,

                            rt_action = ModelViewer.USER_ACTION_MODIFY,
                            rt_terminal_id = base._userClientIP,
                            rt_user_agent = base._userAgent,
                            rt_username = _currentUser.Username,

                            provided = fc["TieupViewDetailInputs.Provided"],
                            dt_last_chg = DateTime.Now,
                            req_type = ModelViewer.REQ_TYPE_MODIFY,
                            req_status = ModelViewer.REQ_STATUS_PENDING,
                            start_record = fc["TieupViewDetailInputs.detail_start_record"],
                            file_type = fc["TieupViewMaintInputs.file_type"],
                            d_prefix = fc["TieupViewDetailInputs.d_prefix"],
                            delimiter_type = fc["TieupViewDetailInputs.delimiter_type"],
                            delimiter = fc["TieupViewDetailInputs.delimiter"],
                            detail_start_record = fc["TieupViewDetailInputs.detail_start_record"],
                            detail_transdate_start_column = fc["TieupViewDetailInputs.detail_transdate_start_column"],
                            detail_transdate_length = fc["TieupViewDetailInputs.detail_transdate_length"],
                            detail_application_start_column = fc["TieupViewDetailInputs.detail_application_start_column"],
                            detail_application_length = fc["TieupViewDetailInputs.detail_application_length"],
                            detail_remitid_start_column = fc["TieupViewDetailInputs.detail_remitid_start_column"],
                            detail_remitid_length = fc["TieupViewDetailInputs.detail_remitid_length"],
                            detail_remitfname_start_column = fc["TieupViewDetailInputs.detail_remitfname_start_column"],
                            detail_remitfname_length = fc["TieupViewDetailInputs.detail_remitfname_length"],
                            detail_remitmidname_start_column = fc["TieupViewDetailInputs.detail_remitmidname_start_column"],
                            detail_remitmidname_length = fc["TieupViewDetailInputs.detail_remitmidname_length"],
                            detail_remitlastname_start_column = fc["TieupViewDetailInputs.detail_remitlastname_start_column"],
                            detail_remitlastname_length = fc["TieupViewDetailInputs.detail_remitlastname_length"],
                            d_remitCusType_start_column = fc["TieupViewDetailInputs.d_remitCusType_start_column"],//
                            d_remitCusType_length = fc["TieupViewDetailInputs.d_remitCusType_length"],
                            d_remitNationality_start_column = fc["TieupViewDetailInputs.d_remitNationality_start_column"],//
                            d_remitNationality_length = fc["TieupViewDetailInputs.d_remitNationality_length"],
                            d_remitAdd1_start_column = fc["TieupViewDetailInputs.d_remitAdd1_start_column"],
                            d_remitAdd1_length = fc["TieupViewDetailInputs.d_remitAdd1_length"],
                            d_remitAdd2_start_column = fc["TieupViewDetailInputs.d_remitAdd2_start_column"],
                            d_remitAdd2_length = fc["TieupViewDetailInputs.d_remitAdd2_length"],
                            d_remitAdd3_start_column = fc["TieupViewDetailInputs.d_remitAdd3_start_column"],
                            d_remitAdd3_length = fc["TieupViewDetailInputs.d_remitAdd3_length"],
                            d_remitAdd4_start_column = fc["TieupViewDetailInputs.d_remitAdd4_start_column"],
                            d_remitAdd4_length = fc["TieupViewDetailInputs.d_remitAdd4_length"],
                            d_remitContinent_start_column = fc["TieupViewDetailInputs.d_remitContinent_start_column"],
                            d_remitContinent_length = fc["TieupViewDetailInputs.d_remitContinent_length"],
                            d_remitZipCode_start_column = fc["TieupViewDetailInputs.d_remitZipCode_start_column"],
                            d_remitZipCode_length = fc["TieupViewDetailInputs.d_remitZipCode_length"],
                            d_remitBDate_start_column = fc["TieupViewDetailInputs.d_remitBDate_start_column"],
                            d_remitBDate_length = fc["TieupViewDetailInputs.d_remitBDate_length"],
                            d_remitProf_start_column = fc["TieupViewDetailInputs.d_remitProf_start_column"],
                            d_remitProf_length = fc["TieupViewDetailInputs.d_remitProf_length"],
                            d_remitGender_start_column = fc["TieupViewDetailInputs.d_remitGender_start_column"],
                            d_remitGender_length = fc["TieupViewDetailInputs.d_remitGender_length"],//
                            d_remitCivilStat_start_column = fc["TieupViewDetailInputs.d_remitCivilStat_start_column"],
                            d_remitCivilStat_length = fc["TieupViewDetailInputs.d_remitCivilStat_length"],
                            d_remitPOBox_start_column = fc["TieupViewDetailInputs.d_remitPOBox_start_column"],
                            d_remitPOBox_length = fc["TieupViewDetailInputs.d_remitPOBox_length"],//
                            d_remitMobPhoneNum_start_column = fc["TieupViewDetailInputs.d_remitMobPhoneNum_start_column"],
                            d_remitMobPhoneNum_length = fc["TieupViewDetailInputs.d_remitMobPhoneNum_length"],
                            d_remitOfficePhoneNo_start_column = fc["TieupViewDetailInputs.d_remitOfficePhoneNo_start_column"],
                            d_remitOfficePhoneNo_length = fc["TieupViewDetailInputs.d_remitOfficePhoneNo_length"],
                            d_remitEmailAdd_start_column = fc["TieupViewDetailInputs.d_remitEmailAdd_start_column"],
                            d_remitEmailAdd_length = fc["TieupViewDetailInputs.d_remitEmailAdd_length"],
                            d_remitTaxIDNo_start_column = fc["TieupViewDetailInputs.d_remitTaxIDNo_start_column"],
                            d_remitTaxIDNo_length = fc["TieupViewDetailInputs.d_remitTaxIDNo_length"],
                            d_remitIDType1_start_column = fc["TieupViewDetailInputs.d_remitIDType1_start_column"],
                            d_remitIDType1_length = fc["TieupViewDetailInputs.d_remitIDType1_length"],
                            d_remitIDNum1_start_column = fc["TieupViewDetailInputs.d_remitIDNum1_start_column"],
                            d_remitIDNum1_length = fc["TieupViewDetailInputs.d_remitIDNum1_length"],
                            d_remitIDIssueAt1_start_column = fc["TieupViewDetailInputs.d_remitIDIssueAt1_start_column"],
                            d_remitIDIssueAt1_length = fc["TieupViewDetailInputs.d_remitIDIssueAt1_length"],
                            d_remitIDExpDate1_start_column = fc["TieupViewDetailInputs.d_remitIDExpDate1_start_column"],
                            d_remitIDExpDate1_length = fc["TieupViewDetailInputs.d_remitIDExpDate1_length"],
                            d_remitIDType2_start_column = fc["TieupViewDetailInputs.d_remitIDType2_start_column"],
                            d_remitIDType2_length = fc["TieupViewDetailInputs.d_remitIDType2_length"],
                            d_remitIDNo2_start_column = fc["TieupViewDetailInputs.d_remitIDNo2_start_column"],
                            d_remitIDNo2_length = fc["TieupViewDetailInputs.d_remitIDNo2_length"],
                            d_remitIDIssueAt2_start_column = fc["TieupViewDetailInputs.d_remitIDIssueAt2_start_column"],
                            d_remitIDIssueAt2_length = fc["TieupViewDetailInputs.d_remitIDIssueAt2_length"],
                            d_remitIDExpDate2_start_column = fc["TieupViewDetailInputs.d_remitIDExpDate2_start_column"],
                            d_remitIDExpDate2_length = fc["TieupViewDetailInputs.d_remitIDExpDate2_length"],
                            d_remitNotifType_start_column = fc["TieupViewDetailInputs.d_remitNotifType_start_column"],
                            d_remitNotifType_length = fc["TieupViewDetailInputs.d_remitNotifType_length"],
                            d_beneficiaryID_start_column = fc["TieupViewDetailInputs.d_beneficiaryID_start_column"],
                            d_beneficiaryID_length = fc["TieupViewDetailInputs.d_beneficiaryID_length"],
                            d_beneficiaryFName_start_column = fc["TieupViewDetailInputs.d_beneficiaryFName_start_column"],
                            d_beneficiaryFName_length = fc["TieupViewDetailInputs.d_beneficiaryFName_length"],
                            d_beneficiaryMName_start_column = fc["TieupViewDetailInputs.d_beneficiaryMName_start_column"],
                            d_beneficiaryMName_length = fc["TieupViewDetailInputs.d_beneficiaryMName_length"],
                            d_beneficiaryLName_start_column = fc["TieupViewDetailInputs.d_beneficiaryLName_start_column"],
                            d_beneficiaryLName_length = fc["TieupViewDetailInputs.d_beneficiaryLName_length"],
                            d_beneficiaryCusType_start_column = fc["TieupViewDetailInputs.d_beneficiaryCusType_start_column"],
                            d_beneficiaryCusType_length = fc["TieupViewDetailInputs.d_beneficiaryCusType_length"],
                            d_beneficiaryAdd1_start_column = fc["TieupViewDetailInputs.d_beneficiaryAdd1_start_column"],
                            d_beneficiaryAdd1_length = fc["TieupViewDetailInputs.d_beneficiaryAdd1_length"],//
                            d_beneficiaryAdd2_start_column = fc["TieupViewDetailInputs.d_beneficiaryAdd2_start_column"],
                            d_beneficiaryAdd2_length = fc["TieupViewDetailInputs.d_beneficiaryAdd2_length"],//
                            d_beneficiaryAdd3_start_column = fc["TieupViewDetailInputs.d_beneficiaryAdd3_start_column"],
                            d_beneficiaryAdd3_length = fc["TieupViewDetailInputs.d_beneficiaryAdd3_length"],//
                            d_beneficiaryCountry_start_column = fc["TieupViewDetailInputs.d_beneficiaryCountry_start_column"],
                            d_beneficiaryCountry_length = fc["TieupViewDetailInputs.d_beneficiaryCountry_length"],
                            d_zipCode_start_column = fc["TieupViewDetailInputs.d_zipCode_start_column"],
                            d_zipCode_length = fc["TieupViewDetailInputs.d_zipCode_length"],
                            d_landMark_start_column = fc["TieupViewDetailInputs.d_landMark_start_column"],
                            d_landMark_length = fc["TieupViewDetailInputs.d_landMark_length"],
                            d_beneficiaryNationality_start_column = fc["TieupViewDetailInputs.d_beneficiaryNationality_start_column"],
                            d_beneficiaryNationality_length = fc["TieupViewDetailInputs.d_beneficiaryNationality_length"],
                            d_beneficiaryBDate_start_column = fc["TieupViewDetailInputs.d_beneficiaryBDate_start_column"],
                            d_beneficiaryBDate_length = fc["TieupViewDetailInputs.d_beneficiaryBDate_length"],
                            d_beneficiaryProf_start_column = fc["TieupViewDetailInputs.d_beneficiaryProf_start_column"],
                            d_beneficiaryProf_length = fc["TieupViewDetailInputs.d_beneficiaryProf_length"],
                            d_beneficiaryGender_start_column = fc["TieupViewDetailInputs.d_beneficiaryGender_start_column"],
                            d_beneficiaryGender_length = fc["TieupViewDetailInputs.d_beneficiaryGender_length"],
                            d_beneficiaryCivilStat_start_column = fc["TieupViewDetailInputs.d_beneficiaryCivilStat_start_column"],
                            d_beneficiaryCivilStat_length = fc["TieupViewDetailInputs.d_beneficiaryCivilStat_length"],
                            d_beneficiaryPOBox_start_column = fc["TieupViewDetailInputs.d_beneficiaryPOBox_start_column"],
                            d_beneficiaryPOBox_length = fc["TieupViewDetailInputs.d_beneficiaryPOBox_length"],
                            d_beneficiaryRel_tothe_remit_start_column = fc["TieupViewDetailInputs.d_beneficiaryRel_tothe_remit_start_column"],
                            d_beneficiaryRel_tothe_remit_length = fc["TieupViewDetailInputs.d_beneficiaryRel_tothe_remit_length"],
                            d_alterRecipient_reltobeneficiary_start_column = fc["TieupViewDetailInputs.d_alterRecipient_reltobeneficiary_start_column"],
                            d_alterRecipient_reltobeneficiary_length = fc["TieupViewDetailInputs.d_alterRecipient_reltobeneficiary_length"],
                            d_beneficiaryMobPhoneNo_start_column = fc["TieupViewDetailInputs.d_beneficiaryMobPhoneNo_start_column"],
                            d_beneficiaryMobPhoneNo_length = fc["TieupViewDetailInputs.d_beneficiaryMobPhoneNo_length"],
                            d_beneficiaryOfficePhoneNo_start_column = fc["TieupViewDetailInputs.d_beneficiaryOfficePhoneNo_start_column"],
                            d_beneficiaryOfficePhoneNo_length = fc["TieupViewDetailInputs.d_beneficiaryOfficePhoneNo_length"],
                            d_beneficiaryEmailAdd_start_column = fc["TieupViewDetailInputs.d_beneficiaryEmailAdd_start_column"],
                            d_beneficiaryEmailAdd_length = fc["TieupViewDetailInputs.d_beneficiaryEmailAdd_length"],//
                            d_beneficiaryTaxIDNo_start_column = fc["TieupViewDetailInputs.d_beneficiaryTaxIDNo_start_column"],
                            d_beneficiaryTaxIDNo_length = fc["TieupViewDetailInputs.d_beneficiaryTaxIDNo_length"],//
                            d_beneficiaryNotifType_start_column = fc["TieupViewDetailInputs.d_beneficiaryNotifType_start_column"],
                            d_beneficiaryNotifType_length = fc["TieupViewDetailInputs.d_beneficiaryNotifType_length"],
                            d_fundCurrency_start_column = fc["TieupViewDetailInputs.d_fundCurrency_start_column"],
                            d_fundCurrency_length = fc["TieupViewDetailInputs.d_fundCurrency_length"],
                            d_fundAmount_start_column = fc["TieupViewDetailInputs.d_fundAmount_start_column"],
                            d_fundAmount_length = fc["TieupViewDetailInputs.d_fundAmount_length"],
                            d_buyingRate_start_column = fc["TieupViewDetailInputs.d_buyingRate_start_column"],
                            d_buyingRate_length = fc["TieupViewDetailInputs.d_buyingRate_length"],
                            d_settlementCurrency_start_column = fc["TieupViewDetailInputs.d_settlementCurrency_start_column"],
                            d_settlementCurrency_length = fc["TieupViewDetailInputs.d_settlementCurrency_length"],
                            d_settlementAmount_start_column = fc["TieupViewDetailInputs.d_settlementAmount_start_column"],
                            d_settlementAmount_length = fc["TieupViewDetailInputs.d_settlementAmount_length"],
                            d_settlementMode_start_column = fc["TieupViewDetailInputs.d_settlementMode_start_column"],
                            d_settlementMode_length = fc["TieupViewDetailInputs.d_settlementMode_length"],
                            d_bankCode_start_column = fc["TieupViewDetailInputs.d_bankCode_start_column"],
                            d_bankCode_length = fc["TieupViewDetailInputs.d_bankCode_length"],
                            d_bankName_start_column = fc["TieupViewDetailInputs.d_bankName_start_column"],
                            d_bankName_length = fc["TieupViewDetailInputs.d_bankName_length"],
                            d_branchCode_start_column = fc["TieupViewDetailInputs.d_branchCode_start_column"],
                            d_branchCode_length = fc["TieupViewDetailInputs.d_branchCode_length"],
                            d_branchName_start_column = fc["TieupViewDetailInputs.d_branchName_start_column"],
                            d_branchName_length = fc["TieupViewDetailInputs.d_branchName_length"],
                            d_accountType_start_column = fc["TieupViewDetailInputs.d_accountType_start_column"],
                            d_accountType_length = fc["TieupViewDetailInputs.d_accountType_length"],
                            d_accountNo_start_column = fc["TieupViewDetailInputs.d_accountNo_start_column"],
                            d_accountNo_length = fc["TieupViewDetailInputs.d_accountNo_length"],//
                            d_goldCardNo_start_column = fc["TieupViewDetailInputs.d_goldCardNo_start_column"],
                            d_goldCardNo_length = fc["TieupViewDetailInputs.d_goldCardNo_length"],
                            d_billsPayField1_start_column = fc["TieupViewDetailInputs.d_billsPayField1_start_column"],
                            d_billsPayField1_length = fc["TieupViewDetailInputs.d_billsPayField1_length"],
                            d_billsPayField2_start_column = fc["TieupViewDetailInputs.d_billsPayField2_start_column"],
                            d_billsPayField2_length = fc["TieupViewDetailInputs.d_billsPayField2_length"],
                            d_billsPayField3_start_column = fc["TieupViewDetailInputs.d_billsPayField3_start_column"],
                            d_billsPayField3_length = fc["TieupViewDetailInputs.d_billsPayField3_length"],
                            d_billsPayField4_start_column = fc["TieupViewDetailInputs.d_billsPayField4_start_column"],
                            d_billsPayField4_length = fc["TieupViewDetailInputs.d_billsPayField4_length"],
                            d_billsPayField5_start_column = fc["TieupViewDetailInputs.d_billsPayField5_start_column"],
                            d_billsPayField5_length = fc["TieupViewDetailInputs.d_billsPayField5_length"],
                            d_outletCode_start_column = fc["TieupViewDetailInputs.d_outletCode_start_column"],
                            d_outletCode_length = fc["TieupViewDetailInputs.d_outletCode_length"],
                            d_outletBranchCode_start_column = fc["TieupViewDetailInputs.d_outletBranchCode_start_column"],
                            d_outletBranchCode_length = fc["TieupViewDetailInputs.d_outletBranchCode_length"],
                            d_alterBeneficiaryName_start_column = fc["TieupViewDetailInputs.d_alterBeneficiaryName_start_column"],
                            d_alterBeneficiaryName_length = fc["TieupViewDetailInputs.d_alterBeneficiaryName_length"],
                            d_alterBeneficiary_relto_beneficiary_start_column = fc["TieupViewDetailInputs.d_alterBeneficiary_relto_beneficiary_start_column"],
                            d_alterBeneficiary_relto_beneficiary_length = fc["TieupViewDetailInputs.d_alterBeneficiary_relto_beneficiary_length"],
                            d_messageToBeneficiary_start_column = fc["TieupViewDetailInputs.d_messageToBeneficiary_start_column"],
                            d_messageToBeneficiary_length = fc["TieupViewDetailInputs.d_messageToBeneficiary_length"],
                            d_receiverCorresBank_start_column = fc["TieupViewDetailInputs.d_receiverCorresBank_start_column"],
                            d_receiverCorresBank_length = fc["TieupViewDetailInputs.d_receiverCorresBank_length"],
                            d_senderCorresBank_start_column = fc["TieupViewDetailInputs.d_senderCorresBank_start_column"],
                            d_senderCorresBank_length = fc["TieupViewDetailInputs.d_senderCorresBank_length"],
                            d_sendingBank_start_column = fc["TieupViewDetailInputs.d_sendingBank_start_column"],
                            d_sendingBank_length = fc["TieupViewDetailInputs.d_sendingBank_length"],
                            d_receivingBank_start_column = fc["TieupViewDetailInputs.d_receivingBank_start_column"],
                            d_receivingBank_length = fc["TieupViewDetailInputs.d_receivingBank_length"],
                            d_modeOfChange_start_column = fc["TieupViewDetailInputs.d_modeOfChange_start_column"],
                            d_modeOfChange_length = fc["TieupViewDetailInputs.d_modeOfChange_length"],
                            d_purposeCode_start_column = fc["TieupViewDetailInputs.d_purposeCode_start_column"],
                            d_purposeCode_length = fc["TieupViewDetailInputs.d_purposeCode_length"],
                            d_indvCode_start_column = fc["TieupViewDetailInputs.d_indvCode_start_column"],
                            d_indvCode_length = fc["TieupViewDetailInputs.d_indvCode_length"],

                            // ---- Default Data ----
                            applicationNumber = fc["TieupViewDetailInputs.applicationNumber"],
                            settlementMode = fc["TieupViewDetailInputs.settlementMode"],
                            fundingAmount = fc["TieupViewDetailInputs.fundingAmount"],
                            fundingCurrency = fc["TieupViewDetailInputs.fundingCurrency"],
                            remitterFirstName = fc["TieupViewDetailInputs.remitterFirstName"],
                            beneficiaryFirstName = fc["TieupViewDetailInputs.beneficiaryFirstName"],
                            accountNumber = fc["TieupViewDetailInputs.accountNumber"],
                            bankCode = fc["TieupViewDetailInputs.bankCode"],
                            beneficiaryAddress1 = fc["TieupViewDetailInputs.beneficiaryAddress1"],
                            remitterNationality = fc["TieupViewDetailInputs.remitterNationality"],
                            remitterAddress1 = fc["TieupViewDetailInputs.remitterAddress1"],
                            //remitterCountry = fc["TieupViewDetailInputs.remitterCountry"],
                            //remitterZipCode = fc["TieupViewDetailInputs.remitterZipCode"],
                            remitterBirthDate = fc["TieupViewDetailInputs.remitterBirthDate"],
                            remitterMobileNumber = fc["TieupViewDetailInputs.remitterMobileNumber"],
                            remitterIdType1 = fc["TieupViewDetailInputs.remitterIdType1"],
                            remitterIdNumber1 = fc["TieupViewDetailInputs.remitterIdNumber1"],
                            remitterIdIssuedAt1 = fc["TieupViewDetailInputs.remitterIdIssuedAt1"],
                            remitterIdExpiry1 = fc["TieupViewDetailInputs.remitterIdExpiry1"],
                            beneficiaryMiddleName = fc["TieupViewDetailInputs.beneficiaryMiddleName"],
                            beneficiaryLastName = fc["TieupViewDetailInputs.beneficiaryLastName"],
                            beneficiaryCustomerType = fc["TieupViewDetailInputs.beneficiaryCustomerType"],
                            beneficiaryZipCode = fc["TieupViewDetailInputs.beneficiaryZipCode"],
                            beneficiaryNationality = fc["TieupViewDetailInputs.beneficiaryNationality"],
                            beneficiaryBirthDate = fc["TieupViewDetailInputs.beneficiaryBirthDate"],
                            beneficiaryRelationToRemitter = fc["TieupViewDetailInputs.beneficiaryRelationToRemitter"],
                            beneficiaryMobileNumber = fc["TieupViewDetailInputs.beneficiaryMobileNumber"],
                            buyingRate = fc["TieupViewDetailInputs.buyingRate"],
                            settlementCurrency = fc["TieupViewDetailInputs.settlementCurrency"] == "Funding Currency" ? "FunCur" : fc["TieupViewDetailInputs.settlementCurrency"],
                            paymentField1 = fc["TieupViewDetailInputs.paymentField1"],
                            paymentField2 = fc["TieupViewDetailInputs.paymentField2"],
                            paymentField3 = fc["TieupViewDetailInputs.paymentField3"],
                            outletCode = fc["TieupViewDetailInputs.outletCode"],
                            //senderCorrespondentBank = fc["TieupViewDetailInputs.senderCorrespondentBank"],
                            //sendingBank = fc["TieupViewDetailInputs.sendingBank"],
                            //receivingBank = fc["TieupViewDetailInputs.receivingBank"],
                            //modeOfCharge = fc["TieupViewDetailInputs.modeOfCharge"],
                            //purposeCode = fc["TieupViewDetailInputs.purposeCode"],
                            //individualCode = fc["TieupViewDetailInputs.individualCode"],
                            //natureOfBusiness = fc["TieupViewDetailInputs.natureOfBusiness"],
                            remitterId = fc["TieupViewDetailInputs.remitterId"],
                            remitterMiddleName = fc["TieupViewDetailInputs.remitterMiddleName"],
                            remitterLastName = fc["TieupViewDetailInputs.remitterLastName"],
                            remitterCustomerType = fc["TieupViewDetailInputs.remitterCustomerType"],
                            settlementAmount = fc["TieupViewDetailInputs.settlementAmount"],

                            remitterAddress2 = fc["TieupViewDetailInputs.remitterAddress2"],
                            remitterAddress3 = fc["TieupViewDetailInputs.remitterAddress3"],
                            remitterAddress4 = fc["TieupViewDetailInputs.remitterAddress4"],
                            //remitterContinent = fc["TieupViewDetailInputs.remitterContinent"],
                            //remitterProfession = fc["TieupViewDetailInputs.remitterProfession"],
                            //remitterGender = fc["TieupViewDetailInputs.remitterGender"],
                            //remitterCivilStatus = fc["TieupViewDetailInputs.remitterCivilStatus"],
                            //remitterPoBox = fc["TieupViewDetailInputs.remitterPoBox"],
                            //remitterOfficeNumber = fc["TieupViewDetailInputs.remitterOfficeNumber"],
                            //remitterEmail = fc["TieupViewDetailInputs.remitterEmail"],
                            //remitterTin = fc["TieupViewDetailInputs.remitterTin"],
                            //remitterIdType2 = fc["TieupViewDetailInputs.remitterIdType2"],
                            //remitterIdNumber2 = fc["TieupViewDetailInputs.remitterIdNumber2"],
                            //remitterIdIssuedAt2 = fc["TieupViewDetailInputs.remitterIdIssuedAt2"],
                            //remitterIdExpiry2 = fc["TieupViewDetailInputs.remitterIdExpiry2"],
                            //remitterNotificationType = fc["TieupViewDetailInputs.remitterNotificationType"],
                            //beneficiaryId = fc["TieupViewDetailInputs.beneficiaryId"],
                            beneficiaryAddress2 = fc["TieupViewDetailInputs.beneficiaryAddress2"],
                            beneficiaryAddress3 = fc["TieupViewDetailInputs.beneficiaryAddress3"],
                            beneficiaryCountry = fc["TieupViewDetailInputs.beneficiaryCountry"],
                            //beneficiaryLandmark = fc["TieupViewDetailInputs.beneficiaryLandmark"],
                            //beneficiaryProfession = fc["TieupViewDetailInputs.beneficiaryProfession"],
                            //beneficiaryGender = fc["TieupViewDetailInputs.beneficiaryGender"],
                            //beneficiaryCivilStatus = fc["TieupViewDetailInputs.beneficiaryCivilStatus"],
                            //beneficiaryPoBox = fc["TieupViewDetailInputs.beneficiaryPoBox"],
                            //alternateRecipientRelationToBeneficiary = fc["TieupViewDetailInputs.alternateRecipientRelationToBeneficiary"],
                            //beneficiaryOfficeNumber = fc["TieupViewDetailInputs.beneficiaryOfficeNumber"],
                            beneficiaryEmail = fc["TieupViewDetailInputs.beneficiaryEmail"],
                            //beneficiaryTin = fc["TieupViewDetailInputs.beneficiaryTin"],
                            //beneficiaryNotificationType = fc["TieupViewDetailInputs.beneficiaryNotificationType"],
                            transactionDate = fc["TieupViewDetailInputs.transactionDate"],
                            bankName = fc["TieupViewDetailInputs.bankName"],
                            //branchName = fc["TieupViewDetailInputs.branchName"],
                            //accountType = fc["TieupViewDetailInputs.accountType"],
                            //goldCardNumber = fc["TieupViewDetailInputs.goldCardNumber"],
                            paymentField4 = fc["TieupViewDetailInputs.paymentField4"],
                            paymentField5 = fc["TieupViewDetailInputs.paymentField5"],
                            outletBranchCode = fc["TieupViewDetailInputs.outletBranchCode"],
                            //alternateBeneficiary = fc["TieupViewDetailInputs.alternateBeneficiary"],
                            //alternateBeneficiaryRelationToBeneficiary = fc["TieupViewDetailInputs.alternateBeneficiaryRelationToBeneficiary"],
                            //messageToBeneficiary = fc["TieupViewDetailInputs.messageToBeneficiary"],
                            //receiverCorrespondentBank = fc["TieupViewDetailInputs.receiverCorrespondentBank"],
                            branchCode = fc["TieupViewDetailInputs.branchCode"],

                            //Added Format Mapper 2.0 
                            //partnerCode_start_column = fc["TieupViewDetailInputs.PartnerCodeStartColumn"],
                            //partnerCode_length = fc["TieupViewDetailInputs.PartnerCodeLength"],
                            //partnerCode_default = fc["TieupViewDetailInputs.PartnerCodeDefault"],
                            acctName_start_column = fc["TieupViewDetailInputs.AcctNameStartColumn"],
                            acctName_length = fc["TieupViewDetailInputs.AcctNameLength"],
                            acctName_default = fc["TieupViewDetailInputs.AcctNameDefault"],
                            remCountry_start_column = fc["TieupViewDetailInputs.RemCountryStartColumn"],
                            remCountry_length = fc["TieupViewDetailInputs.RemCountryLength"],
                            remCountry_default = fc["TieupViewDetailInputs.RemCountryDefault"],
                            remFourthNme_start_column = fc["TieupViewDetailInputs.RemFourthNmeStartColumn"],
                            remFourthNme_length = fc["TieupViewDetailInputs.RemFourthNmeLength"],
                            remFourthNme_default = fc["TieupViewDetailInputs.RemFourthNmeDefault"],
                            remPlaceBrth_start_column = fc["TieupViewDetailInputs.RemPlaceBrthStartColumn"],
                            remPlaceBrth_length = fc["TieupViewDetailInputs.RemPlaceBrthLength"],
                            remPlaceBrth_default = fc["TieupViewDetailInputs.RemPlaceBrthDefault"],
                            remAcctNum_start_column = fc["TieupViewDetailInputs.RemAcctNumStartColumn"],
                            remAcctNum_length = fc["TieupViewDetailInputs.RemAcctNumLength"],
                            remAcctNum_default = fc["TieupViewDetailInputs.RemAcctNumDefault"],
                            remIBAN_start_column = fc["TieupViewDetailInputs.RemIBANStartColumn"],
                            remIBAN_length = fc["TieupViewDetailInputs.RemIBANLength"],
                            remIBAN_default = fc["TieupViewDetailInputs.RemIBANDefault"],
                            res1_start_column = fc["TieupViewDetailInputs.Res1StartColumn"],
                            res1_length = fc["TieupViewDetailInputs.Res1Length"],
                            res1_default = fc["TieupViewDetailInputs.Res1Default"],
                            res2_start_column = fc["TieupViewDetailInputs.Res2StartColumn"],
                            res2_length = fc["TieupViewDetailInputs.Res2Length"],
                            res2_default = fc["TieupViewDetailInputs.Res2Default"],
                            res3_start_column = fc["TieupViewDetailInputs.Res3StartColumn"],
                            res3_length = fc["TieupViewDetailInputs.Res3Length"],
                            res3_default = fc["TieupViewDetailInputs.Res3Default"],
                            res4_start_column = fc["TieupViewDetailInputs.Res4StartColumn"],
                            res4_length = fc["TieupViewDetailInputs.Res4Length"],
                            res4_default = fc["TieupViewDetailInputs.Res4Default"],
                            res5_start_column = fc["TieupViewDetailInputs.Res5StartColumn"],
                            res5_length = fc["TieupViewDetailInputs.Res5Length"],
                            res5_default = fc["TieupViewDetailInputs.Res5Default"],
                            res6_start_column = fc["TieupViewDetailInputs.Res6StartColumn"],
                            res6_length = fc["TieupViewDetailInputs.Res6Length"],
                            res6_default = fc["TieupViewDetailInputs.Res6Default"],
                            res7_start_column = fc["TieupViewDetailInputs.Res7StartColumn"],
                            res7_length = fc["TieupViewDetailInputs.Res7Length"],
                            res7_default = fc["TieupViewDetailInputs.Res7Default"],
                            res8_start_column = fc["TieupViewDetailInputs.Res8StartColumn"],
                            res8_length = fc["TieupViewDetailInputs.Res8Length"],
                            res8_default = fc["TieupViewDetailInputs.Res8Default"],
                            res9_start_column = fc["TieupViewDetailInputs.Res9StartColumn"],
                            res9_length = fc["TieupViewDetailInputs.Res9Length"],
                            res9_default = fc["TieupViewDetailInputs.Res9Default"],
                            res10_start_column = fc["TieupViewDetailInputs.Res10StartColumn"],
                            res10_length = fc["TieupViewDetailInputs.Res10Length"],
                            res10_default = fc["TieupViewDetailInputs.Res10Default"],
                            sourceOfRem_start_column = fc["TieupViewDetailInputs.SourceOfRemStartColumn"],
                            sourceOfRem_length = fc["TieupViewDetailInputs.SourceOfRemLength"],
                            sourceOfRem_default = fc["TieupViewDetailInputs.SourceOfRemDefault"],
                            natrueOfBuss_start_column = fc["TieupViewDetailInputs.NatrueOfBussStartColumn"],
                            natrueOfBuss_length = fc["TieupViewDetailInputs.NatrueOfBussLength"],
                            natrueOfBuss_default = fc["TieupViewDetailInputs.NatrueOfBussDefault"],
                            purposeOfRem_start_column = fc["TieupViewDetailInputs.PurposeOfRemStartColumn"],
                            purposeOfRem_length = fc["TieupViewDetailInputs.PurposeOfRemLength"],
                            purposeOfRem_default = fc["TieupViewDetailInputs.PurposeOfRemDefault"],

                        };

                        //to update the tieup in restarting to insert again.
                        List<Tieup_Codes_Request> Tieup_Codes = new List<Tieup_Codes_Request>();
                        sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
                        customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes_Request();
                        customsqlCode3.sql = "UPDATE tbl_tieup_codes_request SET "
                                            + "status = '" + ModelViewer.REQ_STATUS_APPROVED + "'"
                                            + "where tieup_codes = '" + fc["TieupViewMaintInputs.tieup_codes"] + "'";
                        Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Request>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes_Request", g_global_region_code));
                        formViewModel1.TieupCodesRequest = Tieup_Codes;

                        ModelResponse modelResponseReq = dal.GenericCreate(countryReq, "Tieup_Codes_Request", g_global_region_code);

                        if (modelResponseReq.code == 0)
                        {
                            return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to update bank");

                            return Json("", JsonRequestBehavior.AllowGet);
                        }

                    }
                    else if (valid == 2) //Approve
                    {
                        tprequest.pk_tieup_code_id = id;
                        tprequest = JsonConvert.DeserializeObject<Tieup_Codes_Request>(dal.GetGenericDetails(tprequest, "Tieup_Codes_Request", g_global_region_code));


                        if (tprequest.req_type == ModelViewer.REQ_TYPE_CREATE) //Approve from new creation
                        {
                            var tieupDetail = new VREMIT.ModelMVC.Tieup_Codes_Detail
                            {
                                pk_detail_tieup_id = tprequest.pk_tieup_code_id,
                                tieup_code_id = tprequest.tieup_codes, //Tieup Partner
                                //remarks = fc["TieupViewMaintInputs.remarks"],
                                remarks = tprequest.remarks,
                                status = newStat,
                                username = _currentUser.Username,

                                rt_action = ModelViewer.USER_ACTION_APPROVE,
                                rt_terminal_id = base._userClientIP,
                                rt_user_agent = base._userAgent,
                                rt_username = _currentUser.Username,

                                provided = tprequest.provided,
                                dt_last_chg = DateTime.Now,
                                file_type = tprequest.file_type,
                                delimiter_val = tprequest.delimiter_val,
                                d_prefix = tprequest.d_prefix,
                                delimiter_type = tprequest.delimiter_type,
                                delimiter = tprequest.delimiter,
                                detail_start_record = tprequest.detail_start_record,
                                detail_transdate_start_column = tprequest.detail_transdate_start_column,
                                detail_transdate_length = tprequest.detail_transdate_length,
                                detail_application_start_column = tprequest.detail_application_start_column,
                                detail_application_length = tprequest.detail_application_length,
                                detail_remitid_start_column = tprequest.detail_remitid_start_column,
                                detail_remitid_length = tprequest.detail_remitid_length,
                                detail_remitfname_start_column = tprequest.detail_remitfname_start_column,
                                detail_remitfname_length = tprequest.detail_remitfname_length,
                                detail_remitmidname_start_column = tprequest.detail_remitmidname_start_column,
                                detail_remitmidname_length = tprequest.detail_remitmidname_length,
                                detail_remitlastname_start_column = tprequest.detail_remitlastname_start_column,
                                detail_remitlastname_length = tprequest.detail_remitlastname_length,
                                d_remitCusType_start_column = tprequest.d_remitCusType_start_column,//
                                d_remitCusType_length = tprequest.d_remitCusType_length,
                                d_remitNationality_start_column = tprequest.d_remitNationality_start_column,//
                                d_remitNationality_length = tprequest.d_remitNationality_length,
                                d_remitAdd1_start_column = tprequest.d_remitAdd1_start_column,
                                d_remitAdd1_length = tprequest.d_remitAdd1_length,
                                d_remitAdd2_start_column = tprequest.d_remitAdd2_start_column,
                                d_remitAdd2_length = tprequest.d_remitAdd2_length,
                                d_remitAdd3_start_column = tprequest.d_remitAdd3_start_column,
                                d_remitAdd3_length = tprequest.d_remitAdd3_length,
                                d_remitAdd4_start_column = tprequest.d_remitAdd4_start_column,
                                d_remitAdd4_length = tprequest.d_remitAdd4_length,
                                d_remitContinent_start_column = tprequest.d_remitContinent_start_column,
                                d_remitContinent_length = tprequest.d_remitContinent_length,
                                d_remitZipCode_start_column = tprequest.d_remitZipCode_start_column,
                                d_remitZipCode_length = tprequest.d_remitZipCode_length,
                                d_remitBDate_start_column = tprequest.d_remitBDate_start_column,
                                d_remitBDate_length = tprequest.d_remitBDate_length,
                                d_remitProf_start_column = tprequest.d_remitProf_start_column,
                                d_remitProf_length = tprequest.d_remitProf_length,
                                d_remitGender_start_column = tprequest.d_remitGender_start_column,
                                d_remitGender_length = tprequest.d_remitGender_length,//
                                d_remitCivilStat_start_column = tprequest.d_remitCivilStat_start_column,
                                d_remitCivilStat_length = tprequest.d_remitCivilStat_length,
                                d_remitPOBox_start_column = tprequest.d_remitPOBox_start_column,
                                d_remitPOBox_length = tprequest.d_remitPOBox_length,//
                                d_remitMobPhoneNum_start_column = tprequest.d_remitMobPhoneNum_start_column,
                                d_remitMobPhoneNum_length = tprequest.d_remitMobPhoneNum_length,
                                d_remitOfficePhoneNo_start_column = tprequest.d_remitOfficePhoneNo_start_column,
                                d_remitOfficePhoneNo_length = tprequest.d_remitOfficePhoneNo_length,
                                d_remitEmailAdd_start_column = tprequest.d_remitEmailAdd_start_column,
                                d_remitEmailAdd_length = tprequest.d_remitEmailAdd_length,
                                d_remitTaxIDNo_start_column = tprequest.d_remitTaxIDNo_start_column,
                                d_remitTaxIDNo_length = tprequest.d_remitTaxIDNo_length,
                                d_remitIDType1_start_column = tprequest.d_remitIDType1_start_column,
                                d_remitIDType1_length = tprequest.d_remitIDType1_length,
                                d_remitIDNum1_start_column = tprequest.d_remitIDNum1_start_column,
                                d_remitIDNum1_length = tprequest.d_remitIDNum1_length,
                                d_remitIDIssueAt1_start_column = tprequest.d_remitIDIssueAt1_start_column,
                                d_remitIDIssueAt1_length = tprequest.d_remitIDIssueAt1_length,
                                d_remitIDExpDate1_start_column = tprequest.d_remitIDExpDate1_start_column,
                                d_remitIDExpDate1_length = tprequest.d_remitIDExpDate1_length,
                                d_remitIDType2_start_column = tprequest.d_remitIDType2_start_column,
                                d_remitIDType2_length = tprequest.d_remitIDType2_length,
                                d_remitIDNo2_start_column = tprequest.d_remitIDNo2_start_column,
                                d_remitIDNo2_length = tprequest.d_remitIDNo2_length,
                                d_remitIDIssueAt2_start_column = tprequest.d_remitIDIssueAt2_start_column,
                                d_remitIDIssueAt2_length = tprequest.d_remitIDIssueAt2_length,
                                d_remitIDExpDate2_start_column = tprequest.d_remitIDExpDate2_start_column,
                                d_remitIDExpDate2_length = tprequest.d_remitIDExpDate2_length,
                                d_remitNotifType_start_column = tprequest.d_remitNotifType_start_column,
                                d_remitNotifType_length = tprequest.d_remitNotifType_length,
                                d_beneficiaryID_start_column = tprequest.d_beneficiaryID_start_column,
                                d_beneficiaryID_length = tprequest.d_beneficiaryID_length,
                                d_beneficiaryFName_start_column = tprequest.d_beneficiaryFName_start_column,
                                d_beneficiaryFName_length = tprequest.d_beneficiaryFName_length,
                                d_beneficiaryMName_start_column = tprequest.d_beneficiaryMName_start_column,
                                d_beneficiaryMName_length = tprequest.d_beneficiaryMName_length,
                                d_beneficiaryLName_start_column = tprequest.d_beneficiaryLName_start_column,
                                d_beneficiaryLName_length = tprequest.d_beneficiaryLName_length,
                                d_beneficiaryCusType_start_column = tprequest.d_beneficiaryCusType_start_column,
                                d_beneficiaryCusType_length = tprequest.d_beneficiaryCusType_length,
                                d_beneficiaryAdd1_start_column = tprequest.d_beneficiaryAdd1_start_column,
                                d_beneficiaryAdd1_length = tprequest.d_beneficiaryAdd1_length,//
                                d_beneficiaryAdd2_start_column = tprequest.d_beneficiaryAdd2_start_column,
                                d_beneficiaryAdd2_length = tprequest.d_beneficiaryAdd2_length,//
                                d_beneficiaryAdd3_start_column = tprequest.d_beneficiaryAdd3_start_column,
                                d_beneficiaryAdd3_length = tprequest.d_beneficiaryAdd3_length,//
                                d_beneficiaryCountry_start_column = tprequest.d_beneficiaryCountry_start_column,
                                d_beneficiaryCountry_length = tprequest.d_beneficiaryCountry_length,
                                d_zipCode_start_column = tprequest.d_zipCode_start_column,
                                d_zipCode_length = tprequest.d_zipCode_length,
                                d_landMark_start_column = tprequest.d_landMark_start_column,
                                d_landMark_length = tprequest.d_landMark_length,
                                d_beneficiaryNationality_start_column = tprequest.d_beneficiaryNationality_start_column,
                                d_beneficiaryNationality_length = tprequest.d_beneficiaryNationality_length,
                                d_beneficiaryBDate_start_column = tprequest.d_beneficiaryBDate_start_column,
                                d_beneficiaryBDate_length = tprequest.d_beneficiaryBDate_length,
                                d_beneficiaryProf_start_column = tprequest.d_beneficiaryProf_start_column,
                                d_beneficiaryProf_length = tprequest.d_beneficiaryProf_length,
                                d_beneficiaryGender_start_column = tprequest.d_beneficiaryGender_start_column,
                                d_beneficiaryGender_length = tprequest.d_beneficiaryGender_length,
                                d_beneficiaryCivilStat_start_column = tprequest.d_beneficiaryCivilStat_start_column,
                                d_beneficiaryCivilStat_length = tprequest.d_beneficiaryCivilStat_length,
                                d_beneficiaryPOBox_start_column = tprequest.d_beneficiaryPOBox_start_column,
                                d_beneficiaryPOBox_length = tprequest.d_beneficiaryPOBox_length,
                                d_beneficiaryRel_tothe_remit_start_column = tprequest.d_beneficiaryRel_tothe_remit_start_column,
                                d_beneficiaryRel_tothe_remit_length = tprequest.d_beneficiaryRel_tothe_remit_length,
                                d_alterRecipient_reltobeneficiary_start_column = tprequest.d_alterRecipient_reltobeneficiary_start_column,
                                d_alterRecipient_reltobeneficiary_length = tprequest.d_alterRecipient_reltobeneficiary_length,
                                d_beneficiaryMobPhoneNo_start_column = tprequest.d_beneficiaryMobPhoneNo_start_column,
                                d_beneficiaryMobPhoneNo_length = tprequest.d_beneficiaryMobPhoneNo_length,
                                d_beneficiaryOfficePhoneNo_start_column = tprequest.d_beneficiaryOfficePhoneNo_start_column,
                                d_beneficiaryOfficePhoneNo_length = tprequest.d_beneficiaryOfficePhoneNo_length,
                                d_beneficiaryEmailAdd_start_column = tprequest.d_beneficiaryEmailAdd_start_column,
                                d_beneficiaryEmailAdd_length = tprequest.d_beneficiaryEmailAdd_length,//
                                d_beneficiaryTaxIDNo_start_column = tprequest.d_beneficiaryTaxIDNo_start_column,
                                d_beneficiaryTaxIDNo_length = tprequest.d_beneficiaryTaxIDNo_length,//
                                d_beneficiaryNotifType_start_column = tprequest.d_beneficiaryNotifType_start_column,
                                d_beneficiaryNotifType_length = tprequest.d_beneficiaryNotifType_length,
                                d_fundCurrency_start_column = tprequest.d_fundCurrency_start_column,
                                d_fundCurrency_length = tprequest.d_fundCurrency_length,
                                d_fundAmount_start_column = tprequest.d_fundAmount_start_column,
                                d_fundAmount_length = tprequest.d_fundAmount_length,
                                d_buyingRate_start_column = tprequest.d_buyingRate_start_column,
                                d_buyingRate_length = tprequest.d_buyingRate_length,
                                d_settlementCurrency_start_column = tprequest.d_settlementCurrency_start_column,
                                d_settlementCurrency_length = tprequest.d_settlementCurrency_length,
                                d_settlementAmount_start_column = tprequest.d_settlementAmount_start_column,
                                d_settlementAmount_length = tprequest.d_settlementAmount_length,
                                d_settlementMode_start_column = tprequest.d_settlementMode_start_column,
                                d_settlementMode_length = tprequest.d_settlementMode_length,
                                d_bankCode_start_column = tprequest.d_bankCode_start_column,
                                d_bankCode_length = tprequest.d_bankCode_length,
                                d_bankName_start_column = tprequest.d_bankName_start_column,
                                d_bankName_length = tprequest.d_bankName_length,
                                d_branchCode_start_column = tprequest.d_branchCode_start_column,
                                d_branchCode_length = tprequest.d_branchCode_length,
                                d_branchName_start_column = tprequest.d_branchName_start_column,
                                d_branchName_length = tprequest.d_branchName_length,
                                d_accountType_start_column = tprequest.d_accountType_start_column,
                                d_accountType_length = tprequest.d_accountType_length,
                                d_accountNo_start_column = tprequest.d_accountNo_start_column,
                                d_accountNo_length = tprequest.d_accountNo_length,//
                                d_goldCardNo_start_column = tprequest.d_goldCardNo_start_column,
                                d_goldCardNo_length = tprequest.d_goldCardNo_length,
                                d_billsPayField1_start_column = tprequest.d_billsPayField1_start_column,
                                d_billsPayField1_length = tprequest.d_billsPayField1_length,
                                d_billsPayField2_start_column = tprequest.d_billsPayField2_start_column,
                                d_billsPayField2_length = tprequest.d_billsPayField2_length,
                                d_billsPayField3_start_column = tprequest.d_billsPayField3_start_column,
                                d_billsPayField3_length = tprequest.d_billsPayField3_length,
                                d_billsPayField4_start_column = tprequest.d_billsPayField4_start_column,
                                d_billsPayField4_length = tprequest.d_billsPayField4_length,
                                d_billsPayField5_start_column = tprequest.d_billsPayField5_start_column,
                                d_billsPayField5_length = tprequest.d_billsPayField5_length,
                                d_outletCode_start_column = tprequest.d_outletCode_start_column,
                                d_outletCode_length = tprequest.d_outletCode_length,
                                d_outletBranchCode_start_column = tprequest.d_outletBranchCode_start_column,
                                d_outletBranchCode_length = tprequest.d_outletBranchCode_length,
                                d_alterBeneficiaryName_start_column = tprequest.d_alterBeneficiaryName_start_column,
                                d_alterBeneficiaryName_length = tprequest.d_alterBeneficiaryName_length,
                                d_alterBeneficiary_relto_beneficiary_start_column = tprequest.d_alterBeneficiary_relto_beneficiary_start_column,
                                d_alterBeneficiary_relto_beneficiary_length = tprequest.d_alterBeneficiary_relto_beneficiary_length,
                                d_messageToBeneficiary_start_column = tprequest.d_messageToBeneficiary_start_column,
                                d_messageToBeneficiary_length = tprequest.d_messageToBeneficiary_length,
                                d_receiverCorresBank_start_column = tprequest.d_receiverCorresBank_start_column,
                                d_receiverCorresBank_length = tprequest.d_receiverCorresBank_length,
                                d_senderCorresBank_start_column = tprequest.d_senderCorresBank_start_column,
                                d_senderCorresBank_length = tprequest.d_senderCorresBank_length,
                                d_sendingBank_start_column = tprequest.d_sendingBank_start_column,
                                d_sendingBank_length = tprequest.d_sendingBank_length,
                                d_receivingBank_start_column = tprequest.d_receivingBank_start_column,
                                d_receivingBank_length = tprequest.d_receivingBank_length,
                                d_modeOfChange_start_column = tprequest.d_modeOfChange_start_column,
                                d_modeOfChange_length = tprequest.d_modeOfChange_length,
                                d_purposeCode_start_column = tprequest.d_purposeCode_start_column,
                                d_purposeCode_length = tprequest.d_purposeCode_length,
                                d_indvCode_start_column = tprequest.d_indvCode_start_column,
                                d_indvCode_length = tprequest.d_indvCode_length,

                                //---- Default Data ----- 
                                applicationNumber = tprequest.applicationNumber,
                                settlementMode = tprequest.settlementMode,
                                fundingAmount = tprequest.fundingAmount,
                                fundingCurrency = tprequest.fundingCurrency,
                                remitterFirstName = tprequest.remitterFirstName,
                                beneficiaryFirstName = tprequest.beneficiaryFirstName,
                                accountNumber = tprequest.accountNumber,
                                bankCode = tprequest.bankCode,
                                beneficiaryAddress1 = tprequest.beneficiaryAddress1,
                                remitterNationality = tprequest.remitterNationality,
                                remitterAddress1 = tprequest.remitterAddress1,
                                //remitterCountry = tprequest.remitterCountry,
                                //remitterZipCode = tprequest.remitterZipCode,
                                remitterBirthDate = tprequest.remitterBirthDate,
                                remitterMobileNumber = tprequest.remitterMobileNumber,
                                remitterIdType1 = tprequest.remitterIdType1,
                                remitterIdNumber1 = tprequest.remitterIdNumber1,
                                remitterIdIssuedAt1 = tprequest.remitterIdIssuedAt1,
                                remitterIdExpiry1 = tprequest.remitterIdExpiry1,
                                beneficiaryMiddleName = tprequest.beneficiaryMiddleName,
                                beneficiaryLastName = tprequest.beneficiaryLastName,
                                beneficiaryCustomerType = tprequest.beneficiaryCustomerType,
                                beneficiaryZipCode = tprequest.beneficiaryZipCode,
                                beneficiaryNationality = tprequest.beneficiaryNationality,
                                beneficiaryBirthDate = tprequest.beneficiaryBirthDate,
                                beneficiaryRelationToRemitter = tprequest.beneficiaryRelationToRemitter,
                                beneficiaryMobileNumber = tprequest.beneficiaryMobileNumber,
                                buyingRate = tprequest.buyingRate,
                                settlementCurrency = tprequest.settlementCurrency,
                                paymentField1 = tprequest.paymentField1,
                                paymentField2 = tprequest.paymentField2,
                                paymentField3 = tprequest.paymentField3,
                                outletCode = tprequest.outletCode,
                                //senderCorrespondentBank = tprequest.senderCorrespondentBank,
                                //sendingBank = tprequest.sendingBank,
                                //receivingBank = tprequest.receivingBank,
                                //modeOfCharge = tprequest.modeOfCharge,
                                //purposeCode = tprequest.purposeCode,
                                //individualCode = tprequest.individualCode,
                                //natureOfBusiness = tprequest.natureOfBusiness,
                                remitterId = tprequest.remitterId,
                                remitterMiddleName = tprequest.remitterMiddleName,
                                remitterLastName = tprequest.remitterLastName,
                                remitterCustomerType = tprequest.remitterCustomerType,
                                settlementAmount = tprequest.settlementAmount,

                                remitterAddress2 = tprequest.remitterAddress2,
                                remitterAddress3 = tprequest.remitterAddress3,
                                remitterAddress4 = tprequest.remitterAddress4,
                                //remitterContinent = tprequest.remitterContinent,
                                //remitterProfession = tprequest.remitterProfession,
                                //remitterGender = tprequest.remitterGender,
                                //remitterCivilStatus = tprequest.remitterCivilStatus,
                                //remitterPoBox = tprequest.remitterPoBox,
                                //remitterOfficeNumber = tprequest.remitterOfficeNumber,
                                //remitterEmail = tprequest.remitterEmail,
                                //remitterTin = tprequest.remitterTin,
                                //remitterIdType2 = tprequest.remitterIdType2,
                                //remitterIdNumber2 = tprequest.remitterIdNumber2,
                                //remitterIdIssuedAt2 = tprequest.remitterIdIssuedAt2,
                                //remitterIdExpiry2 = tprequest.remitterIdExpiry2,
                                //remitterNotificationType = tprequest.remitterNotificationType,
                                //beneficiaryId = tprequest.beneficiaryId,
                                beneficiaryAddress2 = tprequest.beneficiaryAddress2,
                                beneficiaryAddress3 = tprequest.beneficiaryAddress3,
                                beneficiaryCountry = tprequest.beneficiaryCountry,
                                //beneficiaryLandmark = tprequest.beneficiaryLandmark,
                                //beneficiaryProfession = tprequest.beneficiaryProfession,
                                //beneficiaryGender = tprequest.beneficiaryGender,
                                //beneficiaryCivilStatus = tprequest.beneficiaryCivilStatus,
                                //beneficiaryPoBox = tprequest.beneficiaryPoBox,
                                //alternateRecipientRelationToBeneficiary = tprequest.alternateRecipientRelationToBeneficiary,
                                //beneficiaryOfficeNumber = tprequest.beneficiaryOfficeNumber,
                                beneficiaryEmail = tprequest.beneficiaryEmail,
                                //beneficiaryTin = tprequest.beneficiaryTin,
                                //beneficiaryNotificationType = tprequest.beneficiaryNotificationType,
                                transactionDate = tprequest.transactionDate,
                                bankName = tprequest.bankName,
                                //branchName = tprequest.branchName,
                                //accountType = tprequest.accountType,
                                //goldCardNumber = tprequest.goldCardNumber,
                                paymentField4 = tprequest.paymentField4,
                                paymentField5 = tprequest.paymentField5,
                                outletBranchCode = tprequest.outletBranchCode,
                                //alternateBeneficiary = tprequest.alternateBeneficiary,
                                //alternateBeneficiaryRelationToBeneficiary = tprequest.alternateBeneficiaryRelationToBeneficiary,
                                //messageToBeneficiary = tprequest.messageToBeneficiary,
                                //receiverCorrespondentBank = tprequest.receiverCorrespondentBank,
                                branchCode = tprequest.branchCode,

                                // New Add Format Mapper 2.0
                                //partnerCode_start_column = tprequest.partnerCode_start_column,
                                //partnerCode_length = tprequest.partnerCode_length,
                                //partnerCode_default = tprequest.partnerCode_default,
                                acctName_start_column = tprequest.acctName_start_column,
                                acctName_length = tprequest.acctName_length,
                                acctName_default = tprequest.acctName_default,
                                remCountry_start_column = tprequest.remCountry_start_column,
                                remCountry_length = tprequest.remCountry_length,
                                remCountry_default = tprequest.remCountry_default,
                                remFourthNme_start_column = tprequest.remFourthNme_start_column,
                                remFourthNme_length = tprequest.remFourthNme_length,
                                remFourthNme_default = tprequest.remFourthNme_default,
                                remPlaceBrth_start_column = tprequest.remPlaceBrth_start_column,
                                remPlaceBrth_length = tprequest.remPlaceBrth_length,
                                remPlaceBrth_default = tprequest.remPlaceBrth_default,
                                remAcctNum_start_column = tprequest.remAcctNum_start_column,
                                remAcctNum_length = tprequest.remAcctNum_length,
                                remAcctNum_default = tprequest.remAcctNum_default,
                                remIBAN_start_column = tprequest.remIBAN_start_column,
                                remIBAN_length = tprequest.remIBAN_length,
                                remIBAN_default = tprequest.remIBAN_default,
                                res1_start_column = tprequest.res1_start_column,
                                res1_length = tprequest.res1_length,
                                res1_default = tprequest.res1_default,
                                res2_start_column = tprequest.res2_start_column,
                                res2_length = tprequest.res2_length,
                                res2_default = tprequest.res2_default,
                                res3_start_column = tprequest.res3_start_column,
                                res3_length = tprequest.res3_length,
                                res3_default = tprequest.res3_default,
                                res4_start_column = tprequest.res4_start_column,
                                res4_length = tprequest.res4_length,
                                res4_default = tprequest.res4_default,
                                res5_start_column = tprequest.res5_start_column,
                                res5_length = tprequest.res5_length,
                                res5_default = tprequest.res5_default,
                                res6_start_column = tprequest.res6_start_column,
                                res6_length = tprequest.res6_length,
                                res6_default = tprequest.res6_default,
                                res7_start_column = tprequest.res7_start_column,
                                res7_length = tprequest.res7_length,
                                res7_default = tprequest.res7_default,
                                res8_start_column = tprequest.res8_start_column,
                                res8_length = tprequest.res8_length,
                                res8_default = tprequest.res8_default,
                                res9_start_column = tprequest.res9_start_column,
                                res9_length = tprequest.res9_length,
                                res9_default = tprequest.res9_default,
                                res10_start_column = tprequest.res10_start_column,
                                res10_length = tprequest.res10_length,
                                res10_default = tprequest.res10_default,
                                sourceOfRem_start_column = tprequest.sourceOfRem_start_column,
                                sourceOfRem_length = tprequest.sourceOfRem_length,
                                sourceOfRem_default = tprequest.sourceOfRem_default,
                                natrueOfBuss_start_column = tprequest.natrueOfBuss_start_column,
                                natrueOfBuss_length = tprequest.natrueOfBuss_length,
                                natrueOfBuss_default = tprequest.natrueOfBuss_default,
                                purposeOfRem_start_column = tprequest.purposeOfRem_start_column,
                                purposeOfRem_length = tprequest.purposeOfRem_length,
                                purposeOfRem_default = tprequest.purposeOfRem_default,

                            };

                            ModelResponse modelResponseApprove1 = dal.GenericCreate(tieupDetail, "Tieup_Codes_Detail", g_global_region_code);

                            var tieupDetail1 = new VREMIT.ModelMVC.Tieup_Codes
                            {
                                pk_tieup_code_id = tprequest.pk_tieup_code_id,
                                file_type = tprequest.file_type,
                                tieup_codes = tprequest.tieup_codes,
                                settlement_currency = tprequest.settlement_currency == "Funding Currency" ? "FunCur" : tprequest.settlement_currency,
                                start_record = tprequest.start_record,
                                status = newStat,
                                filename_validation = tprequest.filename_validation,
                                footer_validation = tprequest.footer_validation,
                                //remarks = fc["TieupViewMaintInputs.remarks"],
                                remarks = tprequest.remarks,
                                dt_last_chg = DateTime.Now
                            };

                            ModelResponse modelResponseApprove = dal.GenericCreate(tieupDetail1, "Tieup_Codes", g_global_region_code);

                            if (modelResponseApprove.code == 0 && modelResponseApprove1.code == 0)
                            {

                                tprequest.rt_action = ModelViewer.USER_ACTION_APPROVE;
                                tprequest.rt_terminal_id = base._userClientIP;
                                tprequest.rt_user_agent = base._userAgent;
                                tprequest.rt_username = _currentUser.Username;
                                tprequest.req_status = ModelViewer.REQ_STATUS_APPROVED;
                                tprequest.req_status_reason = fc["TieupViewMaintInputs.remarks"];
                                dal.GenericUpdate(tprequest, "Tieup_Codes_Request", g_global_region_code);

                                //TempData["title"] = "Approved!";
                                //TempData["text"] = "New request has been successfully approved.";
                                //TempData["icon"] = "success";
                                return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Failed to update bank");

                                return Json("", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else //Approve from old creation
                        {
                            if (tprequest.req_type == ModelViewer.REQ_TYPE_MODIFY) //Approved from Edit
                            {
                                List<Tieup_Codes> Tieup_Codes = new List<Tieup_Codes>();
                                sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
                                customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes();
                                customsqlCode3.sql = "UPDATE tbl_tieup_codes SET "
                                                    + "file_type = '" + tprequest.file_type + "', "
                                                    + "username = '" + tprequest.username + "', "
                                                    + "[dt_last_chg] = getDate(), "
                                                    //+ "status = '" + ModelViewer.REQ_STATUS_APPROVED + "', "
                                                    + "remarks = '" + tprequest.remarks + "', "
                                                    + "start_record = '" + tprequest.start_record + "', "
                                                    + "filename_validation = '" + tprequest.filename_validation + "', "
                                                    + "footer_validation = '" + tprequest.footer_validation + "', "
                                                    + "settlement_currency = '" + tprequest.settlement_currency + "'"
                                                    + "where tieup_codes = '" + tprequest.tieup_codes + "'";
                                Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes", g_global_region_code));
                                formViewModel1.TieupCodesMaintParam = Tieup_Codes;

                                List<Tieup_Codes_Detail> TieupCodesDetail = new List<Tieup_Codes_Detail>();
                                sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
                                customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_Detail();
                                customsqlCode2.sql = "UPDATE tbl_tieup_codes_detail SET "
                                + "[file_type] = '" + (tprequest.file_type ?? "") + "', "
                                + "[delimiter_type] = '" + (tprequest.delimiter_type ?? "") + "', "
                                + "[delimiter] = '" + (tprequest.delimiter ?? "") + "', "
                                + "[username] = '" + (tprequest.username ?? "") + "', "
                                + "[dt_last_chg] = getDate(), "
                                + "[provided] = '" + (tprequest.provided ?? "") + "', "
                                + "[remarks] = '" + (tprequest.remarks ?? "") + "', "
                                + "[delimiter_val] = '" + (tprequest.delimiter_val ?? "") + "', "
                                + "[detail_start_record] = '" + (tprequest.detail_start_record ?? "") + "', "
                                + "[detail_transdate_start_column] = '" + (tprequest.detail_transdate_start_column ?? "").Trim() + "', "
                                + "[detail_transdate_length] = '" + (tprequest.detail_transdate_length ?? "").Trim() + "', "
                                + "[detail_application_start_column] = '" + (tprequest.detail_application_start_column ?? "").Trim() + "', "
                                + "[detail_application_length] = '" + (tprequest.detail_application_length ?? "").Trim() + "', "
                                + "[detail_remitid_start_column] = '" + (tprequest.detail_remitid_start_column ?? "").Trim() + "', "
                                + "[detail_remitid_length] = '" + (tprequest.detail_remitid_length ?? "").Trim() + "', "
                                + "[detail_remitfname_start_column] = '" + (tprequest.detail_remitfname_start_column ?? "").Trim() + "', "
                                + "[detail_remitfname_length] = '" + (tprequest.detail_remitfname_length ?? "").Trim() + "', "
                                + "[detail_remitmidname_start_column] = '" + (tprequest.detail_remitmidname_start_column ?? "").Trim() + "', "
                                + "[detail_remitmidname_length] = '" + (tprequest.detail_remitmidname_length ?? "").Trim() + "', "
                                + "[detail_remitlastname_start_column] = '" + (tprequest.detail_remitlastname_start_column ?? "").Trim() + "', "
                                + "[detail_remitlastname_length] = '" + (tprequest.detail_remitlastname_length ?? "").Trim() + "', "
                                + "[d_remitCusType_start_column] = '" + (tprequest.d_remitCusType_start_column ?? "").Trim() + "', "
                                + "[d_remitCusType_length] = '" + (tprequest.d_remitCusType_length ?? "").Trim() + "', "
                                + "[d_remitNationality_start_column] = '" + (tprequest.d_remitNationality_start_column ?? "").Trim() + "', "
                                + "[d_remitNationality_length] = '" + (tprequest.d_remitNationality_length ?? "").Trim() + "', "
                                + "[d_remitAdd1_start_column] = '" + (tprequest.d_remitAdd1_start_column ?? "").Trim() + "', "
                                + "[d_remitAdd1_length] = '" + (tprequest.d_remitAdd1_length ?? "").Trim() + "', "
                                + "[d_remitAdd2_start_column] = '" + (tprequest.d_remitAdd2_start_column ?? "").Trim() + "', "
                                + "[d_remitAdd2_length] = '" + (tprequest.d_remitAdd2_length ?? "").Trim() + "', "
                                + "[d_remitAdd3_start_column] = '" + (tprequest.d_remitAdd3_start_column ?? "").Trim() + "', "
                                + "[d_remitAdd3_length] = '" + (tprequest.d_remitAdd3_length ?? "").Trim() + "', "
                                + "[d_remitAdd4_start_column] = '" + (tprequest.d_remitAdd4_start_column ?? "").Trim() + "', "
                                + "[d_remitAdd4_length] = '" + (tprequest.d_remitAdd4_length ?? "").Trim() + "', "
                                + "[d_remitContinent_start_column] = '" + (tprequest.d_remitContinent_start_column ?? "").Trim() + "', "
                                + "[d_remitContinent_length] = '" + (tprequest.d_remitContinent_length ?? "").Trim() + "', "
                                + "[d_remitZipCode_start_column] = '" + (tprequest.d_remitZipCode_start_column ?? "").Trim() + "', "
                                + "[d_remitZipCode_length] = '" + (tprequest.d_remitZipCode_length ?? "").Trim() + "', "
                                + "[d_remitBDate_start_column] = '" + (tprequest.d_remitBDate_start_column ?? "").Trim() + "', "
                                + "[d_remitBDate_length] = '" + (tprequest.d_remitBDate_length ?? "").Trim() + "', "
                                + "[d_remitProf_start_column] = '" + (tprequest.d_remitProf_start_column ?? "").Trim() + "', "
                                + "[d_remitProf_length] = '" + (tprequest.d_remitProf_length ?? "").Trim() + "', "
                                + "[d_remitGender_start_column] = '" + (tprequest.d_remitGender_start_column ?? "").Trim() + "', "
                                + "[d_remitGender_length] = '" + (tprequest.d_remitGender_length ?? "").Trim() + "', "
                                + "[d_remitCivilStat_start_column] = '" + (tprequest.d_remitCivilStat_start_column ?? "").Trim() + "', "
                                + "[d_remitCivilStat_length] = '" + (tprequest.d_remitCivilStat_length ?? "").Trim() + "', "
                                + "[d_remitPOBox_start_column] = '" + (tprequest.d_remitPOBox_start_column ?? "").Trim() + "', "
                                + "[d_remitPOBox_length] = '" + (tprequest.d_remitPOBox_length ?? "").Trim() + "', "
                                + "[d_remitMobPhoneNum_start_column] = '" + (tprequest.d_remitMobPhoneNum_start_column ?? "").Trim() + "', "
                                + "[d_remitMobPhoneNum_length] = '" + (tprequest.d_remitMobPhoneNum_length ?? "").Trim() + "', "
                                + "[d_remitOfficePhoneNo_start_column] = '" + (tprequest.d_remitOfficePhoneNo_start_column ?? "").Trim() + "', "
                                + "[d_remitOfficePhoneNo_length] = '" + (tprequest.d_remitOfficePhoneNo_length ?? "").Trim() + "', "
                                + "[d_remitEmailAdd_start_column] = '" + (tprequest.d_remitEmailAdd_start_column ?? "").Trim() + "', "
                                + "[d_remitEmailAdd_length] = '" + (tprequest.d_remitEmailAdd_length ?? "").Trim() + "', "
                                + "[d_remitTaxIDNo_start_column] = '" + (tprequest.d_remitTaxIDNo_start_column ?? "").Trim() + "', "
                                + "[d_remitTaxIDNo_length] = '" + (tprequest.d_remitTaxIDNo_length ?? "").Trim() + "', "
                                + "[d_remitIDType1_start_column] = '" + (tprequest.d_remitIDType1_start_column ?? "").Trim() + "', "
                                + "[d_remitIDType1_length] = '" + (tprequest.d_remitIDType1_length ?? "").Trim() + "', "
                                + "[d_remitIDNum1_start_column] = '" + (tprequest.d_remitIDNum1_start_column ?? "").Trim() + "', "
                                + "[d_remitIDNum1_length] = '" + (tprequest.d_remitIDNum1_length ?? "").Trim() + "', "
                                + "[d_remitIDIssueAt1_start_column] = '" + (tprequest.d_remitIDIssueAt1_start_column ?? "").Trim() + "', "
                                + "[d_remitIDIssueAt1_length] = '" + (tprequest.d_remitIDIssueAt1_length ?? "").Trim() + "', "
                                + "[d_remitIDExpDate1_start_column] = '" + (tprequest.d_remitIDExpDate1_start_column ?? "").Trim() + "', "
                                + "[d_remitIDExpDate1_length] = '" + (tprequest.d_remitIDExpDate1_length ?? "").Trim() + "', "
                                + "[d_remitIDType2_start_column] = '" + (tprequest.d_remitIDType2_start_column ?? "").Trim() + "', "
                                + "[d_remitIDType2_length] = '" + (tprequest.d_remitIDType2_length ?? "").Trim() + "', "
                                + "[d_remitIDNo2_start_column] = '" + (tprequest.d_remitIDNo2_start_column ?? "").Trim() + "', "
                                + "[d_remitIDNo2_length] = '" + (tprequest.d_remitIDNo2_length ?? "").Trim() + "', "

                                + "[d_remitIDIssueAt2_start_column] = '" + (tprequest.d_remitIDIssueAt2_start_column ?? "").Trim() + "', "
                                + "[d_remitIDIssueAt2_length] = '" + (tprequest.d_remitIDIssueAt2_length ?? "").Trim() + "', "
                                + "[d_remitIDExpDate2_start_column] = '" + (tprequest.d_remitIDExpDate2_start_column ?? "").Trim() + "', "
                                + "[d_remitIDExpDate2_length] = '" + (tprequest.d_remitIDExpDate2_length ?? "").Trim() + "', "
                                + "[d_remitNotifType_start_column] = '" + (tprequest.d_remitNotifType_start_column ?? "").Trim() + "', "
                                + "[d_remitNotifType_length] = '" + (tprequest.d_remitNotifType_length ?? "").Trim() + "', "
                                + "[d_beneficiaryID_start_column] = '" + (tprequest.d_beneficiaryID_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryID_length] = '" + (tprequest.d_beneficiaryID_length ?? "").Trim() + "', "
                                + "[d_beneficiaryFName_start_column] = '" + (tprequest.d_beneficiaryFName_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryFName_length] = '" + (tprequest.d_beneficiaryFName_length ?? "").Trim() + "', "
                                + "[d_beneficiaryMName_start_column] = '" + (tprequest.d_beneficiaryMName_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryMName_length] = '" + (tprequest.d_beneficiaryMName_length ?? "").Trim() + "', "
                                + "[d_beneficiaryLName_start_column] = '" + (tprequest.d_beneficiaryLName_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryLName_length] = '" + (tprequest.d_beneficiaryLName_length ?? "").Trim() + "', "
                                + "[d_beneficiaryCusType_start_column] = '" + (tprequest.d_beneficiaryCusType_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryCusType_length] = '" + (tprequest.d_beneficiaryCusType_length ?? "").Trim() + "', "
                                + "[d_beneficiaryAdd1_start_column] = '" + (tprequest.d_beneficiaryAdd1_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryAdd1_length] = '" + (tprequest.d_beneficiaryAdd1_length ?? "").Trim() + "', "
                                + "[d_beneficiaryAdd2_start_column] = '" + (tprequest.d_beneficiaryAdd2_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryAdd2_length] = '" + (tprequest.d_beneficiaryAdd2_length ?? "").Trim() + "', "
                                + "[d_beneficiaryAdd3_start_column] = '" + (tprequest.d_beneficiaryAdd3_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryAdd3_length] = '" + (tprequest.d_beneficiaryAdd3_length ?? "").Trim() + "', "
                                + "[d_beneficiaryCountry_start_column] = '" + (tprequest.d_beneficiaryCountry_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryCountry_length] = '" + (tprequest.d_beneficiaryCountry_length ?? "").Trim() + "', "
                                + "[d_zipCode_start_column] = '" + (tprequest.d_zipCode_start_column ?? "").Trim() + "', "
                                + "[d_zipCode_length] = '" + (tprequest.d_zipCode_length ?? "").Trim() + "', "
                                + "[d_landMark_start_column] = '" + (tprequest.d_landMark_start_column ?? "").Trim() + "', "
                                + "[d_landMark_length] = '" + (tprequest.d_landMark_length ?? "").Trim() + "', "

                                + "[d_beneficiaryNationality_start_column] = '" + (tprequest.d_beneficiaryNationality_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryNationality_length] = '" + (tprequest.d_beneficiaryNationality_length ?? "").Trim() + "', "
                                + "[d_beneficiaryBDate_start_column] = '" + (tprequest.d_beneficiaryBDate_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryBDate_length] = '" + (tprequest.d_beneficiaryBDate_length ?? "").Trim() + "', "
                                + "[d_beneficiaryProf_start_column] = '" + (tprequest.d_beneficiaryProf_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryProf_length] = '" + (tprequest.d_beneficiaryProf_length ?? "").Trim() + "', "
                                + "[d_beneficiaryGender_start_column] = '" + (tprequest.d_beneficiaryGender_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryGender_length] = '" + (tprequest.d_beneficiaryGender_length ?? "").Trim() + "', "
                                + "[d_beneficiaryCivilStat_start_column] = '" + (tprequest.d_beneficiaryCivilStat_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryCivilStat_length] = '" + (tprequest.d_beneficiaryCivilStat_length ?? "").Trim() + "', "
                                + "[d_beneficiaryPOBox_start_column] = '" + (tprequest.d_beneficiaryPOBox_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryPOBox_length] = '" + (tprequest.d_beneficiaryPOBox_length ?? "").Trim() + "', "
                                + "[d_beneficiaryRel_tothe_remit_start_column] = '" + (tprequest.d_beneficiaryRel_tothe_remit_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryRel_tothe_remit_length] = '" + (tprequest.d_beneficiaryRel_tothe_remit_length ?? "").Trim() + "', "
                                + "[d_alterRecipient_reltobeneficiary_start_column] = '" + (tprequest.d_alterRecipient_reltobeneficiary_start_column ?? "").Trim() + "', "
                                + "[d_alterRecipient_reltobeneficiary_length] = '" + (tprequest.d_alterRecipient_reltobeneficiary_length ?? "").Trim() + "', "
                                + "[d_beneficiaryMobPhoneNo_start_column] = '" + (tprequest.d_beneficiaryMobPhoneNo_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryMobPhoneNo_length] = '" + (tprequest.d_beneficiaryMobPhoneNo_length ?? "").Trim() + "', "
                                + "[d_beneficiaryOfficePhoneNo_start_column] = '" + (tprequest.d_beneficiaryOfficePhoneNo_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryOfficePhoneNo_length] = '" + (tprequest.d_beneficiaryOfficePhoneNo_length ?? "").Trim() + "', "
                                + "[d_beneficiaryEmailAdd_start_column] = '" + (tprequest.d_beneficiaryEmailAdd_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryEmailAdd_length] = '" + (tprequest.d_beneficiaryEmailAdd_length ?? "").Trim() + "', "
                                + "[d_beneficiaryTaxIDNo_start_column] = '" + (tprequest.d_beneficiaryTaxIDNo_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryTaxIDNo_length] = '" + (tprequest.d_beneficiaryTaxIDNo_length ?? "").Trim() + "', "
                                + "[d_beneficiaryNotifType_start_column] = '" + (tprequest.d_beneficiaryNotifType_start_column ?? "").Trim() + "', "
                                + "[d_beneficiaryNotifType_length] = '" + (tprequest.d_beneficiaryNotifType_length ?? "").Trim() + "', "
                                + "[d_fundCurrency_start_column] = '" + (tprequest.d_fundCurrency_start_column ?? "").Trim() + "', "
                                + "[d_fundCurrency_length] = '" + (tprequest.d_fundCurrency_length ?? "").Trim() + "', "
                                + "[d_fundAmount_start_column] = '" + (tprequest.d_fundAmount_start_column ?? "").Trim() + "', "
                                + "[d_fundAmount_length] = '" + (tprequest.d_fundAmount_length ?? "").Trim() + "', "
                                + "[d_buyingRate_start_column] = '" + (tprequest.d_buyingRate_start_column ?? "").Trim() + "', "
                                + "[d_buyingRate_length] = '" + (tprequest.d_buyingRate_length ?? "").Trim() + "', "

                                + "[d_settlementCurrency_start_column] = '" + (tprequest.d_settlementCurrency_start_column ?? "").Trim() + "', "
                                + "[d_settlementCurrency_length] = '" + (tprequest.d_settlementCurrency_length ?? "").Trim() + "', "
                                + "[d_settlementAmount_start_column] = '" + (tprequest.d_settlementAmount_start_column ?? "").Trim() + "', "
                                + "[d_settlementAmount_length] = '" + (tprequest.d_settlementAmount_length ?? "").Trim() + "', "
                                + "[d_settlementMode_start_column] = '" + (tprequest.d_settlementMode_start_column ?? "").Trim() + "', "
                                + "[d_settlementMode_length] = '" + (tprequest.d_settlementMode_length ?? "").Trim() + "', "
                                + "[d_bankCode_start_column] = '" + (tprequest.d_bankCode_start_column ?? "").Trim() + "', "
                                + "[d_bankCode_length] = '" + (tprequest.d_bankCode_length ?? "").Trim() + "', "
                                + "[d_bankName_start_column] = '" + (tprequest.d_bankName_start_column ?? "").Trim() + "', "
                                + "[d_bankName_length] = '" + (tprequest.d_bankName_length ?? "").Trim() + "', "
                                + "[d_branchCode_start_column] = '" + (tprequest.d_branchCode_start_column ?? "").Trim() + "', "
                                + "[d_branchCode_length] = '" + (tprequest.d_branchCode_length ?? "").Trim() + "', "
                                + "[d_branchName_start_column] = '" + (tprequest.d_branchName_start_column ?? "").Trim() + "', "
                                + "[d_branchName_length] = '" + (tprequest.d_branchName_length ?? "").Trim() + "', "
                                + "[d_accountType_start_column] = '" + (tprequest.d_accountType_start_column ?? "").Trim() + "', "
                                + "[d_accountType_length] = '" + (tprequest.d_accountType_length ?? "").Trim() + "', "
                                + "[d_accountNo_start_column] = '" + (tprequest.d_accountNo_start_column ?? "").Trim() + "', "
                                + "[d_accountNo_length] = '" + (tprequest.d_accountNo_length ?? "").Trim() + "', "
                                + "[d_goldCardNo_start_column] = '" + (tprequest.d_goldCardNo_start_column ?? "").Trim() + "', "
                                + "[d_goldCardNo_length] = '" + (tprequest.d_goldCardNo_length ?? "").Trim() + "', "
                                + "[d_billsPayField1_start_column] = '" + (tprequest.d_billsPayField1_start_column ?? "").Trim() + "', "
                                + "[d_billsPayField1_length] = '" + (tprequest.d_billsPayField1_length ?? "").Trim() + "', "
                                + "[d_billsPayField2_start_column] = '" + (tprequest.d_billsPayField2_start_column ?? "").Trim() + "', "
                                + "[d_billsPayField2_length] = '" + (tprequest.d_billsPayField2_length ?? "").Trim() + "', "
                                + "[d_billsPayField3_start_column] = '" + (tprequest.d_billsPayField3_start_column ?? "").Trim() + "', "
                                + "[d_billsPayField3_length] = '" + (tprequest.d_billsPayField3_length ?? "").Trim() + "', "
                                + "[d_billsPayField4_start_column] = '" + (tprequest.d_billsPayField4_start_column ?? "").Trim() + "', "
                                + "[d_billsPayField4_length] = '" + (tprequest.d_billsPayField4_length ?? "").Trim() + "', "
                                + "[d_billsPayField5_start_column] = '" + (tprequest.d_billsPayField5_start_column ?? "").Trim() + "', "
                                + "[d_billsPayField5_length] = '" + (tprequest.d_billsPayField5_length ?? "").Trim() + "', "
                                + "[d_outletCode_start_column] = '" + (tprequest.d_outletCode_start_column ?? "").Trim() + "', "
                                + "[d_outletCode_length] = '" + (tprequest.d_outletCode_length ?? "").Trim() + "', "

                                + "[d_outletBranchCode_start_column] = '" + (tprequest.d_outletBranchCode_start_column ?? "").Trim() + "', "
                                + "[d_outletBranchCode_length] = '" + (tprequest.d_outletBranchCode_length ?? "").Trim() + "', "
                                + "[d_alterBeneficiaryName_start_column] = '" + (tprequest.d_alterBeneficiaryName_start_column ?? "").Trim() + "', "
                                + "[d_alterBeneficiaryName_length] = '" + (tprequest.d_alterBeneficiaryName_length ?? "").Trim() + "', "
                                + "[d_alterBeneficiary_relto_beneficiary_start_column] = '" + (tprequest.d_alterBeneficiary_relto_beneficiary_start_column ?? "").Trim() + "', "
                                + "[d_alterBeneficiary_relto_beneficiary_length] = '" + (tprequest.d_alterBeneficiary_relto_beneficiary_length ?? "").Trim() + "', "
                                + "[d_messageToBeneficiary_start_column] = '" + (tprequest.d_messageToBeneficiary_start_column ?? "").Trim() + "', "
                                + "[d_messageToBeneficiary_length] = '" + (tprequest.d_messageToBeneficiary_length ?? "").Trim() + "', "
                                + "[d_receiverCorresBank_start_column] = '" + (tprequest.d_receiverCorresBank_start_column ?? "").Trim() + "', "
                                + "[d_receiverCorresBank_length] = '" + (tprequest.d_receiverCorresBank_length ?? "").Trim() + "', "
                                + "[d_senderCorresBank_start_column] = '" + (tprequest.d_senderCorresBank_start_column ?? "").Trim() + "', "
                                + "[d_senderCorresBank_length] = '" + (tprequest.d_senderCorresBank_length ?? "").Trim() + "', "
                                + "[d_sendingBank_start_column] = '" + (tprequest.d_sendingBank_start_column ?? "").Trim() + "', "
                                + "[d_sendingBank_length] = '" + (tprequest.d_sendingBank_length ?? "").Trim() + "', "
                                + "[d_receivingBank_start_column] = '" + (tprequest.d_receivingBank_start_column ?? "").Trim() + "', "
                                + "[d_receivingBank_length] = '" + (tprequest.d_receivingBank_length ?? "").Trim() + "', "
                                + "[d_modeOfChange_start_column] = '" + (tprequest.d_modeOfChange_start_column ?? "").Trim() + "', "
                                + "[d_modeOfChange_length] = '" + (tprequest.d_modeOfChange_length ?? "").Trim() + "', "
                                + "[d_purposeCode_start_column] = '" + (tprequest.d_purposeCode_start_column ?? "").Trim() + "', "
                                + "[d_purposeCode_length] = '" + (tprequest.d_purposeCode_length ?? "").Trim() + "', "
                                + "[d_indvCode_start_column] = '" + (tprequest.d_indvCode_start_column ?? "").Trim() + "', "
                                + "[d_indvCode_length] = '" + (tprequest.d_indvCode_length ?? "").Trim() + "', "
                                + "[d_prefix] = '" + (tprequest.d_prefix ?? "") + "', "


                                //---- Defaul Data -----
                               + "[applicationNumber] = '" + (tprequest.applicationNumber ?? "").Trim() + "', "
                                + "[settlementMode] = '" + (tprequest.settlementMode ?? "").Trim() + "', "
                                + "[fundingAmount] = '" + (tprequest.fundingAmount ?? "").Trim() + "', "
                                + "[fundingCurrency] = '" + (tprequest.fundingCurrency ?? "").Trim() + "', "
                                + "[remitterFirstName] = '" + (tprequest.remitterFirstName ?? "").Trim() + "', "
                                + "[beneficiaryFirstName] = '" + (tprequest.beneficiaryFirstName ?? "").Trim() + "', "
                                + "[accountNumber] = '" + (tprequest.accountNumber ?? "").Trim() + "', "
                                + "[bankCode] = '" + (tprequest.bankCode ?? "").Trim() + "', "
                                + "[beneficiaryAddress1] = '" + (tprequest.beneficiaryAddress1 ?? "").Trim() + "', "
                                + "[remitterNationality] = '" + (tprequest.remitterNationality ?? "").Trim() + "', "
                                + "[remitterAddress1] = '" + (tprequest.remitterAddress1 ?? "").Trim() + "', "
                                //+ "[remitterCountry] = '" + (tprequest.remitterCountry ?? "").Trim() + "', "
                                //+ "[remitterZipCode] = '" + (tprequest.remitterZipCode ?? "").Trim() + "', "
                                + "[remitterBirthDate] = '" + (tprequest.remitterBirthDate ?? "").Trim() + "', "
                                + "[remitterMobileNumber] = '" + (tprequest.remitterMobileNumber ?? "").Trim() + "', "
                                + "[remitterIdType1] = '" + (tprequest.remitterIdType1 ?? "").Trim() + "', "
                                + "[remitterIdNumber1] = '" + (tprequest.remitterIdNumber1 ?? "").Trim() + "', "
                                + "[remitterIdIssuedAt1] = '" + (tprequest.remitterIdIssuedAt1 ?? "").Trim() + "', "
                                + "[remitterIdExpiry1] = '" + (tprequest.remitterIdExpiry1 ?? "").Trim() + "', "
                                + "[beneficiaryMiddleName] = '" + (tprequest.beneficiaryMiddleName ?? "").Trim() + "', "
                                + "[beneficiaryLastName] = '" + (tprequest.beneficiaryLastName ?? "").Trim() + "', "
                                + "[beneficiaryCustomerType] = '" + (tprequest.beneficiaryCustomerType ?? "").Trim() + "', "
                                + "[beneficiaryZipCode] = '" + (tprequest.beneficiaryZipCode ?? "").Trim() + "', "
                                + "[beneficiaryNationality] = '" + (tprequest.beneficiaryNationality ?? "").Trim() + "', "
                                + "[beneficiaryBirthDate] = '" + (tprequest.beneficiaryBirthDate ?? "").Trim() + "', "
                                + "[beneficiaryRelationToRemitter] = '" + (tprequest.beneficiaryRelationToRemitter ?? "").Trim() + "', "
                                + "[beneficiaryMobileNumber] = '" + (tprequest.beneficiaryMobileNumber ?? "").Trim() + "', "
                                + "[buyingRate] = '" + (tprequest.buyingRate ?? "").Trim() + "', "
                                + "[settlementCurrency] = '" + (tprequest.settlementCurrency ?? "").Trim() + "', "
                                + "[paymentField1] = '" + (tprequest.paymentField1 ?? "").Trim() + "', "
                                + "[paymentField2] = '" + (tprequest.paymentField2 ?? "").Trim() + "', "
                                + "[paymentField3] = '" + (tprequest.paymentField3 ?? "").Trim() + "', "
                                + "[outletCode] = '" + (tprequest.outletCode ?? "").Trim() + "', "
                                //+ "[senderCorrespondentBank] = '" + (tprequest.senderCorrespondentBank ?? "").Trim() + "', "
                                //+ "[sendingBank] = '" + (tprequest.sendingBank ?? "").Trim() + "', "
                                //+ "[receivingBank] = '" + (tprequest.receivingBank ?? "").Trim() + "', "
                                //+ "[modeOfCharge] = '" + (tprequest.modeOfCharge ?? "").Trim() + "', "
                                //+ "[purposeCode] = '" + (tprequest.purposeCode ?? "").Trim() + "', "
                                //+ "[individualCode] = '" + (tprequest.individualCode ?? "").Trim() + "', "
                                //+ "[natureOfBusiness] = '" + (tprequest.natureOfBusiness ?? "").Trim() + "', "
                                + "[remitterId] = '" + (tprequest.remitterId ?? "").Trim() + "', "
                                + "[remitterMiddleName] = '" + (tprequest.remitterMiddleName ?? "").Trim() + "', "
                                + "[remitterLastName] = '" + (tprequest.remitterLastName ?? "").Trim() + "', "
                                + "[remitterCustomerType] = '" + (tprequest.remitterCustomerType ?? "").Trim() + "', "
                                + "[settlementAmount] = '" + (tprequest.settlementAmount ?? "").Trim() + "', "

                                + "[remitterAddress2] = '" + (tprequest.remitterAddress2 ?? "").Trim() + "', "
                                + "[remitterAddress3] = '" + (tprequest.remitterAddress3 ?? "").Trim() + "', "
                                + "[remitterAddress4] = '" + (tprequest.remitterAddress4 ?? "").Trim() + "', "
                                //+ "[remitterContinent] = '" + (tprequest.remitterContinent ?? "").Trim() + "', "
                                //+ "[remitterProfession] = '" + (tprequest.remitterProfession ?? "").Trim() + "', "
                                //+ "[remitterGender] = '" + (tprequest.remitterGender ?? "").Trim() + "', "
                                //+ "[remitterCivilStatus] = '" + (tprequest.remitterCivilStatus ?? "").Trim() + "', "
                                //+ "[remitterPoBox] = '" + (tprequest.remitterPoBox ?? "").Trim() + "', "
                                //+ "[remitterOfficeNumber] = '" + (tprequest.remitterOfficeNumber ?? "").Trim() + "', "
                                //+ "[remitterEmail] = '" + (tprequest.remitterEmail ?? "").Trim() + "', "
                                //+ "[remitterTin] = '" + (tprequest.remitterTin ?? "").Trim() + "', "
                                //+ "[remitterIdType2] = '" + (tprequest.remitterIdType2 ?? "").Trim() + "', "
                                //+ "[remitterIdNumber2] = '" + (tprequest.remitterIdNumber2 ?? "").Trim() + "', "
                                //+ "[remitterIdIssuedAt2] = '" + (tprequest.remitterIdIssuedAt2 ?? "").Trim() + "', "
                                //+ "[remitterIdExpiry2] = '" + (tprequest.remitterIdExpiry2 ?? "").Trim() + "', "
                                //+ "[remitterNotificationType] = '" + (tprequest.remitterNotificationType ?? "").Trim() + "', "
                                //+ "[beneficiaryId] = '" + (tprequest.beneficiaryId ?? "").Trim() + "', "
                                + "[beneficiaryAddress2] = '" + (tprequest.beneficiaryAddress2 ?? "").Trim() + "', "
                                + "[beneficiaryAddress3] = '" + (tprequest.beneficiaryAddress3 ?? "").Trim() + "', "
                                + "[beneficiaryCountry] = '" + (tprequest.beneficiaryCountry ?? "").Trim() + "', "
                                //+ "[beneficiaryLandmark] = '" + (tprequest.beneficiaryLandmark ?? "").Trim() + "', "
                                //+ "[beneficiaryProfession] = '" + (tprequest.beneficiaryProfession ?? "").Trim() + "', "
                                //+ "[beneficiaryGender] = '" + (tprequest.beneficiaryGender ?? "").Trim() + "', "
                                //+ "[beneficiaryCivilStatus] = '" + (tprequest.beneficiaryCivilStatus ?? "").Trim() + "', "
                                //+ "[beneficiaryPoBox] = '" + (tprequest.beneficiaryPoBox ?? "").Trim() + "', "
                                //+ "[alternateRecipientRelationToBeneficiary] = '" + (tprequest.alternateRecipientRelationToBeneficiary ?? "").Trim() + "', "
                                //+ "[beneficiaryOfficeNumber] = '" + (tprequest.beneficiaryOfficeNumber ?? "").Trim() + "', "
                                + "[beneficiaryEmail] = '" + (tprequest.beneficiaryEmail ?? "").Trim() + "', "
                                //+ "[beneficiaryTin] = '" + (tprequest.beneficiaryTin ?? "").Trim() + "', "
                                //+ "[beneficiaryNotificationType] = '" + (tprequest.beneficiaryNotificationType ?? "").Trim() + "', "
                                + "[transactionDate] = '" + (tprequest.transactionDate ?? "").Trim() + "', "
                                + "[bankName] = '" + (tprequest.bankName ?? "").Trim() + "', "
                                //+ "[branchName] = '" + (tprequest.branchName ?? "").Trim() + "', "
                                //+ "[accountType] = '" + (tprequest.accountType ?? "").Trim() + "', "
                                //+ "[goldCardNumber] = '" + (tprequest.goldCardNumber ?? "").Trim() + "', "
                                + "[paymentField4] = '" + (tprequest.paymentField4 ?? "").Trim() + "', "
                                + "[paymentField5] = '" + (tprequest.paymentField5 ?? "").Trim() + "', "
                                + "[outletBranchCode] = '" + (tprequest.outletBranchCode ?? "").Trim() + "', "
                                //+ "[alternateBeneficiary] = '" + (tprequest.alternateBeneficiary ?? "").Trim() + "', "
                                //+ "[alternateBeneficiaryRelationToBeneficiary] = '" + (tprequest.alternateBeneficiaryRelationToBeneficiary ?? "").Trim() + "', "
                                //+ "[messageToBeneficiary] = '" + (tprequest.messageToBeneficiary ?? "").Trim() + "', "
                                //+ "[receiverCorrespondentBank] = '" + (tprequest.receiverCorrespondentBank ?? "").Trim() + "', "
                                + "[branchCode] = '" + (tprequest.branchCode ?? "").Trim() + "' "


                                //+ ", [partnerCode_start_column] = '" + (tprequest.partnerCode_start_column ?? "").Trim() + "' "
                                //+ ", [partnerCode_length] = '" + (tprequest.partnerCode_length ?? "").Trim() + "' "
                                //+ ", [partnerCode_default] = '" + (tprequest.partnerCode_default ?? "").Trim() + "' "
                                + ", [acctName_start_column] = '" + (tprequest.acctName_start_column ?? "").Trim() + "' "
                                + ", [acctName_length] = '" + (tprequest.acctName_length ?? "").Trim() + "' "
                                + ", [acctName_default] = '" + (tprequest.acctName_default ?? "").Trim() + "' "
                                + ", [remCountry_start_column] = '" + (tprequest.remCountry_start_column ?? "").Trim() + "' "
                                + ", [remCountry_length] = '" + (tprequest.remCountry_length ?? "").Trim() + "' "
                                + ", [remCountry_default] = '" + (tprequest.remCountry_default ?? "").Trim() + "' "
                                + ", [remFourthNme_start_column] = '" + (tprequest.remFourthNme_start_column ?? "").Trim() + "' "
                                + ", [remFourthNme_length] = '" + (tprequest.remFourthNme_length ?? "").Trim() + "' "
                                + ", [remFourthNme_default] = '" + (tprequest.remFourthNme_default ?? "").Trim() + "' "
                                + ", [remPlaceBrth_start_column] = '" + (tprequest.remPlaceBrth_start_column ?? "").Trim() + "' "
                                + ", [remPlaceBrth_length] = '" + (tprequest.remPlaceBrth_length ?? "").Trim() + "' "
                                + ", [remPlaceBrth_default] = '" + (tprequest.remPlaceBrth_default ?? "").Trim() + "' "
                                + ", [remAcctNum_start_column] = '" + (tprequest.remAcctNum_start_column ?? "").Trim() + "' "
                                + ", [remAcctNum_length] = '" + (tprequest.remAcctNum_length ?? "").Trim() + "' "
                                + ", [remAcctNum_default] = '" + (tprequest.remAcctNum_default ?? "").Trim() + "' "
                                + ", [remIBAN_start_column] = '" + (tprequest.remIBAN_start_column ?? "").Trim() + "' "
                                + ", [remIBAN_length] = '" + (tprequest.remIBAN_length ?? "").Trim() + "' "
                                + ", [remIBAN_default] = '" + (tprequest.remIBAN_default ?? "").Trim() + "' "
                                + ", [res1_start_column] = '" + (tprequest.res1_start_column ?? "").Trim() + "' "
                                + ", [res1_length] = '" + (tprequest.res1_length ?? "").Trim() + "' "
                                + ", [res1_default] = '" + (tprequest.res1_default ?? "").Trim() + "' "
                                + ", [res2_start_column] = '" + (tprequest.res2_start_column ?? "").Trim() + "' "
                                + ", [res2_length] = '" + (tprequest.res2_length ?? "").Trim() + "' "
                                + ", [res2_default] = '" + (tprequest.res2_default ?? "").Trim() + "' "
                                + ", [res3_start_column] = '" + (tprequest.res3_start_column ?? "").Trim() + "' "
                                + ", [res3_length] = '" + (tprequest.res3_length ?? "").Trim() + "' "
                                + ", [res3_default] = '" + (tprequest.res3_default ?? "").Trim() + "' "
                                + ", [res4_start_column] = '" + (tprequest.res4_start_column ?? "").Trim() + "' "
                                + ", [res4_length] = '" + (tprequest.res4_length ?? "").Trim() + "' "
                                + ", [res4_default] = '" + (tprequest.res4_default ?? "").Trim() + "' "
                                + ", [res5_start_column] = '" + (tprequest.res5_start_column ?? "").Trim() + "' "
                                + ", [res5_length] = '" + (tprequest.res5_length ?? "").Trim() + "' "
                                + ", [res5_default] = '" + (tprequest.res5_default ?? "").Trim() + "' "
                                + ", [res6_start_column] = '" + (tprequest.res6_start_column ?? "").Trim() + "' "
                                + ", [res6_length] = '" + (tprequest.res6_length ?? "").Trim() + "' "
                                + ", [res6_default] = '" + (tprequest.res6_default ?? "").Trim() + "' "
                                + ", [res7_start_column] = '" + (tprequest.res7_start_column ?? "").Trim() + "' "
                                + ", [res7_length] = '" + (tprequest.res7_length ?? "").Trim() + "' "
                                + ", [res7_default] = '" + (tprequest.res7_default ?? "").Trim() + "' "
                                + ", [res8_start_column] = '" + (tprequest.res8_start_column ?? "").Trim() + "' "
                                + ", [res8_length] = '" + (tprequest.res8_length ?? "").Trim() + "' "
                                + ", [res8_default] = '" + (tprequest.res8_default ?? "").Trim() + "' "
                                + ", [res9_start_column] = '" + (tprequest.res9_start_column ?? "").Trim() + "' "
                                + ", [res9_length] = '" + (tprequest.res9_length ?? "").Trim() + "' "
                                + ", [res9_default] = '" + (tprequest.res9_default ?? "").Trim() + "' "
                                + ", [res10_start_column] = '" + (tprequest.res10_start_column ?? "").Trim() + "' "
                                + ", [res10_length] = '" + (tprequest.res10_length ?? "").Trim() + "' "
                                + ", [res10_default] = '" + (tprequest.res10_default ?? "").Trim() + "' "
                                + ", [sourceOfRem_start_column] = '" + (tprequest.sourceOfRem_start_column ?? "").Trim() + "' "
                                + ", [sourceOfRem_length] = '" + (tprequest.sourceOfRem_length ?? "").Trim() + "' "
                                + ", [sourceOfRem_default] = '" + (tprequest.sourceOfRem_default ?? "").Trim() + "' "
                                + ", [natrueOfBuss_start_column] = '" + (tprequest.natrueOfBuss_start_column ?? "").Trim() + "' "
                                + ", [natrueOfBuss_length] = '" + (tprequest.natrueOfBuss_length ?? "").Trim() + "' "
                                + ", [natrueOfBuss_default] = '" + (tprequest.natrueOfBuss_default ?? "").Trim() + "' "
                                + ", [purposeOfRem_start_column] = '" + (tprequest.purposeOfRem_start_column ?? "").Trim() + "' "
                                + ", [purposeOfRem_length] = '" + (tprequest.purposeOfRem_length ?? "").Trim() + "' "
                                + ", [purposeOfRem_default] = '" + (tprequest.purposeOfRem_default ?? "").Trim() + "' "

                                + "WHERE tieup_code_id = '" + tprequest.tieup_codes + "'";

                                TieupCodesDetail = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Detail>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_Detail", g_global_region_code));
                                formViewModel1.Tieup_Codes_Detail = TieupCodesDetail;

                                Debug.WriteLine("REASONNNNNN: " + fc["TieupViewMaintInputs.remarks"]);
                                var tieupDetail1 = new VREMIT.ModelMVC.Tieup_Codes_Request()
                                {
                                    pk_tieup_code_id = tprequest.pk_tieup_code_id,
                                    file_type = tprequest.file_type,
                                    req_status_reason = fc["TieupViewMaintInputs.remarks"],
                                    status = ModelViewer.REQ_STATUS_APPROVED.ToString(),
                                    req_status = ModelViewer.REQ_STATUS_APPROVED,
                                    req_type = ModelViewer.REQ_TYPE_MODIFY,
                                    dt_last_chg = DateTime.Now,

                                    rt_action = ModelViewer.USER_ACTION_APPROVE,
                                    rt_terminal_id = base._userClientIP,
                                    rt_user_agent = base._userAgent,
                                    rt_username = _currentUser.Username
                                };

                                ModelResponse modelResponseApprove = dal.GenericUpdate(tieupDetail1, "Tieup_Codes_Request", g_global_region_code);

                                if (modelResponseApprove.code == 0)
                                {
                                    TempData["title"] = "Approved!";
                                    TempData["text"] = "Amendment request has been successfully approved.";
                                    TempData["icon"] = "success";
                                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else if (tprequest.req_type == ModelViewer.REQ_TYPE_DELETE) //Approved for Deletion
                            {

                                List<Tieup_Codes> Tieup_Codes = new List<Tieup_Codes>();
                                sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
                                customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes();
                                customsqlCode3.sql = "DELETE FROM tbl_tieup_codes where tieup_codes = '" + tprequest.tieup_codes + "'";
                                Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes", g_global_region_code));
                                formViewModel1.TieupCodesMaintParam = Tieup_Codes;

                                List<Tieup_Codes_Detail> TieupCodesDetail = new List<Tieup_Codes_Detail>();
                                sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
                                customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_Detail();
                                customsqlCode2.sql = "DELETE FROM tbl_tieup_codes_detail where tieup_code_id = '" + tprequest.tieup_codes + "' ";
                                TieupCodesDetail = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Detail>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_Detail", g_global_region_code));
                                formViewModel1.Tieup_Codes_Detail = TieupCodesDetail;


                                var tieupDetail1 = new VREMIT.ModelMVC.Tieup_Codes_Request()
                                {
                                    pk_tieup_code_id = tprequest.pk_tieup_code_id,
                                    file_type = tprequest.file_type,
                                    req_status_reason = fc["TieupViewMaintInputs.remarks"],
                                    req_status = ModelViewer.USER_STATUS_DELETED,
                                    req_type = ModelViewer.REQ_TYPE_DELETE,
                                    status = ModelViewer.USER_STATUS_DELETED.ToString(),
                                    dt_last_chg = DateTime.Now,

                                    rt_action = ModelViewer.USER_ACTION_APPROVE,
                                    rt_terminal_id = base._userClientIP,
                                    rt_user_agent = base._userAgent,
                                    rt_username = _currentUser.Username,
                                };

                                ModelResponse modelResponseApprove = dal.GenericUpdate(tieupDetail1, "Tieup_Codes_Request", g_global_region_code);

                                if (modelResponseApprove.code == 0)
                                {
                                    //TempData["title"] = "Approved!";
                                    //TempData["text"] = "Deletion request has been successfully approved";
                                    //TempData["icon"] = "success";
                                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                    else if (valid == 3) //Reject
                    {
                        try
                        {
                            //get data first
                            //var m = new VREMIT.ModelMVC.Tieup_Codes_Request();

                            tprequest.pk_tieup_code_id = id;
                            tprequest = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes_Request>(dal.GetGenericDetails(tprequest, "Tieup_Codes_Request", g_global_region_code));

                            if (tprequest == null)
                            {
                                return Json("N_OK", JsonRequestBehavior.AllowGet);
                            }


                            List<Tieup_Codes> Tieup_Codes = new List<Tieup_Codes>();
                            sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
                            customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes();
                            customsqlCode3.sql = "UPDATE tbl_tieup_codes SET "
                                                + "status = 0 "
                                                + "where tieup_codes = '" + tprequest.tieup_codes + "' AND tieup_code_id = '" + id + "' ";
                            Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes", g_global_region_code));
                            formViewModel1.TieupCodesMaintParam = Tieup_Codes;

                            List<Tieup_Codes_Detail> TieupCodesDetail = new List<Tieup_Codes_Detail>();
                            sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
                            customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_Detail();
                            customsqlCode2.sql = "UPDATE tbl_tieup_codes_detail SET "
                                                + "status = 0 "
                                                + "where tieup_code_id = '" + tprequest.tieup_codes + "' AND tieup_code_id = '" + id + "' ";
                            TieupCodesDetail = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Detail>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_Detail", g_global_region_code));
                            formViewModel1.Tieup_Codes_Detail = TieupCodesDetail;


                            tprequest.req_status = ModelViewer.REQ_STATUS_REJECTED;
                            tprequest.status = "1";
                            tprequest.rt_action = ModelViewer.USER_ACTION_REJECT;
                            tprequest.rt_terminal_id = base._userClientIP;
                            tprequest.rt_user_agent = base._userAgent;
                            tprequest.rt_username = _currentUser.Username;

                            tprequest.req_status_reason = fc["TieupViewMaintInputs.remarks"];
                            ModelResponse modelResponse = dal.GenericUpdate(tprequest, "Tieup_Codes_Request", g_global_region_code);

                            if (modelResponse.code == 0)
                            {
                                if (tprequest.req_status == 2 && tprequest.req_type == 2 && tprequest.status == "1") //value changes in db by the time it executes from "ModelResponse"
                                {
                                    //TempData["title"] = "Rejected!";
                                    //TempData["text"] = "Amendment request has been rejected.";
                                    //TempData["icon"] = "success";
                                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                                }
                                else if (tprequest.status == "1" && tprequest.req_status == 2 && tprequest.req_type == 1)
                                {
                                    //TempData["title"] = "Rejected!";
                                    //TempData["text"] = "New request has been rejected.";
                                    //TempData["icon"] = "success";
                                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                                }
                                else if (tprequest.req_type == 3)
                                {
                                    tprequest.status = "1"; // to achieve condition before update
                                    //TempData["title"] = "Rejected!";
                                    //TempData["text"] = "Deletion request has been rejected.";
                                    //TempData["icon"] = "success";
                                    return Json(new { title = TempData["title"], text = TempData["text"], icon = TempData["icon"] }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else if (modelResponse.code != 0)
                            {
                                return Json("N_OK", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Failed to update bank");

                                return Json("", JsonRequestBehavior.AllowGet);
                            }


                        }
                        catch (Exception ex)
                        {

                            LOGGER.Error(ex.ToString(), ex);

                            return Json("N_OK", JsonRequestBehavior.AllowGet);
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                LOGGER.Error(ex.ToString(), ex);

                ModelState.AddModelError("", "An error encountered while processing request");

            }

            TempData["title"] = "Approved!";
            TempData["text"] = "Format mapper request has been successfully approved.";
            TempData["icon"] = "success";

            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult MaintDelete(string i, string remarks)
        {
            try
            {
                VREMIT.ModelMVC.Tieup_Codes m = new VREMIT.ModelMVC.Tieup_Codes();
                m.pk_tieup_code_id = i;

                m = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes>(dal.GetGenericDetails(m, "Tieup_Codes", g_global_region_code));

                if (m != null)
                {
                    //get data first
                    DARS.Web.Models.FormatMapperFormViewModel formViewModel = new DARS.Web.Models.FormatMapperFormViewModel();

                    //main table - [tbl_tieup_codes]
                    VREMIT.ModelMVC.Tieup_Codes tprequest = new VREMIT.ModelMVC.Tieup_Codes();
                    tprequest.pk_tieup_code_id = i;
                    tprequest = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes>(dal.GetGenericDetails(tprequest, "Tieup_Codes", g_global_region_code));

                    //detail table - tbl_tieup_codes_detail
                    VREMIT.ModelMVC.Tieup_Codes_Detail mainDetail = new VREMIT.ModelMVC.Tieup_Codes_Detail();
                    mainDetail.pk_detail_tieup_id = i;
                    mainDetail = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes_Detail>(dal.GetGenericDetails(mainDetail, "Tieup_Codes_Detail", g_global_region_code));

                    string tranID = Common.GetgenerateID(1, Convert.ToString(ModelViewer.TIEUP_FILE_MAPPER), g_global_region_code); //Bank Code TIEUP_FILE_MAPPER

                    var deleteReq = new VREMIT.ModelMVC.Tieup_Codes_Request
                    {
                        pk_tieup_code_id = tranID,
                        tieup_codes = tprequest.tieup_codes, //Tieup Partner
                        //req_status_reason = tprequest.remarks,
                        status = Convert.ToString(ModelViewer.REQ_STATUS_APPROVED),

                        rt_action = ModelViewer.USER_ACTION_DELETE,
                        rt_terminal_id = base._userClientIP,
                        rt_user_agent = base._userAgent,
                        rt_username = _currentUser.Username,

                        remarks = remarks,
                        username = _currentUser.Username,
                        req_type = ModelViewer.REQ_TYPE_DELETE,
                        req_status = ModelViewer.REQ_STATUS_PENDING,
                        dt_last_chg = DateTime.Now,
                        file_type = tprequest.file_type,
                        filename_validation = tprequest.filename_validation,
                        footer_validation = tprequest.footer_validation,
                        settlement_currency = tprequest.settlement_currency,
                        start_record = tprequest.start_record,

                        provided = mainDetail.provided,
                        delimiter_val = mainDetail.delimiter_val,
                        d_prefix = mainDetail.d_prefix,
                        delimiter_type = mainDetail.delimiter_type,
                        delimiter = mainDetail.delimiter,
                        detail_start_record = mainDetail.detail_start_record,
                        detail_transdate_start_column = mainDetail.detail_transdate_start_column,
                        detail_transdate_length = mainDetail.detail_transdate_length,
                        detail_application_start_column = mainDetail.detail_application_start_column,
                        detail_application_length = mainDetail.detail_application_length,
                        detail_remitid_start_column = mainDetail.detail_remitid_start_column,
                        detail_remitid_length = mainDetail.detail_remitid_length,
                        detail_remitfname_start_column = mainDetail.detail_remitfname_start_column,
                        detail_remitfname_length = mainDetail.detail_remitfname_length,
                        detail_remitmidname_start_column = mainDetail.detail_remitmidname_start_column,
                        detail_remitmidname_length = mainDetail.detail_remitmidname_length,
                        detail_remitlastname_start_column = mainDetail.detail_remitlastname_start_column,
                        detail_remitlastname_length = mainDetail.detail_remitlastname_length,
                        d_remitCusType_start_column = mainDetail.d_remitCusType_start_column,//
                        d_remitCusType_length = mainDetail.d_remitCusType_length,
                        d_remitNationality_start_column = mainDetail.d_remitNationality_start_column,//
                        d_remitNationality_length = mainDetail.d_remitNationality_length,
                        d_remitAdd1_start_column = mainDetail.d_remitAdd1_start_column,
                        d_remitAdd1_length = mainDetail.d_remitAdd1_length,
                        d_remitAdd2_start_column = mainDetail.d_remitAdd2_start_column,
                        d_remitAdd2_length = mainDetail.d_remitAdd2_length,
                        d_remitAdd3_start_column = mainDetail.d_remitAdd3_start_column,
                        d_remitAdd3_length = mainDetail.d_remitAdd3_length,
                        d_remitAdd4_start_column = mainDetail.d_remitAdd4_start_column,
                        d_remitAdd4_length = mainDetail.d_remitAdd4_length,
                        d_remitContinent_start_column = mainDetail.d_remitContinent_start_column,
                        d_remitContinent_length = mainDetail.d_remitContinent_length,
                        d_remitZipCode_start_column = mainDetail.d_remitZipCode_start_column,
                        d_remitZipCode_length = mainDetail.d_remitZipCode_length,
                        d_remitBDate_start_column = mainDetail.d_remitBDate_start_column,
                        d_remitBDate_length = mainDetail.d_remitBDate_length,
                        d_remitProf_start_column = mainDetail.d_remitProf_start_column,
                        d_remitProf_length = mainDetail.d_remitProf_length,
                        d_remitGender_start_column = mainDetail.d_remitGender_start_column,
                        d_remitGender_length = mainDetail.d_remitGender_length,//
                        d_remitCivilStat_start_column = mainDetail.d_remitCivilStat_start_column,
                        d_remitCivilStat_length = mainDetail.d_remitCivilStat_length,
                        d_remitPOBox_start_column = mainDetail.d_remitPOBox_start_column,
                        d_remitPOBox_length = mainDetail.d_remitPOBox_length,//
                        d_remitMobPhoneNum_start_column = mainDetail.d_remitMobPhoneNum_start_column,
                        d_remitMobPhoneNum_length = mainDetail.d_remitMobPhoneNum_length,
                        d_remitOfficePhoneNo_start_column = mainDetail.d_remitOfficePhoneNo_start_column,
                        d_remitOfficePhoneNo_length = mainDetail.d_remitOfficePhoneNo_length,
                        d_remitEmailAdd_start_column = mainDetail.d_remitEmailAdd_start_column,
                        d_remitEmailAdd_length = mainDetail.d_remitEmailAdd_length,
                        d_remitTaxIDNo_start_column = mainDetail.d_remitTaxIDNo_start_column,
                        d_remitTaxIDNo_length = mainDetail.d_remitTaxIDNo_length,
                        d_remitIDType1_start_column = mainDetail.d_remitIDType1_start_column,
                        d_remitIDType1_length = mainDetail.d_remitIDType1_length,
                        d_remitIDNum1_start_column = mainDetail.d_remitIDNum1_start_column,
                        d_remitIDNum1_length = mainDetail.d_remitIDNum1_length,
                        d_remitIDIssueAt1_start_column = mainDetail.d_remitIDIssueAt1_start_column,
                        d_remitIDIssueAt1_length = mainDetail.d_remitIDIssueAt1_length,
                        d_remitIDExpDate1_start_column = mainDetail.d_remitIDExpDate1_start_column,
                        d_remitIDExpDate1_length = mainDetail.d_remitIDExpDate1_length,
                        d_remitIDType2_start_column = mainDetail.d_remitIDType2_start_column,
                        d_remitIDType2_length = mainDetail.d_remitIDType2_length,
                        d_remitIDNo2_start_column = mainDetail.d_remitIDNo2_start_column,
                        d_remitIDNo2_length = mainDetail.d_remitIDNo2_length,
                        d_remitIDIssueAt2_start_column = mainDetail.d_remitIDIssueAt2_start_column,
                        d_remitIDIssueAt2_length = mainDetail.d_remitIDIssueAt2_length,
                        d_remitIDExpDate2_start_column = mainDetail.d_remitIDExpDate2_start_column,
                        d_remitIDExpDate2_length = mainDetail.d_remitIDExpDate2_length,
                        d_remitNotifType_start_column = mainDetail.d_remitNotifType_start_column,
                        d_remitNotifType_length = mainDetail.d_remitNotifType_length,
                        d_beneficiaryID_start_column = mainDetail.d_beneficiaryID_start_column,
                        d_beneficiaryID_length = mainDetail.d_beneficiaryID_length,
                        d_beneficiaryFName_start_column = mainDetail.d_beneficiaryFName_start_column,
                        d_beneficiaryFName_length = mainDetail.d_beneficiaryFName_length,
                        d_beneficiaryMName_start_column = mainDetail.d_beneficiaryMName_start_column,
                        d_beneficiaryMName_length = mainDetail.d_beneficiaryMName_length,
                        d_beneficiaryLName_start_column = mainDetail.d_beneficiaryLName_start_column,
                        d_beneficiaryLName_length = mainDetail.d_beneficiaryLName_length,
                        d_beneficiaryCusType_start_column = mainDetail.d_beneficiaryCusType_start_column,
                        d_beneficiaryCusType_length = mainDetail.d_beneficiaryCusType_length,
                        d_beneficiaryAdd1_start_column = mainDetail.d_beneficiaryAdd1_start_column,
                        d_beneficiaryAdd1_length = mainDetail.d_beneficiaryAdd1_length,//
                        d_beneficiaryAdd2_start_column = mainDetail.d_beneficiaryAdd2_start_column,
                        d_beneficiaryAdd2_length = mainDetail.d_beneficiaryAdd2_length,//
                        d_beneficiaryAdd3_start_column = mainDetail.d_beneficiaryAdd3_start_column,
                        d_beneficiaryAdd3_length = mainDetail.d_beneficiaryAdd3_length,//
                        d_beneficiaryCountry_start_column = mainDetail.d_beneficiaryCountry_start_column,
                        d_beneficiaryCountry_length = mainDetail.d_beneficiaryCountry_length,
                        d_zipCode_start_column = mainDetail.d_zipCode_start_column,
                        d_zipCode_length = mainDetail.d_zipCode_length,
                        d_landMark_start_column = mainDetail.d_landMark_start_column,
                        d_landMark_length = mainDetail.d_landMark_length,
                        d_beneficiaryNationality_start_column = mainDetail.d_beneficiaryNationality_start_column,
                        d_beneficiaryNationality_length = mainDetail.d_beneficiaryNationality_length,
                        d_beneficiaryBDate_start_column = mainDetail.d_beneficiaryBDate_start_column,
                        d_beneficiaryBDate_length = mainDetail.d_beneficiaryBDate_length,
                        d_beneficiaryProf_start_column = mainDetail.d_beneficiaryProf_start_column,
                        d_beneficiaryProf_length = mainDetail.d_beneficiaryProf_length,
                        d_beneficiaryGender_start_column = mainDetail.d_beneficiaryGender_start_column,
                        d_beneficiaryGender_length = mainDetail.d_beneficiaryGender_length,
                        d_beneficiaryCivilStat_start_column = mainDetail.d_beneficiaryCivilStat_start_column,
                        d_beneficiaryCivilStat_length = mainDetail.d_beneficiaryCivilStat_length,
                        d_beneficiaryPOBox_start_column = mainDetail.d_beneficiaryPOBox_start_column,
                        d_beneficiaryPOBox_length = mainDetail.d_beneficiaryPOBox_length,
                        d_beneficiaryRel_tothe_remit_start_column = mainDetail.d_beneficiaryRel_tothe_remit_start_column,
                        d_beneficiaryRel_tothe_remit_length = mainDetail.d_beneficiaryRel_tothe_remit_length,
                        d_alterRecipient_reltobeneficiary_start_column = mainDetail.d_alterRecipient_reltobeneficiary_start_column,
                        d_alterRecipient_reltobeneficiary_length = mainDetail.d_alterRecipient_reltobeneficiary_length,
                        d_beneficiaryMobPhoneNo_start_column = mainDetail.d_beneficiaryMobPhoneNo_start_column,
                        d_beneficiaryMobPhoneNo_length = mainDetail.d_beneficiaryMobPhoneNo_length,
                        d_beneficiaryOfficePhoneNo_start_column = mainDetail.d_beneficiaryOfficePhoneNo_start_column,
                        d_beneficiaryOfficePhoneNo_length = mainDetail.d_beneficiaryOfficePhoneNo_length,
                        d_beneficiaryEmailAdd_start_column = mainDetail.d_beneficiaryEmailAdd_start_column,
                        d_beneficiaryEmailAdd_length = mainDetail.d_beneficiaryEmailAdd_length,//
                        d_beneficiaryTaxIDNo_start_column = mainDetail.d_beneficiaryTaxIDNo_start_column,
                        d_beneficiaryTaxIDNo_length = mainDetail.d_beneficiaryTaxIDNo_length,//
                        d_beneficiaryNotifType_start_column = mainDetail.d_beneficiaryNotifType_start_column,
                        d_beneficiaryNotifType_length = mainDetail.d_beneficiaryNotifType_length,
                        d_fundCurrency_start_column = mainDetail.d_fundCurrency_start_column,
                        d_fundCurrency_length = mainDetail.d_fundCurrency_length,
                        d_fundAmount_start_column = mainDetail.d_fundAmount_start_column,
                        d_fundAmount_length = mainDetail.d_fundAmount_length,
                        d_buyingRate_start_column = mainDetail.d_buyingRate_start_column,
                        d_buyingRate_length = mainDetail.d_buyingRate_length,
                        d_settlementCurrency_start_column = mainDetail.d_settlementCurrency_start_column,
                        d_settlementCurrency_length = mainDetail.d_settlementCurrency_length,
                        d_settlementAmount_start_column = mainDetail.d_settlementAmount_start_column,
                        d_settlementAmount_length = mainDetail.d_settlementAmount_length,
                        d_settlementMode_start_column = mainDetail.d_settlementMode_start_column,
                        d_settlementMode_length = mainDetail.d_settlementMode_length,
                        d_bankCode_start_column = mainDetail.d_bankCode_start_column,
                        d_bankCode_length = mainDetail.d_bankCode_length,
                        d_bankName_start_column = mainDetail.d_bankName_start_column,
                        d_bankName_length = mainDetail.d_bankName_length,
                        d_branchCode_start_column = mainDetail.d_branchCode_start_column,
                        d_branchCode_length = mainDetail.d_branchCode_length,
                        d_branchName_start_column = mainDetail.d_branchName_start_column,
                        d_branchName_length = mainDetail.d_branchName_length,
                        d_accountType_start_column = mainDetail.d_accountType_start_column,
                        d_accountType_length = mainDetail.d_accountType_length,
                        d_accountNo_start_column = mainDetail.d_accountNo_start_column,
                        d_accountNo_length = mainDetail.d_accountNo_length,//
                        d_goldCardNo_start_column = mainDetail.d_goldCardNo_start_column,
                        d_goldCardNo_length = mainDetail.d_goldCardNo_length,
                        d_billsPayField1_start_column = mainDetail.d_billsPayField1_start_column,
                        d_billsPayField1_length = mainDetail.d_billsPayField1_length,
                        d_billsPayField2_start_column = mainDetail.d_billsPayField2_start_column,
                        d_billsPayField2_length = mainDetail.d_billsPayField2_length,
                        d_billsPayField3_start_column = mainDetail.d_billsPayField3_start_column,
                        d_billsPayField3_length = mainDetail.d_billsPayField3_length,
                        d_billsPayField4_start_column = mainDetail.d_billsPayField4_start_column,
                        d_billsPayField4_length = mainDetail.d_billsPayField4_length,
                        d_billsPayField5_start_column = mainDetail.d_billsPayField5_start_column,
                        d_billsPayField5_length = mainDetail.d_billsPayField5_length,
                        d_outletCode_start_column = mainDetail.d_outletCode_start_column,
                        d_outletCode_length = mainDetail.d_outletCode_length,
                        d_outletBranchCode_start_column = mainDetail.d_outletBranchCode_start_column,
                        d_outletBranchCode_length = mainDetail.d_outletBranchCode_length,
                        d_alterBeneficiaryName_start_column = mainDetail.d_alterBeneficiaryName_start_column,
                        d_alterBeneficiaryName_length = mainDetail.d_alterBeneficiaryName_length,
                        d_alterBeneficiary_relto_beneficiary_start_column = mainDetail.d_alterBeneficiary_relto_beneficiary_start_column,
                        d_alterBeneficiary_relto_beneficiary_length = mainDetail.d_alterBeneficiary_relto_beneficiary_length,
                        d_messageToBeneficiary_start_column = mainDetail.d_messageToBeneficiary_start_column,
                        d_messageToBeneficiary_length = mainDetail.d_messageToBeneficiary_length,
                        d_receiverCorresBank_start_column = mainDetail.d_receiverCorresBank_start_column,
                        d_receiverCorresBank_length = mainDetail.d_receiverCorresBank_length,
                        d_senderCorresBank_start_column = mainDetail.d_senderCorresBank_start_column,
                        d_senderCorresBank_length = mainDetail.d_senderCorresBank_length,
                        d_sendingBank_start_column = mainDetail.d_sendingBank_start_column,
                        d_sendingBank_length = mainDetail.d_sendingBank_length,
                        d_receivingBank_start_column = mainDetail.d_receivingBank_start_column,
                        d_receivingBank_length = mainDetail.d_receivingBank_length,
                        d_modeOfChange_start_column = mainDetail.d_modeOfChange_start_column,
                        d_modeOfChange_length = mainDetail.d_modeOfChange_length,
                        d_purposeCode_start_column = mainDetail.d_purposeCode_start_column,
                        d_purposeCode_length = mainDetail.d_purposeCode_length,
                        d_indvCode_start_column = mainDetail.d_indvCode_start_column,
                        d_indvCode_length = mainDetail.d_indvCode_length,

                        //---- Default Data ----- 
                        applicationNumber = mainDetail.applicationNumber,
                        settlementMode = mainDetail.settlementMode,
                        fundingAmount = mainDetail.fundingAmount,
                        fundingCurrency = mainDetail.fundingCurrency,
                        remitterFirstName = mainDetail.remitterFirstName,
                        beneficiaryFirstName = mainDetail.beneficiaryFirstName,
                        accountNumber = mainDetail.accountNumber,
                        bankCode = mainDetail.bankCode,
                        beneficiaryAddress1 = mainDetail.beneficiaryAddress1,
                        remitterNationality = mainDetail.remitterNationality,
                        remitterAddress1 = mainDetail.remitterAddress1,
                        //remitterCountry = mainDetail.remitterCountry,
                        //remitterZipCode = mainDetail.remitterZipCode,
                        remitterBirthDate = mainDetail.remitterBirthDate,
                        remitterMobileNumber = mainDetail.remitterMobileNumber,
                        remitterIdType1 = mainDetail.remitterIdType1,
                        remitterIdNumber1 = mainDetail.remitterIdNumber1,
                        remitterIdIssuedAt1 = mainDetail.remitterIdIssuedAt1,
                        remitterIdExpiry1 = mainDetail.remitterIdExpiry1,
                        beneficiaryMiddleName = mainDetail.beneficiaryMiddleName,
                        beneficiaryLastName = mainDetail.beneficiaryLastName,
                        beneficiaryCustomerType = mainDetail.beneficiaryCustomerType,
                        beneficiaryZipCode = mainDetail.beneficiaryZipCode,
                        beneficiaryNationality = mainDetail.beneficiaryNationality,
                        beneficiaryBirthDate = mainDetail.beneficiaryBirthDate,
                        beneficiaryRelationToRemitter = mainDetail.beneficiaryRelationToRemitter,
                        beneficiaryMobileNumber = mainDetail.beneficiaryMobileNumber,
                        buyingRate = mainDetail.buyingRate,
                        settlementCurrency = mainDetail.settlementCurrency,
                        paymentField1 = mainDetail.paymentField1,
                        paymentField2 = mainDetail.paymentField2,
                        paymentField3 = mainDetail.paymentField3,
                        outletCode = mainDetail.outletCode,
                        //senderCorrespondentBank = mainDetail.senderCorrespondentBank,
                        //sendingBank = mainDetail.sendingBank,
                        //receivingBank = mainDetail.receivingBank,
                        //modeOfCharge = mainDetail.modeOfCharge,
                        //purposeCode = mainDetail.purposeCode,
                        //individualCode = mainDetail.individualCode,
                        //natureOfBusiness = mainDetail.natureOfBusiness,
                        remitterId = mainDetail.remitterId,
                        remitterMiddleName = mainDetail.remitterMiddleName,
                        remitterLastName = mainDetail.remitterLastName,
                        remitterCustomerType = mainDetail.remitterCustomerType,
                        settlementAmount = mainDetail.settlementAmount,

                        remitterAddress2 = mainDetail.remitterAddress2,
                        remitterAddress3 = mainDetail.remitterAddress3,
                        remitterAddress4 = mainDetail.remitterAddress4,
                        //remitterContinent = mainDetail.remitterContinent,
                        //remitterProfession = mainDetail.remitterProfession,
                        //remitterGender = mainDetail.remitterGender,
                        //remitterCivilStatus = mainDetail.remitterCivilStatus,
                        //remitterPoBox = mainDetail.remitterPoBox,
                        //remitterOfficeNumber = mainDetail.remitterOfficeNumber,
                        //remitterEmail = mainDetail.remitterEmail,
                        //remitterTin = mainDetail.remitterTin,
                        //remitterIdType2 = mainDetail.remitterIdType2,
                        //remitterIdNumber2 = mainDetail.remitterIdNumber2,
                        //remitterIdIssuedAt2 = mainDetail.remitterIdIssuedAt2,
                        //remitterIdExpiry2 = mainDetail.remitterIdExpiry2,
                        //remitterNotificationType = mainDetail.remitterNotificationType,
                        //beneficiaryId = mainDetail.beneficiaryId,
                        beneficiaryAddress2 = mainDetail.beneficiaryAddress2,
                        beneficiaryAddress3 = mainDetail.beneficiaryAddress3,
                        beneficiaryCountry = mainDetail.beneficiaryCountry,
                        //beneficiaryLandmark = mainDetail.beneficiaryLandmark,
                        //beneficiaryProfession = mainDetail.beneficiaryProfession,
                        //beneficiaryGender = mainDetail.beneficiaryGender,
                        //beneficiaryCivilStatus = mainDetail.beneficiaryCivilStatus,
                        //beneficiaryPoBox = mainDetail.beneficiaryPoBox,
                        //alternateRecipientRelationToBeneficiary = mainDetail.alternateRecipientRelationToBeneficiary,
                        //beneficiaryOfficeNumber = mainDetail.beneficiaryOfficeNumber,
                        beneficiaryEmail = mainDetail.beneficiaryEmail,
                        //beneficiaryTin = mainDetail.beneficiaryTin,
                        //beneficiaryNotificationType = mainDetail.beneficiaryNotificationType,
                        transactionDate = mainDetail.transactionDate,
                        bankName = mainDetail.bankName,
                        //branchName = mainDetail.branchName,
                        //accountType = mainDetail.accountType,
                        //goldCardNumber = mainDetail.goldCardNumber,
                        paymentField4 = mainDetail.paymentField4,
                        paymentField5 = mainDetail.paymentField5,
                        outletBranchCode = mainDetail.outletBranchCode,
                        //alternateBeneficiary = mainDetail.alternateBeneficiary,
                        //alternateBeneficiaryRelationToBeneficiary = mainDetail.alternateBeneficiaryRelationToBeneficiary,
                        //messageToBeneficiary = mainDetail.messageToBeneficiary,
                        //receiverCorrespondentBank = mainDetail.receiverCorrespondentBank,
                        branchCode = mainDetail.branchCode,
                    };

                    ModelResponse modelResponse = dal.GenericCreate(deleteReq, "Tieup_Codes_Request", g_global_region_code);

                    if (modelResponse.code != 0)
                    {
                        return Json("N_OK", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.ToString(), ex);

                return Json("N_OK", JsonRequestBehavior.AllowGet);
            }

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        private SelectList App_Codes()
        {
            List<SelectListItem> appcode = new List<SelectListItem>();
            appcode.Add(new SelectListItem { Text = "Select Default Value", Value = "", Selected = true });


            Settlement_Currency oTieup = new Settlement_Currency();
            try
            {
                var tieup = JsonConvert.DeserializeObject<List<Settlement_Currency>>(dal.GenericViewAll(oTieup, "Settlement_Currency", g_global_region_code));

                if (tieup != null)
                {
                    foreach (Settlement_Currency tu in tieup)
                    {
                        string customValue = tu.code_val;
                        string customText = tu.code_val;
                        appcode.Add(new SelectListItem { Text = tu.code_val, Value = tu.code_val });
                    }
                }
            }
            catch (Exception e)
            {
                LOGGER.Fatal(e);
            }

            return new SelectList(appcode, "Value", "Text");

        }

        public ActionResult AddNew()
        {
            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_CREATE))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }

            DARS.Web.Models.FormatMapperFormViewModel formViewModel = new DARS.Web.Models.FormatMapperFormViewModel();
            formViewModel.TieupViewDetailInputs = new TieupViewDetailInputs();
            formViewModel.Action = 1;

            List<Settlement_Currency> settlementCur = new List<Settlement_Currency>();
            sqlCompositeModel customsqlFunCur = new sqlCompositeModel();
            customsqlFunCur.model = new VREMIT.ModelMVC.Settlement_Currency();
            customsqlFunCur.sql = "SELECT CASE WHEN code_val = 'FunCur' THEN 'Funding Currency' ELSE code_val END AS code_val FROM [tbl_settlement_currency] WHERE code_val IN ('PHP', 'USD', 'FunCur'); ";
            settlementCur = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Settlement_Currency>>(dal.GetCustomSQL(customsqlFunCur, "Settlement_Currency", g_global_region_code));
            formViewModel.SettlementCurrency1 = settlementCur;

            List<Tieup> TieupMot = new List<Tieup>();
            sqlCompositeModel customsqlCodeMot = new sqlCompositeModel();
            customsqlCodeMot.model = new VREMIT.ModelMVC.Tieup();
            customsqlCodeMot.sql = "SELECT t.*, p.* FROM tbl_tieup t INNER JOIN tbl_partner_category p ON t.tieup_type = p.id where t.tieup_name <> '' and t.tieup_status = 0 and p.partner_type_desc like '%Source%' order by t.tieup_code";
            TieupMot = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCodeMot, "Tieup", g_global_region_code));
            formViewModel.TieupMot = TieupMot;

            List<Tieup> TieupCode = new List<Tieup>();
            sqlCompositeModel customsqlCode = new sqlCompositeModel();
            customsqlCode.model = new VREMIT.ModelMVC.Tieup();
            customsqlCode.sql = "select * from tbl_tieup where tieup_name <> '' ";
            TieupCode = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCode, "Tieup", g_global_region_code));
            formViewModel.TieupCode = TieupCode;

            sqlCompositeModel customsqlCode1 = new sqlCompositeModel();
            customsqlCode1.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
            customsqlCode1.sql = "SELECT * FROM [tbl_tieup_codes_fileType] where delimiterType in ('Delimiter','Fixed Length') ";
            formViewModel.TieupCodesFileTypes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode1, "Tieup_Codes_FileType", g_global_region_code));

            sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
            customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
            customsqlCode2.sql = "SELECT file_type FROM [tbl_tieup_codes_fileType] where file_type <> '' ";
            formViewModel.TieupCodesFileTypes1 = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_FileType", g_global_region_code));

            sqlCompositeModel customsqlCode3 = new sqlCompositeModel();
            customsqlCode3.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
            customsqlCode3.sql = "SELECT delimiters from [tbl_tieup_codes_fileType] where delimiters <> '' ";
            formViewModel.TieupCodesFileTypes2 = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode3, "Tieup_Codes_FileType", g_global_region_code));

            List<Tieup_Codes> Tieup_Codes = new List<Tieup_Codes>();
            sqlCompositeModel customsqlCode4 = new sqlCompositeModel();
            customsqlCode4.model = new VREMIT.ModelMVC.Tieup_Codes();
            customsqlCode4.sql = "SELECT * FROM tbl_tieup_codes where status = 0 order by tieup_codes ASC";
            Tieup_Codes = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes>>(dal.GetCustomSQL(customsqlCode4, "Tieup_Codes", g_global_region_code));
            formViewModel.TieupCodesMaintParam = Tieup_Codes;

            formViewModel.Formatmapper_Fieldname_Details = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Formatmapper_Fieldname_Details>>(dal.GenericViewAll(new VREMIT.ModelMVC.Formatmapper_Fieldname_Details(), "Formatmapper_Fieldname_Details", g_global_region_code));

            return View(formViewModel);
        }

        // GET: Accounts/FormatMapperMaint/FormatMapperMaintPending
        public ActionResult FormatMapperMaintPending()
        {
            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_VIEW))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }

            try
            {
                GetFileType();

                IList<VREMIT.ModelMVC.Tieup_Codes_Request> banks_pending;

                sqlCompositeModel customsqlCode = new sqlCompositeModel
                {
                    model = new VREMIT.ModelMVC.Tieup_Codes_Request(),
                    sql = "select * from tbl_tieup_codes_request where req_status = 0 AND exists (SELECT 1 FROM tbl_tieup A where tbl_tieup_codes_request.tieup_codes = A.tieup_code AND A.tieup_status = 0) order by dt_last_chg asc"
                };
                banks_pending = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_Request>>(dal.GetCustomSQL(customsqlCode, "Tieup_Codes_Request", g_global_region_code));

                TempData["banks_pending"] = banks_pending;
                ViewBag.BanksPending = banks_pending;

                //Search
                List<Tieup> TieupCode = new List<Tieup>();
                sqlCompositeModel customsqlCodeSearch = new sqlCompositeModel();
                customsqlCodeSearch.model = new VREMIT.ModelMVC.Tieup();
                customsqlCodeSearch.sql = "SELECT t.*, p.* FROM tbl_tieup t INNER JOIN tbl_partner_category p ON t.tieup_type = p.id where t.tieup_name <> '' and t.tieup_status = 0 and p.partner_type_desc like '%Source%' order by t.tieup_code";
                TieupCode = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup>>(dal.GetCustomSQL(customsqlCodeSearch, "Tieup", g_global_region_code));
                ViewBag.TieupCode = TieupCode;

            }
            catch (Exception ex)
            {
                //var lineNumber = new System.Diagnostics.StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                //VREMIT.CoreEngine.EventManager.LogError("Web", Guid.NewGuid().ToString(), "MaintBanksControllers.MaintBanksPending: " + ex.ToString() + " Error line No: " + lineNumber);
                LOGGER.Error(ex.ToString(), ex);
            }

            ViewBag.stitle = TempData["title"];
            ViewBag.stext = TempData["text"];
            ViewBag.sicon = TempData["icon"];
            ViewBag.req = TempData["req"];
            ViewBag.app_status = TempData["app_status"];

            return View();
        }


        public ActionResult MaintFormatMapperPendingDetail(string id, int reqtype)
        {
            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_VIEW))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }

            DARS.Web.Models.FormatMapperFormViewModel formViewModel = new DARS.Web.Models.FormatMapperFormViewModel();
            formViewModel.Action = 0;

            formViewModel.TieupCodesRequest = ViewBag.TieupCodesRequest;
            ViewBag.reqtype = reqtype;

            try
            {
                ViewBag.BanksPending = TempData["banks_pending"];

                TempData.Keep("banks_pending");

                VREMIT.ModelMVC.Tieup_Codes_Request m = new VREMIT.ModelMVC.Tieup_Codes_Request();
                m.pk_tieup_code_id = id;

                VREMIT.ModelMVC.Tieup_Codes n = new VREMIT.ModelMVC.Tieup_Codes();
                n.pk_tieup_code_id = id;

                VREMIT.ModelMVC.vwDeatilTieupFormat o = new VREMIT.ModelMVC.vwDeatilTieupFormat();
                o.tieup_code_id = id;

                //NEED TO SET THESE VALUES TO THE MAX
                //SO THAT THE ENGINE WILL NOT INCLUDE THESE IN THE WHERE CLAUSE
                //AND ONLY THE PK (pk_***)
                m.req_status = System.Byte.MaxValue;
                m.req_type = System.Byte.MaxValue;

                m = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes_Request>(dal.GetGenericDetails(m, "Tieup_Codes_Request", g_global_region_code));

                formViewModel.tieup_code_id = m.pk_tieup_code_id;
                formViewModel.delimiter_type = m.delimiter_type;
                formViewModel.delimiter = m.delimiter;
                formViewModel.RequestType = m.req_type;
                formViewModel.RequestDateTime = m.dt_last_chg.ToLongDateString();
                formViewModel.RequestBy = m.username;
                formViewModel.RequestStatusReason = m.remarks;
                formViewModel.start_record = m.detail_start_record;
                formViewModel.tieup_codes = m.tieup_codes;
                formViewModel.d_prefix = m.d_prefix;

                formViewModel.delimiter = m.delimiter;
                formViewModel.delimiter_val = m.delimiter_val;
                formViewModel.file_type = m.file_type;
                formViewModel.file_type = m.file_type;

                formViewModel.isFilenameValidation = m.filename_validation;
                formViewModel.isFooterValidation = m.footer_validation;
                //formViewModel.settlementCurrency = m.settlementCurrency == "FunCur" ? "Funding Currency" : m.settlementCurrency;

                formViewModel.TieupViewDetailInputs = formViewModel;
                formViewModel.TieupViewMaintInputs = formViewModel;

                formViewModel.Provided = m.provided;

                // Deserialize JSON into Dictionary
                var providedDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(m.provided);


                //DARS.Web.Models.FieldProperty p;

                //if (m.detail_transdate_start_column != null && !m.detail_transdate_start_column.IsEmpty())
                //{
                //    p = new FieldProperty("Transaction Date", m.detail_transdate_start_column, m.transactionDate, m.detail_transdate_length);
                //    formViewModel.FieldPropertiesDetail.Add(p);
                //}

                // Configuration for field properties
                var fieldConfigs = new List<(string Label, string StartColumn, string Value, string Length, string checkBox)>
                {
                    //("Partner Code", m.partnerCode_start_column, m.partnerCode_default, m.partnerCode_length),
                    ("Application Number", m.detail_application_start_column, m.applicationNumber, m.detail_application_length,  m.provided),
                    ("Funding Currency", m.d_fundCurrency_start_column, m.fundingCurrency == "STC" ? "Settlement Currency" : m.fundingCurrency, m.d_fundCurrency_length, m.provided),
                    ("Funding Amount", m.d_fundAmount_start_column, m.fundingAmount, m.d_fundAmount_length,  m.provided),
                    //("Buying Rate", m.d_buyingRate_start_column, m.buyingRate, m.d_buyingRate_length,  m.provided),
                    ("Settlement Currency", m.d_settlementCurrency_start_column, m.settlementCurrency == "FunCur" ? "Funding Currency" : m.settlementCurrency, m.d_settlementCurrency_length,  m.provided),
                    ("Settlement Amount", m.d_settlementAmount_start_column, m.settlementAmount, m.d_settlementAmount_length,  m.provided),
                    ("Settlement Mode", m.d_settlementMode_start_column, m.settlementMode, m.d_settlementMode_length,  m.provided),
                    ("Bank Code", m.d_bankCode_start_column, m.bankCode, m.d_bankCode_length,  m.provided),
                    ("Account Number", m.d_accountNo_start_column, m.accountNumber, m.d_accountNo_length,  m.provided),
                    ("Outlet Code", m.d_outletCode_start_column, m.outletCode, m.d_outletCode_length,  m.provided),
                    ("Remitter First Name", m.detail_remitfname_start_column, m.remitterFirstName, m.detail_remitfname_length,  m.provided),
                    ("Remitter Middle Name", m.detail_remitmidname_start_column, m.remitterMiddleName, m.detail_remitmidname_length,  m.provided),
                    ("Remitter Last Name", m.detail_remitlastname_start_column, m.remitterLastName, m.detail_remitlastname_length,  m.provided),
                    ("Remitter ID", m.detail_remitid_start_column, m.remitterId, m.detail_remitid_length,  m.provided),
                    ("Remitter Mobile Number", m.d_remitMobPhoneNum_start_column, m.remitterMobileNumber, m.d_remitMobPhoneNum_length,  m.provided),
                    ("Remitter Customer Type", m.d_remitCusType_start_column, m.remitterCustomerType, m.d_remitCusType_length,  m.provided),
                    ("Remitter Birth Date", m.d_remitBDate_start_column, m.remitterBirthDate, m.d_remitBDate_length,  m.provided),
                    ("Remitter Address 1", m.d_remitAdd1_start_column, m.remitterAddress1, m.d_remitAdd1_length,  m.provided),
                    ("Remitter Nationality", m.d_remitNationality_start_column, m.remitterNationality, m.d_remitNationality_length,  m.provided),
                    ("Remitter Country", m.remCountry_start_column, m.remCountry_default, m.remCountry_length,  m.provided),
                    ("Remitter Id Type 1", m.d_remitIDType1_start_column, m.remitterIdType1, m.d_remitIDType1_length,  m.provided),
                    ("Remitter Id Issued At 1", m.d_remitIDIssueAt1_start_column, m.remitterIdIssuedAt1, m.d_remitIDIssueAt1_length,  m.provided),
                    ("Remitter Id Expiry 1", m.d_remitIDExpDate1_start_column, m.remitterIdExpiry1, m.d_remitIDExpDate1_length,  m.provided),
                    ("Beneficiary First Name", m.d_beneficiaryFName_start_column, m.beneficiaryFirstName, m.d_beneficiaryFName_length,  m.provided),
                    ("Beneficiary Middle Name", m.d_beneficiaryMName_start_column, m.beneficiaryMiddleName, m.d_beneficiaryMName_length,  m.provided),
                    ("Beneficiary Last Name", m.d_beneficiaryLName_start_column, m.beneficiaryLastName, m.d_beneficiaryLName_length,  m.provided),
                    ("Beneficiary Mobile Number", m.d_beneficiaryMobPhoneNo_start_column, m.beneficiaryMobileNumber, m.d_beneficiaryMobPhoneNo_length,  m.provided),
                    ("Beneficiary Customer Type", m.d_beneficiaryCusType_start_column, m.beneficiaryCustomerType, m.d_beneficiaryCusType_length,  m.provided),
                    ("Beneficiary Address 1", m.d_beneficiaryAdd1_start_column, m.beneficiaryAddress1, m.d_beneficiaryAdd1_length,  m.provided),
                    ("Beneficiary Nationality", m.d_beneficiaryNationality_start_column, m.beneficiaryNationality, m.d_beneficiaryNationality_length,  m.provided),
                    ("Beneficiary Relation to Remitter", m.d_beneficiaryRel_tothe_remit_start_column, m.beneficiaryRelationToRemitter, m.d_beneficiaryRel_tothe_remit_length,  m.provided),
                    ("Beneficiary Zip Code", m.d_zipCode_start_column, m.beneficiaryZipCode, m.d_zipCode_length,  m.provided),
                    ("Payment Field 1", m.d_billsPayField1_start_column, m.paymentField1, m.d_billsPayField1_length,  m.provided),
                    ("Payment Field 2", m.d_billsPayField2_start_column, m.paymentField2, m.d_billsPayField2_length,  m.provided),
                    ("Payment Field 3", m.d_billsPayField3_start_column, m.paymentField3, m.d_billsPayField3_length,  m.provided),
                    ("Nature Of Business", m.natrueOfBuss_start_column, m.natrueOfBuss_default, m.natrueOfBuss_length,  m.provided),
                    ("Transaction Date", m.detail_transdate_start_column, m.transactionDate, m.detail_transdate_length,  m.provided),
                    ("Bank Name", m.d_bankName_start_column, m.bankName, m.d_bankName_length,  m.provided),
                    ("Account Name", m.acctName_start_column, m.acctName_default, m.acctName_length,  m.provided),
                    ("Remitter Address 2", m.d_remitAdd2_start_column, m.remitterAddress2, m.d_remitAdd2_length,  m.provided),
                    ("Remitter Address 3", m.d_remitAdd3_start_column, m.remitterAddress3, m.d_remitAdd3_length,  m.provided),
                    ("Remitter Address 4", m.d_remitAdd4_start_column, m.remitterAddress4, m.d_remitAdd4_length,  m.provided),
                    ("Remitter Fourth Name", m.remFourthNme_start_column, m.remFourthNme_default, m.remFourthNme_length,  m.provided),
                    ("Remitter Place of Birth", m.remPlaceBrth_start_column, m.remPlaceBrth_default, m.remPlaceBrth_length,  m.provided),
                    ("Remitter Account Number", m.remAcctNum_start_column, m.remAcctNum_default, m.remAcctNum_length,  m.provided),
                    ("Remitter IBAN", m.remIBAN_start_column, m.remIBAN_default, m.remIBAN_length,  m.provided),
                    ("Beneficiary Birth Date", m.d_beneficiaryBDate_start_column, m.beneficiaryBirthDate, m.d_beneficiaryBDate_length,  m.provided),
                    ("Beneficiary Address 2", m.d_beneficiaryAdd2_start_column, m.beneficiaryAddress2, m.d_beneficiaryAdd2_length,  m.provided),
                    ("Beneficiary Address 3", m.d_beneficiaryAdd3_start_column, m.beneficiaryAddress3, m.d_beneficiaryAdd3_length,  m.provided),
                    ("Beneficiary Country", m.d_beneficiaryCountry_start_column, m.beneficiaryCountry, m.d_beneficiaryCountry_length,  m.provided),
                    ("Beneficiary Email", m.d_beneficiaryEmailAdd_start_column, m.beneficiaryEmail, m.d_beneficiaryEmailAdd_length,  m.provided),
                    ("Payment Field 4", m.d_billsPayField4_start_column, m.paymentField4, m.d_billsPayField4_length,  m.provided),
                    ("Payment Field 5", m.d_billsPayField5_start_column, m.paymentField5, m.d_billsPayField5_length,  m.provided),
                    ("Outlet Branch Code", m.d_outletBranchCode_start_column, m.outletBranchCode, m.d_outletBranchCode_length,  m.provided),
                    ("Reserve 1", m.res1_start_column, m.res1_default, m.res1_length,  m.provided),
                    ("Reserve 2", m.res2_start_column, m.res2_default, m.res2_length,  m.provided),
                    ("Reserve 3", m.res3_start_column, m.res3_default, m.res3_length,  m.provided),
                    ("Reserve 4", m.res4_start_column, m.res4_default, m.res4_length,  m.provided),
                    ("Reserve 5", m.res5_start_column, m.res5_default, m.res5_length,  m.provided),
                    ("Reserve 6", m.res6_start_column, m.res6_default, m.res6_length,  m.provided),
                    ("Reserve 7", m.res7_start_column, m.res7_default, m.res7_length,  m.provided),
                    ("Reserve 8", m.res8_start_column, m.res8_default, m.res8_length,  m.provided),
                    ("Reserve 9", m.res9_start_column, m.res9_default, m.res9_length,  m.provided),
                    ("Reserve 10", m.res10_start_column, m.res10_default, m.res10_length,  m.provided),
                    ("Source Of Remitter", m.sourceOfRem_start_column, m.sourceOfRem_default, m.sourceOfRem_length,  m.provided),
                    ("Purpose Of Remittance", m.purposeOfRem_start_column, m.purposeOfRem_default, m.purposeOfRem_length,  m.provided),
                    ("Remitter Id Number 1", m.d_remitIDNum1_start_column, m.remitterIdNumber1, m.d_remitIDNum1_length,  m.provided)

                    //("Remitter Continent", m.d_remitContinent_start_column, m.remitterContinent, m.d_remitContinent_length),
                    //("Remitter Zip Code", m.d_remitZipCode_start_column, m.remitterZipCode, m.d_remitZipCode_length),
                    //("Remitter Profession", m.d_remitProf_start_column, m.remitterProfession, m.d_remitProf_length),
                    //("Remitter Gender", m.d_remitGender_start_column, m.remitterGender, m.d_remitGender_length),
                    //("Remitter Civil Status", m.d_remitCivilStat_start_column, m.remitterCivilStatus, m.d_remitCivilStat_length),
                    //("Remitter P.O. Box", m.d_remitPOBox_start_column, m.remitterPoBox, m.d_remitPOBox_length),
                    //("Remitter Office No.", m.d_remitOfficePhoneNo_start_column, m.remitterOfficeNumber, m.d_remitOfficePhoneNo_length),
                    //("Remitter Email", m.d_remitEmailAdd_start_column, m.remitterEmail, m.d_remitEmailAdd_length),
                    //("Remitter TIN", m.d_remitTaxIDNo_start_column, m.remitterTin, m.d_remitTaxIDNo_length),
                    //("Remitter ID Type 2", m.d_remitIDType2_start_column, m.remitterIdType2, m.d_remitIDType2_length),
                    //("Remitter ID Number 2", m.d_remitIDNo2_start_column, m.remitterIdNumber2, m.d_remitIDNo2_length),
                    //("Remitter ID Issued At 2", m.d_remitIDIssueAt2_start_column, m.remitterIdIssuedAt2, m.d_remitIDIssueAt2_length),
                    //("Remitter Expiry 2", m.d_remitIDExpDate2_start_column, m.remitterIdExpiry2, m.d_remitIDExpDate2_length),
                    //("Remitter Notification Type", m.d_remitNotifType_start_column, m.remitterNotificationType, m.d_remitNotifType_length),
                    //("Beneficiary ID", m.d_beneficiaryID_start_column, m.beneficiaryId, m.d_beneficiaryID_length),
                    //("Beneficiary Landmark", m.d_landMark_start_column, m.beneficiaryLandmark, m.d_landMark_length),
                    //("Beneficiary Profession", m.d_beneficiaryProf_start_column, m.beneficiaryProfession, m.d_beneficiaryProf_length),
                    //("Beneficiary Gender", m.d_beneficiaryGender_start_column, m.beneficiaryGender, m.d_beneficiaryGender_length),
                    //("Beneficiary Civil Status", m.d_beneficiaryCivilStat_start_column, m.beneficiaryCivilStatus, m.d_beneficiaryCivilStat_length),
                    //("Beneficiary P.O. Box", m.d_beneficiaryPOBox_start_column, m.beneficiaryPoBox, m.d_beneficiaryPOBox_length),
                    //("Alter Recipient", m.d_alterRecipient_reltobeneficiary_start_column, m.alternateRecipientRelationToBeneficiary, m.d_alterRecipient_reltobeneficiary_length),
                    //("Beneficiary Office Phone No.", m.d_beneficiaryOfficePhoneNo_start_column, m.beneficiaryOfficeNumber, m.d_beneficiaryOfficePhoneNo_length),
                    //("Beneficiary Tax ID No.", m.d_beneficiaryTaxIDNo_start_column, m.beneficiaryTin, m.d_beneficiaryTaxIDNo_length),
                    //("Beneficiary Notification Type", m.d_beneficiaryNotifType_start_column, m.beneficiaryNotificationType, m.d_beneficiaryNotifType_length),
                    //("Branch Code", m.d_branchCode_start_column, m.branchCode, m.d_branchCode_length),
                    //("Branch Name", m.d_branchName_start_column, m.branchName, m.d_branchName_length),
                    //("Account Type", m.d_accountType_start_column, m.accountType, m.d_accountType_length),
                    //("Gold Card Number", m.d_goldCardNo_start_column, m.goldCardNumber, m.d_goldCardNo_length),
                    //("Alternate Beneficiary Name", m.d_alterBeneficiaryName_start_column, m.messageToBeneficiary, m.d_alterBeneficiaryName_length),
                    //("Alter Beneficiary", m.d_alterBeneficiary_relto_beneficiary_start_column, m.alternateBeneficiary, m.d_alterBeneficiary_relto_beneficiary_length),
                    //("Message To Beneficiary", m.d_messageToBeneficiary_start_column, m.messageToBeneficiary, m.d_messageToBeneficiary_length),
                    //("Receiver Corres Bank", m.d_receiverCorresBank_start_column, m.receiverCorrespondentBank, m.d_receiverCorresBank_length),
                    //("Sender Corres Bank", m.d_senderCorresBank_start_column, m.senderCorrespondentBank, m.d_senderCorresBank_length),
                    //("Sending Bank", m.d_sendingBank_start_column, m.sendingBank, m.d_sendingBank_length),
                    //("Receiving Bank", m.d_receivingBank_start_column, m.receivingBank, m.d_receivingBank_length),
                    //("Mode Of Charge", m.d_modeOfChange_start_column, m.modeOfCharge, m.d_modeOfChange_length),
                    //("Purpose Code", m.d_purposeCode_start_column, m.purposeCode, m.d_purposeCode_length),
                    //("Indv Code", m.d_indvCode_start_column, m.individualCode, m.d_indvCode_length)
                };

                var result = fieldConfigs.ToDictionary(
                    field => field.Label,
                    field => providedDictionary.ContainsKey(field.Label) ? providedDictionary[field.Label] : false // Default to false if not found
                );

                //// Print results
                //foreach (var item in result)
                //{
                //    Console.WriteLine($"{item.Key}: {item.Value}");
                //}

                //// Iterating and adding field properties
                //foreach (var config in fieldConfigs)
                //{
                //    var fieldProperty = new FieldProperty(config.Label, config.StartColumn, config.Value, config.Length, $"{item.Value}");
                //    formViewModel.FieldPropertiesDetail.Add(fieldProperty);
                   
                //}

                // Iterating and adding field properties
                foreach (var config in fieldConfigs)
                {
                    // Fetch the corresponding true/false value
                    bool isProvided = result.ContainsKey(config.Label) ? result[config.Label] : false;

                    // Create field property and add it to the ViewModel
                    var fieldProperty = new FieldProperty(config.Label, config.StartColumn, config.Value, config.Length, isProvided.ToString() == "True" ? "Yes" : "No");
                    formViewModel.FieldPropertiesDetail.Add(fieldProperty);
                }


                //For Pending Amendment New and Old Record(s)
                if (m.req_type == ModelViewer.REQ_TYPE_MODIFY)
                {
                    sqlCompositeModel customsql = new sqlCompositeModel
                    {
                        model = new VREMIT.ModelMVC.vwDeatilTieupFormat(),
                        sql = String.Format("SELECT * FROM tbl_vwDeatilTieupFormat where tieup_codes = '{0}' ", m.tieup_codes)
                    };
                    IList<VREMIT.ModelMVC.vwDeatilTieupFormat> modelvwDetail = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.vwDeatilTieupFormat>>(dal.GetCustomSQL(customsql, "vwDeatilTieupFormat", g_global_region_code));

                    List<FormatMapperViewAmendment> amendmentList = new List<FormatMapperViewAmendment>();

                    string[] fieldsToCheck = { //from the req table column
                        "file_type",
                        "start_record",
                        "delimiter_type",
                        "delimiter",
                        "filename_validation",
                        "footer_validation",
                        "delimiter_val",
                        //"detail_start_record",
                        "partnerCode_length",
                        "partnerCode_default",
                        "detail_transdate_start_column",
                        "detail_transdate_length",
                        "transactionDate",
                        "detail_application_start_column",
                        "detail_application_length",
                        "applicationNumber",
                        "d_fundCurrency_start_column",
                        "d_fundCurrency_length",
                        "fundingCurrency",
                        "d_fundAmount_start_column",
                        "d_fundAmount_length",
                        "fundingAmount",
                        "d_settlementCurrency_start_column",
                        "d_settlementCurrency_length",
                        "settlementCurrency",

                        "d_settlementAmount_start_column",
                        "d_settlementAmount_length",
                        "settlementAmount",

                        "d_settlementMode_start_column",
                        "d_settlementMode_length",
                        "settlementMode",
                        "d_bankCode_start_column",
                        "d_bankCode_length",
                        "bankCode",
                        "d_bankName_start_column",
                        "d_bankName_length",
                        "bankName",
                        "d_accountNo_start_column",
                        "d_accountNo_length",
                        "accountNumber",
                        "acctName_start_column",
                        "acctName_length",
                        "acctName_default",
                        "d_outletCode_start_column",
                        "d_outletCode_length",
                        "outletCode",
                        "detail_remitfname_start_column",
                        "detail_remitfname_length",
                        "remitterFirstName",
                        "d_beneficiaryMName_start_column",
                        "d_beneficiaryMName_length",
                        "remitterMiddleName",
                        "d_beneficiaryLName_start_column",
                        "d_beneficiaryLName_length",
                        "remitterLastName",
                        "detail_remitid_start_column",
                        "detail_remitid_length",
                        "remitterId",
                        "d_remitMobPhoneNum_start_column",
                        "d_remitMobPhoneNum_length",
                        "remitterMobileNumber",
                        "d_remitCusType_start_column",
                        "d_remitCusType_length",
                        "remitterCustomerType",
                        "d_remitBDate_start_column",
                        "d_remitBDate_length",
                        "remitterBirthDate",
                        "d_remitAdd1_start_column",
                        "d_remitAdd1_length",
                        "remitterAddress1",
                        "d_remitAdd2_start_column",
                        "d_remitAdd2_length",
                        "remitterAddress2",
                        "d_remitAdd3_start_column",
                        "d_remitAdd3_length",
                        "remitterAddress3",
                        "d_remitAdd4_start_column",
                        "d_remitAdd4_length",
                        "remitterAddress4",
                        "d_remitNationality_start_column",
                        "d_remitNationality_length",
                        "remitterNationality",
                        "remCountry_start_column",
                        "remCountry_length",
                        "remCountry_default",
                        "d_remitIDType1_start_column",
                        "d_remitIDType1_length",
                        "remitterIdType1",
                        "d_remitIDNum1_start_column",
                        "d_remitIDNum1_length",
                        "remitterIdNumber1",
                        "d_remitIDIssueAt1_start_column",
                        "d_remitIDIssueAt1_length",
                        "remitterIdIssuedAt1",
                        "d_remitIDExpDate1_start_column",
                        "d_remitIDExpDate1_length",
                        "remitterIdExpiry1",
                        "remFourthNme_start_column",
                        "remFourthNme_length",
                        "remFourthNme_default",
                        "remPlaceBrth_start_column",
                        "remPlaceBrth_length",
                        "remPlaceBrth_default",
                        "remAcctNum_start_column",
                        "remAcctNum_length",
                        "remAcctNum_default",
                        "remIBAN_start_column",
                        "remIBAN_length",
                        "remIBAN_default",
                        "d_beneficiaryFName_start_column",
                        "d_beneficiaryFName_length",
                        "beneficiaryFirstName",
                        "d_beneficiaryMName_start_column",
                        "d_beneficiaryMName_length",
                        "beneficiaryMiddleName",
                        "d_beneficiaryLName_start_column",
                        "d_beneficiaryLName_length",
                        "beneficiaryLastName",
                        "d_beneficiaryMobPhoneNo_start_column",
                        "d_beneficiaryMobPhoneNo_length",
                        "beneficiaryMobileNumber",
                        "d_beneficiaryCusType_start_column",
                        "d_beneficiaryCusType_length",
                        "beneficiaryCustomerType",
                        "d_beneficiaryBDate_start_column",
                        "d_beneficiaryBDate_length",
                        "beneficiaryBirthDate",
                        "d_beneficiaryAdd1_start_column",
                        "d_beneficiaryAdd1_length",
                        "beneficiaryAddress1",
                        "d_beneficiaryAdd2_start_column",
                        "d_beneficiaryAdd2_length",
                        "beneficiaryAddress2",
                        "d_beneficiaryAdd3_start_column",
                        "d_beneficiaryAdd3_length",
                        "beneficiaryAddress3",
                        "d_beneficiaryNationality_start_column",
                        "d_beneficiaryNationality_length",
                        "beneficiaryNationality",
                        "d_beneficiaryRel_tothe_remit_start_column",
                        "d_beneficiaryRel_tothe_remit_length",
                        "beneficiaryRelationToRemitter",
                        "d_beneficiaryCountry_start_column",
                        "d_beneficiaryCountry_length",
                        "beneficiaryCountry",
                        "d_zipCode_start_column",
                        "d_zipCode_length",
                        "beneficiaryZipCode",
                        "d_beneficiaryEmailAdd_start_column",
                        "d_beneficiaryEmailAdd_length",
                        "beneficiaryEmail",
                        "d_billsPayField1_start_column",
                        "d_billsPayField1_length",
                        "paymentField1",
                        "d_billsPayField2_start_column",
                        "d_billsPayField2_length",
                        "paymentField2",
                        "d_billsPayField3_start_column",
                        "d_billsPayField3_length",
                        "paymentField3",
                        "d_billsPayField4_start_column",
                        "d_billsPayField4_length",
                        "paymentField4",
                        "d_billsPayField5_start_column",
                        "d_billsPayField5_length",
                        "paymentField5",
                        "d_outletBranchCode_start_column",
                        "d_outletBranchCode_length",
                        "outletBranchCode",
                        "res1_start_column",
                        "res1_length",
                        "res1_default",
                        "res2_start_column",
                        "res2_length",
                        "res2_default",
                        "res3_start_column",
                        "res3_length",
                        "res3_default",
                        "res4_start_column",
                        "res4_length",
                        "res4_default",
                        "res5_start_column",
                        "res5_length",
                        "res5_default",
                        "res6_start_column",
                        "res6_length",
                        "res6_default",
                        "res7_start_column",
                        "res7_length",
                        "res7_default",
                        "res8_start_column",
                        "res8_length",
                        "res8_default",
                        "res9_start_column",
                        "res9_length",
                        "res9_default",
                        "res10_start_column",
                        "res10_length",
                        "res10_default",
                        "sourceOfRem_start_column",
                        "sourceOfRem_length",
                        "sourceOfRem_default",
                        "natrueOfBuss_start_column",
                        "natrueOfBuss_length",
                        "natrueOfBuss_default",
                        "purposeOfRem_start_column",
                        "purposeOfRem_length",
                        "purposeOfRem_default",
                        "d_prefix"
                    };
                    string[] fieldsName = {
                        "File Type",
                        "Start Record",
                        "Delimiter Type",
                        "Delimiter",
                        "File Name Validation",
                        "Footer Validation",
                        "Delimiter Value",
                        //"Start Record",
                        "Partner Code Length",
                        "Default Partner Code",
                        "Transaction Date Start Column",
                        "Transaction Date Length",
                        "Default Transaction Date",
                        "Application Number Start Column",
                        "Application Number Length",
                        "Default Application Number",
                        "Funding Currency Start Column",
                        "Funding Currency Length",
                        "Default Funding Currency",
                        "Funding Amount Start Column",
                        "Funding Amount Length",
                        "Default Funding Amount",
                        "Settlement Currency Start Column",
                        "Settlement Currency Length",
                        "Default Settlement Currency",

                        "Settlement Amount Start Column",
                        "Settlement Amount Length",
                        "Default Settlement Amount",

                        "Settlement Mode Start Column",
                        "Settlement Mode Length",
                        "Default Settlement Mode",
                        "Bank Code Start Column",
                        "Bank Code Length",
                        "Default Bank Code",
                        "Bank Name Start Column",
                        "Bank Name Length",
                        "Default Bank Name",
                        "Account Number Start Column",
                        "Account Number Length",
                        "Default Account Number",
                        "Account Name Start Column",
                        "Account Name Length",
                        "Default Account Name",
                        "Outlet Code Start Column",
                        "Outlet Code Length",
                        "Default Outlet Code",
                        "Remitter First Name Start Column",
                        "Remitter First Name Length",
                        "Default Remitter First Name",
                        "Remitter Middle Name Start Column",
                        "Remitter Middle Name Length",
                        "Default Remitter Middle Name",
                        "Remitter Last Name Start Column",
                        "Remitter Last Name Length",
                        "Default Remitter Last Name",
                        "Remitter ID Start Column",
                        "Remitter ID Length",
                        "Default Remitter ID",
                        "Remitter Mobile Number Start Column",
                        "Remitter Mobile Number Length",
                        "Default Remitter Mobile Number",
                        "Remitter Customer Type Start Column",
                        "Remitter Customer Type Length",
                        "Default Remitter Customer Type",
                        "Remitter Birth Date Start Column",
                        "Remitter Birth Date Length",
                        "Default Remitter Birth Date",
                        "Remitter Address 1 Start Column",
                        "Remitter Address 1 Length",
                        "Default Remitter Address 1",
                        "Remitter Address 2 Start Column",
                        "Remitter Address 2 Length",
                        "Default Remitter Address 2",
                        "Remitter Address 3 Start Column",
                        "Remitter Address 3 Length",
                        "Default Remitter Address 3",
                        "Remitter Address 4 Start Column",
                        "Remitter Address 4 Length",
                        "Default Remitter Address 4",
                        "Remitter Nationality Start Column",
                        "Remitter Nationality Length",
                        "Default Remitter Nationality",
                        "Remitter Country Start Column",
                        "Remitter Country Length",
                        "Default Remitter Country",
                        "Remitter Id Type 1 Start Column",
                        "Remitter Id Type 1 Length",
                        "Default Remitter Id Type 1",
                        "Remitter Id Number 1 Start Column",
                        "Remitter Id Number 1 Length",
                        "Default Remitter Id Number 1",
                        "Remitter Id Issue At 1 Start Column",
                        "Remitter Id Issue At 1 Length",
                        "Default Remitter Id Issue At 1",
                        "Remitter Id Expiry 1 Start Column",
                        "Remitter Id Expiry 1 Length",
                        "Default Remitter Id Expiry 1",
                        "Remitter Fourth Name Start Column",
                        "Remitter Fourth Name Length",
                        "Default Remitter Fourth Name",
                        "Remitter Place of Birth Start Column",
                        "Remitter Place of Birth Length",
                        "Default Remitter Place of Birth",
                        "Remitter Account Number Start Column",
                        "Remitter Account Number Length",
                        "Default Remitter Account Number",
                        "Remitter IBAN Start Column",
                        "Remitter IBAN Length",
                        "Default Remitter IBAN",
                        "Beneficiary First Name Start Column",
                        "Beneficiary First Name Length",
                        "Default Beneficiary First Name",
                        "Beneficiary Middle Name Start Column",
                        "Beneficiary Middle Name Length",
                        "Default Beneficiary Middle Name",
                        "Beneficiary Last Name Start Column",
                        "Beneficiary Last Name Length",
                        "Default Beneficiary Last Name",
                        "Beneficiary Mobile Number Start Column",
                        "Beneficiary Mobile Number Length",
                        "Default Beneficiary Mobile Number",
                        "Beneficiary Customer Type Start Column",
                        "Beneficiary Customer Type Length",
                        "Default Beneficiary Customer Type",
                        "Beneficiary Birth Date Start Column",
                        "Beneficiary Birth Date Length",
                        "Default Beneficiary Birth Date",
                        "Beneficiary Address 1 Start Column",
                        "Beneficiary Address 1 Length",
                        "Default Beneficiary Address 1",
                        "Beneficiary Address 2 Start Column",
                        "Beneficiary Address 2 Length",
                        "Default Beneficiary Address 2",
                        "Beneficiary Address 3 Start Column",
                        "Beneficiary Address 3 Length",
                        "Default Beneficiary Address 3",
                        "Beneficiary Nationality Start Column",
                        "Beneficiary Nationality Length",
                        "Default Beneficiary Nationality",
                        "Beneficiary Relation to Remitter Start Column",
                        "Beneficiary Relation to Remitter Length",
                        "Default Beneficiary Relation to Remitter",
                        "Beneficiary Country Start Column",
                        "Beneficiary Country Length",
                        "Default Beneficiary Country",
                        "Beneficiary Zip Code Start Column",
                        "Beneficiary Zip Code Length",
                        "Default Beneficiary Zip Code",
                        "Beneficiary Email Start Column",
                        "Beneficiary Email Length",
                        "Default Beneficiary Email",
                        "Payment Field 1 Start Column",
                        "Payment Field 1 Length",
                        "Default Payment Field 1",
                        "Payment Field 2 Start Column",
                        "Payment Field 2 Length",
                        "Default Payment Field 2",
                        "Payment Field 3 Start Column",
                        "Payment Field 3 Length",
                        "Default Payment Field 3",
                        "Payment Field 4 Start Column",
                        "Payment Field 4 Length",
                        "Default Payment Field 4",
                        "Payment Field 5 Start Column",
                        "Payment Field 5 Length",
                        "Default Payment Field 5",
                        "Outlet Branch Code Start Column",
                        "Outlet Branch Code Length",
                        "Default Outlet Branch Code",
                        "Reserve 1 Start Column",
                        "Reserve 1 Length",
                        "Default Reserve 1",
                        "Reserve 2 Start Column",
                        "Reserve 2 Length",
                        "Default Reserve 2",
                        "Reserve 3 Start Column",
                        "Reserve 3 Length",
                        "Default Reserve 3",
                        "Reserve 4 Start Column",
                        "Reserve 4 Length",
                        "Default Reserve 4",
                        "Reserve 5 Start Column",
                        "Reserve 5 Length",
                        "Default Reserve 5",
                        "Reserve 6 Start Column",
                        "Reserve 6 Length",
                        "Default Reserve 6",
                        "Reserve 7 Start Column",
                        "Reserve 7 Length",
                        "Default Reserve 7",
                        "Reserve 8 Start Column",
                        "Reserve 8 Length",
                        "Default Reserve 8",
                        "Reserve 9 Start Column",
                        "Reserve 9 Length",
                        "Default Reserve 9",
                        "Reserve 10 Start Column",
                        "Reserve 10 Length",
                        "Default Reserve 10",
                        "Source Of Remitter Start Column",
                        "Source Of Remitter Length",
                        "Default Source Of Remitter",
                        "Nature Of Business Start Column",
                        "Nature Of Business Length",
                        "Default Nature Of Business",
                        "Purpose Of Remittance Start Column",
                        "Purpose Of Remittance Length",
                        "Default Purpose Of Remittance",
                        "Prefix"
                    };

                    foreach (string fc in fieldsToCheck)
                    {
                        object currentValue = typeof(VREMIT.ModelMVC.vwDeatilTieupFormat).GetProperty(fc)?.GetValue(modelvwDetail[0]);
                        object newValue = typeof(VREMIT.ModelMVC.Tieup_Codes_Request).GetProperty(fc)?.GetValue(m);

                        currentValue = currentValue == null ? "" : currentValue;

                        if (!object.Equals(currentValue, newValue))
                        {
                            int index = Array.IndexOf(fieldsToCheck, fc);
                            FormatMapperViewAmendment amendment = new FormatMapperViewAmendment();
                            amendment.fieldName = fieldsName[index];
                            //amendment.currentValue = currentValue?.ToString();

                            if (currentValue?.ToString() == "" || currentValue?.ToString() == null)
                            {
                                amendment.currentValue = "";
                            }
                            else
                            {
                                amendment.currentValue = currentValue?.ToString();
                            }

                            if (newValue?.ToString() == "" || newValue?.ToString() == null || newValue?.ToString() == " ")
                            {
                                amendment.newValue = "";
                            }
                            else
                            {
                                amendment.newValue = newValue?.ToString() == "STC" ? "Settlement Currency" : newValue?.ToString();
                            }

                            amendmentList.Add(amendment);
                        }
                    }
                    formViewModel.amendmentDatatable = amendmentList;
                }
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.ToString(), ex);

                ViewBag.Status = -1;
                ViewBag.ErrorMessage = "An error encountered while processing request.";

                return View();
            }

            return View(formViewModel);
        }

        public ActionResult Detail(string i)
        {
            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_VIEW))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }

            var formViewModel = new DARS.Web.Models.FormatMapperDetailFormViewModel();
            //formViewModel.Action = 0;

            try
            {
                var t = new VREMIT.ModelMVC.Tieup_Codes();
                t.pk_tieup_code_id = i;

                var m = new VREMIT.ModelMVC.Tieup_Codes_Detail();
                m.pk_detail_tieup_id = i;

                t = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes>(dal.GetGenericDetails(t, "Tieup_Codes", g_global_region_code));
                m = JsonConvert.DeserializeObject<VREMIT.ModelMVC.Tieup_Codes_Detail>(dal.GetGenericDetails(m, "Tieup_Codes_Detail", g_global_region_code));

                formViewModel.DetailTieupId = m.pk_detail_tieup_id;
                formViewModel.TieupCodeId = m.tieup_code_id;
                formViewModel.FileType = m.file_type;
                formViewModel.DelimiterType = m.delimiter_type;
                formViewModel.Delimiter = m.delimiter;
                formViewModel.Remarks = m.remarks;
                formViewModel.Status = m.status;
                formViewModel.Username = m.username;
                formViewModel.DateLastChange = m.dt_last_chg.ToLongDateString();
                formViewModel.StartRecord = m.detail_start_record;

                formViewModel.delimiter = m.delimiter;
                formViewModel.delimiter_val = m.delimiter_val;
                formViewModel.file_type = m.file_type;
                formViewModel.FileType = t.file_type;

                formViewModel.isFilenameValidation = t.filename_validation;
                formViewModel.isFooterValidation = t.footer_validation;
                //formViewModel.settlement_currency = t.settlement_currency == "FunCur" ? "Funding Currency" : t.settlement_currency;

                formViewModel.Prefix = m.d_prefix;

                //DARS.Web.Models.FieldProperty p;

                // Deserialize JSON into Dictionary
                var providedDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(m.provided);

                var fieldConfigs = new List<(string Label, string StartColumn, string Value, string Length, string checkBox)>
                {
                    //("Partner Code", m.partnerCode_start_column, m.partnerCode_default, m.partnerCode_length),
                    ("Application Number", m.detail_application_start_column, m.applicationNumber, m.detail_application_length,  m.provided),
                    ("Funding Currency", m.d_fundCurrency_start_column, m.fundingCurrency == "STC" ? "Settlement Currency" : m.fundingCurrency, m.d_fundCurrency_length, m.provided),
                    ("Funding Amount", m.d_fundAmount_start_column, m.fundingAmount, m.d_fundAmount_length,  m.provided),
                    //("Buying Rate", m.d_buyingRate_start_column, m.buyingRate, m.d_buyingRate_length,  m.provided),
                    ("Settlement Currency", m.d_settlementCurrency_start_column, m.settlementCurrency == "FunCur" ? "Funding Currency" : m.settlementCurrency, m.d_settlementCurrency_length,  m.provided),
                    ("Settlement Amount", m.d_settlementAmount_start_column, m.settlementAmount, m.d_settlementAmount_length,  m.provided),
                    ("Settlement Mode", m.d_settlementMode_start_column, m.settlementMode, m.d_settlementMode_length,  m.provided),
                    ("Bank Code", m.d_bankCode_start_column, m.bankCode, m.d_bankCode_length,  m.provided),
                    ("Account Number", m.d_accountNo_start_column, m.accountNumber, m.d_accountNo_length,  m.provided),
                    ("Outlet Code", m.d_outletCode_start_column, m.outletCode, m.d_outletCode_length,  m.provided),
                    ("Remitter First Name", m.detail_remitfname_start_column, m.remitterFirstName, m.detail_remitfname_length,  m.provided),
                    ("Remitter Middle Name", m.detail_remitmidname_start_column, m.remitterMiddleName, m.detail_remitmidname_length,  m.provided),
                    ("Remitter Last Name", m.detail_remitlastname_start_column, m.remitterLastName, m.detail_remitlastname_length,  m.provided),
                    ("Remitter ID", m.detail_remitid_start_column, m.remitterId, m.detail_remitid_length,  m.provided),
                    ("Remitter Mobile Number", m.d_remitMobPhoneNum_start_column, m.remitterMobileNumber, m.d_remitMobPhoneNum_length,  m.provided),
                    ("Remitter Customer Type", m.d_remitCusType_start_column, m.remitterCustomerType, m.d_remitCusType_length,  m.provided),
                    ("Remitter Birth Date", m.d_remitBDate_start_column, m.remitterBirthDate, m.d_remitBDate_length,  m.provided),
                    ("Remitter Address 1", m.d_remitAdd1_start_column, m.remitterAddress1, m.d_remitAdd1_length,  m.provided),
                    ("Remitter Nationality", m.d_remitNationality_start_column, m.remitterNationality, m.d_remitNationality_length,  m.provided),
                    ("Remitter Country", m.remCountry_start_column, m.remCountry_default, m.remCountry_length,  m.provided),
                    ("Remitter Id Type 1", m.d_remitIDType1_start_column, m.remitterIdType1, m.d_remitIDType1_length,  m.provided),
                    ("Remitter Id Issued At 1", m.d_remitIDIssueAt1_start_column, m.remitterIdIssuedAt1, m.d_remitIDIssueAt1_length,  m.provided),
                    ("Remitter Id Expiry 1", m.d_remitIDExpDate1_start_column, m.remitterIdExpiry1, m.d_remitIDExpDate1_length,  m.provided),
                    ("Beneficiary First Name", m.d_beneficiaryFName_start_column, m.beneficiaryFirstName, m.d_beneficiaryFName_length,  m.provided),
                    ("Beneficiary Middle Name", m.d_beneficiaryMName_start_column, m.beneficiaryMiddleName, m.d_beneficiaryMName_length,  m.provided),
                    ("Beneficiary Last Name", m.d_beneficiaryLName_start_column, m.beneficiaryLastName, m.d_beneficiaryLName_length,  m.provided),
                    ("Beneficiary Mobile Number", m.d_beneficiaryMobPhoneNo_start_column, m.beneficiaryMobileNumber, m.d_beneficiaryMobPhoneNo_length,  m.provided),
                    ("Beneficiary Customer Type", m.d_beneficiaryCusType_start_column, m.beneficiaryCustomerType, m.d_beneficiaryCusType_length,  m.provided),
                    ("Beneficiary Address 1", m.d_beneficiaryAdd1_start_column, m.beneficiaryAddress1, m.d_beneficiaryAdd1_length,  m.provided),
                    ("Beneficiary Nationality", m.d_beneficiaryNationality_start_column, m.beneficiaryNationality, m.d_beneficiaryNationality_length,  m.provided),
                    ("Beneficiary Relation to Remitter", m.d_beneficiaryRel_tothe_remit_start_column, m.beneficiaryRelationToRemitter, m.d_beneficiaryRel_tothe_remit_length,  m.provided),
                    ("Beneficiary Zip Code", m.d_zipCode_start_column, m.beneficiaryZipCode, m.d_zipCode_length,  m.provided),
                    ("Payment Field 1", m.d_billsPayField1_start_column, m.paymentField1, m.d_billsPayField1_length,  m.provided),
                    ("Payment Field 2", m.d_billsPayField2_start_column, m.paymentField2, m.d_billsPayField2_length,  m.provided),
                    ("Payment Field 3", m.d_billsPayField3_start_column, m.paymentField3, m.d_billsPayField3_length,  m.provided),
                    ("Nature Of Business", m.natrueOfBuss_start_column, m.natrueOfBuss_default, m.natrueOfBuss_length,  m.provided),
                    ("Transaction Date", m.detail_transdate_start_column, m.transactionDate, m.detail_transdate_length,  m.provided),
                    ("Bank Name", m.d_bankName_start_column, m.bankName, m.d_bankName_length,  m.provided),
                    ("Account Name", m.acctName_start_column, m.acctName_default, m.acctName_length,  m.provided),
                    ("Remitter Address 2", m.d_remitAdd2_start_column, m.remitterAddress2, m.d_remitAdd2_length,  m.provided),
                    ("Remitter Address 3", m.d_remitAdd3_start_column, m.remitterAddress3, m.d_remitAdd3_length,  m.provided),
                    ("Remitter Address 4", m.d_remitAdd4_start_column, m.remitterAddress4, m.d_remitAdd4_length,  m.provided),
                    ("Remitter Fourth Name", m.remFourthNme_start_column, m.remFourthNme_default, m.remFourthNme_length,  m.provided),
                    ("Remitter Place of Birth", m.remPlaceBrth_start_column, m.remPlaceBrth_default, m.remPlaceBrth_length,  m.provided),
                    ("Remitter Account Number", m.remAcctNum_start_column, m.remAcctNum_default, m.remAcctNum_length,  m.provided),
                    ("Remitter IBAN", m.remIBAN_start_column, m.remIBAN_default, m.remIBAN_length,  m.provided),
                    ("Beneficiary Birth Date", m.d_beneficiaryBDate_start_column, m.beneficiaryBirthDate, m.d_beneficiaryBDate_length,  m.provided),
                    ("Beneficiary Address 2", m.d_beneficiaryAdd2_start_column, m.beneficiaryAddress2, m.d_beneficiaryAdd2_length,  m.provided),
                    ("Beneficiary Address 3", m.d_beneficiaryAdd3_start_column, m.beneficiaryAddress3, m.d_beneficiaryAdd3_length,  m.provided),
                    ("Beneficiary Country", m.d_beneficiaryCountry_start_column, m.beneficiaryCountry, m.d_beneficiaryCountry_length,  m.provided),
                    ("Beneficiary Email", m.d_beneficiaryEmailAdd_start_column, m.beneficiaryEmail, m.d_beneficiaryEmailAdd_length,  m.provided),
                    ("Payment Field 4", m.d_billsPayField4_start_column, m.paymentField4, m.d_billsPayField4_length,  m.provided),
                    ("Payment Field 5", m.d_billsPayField5_start_column, m.paymentField5, m.d_billsPayField5_length,  m.provided),
                    ("Outlet Branch Code", m.d_outletBranchCode_start_column, m.outletBranchCode, m.d_outletBranchCode_length,  m.provided),
                    ("Reserve 1", m.res1_start_column, m.res1_default, m.res1_length,  m.provided),
                    ("Reserve 2", m.res2_start_column, m.res2_default, m.res2_length,  m.provided),
                    ("Reserve 3", m.res3_start_column, m.res3_default, m.res3_length,  m.provided),
                    ("Reserve 4", m.res4_start_column, m.res4_default, m.res4_length,  m.provided),
                    ("Reserve 5", m.res5_start_column, m.res5_default, m.res5_length,  m.provided),
                    ("Reserve 6", m.res6_start_column, m.res6_default, m.res6_length,  m.provided),
                    ("Reserve 7", m.res7_start_column, m.res7_default, m.res7_length,  m.provided),
                    ("Reserve 8", m.res8_start_column, m.res8_default, m.res8_length,  m.provided),
                    ("Reserve 9", m.res9_start_column, m.res9_default, m.res9_length,  m.provided),
                    ("Reserve 10", m.res10_start_column, m.res10_default, m.res10_length,  m.provided),
                    ("Source Of Remitter", m.sourceOfRem_start_column, m.sourceOfRem_default, m.sourceOfRem_length,  m.provided),
                    ("Purpose Of Remittance", m.purposeOfRem_start_column, m.purposeOfRem_default, m.purposeOfRem_length,  m.provided),
                    ("Remitter Id Number 1", m.d_remitIDNum1_start_column, m.remitterIdNumber1, m.d_remitIDNum1_length,  m.provided)

                    //("Remitter Continent", m.d_remitContinent_start_column, m.remitterContinent, m.d_remitContinent_length),
                    //("Remitter Zip Code", m.d_remitZipCode_start_column, m.remitterZipCode, m.d_remitZipCode_length),
                    //("Remitter Profession", m.d_remitProf_start_column, m.remitterProfession, m.d_remitProf_length),
                    //("Remitter Gender", m.d_remitGender_start_column, m.remitterGender, m.d_remitGender_length),
                    //("Remitter Civil Status", m.d_remitCivilStat_start_column, m.remitterCivilStatus, m.d_remitCivilStat_length),
                    //("Remitter P.O. Box", m.d_remitPOBox_start_column, m.remitterPoBox, m.d_remitPOBox_length),
                    //("Remitter Office No.", m.d_remitOfficePhoneNo_start_column, m.remitterOfficeNumber, m.d_remitOfficePhoneNo_length),
                    //("Remitter Email", m.d_remitEmailAdd_start_column, m.remitterEmail, m.d_remitEmailAdd_length),
                    //("Remitter TIN", m.d_remitTaxIDNo_start_column, m.remitterTin, m.d_remitTaxIDNo_length),
                    //("Remitter ID Type 2", m.d_remitIDType2_start_column, m.remitterIdType2, m.d_remitIDType2_length),
                    //("Remitter ID Number 2", m.d_remitIDNo2_start_column, m.remitterIdNumber2, m.d_remitIDNo2_length),
                    //("Remitter ID Issued At 2", m.d_remitIDIssueAt2_start_column, m.remitterIdIssuedAt2, m.d_remitIDIssueAt2_length),
                    //("Remitter Expiry 2", m.d_remitIDExpDate2_start_column, m.remitterIdExpiry2, m.d_remitIDExpDate2_length),
                    //("Remitter Notification Type", m.d_remitNotifType_start_column, m.remitterNotificationType, m.d_remitNotifType_length),
                    //("Beneficiary ID", m.d_beneficiaryID_start_column, m.beneficiaryId, m.d_beneficiaryID_length),
                    //("Beneficiary Landmark", m.d_landMark_start_column, m.beneficiaryLandmark, m.d_landMark_length),
                    //("Beneficiary Profession", m.d_beneficiaryProf_start_column, m.beneficiaryProfession, m.d_beneficiaryProf_length),
                    //("Beneficiary Gender", m.d_beneficiaryGender_start_column, m.beneficiaryGender, m.d_beneficiaryGender_length),
                    //("Beneficiary Civil Status", m.d_beneficiaryCivilStat_start_column, m.beneficiaryCivilStatus, m.d_beneficiaryCivilStat_length),
                    //("Beneficiary P.O. Box", m.d_beneficiaryPOBox_start_column, m.beneficiaryPoBox, m.d_beneficiaryPOBox_length),
                    //("Alter Recipient", m.d_alterRecipient_reltobeneficiary_start_column, m.alternateRecipientRelationToBeneficiary, m.d_alterRecipient_reltobeneficiary_length),
                    //("Beneficiary Office Phone No.", m.d_beneficiaryOfficePhoneNo_start_column, m.beneficiaryOfficeNumber, m.d_beneficiaryOfficePhoneNo_length),
                    //("Beneficiary Tax ID No.", m.d_beneficiaryTaxIDNo_start_column, m.beneficiaryTin, m.d_beneficiaryTaxIDNo_length),
                    //("Beneficiary Notification Type", m.d_beneficiaryNotifType_start_column, m.beneficiaryNotificationType, m.d_beneficiaryNotifType_length),
                    //("Branch Code", m.d_branchCode_start_column, m.branchCode, m.d_branchCode_length),
                    //("Branch Name", m.d_branchName_start_column, m.branchName, m.d_branchName_length),
                    //("Account Type", m.d_accountType_start_column, m.accountType, m.d_accountType_length),
                    //("Gold Card Number", m.d_goldCardNo_start_column, m.goldCardNumber, m.d_goldCardNo_length),
                    //("Alternate Beneficiary Name", m.d_alterBeneficiaryName_start_column, m.messageToBeneficiary, m.d_alterBeneficiaryName_length),
                    //("Alter Beneficiary", m.d_alterBeneficiary_relto_beneficiary_start_column, m.alternateBeneficiary, m.d_alterBeneficiary_relto_beneficiary_length),
                    //("Message To Beneficiary", m.d_messageToBeneficiary_start_column, m.messageToBeneficiary, m.d_messageToBeneficiary_length),
                    //("Receiver Corres Bank", m.d_receiverCorresBank_start_column, m.receiverCorrespondentBank, m.d_receiverCorresBank_length),
                    //("Sender Corres Bank", m.d_senderCorresBank_start_column, m.senderCorrespondentBank, m.d_senderCorresBank_length),
                    //("Sending Bank", m.d_sendingBank_start_column, m.sendingBank, m.d_sendingBank_length),
                    //("Receiving Bank", m.d_receivingBank_start_column, m.receivingBank, m.d_receivingBank_length),
                    //("Mode Of Charge", m.d_modeOfChange_start_column, m.modeOfCharge, m.d_modeOfChange_length),
                    //("Purpose Code", m.d_purposeCode_start_column, m.purposeCode, m.d_purposeCode_length),
                    //("Indv Code", m.d_indvCode_start_column, m.individualCode, m.d_indvCode_length)
                };

                var result = fieldConfigs.ToDictionary(
                    field => field.Label,
                    field => providedDictionary.ContainsKey(field.Label) ? providedDictionary[field.Label] : false // Default to false if not found
                );

                // Iterating and adding field properties
                foreach (var config in fieldConfigs)
                {
                    // Fetch the corresponding true/false value
                    bool isProvided = result.ContainsKey(config.Label) ? result[config.Label] : false;

                    // Create field property and add it to the ViewModel
                    var fieldProperty = new FieldProperty(config.Label, config.StartColumn, config.Value, config.Length, isProvided.ToString() == "True" ? "Yes" : "No");
                    formViewModel.FieldProperties.Add(fieldProperty);
                }

                // Iterating and adding field properties
                //foreach (var config in fieldConfigs)
                //{
                //    if ((config.StartColumn != null && !config.StartColumn.IsEmpty()) || (config.Value != null && !config.Value.IsEmpty()))
                //    {
                //        var fieldProperty = new FieldProperty(config.Label, config.StartColumn, config.Value, config.Length);
                //        formViewModel.FieldProperties.Add(fieldProperty);
                //    }
                //}

            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.ToString(), ex);

                ViewBag.STitle = "Error";
                ViewBag.SText = "An error encountered while processing request.";
                ViewBag.SIcon = "error";
            }

            return View(formViewModel);
        }

        private void GetFileType()
        {
            DARS.Web.Models.FormatMapperFormViewModel formViewModel = new DARS.Web.Models.FormatMapperFormViewModel();
            formViewModel.TieupViewDetailInputs = new TieupViewDetailInputs();

            IList<VREMIT.ModelMVC.Tieup_Codes_FileType> fileType;
            sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
            customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
            customsqlCode2.sql = "SELECT file_type FROM [tbl_tieup_codes_fileType] where file_type <> '' ";
            fileType = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_FileType", g_global_region_code));
            formViewModel.TieupCodesFileTypes1 = fileType;

            ViewBag.FielType = fileType;
        }

        //[HttpPost]
        //public ActionResult GetDefaultDataValue(string defaultval)
        //{
        //    Trace.WriteLine("defaultval: " + defaultval);

        //    IList<VREMIT.ModelMVC.Ripple_Data_Map_Detail> tieupcode;

        //    sqlCompositeModel customsqlCode = new sqlCompositeModel
        //    {
        //        model = new VREMIT.ModelMVC.Ripple_Data_Map_Detail(),
        //        sql = String.Format("Select det.* from tbl_ripple_data_map_detail det " +
        //                                " INNER JOIN tbl_ripple_data_map main" +
        //                                " ON det.validator_id = main.validator_id" +
        //                                " INNER JOIN tbl_dars_api_elements elem" +
        //                                " ON det.api_fieldname = elem.api_fieldname" +
        //                                " where main.tieup_code='{0}' ORDER BY elem.Id, elem.is_Mandatory", defaultval)
        //    };
        //    tieupcode = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Ripple_Data_Map_Detail>>(dal.GetCustomSQL(customsqlCode, "Ripple_Data_Map_Detail", g_global_region_code));

        //    List<DefaultDataValue> keyValuePairs = new List<DefaultDataValue>();

        //    foreach (var i in tieupcode)
        //    {
        //        DefaultDataValue defaultdata = new DefaultDataValue();
        //        defaultdata.FieldName = i.pk_api_fieldname;
        //        defaultdata.DefaultValue = i.def_value;

        //        keyValuePairs.Add(defaultdata);
        //    }
        //    return Json(keyValuePairs);
        //}

        public ActionResult RequestHistory()
        {
            if (!AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_CREATE) &&
                !AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_MODIFY) &&
                !AccessRightsHelper.HasRights(_currentUser.AccessRights, ModelViewer.TIEUP_FILE_MAPPER, ModelViewer.ACTION_DELETE))
            {
                return View("~/Views/Home/NoRights.cshtml");
            }
         
            try
            {

                IList<VREMIT.ModelMVC.vwRequestHistFormatMapper> requestHist;
                sqlCompositeModel customsqlCode = new sqlCompositeModel
                {
                    model = new VREMIT.ModelMVC.vwRequestHistFormatMapper(),
                    sql = String.Format("SELECT * FROM tbl_vwRequestHistFormatMapper WHERE maker='{0}';", _currentUser.Username)
                };
                requestHist = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.vwRequestHistFormatMapper>>(dal.GetCustomSQL(customsqlCode, "vwRequestHistFormatMapper", g_global_region_code));

                //search
                IList<VREMIT.ModelMVC.Tieup_Codes_FileType> formViewModel;
                sqlCompositeModel customsqlCode2 = new sqlCompositeModel();
                customsqlCode2.model = new VREMIT.ModelMVC.Tieup_Codes_FileType();
                customsqlCode2.sql = "SELECT file_type FROM [tbl_tieup_codes_fileType] where file_type <> '' ";
                formViewModel = JsonConvert.DeserializeObject<List<VREMIT.ModelMVC.Tieup_Codes_FileType>>(dal.GetCustomSQL(customsqlCode2, "Tieup_Codes_FileType", g_global_region_code));
                ViewBag.formViewModel = formViewModel;

               TempData["RequestHist"] = requestHist;

                ViewBag.RequestHist = requestHist;

                ViewBag.STitle = TempData["title"];
                ViewBag.SText = TempData["text"];
                ViewBag.SIcon = TempData["icon"];
            }
            catch (Exception ex)
            {
                LOGGER.Error(ex.Message, ex);

                ViewBag.Status = -1;
                ViewBag.ErrorMessage = "An error encountered while processing request.";
            }

            return View();
        }
    }
}