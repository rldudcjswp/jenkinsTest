using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Specialized;
using System.Configuration;
// TODO: Use Enterprise Library Data Block
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Newtonsoft.Json.Linq;

namespace KBOLib.Util
{
    public class CacheUtil
    {
        private static Database webDB = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_web");
        private static Database kboDB2 = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_db2");

        #region 구분(news, pressbox, qna, faq, contacus, game, cancel) key
        private const string SECTION_NEWS_KEY = "SECTION_NEWS_KEY"; // news 구분
        private const string SECTION_PRESSBOX_KEY = "SECTION_PRESSBOX_KEY"; // pres
        private const string SECTION_QNA_KEY = "SECTION_QNA_KEY";
        private const string SECTION_FAQ_KEY = "SECTION_FAQ_KEY";
        private const string SECTION_INQUIRY_KEY = "SECTION_INQUIRY_KEY";
        private const string SECTION_CONTACTUS_KEY = "SECTION_CONTACTUS_KEY";
        
        private const string SECTION_GAME_KEY = "SECTION_GAME_KEY"; // 경기 구분
        private const string SECTION_CANCEL_KEY = "SECTION_CANCEL_KEY"; // 경기 취소 구분
        #endregion

        #region 공통 (방송사 리스트)
        private const string BROADCAST_KEY = "BROADCAST_KEY";   // 라디오, TV 방송국
        #endregion

        #region KBO 마스터(리그, 시리즈, 구장, 팀, 선수) key
        private const string LEAGUE_KEY = "LEAGUE_KEY"; // 리그
        private const string SERIES_KEY = "SERIES_KEY"; // 시리즈
        private const string STADIUM_KEY = "STADIUM_KEY";   // 구장
        private const string GAME_STADIUM_KEY = "GAME_STADIUM_KEY";   // 1군 경기 구장
        private const string TEAM_KEY = "TEAM_KEY"; // 팀
        private const string CAREER_TEAM_KEY = "CAREER_TEAM_KEY"; // 팀 (경력관리용)
        private const string PLAYER_KEY = "PLAYER_KEY"; // 선수
        private const string FUTURES_TAB_KEY = "FUTURES_TAB_KEY";   // 퓨처스리그 탭
        #endregion

        #region 문자 중계(스코어보드, 타구장, 수비/주자 정보, 라인업, 라이브텍스트, 선수, 박스스코어) key
        private const string GAMECAST_SCOREBOARD_KEY = "GAMECAST_SCOREBOARD_KEY";   // 스코어보드
        private const string GAMECAST_OTHER_GAME_KEY = "GAMECAST_OTHER_GAME_KEY";   // 타구장소식
        private const string GAMECAST_GROUND_INFO_KEY = "GAMECAST_GROUND_INFO_KEY"; // 경기장 수비/주자 정보
        private const string GAMECAST_LINEUP_INFO_KEY = "GAMECAST_LINEUP_INFO_KEY"; // 라인업정보  
        private const string GAMECAST_LIVETEXT_KEY = "GAMECAST_LIVETEXT_KEY";   // 라이브텍스트
        private const string GAMECAST_PLAYER_INFO_KEY = "GAMECAST_PLAYER_INFO_KEY"; // 선수정보
        private const string GAMECAST_BOXSCORE_KEY = "GAMECAST_BOXSCORE_KEY";   // 박스스코어
        #endregion

        #region 메인(뉴스, 공지사항, 프리뷰, 스타인터뷰, 예상달성기록, vod, 팀순위, 다승, 평균자책, 탈삼진, 타율, 홈런, 타점, 앨범, 1군 경기일정, 2군 경기일정, 중계일정, 하이라이트) key
        private const string MAIN_NEWS_KEY = "MAIN_NEWS_KEY";   // 메인 뉴스
        private const string MAIN_NOTICE_KEY = "MAIN_NOTICE_KEY";   // 공지사항
        private const string MAIN_BREAKING_NEWS_KEY = "MAIN_BREAKING_NEWS_KEY"; // 속보
        private const string MAIN_PREVIEW_KEY = "MAIN_PREVIEW_KEY"; // 프리뷰
        private const string MAIN_INTERVIEW_KEY = "MAIN_INTERVIEW_KEY"; // 스타인터뷰
        private const string MAIN_EXPECTATION_WEEKLY_KEY = "MAIN_EXPECTATION_WEEKLY_KEY"; // 주간 예상달성기록
        private const string MAIN_EXPECTATION_DAILY_KEY = "MAIN_EXPECTATION_DAILY_KEY"; // 일간 예상달성기록
        private const string MAIN_VIDEO_NEWS_KEY = "MAIN_VIDEO_NEWS_KEY"; // 영상뉴스
        private const string MAIN_VOD_KEY = "MAIN_VOD_KEY"; // VOD
        private const string MAIN_TEAMRANK_KEY = "MAIN_TEAMRANK_KEY";   // 팀순위
        private const string MAIN_PITCHER_TOP5_ERA_RT_KEY = "MAIN_PITCHER_TOP5_ERA_RT_KEY";   // 평균자책 5
        private const string MAIN_PITCHER_TOP5_W_CN_KEY = "MAIN_PITCHER_TOP5_W_CN_KEY";   // 다승 5
        private const string MAIN_PITCHER_TOP5_SV_CN_KEY = "MAIN_PITCHER_TOP5_SV_CN_KEY";   // 세이브 5
        private const string MAIN_PITCHER_TOP5_INN2_CN_KEY = "MAIN_PITCHER_TOP5_INN2_CN_KEY"; // 이닝 5
        private const string MAIN_PITCHER_TOP5_KK_CN_KEY = "MAIN_PITCHER_TOP5_KK_KEY"; // 탈삼진 5
        private const string MAIN_PITCHER_TOP5_HOLD_CN_KEY = "MAIN_PITCHER_TOP5_HOLD_CN_KEY"; // 홀드 5
        private const string MAIN_HITTER_TOP5_HRA_RT_KEY = "MAIN_HITTER_TOP5_HRA_RT_KEY"; // 타율 5
        private const string MAIN_HITTER_TOP5_RUN_CN_KEY = "MAIN_HITTER_TOP5_RUN_CN_KEY";   // 득점 5
        private const string MAIN_HITTER_TOP5_HR_CN_KEY = "MAIN_HITTER_TOP5_HR_CN_KEY";   // 홈런 5
        private const string MAIN_HITTER_TOP5_RBI_CN_KEY = "MAIN_HITTER_TOP5_RBI_CN_KEY"; // 타점 5
        private const string MAIN_HITTER_TOP5_SB_CN_KEY = "MAIN_HITTER_TOP5_SB_CN_KEY"; // 도루 5
        private const string MAIN_ALBUM_KEY = "MAIN_ALBUM_KEY"; // 앨범
        private const string MAIN_TODAY_GAME_SERIES_KEY = "MAIN_TODAY_GAME_SERIES_KEY";   // 1군 경기일정 (시범경기 제외), 오늘날짜(제일 빠른 경기날짜) 가져오기
        private const string MAIN_SCHEDULE_KEY = "MAIN_SCHEDULE_KEY";   // 1군 경기일정 (시범경기 제외)
        private const string MAIN_BROADCAST_SCHEDULE_KEY = "MAIN_BROADCAST_SCHEDULE_KEY";   // 1군 경기일정 (시범경기 제외)
        
