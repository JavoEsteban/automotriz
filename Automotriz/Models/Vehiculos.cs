using Automotriz.connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Automotriz.Models
{
    public class Vehiculos
    {
        public int ID_VEHICULO { get; set; }
        public int ID_SUCURSAL { get; set; }
        public int ID_USUARIOS { get; set; }
        public int ID_DISPONIBILIDAD { get; set; }
        public int ID_MARCA { get; set; }
        public int ID_TIPOTRACCION { get; set; }
        public int ID_TIPO_CONSIGNACION { get; set; }
        public int ID_COLOR { get; set; }
        public int ID_TIPO_TRANSMICION { get; set; }
        public int ID_TIPO_VEHICULO { get; set; }
        public int ID_TIPO_COMBUSTIBLE { get; set; }
        public int ID_ESTADO { get; set; }
        public int ID_MODELO { get; set; }
        public string PATENTE { get; set; }
        public DateTime FECHA_INGRESO { get; set; }
        public string VERSION { get; set; }
        public string MOTOR { get; set; }
        public int ANO { get; set; }
        public string CILINDRADA { get; set; }
        public int PRECIO_VENTA { get; set; }
        public int PRECIO_COMPRA { get; set; }
        public int PRECIO_CONSIGNACION { get; set; }
        public int PRECIO_MINIMO_VENTA { get; set; }
        public int KILOMETRAJE { get; set; }
        public string CHASIS { get; set; }
        public int CANTIDAD_DUENIOS { get; set; }
        public string NOMBRE_MODELO { get; set; }
        public string NOMBRE_TRANSMISION { get; set; }
        public string IMAGEN_PRINCIPAL { get; set; }
        public List<ImagenVehiculo> IMAGENES_VEHICULO { get; set; }




    }
}