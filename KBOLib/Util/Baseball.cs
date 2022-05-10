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
    /// <summary>
    /// 야구 코드
    /// </summary>
    public class Baseball
    {
        public const int KBO_START_YEAR = 1982;             // KBO 1군 시작 년도
        public const int KBO_DETAIL_START_YEAR = 2001;      // KBO 1군 세부 기록 제공 시작 년도
        public const int KBO_REGULAR_SR_END_YEAR = 2018;    // KBO 1군 정규시즌 최종 년도
        public const int KBO_END_YEAR = 2022;               // KBO 1군 최종 년도
        public const int KBO_EXHIBITION_START_YEAR = 2002;  // KBO 1군 시범경기 서비스 시작 년도
        public const int KBO_POST_START_YEAR = 1982;        // KBO 1군 포스트시즌 서비스 시작 년도

        public const int FUTURES_START_YEAR = 2010;     // 퓨처스 시작 년도
        public const int FUTURES_END_YEAR = 2022;       // 퓨처스 최종 년도

        public const int KBO_END_TAB_YEAR = 2020;       // 탭 표출용 KBO 1군 최종 년도
        public const int FUTURES_END_TAB_YEAR = 2021;   // 탭 표출용 퓨처스 최종 년도
        public const int KBO_ENTRY_YEAR = 2022;         // 엔트리 등록 일수 최종 년도
        public const int KBO_CHANGE_YEAR = 2021;        // 팀명 바뀐 가장 최신 년도
        public const int KBO_TEAMRANK_STANDARD_YEAR = 2022; // 전년도 최종 팀 순위를 구하기 위한 값. ex) 새로운 시즌이 시작하며 KBO_END_YEAR가 바뀌어도 
                                                            // 해당 시즌 종료전까지 값 유지.

        public const int KBO_LE_ID = 1;                 // 1군 리그 ID
        public const int FUTURES_LE_ID = 2;             // 퓨처스리그 ID

        public const int REGULAR_SR_ID = 0;             // 정규시즌
        public const int EXHIBITION_SR_ID = 1;          // 시범 경기
        public const int PRACTICE_SR_ID = 2;            // 연습경기
        public const int SEMI_PLAYOFFS_SR_ID = 3;       // 준플레이오프
        public const int WILDCARD_SR_ID = 4;            // 와일드카드
        public const int PLAYOFFS_SR_ID = 5;            // 플레이오프
        public const int KOREAN_SR_ID = 7;              // 한국시리즈
        public const string POST_SR_ID = "3,4,5,7";     // 플레이오프
        public const int INTERNATIONAL_SR_ID = 8;       // 국제 경기
        public const int ALL_START_SR_ID = 9;           // 올스타전
        public const int INDIE_SR_ID = 10;              // 교류경기

        /**********************************************************************
         * 팀명, 구장명 : 현재 제공되고있는 모든 명칭들이 DB화 될경우 변경예정
        ***********************************************************************/

        // 포지션 정보
        public static string[,] position = {{"1루수", "一", "3"}, {"2루수", "二", "4"}, {"3루수", "三", "5"},{"유격수","유", "6"}
                                            ,{"중견수","중", "8"},{"좌익수","좌", "7"},{"우익수","우", "9"},{"포수","포", "2"},{"대타","타", ""}
                                            ,{"대주자","주", ""},{"지명타자","지", ""},{"투수","투", "1"}};

        //// 현재 사용중인 경기장 정보
        public static String[] stadium = {"고양","고척","군산","광주","기장","대구","대전","마산","문학","사직","상동","수원","울산","인천","잠실","목동","제주","청주","포항","팻코", "파크", "화성", "창원"
                                            ,"다저 스타디움","도쿄돔","타이중","타오위앤","인터컨티넨탈","도우리우","WKB 필드2"
                                            , "삿포로돔", "티엔무", "도류"   // 2016-07-14 yeeun, 김인성 대리 요청, 고척돔->고척 수정 20151027 추가 ( 프리미어21 )
                                            ,"WKB 메인필드","빅N ","미정","야후돔", "타이중", "히람비톤", "솔트리버", "체이스필드", "AT&T파크", "말린스파크"
                                            , "할리스코", "펫코 파크", "GBK", "라와망운", "지바", "요코하마", "후쿠시마"
                                            , "Field1"};

        // 한국프로야구 번외경기 팀 정보
        public static String[,] minorIndieTeam = { {"HT","KIA","KIA 타이거즈"}
                                            , {"OB","두산","두산 베어스"}
                                            , {"LT","롯데","롯데 자이언츠"}
                                            , {"SS","삼성","삼성 라이온즈"}
                                            , {"SM","상무","상무"}
                                            , {"SK","SSG","SSG 랜더스"}
                                            , {"LG","LG","LG 트윈스"}
                                            , {"HH","한화","한화 이글스"}
                                            , {"WO","화성","화성 히어로즈"}
                                            , {"PL","경찰","경찰"}
                                            , {"NC","고양","고양 다이노스"}
                                            , {"KT","KT","KT 위즈"}
                                            , {"SO","소프트","소프트뱅크 호크스"}};
                                            //, {"KY","고양","고양원더스"} 


        // 한국프로야구 2군 구장 정보
        public static String[] minorStadium = {
                                                "강진", "강화", "경산", "고양", "고척", "광주", "구리", "군산", "기장"
                                                , "남해", "대구", "대전", "마산", "목동", "무등", "문경", "문학", "벽제"
                                                , "사직", "상동", "상무", "서산", "성대", "송도", "수원", "오라", "울산"
                                                , "이천", "이천(두산)", "이천(LG)", "익산", "잠실", "창원", "청주", "춘천", "포항"
                                                , "함평", "화성"
                                              };

        // 현재 사용중인 팀정보 
        // 2015-10-05 yeeun, 반상호대리 요청 {"WO","우리","넥센 히어로즈"}
        // 2018-12-03 yeeun, 홍지희사원 요청 {"WO","넥센","넥센 히어로즈"}
        public static String[,] team ={{"SS","삼성","삼성 라이온즈"},{"HD","현대","현대 유니콘스"},{"OB","두산","두산 베어스"},{"HT","KIA","KIA 타이거즈"},{"LT","롯데","롯데 자이언츠"}
                                            ,{"HH","한화","한화 이글스"},{"SK","SSG","SSG 랜더스"},{"LG","LG","LG 트윈스"},{"WO","키움","키움 히어로즈"},{"NC","NC","NC 다이노스"},{"KT","KT","KT 위즈"}
                                            ,{"SM","상무","상무"},{"PL","경찰","경찰"},{"WE","나눔","올스타"},{"EA","드림","올스타"},{"K1","대표팀","대표팀"}
                                            ,{"K2","상비군","상비군"},{"PO","준결승","준결승 진출팀"},{"SB","남부","남부"},{"NB","북부","북부"}
                                            ,{"KR","대한민국","대한민국"},{"JP","일본","일본"},{"TW","대만","대만"},{"US","미국","미국"},{"DO","도미니카","도미니카"}
                                            ,{"CN","중국","중국"},{"MN","몽고","몽고"},{"CA","캐나다","캐나다"},{"IT","이탈리아","이탈리아"},{"VE","베네수엘라","베네수엘라"}
                                            ,{"AU","호주","호주"},{"MX","멕시코","멕시코"},{"P1","필리핀","필리핀"},{"ES","스페인","스페인"},{"DE","독일","독일"}
                                            ,{"TH","태국","태국"},{"HK","홍콩","홍콩"},{"PK","파키스탄","파키스탄"},{"PA","파나마","파나마"},{"PR","푸에르토","푸에르토"}
                                            ,{"ZA","남아공","남아공"},{"SD","샌디에고","샌디에고"},{"LA","LA다저스","LA다저스"}
                                            ,{"BE","슝디","슝디 엘리펀츠"},{"CL","지바롯데","지바롯데"},{"CD","주니치","주니치 드래곤즈"}
                                            ,{"YO","요미우리","요미우리 자이언츠"}, {"SO","소프트뱅크","소프트뱅크 호크스"}, {"PE","퍼스","퍼스 히트"}, {"TJ","톈진","톈진"},{"TE","퉁이","퉁이 라이온즈"}
                                            ,{"LM", "라미고", "라미고 몽키즈"},{"CS", "차이나", "차이나 스타즈"},{"DO", "도미니카", "도미니카"},{"BR", "브라질", "브라질"},{"CU", "쿠바", "쿠바"}
                                            ,{"NL", "네덜란드", "네덜란드"},{"FB", "볼로냐", "포르티투도 볼로냐"},{"CV", "캔버라", "캔버라 캐벌리"},{"RK", "라쿠텐", "라쿠텐 골든이글스"}
                                            ,{"IL", "이스라엘", "이스라엘"},{"CO", "콜롬비아", "콜롬비아"}, {"ID", "인도네시아", "인도네시아"}, {"LK", "스리랑카", "스리랑카"}, {"LA", "라오스", "라오스"}, {"YB", "라이징", "라이징스타"}};

        // 한국프로야구 1군 팀정보
        public static String[,] Kteam = {  
                                        {"SS","삼성","삼성 라이온즈","SAMSUNG", "SAMSUNG LIONS"},
                                        {"SK","SSG","SSG 랜더스","SSG", "SSG LANDERS"},
                                        {"LT","롯데","롯데 자이언츠","LOTTE", "LOTTE GIANTS"},
                                        {"HT","KIA","KIA 타이거즈","KIA", "KIA TIGERS"},
                                        {"OB","두산","두산 베어스","DOOSAN", "DOOSAN BEARS"},
                                        {"LG","LG","LG 트윈스","LG", "LG TWINS"},
                                        {"HH","한화","한화 이글스","HANWHA", "HANWHA EAGLES"},
                                        //{"WO","넥센","넥센 히어로즈","NEXEN", "NEXEN HEROES"},
                                        {"WO","키움","키움 히어로즈","KIWOOM", "KIWOOM HEROES"}, // 2018-12-04, yeeun, 홍지희사원 요청, 영문명은 확실치 않음
                                        {"NC","NC","NC 다이노스","NC", "NC DINOS"},
                                        {"KT","KT","KT 위즈","KT", "KT WIZ"},
                                        {"WE","웨스턴","웨스턴", "", ""},
                                        {"EA","이스턴","이스턴", "", ""}
                                    };

        // 경기 TYPE
        public static string[,] gameType = { { "0", "REGULAR" }, { "4", "EXHIBITION" }, { "51", "WBC" }, { "31", "Asia Series" }, {"5", "Allstar game"}
                                            , { "7", "Playoff Semi-final game1" }, { "8", "Playoff Semi-final game2" }, { "9", "Playoff Semi-final game3" }, { "10", "Playoff Semi-final game4" }
                                            , { "11", "Playoff Semi-final game5" }
                                            , { "12", "Playoff final game1" }, { "13", "Playoff final game2" }, { "14", "Playoff final game3" }, { "15", "Playoff final game4" }, { "16", "Playoff final game5" }
                                            , { "17", "Playoff final game6" }, { "18", "Playoff final game7" }
                                            , { "19", "Korean Series game1" }, { "20", "Korean Series game2" }, { "21", "Korean Series game3" }, { "22", "Korean Series game4" }, { "23", "Korean Series game5" }
                                            , { "24", "Korean Series game6" }, { "25", "Korean Series game7" }, { "26", "Wild Card" }, { "27", "Wild Card" }, { "99", "Tie Breaker" } };
        
        //더블헤더 정보
        public static string[,] doubleHead = { { "0", "-" }, { "1", "DH1" }, { "2", "DH2" } };

        #region ID -> 한글명 변환
        /// <summary>
        /// cd_detail -> 이름을 반환
        /// </summary>
        /// <param name="id">경기구분 아이디</param>
        /// <returns>경기구분 이름</returns>
        public static string GetCdDetailName(string id)
        {
            string result = "";
            DataSet dsDetail = CacheUtil.GetDetailCode();
            DataRow[] drGame = dsDetail.Tables[0].Select();

            foreach (DataRow row in drGame)
            {
                if (id.Equals(row["CD_SE"].ToString()))
                {
                    result = row["CD_NM"].ToString().Trim();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 취소 경기 아이디 -> 이름을 반환
        /// </summary>
        /// <param name="id">취소 경기 아이디</param>
        /// <returns>취소 경기 이름</returns>
        public static string GetCancelName(string id)
        {
            string result = "";
            DataSet dsCancel = CacheUtil.GetCancelSc();
            DataRow[] drCancel = dsCancel.Tables[0].Select();

            foreach (DataRow row in drCancel)
            {
                if (id.Equals(row["SC_ID"].ToString()))
                {
                    result = row["SC_NM"].ToString();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 방송사 ID -> 방송사 Short Name
        /// </summary>
        /// <param name="bcSc">방송사 구분(RADIO, TV)</param>
        /// <param name="id">방송사 ID</param>
        /// <returns>방송사 short Name</returns>
        public static string GetBroadCastName(string bcSc, string id)
        {
            string result = "";
            DataSet dsBroadcast = CacheUtil.GetBroadcast();
            DataRow[] drBroadcast = dsBroadcast.Tables[0].Select(string.Format("BC_SC='{0}'", bcSc));

            foreach (DataRow row in drBroadcast)
            {
                if (id.Equals(row["BC_ID"].ToString()))
                {
                    result = row["SHORT_NM"].ToString().Trim();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 방송사 ID -> 방송사 Full Name
        /// </summary>
        /// <param name="bcSc">방송사 구분(RADIO, TV)</param>
        /// <param name="id">방송사 ID</param>
        /// <returns>방송사 Full Name</returns>
        public static string GetBroadCastFullName(string bcSc, string id)
        {
            string result = "";
            DataSet dsBroadcast = CacheUtil.GetBroadcast();
            DataRow[] drBroadcast = dsBroadcast.Tables[0].Select(string.Format("BC_SC='{0}'", bcSc));

            foreach (DataRow row in drBroadcast)
            {
                if (id.Equals(row["BC_ID"].ToString()))
                {
                    result = row["FULL_NM"].ToString().Trim();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 경기 구분 아아디 -> 이름을 반환
        /// </summary>
        /// <param name="id">경기구분 아이디</param>
        /// <returns>경기구분 이름</returns>
        public static string GetGameName(string id)
        {
            string result = "";
            DataSet dsGame = CacheUtil.GetGameSc();
            DataRow[] drGame = dsGame.Tables[0].Select();

            foreach (DataRow row in drGame)
            {
                if (id.Equals(row["SC_ID"].ToString()))
                {
                    result = row["SC_NM"].ToString().Trim();
                    break;
                }
            }

            return result;
        }
        
        /// <summary>
        /// 리그 아이디 -> 리그명
        /// </summary>
        /// <param name="leagueId">리그 아이디</param>
        /// <returns>리그명</returns>
        public static string GetLeagueName(object leagueId)
        {
            string leagueName = "";
            DataTable dtLeague = CacheUtil.GetLeague();
            DataRow[] drLeague = dtLeague.Select(string.Format("LE_ID={0}", leagueId.ToString()));

            if (drLeague.Length > 0)
            {
                leagueName = drLeague[0]["LE_NM"].ToString();
            }

            return leagueName;
        }

        /// <summary>
        /// 시리즈 ID -> 시리즈명
        /// </summary>
        /// <param name="seriesId">시리즈 ID</param>
        /// <returns>시리즈명</returns>
        public static string GetSeriesName(string seriesId)
        {   
            NameValueCollection nvcSeries = new NameValueCollection();

            nvcSeries.Add("0", "KBO 정규시즌");
            nvcSeries.Add("1", "KBO 시범경기");
            nvcSeries.Add("2", "연습경기");
            nvcSeries.Add("3", "준플레이오프");
            nvcSeries.Add("4", "와일드카드 결정전");
            nvcSeries.Add("5", "플레이오프");
            nvcSeries.Add("7", "한국시리즈");
            nvcSeries.Add("8", "국제경기");
            nvcSeries.Add("9", "KBO 올스타전");
            nvcSeries.Add("10", "교류경기");
            nvcSeries.Add("6", "연습경기");

            return nvcSeries.Get(seriesId);
        }

        /// <summary>
        /// 구장 ID -> 구장명
        /// </summary>
        /// <param name="stadiumId">구장 ID</param>
        /// <returns>구장명</returns>
        public static string GetStadiumName(object seasonId, object stadiumId)
        {
            string seriesName = "";
            DataTable dtStadium = CacheUtil.GetStadium();
            DataRow[] drStadium = dtStadium.Select(string.Format("SEASON_ID={0} AND S_ID='{1}'", seasonId.ToString(), stadiumId.ToString()));

            if (drStadium.Length > 0)
            {
                seriesName = drStadium[0]["S_NM"].ToString();
            }

            return seriesName;
        }

        /// <summary>
        /// 리그/시즌별 팀 ID -> 팀명 (ex, 삼성)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="teamId">팀 ID</param>
        /// <returns>팀명</returns>
        public static string GetTeamName(object leagueId, object seasonId, object teamId)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetTeam();
            DataRow[] drTeam = null;

            if (teamId.ToString() == "SM")
            {
                drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", Baseball.FUTURES_LE_ID, seasonId.ToString(), teamId.ToString()));
            }
            else {
                drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString()));
            }
            

            if (drTeam.Length > 0)
            {
                teamName = drTeam[0]["FIRST_NM"].ToString();

                if (string.IsNullOrEmpty(teamName))
                {
                    teamName = drTeam[0]["LAST_NM"].ToString();
                }

                if (int.Parse(leagueId.ToString()) == Baseball.FUTURES_LE_ID && (int.Parse(seasonId.ToString()) >= 2010 && int.Parse(seasonId.ToString()) <= 2012) && teamName == "우리")
                {
                    teamName = "넥센";
                }
            }

            if (teamId.ToString() == "KY")
            {
                teamName = "고양";
            }

            return teamName;
        }

        /// <summary>
        /// 리그/시즌별 팀 ID -> 전체 팀명 (ex, 삼성 라이오즌)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="teamId">팀 ID</param>
        /// <returns>전체 팀명</returns>
        public static string GetTeamFullName(object leagueId, object seasonId, object teamId)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetTeam();
            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString()));

            if (drTeam.Length > 0)
            {
                teamName = string.Format("{0} {1}", drTeam[0]["FIRST_NM"].ToString(), drTeam[0]["LAST_NM"].ToString());
            }

            return teamName;
        }

        /// <summary>
        /// 리그/시즌별 팀 ID -> 팀명 (ex, 삼성)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="teamId">팀 ID</param>
        /// <returns>팀명</returns>
        public static string GetCareerTeamName(object leagueId, object seasonId, object teamId, object pageGubun)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetCareerTeam();
            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString()));

            if (drTeam.Length > 0)
            {
                teamName = drTeam[0]["FIRST_NM"].ToString();

                if (string.IsNullOrEmpty(teamName))
                {
                    teamName = drTeam[0]["LAST_NM"].ToString();
                }

                if (int.Parse(leagueId.ToString()) == Baseball.FUTURES_LE_ID && (int.Parse(seasonId.ToString()) >= 2010 && int.Parse(seasonId.ToString()) <= 2012) && teamName == "우리")
                {
                    teamName = "넥센";
                }

                if (pageGubun.ToString() == "draft" && seasonId.ToString() == "1999" && teamId.ToString() == "OB")
                {
                    teamName = "OB";
                }
                else if (pageGubun.ToString() == "draft" && seasonId.ToString() == "2001" && teamId.ToString() == "HT")
                {
                    teamName = "해태";
                }
            }
            else // 2015-11-09 yeeun, 추가
            {
                //if (leagueId.ToString() == KBO_LE_ID.ToString() && seasonId.ToString() == "2000" && teamId.ToString() == "SB")
                //{
                //    teamName = "쌍방울";
                //}
                //else if (leagueId.ToString() == KBO_LE_ID.ToString() && seasonId.ToString() == "2008" && teamId.ToString() == "HD")
                //{
                //    teamName = "현대";
                //}
                //else if (leagueId.ToString() == KBO_LE_ID.ToString() && (seasonId.ToString() == "2011" || seasonId.ToString() == "2012") && teamId.ToString() == "NC")
                //{
                //    teamName = "NC";
                //}
                //else if (leagueId.ToString() == KBO_LE_ID.ToString() && (seasonId.ToString() == "2013" || seasonId.ToString() == "2014") && teamId.ToString() == "KT")
                //{
                //    teamName = "kt";
                //}
                if (teamId.ToString() == "SM")
                {
                    teamName = "상무";
                }
                else if (teamId.ToString() == "PL")
                {
                    teamName = "경찰";
                }
            }

            return teamName;
        }

        /// <summary>
        /// 리그/시즌별 팀 ID -> 전체 팀명 (ex, 삼성 라이오즌)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="teamId">팀 ID</param>
        /// <returns>전체 팀명</returns>
        public static string GetCareerTeamFullName(object leagueId, object seasonId, object teamId, object pageGubun)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetCareerTeam();
            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString()));

            if (drTeam.Length > 0)
            {
                teamName = string.Format("{0} {1}", drTeam[0]["FIRST_NM"].ToString(), drTeam[0]["LAST_NM"].ToString());

                if (pageGubun.ToString() == "draft" && seasonId.ToString() == "1999" && teamId.ToString() == "OB")
                {
                    teamName = "OB 베어스";
                }
                else if (pageGubun.ToString() == "draft" && seasonId.ToString() == "2001" && teamId.ToString() == "HT")
                {
                    teamName = "해태 타이거즈";
                }
            }
            else // 2015-11-09 yeeun, 추가
            {
                //if (leagueId.ToString() == KBO_LE_ID.ToString() && seasonId.ToString() == "2000" && teamId.ToString() == "SB")
                //{
                //    teamName = "쌍방울 레이더스";
                //}
                //else if (leagueId.ToString() == KBO_LE_ID.ToString() && seasonId.ToString() == "2008" && teamId.ToString() == "HD")
                //{
                //    teamName = "현대 유니콘스";
                //}
                //else if (leagueId.ToString() == KBO_LE_ID.ToString() && (seasonId.ToString() == "2011" || seasonId.ToString() == "2012") && teamId.ToString() == "NC")
                //{
                //    teamName = "NC 다이노스";
                //}
                //else if (leagueId.ToString() == KBO_LE_ID.ToString() && (seasonId.ToString() == "2013" || seasonId.ToString() == "2014") && teamId.ToString() == "KT")
                //{
                //    teamName = "kt wiz";
                //}
                if (teamId.ToString() == "SM")
                {
                    teamName = "상무";
                }
                else if (teamId.ToString() == "PL")
                {
                    teamName = "경찰";
                }
            }

            return teamName;
        }

        /// <summary>
        /// KBO관리자 - 경기일정관리, 선발투수
        /// </summary>
        /// <param name="leagueId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <param name="position">포지션</param>
        /// <returns></returns>
        public static DataRow[] GetAdminSartPitcher(object leagueId, object seasonId, object teamId, object positionNo)
        {
            DataRow[] playerList = null;

            DataTable dtPlayer = CacheUtil.GetPlayer();
            //DataRow[] drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}' AND POS_NO='{3}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString(), positionNo.ToString()));

            DataView dvPlayer = new DataView(dtPlayer);
            dvPlayer.RowFilter = string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}' AND POS_NO='{3}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString(), positionNo.ToString());
            dvPlayer.Sort = "P_NM";

            DataRow[] drPlayer = dvPlayer.ToTable().Select();

            playerList = drPlayer;

            return playerList;
        }

        /// <summary>
        /// 올스타 선발투수
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="seasonId"></param>
        /// <param name="positionNo"></param>
        /// <returns></returns>
        public static DataRow[] GetAdminAllstarStartPitcher(object leagueId, object seasonId, object positionNo)
        {
            DataRow[] playerList = null;

            DataTable dtPlayer = CacheUtil.GetPlayer();
            //DataRow[] drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}' AND POS_NO='{3}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString(), positionNo.ToString()));

            DataView dvPlayer = new DataView(dtPlayer);
            dvPlayer.RowFilter = string.Format("LE_ID={0} AND SEASON_ID={1} AND POS_NO='{2}'", leagueId.ToString(), seasonId.ToString(), positionNo.ToString());
            dvPlayer.Sort = "P_NM";

            DataRow[] drPlayer = dvPlayer.ToTable().Select();

            playerList = drPlayer;

            return playerList;
        }

        /// <summary>
        /// 선수 ID -> 선수명 (ex, 이승엽)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="playerId">선수 ID</param>
        /// <returns>선수명</returns>
        public static string GetPlayerName(object leagueId, object seasonId, object playerId)
        {
            string playerName = string.Empty;
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = null;

            if (!string.IsNullOrEmpty(playerId.ToString()))
            {
                drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND P_ID='{2}'", leagueId.ToString(), seasonId.ToString(), playerId.ToString()));

                if (drPlayer.Length > 0)
                {
                    playerName = drPlayer[0]["P_NM"].ToString();
                }
            }

            return playerName;
        }

        /// <summary>
        /// 선수 ID -> 선수(컬럼) 정보 
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="playerId">선수 ID</param>
        /// <param name="col">컬럼명</param>
        /// <returns>선수 정보</returns>
        public static string GetPlayerType(object leagueId, object seasonId, object playerId, string col)
        {
            string playerName = string.Empty;
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = null;

            if (!string.IsNullOrEmpty(playerId.ToString()))
            {
                drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND P_ID='{2}'", leagueId.ToString(), seasonId.ToString(), playerId.ToString()));

                if (drPlayer.Length > 0)
                {
                    if (col.Equals("PIT_DIREC_CD") || col.Equals("PIT_FORM_CD") || col.Equals("HIT_DIREC_CD"))
                    {
                        playerName = Baseball.GetCdDetailName(drPlayer[0][col].ToString());
                    }
                    else
                    {
                        playerName = drPlayer[0][col].ToString();
                    }
                }
            }

            return playerName;
        }

        /// <summary>
        /// 선수 ID -> 선수명 (ex, 이승엽)
        /// </summary>
        /// <param name="playerId">선수 ID</param>
        /// <returns>선수명</returns>
        public static string GetPlayerNameSimple(object playerId)
        {
            string playerName = "";
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = dtPlayer.Select(string.Format("P_ID='{0}'", playerId.ToString()), "SEASON_ID DESC");

            if (drPlayer.Length > 0)
            {
                playerName = drPlayer[0]["P_NM"].ToString();
            }

            return playerName;
        }

        /// <summary>
        /// 선수 ID -> 팀 ID
        /// </summary>
        /// <param name="playerId">선수 ID</param>
        /// <returns>선수명</returns>
        public static string GetPlayerTNmSimple(object playerId)
        {
            string tNm = "";
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = dtPlayer.Select(string.Format("P_ID='{0}' AND SECTION_CD = '12' AND LE_ID = 1 AND FIRST_NM <> ''", playerId.ToString()), "SEASON_ID DESC");

            if (drPlayer.Length > 0)
            {
                tNm = drPlayer[0]["FIRST_NM"].ToString();
            }

            return tNm;
        }

        /// <summary>
        /// 선수 ID -> 선수명 (ex, 이승엽(19760818))
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="playerId">선수 ID</param>
        /// <returns>선수명</returns>
        public static string GetPlayerNameBirthDay(object playerId)
        {
            string playerName = "";
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = dtPlayer.Select(string.Format("P_ID='{0}'", playerId.ToString()));

            if (drPlayer.Length > 0)
            {
                playerName = string.Format("{0}({1})", drPlayer[0]["P_NM"], DateUtil.GetFormatDate(drPlayer[0]["BIRTH_DT"].ToString(), "yyyyMMdd"));
            }

            return playerName;
        }

        /// <summary>
        /// 선수 ID -> 선수 투타유형 (ex, 우투우타)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="playerId">선수 ID</param>
        /// <returns>선수명</returns>
        public static string GetPlayerPitBatInfo(object leId, object seasonId, object playerId)
        {
            string result = "";
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND P_ID='{2}'", leId.ToString(), seasonId.ToString(), playerId.ToString()));

            if (drPlayer.Length > 0)
            {
                result = GetPitBatInfo(drPlayer[0]["PIT_DIREC_CD"], drPlayer[0]["PIT_FORM_CD"], drPlayer[0]["HIT_DIREC_CD"]);
            }

            return result;
        }

        /// <summary>
        /// 선수 ID -> 은퇴유무
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="playerId">선수 ID</param>
        /// <returns>선수은퇴유무</returns>
        public static bool GetPlayerRetire(object leagueId, object seasonId, object playerId)
        {
            bool retireYN = false;
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataRow[] drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID<={1} AND P_ID='{2}'", leagueId.ToString(), seasonId.ToString(), playerId.ToString()), "SEASON_ID DESC");

            if (drPlayer.Length > 0)
            {
                retireYN = drPlayer[0]["RETIRE_CK"].ToString() == "1" ? true : false;
            }

            return retireYN;
        }

        /// <summary>
        /// 포지션 아이디 -> 이름
        /// </summary>
        /// <param name="posNo">포지션 번호</param>
        /// <returns>포지션 이름</returns>
        public static string GetPosName(object posNo)
        {
            NameValueCollection nvcPos = new NameValueCollection();

            nvcPos.Add("1", "투수");
            nvcPos.Add("2", "포수");
            nvcPos.Add("3", "1루수");
            nvcPos.Add("4", "2루수");
            nvcPos.Add("5", "3루수");
            nvcPos.Add("6", "유격수");
            nvcPos.Add("7", "좌익수");
            nvcPos.Add("8", "중견수");
            nvcPos.Add("9", "우익수");
            nvcPos.Add("D", "지명타자");
            nvcPos.Add("H", "대타");
            nvcPos.Add("R", "대주자");

            return nvcPos.Get(posNo.ToString());
        }

        /// <summary>
        /// 포지션 아이디 -> 이름
        /// </summary>
        /// <param name="code">포지션 번호</param>
        /// <returns>포지션 이름</returns>
        public static string GetPositionFullName(object code)
        {
            string result = string.Empty;

            if (code.ToString() == "1")
                result = "투수";
            else if (code.ToString() == "2")
                result = "포수";
            else if (code.ToString() == "3" || code.ToString() == "4" || code.ToString() == "5" || code.ToString() == "6")
                result = "내야수";
            else if (code.ToString() == "7" || code.ToString() == "8" || code.ToString() == "9")
                result = "외야수";
            else
                result = "";

            return result;
        }

        /// <summary>
        /// 포지션 아이디 -> 이름
        /// </summary>
        /// <param name="posNo">포지션 번호</param>
        /// <returns>포지션 이름</returns>
        public static string GetPositionName(string data)
        {
            string result = string.Empty;

            if (data == "1")
                result = "투";
            else if (data == "2")
                result = "포";
            else if (data == "3" || data == "4" || data == "5" || data == "6")
                result = "내";
            else
                result = "외";

            return result;
        }

        /// <summary>
        /// 포지션 아이디 -> 이름
        /// </summary>
        /// <param name="posNo">포지션 번호</param>
        /// <returns>포지션 이름</returns>
        public static string GetPositionNameEntry(object leagueId, string data, object playerId)
        {
            //string[] arrayList = { "90419", "88274", "86321", "87721", "85410", "72177", "83574", "30003", "82460", "90660" };  // 2군 감독 리스트
            // SK-김무관, KT-이상훈, 두산-이강철, KIA-정회열, 경찰-유승안, 고양(nc)-한문연, 화성-스펜서, 롯데-손상대, 삼성-성준, LG-김동수, 한화-최계훈
            string[] arrayList = { "96473", "80091", "89620", "90660", "82174", "83574", "66390", "30003", "86470", "90419", "95488" };  // 2군 감독 리스트
            
            if (leagueId.ToString() == Baseball.FUTURES_LE_ID.ToString())
            {
                foreach (string array in arrayList)
                {
                    if (array == playerId.ToString())
                    {
                        data = "1";
                    }
                }
            }

            NameValueCollection nvcPos = new NameValueCollection();

            nvcPos.Add("1", "감독");
            nvcPos.Add("2", "코치");
            nvcPos.Add("3", "투");
            nvcPos.Add("4", "포");
            nvcPos.Add("5", "내");
            nvcPos.Add("6", "외");

            return nvcPos.Get(data);
        }

        /// <summary>
        /// PERSON에서 사용 되는 포지션 내 -> 내야수
        /// </summary>
        /// <param name="shotName">축약어</param>
        /// <returns>명칭(full)</returns>
        public static string GetPosFullName(string shotName)
        {
            NameValueCollection nvcPos = new NameValueCollection();

            nvcPos.Add("코치", "코치");
            nvcPos.Add("투", "투수");
            nvcPos.Add("내", "내야수");
            nvcPos.Add("포", "포수");
            nvcPos.Add("외", "외야수");
            nvcPos.Add("감독", "감독");

            return nvcPos.Get(shotName);
        }

        /// <summary>
        /// 올스타 포지션 아이디 -> 이름
        /// </summary>
        /// <param name="posNo">포지션 번호</param>
        /// <returns>포지션 이름</returns>
        public static string GetAllStarPosName(string posNo)
        {
            NameValueCollection nvcPos = new NameValueCollection();

            nvcPos.Add("1", "투수");
            nvcPos.Add("M", "중간");
            nvcPos.Add("G", "구원");
            nvcPos.Add("2", "포수");
            nvcPos.Add("3", "1루수");
            nvcPos.Add("4", "2루수");
            nvcPos.Add("5", "3루수");
            nvcPos.Add("6", "유격수");
            nvcPos.Add("7", "외야수");
            nvcPos.Add("D", "지명타자");

            return nvcPos.Get(posNo);
        }

        /// <summary>
        /// 승/패/세 결과값 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetWlsName(string code)
        {
            NameValueCollection nvcWls = new NameValueCollection();
            
            nvcWls.Add("W", "승");
            nvcWls.Add("L", "패");
            nvcWls.Add("S", "세");
            nvcWls.Add("H", "홀");

            return nvcWls.Get(code);
        }

        /// <summary>
        /// 승/패/세 결과값 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>s>
        public static string GetWlsPitcherName(string code)
        {
            NameValueCollection nvcWls = new NameValueCollection();

            nvcWls.Add("W", "승리투수");
            nvcWls.Add("L", "패전투수");
            nvcWls.Add("S", "세이브투수");
            nvcWls.Add("H", "홀드투수");
            nvcWls.Add("D", "무승부투수");

            return nvcWls.Get(code);
        }

        /// <summary>
        /// 초/말 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetTbName(object code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("T", "초");
            nvcTb.Add("B", "말");

            return nvcTb.Get(code.ToString());
        }

        /// <summary>
        /// 홈/방문 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetHomeAwayName(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("T", "방문");
            nvcTb.Add("B", "홈");

            return nvcTb.Get(code);
        }

        /// <summary>
        /// 구종 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetPitKind(string code)
        {
            NameValueCollection nvcPitKind = new NameValueCollection();

            nvcPitKind.Add("F", "직구");
            nvcPitKind.Add("B", "변화구");

            return nvcPitKind.Get(code);
        }

        /// <summary>
        /// 주/야간 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetDayNightName(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("D", "주간");
            nvcTb.Add("N", "야간");

            return nvcTb.Get(code);
        }

        /// <summary>
        /// 기간별 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetHalfName(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("F", "전반기");
            nvcTb.Add("S", "후반기");

            return nvcTb.Get(code);
        }

        /// <summary>
        /// 투수유형별 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GePitcherTypeName(string code)
        {
            NameValueCollection nvcPitcherType = new NameValueCollection();

            nvcPitcherType.Add("LO", "좌투수");
            nvcPitcherType.Add("RO", "우투수");
            nvcPitcherType.Add("RU", "언더투수");

            return nvcPitcherType.Get(code);
        }

        /// <summary>
        /// 타자유형별 한글명 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GeHitterTypeName(string code)
        {
            NameValueCollection nvcPitcherType = new NameValueCollection();

            nvcPitcherType.Add("L", "좌타자");
            nvcPitcherType.Add("R", "우타자");

            return nvcPitcherType.Get(code);
        }

        /// <summary>
        /// 선수 투타유형
        /// </summary>
        /// <param name="pitDirec"></param>
        /// <param name="pitForm"></param>
        /// <param name="hitDirec"></param>
        /// <returns></returns>
        public static string GetPitBatInfo(object pitDirec, object pitForm, object hitDirec)
        {
            string result = string.Empty;

            result = Baseball.GetPitDirec(pitDirec.ToString()) + Baseball.GetPitForm(pitForm.ToString()) + Baseball.GetHitType(hitDirec.ToString());

            return result;
        }

        /// <summary>
        /// 투수 투구방향 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetPitDirec(string code)
        {
            NameValueCollection nvcPitDirec = new NameValueCollection();

            nvcPitDirec.Add("3", "우");
            nvcPitDirec.Add("4", "좌");
            nvcPitDirec.Add("5", "양");

            return nvcPitDirec.Get(code);
        }

        /// <summary>
        /// 투구폼 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetPitForm(string code)
        {
            NameValueCollection nvcPitForm = new NameValueCollection();

            nvcPitForm.Add("6", "투");   // 오버
            nvcPitForm.Add("7", "언");   // 언더
            nvcPitForm.Add("8", "사");   // 사이드

            return nvcPitForm.Get(code);
        }

        /// <summary>
        /// 타격방향 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetHitType(string code)
        {
            NameValueCollection nvcHitType = new NameValueCollection();

            nvcHitType.Add("9", "우타");
            nvcHitType.Add("10", "좌타");
            nvcHitType.Add("11", "양타");

            return nvcHitType.Get(code);
        }

        /// <summary>
        /// 경기 결과 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetResult(string code)
        {
            NameValueCollection nvcResult = new NameValueCollection();

            nvcResult.Add("W", "승");
            nvcResult.Add("L", "패");
            nvcResult.Add("S", "세");
            nvcResult.Add("H", "홀");
            nvcResult.Add("D", "무");

            return nvcResult.Get(code);
        }

        /// <summary>
        /// 등/말소 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetEntryInOut(string code)
        {
            NameValueCollection nvcInOut = new NameValueCollection();

            nvcInOut.Add("Y", "등록");
            nvcInOut.Add("N", "말소");

            return nvcInOut.Get(code);
        }

        /// <summary>
        /// 백신 접종 여부 체크
        /// </summary>
        /// <param name="pNm">선수 이름</param>
        /// <param name="ReasonSc">이유 SC</param>
        /// <returns>선수이름</returns>
        public static string CheckPlayerVaccineCancel(object pNm, object ReasonSc)
        {
            string userName = pNm.ToString();
            string Reason = ReasonSc.ToString();
            if (Reason == "3") // 백신접종 특별말소(6) -> 코로나19 특별규정(3)
            {
                userName = string.Format("* {0}", userName);
            }
            return userName;
        }

        /// <summary>
        /// 게임상태 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetGameStateSection(string code)
        {
            NameValueCollection nvcGameStateSection = new NameValueCollection();

            nvcGameStateSection.Add("1", "경기전");
            nvcGameStateSection.Add("2", "경기중");
            nvcGameStateSection.Add("3", "경기종료");
            nvcGameStateSection.Add("4", "경기취소");
            nvcGameStateSection.Add("5", "서스펜디드");

            return nvcGameStateSection.Get(code);
        }

        /// <summary>
        /// 퓨처스 그룹 축약명
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>명</returns>
        public static string GetFuturesGroupName(object code)
        {
            NameValueCollection nvcGroup = new NameValueCollection();

            nvcGroup.Add("SOUTH", "남부");
            nvcGroup.Add("NORTH", "북부");

            return nvcGroup.Get(code.ToString());
        }

        /// <summary>
        /// 퓨처스 그룹 전체명
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>명</returns>
        public static string GetFuturesGroupFullName(object code)
        {
            NameValueCollection nvcGroup = new NameValueCollection();

            nvcGroup.Add("SOUTH", "남부리그");
            nvcGroup.Add("NORTH", "북부리그");

            return nvcGroup.Get(code.ToString());
        }

        /// <summary>
        /// 베이스 상태 한글명
        /// </summary>
        /// <param name="baseNum"></param>
        /// <returns>베이스 상태</returns>
        public static string GetBaseState(string code)
        {
            string baseName = "";

            if (code != null && code != string.Empty)
            {
                if (code.Equals("0"))
                {
                    baseName = "주자없음";
                }
                else if (code.Equals("123"))
                {
                    baseName = "만루";
                }
                else
                {
                    if (code.Length == 2)
                    {
                        code = code[0] + "," + code[1];
                    }
                    baseName = code + "루";
                }
            }
            return baseName;
        }
        #endregion

        #region 리스트 (팀, 팀별 선수)
        /// <summary>
        /// 리그/시즌별 팀리스트
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="groupSc">그룹</param>
        public static DataTable SetTeamList(string leagueId, string seasonId, string groupSc)
        {
            DataTable dtTeam = CacheUtil.GetTeam();
            DataView dvTeam = new DataView(dtTeam);
            string addFilter = string.Empty;

            if (leagueId.Equals(KBO_LE_ID.ToString()))
            {
                addFilter = " AND (GROUP_SC IS NULL OR GROUP_SC IN ('DREAM', 'MAGIC'))";  // kbo 1군 팀 리스트
            }

            if (!string.IsNullOrEmpty(groupSc) && groupSc != "NORTH,MID,SOUTH")
            {
                addFilter = addFilter + string.Format(" AND GROUP_SC IN ('{0}')", groupSc);
            }
            else if (groupSc == "NORTH,MID,SOUTH")
            {
                addFilter = addFilter + string.Format(" AND GROUP_SC IN ('{0}', '{1}', '{2}')", groupSc.Split(',')[0], groupSc.Split(',')[1], groupSc.Split(',')[2]);
            }

            dvTeam.RowFilter = string.Format("LE_ID={0} AND SEASON_ID={1}{2}", leagueId, seasonId, addFilter);

            if (string.IsNullOrEmpty(groupSc))
            {
                dvTeam.Sort = "LE_ID, SEASON_ID, RANK_NO ASC";
            }
            else
            {
                dvTeam.Sort = "LE_ID, SEASON_ID, GROUP_SC, GROUP_CHANGE_CK, RANK_NO ASC";
            }

            return dvTeam.ToTable();
        }

        /// <summary>
        /// 리그/시즌/팀별 선수리스트
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="groupSc">그룹</param>
        public static DataTable SetPlayerList(string leagueId, string seasonId, string teamId, string sectionCode)
        {
            DataTable dtPlayer = CacheUtil.GetPlayer();
            DataView dvPlayer = new DataView(dtPlayer);

            string addFilter = string.Empty;

            if (sectionCode != "0")
            {
                addFilter = string.Format("AND SECTION_CD={0}", sectionCode);
            }

            dvPlayer.RowFilter = string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}' {3}", leagueId, seasonId, teamId, addFilter);
            dvPlayer.Sort = "SECTION_CD, P_NM ASC";
            return dvPlayer.ToTable();
        }

        /// <summary>
        /// 퓨처스리그 탭
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="groupSc">그룹</param>
        public static DataTable SetFuturesTab(string leagueId, string seasonId, string groupSc)
        {
            DataTable dtTeam = CacheUtil.GetFuturesTab();
            DataView dvTeam = new DataView(dtTeam);
            string addFilter = string.Empty;

            if (leagueId.Equals(KBO_LE_ID.ToString()))
            {
                addFilter = " AND (GROUP_SC IS NULL OR GROUP_SC IN ('DREAM', 'MAGIC'))";  // kbo 1군 팀 리스트
            }

            if (!string.IsNullOrEmpty(groupSc) && groupSc != "NORTH,MID,SOUTH")
            {
                addFilter = addFilter + string.Format(" AND GROUP_SC IN ('{0}')", groupSc);
            }
            else if (groupSc == "NORTH,MID,SOUTH")
            {
                addFilter = addFilter + string.Format(" AND GROUP_SC IN ('{0}', '{1}', '{2}')", groupSc.Split(',')[0], groupSc.Split(',')[1], groupSc.Split(',')[2]);
            }

            dvTeam.RowFilter = string.Format("LE_ID={0} AND SEASON_ID={1}{2}", leagueId, seasonId, addFilter);

            if (string.IsNullOrEmpty(groupSc))
            {
                dvTeam.Sort = "LE_ID, SEASON_ID, RANK_NO ASC";
            }
            else
            {
                dvTeam.Sort = "LE_ID, SEASON_ID, GROUP_SC, GROUP_CHANGE_CK, RANK_NO ASC";
            }

            return dvTeam.ToTable();
        }
        #endregion

        #region 기록 계산 공식(선수)

        /// <summary>
        /// inn2 이닝을 실제 이닝으로 (3 1/3) 계산
        /// </summary>
        /// <param name="inn2">이닝*3</param>
        /// <returns>실제이닝</returns>
        public static string ConvertInn(object inn2)
        {
            string result = "";

            if (!string.IsNullOrEmpty(inn2.ToString()))
            {
                int inn = int.Parse(inn2.ToString());
                int quotient = inn / 3;   // 몫
                int rest = inn % 3;   // 나머지

                if (inn == 0)
                {
                    result = "0";
                }
                else if (quotient == 0)
                {
                    result = string.Format("{0}/3", rest.ToString());
                }
                else
                {
                    if (rest == 0)
                    {
                        result = quotient.ToString();
                    }
                    else
                    {
                        result = string.Format("{0} {1}/3", quotient.ToString(), rest.ToString());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// inn2 이닝을 실제 이닝으로 (3 1/3) 계산
        /// </summary>
        /// <param name="inn2">이닝*3</param>
        /// <returns>실제이닝</returns>
        public static string ConvertInnComma(object inn2)
        {
            string result = "";

            if (!string.IsNullOrEmpty(inn2.ToString()))
            {
                int inn = int.Parse(inn2.ToString());
                int quotient = inn / 3;   // 몫
                int rest = inn % 3;   // 나머지

                if (inn == 0)
                {
                    result = "0";
                }
                else if (quotient == 0)
                {
                    result = string.Format("{0}/3", rest.ToString());
                }
                else
                {
                    if (rest == 0)
                    {
                        result = string.Format("{0}", string.Format("{0:#,###}", quotient));
                        //result = quotient.ToString();
                    }
                    else
                    {
                        result = string.Format("{0} {1}/3", string.Format("{0:#,###}", quotient), rest.ToString());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// inn2 이닝을 실제 이닝으로 (3.1) 계산
        /// </summary>
        /// <param name="inn2">이닝*3</param>
        /// <returns>실제이닝</returns>
        public static string ConvertInnToDecimal(object inn2)
        {
            string result = "";

            if (!string.IsNullOrEmpty(inn2.ToString()))
            {
                int inn = int.Parse(inn2.ToString());
                int quotient = inn / 3;   // 몫
                int rest = inn % 3;   // 나머지

                if (quotient == 0)
                {
                    result = string.Format("0.{0}", rest.ToString());
                }
                else
                {
                    if (rest == 0)
                    {
                        result = string.Format("{0}.0", quotient.ToString());
                    }
                    else
                    {
                        result = string.Format("{0}.{1}", quotient.ToString(), rest.ToString());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 타율 (HRA_RT) 계산
        /// </summary>
        public static string GetHraRateToZero(string columnAb, string columnHit)
        {
            string rateHra = "0.000";

            if (columnAb != "0")
            {
                rateHra = Round((float.Parse(columnHit) / float.Parse(columnAb)), 3, columnAb);
            }
            else
            {
                rateHra = "-";
            }

            return rateHra;
        }

        /// <summary>
        /// 방어율 (ERA_RT) 계산
        /// </summary>
        public static string GetEraRateToZero(string columnInn2, string columnEr)
        {
            string rateEra = "0.00";

            if (columnInn2 != "0")
            {
                rateEra = Round(((float.Parse(columnEr) * 27) / float.Parse(columnInn2)), 2, columnInn2);
            }
            else
            {
                rateEra = "-";
            }

            return rateEra;
        }

        /// <summary>
        /// 장타율 (SLG_RT) 계산
        /// </summary>
        public static string GetSlgRateToZero(string columnAb, string columnHit, string columnH2, string columnH3, string columnHr)
        {
            string rateSlg = "0.000";

            if (columnAb != "0")
            {
                rateSlg = Round(((float.Parse(columnHit) + float.Parse(columnH2) + (float.Parse(columnH3) * 2) + (float.Parse(columnHr) * 3)) / float.Parse(columnAb)), 3);
            }
            else
            {
                rateSlg = "-";
            }

            return rateSlg;
        }

        /// <summary>
        /// 출루율 (OBP_RT) 계산
        /// </summary>
        public static string GetObpRateToZero(string columnAb, string columnHit, string columnBb, string columnHp, string columnSf)
        {
            string rateObp = "0.000";

            if (float.Parse(columnAb) + float.Parse(columnBb) + float.Parse(columnSf) + float.Parse(columnHp) != 0)
            {
                rateObp = Round(((float.Parse(columnBb) + float.Parse(columnHit) + float.Parse(columnHp)) / (float.Parse(columnAb) + float.Parse(columnBb) + float.Parse(columnSf) + float.Parse(columnHp))), 3, (float.Parse(columnAb) + float.Parse(columnBb) + float.Parse(columnSf) + float.Parse(columnHp)));
            }
            else
            {
                rateObp = "-";
            }

            return rateObp;
        }

        /// <summary>
        /// 이닝당 출루 허용율 (WHIP_RT) 계산
        /// </summary>
        public static string GetWhipRateToZero(string columnInn2, string columnHit, string columnBb)
        {
            string rateWhip = "0.00";

            if (float.Parse(columnInn2) != 0)
            {
                rateWhip = Round((((float.Parse(columnHit) + float.Parse(columnBb)) * 3) / float.Parse(columnInn2)), 2, columnInn2);
            }
            else
            {
                rateWhip = "-";
            }

            return rateWhip;
        }
        #endregion

        #region 기록 계산 공식(합계)
        /// <summary>
        /// 타율 (HRA_RT) 계산(합계)
        /// </summary>
        public static string GetHraRate(DataTable dtTemp, string columnAb, string columnHit, string filterAb, string filterHit)
        {
            decimal sumAb = decimal.Parse(Sum(dtTemp, columnAb, filterAb));
            decimal sumHit = decimal.Parse(Sum(dtTemp, columnHit, filterHit));
            string rateHra = "0.000";

            if (sumAb != 0)
            {
                rateHra = Round((sumHit / sumAb), 3, sumAb);
            }
            else
            {
                rateHra = "-";
            }

            return rateHra;
        }

        /// <summary>
        /// 방어율 (ERA_RT) 계산(합계)
        /// </summary>
        public static string GetEraRate(DataTable dtTemp, string columnInn2, string columnEr, string filterInn2, string filterEr)
        {
            decimal sumInn2 = decimal.Parse(Sum(dtTemp, columnInn2, filterInn2));
            decimal sumEr = decimal.Parse(Sum(dtTemp, columnEr, filterEr));
            string rateEra = "0.00";

            if (sumInn2 != 0)
            {
                rateEra = Round(((sumEr * 27) / sumInn2), 2, sumInn2);
            }
            else
            {
                rateEra = "-";
            }

            return rateEra;
        }

        /// <summary>
        /// 장타율 (SLG_RT) 계산(합계)
        /// </summary>
        public static string GetSlgRate(DataTable dtTemp, string columnAb, string columnHit, string columnH2, string columnH3, string columnHr, string filterAb, string filterHit, string filterH2, string filterH3, string filterHr)
        {
            decimal sumHit = decimal.Parse(Sum(dtTemp, columnHit, filterHit));
            decimal sumH2 = decimal.Parse(Sum(dtTemp, columnH2, filterH2));
            decimal sumH3 = decimal.Parse(Sum(dtTemp, columnH3, filterH3));
            decimal sumHr = decimal.Parse(Sum(dtTemp, columnHr, filterHr));
            decimal sumAb = decimal.Parse(Sum(dtTemp, columnAb, filterAb));
            string rateSlg = "0.000";

            if (sumAb != 0)
            {
                rateSlg = Round(((sumHit + sumH2 + (sumH3 * 2) + (sumHr * 3)) / sumAb), 3);
            }
            else
            {
                rateSlg = "-";
            }

            return rateSlg;
        }

        /// <summary>
        /// 출루율 (OBP_RT) 계산(합계)
        /// </summary>
        public static string GetObpRate(DataTable dtTemp, string columnAb, string columnHit, string columnHr, string columnBb, string columnHp, string columnSf, string filterAb, string filterHit, string filterHr, string filterBb, string filterHp, string filterSf)
        {
            decimal sumHit = decimal.Parse(Sum(dtTemp, columnHit, filterHit));
            decimal sumHr = decimal.Parse(Sum(dtTemp, columnHr, filterHr));
            decimal sumAb = decimal.Parse(Sum(dtTemp, columnAb, filterAb));
            decimal sumBb = decimal.Parse(Sum(dtTemp, columnBb, filterBb));
            decimal sumHp = decimal.Parse(Sum(dtTemp, columnHp, filterHp));
            decimal sumSf = decimal.Parse(Sum(dtTemp, columnSf, filterSf));
            string rateObp = "0.000";

            if (sumAb + sumBb + sumSf + sumHp != 0)
            {
                rateObp = Round(((sumBb + sumHit + sumHp) / (sumAb + sumBb + sumSf + sumHp)), 3, (sumAb + sumBb + sumSf + sumHp));
            }
            else
            {
                rateObp = "-";
            }

            return rateObp;
        }

        /// <summary>
        /// 이닝당 출루 허용율 (WHIP_RT) 계산(합계)
        /// </summary>
        public static string GetWhipRate(DataTable dtTemp, string columnInn2, string columnHit, string columnBb, string filterInn2, string filterHit, string filterBb)
        {
            decimal sumInn2 = decimal.Parse(Sum(dtTemp, columnInn2, filterInn2));
            decimal sumHit = decimal.Parse(Sum(dtTemp, columnHit, filterHit));
            decimal sumBb = decimal.Parse(Sum(dtTemp, columnBb, filterBb));
            string rateWhip = "0.000";

            if (sumInn2 != 0)
            {
                rateWhip = Round((((sumHit + sumBb) * 3) / sumInn2), 2, sumInn2);
            }
            else
            {
                rateWhip = "-";
            }

            return rateWhip;
        }

        /// <summary>
        /// 승률 (WRA_RT) 계산(합계)
        /// </summary>
        public static string GetWraRate(DataTable dtTemp, string columnWin, string columnLose, string filterWin, string filterLose)
        {
            decimal sumWin = decimal.Parse(Sum(dtTemp, columnWin, filterWin));
            decimal sumLose = decimal.Parse(Sum(dtTemp, columnLose, filterLose));
            string rateWra = "0.000";

            if (sumWin + sumLose != 0)
            {
                rateWra = Round((sumWin / (sumWin + sumLose)), 3, (sumWin + sumLose));
            }
            else
            {
                rateWra = "-";
            }

            return rateWra;
        }

        /// <summary>
        /// 시즌별 승률 (WRA_RT) 계산
        /// </summary>
        public static string GetSeasonWraRate(object objSeason, object objWin, object objLose, object objDraw, object objGame)
        {
            int season = int.Parse(objSeason.ToString());
            float win = float.Parse(objWin.ToString());
            float lose = float.Parse(objLose.ToString());
            float draw = float.Parse(objDraw.ToString());
            float game = float.Parse(objGame.ToString());

            string rateWra = "0.000";

            int caseSc = 0;

            if ((season >= 1982 && season <= 1986) || (season >= 1998 && season <= 2002) || (season >= 2005 && season <= 2008) || (season >= 2011 && season <= 2016))
            {
                caseSc = 1; // 승/승+패
            }
            else if (season >= 1987 && season <= 1997)
            {
                caseSc = 2; // 승+(무x0.5)/경기수
            }
            else if (season >= 2009 && season <= 2010)
            {
                caseSc = 3; // 승/경기수
            }
            else if (season >= 2003 && season <= 2004)
            {
                //caseSc = 4; // 다승제 승차
                caseSc = 1; // 김인성대리 요청
            }

            switch (caseSc)
            {
                case 1:
                    rateWra = Round((win / (win + lose)), 3, (win + lose));
                    break;
                case 2:
                    rateWra = Round(((win + (draw * 0.5)) / game), 3, game);
                    break;
                case 3:
                    rateWra = Round((win / game), 3, game);
                    break;
                case 4:
                    rateWra = "-";
                    break;
                default:
                    rateWra = "-";
                    break;
            }

            return rateWra;
        }
        #endregion

        #region 반올림, count, sum, avg, min, max
        /// <summary>
        /// 지정된 소수 자릿수로 반올림
        /// </summary>
        /// <param name="data">데이터</param>
        /// <param name="decimals">반환값의 소수 자릿수</param>
        /// <returns>digits와 일치하는 소수 자릿수가 들어 있는 data에 가장 가까운 수입니다. </returns>
        public static string Round(object data, int digits)
        {
            Decimal result = 0;

            if (data.ToString().Trim() == "")
            {
                data = 0;
            }
            else
            {
                result = Math.Round(Convert.ToDecimal(data), digits, MidpointRounding.AwayFromZero);
            }

            return ConvertFormat(result, digits);
        }

        /// <summary>
        /// 지정된 소수 자릿수로 반올림
        /// </summary>
        /// <param name="data">데이터</param>
        /// <param name="decimals">반환값의 소수 자릿수</param>
        /// <returns>digits와 일치하는 소수 자릿수가 들어 있는 data에 가장 가까운 수입니다. </returns>
        public static string Round(object data, int digits, object checkZeroData)
        {
            Decimal result = 0;

            if (string.IsNullOrEmpty(checkZeroData.ToString()) || (checkZeroData.ToString() == "0")) // checkZeroData = 1 일때 : 분모 구하기 어려운경우
            {
                return "-";
            }
            else
            {
                if (data.ToString().Trim() == "")
                {
                    data = 0;
                }
                else
                {
                    result = Math.Round(Convert.ToDecimal(data), digits, MidpointRounding.AwayFromZero);
                }
                return ConvertFormat(result, digits);
            }
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

            switch(digits)
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

        public static string ConvertThousandComma(string data)
        {
            string ret = "";

            if (data == null || data.Equals("") || data == "NaN")
                return ret;

            if (data.Length > 10)
            {
                data = data.Substring(0, 10);
            }

            ret = string.Format("{0:#,###}", Convert.ToDecimal(data));

            return ret;
        }

        /// <summary>
        /// 데이터 갯수
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>데이터 갯수</returns>
        public static string Count(DataTable dt, string colName, string filter)
        {
            object count = dt.Compute(string.Format("Count([{0}])", colName), filter);

            return count.ToString();
        }

        /// <summary>
        /// 데이터 합계
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>합계</returns>
        public static string Sum(DataTable dt, string colName, string filter)
        {
            object sum = dt.Compute(string.Format("Sum([{0}])", colName), filter);

            return sum.ToString();
        }

        /// <summary>
        /// 데이터 평균
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>평균</returns>
        public static string Avg(DataTable dt, string colName, string filter)
        {
            object avg = dt.Compute(string.Format("Avg([{0}])", colName), filter);

            return avg.ToString();
        }

        /// <summary>
        /// 데이터 최소값
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>최소값</returns>
        public static string Min(DataTable dt, string colName, string filter)
        {
            object min = dt.Compute(string.Format("Min([{0}])", colName), filter);

            return min.ToString();
        }

        /// <summary>
        /// 데이터 최대값
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>최대값</returns>
        public static string Max(DataTable dt, string colName, string filter)
        {
            object max = dt.Compute(string.Format("Max([{0}])", colName), filter);

            return max.ToString();
        }
        #endregion

        #region 이벤트
        /// <summary>
        /// KBO리그발전포럼 아이디 -> 이름
        /// </summary>
        public static string GetKboForumName(string seasonId, string forumNo)
        {
            string[,] forum = { { "2015", "6", "MLB의 성장전략과 리그 비전" }, { "2015", "7", "스포츠 마케팅의 제왕, NFL" }, { "2015", "1", "스포츠산업 진흥법 활용하기" }, { "2015", "2", "KBO 리그 광고현황과 개선방안" }, { "2015", "3", "2015 KBO 리그 이슈 점검" }, { "2015", "4", "퓨처스리그, 어떻게 성장시켜야 하는가?" }, { "2015", "5", "유소년 야구선수의 부상이야기" } };

            string result = string.Empty;

            for (int i = 0; i < forum.GetLength(0); i++)
            {
                if (seasonId.Equals(forum[i, 0]) && forumNo.Equals(forum[i, 1]))
                {
                    result = forum[i, 2];
                    break;
                }
            }

            return result;
        }
        #endregion

        #region 영문홈페이지

        /// <summary>
        /// 1군 팀명 -> 영문팀명
        /// </summary>
        /// <param name="name">팀명</param>
        /// <returns>팀명</returns>
        public static string GetEngTeamName(object leagueId, object seasonId, string teamNm)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetTeam();
            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND FIRST_NM='{2}'", leagueId.ToString(), seasonId.ToString(), teamNm.ToString()));

            if (drTeam.Length > 0)
            {
                teamName = string.Format("{0} {1}", drTeam[0]["FIRST_ENG_NM"].ToString(), drTeam[0]["LAST_ENG_NM"].ToString());                
            }

            return teamName;
        }

        /// <summary>
        /// 1군 팀코드 -> 영문팀명
        /// </summary>
        /// <param name="code">팀명</param>
        /// <returns>팀명</returns>
        public static string GetEngTeamCode(object leagueId, object seasonId, object teamId)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetTeam();
            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString()));

            if (drTeam.Length > 0)
            {
                teamName = string.Format("{0}", drTeam[0]["FIRST_ENG_NM"].ToString());

                if (string.IsNullOrEmpty(teamName))
                {
                    teamName = drTeam[0]["LAST_ENG_NM"].ToString();
                }

                if (teamName.ToLower() == "kt")
                {
                    teamName = "KT";
                }
            }

            return teamName;
        }

        /// <summary>
        /// 리그/시즌별 팀 ID -> 팀명 (ex, SAMSUNG LIONS)
        /// </summary>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="teamId">팀 ID</param>
        /// <returns>팀명</returns>
        public static string GetEngTeamFullName(object leagueId, object seasonId, object teamId)
        {
            string teamName = "";
            DataTable dtTeam = CacheUtil.GetTeam();
            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leagueId.ToString(), seasonId.ToString(), teamId.ToString()));

            if (drTeam.Length > 0)
            {
                teamName = string.Format("{0} {1}", drTeam[0]["FIRST_ENG_NM"].ToString(), drTeam[0]["LAST_ENG_NM"].ToString());
            }

            return teamName;
        }

        /// <summary>
        /// 구장 -> ENG 구장명칭
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="stadiumId">구장ID</param>
        /// <returns>구장명</returns>
        public static string GetEngStadiumName(object seasonId, object stadiumId)
        {
            string engStadium = "";
            DataTable dtStadium = CacheUtil.GetStadium();
            DataRow[] drStadium = dtStadium.Select(string.Format("SEASON_ID={0} AND S_ID='{1}'", seasonId, stadiumId));

            if (drStadium.Length > 0)
            {
                engStadium = drStadium[0]["S_ENG_NM"].ToString();
            }
           
            return engStadium;
        }

        /// <summary>
        /// 구장 -> ENG 구장명칭
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="stadiumNm">구장Name</param>
        /// <returns>구장명</returns>
        public static string GetEngStadiumNm(object seasonId, object stadiumNm)
        {
            string engStadium = "";
            DataTable dtStadium = CacheUtil.GetStadium();
            DataRow[] drStadium = dtStadium.Select(string.Format("SEASON_ID={0} AND S_NM='{1}'", seasonId, stadiumNm.ToString()));

            if (drStadium.Length > 0)
            {
                engStadium = drStadium[0]["S_ENG_NM"].ToString();
            }

            return engStadium;
        }

        /// <summary>
        /// 포지션 아이디 -> 영문 이름
        /// </summary>
        /// <param name="posNo">포지션 번호</param>
        /// <returns>포지션영문 이름</returns>
        public static string GetEngPositionName(string data)
        {
            string result = string.Empty;

            if (data == "1")
                result = "Pitcher";
            else if (data == "2")
                result = "Catcher";
            else if (data == "3" || data == "4" || data == "5" || data == "6")
                result = "Infielder";
            else
                result = "Outfielder";

            return result;
        }

        /// <summary>
        /// inn2 이닝을 실제 이닝으로 (3 1/3) 계산
        /// </summary>
        /// <param name="shotName">포지션</param>
        /// <returns>영문 포지션</returns>
        public static string PositionName(string shotPosition)
        {
            string result = shotPosition;

            for (int i = 0; i < position.GetLength(0); i++)
            {
                if (shotPosition.Equals(position[i, 0]))
                {
                    result = position[i, 1];
                    break;
                }
            }

            return result;
        }

        #endregion

        #region 관리자
        /// <summary>
        /// 더블헤더 리턴
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetDoubleHeadName(string code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("0", "더블헤더 아님");
            nvcTb.Add("1", "더블헤더 1차전");
            nvcTb.Add("2", "더블헤더 2차전");

            return nvcTb.Get(code);
        }

        #endregion

        #region 포스트시즌
        #region 포스트시즌 > 정규시즌 / 와일드카드 디폴트 값 구하기
        /// <summary>
        /// 포스트시즌 > 정규시즌 / 와일드카드 디폴트 값 구하기
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static int GetTodayGameSeries()
        {
            int srId = Baseball.WILDCARD_SR_ID;
            string gameDate = DateTime.Now.ToShortDateString();

            DataSet dsSeries = CacheUtil.GetMainTodayGameSeries(Baseball.KBO_LE_ID, gameDate);
            DataRow[] drSeries = dsSeries.Tables[0].Select();

            srId = int.Parse(drSeries[0]["SR_ID"].ToString());

            if (srId == Baseball.REGULAR_SR_ID)
            {
                srId = Baseball.WILDCARD_SR_ID;
            }

            return srId;
        }
        #endregion

        #region 포스트시즌 > 시리즈 아이디 -> 번호
        /// <summary>
        /// 포스트시즌 > 시리즈 아이디 -> 번호
        /// </summary>
        /// <param name="code">코드</param>
        /// <returns>번호</returns>
        public static string GetPostSeasonSrIdToNo(object code)
        {
            NameValueCollection nvcGroup = new NameValueCollection();

            nvcGroup.Add("4", "1");
            nvcGroup.Add("3", "2");
            nvcGroup.Add("5", "3");
            nvcGroup.Add("7", "4");

            return nvcGroup.Get(code.ToString());
        }
        #endregion
        #endregion

        #region 기타 공통 함수
        public static int GetStrikeLimit(int cnt)
        {
            if (cnt > 2)
            {
                cnt = 2;
            }

            return cnt;
        }

        public static int GetBallLimit(int cnt)
        {
            if (cnt > 3)
            {
                cnt = 3;
            }

            return cnt;
        }

        public static int GetOutLimit(int cnt)
        {
            if (cnt > 2)
            {
                cnt = 2;
            }

            return cnt;
        }
        #endregion
    }
}