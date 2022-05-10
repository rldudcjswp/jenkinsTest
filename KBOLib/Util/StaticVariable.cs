using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace KBOLib.Util
{
    public class StaticVariable
    {
        /****************************** Common Variable ******************************/

        public static string FILE_URL = ConfigurationManager.AppSettings["file_url"].ToString();
        public static string WS_URL = ConfigurationManager.AppSettings["ws_url"].ToString();
        public static string MAP_URL = ConfigurationManager.AppSettings["map_url"].ToString();
        public static string CDN_URL = ConfigurationManager.AppSettings["CDN_URL"].ToString();
        public static string IMG_URL = ConfigurationManager.AppSettings["IMG_URL"].ToString();
        public static string JS_VERSION = ConfigurationManager.AppSettings["JS_VERSION"].ToString();
        public static string CSS_VERSION = ConfigurationManager.AppSettings["CSS_VERSION"].ToString();
    }
}