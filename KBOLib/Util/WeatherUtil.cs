using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Collections.Specialized;

namespace KBOLib.Util
{
    public class WeatherUtil
    {
        #region 구장 정식 명칭
        /// <summary>
        /// 구장 정식 명칭
        /// </summary>
        /// <param name="code">구장코드</param>
        /// <returns>한글명</returns>
        public static string GetStadiumFullName(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("GC", "고척스카이돔");
            nvcTb.Add("KC", "광주기아챔피언스필드");
            nvcTb.Add("DK", "대구삼성라이온즈파크");
            nvcTb.Add("DJ", "한화생명이글스파크");
            nvcTb.Add("MH", "인천SSG랜더스필드");
            nvcTb.Add("SJ", "사직야구장");
            nvcTb.Add("SW", "수원KT위즈파크");
            nvcTb.Add("CW", "창원NC파크");
            nvcTb.Add("JS", "잠실야구장");
            nvcTb.Add("UL", "울산문수야구장");
            nvcTb.Add("CJ", "청주야구장");
            nvcTb.Add("PH", "포항야구장");
            nvcTb.Add("GH", "강화SSG퓨처스필드");
            nvcTb.Add("GS", "경산볼파크");
            nvcTb.Add("KY", "고양야구장");
            nvcTb.Add("MS", "마산야구장");
            nvcTb.Add("MG", "문경상무야구장");
            nvcTb.Add("SD", "상동야구장");
            nvcTb.Add("SS", "서산야구장");
            nvcTb.Add("EC", "이천베어스파크");
            nvcTb.Add("EL", "이천챔피언스파크");
            nvcTb.Add("IS", "익산야구장");
            nvcTb.Add("HP", "기아챌린저스필드");
            nvcTb.Add("CC", "춘천의암야구장");
            nvcTb.Add("KJ", "기장현대차드림볼파크");

            return nvcTb.Get(code);
        }
        #endregion

        #region 미세먼지 코드 -> 명칭
        /// <summary>
        /// 미세먼지 코드 -> 명칭
        /// </summary>
        /// <param name="code">미세먼지코드</param>
        /// <returns>한글명</returns>
        public static string GetDustName(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("01", "좋음");
            nvcTb.Add("02", "보통");
            nvcTb.Add("03", "나쁨");
            nvcTb.Add("04", "매우나쁨");
            nvcTb.Add("-", "-");

            return nvcTb.Get(code);
        }
        #endregion

        #region 구장명 -> 구장 ID
        /// <summary>
        /// 구장명 -> 구장 ID
        /// </summary>
        /// <param name="stadiumName">구장명</param>
        /// <returns>구장명</returns>
        public static string GetStadiumName(object seasonId, object stadiumName)
        {
            string seriesName = "";
            DataTable dtStadium = CacheUtil.GetStadium();
            DataRow[] drStadium = dtStadium.Select(string.Format("SEASON_ID={0} AND S_NM='{1}'", seasonId.ToString(), stadiumName.ToString()));

            if (drStadium.Length > 0)
            {
                seriesName = drStadium[0]["S_ID"].ToString();
            }

            return seriesName;
        }
        #endregion

        #region 풍향 코드 => 풍향 텍스트
        // 풍향 코드 => 풍향 텍스트
        public static string GetWeatherWindDirection(object code)
        {
            int codeInt = int.Parse(code.ToString());
            switch (codeInt)
            {
                case 1:
                    return "북";
                case 2:
                    return "북동";
                case 3:
                    return "동";
                case 4:
                    return "남동";
                case 5:
                    return "남";
                case 6:
                    return "남서";
                case 7:
                    return "서";
                case 8:
                    return "북서";
                case 9:
                    return "북북동";
                case 10:
                    return "동북동";
                case 11:
                    return "동남동";
                case 12:
                    return "남남동";
                case 13:
                    return "남남서";
                case 14:
                    return "서남서";
                case 15:
                    return "서북서";
                case 16:
                    return "북북서";
                default:
                    return "-";
            }
        }
        #endregion

        #region 미세먼지 값 => 미세먼지 텍스트
        public static string GetDustPM10Text(object value)
        {
            double valueDouble = double.Parse(value.ToString());

            if(valueDouble < 30)
            {
                return GetDustName("01");
            }
            else if(valueDouble < 80)
            {
                return GetDustName("02");
            }
            else if(valueDouble < 150)
            {
                return GetDustName("03");
            }
            else
            {
                return GetDustName("04");
            }
        }
        #endregion

        #region 초 미세먼지 값 => 초 미세먼지 텍스트
        public static string GetDustPM25Text(object value)
        {
            double valueDouble = double.Parse(value.ToString());

            if (valueDouble < 15)
            {
                return GetDustName("01");
            }
            else if (valueDouble < 35)
            {
                return GetDustName("02");
            }
            else if (valueDouble < 75)
            {
                return GetDustName("03");
            }
            else
            {
                return GetDustName("04");
            }
        }
        #endregion

        #region 날씨 코드 => 날씨 텍스트
        /// <summary>
        /// 날씨 코드 => 날씨 텍스트
        /// </summary>
        /// <param name="code">미세먼지코드</param>
        /// <returns>한글명</returns>
        public static string GetWeatherText(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();
            nvcTb.Add("01", "맑음");
            nvcTb.Add("02", "구름조금");
            nvcTb.Add("03", "구름많음");
            nvcTb.Add("04", "흐림");
            nvcTb.Add("05", "흐린후차차갬");
            nvcTb.Add("06", "맑은후차차흐려짐");
            nvcTb.Add("07", "소나기");
            nvcTb.Add("08", "오전소나기");
            nvcTb.Add("09", "오후소나기");
            nvcTb.Add("10", "흐리고비");
            nvcTb.Add("11", "오전비");
            nvcTb.Add("12", "오후비");
            nvcTb.Add("13", "차차흐려져비");
            nvcTb.Add("14", "차차흐려져오후비");
            nvcTb.Add("15", "비온후갬");
            nvcTb.Add("16", "오전비온후갬");
            nvcTb.Add("17", "오후비온후갬");
            nvcTb.Add("18", "흐리고눈");
            nvcTb.Add("19", "오전눈");
            nvcTb.Add("20", "오후눈");
            nvcTb.Add("21", "차차흐려져눈");
            nvcTb.Add("22", "차차흐려져오후눈");
            nvcTb.Add("23", "눈온후갬");
            nvcTb.Add("24", "오전눈온후갬");
            nvcTb.Add("25", "오후눈온후갬");
            nvcTb.Add("26", "비또는눈");
            nvcTb.Add("27", "오전비또는눈");
            nvcTb.Add("28", "오후비또는눈");
            nvcTb.Add("29", "차차흐려져비또는눈");
            nvcTb.Add("30", "차차흐려져오후비또는눈");
            nvcTb.Add("31", "눈또는비");
            nvcTb.Add("32", "오전눈또는비");
            nvcTb.Add("33", "오후눈또는비");
            nvcTb.Add("34", "차차흐려져눈또는비");
            nvcTb.Add("35", "차차흐려져오후비또는눈");
            nvcTb.Add("36", "눈또는비온후갬");
            nvcTb.Add("37", "오전비또는눈");
            nvcTb.Add("38", "오후비또는눈");
            nvcTb.Add("39", "천둥번개");
            nvcTb.Add("40", "안개");

            return nvcTb.Get(code);
        }
        #endregion
    }
}
