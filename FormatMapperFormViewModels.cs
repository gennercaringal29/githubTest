using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using VREMIT.ModelMVC;

namespace DARS.Web.Models
{

    public class TieupViewMaintInputs
    {
        [Required(ErrorMessage = "Tie-Up is required")]
        public string tieup_code_id { get; set; }

        [Display(Name = "Tie-Up:")]
        [Required(ErrorMessage = "Tie-Up is required")]
        public string tieup_codes { get; set; }

        [Display(Name = "File Type:")]
        [Required(ErrorMessage = "File type is required")]
        public string file_type { get; set; }
        public string dt_last_chg { get; set; }
        public byte? status { get; set; }

        [Display(Name = "Remarks:")]
        [Required(ErrorMessage = "Remarks is required")]
        [RegularExpression(@"^(?!\s).*$", ErrorMessage = "Invalid value for Remarks")]
        [MaxLength(80, ErrorMessage = "Invalid Remarks Length")]
        public string remarks { get; set; }

        [Display(Name = "Start Record:")]
        public string start_record { get; set; }

        [Display(Name = "Settlement Currency:")]       
        public string settlement_currency { get; set; }



        public string filenameV { get; set; }
        public string footerV { get; set; }

        public int Action { get; set; }      
    }

    public class TieupViewDetailInputs : TieupViewMaintInputs
    {

        public string fieldName { get; set; }
        //public string tieup_code_id { get; set; }
        //public string file_type { get; set; }

        [Display(Name = "Delimiter Type:")]
        public string delimiter_type { get; set; }


        [Display(Name = "Delimiter:")]
        [Required(ErrorMessage = "Delmiter is required")]
        public string delimiter { get; set; }

        [RegularExpression("[^a-zA-Z0-9]+", ErrorMessage = "Invalid value for delimiter")]
        [Required(ErrorMessage = "value is required")]
        public string delimiter_val { get; set; }
        public string detail_tieup_id { get; set; }
        //public string dt_last_chg { get; set; }
        //public byte? status { get; set; }
        //public string remarks { get; set; }
        //public string settlement_currency { get; set; }

        [Display(Name = "Application Number Prefix:")]
        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Application Number Prefix")]
        //[Required(ErrorMessage = "Application Number Prefix is required")]
        [MaxLength(3, ErrorMessage = "Invalid Application Number Prefix Length")]
        public string d_prefix { get; set; }


        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Start Record")]
        [Required(ErrorMessage = "Start record is required")]
        public string detail_start_record { get; set; }


