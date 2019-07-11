using Automotriz.connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Automotriz.Models
{
    public class Roles
    {

        public int ID_ROL { get; set; }
        public string DESCRIPCION { get; set; }
        public int VIGENCIA { get; set; }


        public List<Roles> Consultar_Rol()
        { //consulta los roles de la tabla roles 
            List<Roles> listaRoles = new List<Roles>();


            SQLconn sql = new SQLconn();
            SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

            if (conn.State == System.Data.ConnectionState.Open)
            {
                SqlCommand command = new SqlCommand("CONSULTAR_ROLES", conn);

                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Roles roles = new Roles();
                    roles.ID_ROL = Convert.ToInt32(reader["ID_ROL"].ToString());
                    roles.DESCRIPCION = reader["DESCRIPCION"].ToString();
                    roles.VIGENCIA = Convert.ToInt32(reader["VIGENCIA"].ToString());


                    listaRoles.Add(roles);

                }


            }



            return listaRoles;
        }







    }

   
}