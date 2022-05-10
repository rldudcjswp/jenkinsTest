using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBOLib.Model
{
    public class CAbstractDataWrapper : CommonDataTable
    {
        public string totalCnt { get; set; }

        public List<CAbstractDataTable> tables = new List<CAbstractDataTable>();
    }
}
