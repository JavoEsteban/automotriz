using Automotriz.connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Automotriz.Models
{
    public class Sucursal
    {   public int ID_SUCURSAL { get; set; }
        public string NOMBRE_SUCURSAL { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string IMAGEN { get; set; }
        public string TIPO { get; set; }
        public string EXTENSION { get; set; }
        public int VIGENCIA { get; set; }



        public List<Sucursal> Consultar_sucursales_preview()
        { //trae un preview de los datos de la tabla sucursal
            List<Sucursal> listaSucursales = new List<Sucursal>();


            SQLconn sql = new SQLconn();
            SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

            if (conn.State == System.Data.ConnectionState.Open)
            {
                SqlCommand command = new SqlCommand("TRAER_SUCURSALES", conn);

                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Sucursal sucursal = new Sucursal();
                    sucursal.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"].ToString());
                    sucursal.NOMBRE_SUCURSAL = reader["NOMBRE_SUCURSAL"].ToString();
                    sucursal.VIGENCIA = Convert.ToInt32(reader["VIGENCIA"].ToString());


                    listaSucursales.Add(sucursal);

                }


            }

            return listaSucursales;
        }

        public List<Sucursal> Consultar_Sucursales()
        { //trae las sucursales de la tabla sucursal
            List<Sucursal> listaSucursales = new List<Sucursal>();


            SQLconn sql = new SQLconn();
            SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

            if (conn.State == System.Data.ConnectionState.Open)
            {
                SqlCommand command = new SqlCommand("procedimiento", conn);

                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Sucursal sucursal = new Sucursal();
                    sucursal.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"].ToString());
                    sucursal.NOMBRE_SUCURSAL = reader["NOMBRE_SUCURSAL"].ToString();
                    sucursal.VIGENCIA = Convert.ToInt32(reader["VIGENCIA"].ToString());


                    listaSucursales.Add(sucursal);

                }


            }

            return listaSucursales;
        }







    }




}