        private const string MAIN_EXHIBITION_SCHEDULE_KEY = "MAIN_EXHIBITION_SCHEDULE_KEY";   // 1군 시범경기 경기일정
        private const string MAIN_FUTURES_SCHEDULE_KEY = "MAIN_FUTURES_SCHEDULE_KEY";   // 퓨처스 경기일정
        private const string MAIN_KBO_TV_KEY = "MAIN_KBO_TV_KEY";   // 중계일정
        private const string MAIN_HIGHLIGHTS_KEY = "MAIN_HIGHLIGHTS_KEY";   // 하이라이트

        private const string MAIN_KBO_POSTSEASON_KEY = "MAIN_POSTSEASON_KEY"; // 포스트시즌 게임스케쥴
        private const string MAIN_POSTSEASON_SEED_KEY = "MAIN_POSTSEASON_SEED_KEY"; // 포스트시즌 대진표 정보

        /* 팀순위 */
        private const string GAMEKBO_LASTDATE_KEY = "GAMEKBO_LASTDATE_KEY"; // 마지막 날짜
        #endregion

        #region 팀순위 key
        private const string YEAR_CROWD_KEY = "MAIN_YEAR_CROWD_KEY";   // 년도별 누적관중수
        #endregion

        #region 기록실(투수 TOP5, 타자 TOP5) key
        private const string KBO_PITCHER_TOP5_KEY = "KBO_PITCHER_TOP5_KEY"; // 투수 TOP 5
        private const string KBO_HITTER_TOP5_KEY = "KBO_HITTER_TOP5_KEY";   // 타자 TOP 5
        #endregion

        #region 선수 상세 기록실 key
        private const string KBO_PITCHER_YEAR_TOP10_KEY = "KBO_PITCHER_YEAR_TOP10_KEY"; // 투수 연도별 TOP 10
        private const string KBO_HITTER_YEAR_TOP10_KEY = "KBO_HITTER_YEAR_TOP10_KEY";   // 타자 연도별 TOP 10
        #endregion

        #region 퓨처스기록실(투수 TOP5, 타자 TOP5) key
        private const string FUTURES_PITCHER_TOP5_KEY = "FUTURES_PITCHER_TOP5_KEY"; // 퓨처스 투수 TOP 5
        private const string FUTURES_HITTER_TOP5_KEY = "FUTURES_HITTER_TOP5_KEY";   // 퓨처스 타자 TOP 5
        #endregion

        #region 역대기록실
        private const string HISTORY_TOP10_KEY = "HISTORY_TOP10_KEY";   // 역대 최고기록 10걸 섹션
        private const string HISTORY_HITTER_TOP10_KEY = "HISTORY_HITTER_TOP10_KEY";   // 역대 최고기록 타자 10걸
        private const string HISTORY_PITCHER_TOP10_KEY = "HISTORY_PITCHER_TOP10_KEY";   // 역대 최고기록 투수 10걸
        private const string HISTORY_SEASON_TEAM_RECORD_KEY = "HISTORY_SEASON_TEAM_RECORD_KEY";   // 역대 구단성적
        private const string HISTORY_HITTER_RECORD_KEY = "HISTORY_HITTER_RECORD_KEY";   // 역대 타자
        private const string HISTORY_PITCHER_RECORD_KEY = "HISTORY_PITCHER_RECORD_KEY";   // 역대 투수
        private const string HISTORY_PLAYER_PRIZE_KEY = "HISTORY_PLAYER_PRIZE_KEY";   // 역대 개인수상
        private const string HISTORY_GOLDEN_GLOVE_KEY = "HISTORY_GOLDEN_GLOVE_KEY";   // 역대 골든글러브
        #endregion

        #region 기록실 메인 (투타 순위)
        private const string STATS_MAIN_KEY = "STATS_MAIN_KEY"; // 기록실 투타 순위
        private const string STATS_FUTURES_MAIN_KEY = "STATS_FUTURES_MAIN_KEY"; // 퓨처스 기록실 투타 순위
        #endregion

        #region News(공지사항, 속보, 프리뷰, 스타인터뷰, 앨범) key
        private const string BOARD_NOTICE_KEY = "BOARD_NOTICE_KEY"; // 공지
        private const string BOARD_BREAKING_NEWS_KEY = "BOARD_BREAKING_NEWS_KEY";   // 속보
        private const string BOARD_PREVIEW_KEY = "BOARD_PREVIEW_KEY";   // 프리뷰
        private const string BOARD_INTERVIEW_KEY = "BOARD_INTERVIEW_KEY";   // 스타인터뷰
        private const string BOARD_ALBUM_KEY = "BOARD_ALBUM_KEY";   // 앨범
        #endregion

        #region KBO TV (하이라이트) key
        private const string KBO_TV_HIGHLIGHT_KEY = "KBO_TV_HIGHLIGHT_KEY"; // 하이라이트
        #endregion

        #region 경기일정/결과 (경기상황 상세) key
        private const string SCHEDULE_GAME_SITUATIONDEATIL_KEY = "SCHEDULE_GAME_SITUATIONDEATIL_KEY"; // 경기상황 상세
        #endregion

        #region 선수등록현황
        private const string ROSTER_LASTDATE_KEY = "ROSTER_LASTDATE_KEY"; // 엔트리 자료 있는 최종일
        #endregion

        #region 선수검색
        //private const string PLAYER_SEARCH_KEY = "PLAYER_SEARCH_KEY"; // 메인 선수검색
        #endregion

        #region cdDetail
        private const string KBO_CODE_DETAIL_KEY = "KBO_CODE_DETAIL_KEY"; // CD_DETAIL (상세코드)
        #endregion

        /* 우편번호 */
        private const string ZIP_CODE_KEY = "ZIP_CODE_KEY"; // 우편번호

        /* 스케줄 */
        private const string KBO_SEASON_SCHEDULE_KEY = "KBO_SEASON_SCHEDULE_KEY";   // 한시즌 전체 스케줄
        private const string KBO_BOXSCORE_KEY = "KBO_BOXSCORE_KEY"; // 박스스코어

