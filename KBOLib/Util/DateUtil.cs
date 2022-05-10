using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text;

namespace KBOLib.Util
{
    public class DateUtil
    {
        /// <summary>
        /// YYYYMMDD 형식 오늘 날짜 구하기    
        /// </summary>
        /// <returns>YYYYMMDD</returns>
        public static string GetNowDate()
        {
            string date = DateTime.Now.ToString();

            date = GetFormatDate(date, "yyyyMMdd");

            return date;
        }

        /// <summary>
        /// 날짜 형식 반환 [지정한 형식] 예) "2011-02-08", "yyyy 년 MM 월 dd 일"
        /// </summary>
        /// <param name="data">날짜</param>
        /// <param name="format">날짜 포맷</param>
        /// <returns></returns>
        public static string GetFormatDate(object data, string format)
        {
            string date = data.ToString();

            if (date.Length == 8)
            {
                date = ConvertDateType(date);
            }

            try
            {
                date = (String)DateTime.Parse(date).ToString(format);
            }
            catch (Exception) { }

            return date;
        }

        /// <summary>
        /// 현재 년도 ex, yyyy
        /// </summary>
        /// <returns>yyyy 년도</returns>
        public static string GetNowYear()
        {
            string date = DateTime.Now.ToString();

            return GetFormatDate(date, "yyyy");
        }

        /// <summary>
        /// 현재 월 ex, MM 형태로 변환 후 return
        /// </summary>
        /// <returns>MM 월</returns>
        public static string GetNowMonth()
        {
            string date = DateTime.Now.ToString();

            return GetFormatDate(date, "MM");
        }

        /// <summary>
        /// 한국 현재 일을 dd 형태로 변환 후 return
        /// </summary>
        /// <returns>dd 일</returns>
        public static string GetNowDay()
        {
            string date = DateTime.Now.ToString();

            return GetFormatDate(date, "dd");
        }

        /// <summary>
        /// 해당 날짜의 요일
        /// </summary>
        /// <param name="date">날짜</param>
        /// <returns>요일</returns>
        public static String GetDayOfWeek(DateTime date)
        {
            return date.ToString("ddd", new CultureInfo("ko-KR"));
        }

        /// <summary>
        /// 두 날짜 사이의 기간을 돌려 준다.
        /// </summary>
        /// <param name="sDate">시작날짜</param>
        /// <param name="eDate">종료날짜</param>
        /// <returns>두 날짜 사이의 기간</returns>
        public static int GetCompareDate(String sDate, String eDate)
        {
            DateTime startDate = (sDate.Length.Equals(8)) ? Convert.ToDateTime(ConvertDateType(sDate)) : Convert.ToDateTime(sDate);
            DateTime endDate = (eDate.Length.Equals(8)) ? Convert.ToDateTime(ConvertDateType(eDate)) : Convert.ToDateTime(eDate);

            // 두 날짜 사이의 기간 찾기
            TimeSpan period = endDate - startDate;

            // 두 날짜 사이의 기간 돌려 주기
            return period.Days;
        }

        /// <summary>
        /// 날짜 비교 앞 날짜가 크면 true;
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool IsCompareDate(String data1, String data2)
        {
            bool isCompare = false;

            if (data1.Length.Equals(8)) data1 = ConvertDateType(data1);
            if (data2.Length.Equals(8)) data2 = ConvertDateType(data2);

            DateTime d1 = DateTime.Parse(data1);
            DateTime d2 = DateTime.Parse(data2);

            if (DateTime.Compare(d1, d2) > 0)
            {
                isCompare = true;
            }

            return isCompare;
        }

        /// <summary>
        /// 게시판 최신글 유/무
        /// </summary>
        /// <param name="nowDate">현재 날짜</param>
        /// <param name="digit">몇일 까지 최신글인지 (ex, 1, 2)</param>
        /// <returns>최신글 유무(true/false)</returns>
        public static bool IsListNew(String nowDate, int digit)
        {
            bool isResult = false;

            DateTime date = DateTime.Parse(ConvertDateType(GetNowDate()));
            string beforeDate = date.AddDays(digit * -1).ToString("yyyyMMdd");

            if (IsCompareDate(nowDate, beforeDate))
            {
                isResult = true;
            }

            return isResult;
        }

        /// <summary>
        /// 요일번호
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetWeekStr(string week)
        {
            string weekStr = "";

            switch (week)
            {
                case "1":
                    weekStr = "월";
                    break;
                case "2":
                    weekStr = "화";
                    break;
                case "3":
                    weekStr = "수";
                    break;
                case "4":
                    weekStr = "목";
                    break;
                case "5":
                    weekStr = "금";
                    break;
                case "6":
                    weekStr = "토";
                    break;
                case "7":
                    weekStr = "일";
                    break;
            }

            return weekStr;
        }

        /// <summary>
        ///  날짜 형식 변환 YYYYMMDD=>YYYY-MM-DD
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateType(string date)
        {
            string ret = date;

            if (date.Length >= 8)
            {
                ret = string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
            }

            return ret;
        }

        #region 영문홈페이지
        /// <summary>
        /// 달 영문
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetMonthStr(string num)
        {
            string ret = "";

            switch (num)
            {
                case "1":
                    ret = "JAN";
                    break;
                case "2":
                    ret = "FEB";
                    break;
                case "3":
                    ret = "MAR";
                    break;
                case "4":
                    ret = "APR";
                    break;
                case "5":
                    ret = "MAY";
                    break;
                case "6":
                    ret = "JUN";
                    break;
                case "7":
                    ret = "JUL";
                    break;
                case "8":
                    ret = "AUG";
                    break;
                case "9":
                    ret = "SEP";
                    break;
                case "10":
                    ret = "OCT";
                    break;
                case "11":
                    ret = "NOV";
                    break;
                case "12":
                    ret = "DEC";
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 요일번호
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetWeekEngStr(string num)
        {
            string ret = "";

            switch (num)
            {
                case "0":
                    ret = "MON";
                    break;
                case "1":
                    ret = "TUE";
                    break;
                case "2":
                    ret = "WED";
                    break;
                case "3":
                    ret = "THU";
                    break;
                case "4":
                    ret = "FRI";
                    break;
                case "5":
                    ret = "SAT";
                    break;
                case "6":
                    ret = "SUN";
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 요일 - 영어
        /// </summary>
        /// <param name="date">요일</param>
        /// <returns>영문 요일</returns>
        public static string GetDayOfEngWeek(DateTime date)
        {
            return date.DayOfWeek.ToString().Substring(0, 3).ToUpper();
            //return date.ToString("ddd");
        }

        /// <summary>
        ///  날짜 형식 변환 yyyymmdd=>dd/mm/yyyy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ConvertDateTypeEng(string date)
        {
            string result = date;
            if (date.Length >= 8)
            {
                result = result.Substring(8, 2) + "/" + result.Substring(5, 2) + "/" + result.Substring(0, 4);
            }

            return result;
        }

        /// <summary>
        ///  날짜 형식 변환 yyyymmdd=>dd/mm/yyyy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EngConvertDateType(string date)
        {
            string result = date;
            if (date.Length >= 8)
            {
                result = result.Substring(6, 2) + "/" + result.Substring(4, 2) + "/" + result.Substring(0, 4);
            }

            return result;
        }
        #endregion
    }
}