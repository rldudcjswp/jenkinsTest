using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBOLib.Util
{
    public class CommonWS : System.Web.Services.WebService
    {
        #region 내부 함수
        /// <summary>
        /// json 렌더링 함수 
        /// </summary>
        /// <param name="jsonString">json 스트링 문자열</param>
        protected void Response(string jsonString)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "text/plain; charset=UTF-8";
            Context.Response.Charset = "UTF-8";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            Context.Response.Write(jsonString);
            Context.Response.Flush();
        }
        #endregion
    }
}
