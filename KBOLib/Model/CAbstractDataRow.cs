using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBOLib.Model
{
    public class CAbstractDataRow
    {
        public List<CAbstractDataCol> row = new List<CAbstractDataCol>();
        public string Class { get; set; }
        public string OnClick { get; set; }
        public string Style { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }
}