using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Common;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KBOLib.Util
{
    public class CommonUtil
    {
        public static int row = 0;
        // 회원정보 권한(0-일반,1-불량,2-강퇴)
        public static string[,] authority = { { "", "권한" }, { "0", "일반" }, { "1", "불량" }, { "2", "강퇴" } };
        // 회원정보 검색구분(NAME-이름,ID-아이디,RESIDENT-주민번호)
        public static string[,] searchSection = { { "NAME", "이름" }, { "ID", "아이디" }, { "EMAIL", "이메일" }, { "CPHONE", "핸드폰" } };
        // 불량회원 권한(1-불량,2-강퇴,3-해제)
        public static string[,] badAuth = { { "", "권한" }, { "1", "불량" }, { "2", "강퇴" }, { "3", "해제" } };
        // 회원정보 권한(1-불량,2-강퇴,3-해제)
        public static string[,] authorityPop = { { "3", "일반" }, { "1", "불량" }, { "2", "강퇴" } };

        public static string[,] boardSection = {{"1","한국야구의 기원","title_history"},{"2","골든 글러브","title_golden"},{"3","World Baseball Classic","title_WBC"}
                                 ,{"4","베이징 올림픽","title_Beijing"}, {"5","역대 구단유니폼 및 모자","title_cap"},{"6","역대기념품","title_souvenir"}
                                 ,{"7","시구자 사인볼","title_sign"},{"8","국제대회 매달","title_medal"},{"9","연감","title_yearbook"}
                                 ,{"10","가이드북","title_guidebook"}, {"11","올스타전","title_allstar30"},{"12", "골든글러브", "title_goldenglove"},{"13", "주간야구", "title_WeekBaseball"}};

        public static Boolean prevCheck = true;
        /// <summary>
        /// 회원정보 권한(0-일반,1-불량,2-강퇴)
        /// </summary>
        /// <param name="item">아이템</param>
        /// <returns></returns>
        public static string GetAuthority(string item)
        {
            string result = item;

            for (int i = 0; i < authority.GetLength(0); i++)
            {
                if (item.Equals(authority[i, 0]))
                {
                    result = authority[i, 1];
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 불량회원 권한(1-불량,2-강퇴,3-해제)
        /// </summary>
        /// <param name="item">아이템</param>
        /// <returns></returns>
        public static string GetBadAuth(string item)
        {
            string result = item;

            for (int i = 0; i < badAuth.GetLength(0); i++)
            {
                if (item.Equals(badAuth[i, 0]))
                {
                    result = badAuth[i, 1];
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 숫자 체크 함수
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, "^//d+$");
        }

        /// <summary>
        /// null 또는 공백을 0으로 리턴한다. 
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertNullToZero(object data)
        {
            string result = data.ToString();

            if (string.IsNullOrEmpty(result))
            {
                result = "0";
            }
            return result;
        }

        /// <summary>
        /// null 또는 공백을 -로 변환.
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertNullToDash(object data)
        {
            string result = data.ToString();

            if (string.IsNullOrEmpty(result))
            {
                result = "-";
            }

            return result;
        }

        /// <summary>
        /// null 또는 공백을 공백으로 변환
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>변환된 문자열</returns>
        public static string ConvertNullToSpace(string data)
        {
            string result = "";
            
            if (!string.IsNullOrEmpty(data))
            {
                result = data;
            }

            return result;
        }

        /// <summary>
        /// 문자열 숫자를 int 형으로 변환, 널이면 0을 리턴 
        /// </summary>
        /// <param name="src">문자열 숫자</param>
        /// <returns>숫자</returns>
        public static int ConvertStringToNumber(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 0;
            }

            if (!chkOnlyNum(data))
            {
                return 0;
            }

            return Convert.ToInt32(data);
        }

        /// 파라미터를 받아 허용된 값(숫자)이면 true를 반환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool chkOnlyNum(string param)
        {
            bool retValue = false;
            if (!string.IsNullOrEmpty(param))
            {
                //if (Regex.IsMatch(param, @"^[0-9]"))
                //{
                //    retValue = true;
                //}

                if (Regex.IsMatch(param, @"^[+-]?\d*(\.?\d*)$"))
                {
                    retValue = true;
                }
            }
            return retValue;
        }

        /// <summary>
        /// 입력 문자열이 null이면 대치 문자열로 변환하고, null이 아닐경우 변환하지 않는다.
        /// </summary>
        /// <param name="data">문자열</param>
        /// <param name="target">null을 대치할 문자열</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertNull(string data, string target)
        {
            string result = data;

            if (string.IsNullOrEmpty(data))
            {
                result = target;
            }

            return result;
        }

        /// <summary>
        /// 파라미터를 받아 허용된 값(null, empty)이 아니면 empty를 반환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ParamRequest(string param)
        {
            string retValue = string.Empty;
            if (!string.IsNullOrEmpty(param))
            {
                //retValue = param;
                retValue = CInJectionUtil.replaceSqlInjaction(param);
            }
            return retValue;
        }

        /// <summary>
        /// 파라미터를 받아 허용된 값(null, empty)이 아니면 empty를 반환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ParamRequestInjactionBoard(string param)
        {
            string retValue = string.Empty;
            if (!string.IsNullOrEmpty(param))
            {
                //retValue = param;
                retValue = CInJectionUtil.replaceSqlInjactionBoard(param);
            }
            return retValue;
        }

        #region 구분 코드 (뉴스, 게시판)
        /// <summary>
        /// 뉴스 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetNewsScIdToName(string scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetNewsSection("ALLS");
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId);

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString();
            }

            return result;
        }

        /// <summary>
        /// KBO 보도자료 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetPressboxScIdToName(string scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetPressboxSection();
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId);

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString();
            }

            return result;
        }


        /// <summary>
        /// QNA 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetQnaScIdToName(string scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetQnaSection();
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId);

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString();
            }

            return result;
        }

        /// <summary>
        /// Faq 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetFaqScIdToName(string scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetFaqSection();
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId);

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString().Trim();
            }

            return result;
        }

        /// <summary>
        /// 1:1문의하기 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetInquiryScIdToName(object scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetInquirySection();
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId.ToString());

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString().Trim();
            }

            return result;
        }

        /// <summary>
        /// CONTACTUS 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetContactusScIdToName(string scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetContactusSection();
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId);

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString().Trim();
            }

            return result;
        }

        /// <summary>
        /// VOD 구분ID -> 구분명
        /// </summary>
        /// <param name="scId">구분ID</param>
        /// <returns></returns>
        public static string GetVodScIdToName(string scId)
        {
            string result = "전체";
            DataSet ds = CacheUtil.GetVodSection();
            DataRow[] dr = ds.Tables[0].Select("SC_ID = " + scId);

            if (dr.Length > 0)
            {
                result = dr[0]["SC_NM"].ToString().Trim();
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 메시지 박스 처리
        /// </summary>
        /// <param name="msg">출력 문자열</param>
        /// <returns>메시지 박스</returns>
        public static string AddMsg(string msg)
        {
            msg = msg.Replace("'", "\\'");  // '는 \로 변환
            msg = msg.Replace("\"", "\\\"");    // "는 \로 변환

            return string.Format("<script type=\"text/javascript\">alert('{0}');</script>", msg);
        }

        /// <summary>
        /// 메시지 박스 처리후 페이지 이동
        /// </summary>
        /// <param name="msg">출력 문자열</param>
        /// <returns>메시지 박스</returns>
        public static string AddMsgRedirect(string msg, string url)
        {
            msg = msg.Replace("'", "\\'");  // '는 \로 변환
            msg = msg.Replace("\"", "\\\"");    // "는 \로 변환

            return string.Format("<script type=\"text/javascript\">alert('{0}');location.href='{1}';</script>", msg, url);
        }

        /// <summary>
        /// 이미지 없을 경우 처리 후 경로 리턴
        /// </summary>
        /// <param name="source">원본 이미지 src</param>
        /// <param name="imgSrc">이미지 있을 경우 src</param>
        /// <param name="noImgSrc">이미지 없을 경우 src</param>
        /// <returns>이미지 경로</returns>
        public static string ConvertNoImg(object source, object imgSrc, string noImgSrc)
        {
            //string result = string.Format("{0}{1}", StaticVariable.FILE_URL, imgSrc.ToString());

            //if (string.IsNullOrEmpty(source.ToString()))
            //{
            //    result = string.Format("{0}{1}", StaticVariable.FILE_URL, noImgSrc);
            //}
            string result = string.Empty;

            string imgUrl = @"/FILE/" + imgSrc;
            FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(imgUrl));
            if (fileInfo.Exists)
            {
                result = imgUrl;
            }
            else
            {
                result = string.Format("{0}{1}", StaticVariable.FILE_URL, noImgSrc);
            }

            return result;
        }

        /// <summary>
        /// 이미지 없을 경우 처리 후 경로 리턴
        /// </summary>
        /// <param name="source">원본 이미지 src</param>
        /// <param name="imgSrc">이미지 있을 경우 src</param>
        /// <param name="noImgSrc">이미지 없을 경우 src</param>
        /// <returns>이미지 경로</returns>
        public static string ConvertNoImgSrc(object source, object imgSrc, string noImgSrc)
        {
            string result = string.Empty;

            if(source.ToString() == "")
            {
                result = noImgSrc;
            } else
            {
                result = StaticVariable.FILE_URL + CommonUtil.GetChangeFileName(imgSrc.ToString());
            }
    
            return result;
        }

        /// <summary>
        /// html 태그 제거 후 문자열 길이 넘는 부분 "..."으로 리턴
        /// </summary>
        /// <param name="text">본문</param>
        /// <param name="size">사이즈</param>
        /// <returns>축약문</returns>
        public static string GetTxtLen(object text, int size)
        {
            string ret = Regex.Replace(text.ToString(), "<.*?>", "", RegexOptions.Singleline); // html 태그 삭제 구문 추가

            ret = ret.Replace("\"", "").Replace("&nbsp;", "");

            if (ret.Length > size)
            {
                ret = ret.Substring(0, size) + "...";
            }
            return ret;
        }

        /// <summary>
        /// html 태그 제거 및 특수 문자(&nbsp; 등등) 제거
        /// </summary>
        /// <param name="text">본문</param>
        /// <param name="size">사이즈</param>
        /// <returns>축약문</returns>
        public static string GetTxtLenHtml(object text, int size)
        {
            string ret = Regex.Replace(text.ToString(), "<.*?>", "", RegexOptions.Singleline); // html 태그 삭제 구문 추가
            ret = Regex.Replace(ret, "&.*?;", "", RegexOptions.Singleline);    // 특수 문자 제거
            ret = ret.Replace("\"", "");

            if (ret.Length > size)
            {
                ret = ret.Substring(0, size) + "...";
            }
            return ret;
        }

        /// <summary>
        /// 목표 타켓과 항목이 일치 하면, 해당 cssClass 명을 돌려 준다.
        /// </summary>
        /// <param name="item">항목</param>
        /// <param name="target">목표</param>
        /// <param name="cssClass">cssClass</param>
        /// <returns></returns>
        public static string AddCssClass(string item, string target, string cssClass)
        {
            string result = "";

            if (item.Equals(target))
            {
                result = cssClass;
            }

            return result;
        }

        /// <summary>
        /// bool --> checked
        /// </summary>
        /// <param name="_bool"></param>
        /// <returns></returns>
        public static String GetBool_chk(String data)
        {
            String ret = "";
            bool result = bool.Parse(data);

            if (result)
            {
                ret = "checked";
            }

            return ret;
        }

        /// <summary>
        /// 부울 값을 YN 형태로 변환
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public static string ConvertBoolToYN(object data)
        {
            return ((bool)data == true) ? "Y" : "N";
        }

        /// <summary>
        /// 부울 값을 0/1 형태로 변환
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public static string ConvertBoolToNumber(object data)
        {
            return ((bool)data == true) ? "1" : "0";
        }

        /// <summary>
        /// 썸네일 이미지 처리
        /// </summary>
        /// <param name="dir">디렉터리 경로</param>
        /// <param name="fileName">원본 파일명</param>
        /// <param name="width">썸네일 이미지 넓이</param>
        /// <param name="height">썸네일 이미지 높이</param>
        public static void GetThumbnail(string filepath, int width, int height, string savepath)
        {
            try
            {
                System.Drawing.Image im = System.Drawing.Image.FromFile(filepath);

                //업로드된 이미지를 객체로 생성
                Bitmap img = new Bitmap(im);

                Bitmap resizeImg = new Bitmap(width, height);

                //GDI+를 이용해서 리사이즈된 이미지 생성
                Graphics g = Graphics.FromImage(resizeImg);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, width, height));

                //리사이즈된 이미지 저장
                resizeImg.Save(savepath);//리사이즈된 파일명

                im.Dispose();
                img.Dispose();
                resizeImg.Dispose();
                g.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }

        /// <summary>
        /// 게시판 페이지 이동 스크립트 추가
        /// </summary>
        /// <param name="pageNo">Page 번호 값</param>
        /// <returns>javascript</returns>
        public static string GetMovePageScript(HiddenField pageNo)
        {
            string script = @"function movepage(page_no) {
                document.getElementById('" + pageNo.ClientID + @"').value = page_no;

                var frm = document.getElementById('mainForm');
                frm.submit();
            }";

            return script;
        }

        /// <summary>
        /// 모바일 브라우저 판별
        /// </summary>
        /// <returns></returns>
        public static bool IsMobileBrowser()
        {
            HttpContext context = HttpContext.Current;

            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string[] mobiles =
                {
                    "midp", "j2me", "avant", "docomo", 
                    "novarra", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "blackberry", "mib/", "symbian", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio", 
                    "SIE-", "SEC-", "samsung", "HTC", 
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx", 
                    "NEC", "philips", "mmm", "xx", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java", 
                    "pt", "pg", "vox", "amoi", 
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", 
                    "moto", "iphone"
                };

                foreach (string s in mobiles)
                {
                    if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// http -> https 보안 접속이 필요한 페이지에 적용(KBO전용)
        /// </summary>
        public static void RedirectHttpsKBO()
        {
            string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            string port = HttpContext.Current.Request.Url.Port.ToString();

            if (port == "80" && !currentUrl.Contains("test"))
            {
                string redirectUrl = currentUrl.Replace("http:", "https:");
                if (!redirectUrl.ToUpper().Contains("//WWW."))
                {
                    redirectUrl = redirectUrl.ToLower().Replace("koreabaseball.com", "www.koreabaseball.com");
                }
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
            // 추가 (https://koreabaseball.com을 처리할 수 없음) - 21.06.17 | veron
            else if(port == "8080" && !currentUrl.ToUpper().Contains("//WWW."))
            {
                currentUrl = currentUrl.ToLower().Replace("koreabaseball.com", "www.koreabaseball.com");
                HttpContext.Current.Response.Redirect(currentUrl);
            }
        }

        /// <summary>
        /// http -> https 보안 접속이 필요한 페이지에 적용
        /// </summary>
        public static void RedirectHttps()
        {
            string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            string port = HttpContext.Current.Request.Url.Port.ToString();

            if (port == "80")
            {
                string redirectUrl = currentUrl.Replace("http:", "https:");
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// https -> http 일반 접속이 필요한 페이지에 적용
        /// </summary>
        public static void RedirectHttp()
        {
            string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            if (HttpContext.Current.Request.IsSecureConnection && currentUrl.ToUpper().IndexOf("LOCALHOST") == -1 && currentUrl.ToUpper().IndexOf("TEST") == -1)
            {
                string redirectUrl = currentUrl.Replace("https:", "http:");
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// koreabaseball.com -> www.koreabaseball.com
        /// </summary>
        public static void RedirectURL()
        {
            string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            if (!HttpContext.Current.Request.IsSecureConnection && currentUrl.ToUpper().IndexOf("LOCALHOST") == -1 && currentUrl.ToUpper().IndexOf("TEST") == -1)
            {
                if (!currentUrl.ToUpper().Contains("//WWW."))
                {
                    HttpContext.Current.Response.Redirect(currentUrl.ToLower().Replace("koreabaseball.com", "www.koreabaseball.com"));
                }
            }
        }

        /// <summary>
        /// 프로시저 실행 구문 리턴
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns>프로시저 실행 구문</returns>
        public static string ToProcParam(DbCommand cmd)
        {
            StringBuilder sbProc = new StringBuilder();

            sbProc.Append("EXEC ").Append(cmd.CommandText).Append(" ");

            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                if (cmd.Parameters[i].DbType == DbType.String)
                {
                    sbProc.Append("'");
                }

                sbProc.Append(cmd.Parameters[i].Value);

                if (cmd.Parameters[i].DbType == DbType.String)
                {
                    sbProc.Append("'");
                }

                sbProc.Append(",");
            }

            return sbProc.ToString().Substring(0, sbProc.Length - 1);
        }

        public static String GetPageList(int _list_total_cnt, int _page_no, int _list_cn)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='pagination'>\n");

            if (_list_total_cnt > 0)
            {
                int pageunit = 10;
                int page_tcnt = (_list_total_cnt + _list_cn - 1) / _list_cn;
                int startpage = (((_page_no - 1) / pageunit) * pageunit) + 1;
                int endpage = startpage + pageunit - 1;

                if (endpage > page_tcnt)
                {
                    endpage = page_tcnt;
                }

                if (_page_no > 1)
                {
                    sb.Append("<a href='javascript:movepage(1);' class='first'><img src='/images/common/btn_first.gif' alt='처음' /></a>\n");
                    sb.Append("<a href='javascript:movepage(" + (_page_no - 1) + ");' class='prev'><img src='/images/common/btn_prev.gif' alt='이전' /></a>\n");
                }

                for (int i = startpage; i <= endpage; i++)
                {
                    if (i == _page_no)
                    {
                        sb.Append("<strong>" + i.ToString() + "</strong>\n");
                    }
                    else
                    {
                        sb.Append("<a href='javascript:movepage(" + i.ToString() + ");' >" + i.ToString() + "</a>\n");
                    }
                }

                if (_page_no < page_tcnt)
                {
                    sb.Append("<a href='javascript:movepage(" + (_page_no + 1) + ");' class='next'><img src='/images/common/btn_next.gif' alt='다음' /></a>\n");
                    sb.Append("<a href='javascript:movepage(" + page_tcnt + ");' class='last'><img src='/images/common/btn_last.gif' alt='마지막' /></a>\n");
                }
            }

            sb.Append("</div>\n");

            return sb.ToString();
        }

        public static String GetRecordPaper(String home, String away)
        {
            StringBuilder sb = new StringBuilder();

            if (home == null && home.Equals("") && away == null && away.Equals(""))
            {
                sb.Append("-");
            }
            else
            {
                //20110616 유주영대리님 요청으로 기록지 표출되는순서를 홈-원정에서 원정-홈으로 위치 변경  - mingw

                if (away == null || away.Equals(""))
                {
                    sb.Append("-");
                }
                else
                {
                    sb.Append("&nbsp;<a href='/common/FileDown.aspx?file=/").Append(away).Append("'>");
                    sb.Append("<img src='/images/schedule/schedule_away.gif' alt='원정'/>");
                    sb.Append("</a>");
                }

                sb.Append(":");

                if (home == null || home.Equals(""))
                {
                    sb.Append("-");
                }
                else
                {
                    sb.Append("<a href='/common/FileDown.aspx?file=/").Append(home).Append("'>");
                    sb.Append("<img src='/images/schedule/schedule_home.gif' alt='홈'/>");
                    sb.Append("</a>\n");
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// 소숫점 자릿수 셋팅
        /// </summary>
        /// <param name="cho">표현 소수점 자리수</param>
        /// <param name="data">대상 데이터</param>
        /// <returns></returns>
        public static string ConvertComma(string data)
        {
            string ret = "";

            if (data == null || data.Equals("") || data == "NaN")
                return ret;

            if (data.Length > 10)
            {
                data = data.Substring(0, 10);
            }

            ret = string.Format("{0:#,0}", Convert.ToDecimal(data));

            return ret;
        }

        /// <summary>
        /// 방송 유무에 따른 버튼 출력
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>변환 문자열</returns>
        public static string GetConvertIU(String data)
        {
            return (data.Equals("UPDATE")) ? "수정" : "입력";
        }

        /// <summary>
        /// 한자리 정수를 두자리 정수형태로 변환한다.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetTwoInt(int num)
        {
            string ret;

            return ret = num < 10 ? "0" + num : "" + num;
        }

        /// <summary>
        /// null 또는 공백을 0으로 리턴한다. 
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>변환 문자열</returns>
        public static String GetZero(String data)
        {
            String result = data;

            if (data == null || data == "")
            {
                result = "0";
            }
            return result;
        }

        /// <summary>
        /// select box 생성
        /// </summary>
        /// <param name="max">최대값</param>
        /// <param name="select">selected 값</param>
        /// <returns></returns>
        public static string GetSelectBoxOrder(int max, int select)
        {
            StringBuilder option = new StringBuilder();

            for (int i = 1; i <= max; i++)
            {
                option.Append("<option");
                option.Append(" value='").Append(i.ToString()).Append("'");

                if (i == select)
                {
                    option.Append(" selected='selected'");
                }

                option.Append(">");
                option.Append(i.ToString()).Append("</option>");
            }

            return option.ToString();
        }

        /// <summary>
        /// 파일 정보에서 파일 전체 경로 추출
        /// </summary>
        /// <param name="jsonData">파일정보</param>
        /// <returns>파일 전체 경로</returns>
        public static string GetFilePath(string jsonData)
        {
            string result = "";

            if (jsonData.Contains("No files received"))
            {
                return result;
            }

            if (!string.IsNullOrEmpty(jsonData))
            {
                JObject obj = JObject.Parse(JsonConvert.DeserializeObject(jsonData).ToString());
                JArray list = (JArray)obj["info"];

                for (int i = 0; i < list.Count; i++)
                {
                    JObject item = (JObject)list[i];

                    string path = item["path"].ToString();
                    path = path.Replace("/file/", "");
                    if(i == 0) // 이미지 파일 1개 일 때
                    {
                        result = string.Format("{0}{1}", path, item["name"]);
                    }
                    else
                    {
                        result += string.Format(",{0}{1}", path, item["name"]);
                    }                 
                    //result = string.Format("{0}{1}", item["path"], item["name"]);
                    //break; // 2개 이미지 파일 path insert를 위해 주석처리
                }
            }


            return result;
        }

        /// <summary>
        /// 파일 정보에서 파일 전체 경로 추출
        /// </summary>
        /// <param name="jsonData">파일정보</param>
        /// <param name="filePath">파일경로</param>
        /// <returns>파일 전체 경로</returns>
        public static string GetFilePath(string jsonData, string filePath)
        {
            string result = filePath;

            if (jsonData == "No files received.")
            {
                return result;
            }

            if (!string.IsNullOrEmpty(jsonData))
            {
                result = GetFilePath(jsonData);
            }

            return result;
        }

        /// <summary>
        /// 순위 표출 방식 변경
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public static string GetRankStr(object rank)
        {
            string result = "1st";

            switch (rank.ToString())
            {
                case "1":
                    result = "1st";
                    break;
                case "2":
                    result = "2nd";
                    break;
                case "3":
                    result = "3rd";
                    break;
                case "4":
                    result = "4th";
                    break;
                case "5":
                    result = "3th";
                    break;
                default:
                    result = rank.ToString() + "th";
                    break;
            }

            return result;
        }

        #region 온라인뮤지엄
        /// <summary>
        /// 게시판 아이디 -> 게시판 이름
        /// </summary>
        /// <param name="boardId">게시판 아이디</param>
        /// <returns>게시판 명</returns>
        public static String GetBoardIdToNm(String boardId)
        {
            String result = boardId;

            for (int i = 0; i < boardSection.GetLength(0); i++)
            {
                if (boardId.Equals(boardSection[i, 0]))
                {
                    result = boardSection[i, 1];
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 게시판 아이디 -> 이미지명
        /// </summary>
        /// <param name="boardId">게시판 아이디</param>
        /// <returns>이미지명</returns>
        public static String GetBoardIdToImg(String boardId)
        {
            String result = boardId;

            for (int i = 0; i < boardSection.GetLength(0); i++)
            {
                if (boardId.Equals(boardSection[i, 0]))
                {
                    result = boardSection[i, 2];
                    break;
                }
            }

            return result;
        }

        #endregion

        #region 영문홈페이지

        /// <summary>
        /// 소숫점 자릿수 셋팅
        /// </summary>
        /// <param name="cho">표현 소수점 자리수</param>
        /// <param name="data">대상 데이터</param>
        /// <returns></returns>
        public static string ConvertFormat(int cho, string data)
        {
            string ret = "-";

            if (data == null || data.Equals("") || data == "NaN")
                return ret;

            if (data.Length > 10)
                data = data.Substring(0, 10);

            if (cho == 1)
            {
                ret = string.Format("{0:0.0}", Convert.ToDecimal(data));
            }
            else if (cho == 2)
            {
                ret = string.Format("{0:0.00}", Convert.ToDecimal(data));
            }
            else if (cho == 3)
            {
                ret = string.Format("{0:0.000}", Convert.ToDecimal(data));
            }
            else if (cho == 0)
            {
                ret = string.Format("{0:0}", Convert.ToDecimal(data));
            }
            else if (cho == 99)
            {
                string[] temp;
                temp = string.Format("{0:N}", Convert.ToInt32(data)).Split('.');

                if (temp.Length > 0)
                {
                    ret = temp[0];
                }
                else
                {
                    ret = "0";
                }
            }

            return ret;
        }
        #endregion

        #region 메인 프리뷰
        /// <summary>
        /// 메인프리뷰 - 년도
        /// </summary>
        /// <param name="seasonId">시즌id</param>
        /// <returns></returns>
        public static int GetPreViewYear(int seasonId)
        {
            int result = seasonId;

            // revenge 20170413 특정일자 기준으로 작년도 표출 부분 4월13일 기준으로 해제. 요청자-홍지희
            //if (DateTime.Now.Year == seasonId && prevCheck)
            //{
            //    result = DateTime.Now.Year - 1;
            //}

            return result;
        }
        #endregion

        /// <summary>
        /// 비밀번호 유효성체크
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Boolean GetPasswordRegex(string password)
        {
            Boolean result = false;

            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMiniMaxChars = new Regex(@".{8,12}");
            Regex hasLowerChar = new Regex(@"[a-z]+");
            Regex hasSymbols = new Regex(@"[~!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            // 2019.07.30 비밀번호 정책 변경
            int count = 0;

            //숫자,영어,특수문자
            if (hasNumber.IsMatch(password))    // 숫자일경우
            {
                count++;
            }
            if (hasUpperChar.IsMatch(password))   // 대문자일경우
            {
                count++;
            }
            if (hasLowerChar.IsMatch(password)) // 소문자일 경우
            {
                count++;
            }
            if (hasSymbols.IsMatch(password))   // 특수문자일 경우
            {
                count++;
            }

            if (count == 2 && password.Length >= 10 && password.Length <= 16)    // 2가지 조합 10자 이상 16자 이하
            {
                result = true;
            }

            if (count >= 3 && password.Length >= 8 && password.Length <= 16) // 3가지 조합 8자 이상 16자 이하
            {
                result = true;
            }


            // 소문자, 대문자, 숫자, 특수문자
            //if (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMiniMaxChars.IsMatch(password) && hasLowerChar.IsMatch(password) && hasSymbols.IsMatch(password))
            //if (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && password.Length >= 8 && password.Length <= 12 && hasLowerChar.IsMatch(password) && hasSymbols.IsMatch(password))
            //{
            //    result = true;
            //}
            
            return result;
        }

        /// 글번호 셋팅
        /// </summary>
        /// <returns></returns>
        public static int GetPageNumber(int totalCount, int currentPage, int pageSize, int nowIndex)
        {
            int ret = 0;

            ret = totalCount - nowIndex - ((currentPage - 1) * pageSize);

            return ret;
        }

        /// <summary>
        /// 소수점 데이터 포맷 변경
        /// </summary>
        /// <param name="data">Decimal</param>
        /// <param name="digits">소수 자릿수</param>
        /// <returns>digits와 일치 하는 포맷</returns>
        public static string ConvertFormat(Decimal data, int digits)
        {
            string result = "-";

            switch (digits)
            {
                case 0:
                    result = string.Format("{0:0}", data);
                    break;
                case 1:
                    result = string.Format("{0:0.0}", data);
                    break;
                case 2:
                    result = string.Format("{0:0.00}", data);
                    break;
                case 3:
                    result = string.Format("{0:0.000}", data);
                    break;
            }

            return result;
        }

        public static string GetChangeFileName(string fileName)
        {
            //string[,] list = { { "[", "%5B" }, { "]", "%5D" } };
            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    for (int i = 0; i < list.GetLength(0); i++)
            //    {
            //        fileName = fileName.Replace(list[i, 0], list[i, 1]);
            //    }
            //}
            return HttpUtility.UrlEncode(fileName).Replace("+", "%20");
        }
    }
}