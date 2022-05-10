using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Model
{
    public class UserInfoDataTable : CommonDataTable
    {
        public int u_se { get; set; }
        public string u_id { get; set; }
        public string u_nm { get; set; }
        public string zip1_nu { get; set; }
        public string zip2_nu { get; set; }
        public string address_if { get; set; }
        public string phone_nu { get; set; }
        public string cphone_nu { get; set; }
        public string email_if { get; set; }
        public string sex_if { get; set; }
        public string job_if { get; set; }
        public string team_if { get; set; }
        public bool sms_ck { get; set; }
        public bool ssn_ck { get; set; }
        public bool ipin_ck { get; set; }
        public string join_dt { get; set; }
        // 추가
        public string ci { get; set; }
    }
}