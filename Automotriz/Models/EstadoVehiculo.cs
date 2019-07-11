using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automotriz.Models
{
    public class EstadoVehiculo
    {
        public int ID_ESTADO{ get; set; }
        public string DESCRIPCION { get; set; }
        public int VIGENCIA { get; set; }
    }
}