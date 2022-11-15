using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Domain.Response
{
    public class PhotoResponse
    {

        public const int Save = 1;
        public const int Error = 2;

        public string message { get; set; }
        public int status { get; set; }
        public string url { get; set; }

    }
}
