using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Domain.Request
{
    public class PhotoPersonalRequest
    {
        public IFormFile file { get; set; }
        public String nombre { get; set; }
    }
}