        /*개인통산 기록 */
        private static String[] KBO_HISTORY_PITCHER_KEY = { "", "KBO_HISTORY_PITCHER_KEY_1", "KBO_HISTORY_PITCHER_KEY_2", "KBO_HISTORY_PITCHER_KEY_3", "KBO_HISTORY_PITCHER_KEY_4", "KBO_HISTORY_PITCHER_KEY_5" 
                                                 , "KBO_HISTORY_PITCHER_KEY_6", "KBO_HISTORY_PITCHER_KEY_7", "KBO_HISTORY_PITCHER_KEY_8", "KBO_HISTORY_PITCHER_KEY_9", "KBO_HISTORY_PITCHER_KEY_10"
                                                 , "KBO_HISTORY_PITCHER_KEY_11", "KBO_HISTORY_PITCHER_KEY_12", "KBO_HISTORY_PITCHER_KEY_13", "KBO_HISTORY_PITCHER_KEY_14", "KBO_HISTORY_PITCHER_KEY_15"
                                                 , "KBO_HISTORY_PITCHER_KEY_16", "KBO_HISTORY_PITCHER_KEY_17", "KBO_HISTORY_PITCHER_KEY_18", "KBO_HISTORY_PITCHER_KEY_19", "KBO_HISTORY_PITCHER_KEY_20", "KBO_HISTORY_PITCHER_KEY_21"};

        private static String[] KBO_HISTORY_HITTER_KEY = { "", "KBO_HISTORY_HITTER_KEY_1", "KBO_HISTORY_HITTER_KEY_2", "KBO_HISTORY_HITTER_KEY_3", "KBO_HISTORY_HITTER_KEY_4", "KBO_HISTORY_HITTER_KEY_5" 
                                                 , "KBO_HISTORY_HITTER_KEY_6", "KBO_HISTORY_HITTER_KEY_7", "KBO_HISTORY_HITTER_KEY_8", "KBO_HISTORY_HITTER_KEY_9", "KBO_HISTORY_HITTER_KEY_10"
                                                 , "KBO_HISTORY_HITTER_KEY_11", "KBO_HISTORY_HITTER_KEY_12", "KBO_HISTORY_HITTER_KEY_13", "KBO_HISTORY_HITTER_KEY_14", "KBO_HISTORY_HITTER_KEY_15"
                                                 , "KBO_HISTORY_HITTER_KEY_16", "KBO_HISTORY_HITTER_KEY_17", "KBO_HISTORY_HITTER_KEY_18"};

        /* 이벤트 */
        private const string KBO_EVENT_CROWD_SUM_KEY = "KBO_EVENT_CROWD_SUM_KEY"; // 최종 관중수 맞추기

