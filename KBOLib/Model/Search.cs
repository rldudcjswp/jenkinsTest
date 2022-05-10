using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Model
{
    [Serializable]
    public class Search
    {
        public string seasonId { get; set; }    // 시즌 ID
        public string seriesId { get; set; }    // 시리즈 ID 리스트
        public string seriesId2 { get; set; }    // 시리즈 ID 리스트
        public string teamId { get; set; }  // 팀 ID
        public string pos { get; set; } // 포지션
        public string situation { get; set; }  // 경기상황별 조건
        public string situationDetail { get; set; }  // 경기상황별 조건 상세
        public string orderCol { get; set; }    // 정렬 컬럼명
        public string order { get; set; }   // 정렬 (asc, desc)
    }
}