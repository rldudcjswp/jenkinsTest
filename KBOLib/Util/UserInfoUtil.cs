using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Util
{
    [Serializable]
    public class UserInfo
    {
        public string U_SE;
        public string U_ID;
        public string U_NM;
        public string ZIP1_NU;
        public string ZIP2_NU;
        public string ADDRESS_IF;
        public string PHONE_NU;
        public string CPHONE_NU;
        public string EMAIL_IF;
        public string SEX_IF;
        public string JOB_IF;
        public string TEAM_IF;
        public string SMS_CK;
        public string NEWS_CK;
        public string SSN_CK;
        public string RESIDENT1_NU;
        public string RESIDENT2_CD;
        public string PSW_CD;
        public string REG_DT;
        public string U_IP;
        public string U_NO;
        public string IPIN_CK;
        public string AU_ID_CHK1; // 불량 회원 여부
    }

    // GetUserInfo 메서드에서 사용할 열거형 값
    public enum UserInfoType
    {
        U_SE
      , U_ID
      , U_NM
      , ZIP1_NU
      , ZIP2_NU
      , ADDRESS_IF
      , PHONE_NU
      , CPHONE_NU
      , EMAIL_IF
      , SEX_IF
      , JOB_IF
      , TEAM_IF
      , SMS_CK
      , NEWS_CK
      , SSN_CK
      , RESIDENT1_NU
      , RESIDENT2_CD
      , PSW_CD
      , REG_DT
      , U_IP
      , U_NO
      , IPIN_CK
      , AU_ID_CHK1
    }


    public class UserInfoUtil
    {
        private static string SessionAuthKey = "SessionUserID";
        //private static LocalLogger localLog = new LocalLogger(string.Format("{0}\\KBO_LOG2", "D:\\LOG"));
        /// <summary>
        /// 인증 처리를 위한 메서드
        /// 인증처리 후 초기 액세스 페이지 또는 기본 페이지로 이동시킴
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="strRoles"></param>
        public static void LoginProcess(
                string _U_SE
                , string _U_ID
                , string _U_NM
                , string _ZIP1_NU
                , string _ZIP2_NU
                , string _ADDRESS_IF
                , string _PHONE_NU
                , string _CPHONE_NU
                , string _EMAIL_IF
                , string _SEX_IF
                , string _JOB_IF
                , string _TEAM_IF
                , string _SMS_CK
                , string _NEWS_CK
                , string _SSN_CK
                , string _RESIDENT1_NU
                , string _RESIDENT2_CD
                , string _PSW_CD
                , string _REG_DT
                , string _U_IP
                , string _IPIN_CK
                , string _AU_ID_CHK1
            )
        {
            UserInfo ui = new UserInfo();

            ui.U_SE = _U_SE;
            ui.U_ID = _U_ID;
            ui.U_NM = _U_NM;
            ui.ZIP1_NU = _ZIP1_NU;
            ui.ZIP2_NU = _ZIP2_NU;
            ui.ADDRESS_IF = _ADDRESS_IF;
            ui.PHONE_NU = _PHONE_NU;
            ui.CPHONE_NU = _CPHONE_NU;
            ui.EMAIL_IF = _EMAIL_IF;
            ui.SEX_IF = _SEX_IF;
            ui.JOB_IF = _JOB_IF;
            ui.TEAM_IF = _TEAM_IF;
            ui.SMS_CK = _SMS_CK;
            ui.NEWS_CK = _NEWS_CK;
            ui.SSN_CK = _SSN_CK;
            ui.RESIDENT1_NU = _RESIDENT1_NU;
            ui.RESIDENT2_CD = _RESIDENT2_CD;
            ui.PSW_CD = _PSW_CD;
            ui.REG_DT = _REG_DT;
            ui.U_IP = _U_IP;
            ui.U_NO = DateUtil.GetFormatDate(DateTime.Now.ToString(), "yyyy-MM-dd HH:mm:ss");
            ui.AU_ID_CHK1 = _AU_ID_CHK1;
            ui.IPIN_CK = _IPIN_CK;

            HttpContext.Current.Session[SessionAuthKey] = ui;
        }

        /// <summary>
        ///로그아웃 처리
        ///로그아웃된 후 페이지 자기자신을 한번더 호출함
        /// </summary>
        public static void LogoutProcess()
        {
            HttpContext.Current.Session.Abandon();
        }


        /// <summary>
        /// 로그인 체크를 한다.
        /// </summary>
        public static bool LoginChk()
        {
            bool isLogin = false;

            if (HttpContext.Current.Session[SessionAuthKey] != null)
            {
                HttpContext context = HttpContext.Current;

                string userIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (userIp == "" || userIp == null)
                {
                    userIp = context.Request.UserHostAddress;
                }

                //if (context.Request.ServerVariables["REMOTE_HOST"].Equals(GetUserInfo(UserInfoType.U_IP))){
                if (userIp.Equals(GetUserInfo(UserInfoType.U_IP))){
                    isLogin = true;
                }
            }

            return isLogin;
        }

        /// <summary>
        /// 타입별 사용자 정보 리턴
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static string GetUserInfo(UserInfoType uiType)
        {
            string result = string.Empty;

            if (HttpContext.Current.Session[SessionAuthKey] != null)
            {
                UserInfo ui = (UserInfo)HttpContext.Current.Session[SessionAuthKey];

                switch (uiType)
                {
                    case UserInfoType.U_SE:
                        result = ui.U_SE;
                        break;
                    case UserInfoType.U_ID:
                        result = ui.U_ID;
                        break;
                    case UserInfoType.U_NM:
                        result = ui.U_NM;
                        break;
                    case UserInfoType.ZIP1_NU:
                        result = ui.ZIP1_NU;
                        break;
                    case UserInfoType.ZIP2_NU:
                        result = ui.ZIP2_NU;
                        break;
                    case UserInfoType.ADDRESS_IF:
                        result = ui.ADDRESS_IF;
                        break;
                    case UserInfoType.PHONE_NU:
                        result = ui.PHONE_NU;
                        break;
                    case UserInfoType.CPHONE_NU:
                        result = ui.CPHONE_NU;
                        break;
                    case UserInfoType.EMAIL_IF:
                        result = ui.EMAIL_IF;
                        break;
                    case UserInfoType.SEX_IF:
                        result = ui.SEX_IF;
                        break;
                    case UserInfoType.JOB_IF:
                        result = ui.JOB_IF;
                        break;
                    case UserInfoType.TEAM_IF:
                        result = ui.TEAM_IF;
                        break;
                    case UserInfoType.SMS_CK:
                        result = ui.SMS_CK;
                        break;
                    case UserInfoType.NEWS_CK:
                        result = ui.NEWS_CK;
                        break;
                    case UserInfoType.SSN_CK:
                        result = ui.SSN_CK;
                        break;
                    case UserInfoType.RESIDENT1_NU:
                        result = ui.RESIDENT1_NU;
                        break;
                    case UserInfoType.RESIDENT2_CD:
                        result = ui.RESIDENT2_CD;
                        break;
                    case UserInfoType.PSW_CD:
                        result = ui.PSW_CD;
                        break;
                    case UserInfoType.REG_DT:
                        result = ui.REG_DT;
                        break;
                    case UserInfoType.U_IP:
                        result = ui.U_IP;
                        break;
                    case UserInfoType.U_NO:
                        result = ui.U_NO;
                        break;
                    case UserInfoType.AU_ID_CHK1:
                        result = ui.AU_ID_CHK1;
                        break;
                    case UserInfoType.IPIN_CK:
                        result = ui.IPIN_CK;
                        break;
                    default:
                        result = string.Empty;
                        break;
                }
            }

            return result;
        }
    }
}