        #region 경기일정/결과 경기상황 상세
        /// <summary>
        /// 경기일정/결과 경기상황 상세
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetScheduleSituationDetail(string leagueId, string seriesId, string seasonId, string gameId, string inning, string batterId)
        {
            string situationDetailCache = KBO_BOXSCORE_KEY + "_" + leagueId + "_" + seriesId + "_" + seasonId + "_" + gameId + "_" + inning + "_" + batterId;

            DataSet ds = GetDataSet(situationDetailCache);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_RESULT_LIVETEXT_DETAIL_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, Convert.ToInt16(leagueId));
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int16, Convert.ToInt32(seriesId));
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, seasonId);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);
                kboDB2.AddInParameter(cmd, "@INN_NO", DbType.Int16, inning);
                kboDB2.AddInParameter(cmd, "@BAT_P_ID", DbType.Int32, Convert.ToInt32(batterId));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, situationDetailCache, 0, 0, 1, 0);
            }

            return ds;
        }
        #endregion
        
        #region 구분(news, pressbox, qna, faq, contacus, game, cancel) key
        /// <summary>
        /// NEWS 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetNewsSection(string section)
        {
            string key = SECTION_NEWS_KEY + "_" + section;

            DataSet ds = GetDataSet(key);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_SECTION_S");
                webDB.AddInParameter(cmd, "@SECTION", DbType.String, section);

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, key, 0, 1, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// KBO 보도자료 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetPressboxSection()
        {
            DataSet ds = GetDataSet(SECTION_PRESSBOX_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_PRESSBOX_SECTION_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_PRESSBOX_KEY, 0, 1, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// Qna 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetQnaSection()
        {
            DataSet ds = GetDataSet(SECTION_QNA_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_QNA_SECTION_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_QNA_KEY, 0, 1, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// Faq 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetFaqSection()
        {
            DataSet ds = GetDataSet(SECTION_FAQ_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_FAQ_SECTION_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_FAQ_KEY, 0, 1, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 1:1문의하기 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetInquirySection()
        {
            DataSet ds = GetDataSet(SECTION_INQUIRY_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_INQUIRY_SECTION_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_INQUIRY_KEY, 0, 0, 1, 0);
            }

            return ds;
        }

        /// <summary>
        /// Contactus 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetContactusSection()
        {
            DataSet ds = GetDataSet(SECTION_CONTACTUS_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_CONTACTUS_SECTION_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_CONTACTUS_KEY, 0, 1, 0, 0);
            }

            return ds;
        }


        /// <summary>
        /// VOD 구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetVodSection()
        {
            DataSet ds = GetDataSet(MAIN_VOD_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_VOD_SECTION_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_VOD_KEY, 0, 1, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 경기구분 코드
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameSc()
        {
            DataSet ds = GetDataSet(SECTION_GAME_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_KBO_SECTION_S");
                kboDB2.AddInParameter(cmd, "@SC_SC", DbType.String, "GAME");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_GAME_KEY, 0, 5, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 취소 경기 구분
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetCancelSc()
        {
            DataSet ds = GetDataSet(SECTION_CANCEL_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_KBO_SECTION_S");
                kboDB2.AddInParameter(cmd, "@SC_SC", DbType.String, "CANCEL");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, SECTION_CANCEL_KEY, 0, 5, 0, 0);
            }

            return ds;
        }
        #endregion

        #region 마스터성 데이터 (리그, 시리즈, 구장, 팀, 선수)
        /// <summary>
        /// 리그 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetLeague()
        {
            DataSet ds = GetDataSet(LEAGUE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_LEAGUE_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, LEAGUE_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 시리즈 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetSeries()
        {
            DataSet ds = GetDataSet(SERIES_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_SERIES_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, SERIES_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 구장 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetStadium()
        {
            DataSet ds = GetDataSet(STADIUM_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_STADIUM_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, STADIUM_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 1군 경기 구장 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetGameStadium()
        {
            DataSet ds = GetDataSet(GAME_STADIUM_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_STADIUM_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, GAME_STADIUM_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 팀 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetTeam()
        {
            DataSet ds = GetDataSet(TEAM_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TEAM_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, TEAM_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 팀 데이터 캐싱 - 경력관리용
        /// </summary>
        /// <returns>DataTable</returns>
        // 2015-11-18 yeeun 추가, 유재연 사원, 임석 사원 요청
        public static DataTable GetCareerTeam()
        {
            DataSet ds = GetDataSet(CAREER_TEAM_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TEAM_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                DataTable dt = ds.Tables[0];

                string[,] teamArray = { { "1985", "HH", "빙그레", "이글스" }, { "1989", "SB", "쌍방울", "레이더스" }, { "1990", "SB", "쌍방울", "레이더스" }, { "2000", "SB", "쌍방울", "레이더스" }, { "2008", "HD", "현대", "유니콘스" }, { "2011", "NC", "NC", "다이노스" }, { "2012", "NC", "NC", "다이노스" }, { "2013", "KT", "KT", "위즈" }, { "2014", "KT", "KT", "위즈" } };

                DataRow newRow = dt.NewRow();

                for (int i = 0; i < teamArray.GetLength(0); i++)
                {
                    newRow["LE_ID"] = Baseball.KBO_LE_ID;
                    newRow["SEASON_ID"] = teamArray[i, 0];
                    newRow["T_ID"] = teamArray[i, 1];
                    newRow["FIRST_NM"] = teamArray[i, 2];
                    newRow["LAST_NM"] = teamArray[i, 3];

                    dt.Rows.Add(newRow);
                    newRow = dt.NewRow();
                }

                SetDataSet(ds, CAREER_TEAM_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 구장 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetPlayer()
        {
            DataSet ds = GetDataSet(PLAYER_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_PLAYER_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, PLAYER_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 퓨처스리그 탭 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetFuturesTab()
        {
            DataSet ds = GetDataSet(FUTURES_TAB_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TEAM_FUTURES_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, FUTURES_TAB_KEY, 0, 1, 0, 0);
            }

            return ds.Tables[0];
        }
        #endregion

        #region 메인 뉴스, 속보, 프리뷰, 스타인터뷰, KBO 보도자료, 예상달성기록, 하이라이트, 앨범
        /// <summary>
        /// 메인 뉴스 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainNews()
        {
            DataSet ds = GetDataSet(MAIN_NEWS_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_MAIN_ORDER_ETC_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 1);

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_NEWS_KEY, 0, 0, 10, 0);
            }

            return ds;
        }

        /// <summary>
        /// 속보 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainBreakingNews()
        {
            DataSet ds = GetDataSet(MAIN_BREAKING_NEWS_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 2);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_BREAKING_NEWS_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 프리뷰 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPreview()
        {
            DataSet ds = GetDataSet(MAIN_PREVIEW_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 1);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PREVIEW_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 스타인터뷰 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainInterview()
        {
            DataSet ds = GetDataSet(MAIN_INTERVIEW_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 3);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_INTERVIEW_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 메인 보도자료 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainNotice()
        {
            DataSet ds = GetDataSet(MAIN_NOTICE_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NOTICE_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_NOTICE_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 메인 주간 예상달성기록 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainExpectationWeekly()
        {
            DataSet ds = GetDataSet(MAIN_EXPECTATION_WEEKLY_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_PREDICTION_LIST_S_NEW");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_EXPECTATION_WEEKLY_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 메인 일간 예상달성기록 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainExpectationDaily()
        {
            DataSet ds = GetDataSet(MAIN_EXPECTATION_DAILY_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_PREDICTION_LIST_S_NEW");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int32, 2);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_EXPECTATION_DAILY_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 영상뉴스 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainVideoNews()
        {
            DataSet ds = GetDataSet(MAIN_VIDEO_NEWS_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEW_VOD_LIST_S");
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_VIDEO_NEWS_KEY, 0, 0, 10, 10);
            }

            return ds.Tables[1];
        }

        /// <summary>
        /// 메인 하이라이트 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainHighLights()
        {
            DataSet ds = GetDataSet(MAIN_HIGHLIGHTS_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_GAME_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 1);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_HIGHLIGHTS_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[1];
        }
        
        /// <summary>
        /// 메인 엘범 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainAlbum()
        {
            DataSet ds = GetDataSet(MAIN_ALBUM_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_ALBUM_LIST_S");
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 5);
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_ALBUM_KEY, 0, 0, 10, 0);
            }

            return ds.Tables[1];
        }
        #endregion

        #region 메인 팀랭크
        /// <summary>
        /// 메인 팀랭크 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainTeamRank()
        {
            DataSet ds = GetDataSet(MAIN_TEAMRANK_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("KBO_DB2.DBO.PROC_KBO_DB2_MAIN_TEAMRANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int16, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.Int16, Baseball.KBO_END_YEAR);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_TEAMRANK_KEY, 0, 0, 5, 0);
            }

            return ds;
        }
        #endregion
        
        #region 메인 일정 (1군 경기일정, 퓨처스 경기일정, 방송일정)
        /// <summary>
        /// 메인 1군 경기 스케줄 데이터 캐싱 (시범경기 제외)
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainTodayGameSeries(int leagueId, string gameDate)
        {
            string MAIN_TODAY_GAME_SERIES_KEY = "MAIN_TODAY_GAME_SERIES_KEY_" + gameDate;

            DataSet ds = GetDataSet(MAIN_TODAY_GAME_SERIES_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_KBO_SERIES_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.String, gameDate);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_TODAY_GAME_SERIES_KEY, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 메인 1군 경기 스케줄 데이터 캐싱 (시범경기 제외)
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainSchedule(int leagueId, string seriesList, string gameDate)
        {
            DataSet ds = GetDataSet(MAIN_SCHEDULE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_G_DS_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, seriesList);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.Date, DateUtil.ConvertDateType(gameDate));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_SCHEDULE_KEY, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 메인 1군 경기 스케줄 데이터 캐싱 (TV중계일정 에서 호출)
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetBroadCastMainSchedule(int leagueId, string seriesList, string gameDate)
        {
            DataSet ds = GetDataSet(MAIN_BROADCAST_SCHEDULE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_G_DS_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, seriesList);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.Date, DateUtil.ConvertDateType(gameDate));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_BROADCAST_SCHEDULE_KEY, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 메인 1군 시범경기 스케줄 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainExhibitionSchedule(int leagueId, string seriesList, string gameDate)
        {
            DataSet ds = GetDataSet(MAIN_EXHIBITION_SCHEDULE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_G_DS_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, seriesList);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.Date, DateUtil.ConvertDateType(gameDate));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_EXHIBITION_SCHEDULE_KEY, 0, 0, 5, 0);
            }

            return ds;
        }
        
        /// <summary>
        /// 메인 퓨쳐스 경기 스케줄 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainFuturesSchedule(int leagueId, string seriesList, string gameDate)
        {
            DataSet ds = GetDataSet(MAIN_FUTURES_SCHEDULE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_G_DS_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, seriesList);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.Date, DateUtil.ConvertDateType(gameDate));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_FUTURES_SCHEDULE_KEY, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 메인 플레이오프 스케줄 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainPostSchedule(int leagueId, string seriesList, string gameDate)
        {
            string MAIN_POSTSEASON_KEY = MAIN_KBO_POSTSEASON_KEY + seriesList;

            DataSet ds = GetDataSet(MAIN_POSTSEASON_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_G_DS_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, seriesList);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.Date, DateUtil.ConvertDateType(gameDate));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_POSTSEASON_KEY, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 메인 플레이오프 대진표 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainPostSeed(int seriesId, int seasonId)
        {
            DataSet ds = GetDataSet(MAIN_POSTSEASON_SEED_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_POST_SEED_S");
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int32, seriesId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.Int32, seasonId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_POSTSEASON_SEED_KEY, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 메인 중계일정 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetMainTvSchedule(int leagueId, string seriesList, int seasonId, string gameDate)
        {
            DataSet ds = GetDataSet(MAIN_KBO_TV_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_GAME_G_DS_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leagueId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, seriesList);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.Int16, seasonId);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.Date, DateUtil.ConvertDateType(gameDate));

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_KBO_TV_KEY, 0, 0, 5, 0);
            }

            return ds;
        }
        #endregion

        #region 메인 투수 Top 5 (평균자책점, 승리, 세이브, 이닝, 탈삼진)
        /// <summary>
        /// 메인 평균자책점 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPitTop5Era()
        {
            DataSet ds = GetDataSet(MAIN_PITCHER_TOP5_ERA_RT_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_PITCHER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "ERA_RT");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PITCHER_TOP5_ERA_RT_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }
        
        /// <summary>
        /// 메인 다승 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPitTop5Win()
        {
            DataSet ds = GetDataSet(MAIN_PITCHER_TOP5_W_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_PITCHER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "W_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PITCHER_TOP5_W_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 세이브 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPitTop5Save()
        {
            DataSet ds = GetDataSet(MAIN_PITCHER_TOP5_SV_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_PITCHER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "SV_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PITCHER_TOP5_SV_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 이닝 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPitTop5Inn()
        {
            DataSet ds = GetDataSet(MAIN_PITCHER_TOP5_INN2_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_PITCHER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "INN2_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PITCHER_TOP5_INN2_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 탈삼진 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPitTop5Kk()
        {
            DataSet ds = GetDataSet(MAIN_PITCHER_TOP5_KK_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_PITCHER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "KK_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PITCHER_TOP5_KK_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 홀드 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainPitTop5Hold()
        {
            DataSet ds = GetDataSet(MAIN_PITCHER_TOP5_HOLD_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_PITCHER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "HOLD_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_PITCHER_TOP5_HOLD_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }
        #endregion

        #region 메인 타자 Top 5 (타율, 득점, 홈런, 타점, 도루)
        /// <summary>
        /// 메인 타율 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainHitTop5Hra()
        {
            DataSet ds = GetDataSet(MAIN_HITTER_TOP5_HRA_RT_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_HITTER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "HRA_RT");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_HITTER_TOP5_HRA_RT_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 득점 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainHitTop5Run()
        {
            DataSet ds = GetDataSet(MAIN_HITTER_TOP5_RUN_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_HITTER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "RUN_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_HITTER_TOP5_RUN_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 홈런 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainHitTop5Hr()
        {
            DataSet ds = GetDataSet(MAIN_HITTER_TOP5_HR_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_HITTER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "HR_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_HITTER_TOP5_HR_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 타점 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainHitTop5Rbi()
        {
            DataSet ds = GetDataSet(MAIN_HITTER_TOP5_RBI_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_HITTER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "RBI_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_HITTER_TOP5_RBI_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 메인 도루 5 데이터 캐싱
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetMainHitTop5Sb()
        {
            DataSet ds = GetDataSet(MAIN_HITTER_TOP5_SB_CN_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_HITTER_RANK_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmd, "@RECORD_IF", DbType.String, "SB_CN");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, MAIN_HITTER_TOP5_SB_CN_KEY, 0, 0, 5, 0);
            }

            return ds.Tables[0];
        }
        #endregion

        #region 선수 상세 기록
        /// <summary>
        /// 선수 상세 기록 연도별 TOP 10 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetKboPitYearTop10(int playerId)
        {
            string playerKey = KBO_PITCHER_YEAR_TOP10_KEY + playerId.ToString();

            DataSet ds = GetDataSet(playerKey);

            if (ds == null)
            {
                DbCommand cmdTop10 = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_STATS_PERSONAL_PITCHER_BASIC_TOP10_S");
                kboDB2.AddInParameter(cmdTop10, "@LE_ID", DbType.Int32, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmdTop10, "@SR_ID", DbType.Int32, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmdTop10, "@P_ID", DbType.Int32, playerId);
                ds = kboDB2.ExecuteDataSet(cmdTop10);
                AddColumsRecord(ds.Tables[0]);
                SetDataSet(ds, playerKey, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 선수 상세 기록 연도별 TOP 10 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetKboHitYearTop10(int playerId)
        {
            string playerKey = KBO_HITTER_YEAR_TOP10_KEY + playerId.ToString();
            DataSet ds = GetDataSet(playerKey);

            if (ds == null)
            {
                DbCommand cmdTop10 = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_STATS_PERSONAL_HITTER_BASIC_TOP10_S");
                kboDB2.AddInParameter(cmdTop10, "@LE_ID", DbType.Int32, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmdTop10, "@SR_ID", DbType.Int32, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmdTop10, "@P_ID", DbType.Int32, playerId);
                ds = kboDB2.ExecuteDataSet(cmdTop10);
                AddColumsRecord(ds.Tables[0]);
                SetDataSet(ds, playerKey, 5);
            }

            return ds;
        }

        /// <summary>
        /// DataTable에 컬럼 추가
        /// </summary>
        /// <param name="dtTemp">DataTable</param>
        private static void AddColumsRecord(DataTable dtTemp)
        {
            dtTemp.Columns.Add("RECORD_VA", typeof(string));

            foreach (DataRow row in dtTemp.Rows)
            {
                if (row["ITEM_SC"].ToString() == "타율")
                {
                    row["RECORD_VA"] = Baseball.ConvertFormat(decimal.Parse(row["RECORD_CN"].ToString()), 3);
                }
                else if (row["ITEM_SC"].ToString() == "평균자책점")
                {
                    row["RECORD_VA"] = Baseball.ConvertFormat(decimal.Parse(row["RECORD_CN"].ToString()), 2);
                }
                else if (row["ITEM_SC"].ToString() == "이닝")
                {
                    row["RECORD_VA"] = Baseball.ConvertInn(row["RECORD_CN"]);
                }
                else
                {
                    row["RECORD_VA"] = row["RECORD_CN"].ToString();
                }
            }
        }
        #endregion
        
        #region 기록실 TOP5 (투타순위)
        public static DataSet GetStatsMainList(string playerList)
        {
            String CacheKey = STATS_MAIN_KEY + "_" + playerList;
            DataSet ds = GetDataSet(CacheKey);

            WebUtil.InitSearch();

            if (ds == null)
            {
                DbCommand cmdRecord = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_STATS_MAIN_S");
                kboDB2.AddInParameter(cmdRecord, "@LE_ID", DbType.Int32, Baseball.KBO_LE_ID);
                //kboDB2.AddInParameter(cmdRecord, "@SR_ID", DbType.Int32, WebUtil.GetSearch(SearchType.seriesId));   // 시리즈 변경에 따른 기록표출
                kboDB2.AddInParameter(cmdRecord, "@SR_ID", DbType.Int32, Baseball.REGULAR_SR_ID);     // 정규시즌
                //kboDB2.AddInParameter(cmdRecord, "@SR_ID", DbType.Int32, Baseball.EXHIBITION_SR_ID);    // 시범 경기
                kboDB2.AddInParameter(cmdRecord, "@SEASON_ID", DbType.String, Baseball.KBO_END_YEAR);
                kboDB2.AddInParameter(cmdRecord, "@PLAYER_CK_LIST", DbType.String, playerList);

                ds = kboDB2.ExecuteDataSet(cmdRecord);
                SetDataSet(ds, CacheKey, 0, 0, 10, 0);
            }

            return ds;
        }
        #endregion

        #region 퓨처스기록실 TOP5 기록실
        public static DataSet GetStatsFuturesMainList(string groupSection)
        {
            String CacheKey = STATS_FUTURES_MAIN_KEY + "_" + groupSection;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmdRecord = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MINOR_STATS_MAIN_S");
                kboDB2.AddInParameter(cmdRecord, "@LE_ID", DbType.Int32, Baseball.FUTURES_LE_ID);
                kboDB2.AddInParameter(cmdRecord, "@SR_ID", DbType.Int32, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmdRecord, "@SEASON_ID", DbType.Int32, Baseball.FUTURES_END_YEAR);
                kboDB2.AddInParameter(cmdRecord, "@GROUP_SC", DbType.String, groupSection);

                ds = kboDB2.ExecuteDataSet(cmdRecord);
                SetDataSet(ds, CacheKey, 0, 0, 10, 0);
            }

            return ds;
        }
        #endregion

        #region 역대기록실 상세
        /// <summary>
        /// 문자중계 scoreboard
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetHistoryTop10Section(string section)
        {
            String CacheKey = HISTORY_TOP10_KEY + "_" + section;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_RANK_SECTION_S");
                kboDB2.AddInParameter(cmd, "@SC_SC", DbType.String, section);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 10, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대최고기록 타자
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistoryHitterTop(int division)
        {
            String CacheKey = HISTORY_HITTER_TOP10_KEY + "_" + division;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmdHitter = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TOTALRECORD_TOP10_LIST_S");
                kboDB2.AddInParameter(cmdHitter, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmdHitter, "@SC_ID", DbType.Int32, division);

                ds = kboDB2.ExecuteDataSet(cmdHitter);
                SetDataSet(ds, CacheKey, 0, 0, 5, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대최고기록 투수
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistoryPitcherTop(int division)
        {
            String CacheKey = HISTORY_PITCHER_TOP10_KEY + "_" + division;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmdHitter = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TOTALRECORD_TOP10_LIST_S");
                kboDB2.AddInParameter(cmdHitter, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmdHitter, "@SC_ID", DbType.Int32, division);

                ds = kboDB2.ExecuteDataSet(cmdHitter);
                SetDataSet(ds, CacheKey, 0, 0, 30, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대최고기록 타자
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistorySeasonTeamRecord(int startYear, int endYear, string halfSc)
        {
            String CacheKey = HISTORY_SEASON_TEAM_RECORD_KEY + "_" + startYear + "_" + endYear + "_" + halfSc;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_SEASON_TEAM_OLD_LIST_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int32, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@START_YR", DbType.Int32, startYear);
                kboDB2.AddInParameter(cmd, "@END_YR", DbType.Int32, endYear);
                kboDB2.AddInParameter(cmd, "@HALF_SC", DbType.String, halfSc);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 2, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대 타자
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistoryHitterRecord(string prizeSc, string recordSc)
        {
            String CacheKey = HISTORY_HITTER_RECORD_KEY + "_" + recordSc;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmdHitter = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TOTALRECORD_HITTER_LIST_S");
                kboDB2.AddInParameter(cmdHitter, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmdHitter, "@SR_ID", DbType.Int16, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmdHitter, "@PR_SC", DbType.Int32, prizeSc);
                kboDB2.AddInParameter(cmdHitter, "@RECORD_SC", DbType.String, recordSc);

                ds = kboDB2.ExecuteDataSet(cmdHitter);
                SetDataSet(ds, CacheKey, 0, 2, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대 투수
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistoryPitcherRecord(string prizeSc, string recordSc)
        {
            String CacheKey = HISTORY_PITCHER_RECORD_KEY + "_" + recordSc;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmdPitcher = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TOTALRECORD_PITCHER_LIST_S");
                kboDB2.AddInParameter(cmdPitcher, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmdPitcher, "@SR_ID", DbType.Int16, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmdPitcher, "@PR_SC", DbType.Int32, prizeSc);
                kboDB2.AddInParameter(cmdPitcher, "@RECORD_SC", DbType.String, recordSc);

                ds = kboDB2.ExecuteDataSet(cmdPitcher);
                SetDataSet(ds, CacheKey, 0, 2, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대 개인수상
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistoryPlayerPrize()
        {
            DataSet ds = GetDataSet(HISTORY_PLAYER_PRIZE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TOTALRECORD_PLAYER_PRIZE_LIST_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@LAST_SEASON_ID", DbType.Int32, Baseball.KBO_END_YEAR);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, HISTORY_PLAYER_PRIZE_KEY, 0, 2, 0, 0);
            }

            return ds;
        }

        /// <summary>
        /// 역대 골든글러브
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public static DataSet GetHistoryGoldenGlove()
        {
            DataSet ds = GetDataSet(HISTORY_GOLDEN_GLOVE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_TOTALRECORD_GODENGLOVE_LIST_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@LAST_SEASON_ID", DbType.Int32, Baseball.KBO_END_YEAR);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, HISTORY_GOLDEN_GLOVE_KEY, 0, 2, 0, 0);
            }

            return ds;
        }
        #endregion

        #region News 속보, 프리뷰, 스타인터뷰, 공지사항, KBO PHOTO
        /// <summary>
        /// 속보 데이터 캐싱
        /// </summary>
        /// <param name="listCnt">리스트 갯수</param>
        /// <returns>DataSet</returns>
        public static DataSet GetBreakingNewsCache()
        {
            DataSet ds = GetDataSet(BOARD_BREAKING_NEWS_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 2);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 10);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, BOARD_BREAKING_NEWS_KEY, 0, 0, 10, 0);
            }

            return ds;
        }

        /// <summary>
        /// 프리뷰 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetPreviewCache()
        {
            DataSet ds = GetDataSet(BOARD_PREVIEW_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 1);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 10);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, BOARD_PREVIEW_KEY, 0, 0, 10, 0);
            }

            return ds;
        }

        /// <summary>
        /// 스타인터뷰 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetInterviewCache()
        {
            DataSet ds = GetDataSet(BOARD_INTERVIEW_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NEWS_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int16, 3);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 10);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, BOARD_INTERVIEW_KEY, 0, 0, 10, 0);
            }

            return ds;
        }

        /// <summary>
        /// 앨범 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetAlbumCache()
        {
            DataSet ds = GetDataSet(BOARD_ALBUM_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_ALBUM_LIST_S");
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 10);
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, BOARD_ALBUM_KEY, 0, 0, 10, 0);
            }

            return ds;
        }

        /// <summary>
        /// 공지사항 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetNoticeCache()
        {
            DataSet ds = GetDataSet(BOARD_NOTICE_KEY);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_NOTICE_LIST_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, 1);
                webDB.AddInParameter(cmd, "@LIST_CN", DbType.Int16, 10);
                webDB.AddInParameter(cmd, "@WORD", DbType.String, "");
                webDB.AddInParameter(cmd, "@SEARCH_WORD", DbType.String, "");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, BOARD_NOTICE_KEY, 0, 0, 10, 0);
            }

            return ds;
        }
        #endregion
        
        #region 라디오,TV 방송국 리스트
        /// <summary>
        /// 라디오,TV 방송국 리스트 데이터 캐싱
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetBroadcast()
        {
            DataSet ds = GetDataSet(BROADCAST_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_BROADCAST_COMPANY_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, BROADCAST_KEY, 0, 0, 5, 0);
            }

            return ds;
        }
        #endregion

        #region 상황중계
        /// <summary>
        /// 상황중계
        /// </summary>
        /// <param name="gameDate">경기날짜</param>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCast(int leId, string srId, string gameDate)
        {
            string GAME_SUMMARY_KEY = "GAME_SUMMARY_KEY_" + gameDate + "_" + leId + "_" + srId;  // 상황중계
            DataSet ds = GetDataSet(GAME_SUMMARY_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_FOCUS_GAME_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, srId);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.String, gameDate);

                string param = CommonUtil.ToProcParam(cmd);
                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, GAME_SUMMARY_KEY, 0, 0, 0, 30);
            }

            return ds;
        }

        public static DataSet GetGameCastSuspended(int leId, string srId, string gameDate)
        {
            string GAME_SUMMARY_KEY = "SUSPENDED_GAME_SUMMARY_KEY_" + gameDate + "_" + leId + "_" + srId;  // 상황중계
            DataSet ds = GetDataSet(GAME_SUMMARY_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_MAIN_FOCUS_GAME_S_SUSPENDED");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID_LIST", DbType.String, srId);
                kboDB2.AddInParameter(cmd, "@G_DT", DbType.String, gameDate);

                string param = CommonUtil.ToProcParam(cmd);
                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, GAME_SUMMARY_KEY, 0, 0, 0, 30);
            }

            return ds;
        }
        #endregion

        #region 문자중계
        /// <summary>
        /// 문자중계 scoreboard
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastScoreBoard(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_SCOREBOARD_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_SCOREBOARD_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
            }

            return ds;
        }

        /// <summary>
        /// 문자중계 othergame(타구장, 현재구장정보)
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastOtherGame(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_OTHER_GAME_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_OTHERGAME_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
            }

            return ds;
        }

        /// <summary>
        /// 문자중계 groundinfo (경기장 수비정보, 주자정보)
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastGroundInfo(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_GROUND_INFO_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_GROUND_INFO_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
            }

            return ds;
        }

        /// <summary>
        /// 문자중계 라인업
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastLineupInfo(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_LINEUP_INFO_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_LINEUP_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.String, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.String, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
            }

            return ds;
        }

        /// <summary>
        /// 문자중계 livetext (라이브텍스트)
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastLiveText(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_LIVETEXT_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_LIVETEXT_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int32, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
            }

            return ds;
        }

        /// <summary>
        /// 문자중계 선수정보
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastPlayerInfo(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_PLAYER_INFO_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_PLAYER_INFO_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int32, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
            }

            return ds;
        }

        /// <summary>
        /// 박스스코어 선수정보
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetGameCastBoxScore(int leId, int srId, string gameId, string gameYear)
        {
            String CacheKey = GAMECAST_BOXSCORE_KEY + "_" + leId + "_" + srId + "_" + gameYear + "_" + gameId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_CAST_BOXSCORE_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int32, srId);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.String, gameYear);
                kboDB2.AddInParameter(cmd, "@G_ID", DbType.String, gameId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 0, 10);
                SetDateSetSec(ds, CacheKey, 10);
            }

            return ds;
        }
        #endregion

        #region 우편번호 캐싱
        /// <summary>
        /// 우편번호 캐쉬올리기
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetZipCode()
        {
            String CacheKey = ZIP_CODE_KEY;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_ZIPCODE_S");

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 1, 0, 0, 0);
            }

            return ds;
        }
        #endregion

        #region KBO TV
        /// <summary>
        /// 하이라이트 영상 마지막 업로드 날짜
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetHightLightMaxDate(int boardSc, int leagueId)
        {
            String CacheKey = KBO_TV_HIGHLIGHT_KEY + leagueId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = webDB.GetStoredProcCommand("PROC_KBO_WEB_BOARD_GAME_MAX_DAY_S");
                webDB.AddInParameter(cmd, "@BD_SC", DbType.Int32, boardSc);
                webDB.AddInParameter(cmd, "@LE_ID", DbType.Int32, leagueId);

                ds = webDB.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 5, 0);
            }

            return ds;
        }
        
        #endregion


        #region 선수등록현황
        /// <summary>
        /// 선수등록현황 엔트리 자료 있는 최종일
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetRosterLastDate(int leagueId)
        {
            String CacheKey = ROSTER_LASTDATE_KEY + "_" + leagueId;
            DataSet ds = GetDataSet(CacheKey);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_PLAYERSTATE_ROSTER_LASTDATE_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int32, leagueId);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, CacheKey, 0, 0, 5, 0);
            }

            return ds;
        }
        #endregion

        #region 팀순위 팀별 순위변동 그래프 ( 시즌종료후 마지막 날짜 가져오기 )
        public static DataSet GetKboGameLastDate(int leId, int srId, string searchDate)
        {
            string GAMEKBO_LASTDATE_KEY = "GAMEKBO_LASTDATE_KEY_" + leId + srId + searchDate;  // 마지막날짜
            DataSet ds = GetDataSet(GAMEKBO_LASTDATE_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_KBO_LASTDATE_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, leId);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int16, srId);
                kboDB2.AddInParameter(cmd, "@SEARCH_DT", DbType.Date, searchDate);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, GAMEKBO_LASTDATE_KEY, 0, 0, 0, 10);
            }

            return ds;
        }
        #endregion

        #region 년도별 관중 현황
        /// <summary>
        /// 년도별 관중 현황
        /// </summary>
        /// <param name="leId"></param>
        /// <param name="srId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DataSet GetGameCrowdSum(int leId, int srId, int year)
        {
            DataSet ds = GetDataSet(KBO_EVENT_CROWD_SUM_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_GAME_KBO_CROWD_SUM_S");
                kboDB2.AddInParameter(cmd, "@LE_ID", DbType.Int16, Baseball.KBO_LE_ID);
                kboDB2.AddInParameter(cmd, "@SR_ID", DbType.Int16, Baseball.REGULAR_SR_ID);
                kboDB2.AddInParameter(cmd, "@SEASON_ID", DbType.Int32, year);

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, KBO_EVENT_CROWD_SUM_KEY, 0, 0, 5, 0);
            }

            return ds;
        }
        #endregion

        #region 메인 선수검색
        /// <summary>
        /// 메인 선수 검색
        /// </summary>
        /// <returns>DataTable</returns>
        //public static DataTable GetPlayerSearch()
        //{
        //    DataSet ds = GetDataSet(PLAYER_SEARCH_KEY);

        //    if (ds == null)
        //    {
        //        DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_SEARCH_PLAYER_LIST_S");

        //        ds = kboDB2.ExecuteDataSet(cmd);
        //        SetDataSet(ds, PLAYER_SEARCH_KEY , 0, 0, 5, 0);
        //    }

        //    return ds.Tables[0];
        //}
        #endregion

        #region CD_DETAIL(상세정보)
		/// <summary>
        /// cd_detail
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetDetailCode()
        {
            DataSet ds = GetDataSet(KBO_CODE_DETAIL_KEY);

            if (ds == null)
            {
                DbCommand cmd = kboDB2.GetStoredProcCommand("PROC_KBO_DB2_CD_DETAIL_LIST_S");

                ds = kboDB2.ExecuteDataSet(cmd);
                SetDataSet(ds, KBO_CODE_DETAIL_KEY, 0, 1, 0, 0);
            }

            return ds;
        }
        #endregion 
        
        /// <summary>
        /// 캐쉬에 해당 데이터 정보 캐싱
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="cacheKey">캐싱 키</param>
        /// <param name="minute">캐싱 시간(분)</param>
        private static void SetDataTable(DataTable dt, String cacheKey, int minute)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 0, minute, 0));
            cache.Add(cacheKey, dt, CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
        }

        /// <summary>
        /// 캐쉬에 해당 데이터 정보 캐싱
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="cacheKey">캐싱 키</param>
        /// <param name="minute">캐싱 시간(분)</param>
        private static void SetDataSet(DataSet ds, String cacheKey, int minute)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 0, minute, 0));
            cache.Add(cacheKey, ds, CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
        }

        /// <summary>
        /// 캐쉬에 해당 데이터 정보 캐싱 //문자 중계 초단위.
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="cacheKey">캐싱 키</param>
        /// <param name="minute">캐싱 시간(초)</param>
        private static void SetDateSetSec(DataSet ds, String cacheKey, int sec)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 0, 0, sec));
            cache.Add(cacheKey, ds, CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
        }

        /// <summary>
        /// Cache -> DataSet
        /// </summary>
        /// <param name="cacheKey">Key</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string cacheKey)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            DataSet dsData = (DataSet)cache[cacheKey];
            
            return dsData;
        }

        /// <summary>
        /// JArray -> DataSet
        /// </summary>
        /// <param name="cacheKey">Key</param>
        /// <returns>DataSet</returns>
        public static JArray GetJArray(string cacheKey)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            JArray list = (JArray)cache[cacheKey];

            return list;
        }

        /// <summary>
        /// DataSet -> Cache
        /// </summary>
        /// <param name="dsData">DataSet</param>
        /// <param name="cacheKey">Key</param>
        /// <param name="days">일</param>
        /// <param name="hours">시</param>
        /// <param name="minutes">분</param>
        /// <param name="seconds">초</param>
        public static void SetDataSet(DataSet dsData, string cacheKey, int days, int hours, int minutes, int seconds)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(days, hours, minutes, seconds));
            cache.Add(cacheKey, dsData, CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
        }

        /// <summary>
        /// JArray -> Cache
        /// </summary>
        /// <param name="dsData">DataSet</param>
        /// <param name="cacheKey">Key</param>
        /// <param name="days">일</param>
        /// <param name="hours">시</param>
        /// <param name="minutes">분</param>
        /// <param name="seconds">초</param>
        public static void SetJArray(JArray list, string cacheKey, int days, int hours, int minutes, int seconds)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(days, hours, minutes, seconds));
            cache.Add(cacheKey, list, CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
        }

        /// <summary>
        /// 해당 캐시키 캐시 삭제
        /// </summary>
        /// <param name="cacheKey">Key</param>
        public static void DelCacheDataSet(string cacheKey)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            cache.Remove(cacheKey);
        }
    }
}