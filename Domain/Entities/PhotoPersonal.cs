using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Domain.Entities
{
    public class PhotoPersonal
    {
        [Key]
        public int codigo { get; set; }

        public String nombre { get; set; }

        public double tamanio { get; set; }

        public String ubicacion { get; set; }


    }
}
