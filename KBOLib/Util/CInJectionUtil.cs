using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBOLib.Util
{
    public class CInJectionUtil
    {
        private static string[] arrInjaction = new[] { " or ", "select ", "create ", "exec ", "execute ", "insert ", "update ", "delete ", "drop ", "truncate ", "xp_", "sp_", "filesystem", "createobject", "scripting", "<iframe", "<script", "<!", "<?", "&#8238", "&#160", "&#173", "&#8205", "&#8204", "&#8237", "%3C", "__VIEWSTATE" };
        private static string[] arrInjactionBoard = new[] { " or ", "select ", "create ", "exec ", "execute ", "insert ", "update ", "delete ", "drop ", "truncate ", "xp_", "sp_", "filesystem", "createobject", "scripting", "<script", "<!", "<?", "&#8238", "&#160", "&#173", "&#8205", "&#8204", "&#8237", "%3C", "__VIEWSTATE" };

        public CInJectionUtil() { }

        /// <summary>
        /// SQL Injaction 체크하기
        /// </summary>
        public static bool CheckSqlInjaction(String param)
        {
            bool ret = false;
            

            foreach (string t in arrInjaction)
            {
                int index = param.ToLower().IndexOf(t.ToLower());
                if (index >= 0)
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// SQL Injaction 체크하기
        /// </summary>
        public static String replaceSqlInjaction(String param)
        {
            foreach (string t in arrInjaction)
            {
                int index = param.ToLower().IndexOf(t.ToLower());
                if (index >= 0)
                {
                    String temp = param.Substring(index, t.Length);
                    param = param.Replace(temp, "");    
                }
            }

            return param;
        }

        /// <summary>
        /// SQL Injaction 체크하기
        /// </summary>
        public static String replaceSqlInjactionBoard(String param)
        {
            foreach (string t in arrInjactionBoard)
            {
                int index = param.ToLower().IndexOf(t.ToLower());
                if (index >= 0)
                {
                    String temp = param.Substring(index, t.Length);
                    param = param.Replace(temp, "");
                }
            }

            return param;
        }
    }
}
