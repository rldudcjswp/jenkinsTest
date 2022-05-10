using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Collections.Specialized;
using KBOLib.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace KBOLib.Util
{
    /// <summary>
    /// GetSearch 메서드에서 사용할 열거형 값
    /// </summary>
    public enum SearchType
    {
        seasonId,
        seriesId, // 시범경기, 정규경기, 포스트시즌
        seriesId2, // 시범경기, 정규경기
        teamId,
        pos,
        situation,
        situationDetail
    }

    public class WebUtil
    {
        private static Database kboDB2 = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_db2");
        private const string RECORD_SEARCH_KEY = "RECORD_SEARCH";    // 기록실 검색 조건

        #region 리스트 (시즌, 리그, 팀, 팀별 선수)
        /// <summary>
        /// 시즌 리스트
        /// </summary>
        /// <param name="ddlYear">DropDownList</param>
        /// <param name="startYear">시작 년도</param>
        public static void SetSeasonList(DropDownList ddlYear, int startYear)
        {
            ddlYear.Items.Clear();

            for (int i = startYear; i <= DateTime.Now.Year; i++)
            {
                ddlYear.Items.Add(new ListItem("" + i));
            }
        }

        /// <summary>
        /// 시리즈(정규,시범,올스타,포스트 등등) 리스트
        /// </summary>
        /// <param name="ddlSeries">DropDownList</param>
        public static void SetSeriesList(DropDownList ddlSeries)
        {
            string[,] leagueArray = { { "KBO 정규시즌", "0" }, { "KBO 시범경기", "1" }, { "KBO 와일드카드", "4" }, { "KBO 준플레이오프", "3" }, { "KBO 플레이오프", "5" }, { "KBO 한국시리즈", "7" } };
            ddlSeries.Items.Clear();            

            for (int i = 0; i < leagueArray.GetLength(0); i++)
            {
                ListItem item = new ListItem(leagueArray[i, 0], leagueArray[i, 1]);

                ddlSeries.Items.Add(item);
            }
        }

        /// <summary>
        /// 퓨처스 그룹 리스트
        /// </summary>
        /// <param name="ddlSeries">DropDownList</param>
        public static void SetFuturesGroupList(DropDownList ddlGroup, int seasonId)
        {
            string[,] leagueArray = { { "북부리그", "NORTH" }, { "남부리그", "SOUTH" } };

            if (seasonId == 2015)
            {
                leagueArray = new string[,] { { "북부리그", "NORTH" }, { "중부리그", "MID" }, { "남부리그", "SOUTH" } };
            }

            ddlGroup.Items.Clear();

            for (int i = 0; i < leagueArray.GetLength(0); i++)
            {
                ListItem item = new ListItem(leagueArray[i, 0], leagueArray[i, 1]);

                ddlGroup.Items.Add(item);
            }
        }

        /// <summary>
        /// 포지션 구분 리스트
        /// </summary>
        /// <param name="ddlPos">DropDownList</param>
        public static void SetPosList(DropDownList ddlPos)
        {
            string[,] posArray = { { "포수", "2" }, { "내야수", "3,4,5,6" }, { "외야수", "7,8,9" }, { "투수", "1" } };
            ddlPos.Items.Clear();
            ddlPos.Items.Add(new ListItem("포지션 선택", ""));

            for (int i = 0; i < posArray.GetLength(0); i++)
            {
                ddlPos.Items.Add(new ListItem(posArray[i, 0], posArray[i, 1]));
            }
        }

        /// <summary>
        /// 경기상황 상세 리스트
        /// </summary>
        /// <param name="ddlSituationDetail"></param>
        public static void SetSitutationDetailList(DropDownList ddlSituation, string[,] situationArray, string title)
        {
            ddlSituation.ClearSelection();
            ddlSituation.Items.Clear();

            if (!string.IsNullOrEmpty(title))
            {
                ddlSituation.Items.Add(new ListItem(title, ""));
            }

            if (situationArray != null)
            {
                for (int i = 0; i < situationArray.GetLength(0); i++)
                {
                    ddlSituation.Items.Add(new ListItem(situationArray[i, 0], situationArray[i, 1]));
                }
            }
        }

        /// <summary>
        /// 월별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetMonthList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "3~4월", "3,4" }, { "5월", "5" }, { "6월", "6" }, { "7월", "7" }, { "8월", "8" }, { "9월 이상", "9,10,11,12" } };

            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 요일별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetWeekList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "화", "화요일" }, { "수", "수요일" }, { "목", "목요일" }, { "금", "금요일" }, { "토", "토요일" }, { "일", "일요일" }, { "월", "월요일" } };

            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 요일별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetWeekNoList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "화", "2" }, { "수", "3" }, { "목", "4" }, { "금", "5" }, { "토", "6" }, { "일", "7" }, { "월", "1" } };

            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 시즌별 구장 리스트
        /// </summary>
        /// <param name="ddlStadium">DropDownList</param>
        /// <param name="seasonId">시즌</param>
        public static void SetStadiumList(DropDownList ddlStadium, string seasonId, string title, int leId)
        {
            DataTable dtStadium = CacheUtil.GetGameStadium();
            DataRow[] drStadium = dtStadium.Select(string.Format("SEASON_ID={0} AND LE_ID={1}", seasonId, leId), "SEASON_ID, ROW_NO");

            ddlStadium.Items.Clear();

            if (!string.IsNullOrEmpty(title))
            {
                ddlStadium.Items.Add(new ListItem(title, ""));
            }

            foreach (var team in drStadium)
            {
                ListItem item = new ListItem(team["S_NM"].ToString(), team["S_ID"].ToString());

                if (!ddlStadium.Items.Contains(item))
                {
                    ddlStadium.Items.Add(item);
                }
            }

            // 2017-04-28 yeeun, 김지나 실장 요청, 2군 경기일정에 문학 구장 추가요청
            if (dtStadium.Select("LE_ID=2 AND SEASON_ID=2017 AND S_NM='문학'").Length == 0)
            {
                ddlStadium.Items.Add(new ListItem("문학", "MH"));
            }
        }

        /// <summary>
        /// 홈/방문별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetHomeAwayList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "홈", "B" }, { "방문", "T" } };
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 주/야간별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetDayNightList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "주간", "D" }, { "야간", "N" } };
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 전/후반기별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetHalfList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "전반기", "F" }, { "후반기", "S" } };
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 투수유형별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetPitTypeList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "좌투수", "LO" }, { "우투수", "RO" }, { "언더", "LU,RU" }};
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 타자유형별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetHitTypeList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "우타자", "R" }, { "좌타자", "L" } };
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 주자상황별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetBaseList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "주자없음", "0" }, { "주자있음", "1,2,3,12,13,23,123" }, { "득점권", "2,3,12,13,23,123" }, { "1루", "1" }, { "2루", "2" }
                                     ,{"3루","3"},{"1,2루","12"},{"1,3루","13"},{"2,3루","23"},{"만루","123"}};
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 볼카운트별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetBallCountList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "0-0", "0-0" }, { "1-0", "0-1" }, { "2-0", "0-2" }, { "3-0", "0-3" }, { "0-1", "1-0" }
                                     , { "1-1", "1-1" }, { "2-1", "1-2" }, { "3-1", "1-3" }, { "0-2", "2-0" }, { "1-2", "2-1" }
                                     , { "2-2", "2-2" }, { "3-2", "2-3" } };
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 아웃카운트별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetOutCountList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "0아웃", "0" }, { "1아웃", "1" }, { "2아웃", "2" } };
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 이닝별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetInnList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "1회", "1" }, { "2회", "2" }, { "3회", "3" }, { "4회", "4" }, { "5회", "5" }
                                 , { "6회", "6" }, { "7회", "7" }, { "8회", "8" }, { "9회", "9" }, { "연장", "10,11,12,13,15,16,17,18" }
                                 , { "1~3회", "1,2,3" }, { "4~6회", "4,5,6" }, { "7회이후", "7,8,9,10,11,12,13,14,15,16,17,18" }};
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 타순별 리스트
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetBatOrderList(DropDownList ddlSituation, string title)
        {
            string[,] situationArray = { { "1번", "1" }, { "2번", "2" }, { "3번", "3" }, { "4번", "4" }, { "5번", "5" }
                                      , { "6번", "6" }, { "7번", "7" }, { "8번", "8" }, { "9번", "9" }, { "상위(1~2번)", "1,2" }
                                      , { "중심(3~5번)", "3,4,5" }, { "하위(6~9번)", "6,7,8,9" }};
            SetSitutationDetailList(ddlSituation, situationArray, title);
        }

        /// <summary>
        /// 리그/시즌별 팀리스트
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        public static void SetTeamList(DropDownList ddlTeam, string leagueId, string seasonId, string title)
        {
            DataTable dtTeam = CacheUtil.GetTeam();
            string addFilter = string.Empty;

            if (leagueId.Equals(Baseball.KBO_LE_ID.ToString()))
            {
                addFilter = " AND (GROUP_SC IS NULL OR GROUP_SC IN ('DREAM', 'MAGIC'))";  // kbo 1군 팀 리스트
            }

            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1}{2}", leagueId, seasonId, addFilter), "LE_ID, SEASON_ID, ROW_NO");

            ddlTeam.Items.Clear();

            if (!string.IsNullOrEmpty(title))
            {
                ddlTeam.Items.Add(new ListItem(title, ""));
            }

            foreach (var team in drTeam)
            {
                ListItem item = new ListItem();

                if (string.IsNullOrEmpty(team["FIRST_NM"].ToString())) // 2009년 히어로즈
                {
                    item = new ListItem(team["LAST_NM"].ToString(), team["T_ID"].ToString());
                }
                else
                {
                    item = new ListItem(team["FIRST_NM"].ToString(), team["T_ID"].ToString());
                }

                if (!ddlTeam.Items.Contains(item))
                {
                    ddlTeam.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 리그/시즌별 팀리스트 - 경력관리
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        public static void SetCareerTeamList(DropDownList ddlTeam, string leagueId, string seasonId, string title, string pageGubun)
        {
            DataTable dtTeam = CacheUtil.GetCareerTeam();
            string addFilter = string.Empty;

            if (leagueId.Equals(Baseball.KBO_LE_ID.ToString()))
            {
                addFilter = " AND (GROUP_SC IS NULL OR GROUP_SC IN ('DREAM', 'MAGIC'))";  // kbo 1군 팀 리스트
            }

            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1}{2}", leagueId, seasonId, addFilter), "LE_ID, SEASON_ID, ROW_NO");

            ddlTeam.Items.Clear();

            if (!string.IsNullOrEmpty(title))
            {
                ddlTeam.Items.Add(new ListItem(title, ""));
            }

            foreach (var team in drTeam)
            {
                ListItem item = new ListItem();

                if (string.IsNullOrEmpty(team["FIRST_NM"].ToString())) // 2009년 히어로즈
                {
                    item = new ListItem(team["LAST_NM"].ToString(), team["T_ID"].ToString());
                }
                else if (pageGubun == "draft" && team["SEASON_ID"].ToString() == "1999" && team["T_ID"].ToString() == "OB" && team["FIRST_NM"].ToString() == "두산") // 지명에서만
                {
                    item = new ListItem("OB", team["T_ID"].ToString());
                }
                else if (pageGubun == "draft" && team["SEASON_ID"].ToString() == "2001" && team["T_ID"].ToString() == "HT" && team["FIRST_NM"].ToString() == "KIA") // 지명에서만
                {
                    item = new ListItem("해태", team["T_ID"].ToString());
                }
                else
                {
                    item = new ListItem(team["FIRST_NM"].ToString(), team["T_ID"].ToString());
                }

                if (!ddlTeam.Items.Contains(item))
                {
                    ddlTeam.Items.Add(item);
                }
            }

            // 2016-01-27 yeeun, 유재연사원 요청, 상무,경찰 코치 등록위해 추가
            if (leagueId == Baseball.KBO_LE_ID.ToString() && pageGubun == "career")
            {
                ddlTeam.Items.Add(new ListItem("경찰", "PL"));
                ddlTeam.Items.Add(new ListItem("상무", "SM"));
            }
        }

        /// <summary>
        /// 리그/시즌/그룹별 팀리스트
        /// </summary>
        /// <param name="ddlTeam">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="groupSc">그룹(SOUTH,NORTH)</param>
        public static void SetTeamList(DropDownList ddlTeam, string leagueId, string seasonId, string groupSc, string title)
        {
            DataTable dtTeam = CacheUtil.GetTeam();
            string addFilter = string.IsNullOrEmpty(groupSc) ? " AND GROUP_SC <> 'ALLSTAR' AND GROUP_SC <> 'INDIE'" : string.Format(" AND GROUP_SC='{0}'", groupSc);

            DataRow[] drTeam = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1}{2}", leagueId, seasonId, addFilter), "LE_ID, SEASON_ID, GROUP_SC, RANK_NO");

            ddlTeam.Items.Clear();

            if (!string.IsNullOrEmpty(title))
            {
                ddlTeam.Items.Add(new ListItem(title, ""));
            }

            foreach (var team in drTeam)
            {
                ListItem item = new ListItem(team["FIRST_NM"].ToString(), team["T_ID"].ToString());

                if (!ddlTeam.Items.Contains(item))
                {
                    ddlTeam.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 리그/시즌/팀별 선수 리스트
        /// </summary>
        /// <param name="ddlPlayer">DropDownList</param>
        /// <param name="leagueId">리그 ID</param>
        /// <param name="seasonId">시즌 ID</param>
        /// <param name="teamId">팀 ID</param>
        public static void SetPlayerList(DropDownList ddlPlayer, string leagueId, string seasonId, string teamId, string title, string position)
        {
            DataTable dtPlayer = CacheUtil.GetPlayer();

            string addFilter = string.Empty;

            if (position.Equals("PIT"))
            {
                addFilter = " AND POS_NO = 1";
            }
            else
            {
                addFilter = " AND POS_NO <> 1";
            }

            DataRow[] drPlayer = dtPlayer.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND SECTION_CD=12 AND T_ID='{2}'{3}", leagueId, seasonId, teamId, addFilter), "P_NM");

            ddlPlayer.Items.Clear();

            if (!string.IsNullOrEmpty(title))
            {
                ddlPlayer.Items.Add(new ListItem(title, "0"));
            }

            foreach (var player in drPlayer)
            {
                ListItem item = new ListItem(player["P_NM"].ToString(), player["P_ID"].ToString());

                if (!ddlPlayer.Items.Contains(item))
                {
                    ddlPlayer.Items.Add(item);
                }
            }
        }
        #endregion

        #region 기록실 검색 조건 처리
        #region 검색 조건 값 초기화
        /// <summary>
        /// 검색 조건 값 초기화
        /// </summary>
        public static void InitSearch()
        {
            if (HttpContext.Current.Session[RECORD_SEARCH_KEY] == null)
            {
                ClearSearch();
            }
        }
        #endregion

        #region 검색 조건 초기화
        /// <summary>
        /// 검색 조건 초기화
        /// </summary>
        public static void ClearSearch()
        {
            HttpContext.Current.Session[RECORD_SEARCH_KEY] = null;

            Search searchInfo = new Search();
            searchInfo.seasonId = Baseball.KBO_END_YEAR.ToString();
            //searchInfo.seriesId = GetTodayRecordSeries().ToString(); // 2017-10-05 yeeun, 포스트시즌 시리즈별 디폴트 변경 Baseball.REGULAR_SR_ID.ToString();
            searchInfo.seriesId = Baseball.REGULAR_SR_ID.ToString(); // // 2020.10.14 revenge 포스트시즌위해 주석처리
            //searchInfo.seriesId = Baseball.EXHIBITION_SR_ID.ToString(); // 시범경기
            //searchInfo.seriesId2 = Baseball.REGULAR_SR_ID.ToString();
            searchInfo.seriesId2 = Baseball.EXHIBITION_SR_ID.ToString();
            searchInfo.teamId = "";
            searchInfo.pos = "";
            searchInfo.situation = "";
            searchInfo.situationDetail = "";

            HttpContext.Current.Session[RECORD_SEARCH_KEY] = searchInfo;
        }
        #endregion

        #region 검색 조건 세션값 변경
        /// <summary>
        /// 검색 조건 세션값 변경
        /// </summary>
        /// <param name="searchType">type</param>
        /// <param name="searchData">값</param>
        public static void SetSearch(SearchType searchType, string searchData)
        {
            Search searchInfo = (Search)HttpContext.Current.Session[RECORD_SEARCH_KEY];

            switch (searchType)
            {
                case SearchType.seasonId:
                    searchInfo.seasonId = searchData;
                    break;
                case SearchType.seriesId:
                    searchInfo.seriesId = searchData;
                    break;
                case SearchType.seriesId2:
                    searchInfo.seriesId2 = searchData;
                    break;
                case SearchType.teamId:
                    searchInfo.teamId = searchData;
                    break;
                case SearchType.pos:
                    searchInfo.pos = searchData;
                    break;
                case SearchType.situation:
                    searchInfo.situation = searchData;
                    break;
                case SearchType.situationDetail:
                    searchInfo.situationDetail = searchData;
                    break;
            }

            HttpContext.Current.Session[RECORD_SEARCH_KEY] = searchInfo;
        }
        #endregion

        #region 검색 조건 별 값 추출
        /// <summary>
        /// 검색 조건 별 값 추출
        /// </summary>
        /// <param name="searchType">type</param>
        /// <returns>조건 값</returns>
        public static string GetSearch(SearchType searchType)
        {
            string result = "";

            if (HttpContext.Current.Session[RECORD_SEARCH_KEY] != null)
            {
                Search search = (Search)HttpContext.Current.Session[RECORD_SEARCH_KEY];

                switch (searchType)
                {
                    case SearchType.seasonId:
                        result = search.seasonId;
                        break;
                    case SearchType.seriesId:
                        result = search.seriesId;
                        break;
                    case SearchType.seriesId2:
                        result = search.seriesId2;
                        break;
                    case SearchType.teamId:
                        result = search.teamId;
                        break;
                    case SearchType.pos:
                        result = search.pos;
                        break;
                    case SearchType.situation:
                        result = search.situation;
                        break;
                    case SearchType.situationDetail:
                        result = search.situationDetail;
                        break;
                }
            }

            return result;
        }
        #endregion

        #region 경기상황별 검색 조건별 표출 페이지 결정 (GAME: 경기별, SITUATION: 상황별)
        /// <summary>
        /// 경기상황별 검색 조건별 표출 페이지 결정 (GAME: 경기별, SITUATION: 상황별)
        /// </summary>
        /// <param name="section"></param>
        /// <returns>GAME: 경기별, SITUATION: 상황별</returns>
        public static string ConvertSection(string section)
        {
            string result = "GAME";

            switch (section)
            {
                case "MONTH_SC":
                    result = "GAME";
                    break;
                case "WEEK_SC":
                    result = "GAME";
                    break;
                case "STADIUM_SC":
                    result = "GAME";
                    break;
                case "HOMEAYAY_SC":
                    result = "GAME";
                    break;
                case "OPPTEAM_SC":
                    result = "GAME";
                    break;
                case "DAYNIGHT_SC":
                    result = "GAME";
                    break;
                case "HALF_SC":
                    result = "GAME";
                    break;
                case "41":
                    result = "SITUATION";
                    break;
                case "42":
                    result = "SITUATION";
                    break;
                case "43":
                    result = "SITUATION";
                    break;
                case "44":
                    result = "SITUATION";
                    break;
                case "45":
                    result = "SITUATION";
                    break;
                case "46":
                    result = "SITUATION";
                    break;
                case "47":
                    result = "SITUATION";
                    break;
            }

            return result;
        }
        #endregion

        #region 포스트시즌 > 정규시즌 / 와일드카드 디폴트 값 구하기
        /// <summary>
        /// 포스트시즌 > 정규시즌 / 와일드카드 디폴트 값 구하기
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static int GetTodayRecordSeries()
        {
            string gameDate = DateTime.Now.ToShortDateString();

            DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_PLAYER_PITCHER_SERIES_S");
            kboDB2.AddInParameter(cmd, "LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
            kboDB2.AddInParameter(cmd, "G_DT", DbType.String, gameDate);

            DataSet dsData = kboDB2.ExecuteDataSet(cmd);
            DataRow[] drData = dsData.Tables[0].Select();

            int srId = 0;

            if (drData.Length > 0)
            {
                srId = int.Parse(drData[0]["SR_ID"].ToString());
            }

            return srId;
        }
        #endregion

        public static int GetKboStartYear(string seriesId, bool isDetail)
        {
            int season = Baseball.KBO_START_YEAR;
            string series = Baseball.REGULAR_SR_ID.ToString();

            if (!string.IsNullOrEmpty(seriesId))
            {
                series = seriesId;
            }

            if (isDetail)
            {
                season = Baseball.KBO_DETAIL_START_YEAR;
            }
            else
            {
                if (series.Equals(Baseball.REGULAR_SR_ID.ToString()))
                {
                    season = Baseball.KBO_START_YEAR;
                }
                else if (series.Equals(Baseball.EXHIBITION_SR_ID.ToString()))
                {
                    season = Baseball.KBO_EXHIBITION_START_YEAR;
                }
                else if (series.Equals(Baseball.POST_SR_ID))
                {
                    season = Baseball.KBO_POST_START_YEAR;
                }
            }

            return season;
        }

        public static string GetKboSeriesYear(string seriesId, string select, bool isDetail)
        {
            string season = Baseball.KBO_END_YEAR.ToString();
            string series = Baseball.REGULAR_SR_ID.ToString();
            int selectSeason = int.Parse(select);

            if (!string.IsNullOrEmpty(seriesId))
            {
                series = seriesId;
            }

            if (isDetail)
            {
                if (selectSeason >= Baseball.KBO_DETAIL_START_YEAR)
                {
                    season = Baseball.KBO_DETAIL_START_YEAR.ToString();
                }
            }
            else
            {
                if (series.Equals(Baseball.REGULAR_SR_ID.ToString()))
                {
                    if (selectSeason == 9999)
                    {

                    }
                    else if (selectSeason >= Baseball.KBO_START_YEAR)
                    {
                        season = selectSeason.ToString();
                    }
                }
                else if (series.Equals(Baseball.EXHIBITION_SR_ID.ToString()))
                {
                    if (selectSeason >= Baseball.KBO_EXHIBITION_START_YEAR)
                    {
                        season = selectSeason.ToString();
                    }
                }
                //else if (series.Equals(Baseball.POST_SR_ID))
                else if (series.Equals(Baseball.WILDCARD_SR_ID.ToString()) || series.Equals(Baseball.PLAYOFFS_SR_ID.ToString()) || series.Equals(Baseball.SEMI_PLAYOFFS_SR_ID.ToString()) || series.Equals(Baseball.KOREAN_SR_ID.ToString())) // 2018-01-15 yeeun, 수정
                {
                    if (selectSeason >= Baseball.KBO_POST_START_YEAR)
                    {
                        season = selectSeason.ToString();
                    }
                }
            }

            return season;
        }
        #endregion

        #region 뉴스, 게시판
        public static string GetNewsSectionName(string scId)
        {
            NameValueCollection nvcNews = new NameValueCollection();

            nvcNews.Add("1", "프리뷰");
            nvcNews.Add("2", "경기속보");
            nvcNews.Add("3", "스타인터뷰");
            nvcNews.Add("4", "WBC속보");
            nvcNews.Add("9", "총재기사/컬럼");
            nvcNews.Add("10", "기록위원회");
            nvcNews.Add("11", "NC소식");
            nvcNews.Add("12", "올스타소식");
            nvcNews.Add("13", "아시아뉴스");
            nvcNews.Add("14", "인포그래픽");
            nvcNews.Add("15", "웹툰");
            nvcNews.Add("16", "경기스케치");
            nvcNews.Add("17", "WBC히스토리");
            nvcNews.Add("18", "퓨처스샷");
            nvcNews.Add("19", "퓨처스타그램");

            return nvcNews.Get(scId);
        }

        public static string GetNewsListUrl(string scId)
        {
            NameValueCollection nvcNews = new NameValueCollection();

            nvcNews.Add("1", "/News/Preview/List.aspx");
            nvcNews.Add("2", "/News/BreakingNews/List.aspx");
            nvcNews.Add("3", "/News/Interview/List.aspx");
            //nvcNews.Add("4", "/News/Preview/List.aspx");
            //nvcNews.Add("9", "/News/Preview/List.aspx");

            return nvcNews.Get(scId);
        }

        public static string GetNewsViewUrl(string scId)
        {
            NameValueCollection nvcNews = new NameValueCollection();

            nvcNews.Add("1", "/News/Preview/View.aspx");
            nvcNews.Add("2", "/News/BreakingNews/View.aspx");
            nvcNews.Add("3", "/News/Interview/View.aspx");
            //nvcNews.Add("4", "/News/Preview/List.aspx");
            //nvcNews.Add("9", "/News/Preview/List.aspx");

            return nvcNews.Get(scId);
        }
        #endregion

        #region 관리자

        /// <summary>
        /// 더블헤더
        /// </summary>
        /// <param name="ddlSituation">DropDownList</param>
        public static void SetdoubleHeadList(DropDownList ddlDoubleHead, string title)
        {
            string[,] doubleArray = { { "더블헤더 아님", "0" }, { "더블헤더 1차전", "1" }, { "더블헤더 2차전", "2" } };

            SetSitutationDetailList(ddlDoubleHead, doubleArray, title);
        }
        #endregion

        #region 모바일
        /// <summary>
        /// 뉴스 상세 페이지
        /// </summary>
        /// <param name="bdSc"></param>
        /// <returns></returns>
        public static string GetNewsPage(object bdSc, object bdSe)
        {
            string result = string.Empty;

            switch (bdSc.ToString())
            {
                case "1":
                    result = "/News/Preview/View.aspx?bdSe=" + bdSe;
                    break;
                case "2":
                    result = "/News/BreakingNews/View.aspx?bdSe=" + bdSe;
                    break;
                case "3":
                    result = "/News/Interview/View.aspx?bdSe=" + bdSe;
                    break;
                case "23":
                    result = "/News/CardNews/View.aspx?bdSe=" + bdSe;
                    break;
            }

            return result;
        }
        #endregion

        #region 선수사진
        /// <summary>
        /// 연도별 선수사진 매칭
        /// </summary>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        public static string GetPlayerImgSeasonFolder(object seasonId)
        {
            string result = string.Empty;

            if (Baseball.KBO_END_YEAR  == 2017)
            {
                if (int.Parse(seasonId.ToString()) <= 2015)
                {
                    result = Baseball.KBO_END_YEAR.ToString();
                }
                else
                {
                    result = seasonId.ToString();
                }
            }
            else
            {
                if (int.Parse(seasonId.ToString()) >= Baseball.KBO_END_YEAR)    // 요청:홍지희 사원, 20190228:: 선수사진 ex)2019년 시즌전일때, 2019년 1월에 2018년도 선수사진을 2019년도에 옮긴다. 이후 선수등록시 2019년도 선수이미지가 표출됨
                {  
                    result = seasonId.ToString();
                }
                else if (int.Parse(seasonId.ToString()) <= Baseball.KBO_END_YEAR && int.Parse(seasonId.ToString()) >= Baseball.KBO_END_YEAR - 2)
                {
                    result = seasonId.ToString();
                }
                else
                {
                    result = Baseball.KBO_END_YEAR.ToString();
                }
            }

            return result;
        }
        #endregion

        #region 연도별 팀 엠블럼 매칭
        /// <summary>
        /// 연도별 팀 엠블럼 매칭
        /// </summary>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        public static string GetTeamEmblemSeasonFolder(object srId, object seasonId, object imgSc)
        {
            string result = string.Empty;

            switch (int.Parse(srId.ToString()))
            {
                case 0: // 정규 시리즈
                case 1: // 시범 시리즈
                case 4: // 와일드카드결정전 시리즈
                case 3: // 준플레이오프 시리즈
                case 5: // 플레이오프 시리즈
                case 7: // 한국 시리즈
                case 10: // 교류 시리즈
                case 6: // 연습경기
                    if (int.Parse(seasonId.ToString()) == 0)
                    {
                        result = string.Format("regular/{0}", DateTime.Now.Year);
                    }
                    else if (int.Parse(seasonId.ToString()) <= 2018)
                    {
                        result = "regular/2018";
                    }
                    else
                    {
                        result = string.Format("regular/{0}", seasonId);
                    }
                    break;
                case 8: // 국제 시리즈
                    result = "international";
                    break;
                case 9: // 올스타 시리즈
                    result = string.Format("allstar/{0}", seasonId);
                    break;
            }

            result += string.Format("/{0}", imgSc);

            // 이쪽에 나중에 시즌중 엠블럼 변경된 경우 예외처리 해주세요, 날짜 넘겨받는 부분 추가해야함, 언제 처리할지 몰라 날짜 넘겨받는 부분은 안함
            //if (imgSc.Contains("HT"))
            //{
            //if (DateUtil.IsCompareDate("특정날짜", gameDate))
            //{
            //    result += "_1";
            //}
            //else
            //{
            //    result += "_2";
            //}
            //}

            return result;
        }

        /// <summary>
        /// 연도별 팀 엠블럼 매칭
        /// </summary>
        /// <param name="seasonId"></param>
        /// <returns></returns>
        public static string GetTeamEmblemSeasonFolder(object srId, object seasonId)
        {
            string result = string.Empty;

            switch (int.Parse(srId.ToString()))
            {
                case 0: // 정규 시리즈
                case 1: // 시범 시리즈
                case 4: // 와일드카드결정전 시리즈
                case 3: // 준플레이오프 시리즈
                case 5: // 플레이오프 시리즈
                case 7: // 한국 시리즈
                    if (int.Parse(seasonId.ToString()) == 0)
                    {
                        result = string.Format("regular/{0}", DateTime.Now.Year);
                    }
                    else if (int.Parse(seasonId.ToString()) <= 2018)
                    {
                        result = "regular/2018";
                    }
                    else
                    {
                        result = string.Format("regular/{0}", seasonId);
                    }
                    break;
                case 8: // 국제 시리즈
                    result = "international";
                    break;
                case 9: // 올스타 시리즈
                    result = string.Format("allstar/{0}", seasonId);
                    break;
            }

            //result += string.Format("/{0}", imgSc);

            // 이쪽에 나중에 시즌중 엠블럼 변경된 경우 예외처리 해주세요, 날짜 넘겨받는 부분 추가해야함, 언제 처리할지 몰라 날짜 넘겨받는 부분은 안함
            //if (imgSc.Contains("HT"))
            //{
            //if (DateUtil.IsCompareDate("특정날짜", gameDate))
            //{
            //    result += "_1";
            //}
            //else
            //{
            //    result += "_2";
            //}
            //}

            return result;
        }
        #endregion
    }
}