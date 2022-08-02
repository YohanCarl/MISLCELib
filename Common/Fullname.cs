using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISLCELib.Common
{
    public class Fullname
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Mname { get; set; }
        public string Suffix { get; set; }
        public string GetFullname(NameFormat nameFormat = NameFormat.LnFnMnS)
        {
            string fullname = "";
            switch (nameFormat)
            {
                case NameFormat.LnFnMnS: 
                    fullname = Lname + ", " + Fname + (Mname=="" || Mname == null ? "" : " " + Mname) + (Suffix == "" || Suffix == null ? "" : " " + Suffix) ; 
                    break;
                case NameFormat.FnMnLnS:
                    fullname = Fname + (Mname == "" || Mname == null ? "" : " " + Mname) + Lname + (Suffix == "" || Suffix == null ? "" : " " + Suffix);
                    break;
                default:
                    fullname = "";
                    break;
            }
            return fullname;
        }
    }
    public enum NameFormat
    {
        LnFnMnS,
        FnMnLnS
    }
}
