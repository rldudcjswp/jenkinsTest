using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;
using KBOLib.Util;
using WebApplication2.Engine;

namespace WebApplication2.ws
{
    /// <summary>
    /// Common의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class Common : CommonWS
    {
        /// <summary>
        /// 참고
        /// </summary>
        [WebMethod(EnableSession = true, Description = "참고용")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetTest(string leId, string srId, string gId, string tbSc)
        {
            CommonAction action = new CommonAction();
            Response(JsonConvert.SerializeObject(action.GetTest(leId, srId, gId, tbSc), Formatting.Indented));
        }
    }
}
