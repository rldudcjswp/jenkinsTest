using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace KBOLib.Util
{
    public class ExcelUtil
    {
        public void DownExcel(string htmlbody, string pagename, string title)
        {
            // 파일명 설정        
            string filename = pagename + "[" + DateUtil.GetNowDate() + "]";

            if (htmlbody != "")
            {

                System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
                objResponse.ClearContent();
                objResponse.ClearHeaders();
                objResponse.ContentType = "application/vnd.msexcel";
                objResponse.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls"); // 파일명

                string sep = "";

                htmlbody = htmlbody.Replace("<TABLE", "<TABLE border='1'");
                htmlbody = htmlbody.Replace("<A href=", "<A ");  // 링크제거하기위해
                //htmlbody = htmlbody.Replace("-", "--");  // 링크제거하기위해

                // HEAD부분 색깔칠하기
                htmlbody = htmlbody.Replace("<THEAD>\r\n<TR>", "<THEAD>\r\n<TR style='background-color:#e6ecf9;'>");

                // HEAD
                sep = "<html> \r\n";
                sep += "<style>DIV,TR,TD {font-size:9pt;font-family:돋움;color:#474747}</style> \r\n<body> \r\n";
                sep += "<table> \r\n";
                sep += "<tr> \r\n";
                sep += "    <td colspan=40 align=left>" + title + "</td>\r\n";
                sep += "</tr> \r\n";
                sep += "<tr> \r\n";
                sep += htmlbody;
                sep += "</tr>";
                sep += "</table>";
                sep += "</body></html>";

                objResponse.Charset = "euc-kr";
                objResponse.ContentEncoding = Encoding.UTF8;
                objResponse.Write("<meta http-equiv=Content-Type content=''text/html; charset=utf-8''>"); // 2015-04-20 yeeun 한글깨짐 현상때문에 추가
                objResponse.Write(sep);
                objResponse.Flush();
                objResponse.Close();
                objResponse.End();
            }
        }

        public void DownExcel(string htmlbody, string pagename, string title, string state)
        {
            // 파일명 설정        
            string filename = pagename + "[" + DateUtil.GetNowDate() + "]";

            if (htmlbody != "")
            {

                System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
                objResponse.ClearContent();
                objResponse.ClearHeaders();
                objResponse.ContentType = "application/vnd.msexcel";
                objResponse.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls"); // 파일명

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                htmlbody = htmlbody.Replace("<A href=", "<A ");  // 링크제거하기위해

                // HEAD
                sb.Append("<html> \r\n");
                sb.Append("<body>");

                sb.Append(htmlbody);
                sb.Append("</body></html>");

                objResponse.Charset = "euc-kr";
                objResponse.ContentEncoding = Encoding.UTF8;
                objResponse.Write(sb);
                objResponse.Flush();
                objResponse.Close();
                objResponse.End();
            }
        }

        public void DownExcel(string htmlbody, string pagename)
        {
            // 파일명 설정        
            string filename = pagename + "[" + DateUtil.GetNowDate() + "]";

            if (htmlbody != "")
            {

                System.Web.HttpResponse objResponse = System.Web.HttpContext.Current.Response;
                objResponse.ClearContent();
                objResponse.ClearHeaders();
                objResponse.ContentType = "application/vnd.msexcel";
                objResponse.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls"); // 파일명

                string sep = "";

                htmlbody = htmlbody.Replace("<TABLE", "<TABLE border='1'");
                htmlbody = htmlbody.Replace("<A href=", "<A ");  // 링크제거하기위해
                //htmlbody = htmlbody.Replace("-", "--");  // 링크제거하기위해

                // HEAD부분 색깔칠하기
                htmlbody = htmlbody.Replace("<THEAD>\r\n<TR>", "<THEAD>\r\n<TR style='background-color:#e6ecf9;'>");

                sep += htmlbody;

                objResponse.Charset = "euc-kr";
                objResponse.ContentEncoding = Encoding.UTF8;
                objResponse.Write("<meta http-equiv=Content-Type content=''text/html; charset=utf-8''>"); // 2015-04-20 yeeun 한글깨짐 현상때문에 추가
                objResponse.Write(sep);
                objResponse.Flush();
                objResponse.Close();
                objResponse.End();
            }
        }

        public static void DownExcelCareer(string body, string fileName)
        {
            string headerValue = string.Format("attachment; filename={0}_{1}.xls", fileName, DateUtil.GetNowDate());

            if (body != "")
            {
                body = body.Replace("+", "%2b");
                string htmlBody = HttpUtility.UrlDecode(body);
                htmlBody = Regex.Replace(htmlBody, "<a.*?>", string.Empty, RegexOptions.Singleline); // a 태그 삭제 구문 추가
                htmlBody = Regex.Replace(htmlBody, "</a.*?>", string.Empty, RegexOptions.Singleline); // a 태그 삭제 구문 추가
                htmlBody = Regex.Replace(htmlBody, "<input.*?>", string.Empty, RegexOptions.Singleline); // input 태그 삭제 구문 추가
                htmlBody = Regex.Replace(htmlBody, "<caption>.*?</caption>", string.Empty, RegexOptions.Singleline); // caption 태그 삭제 구문 추가
                htmlBody = Regex.Replace(htmlBody, "<div class=\"right\">.*?</div>", string.Empty, RegexOptions.Singleline); // 오른쪽 컨트롤 제거
                htmlBody = Regex.Replace(htmlBody, "<td id=\"tdDownload\">.*?</td>", string.Empty, RegexOptions.Singleline); // 사진저장 버튼 삭제
                htmlBody = Regex.Replace(htmlBody, "rowSpan=\"5\"", "rowSpan=\"6\"", RegexOptions.Singleline); // 사진저장 버튼 삭제

                System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;

                response.Clear();
                response.ContentType = "application/ms-excel";
                response.AddHeader("content-disposition", headerValue);
                response.Charset = "UTF-8";
                response.ContentEncoding = System.Text.Encoding.UTF8;
                response.Cache.SetCacheability(HttpCacheability.NoCache);
                response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                response.Write("<head>");
                response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                response.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://kboadmin.koreabaseball.com/Common/css/excel_career.css\" />");
                response.Write("<!--[if gte mso 9]><xml>");
                response.Write("<x:ExcelWorkbook>");
                response.Write("<x:ExcelWorksheets>");
                response.Write("<x:ExcelWorksheet>");
                response.Write("<x:Name>Report Data</x:Name>");
                response.Write("<x:WorksheetOptions>");
                response.Write("<x:Print>");
                response.Write("<x:ValidPrinterInfo/>");
                response.Write("</x:Print>");
                response.Write("</x:WorksheetOptions>");
                response.Write("</x:ExcelWorksheet>");
                response.Write("</x:ExcelWorksheets>");
                response.Write("</x:ExcelWorkbook>");
                response.Write("</xml>");
                response.Write("<![endif]--> ");
                //StringWriter tw = new StringWriter();
                //HtmlTextWriter hw = new HtmlTextWriter(tw);
                //hfData.RenderControl(hw);
                response.Write("</head>");
                response.Write("<body>");
                //response.Write(htmlTitle);
                response.Write(htmlBody);
                response.Write("</body></html>");
                response.Flush();
                response.End();
            }
        }
    }
}