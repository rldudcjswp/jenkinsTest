using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Util
{
    [Serializable]
    public class ManagerInfo
    {
        public string MG_ID;
        public string MG_PW;
        public string AU_ID;
        public string MG_IP;
        public string MG_NO;
    }

    // GetUserInfo 메서드에서 사용할 열거형 값
    public enum ManagerInfoType
    {
        MG_ID, MG_PW, AU_ID, MG_IP, MG_NO
    }

    public class ManagerInfoUtil
    {
        private static string SessionAuthKey = "SessionManagerID";

        /// <summary>
        /// 인증 처리를 위한 메서드
        /// 인증처리 후 초기 액세스 페이지 또는 기본 페이지로 이동시킴
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="strRoles"></param>
        public static void LoginProcess(string id, string password, string authId, string ip)
        {
            ManagerInfo mi = new ManagerInfo();

            mi.MG_ID = id;
            mi.MG_PW = password;
            mi.AU_ID = authId;
            mi.MG_IP = ip;
            mi.MG_NO = DateUtil.GetFormatDate(DateTime.Now.ToString(), "yyyy-MM-dd HH:mm:ss");

            HttpContext.Current.Session.Timeout = 600;
            HttpContext.Current.Session[SessionAuthKey] = mi;
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
        /// 타입별 사용자 정보 리턴
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static string GetManagerInfo(ManagerInfoType miType)
        {
            string result = string.Empty;

            if (HttpContext.Current.Session[SessionAuthKey] != null)
            {
                ManagerInfo mi = (ManagerInfo)HttpContext.Current.Session[SessionAuthKey];

                switch (miType)
                {
                    case ManagerInfoType.MG_ID:
                        result = mi.MG_ID;
                        break;
                    case ManagerInfoType.MG_PW:
                        result = mi.MG_PW;
                        break;
                    case ManagerInfoType.AU_ID:
                        result = mi.AU_ID;
                        break;
                    case ManagerInfoType.MG_IP:
                        result = mi.MG_IP;
                        break;
                    case ManagerInfoType.MG_NO:
                        result = mi.MG_NO;
                        break;
                    default:
                        result = string.Empty;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 권한체크
        /// </summary>
        /// <param name="authId">권한ID</param>
        /// <returns></returns>
        public static bool ManagerAUTHChk(string authId)
        {
            bool isMaster = true;
            string[] authIdArray = authId.Split('|');

            for (int i = 0; i < authIdArray.Length; i++)
            {
                if (authIdArray[i].ToString().Equals(GetManagerInfo(ManagerInfoType.AU_ID)))
                {
                    isMaster = false;
                }
            }

            return isMaster;
        }

        /// <summary>
        /// 폴더별 권한
        /// </summary>
        /// <param name="pageUrl">웹페이지 url</param>
        /// <returns>폴더별 권한</returns>
        public static string GetFolerAuth(string pageUrl)
        {
            string authList = "";
            string[,] folderAuthArray = {{"/BOARD/","1"}, {"/ETC/","1"}, {"/EVENT/","1"}, {"/MEMBER/","1|2"}, {"/NEWS/","1|2|4"}
                                        , {"/PLAYER/","1"}, {"/RECORDBOARD/","1|2"}, {"/SCHEDULE/","1|2|3"}, {"/VIDEO/","1|2"}, {"/CAREER/","1|2"}};

            for (int i = 0; i < folderAuthArray.GetLength(0); i++)
            {
                if (pageUrl.IndexOf(folderAuthArray[i, 0]) > -1)
                {
                    authList = folderAuthArray[i, 1];
                    break;
                }
            }

            return authList;
        }
    }
}