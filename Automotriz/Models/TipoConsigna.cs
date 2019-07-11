using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automotriz.Models
{
    public class TipoConsigna
    {
        public int ID_TIPO_CONSIGNACION { get; set; }
        public string DESCRIPCION { get; set; }
        public int VIGENCIA { get; set; }
    }
}