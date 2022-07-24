using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISLCELib.Common
{
    public class MethodReturn
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool HasException { get; set; }
        public Exception Exception { get; set; }
    }
}
