using Automotriz.connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Automotriz.Models
{
    public class TipoCombustible
    {
        public int ID_TIPO_COMBUSTIBLE { get; set; }
        public string DESCRIPCION { get; set; }
        public int VIGENCIA { get; set; }

        public RespuestaServicio AgregarCombustible() //Agrega un tipo de combustible a la db en la tabla TIPO_COMBUSTIBLE
        {
            RespuestaServicio respuesta = new RespuestaServicio();
            SqlConnection conn = null;
            SQLconn sql = new SQLconn();


            try
            {

                conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("INSERTAR_COMBUSTIBLES_TIPO_COMBUSTIBLES", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DESCRIPCION", DESCRIPCION);
                    command.Parameters.AddWithValue("@VIGENCIA", VIGENCIA);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    }

                }
                sql.CerrarConnection(conn);
                respuesta.Respuesta = "OK";
                respuesta.Descripcion = "";
                respuesta.Detalle_Error = "Se agrego correctamente el Tipo de Combustible";
                return respuesta;
            }
            catch (Exception ex)
            {
                sql.CerrarConnection(conn);
                respuesta.Respuesta = "NOK";
                respuesta.Descripcion = "";
                respuesta.Detalle_Error = "ups, no se logro agregar el tipo de Combustible";
                return respuesta;
            }
        }


    }
}
