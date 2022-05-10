using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Util
{
    public class MessageCode
    {
        public const string CD_OK = "100";
        public const string CD_OK_MSG = "성공";

        public const string CD_FAIL = "200";
        public const string CD_FAIL_MSG = "실패";

        public const string CD_INSERT_OK_MSG = "정상적으로 등록되었습니다.";
        public const string CD_DELETE_OK_MSG = "정상적으로 삭제되었습니다.";
        public const string CD_UPDATE_OK_MSG = "정상적으로 수정되었습니다.";

        // 로그인 실패
        public const string CD_LOGIN_FAIL = "201";
        public const string CD_LOGIN_FAIL_MSG = "현재 입력하신 아이디가 등록되어 있지 않거나, 아이디 또는 비밀번호를 잘못 입력 하였습니다.";

        // 아이디 중복
        public const string CD_ID_OVERLAP_FAIL = "202";
        public const string CD_ID_OVERLAP_FAIL_MSG = "중복된 아이디입니다. 다른 아이디를 사용하세요.";

        // 불량 및 강제 탈퇴
        public const string CD_BAD_MEMBERS_FAIL = "203";
        public const string CD_BAD_MEMBERS_FAIL_MSG = "불량 및 강제 탈퇴 회원입니다.";

        // 회원 가입 실패
        public const string CD_ID_REGISTER_FAIL = "204";
        public const string CD_ID_REGISTER_FAIL_MSG = "회원가입 실패.";

        // 이미 가입된 사용자
        public const string CD_USER_OVERLAP_FAIL = "205";
        public const string CD_USER_OVERLAP_FAIL_MSG = "이미 가입된 사용자입니다.";

        // 회원정보 수정 실패
        public const string CD_USER_MODIFY_FAIL = "206";
        public const string CD_USER_MODIFY_FAIL_MSG = "회원정보 수정 실패.";

        // 비밀번호 변경 실패
        public const string CD_USER_MODIFY_PASS_FAIL = "207";
        public const string CD_USER_MODIFY_PASS_FAIL_MSG = "비밀번호 변경 실패.";

        // 비밀번호찾기
        public const string CD_USER_PWSEARCH_FAIL = "208";
        public const string CD_USER_PWSEARCH_FAIL_MSG = "비밀번호 찾기 실패.";

        // 아이디찾기
        public const string CD_USER_IDSEARCH_FAIL = "209";
        public const string CD_USER_IDSEARCH_FAIL_MSG = "아이디찾기 실패.";
        
        // 탈퇴
        public const string CD_USER_LEAVE_FAIL = "210";
        public const string CD_USER_LEAVE_FAIL_MSG = "탈퇴 실패.";

        // 회원정보확인
        public const string CD_USER_USERCHECK_FAIL = "211";
        public const string CD_USER_USERCHECK_FAIL_MSG = "회원정보가 일치하지 않습니다.";


        // 이벤트 중복신청
        public const string CD_EVENT_OVERLAP_FAIL = "201";
        public const string CD_EVENT_OVERLAP_FAIL_MSG = "해당 정보로 신청한 사용자가 있습니다."; // 같은 이름으로 등록된 사용자가 있습니다.

        // 이벤트 신청기간 X
        public const string CD_EVENT_PERIOD_FAIL = "202";
        public const string CD_EVENT_PERIOD_FAIL_MSG = "신청기간이 아닙니다.";
    }
}