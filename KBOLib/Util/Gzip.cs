using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO.Compression;
using System.IO;

namespace KBOLib.Util
{
    public class Gzip
    {
        #region IHttpHandler Members
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            string file = context.Server.MapPath(context.Request.FilePath.Replace(".ashx", ""));//파일 경로
            string filename = file.Substring(file.LastIndexOf('\\') + 1);//파일 이름
            string extension = file.Substring(file.LastIndexOf('.') + 1);//파일 확장자

            //1순위 - Gzip
            if (context.Request.Headers["Accept-encoding"] != null && context.Request.Headers["Accept-encoding"].Contains("gzip"))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Length > 1024)//1K 용량 설정
                {
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                    context.Response.AddHeader("Content-Encoding", "gzip");
                }

            }//2순위 - Deflate
            else if (context.Request.Headers["Accept-encoding"] != null && context.Request.Headers["Accept-encoding"].Contains("deflate"))
            {
                FileInfo fileInfo = new FileInfo(file);

                if (fileInfo.Length > 1024)//1K 용량 설정
                {
                    context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress, true);
                    context.Response.AddHeader("Content-encoding", "deflate");
                }

            }
            //ContentType설정
            if (extension == "aspx")
                context.Response.ContentType = "text/html";
            if (extension == "js")
                context.Response.ContentType = "application/x-javascript";
            if (extension == "css")
                context.Response.ContentType = "text/css";

            //화면 출력
            context.Response.WriteFile(file);
        }
        #endregion
    }
}