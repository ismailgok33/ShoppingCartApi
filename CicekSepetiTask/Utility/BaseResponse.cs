using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Base
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; } = null;
        public int ResponseCode { get; set; } = 200;
    }
}
