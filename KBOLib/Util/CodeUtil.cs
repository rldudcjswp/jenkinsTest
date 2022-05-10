using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.Common;
// TODO: Use Enterprise Library Data Block
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace KBOLib.Util
{
    public class CodeUtil
    {
        private static Database webDB = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_web");
        private static Database kboDB2 = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_db2");

        private static DataTable dtWebSection = null;           // KBO_WEB SECTION 테이블 모음
        private const string KBO_WEB_SECTION_CODE_KEY = "KBO_WEB_SECTION_CODE";

        #region DB or Cache -> DataSet -> DataTable 초기화
        /// <summary>
        /// CD_MASTER (마스터 코드) DataTable 초기화
        /// </summary>
        public static void SetWebSection()
        {
            DataSet dsWebSection = null;

            dsWebSection = CacheUtil.GetDataSet(KBO_WEB_SECTION_CODE_KEY);

            if (dsWebSection == null)
            {
                dsWebSection = GetWebSectionBind();
                CacheUtil.SetDataSet(dsWebSection, KBO_WEB_SECTION_CODE_KEY, 0, 1, 0, 0);
            }

            dtWebSection = dsWebSection.Tables[0];
        }
        #endregion

        #region 프로시져 호출 후 DataSet 저장
        /// <summary>
        /// CD_MASTER (마스터 코드) DB -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetWebSectionBind()
        {
            DataSet dsWebSection = null;
            DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_CODE_LIST_S");

            try
            {
                dsWebSection = webDB.ExecuteDataSet(cmd);
            }
            catch (Exception error)
            {
            }

            return dsWebSection;
        }
        #endregion

        #region 조건에 맞는 DataRow 리턴
        /// <summary>
        /// KBO_WEB SECTION 테이블 리스트
        /// </summary>
        /// <param name="section">구분</param>
        /// <returns></returns>
        public static DataRow[] GetWebSectionList(object section)
        {
            return dtWebSection.Select(string.Format("SECTION='{0}'", section.ToString()));
        }
        #endregion

        #region source 항목 -> target 항목
        /// <summary>
        /// 마스터 코드 CM_SE -> CM_NM
        /// </summary>
        /// <param name="code">CM_SE</param>
        /// <returns>CM_NM</returns>
        public static string GetWebSectionName(object section, object code)
        {
            string result = string.Empty;

            try
            {
                DataRow[] drData = dtWebSection.Select(string.Format("SECTION='{0}' AND SC_ID={1}", section.ToString(), code.ToString()));

                if (drData.Length > 0)
                {
                    result = drData[0]["SC_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// 30주년(BOARD_30TH_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string Get30thName(object code)
        {
            return GetWebSectionName("30TH", code);
        }

        /// <summary>
        /// 앨범(BOARD_ALBUM_ETC_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetAlbumName(object code)
        {
            return GetWebSectionName("ALBUM", code);
        }

        /// <summary>
        /// 증명서(BOARD_CERTIFICATE_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetCertifacateName(object code)
        {
            return GetWebSectionName("CERTIFICATE", code);
        }

        /// <summary>
        /// 댓글(BOARD_COMMENT_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetCommentName(object code)
        {
            return GetWebSectionName("COMMENT", code);
        }

        /// <summary>
        /// 문의하기(BOARD_CONTACTUS_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetContactusName(object code)
        {
            return GetWebSectionName("CONTACTUS", code);
        }

        /// <summary>
        /// FAQ(BOARD_FAQ_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetFaqName(object code)
        {
            return GetWebSectionName("FAQ", code);
        }

        /// <summary>
        /// FAQ(BOARD_INQUIRY_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetInquiryName(object code)
        {
            return GetWebSectionName("INQUIRY", code);
        }

        /// <summary>
        /// GAME(BOARD_GAME_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetGameName(object code)
        {
            return GetWebSectionName("GAME", code);
        }

        /// <summary>
        /// HTML(BOARD_HTML_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetHtmlName(object code)
        {
            return GetWebSectionName("HTML", code);
        }

        /// <summary>
        /// NEWS(BOARD_NEWS_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetNewsName(object code)
        {
            return GetWebSectionName("NEWS", code);
        }

        /// <summary>
        /// NOTICE(BOARD_NOTICE_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetNoticeName(object code)
        {
            return GetWebSectionName("NOTICE", code);
        }

        /// <summary>
        /// PREDICTION(BOARD_PREDICTION_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetPredictionName(object code)
        {
            return GetWebSectionName("PREDICTION", code);
        }

        /// <summary>
        /// PRESSBOX(BOARD_PRESSBOX_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetPressboxName(object code)
        {
            return GetWebSectionName("PRESSBOX", code);
        }

        /// <summary>
        /// QNA(BOARD_QNA_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetQnaName(object code)
        {
            return GetWebSectionName("QNA", code);
        }

        /// <summary>
        /// REQUEST(BOARD_REQUEST_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetRequestName(object code)
        {
            return GetWebSectionName("REQUEST", code);
        }

        /// <summary>
        /// SURVEY(BOARD_SURVEY_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetSurveyName(object code)
        {
            return GetWebSectionName("SURVEY", code);
        }

        /// <summary>
        /// VOD(BOARD_VOD_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetVodName(object code)
        {
            return GetWebSectionName("VOD", code);
        }

        /// <summary>
        /// BANNER(MAIN_BANNER_SECTION) CODE -> NAME
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>이름</returns>
        public static string GetBannerame(object code)
        {
            return GetWebSectionName("BANNER", code);
        }
        #endregion
    }
}
