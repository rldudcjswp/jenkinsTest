using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace WebApplication2.Engine
{
    public class CommonAction
    {
        private Database kboDB = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_db2");

        /// <summary>
        /// 참고
        /// </summary>
        public JObject GetTest(string leId, string srId, string gId, string tbSc)
        {
            JObject result = new JObject();
            try
            {
                DbCommand cmd = kboDB.GetStoredProcCommand("PROC_OJT_KBO_DB2_GAME_BOXSCORE_HITTER_S");
                kboDB.AddInParameter(cmd, "@LE_ID", DbType.Int16, leId);
                kboDB.AddInParameter(cmd, "@SR_ID", DbType.Int16, srId);
                kboDB.AddInParameter(cmd, "@G_ID", DbType.String, gId);
                kboDB.AddInParameter(cmd, "@TB_SC", DbType.String, tbSc);

                DataSet dsData = kboDB.ExecuteDataSet(cmd);

                DataRow[] drData = dsData.Tables[0].Select();

                JArray list = new JArray();
                JArray list2 = new JArray();
                JArray list3 = new JArray();


                foreach (DataRow item in drData)
                {
                    JObject obj = new JObject();
                    obj.Add(new JProperty("POS_IF", item["POS_IF"]));
                    obj.Add(new JProperty("P_NM", item["P_NM"]));
                    obj.Add(new JProperty("CH_INN_NO", item["CH_INN_NO"]));

                    for (int i = 1; i <= 15; i++)
                    {
                        obj.Add(new JProperty("INN" + i + "_1_IF", item["INN" + i + "_1_IF"]));
                    }

                    obj.Add(new JProperty("AB_CN", item["AB_CN"]));
                    obj.Add(new JProperty("HIT_CN", item["HIT_CN"]));
                    obj.Add(new JProperty("RBI_CN", item["RBI_CN"]));
                    obj.Add(new JProperty("RUN_CN", item["RUN_CN"]));
                    obj.Add(new JProperty("SEASON_HRA_RT", item["SEASON_HRA_RT"]));
                    obj.Add(new JProperty("GAME5_HRA_RT", item["GAME5_HRA_RT"]));
                    obj.Add(new JProperty("TEAM_NM", item["TEAM_NM"]));

                    list.Add(obj);
                }


                DataRow[] drData2 = dsData.Tables[1].Select();

                foreach (DataRow item in drData2)
                {
                    JObject obj = new JObject();
                    obj.Add(new JProperty("HRA_RT_5", item["HRA_RT_5"]));

                    list2.Add(obj);
                }

                DataRow[] drData3 = dsData.Tables[2].Select();

                foreach (DataRow item in drData3)
                {
                    JObject obj = new JObject();
                    obj.Add(new JProperty("HRA_RT_SEASON", item["HRA_RT_SEASON"]));

                    list3.Add(obj);
                }

                result.Add(new JProperty("list", list));
                result.Add(new JProperty("list2", list2));
                result.Add(new JProperty("list3", list3));

            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}