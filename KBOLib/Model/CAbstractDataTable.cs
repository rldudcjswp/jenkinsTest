using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Model
{
    public class CAbstractDataTable : CommonDataTable
    {
        public string totalCnt { get; set; }
        public string headerClass { get; set; }
        public string tbodyClass { get; set; }
        public string tfootClass { get; set; }
        public string title { get; set; }
        public string caption { get; set; }

        public List<CAbstractDataRow> colgroup = new List<CAbstractDataRow>();
        public List<CAbstractDataRow> headers = new List<CAbstractDataRow>();
        public List<CAbstractDataRow> rows = new List<CAbstractDataRow>();
        public List<CAbstractDataRow> tfoot = new List<CAbstractDataRow>();
    }
}