using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DTO
{
    public class FilterDTO
    {
        public string malKodu { get; set; }
        public DateTime girisTarih { get; set; }
        public DateTime cikisTarih { get; set; }
    }
}
