using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;

namespace KBOLib.Util
{
    public class IpCheck
    {
        public static bool Checked(string ip)
        {
            // 허용 IP 리스트 
            NameValueCollection nvcipKeys = GetipKeys();
            bool result = false;

            for (int i = 0; i < nvcipKeys.Count; i++)
            {
                if (ip.IndexOf(nvcipKeys.Get(i)) > -1)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private static NameValueCollection GetipKeys()
        {
            //Declare a name value collection to store Database Key List from web.config
            NameValueCollection nvcDatabaseKeyList;
            nvcDatabaseKeyList = (NameValueCollection)ConfigurationManager.GetSection("ipSettings");

            return nvcDatabaseKeyList;
        }
    }
}