        // ============= DETAIL TABLE ============

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Transaction Date Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Transaction Date Length")]
        //[Required(ErrorMessage = "Transaction Date Start Column is required")]
        public string detail_transdate_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Transaction Date Length")]
        [MaxLength(3, ErrorMessage = "Invalid Transaction Date Length")]
        //[Required(ErrorMessage = "Transaction Date Length is required")]
        public string detail_transdate_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Application Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Application Number Length")]
        [Required(ErrorMessage = "Application Number Start Column is required")]
        public string detail_application_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Application Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Application Number Length")]
        [Required(ErrorMessage = "Application Number Length is required")]
        public string detail_application_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter ID Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter ID Length")]
        [Required(ErrorMessage = "Remitter ID Start Column is required")]
        public string detail_remitid_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter ID Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter ID Length")]
        [Required(ErrorMessage = "Remitter ID Length is required")]
        public string detail_remitid_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter First Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter First Name Length")]
        [Required(ErrorMessage = "Remitter First Name Start Column is required")]
        public string detail_remitfname_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter First Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter First Name Length")]
        [Required(ErrorMessage = "Remitter First Name Length is required")]
        public string detail_remitfname_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Middle Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Middle Name Length")]
        [Required(ErrorMessage = "Remitter Middle Name Start Column is required")]
        public string detail_remitmidname_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Middle Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Middle Name Length")]
        [Required(ErrorMessage = "Remitter Middle Name Length is required")]
        public string detail_remitmidname_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Last Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Last Name Length")]
        [Required(ErrorMessage = "Remitter Last Name Start Column is required")]
        public string detail_remitlastname_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Last Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Last Name Length")]
        [Required(ErrorMessage = "Remitter Last Name Length is required")]
        public string detail_remitlastname_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Customer Type Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Customer Type Length")]
        [Required(ErrorMessage = "Remitter Customer Type Start Column is required")]
        public string d_remitCusType_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Customer Type Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Customer Type Length")]
        [Required(ErrorMessage = "Remitter Customer Type Length is required")]
        public string d_remitCusType_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Nationality Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Nationality Length")]
        [Required(ErrorMessage = "Remitter Nationality Start Column is required")]
        public string d_remitNationality_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Nationality Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Nationality Length")]
        [Required(ErrorMessage = "Remitter Nationality Length is required")]
        public string d_remitNationality_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 1 Length")]
        [Required(ErrorMessage = "Remitter Address 1 Start Column is required")]
        public string d_remitAdd1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 1 Length")]
        [Required(ErrorMessage = "Remitter Address 1 Length is required")]
        public string d_remitAdd1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 2 Length")]
        //[Required(ErrorMessage = "Remitter Address 2 Start Column is required")]
        public string d_remitAdd2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 2 Length")]
        //[Required(ErrorMessage = "Remitter Address 2 Length is required")]
        public string d_remitAdd2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 3 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 3 Length")]
        //[Required(ErrorMessage = "Remitter Address 3 Start Column is required")]
        public string d_remitAdd3_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 3 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 3 Length")]
        //[Required(ErrorMessage = "Remitter Address 3 Length is required")]
        public string d_remitAdd3_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 4 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 4 Length")]
        //[Required(ErrorMessage = "Remitter Address 4 Start Column is required")]
        public string d_remitAdd4_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Address 4 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Address 4 Length")]
        //[Required(ErrorMessage = "Remitter Address 4 Length is required")]
        public string d_remitAdd4_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Continent Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Continent Length")]
        public string d_remitContinent_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Continent Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Continent Length")]
        public string d_remitContinent_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Zip Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Zip Code Length")]
        public string d_remitZipCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Zip Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Zip Code Length")]
        public string d_remitZipCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Birth Date Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Birth Date Length")]
        [Required(ErrorMessage = "Remitter Birth Date Start Column is required")]
        public string d_remitBDate_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Birth Date Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Birth Date Length")]
        [Required(ErrorMessage = "Remitter Birth Date Length is required")]
        public string d_remitBDate_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Profession Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Profession Length")]
        public string d_remitProf_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Profession Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Profession Length")]
        public string d_remitProf_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Gender Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Gender Length")]
        public string d_remitGender_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Gender Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Gender Length")]
        public string d_remitGender_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Civil Status Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Civil Status Length")]
        public string d_remitCivilStat_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Civil Status Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Civil Status Length")]
        public string d_remitCivilStat_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter P.O. Box Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter P.O. Box Length")]
        public string d_remitPOBox_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter P.O. Box Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter P.O. Box Length")]
        public string d_remitPOBox_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Mobile Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Mobile Number Length")]
        [Required(ErrorMessage = "Remitter Mobile Number Start Column is required")]
        public string d_remitMobPhoneNum_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Mobile Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Mobile Number Length")]
        [Required(ErrorMessage = "Remitter Mobile Number Length is required")]
        public string d_remitMobPhoneNum_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Office Phone Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Office Phone Number Length")]
        public string d_remitOfficePhoneNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Office Phone Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Office Phone Number Length")]
        public string d_remitOfficePhoneNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Email Address Start Column")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Email Address Length")]
        public string d_remitEmailAdd_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Email Address Length")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Email Address Length")]
        public string d_remitEmailAdd_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Tax Id No. Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Tax Id No. Length")]
        public string d_remitTaxIDNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Tax Id No. Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Tax Id No. Length")]
        public string d_remitTaxIDNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Type 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Type 1 Length")]
        [Required(ErrorMessage = "Remitter Id Type 1 Start Column is required")]
        public string d_remitIDType1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Type 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Type 1 Length")]
        [Required(ErrorMessage = "Remitter Id Type 1 Length is required")]
        public string d_remitIDType1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Number 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Number 1 Length")]
        [Required(ErrorMessage = "Remitter Id Number 1 Start Column is required")]
        public string d_remitIDNum1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Number 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Number 1 Length")]
        [Required(ErrorMessage = "Remitter Id Number 1 Length is required")]
        public string d_remitIDNum1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Issue At 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Issue At 1 Length")]
        [Required(ErrorMessage = "Remitter Id Issue At 1 Start Column is required")]
        public string d_remitIDIssueAt1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Issue At 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Issue At 1 Length")]
        [Required(ErrorMessage = "Remitter Id Issue At 1 Length is required")]
        public string d_remitIDIssueAt1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Expiry 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Expiry 1 Length")]
        [Required(ErrorMessage = "Remitter Id Expiry 1 Start Column is required")]
        public string d_remitIDExpDate1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Expiry 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Expiry 1 Length")]
        [Required(ErrorMessage = "Remitter Id Expiry 1 Length is required")]
        public string d_remitIDExpDate1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Type 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Type 2 Length")]
        public string d_remitIDType2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Type 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Type 2 Length")]
        public string d_remitIDType2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Number 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Number 2 Length")]
        public string d_remitIDNo2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Number 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Number 2 Length")]
        public string d_remitIDNo2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Issue At 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Issue At 2 Length")]
        public string d_remitIDIssueAt2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Issue At 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Issue At 2 Length")]
        public string d_remitIDIssueAt2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Expiry Date 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Expiry Date 2 Length")]
        public string d_remitIDExpDate2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Id Expiry Date 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Id Expiry Date 2 Length")]
        public string d_remitIDExpDate2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Notification Type Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Notification Type Length")]
        public string d_remitNotifType_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Notification Type Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Notification Type Length")]
        public string d_remitNotifType_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Id Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Id Length")]
        public string d_beneficiaryID_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Id Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Id Length")]
        public string d_beneficiaryID_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary First Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary First Name Length")]
        [Required(ErrorMessage = "Beneficiary First Name Start Column is required")]
        public string d_beneficiaryFName_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary First Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary First Name Length")]
        [Required(ErrorMessage = "Beneficiary First Name Length is required")]
        public string d_beneficiaryFName_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Middle Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Middle Name Length")]
        [Required(ErrorMessage = "Beneficiary Middle Name Start Column is required")]
        public string d_beneficiaryMName_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Middle Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Middle Name Length")]
        [Required(ErrorMessage = "Beneficiary Middle Name Length is required")]
        public string d_beneficiaryMName_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Last Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Last Name Length")]
        [Required(ErrorMessage = "Beneficiary Last Name Start Column is required")]
        public string d_beneficiaryLName_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Last Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Last Name Length")]
        [Required(ErrorMessage = "Beneficiary Last Name Length is required")]
        public string d_beneficiaryLName_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Customer Type Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Customer Type Length")]
        [Required(ErrorMessage = "Beneficiary Customer Type Start Column is required")]
        public string d_beneficiaryCusType_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Customer Type Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Customer Type Length")]
        [Required(ErrorMessage = "Beneficiary Customer Type Length is required")]
        public string d_beneficiaryCusType_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Address 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Address 1 Length")]
        [Required(ErrorMessage = "Beneficiary Address 1 Start Column is required")]
        public string d_beneficiaryAdd1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Address 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Address 1 Length")]
        [Required(ErrorMessage = "Beneficiary Address 1 Length is required")]
        public string d_beneficiaryAdd1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Address 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Address 2 Length")]
        //[Required(ErrorMessage = "Beneficiary Address 2 Start Column is required")]
        public string d_beneficiaryAdd2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Address 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Address 2 Length")]
        //[Required(ErrorMessage = "Beneficiary Address 2 Length is required")]
        public string d_beneficiaryAdd2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Address 3 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Address 3 Length")]
        //[Required(ErrorMessage = "Beneficiary Address 3 Start Column is required")]
        public string d_beneficiaryAdd3_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Address 3 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Address 3 Length")]
        //[Required(ErrorMessage = "Beneficiary Address 3 Length is required")]
        public string d_beneficiaryAdd3_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Country Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Country Length")]
        //[Required(ErrorMessage = "Beneficiary Country Start Column is required")]
        public string d_beneficiaryCountry_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Country Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Country Length")]
        //[Required(ErrorMessage = "Beneficiary Country Length is required")]
        public string d_beneficiaryCountry_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Zip Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Zip Code Length")]
        [Required(ErrorMessage = "Beneficiary Zip Code Start Column is required")]
        public string d_zipCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Zip Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Zip Code Length")]
        [Required(ErrorMessage = "Beneficiary Zip Code Length is required")]
        public string d_zipCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Landmark Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Landmark Length")]
        public string d_landMark_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Landmark Length")]
        [MaxLength(3, ErrorMessage = "Invalid Landmark Length")]
        public string d_landMark_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Nationality Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Nationality Length")]
        [Required(ErrorMessage = "Beneficiary Nationality Start Column is required")]
        public string d_beneficiaryNationality_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Nationality Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Nationality Length")]
        [Required(ErrorMessage = "Beneficiary Nationality Length is required")]
        public string d_beneficiaryNationality_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Birth Date Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Birth Date Length")]
        //[Required(ErrorMessage = "Beneficiary Birth Date Start Column is required")]
        public string d_beneficiaryBDate_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Birth Date Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Birth Date Length")]
        //[Required(ErrorMessage = "Beneficiary Birth Date Length is required")]
        public string d_beneficiaryBDate_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Profession Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Profession Length")]
        public string d_beneficiaryProf_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Profession Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Profession Length")]
        public string d_beneficiaryProf_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Gender Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Gender Length")]
        public string d_beneficiaryGender_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Gender Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Gender Length")]
        public string d_beneficiaryGender_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Civil Status Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Civil Status Length")]
        public string d_beneficiaryCivilStat_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Civil Status Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Civil Status Length")]
        public string d_beneficiaryCivilStat_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary P.O. Box Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary P.O. Box Length")]
        public string d_beneficiaryPOBox_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary P.O. Box Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary P.O. Box Length")]
        public string d_beneficiaryPOBox_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Relationship to the Remitter Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Relationship to the Remitter Length")]
        [Required(ErrorMessage = "Beneficiary Relationship to the Remitter Start Column is required")]
        public string d_beneficiaryRel_tothe_remit_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Relationship to the Remitter Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Relationship to the Remitter Length")]
        [Required(ErrorMessage = "Beneficiary Relationship to the Remitter Length is required")]
        public string d_beneficiaryRel_tothe_remit_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for  Alternate Recipient's Relation to Beneficiary Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Alternate Recipient's Relation to Beneficiary Length")]
        public string d_alterRecipient_reltobeneficiary_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Alternate Recipient's Relation to Beneficiary Length")]
        [MaxLength(3, ErrorMessage = "Invalid Alternate Recipient's Relation to Beneficiary Length")]
        public string d_alterRecipient_reltobeneficiary_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Mobile Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Mobile Number Length")]
        [Required(ErrorMessage = "Beneficiary Mobile Number Start Column is required")]
        public string d_beneficiaryMobPhoneNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Mobile Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Mobile Number Length")]
        [Required(ErrorMessage = "Beneficiary Mobile Number Length is required")]
        public string d_beneficiaryMobPhoneNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Office Phone Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Office Phone Number Length")]
        public string d_beneficiaryOfficePhoneNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Office Phone Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Office Phone Number Length")]
        public string d_beneficiaryOfficePhoneNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Email Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Email Length")]
        //[Required(ErrorMessage = "Beneficiary Email Address Start Column is required")]
        public string d_beneficiaryEmailAdd_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Email Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Email Length")]
        //[Required(ErrorMessage = "Beneficiary Email Address Length is required")]
        public string d_beneficiaryEmailAdd_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Tax Id No. Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Tax Id No. Length")]
        public string d_beneficiaryTaxIDNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Tax Id No. Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Tax Id No. Length")]
        public string d_beneficiaryTaxIDNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Notification Type Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Notification Type Length")]
        public string d_beneficiaryNotifType_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Beneficiary Notification Type Length")]
        [MaxLength(3, ErrorMessage = "Invalid Beneficiary Notification Type Length")]
        public string d_beneficiaryNotifType_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Funding Currency Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Funding Currency Length")]
        [Required(ErrorMessage = "Funding Currency Start Column is required")]
        public string d_fundCurrency_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Funding Currency Length")]
        [MaxLength(3, ErrorMessage = "Invalid Funding Currency Length")]
        [Required(ErrorMessage = "Funding Currency Length is required")]
        public string d_fundCurrency_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Funding Amount Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Funding Amount Length")]
        [Required(ErrorMessage = "Funding Amount Start Column is required")]
        public string d_fundAmount_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Funding Amount Length")]
        [MaxLength(3, ErrorMessage = "Invalid Funding Amount Length")]
        [Required(ErrorMessage = "Funding Amount Length is required")]
        public string d_fundAmount_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Buying Rate Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Buying Rate Length")]
        public string d_buyingRate_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Buying Rate Length")]
        [MaxLength(3, ErrorMessage = "Invalid Buying Rate Length")]
        public string d_buyingRate_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Settlement Currency Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Settlement Currency Length")]
        [Required(ErrorMessage = "Settlement Currency Start Column is required")]
        public string d_settlementCurrency_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Settlement Currency Length")]
        [MaxLength(3, ErrorMessage = "Invalid Settlement Currency Length")]
        [Required(ErrorMessage = "Settlement Currency Length is required")]
        public string d_settlementCurrency_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Settlement Amount Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Settlement Amount Length")]
        [Required(ErrorMessage = "Settlement Amount Start Column is required")]
        public string d_settlementAmount_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Settlement Amount Length")]
        [MaxLength(3, ErrorMessage = "Invalid Settlement Amount Length")]
        [Required(ErrorMessage = "Settlement Amount Length is required")]
        public string d_settlementAmount_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Settlement Mode Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Settlement Mode Length")]
        [Required(ErrorMessage = "Settlement Mode Start Column is required")]
        public string d_settlementMode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Settlement Mode Length")]
        [MaxLength(3, ErrorMessage = "Invalid Settlement Mode Length")]
        [Required(ErrorMessage = "Settlement Mode Length is required")]
        public string d_settlementMode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Bank Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Bank Code Length")]
        [Required(ErrorMessage = "Bank Code Start Column is required")]
        public string d_bankCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Bank Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Bank Code Length")]
        [Required(ErrorMessage = "Bank Code Length is required")]
        public string d_bankCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Bank Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Bank Name Length")]
        //[Required(ErrorMessage = "Bank Name Start Column is required")]
        public string d_bankName_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Bank Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Bank Name Length")]
        //[Required(ErrorMessage = "Bank Name Length is required")]
        public string d_bankName_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Branch Code Start Column")]
        [MaxLength(4, ErrorMessage = "Invalid Branch Code Length")]
        public string d_branchCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Branch Code Length")]
        [MaxLength(4, ErrorMessage = "Invalid Branch Code Length")]
        public string d_branchCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Branch Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Branch Name Length")]
        public string d_branchName_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Branch Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Branch Name Length")]
        public string d_branchName_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Account Type Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Account Type Length")]
        public string d_accountType_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Account Type Length")]
        [MaxLength(3, ErrorMessage = "Invalid Account Type Length")]
        public string d_accountType_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Account Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Account Number Length")]
        [Required(ErrorMessage = "Account Number Start Column is required")]
        public string d_accountNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Account Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Account Number Length")]
        [Required(ErrorMessage = "Account Number Length is required")]
        public string d_accountNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Gold Card Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Gold Card Number Length")]
        public string d_goldCardNo_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Gold Card Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Gold Card Number Length")]
        public string d_goldCardNo_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 1 Length")]
        [Required(ErrorMessage = "Payment Field 1 Start Column is required")]
        public string d_billsPayField1_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 1 Length")]
        [Required(ErrorMessage = "Payment Field 1 Length is required")]
        public string d_billsPayField1_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 2 Length")]
        [Required(ErrorMessage = "Payment Field 2 Start Column is required")]
        public string d_billsPayField2_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 2 Length")]
        [Required(ErrorMessage = "Payment Field 2 Length is required")]
        public string d_billsPayField2_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 3 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 3 Length")]
        [Required(ErrorMessage = "Payment Field 3 Start Column is required")]
        public string d_billsPayField3_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 3 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 3 Length")]
        [Required(ErrorMessage = "Payment Field 3 Length is required")]
        public string d_billsPayField3_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 4 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 4 Length")]
        //[Required(ErrorMessage = "Payment Field 4 Start Column is required")]
        public string d_billsPayField4_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 4 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 4 Length")]
        //[Required(ErrorMessage = "Payment Field 4 Length is required")]
        public string d_billsPayField4_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 5 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 5 Length")]
        //[Required(ErrorMessage = "Payment Field 5 Start Column is required")]
        public string d_billsPayField5_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Payment Field 5 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Payment Field 5 Length")]
        //[Required(ErrorMessage = "Payment Field 5 Length is required")]
        public string d_billsPayField5_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Outlet Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Outlet Code Length")]
        [Required(ErrorMessage = "Outlet Code Start Column is required")]
        public string d_outletCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Outlet Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Outlet Code Length")]
        [Required(ErrorMessage = "Outlet Code Length is required")]
        public string d_outletCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Outlet Branch Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Outlet Branch Code Length")]
        //[Required(ErrorMessage = "Outlet Branch Code Start Column is required")]
        public string d_outletBranchCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Outlet Branch Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Outlet Branch Code Length")]
        //[Required(ErrorMessage = "Outlet Branch Code Length is required")]
        public string d_outletBranchCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Alternate Beneficiary Name Start Column")]
        [MaxLength(50, ErrorMessage = "Invalid Alternate Beneficiary Name Length")]
        public string d_alterBeneficiaryName_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Alternate Beneficiary Name Length")]
        [MaxLength(50, ErrorMessage = "Invalid Alternate Beneficiary Name Length")]
        public string d_alterBeneficiaryName_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Alternate Beneficiary Relation to Beneficiary Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Alternate Beneficiary Relation to Beneficiary Length")]
        public string d_alterBeneficiary_relto_beneficiary_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Alternate Beneficiary Relation to Beneficiary Length")]
        [MaxLength(3, ErrorMessage = "Invalid Alternate Beneficiary Relation to Beneficiary Length")]
        public string d_alterBeneficiary_relto_beneficiary_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Message to Beneficiary Start Column")]
        [MaxLength(255, ErrorMessage = "Invalid Message to Beneficiary Length")]
        public string d_messageToBeneficiary_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Message to Beneficiary Length")]
        [MaxLength(255, ErrorMessage = "Invalid Message to Beneficiary Length")]
        public string d_messageToBeneficiary_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Receiver’s Correspondent Bank Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Receiver’s Correspondent Bank Length")]
        public string d_receiverCorresBank_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Receiver’s Correspondent Bank Length")]
        [MaxLength(3, ErrorMessage = "Invalid Receiver’s Correspondent Bank Length")]
        public string d_receiverCorresBank_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Sender’s Correspondent Bank Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Sender’s Correspondent Bank Length")]
        public string d_senderCorresBank_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Sender’s Correspondent Bank Length")]
        [MaxLength(3, ErrorMessage = "Invalid Sender’s Correspondent Bank Length")]
        public string d_senderCorresBank_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Sending Bank Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Sending Bank Length")]
        public string d_sendingBank_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Sending Bank Length")]
        [MaxLength(3, ErrorMessage = "Invalid Sending Bank Length")]
        public string d_sendingBank_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Receiving Bank Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Receiving Bank Length")]
        public string d_receivingBank_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Receiving Bank Length")]
        [MaxLength(3, ErrorMessage = "Invalid Receiving Bank Length")]
        public string d_receivingBank_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Mode of Charge Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Mode of Charge Length")]
        public string d_modeOfChange_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Mode of Charge Length")]
        [MaxLength(3, ErrorMessage = "Invalid Mode of Charge Length")]
        public string d_modeOfChange_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Purpose Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Purpose Code Length")]
        public string d_purposeCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Purpose Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Purpose Code Length")]
        public string d_purposeCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Indv Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Indv Code Length")]
        public string d_indvCode_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Indv Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Indv Code Length")]
        public string d_indvCode_length { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Start Column")]
        [MaxLength(15, ErrorMessage = "Invalid Length")]
        public string d_detailHash_start_column { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Length")]
        [MaxLength(15, ErrorMessage = "Invalid Length")]
        public string d_detailHash_length { get; set; }


        //---Default Data---
        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Transaction Date Default Value")]
        public string transactionDate { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Application Number Default Value")]
        //[Required(ErrorMessage = "Application Number Default Value is required")]
        [MaxLength(40, ErrorMessage = "Invalid Application Number Length")]
        public string applicationNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Default Value")]
        //[Required(ErrorMessage = "Remitter ID Default Value is required")]
        [MaxLength(32, ErrorMessage = "Invalid Remitter ID Length")]
        public string remitterId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter First Name Default Value")]
        //[Required(ErrorMessage = "Remitter First Name Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter First Name Length")]
        public string remitterFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Middle Name Default Value")]
        //[Required(ErrorMessage = "Remitter Middle Name Length is required")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Middle Name Length")]
        public string remitterMiddleName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Last Name Default Value")]
        //[Required(ErrorMessage = "Remitter Last Name Length is required")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Last Name Length")]
        public string remitterLastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Customer Type Default Value")]
        //[Required(ErrorMessage = "Remitter Customer Type Default Value is required")]
        [MaxLength(1, ErrorMessage = "Invalid Remitter Customer Type Length")]
        public string remitterCustomerType { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Nationality Default Value")]
        //[Required(ErrorMessage = "Remitter Nationality Default Value is required")]
        [MaxLength(30, ErrorMessage = "Invalid Remitter Nationality Length")]
        public string remitterNationality { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Address 1 Default Value")]
        //[Required(ErrorMessage = "Remitter Address 1 Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Address 1 Length")]
        public string remitterAddress1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Address 2 Default Value")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Address 2 Length")]
        public string remitterAddress2 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Address 3 Default Value")]
        [MaxLength(100, ErrorMessage = "Invalid Remitter Address 3 Length")]
        public string remitterAddress3 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Address 4 Default Value")]
        [MaxLength(35, ErrorMessage = "Invalid Remitter Address 4 Length")]
        public string remitterAddress4 { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Continent Default Value")]
        //public string remitterContinent { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Zip Code Default Value")]
        //public string remitterZipCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Birth Date Default Value")]
        //[Required(ErrorMessage = "Remitter Birth Date Default Value is required")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter Birth Date Length")]
        public string remitterBirthDate { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Profession Default Value")]
        //public string remitterProfession { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Gender Default Value")]
        //public string remitterGender { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Civil Status Default Value")]
        //public string remitterCivilStatus { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter P.O. Box Default Value")]
        //public string remitterPoBox { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Mobile Number Default Value")]
        //[Required(ErrorMessage = "Remitter Mobile Number Default Value is required")]
        [MaxLength(15, ErrorMessage = "Invalid Remitter Mobile Number Length")]
        public string remitterMobileNumber { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Office Number Default Value")]
        //public string remitterOfficeNumber { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Email Default Value")]
        //public string remitterEmail { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter TIN Default Value")]
        //public string remitterTin { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Type 1 Default Value")]
        //[Required(ErrorMessage = "Remitter ID Type 1 Default Value is required")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter ID Type 1 Length")]
        public string remitterIdType1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Id Number 1 Default Value")]
        //[Required(ErrorMessage = "Remitter Id Number 1 Default Value is required")]
        [MaxLength(60, ErrorMessage = "Invalid Remitter Id Number 1 Length")]
        public string remitterIdNumber1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Id Issued At 1 Default Value")]
        //[Required(ErrorMessage = "Remitter Id Issued At 1 Default Value is required")]
        [MaxLength(60, ErrorMessage = "Invalid Remitter Id Issued At 1 Length")]
        public string remitterIdIssuedAt1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Expiry 1 Default Value")]
        //[Required(ErrorMessage = "Remitter ID Expiry 1 Default Value is required")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter ID Expiry 1 Length")]
        public string remitterIdExpiry1 { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Type 2 Default Value")]
        //public string remitterIdType2 { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Number 2 Default Value")]
        //public string remitterIdNumber2 { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Issued At 2 Default Value")]
        //public string remitterIdIssuedAt2 { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter ID Expiry 2 Default Value")]
        //public string remitterIdExpiry2 { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Notification Type Default Value")]
        //public string remitterNotificationType { get; set; }

        //// Beneficiary properties
        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary ID Default Value")]
        //public string beneficiaryId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary First Name Default Value")]
        //[Required(ErrorMessage = "Beneficiary First Name Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Beneficiary First Name Length")]
        public string beneficiaryFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Middle Name Default Value")]
        //[Required(ErrorMessage = "Beneficiary Middle Name Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Beneficiary Middke Name Length")]
        public string beneficiaryMiddleName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Last Name Default Value")]
        //[Required(ErrorMessage = "Beneficiary Last Name Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Beneficiary Last Name Length")]
        public string beneficiaryLastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Customer Type Default Value")]
        //[Required(ErrorMessage = "Beneficiary Customer Type Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Beneficiary Customer Type Length")]
        public string beneficiaryCustomerType { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Address 1 Default Value")]
        //[Required(ErrorMessage = "Beneficiary Address 1 Default Value is required")]
        [MaxLength(100, ErrorMessage = "Invalid Beneficiary Address 1 Length")]
        public string beneficiaryAddress1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Address 2 Default Value")]
        public string beneficiaryAddress2 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Address 3 Default Value")]
        public string beneficiaryAddress3 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Country Default Value")]
        public string beneficiaryCountry { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Zip Code Default Value")]
        //[Required(ErrorMessage = "Beneficiary Zip Code Default Value is required")]
        [MaxLength(10, ErrorMessage = "Invalid Beneficiary Zip Code Length")]
        public string beneficiaryZipCode { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Landmark Default Value")]
        //public string beneficiaryLandmark { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Nationality Default Value")]
        //[Required(ErrorMessage = "Beneficiary Nationality Default Value is required")]
        [MaxLength(30, ErrorMessage = "Invalid Beneficiary Nationality Length")]
        public string beneficiaryNationality { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Birth Date. Date format must be MM/dd/yyyy Default Value")]
        public string beneficiaryBirthDate { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Profession Default Value")]
        //public string beneficiaryProfession { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Gender Default Value")]
        //public string beneficiaryGender { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Civil Status Default Value")]
        //public string beneficiaryCivilStatus { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary P.O. Box Default Value")]
        //public string beneficiaryPoBox { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Relation to Remitter Default Value")]
        //[Required(ErrorMessage = "Beneficiary Relation to Remitter Default Value is required")]
        [MaxLength(30, ErrorMessage = "Invalid Beneficiary Relation to Remitter Length")]
        public string beneficiaryRelationToRemitter { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Alternate Beneficiary Relation to Beneficiary Default Value")]
        //public string alternateBeneficiaryRelationToBeneficiary { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Mobile Number Default Value")]
        //[Required(ErrorMessage = "Beneficiary Mobile Number Default Value is required")]
        [MaxLength(15, ErrorMessage = "Invalid Beneficiary Mobile Number Length")]
        public string beneficiaryMobileNumber { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Office Number Default Value")]
        //public string beneficiaryOfficeNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Email Default Value")]
        public string beneficiaryEmail { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary TIN Default Value")]
        //public string beneficiaryTin { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Beneficiary Notification Type Default Value")]
        //public string beneficiaryNotificationType { get; set; }

        // Funding properties
        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Funding Currency Default Value")]
        [Required(ErrorMessage = "Funding Currency Default Value is required")]
        [MaxLength(15, ErrorMessage = "Invalid Funding Currency Length")]
        public string fundingCurrency { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Funding Amount Default Value")]
        //[Required(ErrorMessage = "Funding Amount Default Value is required")]
        [MaxLength(18, ErrorMessage = "Invalid Funding Amount Length")]
        public string fundingAmount { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Buying Rate Default Value")]
        public string buyingRate { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Settlement Currency Default Value")]
        [Required(ErrorMessage = "Settlement Currency Default Value is required")]
        //[MaxLength(3, ErrorMessage = "Invalid Settlement Currency Length")]
        public string settlementCurrency { get; set; }

        public string settlementCurrencyHolder { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Settlement Amount Default Value")]
        //[Required(ErrorMessage = "Settlement Amount Default Value is required")]
        public string settlementAmount { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Settlement Mode Default Value")]
        //[Required(ErrorMessage = "Settlement Mode Default Value is required")]
        [MaxLength(2, ErrorMessage = "Invalid Settlement Mode Length")]
        public string settlementMode { get; set; }

        // Bank properties
        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Bank Code Default Value")]
        //[Required(ErrorMessage = "Bank Code Default Value is required")]
        [MaxLength(15, ErrorMessage = "Invalid Bank Code Length")]
        public string bankCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Bank Name Default Value")]
        public string bankName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Branch Code Default Value")]
        public string branchCode { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Branch Name Default Value")]
        //public string branchName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Account Type Default Value")]
        public string accountType { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Account Number Default Value")]
        //[Required(ErrorMessage = "Account Number Default Value is required")]
        [MaxLength(35, ErrorMessage = "Invalid Account Number Length")]
        public string accountNumber { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Gold Card Number Default Value")]
        //public string goldCardNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Payment Field 1 Default Value")]
        //[Required(ErrorMessage = "Payment Field 1 Default Value is required")]
        [MaxLength(10, ErrorMessage = "Invalid Payment Field 1 Length")]
        public string paymentField1 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Payment Field 2 Default Value")]
        //[Required(ErrorMessage = "Payment Field 2 Default Value is required")]
        [MaxLength(20, ErrorMessage = "Invalid Payment Field 2 Length")]
        public string paymentField2 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Payment Field 3 Default Value")]
        //[Required(ErrorMessage = "Payment Field 3 Default Value is required")]
        [MaxLength(20, ErrorMessage = "Invalid Payment Field 2 Length")]
        public string paymentField3 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Payment Field 4 Default Value")]
        public string paymentField4 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Payment Field 5 Default Value")]
        public string paymentField5 { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Outlet Code Default Value")]
        //[Required(ErrorMessage = "Outlet Code Default Value is required")]
        [MaxLength(30, ErrorMessage = "Invalid Outlet Code Length")]
        public string outletCode { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Outlet Branch Code Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Outlet Branch Code Length")]
        public string outletBranchCode { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Alternate Beneficiary Default Value")]
        //public string alternateBeneficiary { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Alternate Recipient Relation to Beneficiary Default Value")]
        //public string alternateRecipientRelationToBeneficiary { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Message to Beneficiary Default Value")]
        //public string messageToBeneficiary { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Receiver Correspondent Bank Default Value")]
        //public string receiverCorrespondentBank { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Sender Correspondent Bank Default Value")]
        //public string senderCorrespondentBank { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Sending Bank Default Value")]
        //public string sendingBank { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Receiving Bank Default Value")]
        //public string receivingBank { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Mode of Charge Default Value")]
        //public string modeOfCharge { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Purpose Code Default Value")]
        //public string purposeCode { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Individual Code Default Value")]
        //public string individualCode { get; set; }

        // New Added Fields
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Partner Code Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Partner Code Length")]
        [Required(ErrorMessage = "Partner Code Start Column is required")]
        public string PartnerCodeStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Partner Code Length")]
        [MaxLength(3, ErrorMessage = "Invalid Partner Code Length")]
        [Required(ErrorMessage = "Partner Code Length is required")]
        public string PartnerCodeLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Partner Code Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Partner Code Length")]
        [Required(ErrorMessage = "Partner Code Dafault Value is required")]
        public string PartnerCodeDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Account Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Account Name Length")]
        //[Required(ErrorMessage = "Account Name Start Column is required")]
        public string AcctNameStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Account Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Account Name Length")]
        //[Required(ErrorMessage = "Account Name Length is required")]
        public string AcctNameLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Account Name Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Account Name Length")]
        public string AcctNameDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Country Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Country Length")]
        [Required(ErrorMessage = "Remitter Country Start Column is required")]
        public string RemCountryStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Country Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Country Length")]
        [Required(ErrorMessage = "Remitter Country Length is required")]
        public string RemCountryLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Country Default Value")]
        [MaxLength(2, ErrorMessage = "Invalid Remitter Country Length")]
        //[Required(ErrorMessage = "Remitter Country Default Value is required")]
        public string RemCountryDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Fourth Name Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Fourth Name Length")]
        //[Required(ErrorMessage = "Remitter Fourth Name Start Column is required")]
        public string RemFourthNmeStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Fourth Name Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Fourth Name Length")]
        //[Required(ErrorMessage = "Remitter Fourth Name Length is required")]
        public string RemFourthNmeLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Fourth Name Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter Fourth Name Length")]
        public string RemFourthNmeDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Place of Birth Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Place of Birth Length")]
        //[Required(ErrorMessage = "Remitter Place of Birth Start Column is required")]
        public string RemPlaceBrthStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Place of Birth Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Place of Birth Length")]
        //[Required(ErrorMessage = "Remitter Place of Birth Length is required")]
        public string RemPlaceBrthLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Place of Birth Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter Place of Birth Length")]
        public string RemPlaceBrthDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Account Number Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Account Number Length")]
        //[Required(ErrorMessage = "Remitter Account Number Start Column is required")]
        public string RemAcctNumStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter Account Number Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter Account Number Length")]
        //[Required(ErrorMessage = "Remitter Account Number Length is required")]
        public string RemAcctNumLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter Account Number Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter Account Number Length")]
        public string RemAcctNumDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter IBAN Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter IBAN Length")]
        //[Required(ErrorMessage = "Remitter IBAN Start Column is required")]
        public string RemIBANStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter IBAN Length")]
        [MaxLength(3, ErrorMessage = "Invalid Remitter IBAN Length")]
        //[Required(ErrorMessage = "Remitter IBAN Length is required")]
        public string RemIBANLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Remitter IBAN Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Remitter IBAN Length")]
        public string RemIBANDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 1 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 1 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 1 Start Column is required")]
        public string Res1StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 1 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 1 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 1 Length is required")]
        public string Res1Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 1 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 1 Length")]
        public string Res1Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 2 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 2 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 2 Start Column is required")]
        public string Res2StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 2 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 2 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 2 Length is required")]
        public string Res2Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 2 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 2 Length")]
        public string Res2Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 3 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 3 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 3 Start Column is required")]
        public string Res3StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 3 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 3 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 3 Length is required")]
        public string Res3Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 3 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 3 Length")]
        public string Res3Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 4 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 4 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 4 Start Column is required")]
        public string Res4StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 4 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 4 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 4 Length is required")]
        public string Res4Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 4 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 4 Length")]
        public string Res4Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 5 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 5 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 5 Start Column is required")]
        public string Res5StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 5 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 5 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 5 Length is required")]
        public string Res5Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 5 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 5 Length")]
        public string Res5Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 6 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 6 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 6 Start Column is required")]
        public string Res6StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 6 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 6 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 6 Length is required")]
        public string Res6Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 6 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 6 Length")]
        public string Res6Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 7 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 7 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 7 Start Column is required")]
        public string Res7StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 7 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 7 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 7 Length is required")]
        public string Res7Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 7 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 7 Length")]
        public string Res7Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 8 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 8 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 8 Start Column is required")]
        public string Res8StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 8 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 8 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 8 Length is required")]
        public string Res8Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 8 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 8 Length")]
        public string Res8Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 9 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 9 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 9 Start Column is required")]
        public string Res9StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 9 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 9 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 9 Length is required")]
        public string Res9Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 9 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 9 Length")]
        public string Res9Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Remitter 10 Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 10 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 10 Start Column is required")]
        public string Res10StartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Reserve 10 Length")]
        [MaxLength(3, ErrorMessage = "Invalid Reserve 10 Length")]
        //[Required(ErrorMessage = "Remitter Reserve 10 Length is required")]
        public string Res10Length { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Reserve 10 Default Value")]
        [MaxLength(10, ErrorMessage = "Invalid Reserve 10 Length")]
        public string Res10Default { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Source of Remittance Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Source of Remitter Length")]
        //[Required(ErrorMessage = "Source of Remittance Start Column is required")]
        public string SourceOfRemStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Source of Remittance Length")]
        [MaxLength(3, ErrorMessage = "Invalid Source of Remitter Length")]
        //[Required(ErrorMessage = "Source of Remittance Length is required")]
        public string SourceOfRemLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Source of Remittance Default Value")]
        [MaxLength(100, ErrorMessage = "Invalid Source of Remitter Length")]
        public string SourceOfRemDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Nature of Business Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Nature of Business Length")]
        [Required(ErrorMessage = "Nature of Business Start Column is required")]
        public string NatrueOfBussStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Nature of Business Length")]
        [MaxLength(3, ErrorMessage = "Invalid Nature of Business Length")]
        [Required(ErrorMessage = "Nature of Business Length is required")]
        public string NatrueOfBussLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Nature of Business Default Value")]
        [MaxLength(100, ErrorMessage = "Invalid Nature of Business Length")]
        //[Required(ErrorMessage = "Nature of Business Default Value is required")]
        public string NatrueOfBussDefault { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Purpose Of Remittance Start Column")]
        [MaxLength(3, ErrorMessage = "Invalid Purpose Of Remittance Length")]
        //[Required(ErrorMessage = "Purpose Of Remittance Start Column is required")]
        public string PurposeOfRemStartColumn { get; set; }

        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid value for Purpose Of Remittance Length")]
        [MaxLength(3, ErrorMessage = "Invalid Purpose Of Remittance Length")]
        //[Required(ErrorMessage = "Purpose Of Remittance Length is required")]
        public string PurposeOfRemLength { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9,]+(\.[0-9]+)?$", ErrorMessage = "Invalid value for Purpose Of Remittance Default Value")]
        [MaxLength(3, ErrorMessage = "Invalid Purpose Of Remittance Length")]
        public string PurposeOfRemDefault { get; set; }

        public string Provided { get; set; }

    }

    public class FormatMapperFormViewModel : TieupViewDetailInputs
    {
        public FormatMapperFormViewModel()
        {
            FieldPropertiesDetail = new List<FieldProperty>();
        }

        [Display(Name = "Request Type")]
        public int RequestType { get; set; }

        [Display(Name = "Request ID")]
        public string TranID { get; set; }

        [Display(Name = "Request By")]
        public string RequestBy { get; set; }

        [Display(Name = "Request Date/Time")]
        public string RequestDateTime { get; set; }
        public string RequestStatusReason { get; set; }

        [Display(Name = "Filename Validation:")]
        public string isFilenameValidation { get; set; }

        [Display(Name = "Footer Validation:")]
        public string isFooterValidation { get; set; }

        public TieupViewMaintInputs TieupViewMaintInputs { get; set; }
        public TieupViewDetailInputs TieupViewDetailInputs { get; set; }
        public IList<Tieup_Codes_Detail> Tieup_Codes_Detail { get; set; }
        public IList<Formatmapper_Fieldname_Details> Formatmapper_Fieldname_Details { get; set; }
        public IList<Tieup_Codes_FileType> TieupCodesFileTypes { get; set; }
        public IList<Tieup_Codes_FileType> TieupCodesFileTypes1 { get; set; }
        public IList<Tieup_Codes_FileType> TieupCodesFileTypes2 { get; set; }
        public IList<Tieup_Codes> TieupCodesMaintParam { get; set; }
        public IList<Tieup_Codes_Header> TieupCodesMaintHeaderParam { get; set; }
        public IList<Tieup_Codes_Request> TieupCodesRequest { get; set; }
        public IList<Tieup> TieupCode { get; set; }
        public IList<Tieup> TieupMot { get; set; }
        public IList<App_Code> App_Codes { get; set; }
        public IList<Settlement_Currency> SettlementCurrency1 { get; set; }
        public IList<FormatMapperViewAmendment> amendmentDatatable { get; set; }
        public IList<FieldProperty> FieldPropertiesDetail { get; set; }

    }
    public class FormatMapperViewAmendment
    {
        public string fieldName { get; set; }
        public string currentValue { get; set; }
        public string newValue { get; set; }
    }

    public class FormatMapperDetailFormViewModel: FormatMapperFormViewModel
    {
        public FormatMapperDetailFormViewModel()
        {
            FieldProperties = new List<FieldProperty>();
        }

        public string DetailTieupId { get; set; }
        [Display(Name = "Tie-Up")]
        public string TieupCodeId { get; set; }
        [Display(Name = "File Type")]
        public string FileType { get; set; }
        [Display(Name = "Delimiter Type")]
        public string DelimiterType { get; set; }
        public string Delimiter { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public string DateLastChange { get; set; }
        [Display(Name = "Start Record")]
        public string StartRecord { get; set; }
        //public string TransdateStartColumn { get; set; }
        //public string TransdateLength { get; set; }
        //public string ApplicationStartColumn { get; set; }
        //public string ApplicationLength { get; set; }
        //public string RemitidStartColumn { get; set; }
        //public string RemitidLength { get; set; }
        //public string RemitfnameStartColumn { get; set; }
        //public string RemitfnameLength { get; set; }
        //public string RemitmidnameStartColumn { get; set; }
        //public string RemitmidnameLength { get; set; }
        //public string RemitlastnameStartColumn { get; set; }
        //public string RemitlastnameLength { get; set; }
        //public string RemitCusTypeStartColumn { get; set; }
        //public string RemitCusTypeLength { get; set; }
        //public string RemitNationalityStartColumn { get; set; }
        //public string RemitNationalityLength { get; set; }
        //public string RemitAdd1StartColumn { get; set; }
        //public string RemitAdd1Length { get; set; }
        //public string RemitAdd2StartColumn { get; set; }
        //public string RemitAdd2Length { get; set; }
        //public string RemitAdd3StartColumn { get; set; }
        //public string RemitAdd3Length { get; set; }
        //public string RemitAdd4StartColumn { get; set; }
        //public string RemitAdd4Length { get; set; }
        //public string RemitContinentStartColumn { get; set; }
        //public string RemitContinentLength { get; set; }
        //public string RemitZipCodeStartColumn { get; set; }
        //public string RemitZipCodeLength { get; set; }
        //public string RemitBDateStartColumn { get; set; }
        //public string RemitBDateLength { get; set; }
        //public string RemitProfStartColumn { get; set; }
        //public string RemitProfLength { get; set; }
        //public string RemitGenderStartColumn { get; set; }
        //public string RemitGenderLength { get; set; }
        //public string RemitCivilStatStartColumn { get; set; }
        //public string RemitCivilStatLength { get; set; }
        //public string RemitPOBoxStartColumn { get; set; }
        //public string RemitPOBoxLength { get; set; }
        //public string RemitMobPhoneNumStartColumn { get; set; }
        //public string RemitMobPhoneNumLength { get; set; }
        //public string RemitOfficePhoneNoStartColumn { get; set; }
        //public string RemitOfficePhoneNoLength { get; set; }
        //public string RemitEmailAddStartColumn { get; set; }
        //public string RemitEmailAddLength { get; set; }
        //public string RemitTaxIDNoStartColumn { get; set; }
        //public string RemitTaxIDNoLength { get; set; }
        //public string RemitIDType1StartColumn { get; set; }
        //public string RemitIDType1Length { get; set; }
        //public string RemitIDNum1StartColumn { get; set; }
        //public string RemitIDNum1Length { get; set; }
        //public string RemitIDIssueAt1StartColumn { get; set; }
        //public string RemitIDIssueAt1Length { get; set; }
        //public string RemitIDExpDate1StartColumn { get; set; }
        //public string RemitIDExpDate1Length { get; set; }
        //public string RemitIDType2StartColumn { get; set; }
        //public string RemitIDType2Length { get; set; }
        //public string RemitIDNo2StartColumn { get; set; }
        //public string RemitIDNo2Length { get; set; }
        //public string RemitIDIssueAt2StartColumn { get; set; }
        //public string RemitIDIssueAt2Length { get; set; }
        //public string RemitIDExpDate2StartColumn { get; set; }
        //public string RemitIDExpDate2Length { get; set; }
        //public string RemitNotifTypeStartColumn { get; set; }
        //public string RemitNotifTypeLength { get; set; }
        //public string BeneficiaryIDStartColumn { get; set; }
        //public string BeneficiaryIDLength { get; set; }
        //public string BeneficiaryFNameStartColumn { get; set; }
        //public string BeneficiaryFNameLength { get; set; }
        //public string BeneficiaryMNameStartColumn { get; set; }
        //public string BeneficiaryMNameLength { get; set; }
        //public string BeneficiaryLNameStartColumn { get; set; }
        //public string BeneficiaryLNameLength { get; set; }
        //public string BeneficiaryCusTypeStartColumn { get; set; }
        //public string BeneficiaryCusTypeLength { get; set; }
        //public string BeneficiaryAdd1StartColumn { get; set; }
        //public string BeneficiaryAdd1Length { get; set; }
        //public string BeneficiaryAdd2StartColumn { get; set; }
        //public string BeneficiaryAdd2Length { get; set; }
        //public string BeneficiaryAdd3StartColumn { get; set; }
        //public string BeneficiaryAdd3Length { get; set; }
        //public string BeneficiaryCountryStartColumn { get; set; }
        //public string BeneficiaryCountryLength { get; set; }
        //public string ZipCodeStartColumn { get; set; }
        //public string ZipCodeLength { get; set; }
        //public string LandMarkStartColumn { get; set; }
        //public string LandMarkLength { get; set; }
        //public string BeneficiaryNationalityStartColumn { get; set; }
        //public string BeneficiaryNationalityLength { get; set; }
        //public string BeneficiaryBDateStartColumn { get; set; }
        //public string BeneficiaryBDateLength { get; set; }
        //public string BeneficiaryProfStartColumn { get; set; }
        //public string BeneficiaryProfLength { get; set; }
        //public string BeneficiaryGenderStartColumn { get; set; }
        //public string BeneficiaryGenderLength { get; set; }
        //public string BeneficiaryCivilStatStartColumn { get; set; }
        //public string BeneficiaryCivilStatLength { get; set; }
        //public string BeneficiaryPOBoxStartColumn { get; set; }
        //public string BeneficiaryPOBoxLength { get; set; }
        //public string BeneficiaryRelTotheRemitStartColumn { get; set; }
        //public string BeneficiaryRelTotheRemitLength { get; set; }
        //public string AlterRecipientReltobeneficiaryStartColumn { get; set; }
        //public string AlterRecipientReltobeneficiaryLength { get; set; }
        //public string BeneficiaryMobPhoneNoStartColumn { get; set; }
        //public string BeneficiaryMobPhoneNoLength { get; set; }
        //public string BeneficiaryOfficePhoneNoStartColumn { get; set; }
        //public string BeneficiaryOfficePhoneNoLength { get; set; }
        //public string BeneficiaryEmailAddStartColumn { get; set; }
        //public string BeneficiaryEmailAddLength { get; set; }
        //public string BeneficiaryTaxIDNoStartColumn { get; set; }
        //public string BeneficiaryTaxIDNoLength { get; set; }
        //public string BeneficiaryNotifTypeStartColumn { get; set; }
        //public string BeneficiaryNotifTypeLength { get; set; }
        //public string FundCurrencyStartColumn { get; set; }
        //public string FundCurrencyLength { get; set; }
        //public string FundAmountStartColumn { get; set; }
        //public string FundAmountLength { get; set; }
        //public string BuyingRateStartColumn { get; set; }
        //public string BuyingRateLength { get; set; }
        //public string SettlementCurrencyStartColumn { get; set; }
        //public string SettlementCurrencyLength { get; set; }
        //public string SettlementAmountStartColumn { get; set; }
        //public string SettlementAmountLength { get; set; }
        //public string SettlementModeStartColumn { get; set; }
        //public string SettlementModeLength { get; set; }
        //public string BankCodeStartColumn { get; set; }
        //public string BankCodeLength { get; set; }
        //public string BankNameStartColumn { get; set; }
        //public string BankNameLength { get; set; }
        //public string BranchCodeStartColumn { get; set; }
        //public string BranchCodeLength { get; set; }
        //public string BranchNameStartColumn { get; set; }
        //public string BranchNameLength { get; set; }
        //public string AccountTypeStartColumn { get; set; }
        //public string AccountTypeLength { get; set; }
        //public string AccountNoStartColumn { get; set; }
        //public string AccountNoLength { get; set; }
        //public string GoldCardNoStartColumn { get; set; }
        //public string GoldCardNoLength { get; set; }
        //public string BillsPayField1StartColumn { get; set; }
        //public string BillsPayField1Length { get; set; }
        //public string BillsPayField2StartColumn { get; set; }
        //public string BillsPayField2Length { get; set; }
        //public string BillsPayField3StartColumn { get; set; }
        //public string BillsPayField3Length { get; set; }
        //public string BillsPayField4StartColumn { get; set; }
        //public string BillsPayField4Length { get; set; }
        //public string BillsPayField5StartColumn { get; set; }
        //public string BillsPayField5Length { get; set; }
        //public string OutletCodeStartColumn { get; set; }
        //public string OutletCodeLength { get; set; }
        //public string OutletBranchCodeStartColumn { get; set; }
        //public string OutletBranchCodeLength { get; set; }
        //public string AlterBeneficiaryNameStartColumn { get; set; }
        //public string AlterBeneficiaryNameLength { get; set; }
        //public string AlterBeneficiaryReltoBeneficiaryStartColumn { get; set; }
        //public string AlterBeneficiaryReltoBeneficiaryLength { get; set; }
        //public string MessageToBeneficiaryStartColumn { get; set; }
        //public string MessageToBeneficiaryLength { get; set; }
        //public string ReceiverCorresBankStartColumn { get; set; }
        //public string ReceiverCorresBankLength { get; set; }
        //public string SenderCorresBankStartColumn { get; set; }
        //public string SenderCorresBankLength { get; set; }
        //public string SendingBankStartColumn { get; set; }
        //public string SendingBankLength { get; set; }
        //public string ReceivingBankStartColumn { get; set; }
        //public string ReceivingBankLength { get; set; }
        //public string ModeOfChangeStartColumn { get; set; }
        //public string ModeOfChangeLength { get; set; }
        //public string PurposeCodeStartColumn { get; set; }
        //public string PurposeCodeLength { get; set; }
        //public string IndvCodeStartColumn { get; set; }
        //public string IndvCodeLength { get; set; }
        //public string DetailHashStartColumn { get; set; }
        //public string DetailHashLength { get; set; }
        [Display(Name = "Application Number Prefix")]
        public string Prefix { get; set; }

        public IList<FieldProperty> FieldProperties { get; set; }
    }

    public class FieldProperty
    {
        public FieldProperty(string fieldName, string columnStart, string columnDefault, string columnLength, string checkBox)
        {
            FieldName = fieldName;
            ColumnStart = columnStart;
            ColumnDefaultData = columnDefault;
            ColumnLength = columnLength;
            CheckBox = checkBox;

            if (columnLength == null || columnLength.IsEmpty())
            {
                ColumnLength = "";
            }
            if (columnDefault == " " || columnDefault.IsEmpty())
            {
                ColumnDefaultData = "";
            }
            if (columnStart == " " || columnStart.IsEmpty())
            {
                ColumnStart = "";
            }

        }
        public string FieldName { get; set; }
        public string ColumnStart { get; set; }
        public string ColumnDefaultData { get; set; }
        public string ColumnLength { get; set; }
        public string CheckBox { get; set; }
    }

    public class DefaultDataValue
    {
        public HashSet<String> defaultDataValue { get; set; }
        public String FieldName { get; set; }
        public String DefaultValue { get; set; }
    }
}