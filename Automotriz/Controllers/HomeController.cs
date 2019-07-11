using Automotriz.connection;
using Automotriz.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Automotriz.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Index()
        {
                 ViewBag.NOMBRE_USUARIO = Session["NOMBRE_USUARIO"];
                 ViewBag.IMAGEN = Session["IMAGEN"];
                 ViewBag.SUCURSAL = Session["SUCURSAL"];
            return View();
        }
        public ActionResult Dashboard()
        {

            ViewBag.NOMBRE_USUARIO = Session["NOMBRE_USUARIO"];
            ViewBag.IMAGEN = (string)Session["IMAGEN"];
            ViewBag.SUCURSAL = Session["SUCURSAL"];
            return View();
        }

        public static string Base64Encoder(byte[] plainText) //codifica a base 64
        {
            string strIMG = System.Convert.ToBase64String(plainText);
            return strIMG;
        }

        public static byte[] Base64Decoder(string base64EncodedData) // pasa de base 64 a byte []
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }
        public string LoginUsuario(Usuarios usuario) {
            Usuarios usuariosAutenticado = new Usuarios();
            string rutConPuntos = "";

            rutConPuntos = usuario.RUT;
            string rutCorregido = "";

            rutCorregido = rutConPuntos.Replace(".",""); 

            RespuestaServicio respuestaServicio = new RespuestaServicio();
                try
                {
                    SQLconn sql = new SQLconn();
                    SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand command = new SqlCommand("LOGIN_USUARIO_USUARIOS", conn);

                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RUT", rutCorregido);
                        command.Parameters.AddWithValue("@PASSWORD", usuario.PASSWORD);

                        SqlDataReader reader = command.ExecuteReader();

                        bool user= false;
                        bool pass = false;
                        while (reader.Read())
                        {

                            usuariosAutenticado.ID_USUARIOS = Convert.ToInt32(reader["ID_USUARIOS"]);
                            usuariosAutenticado.NOMBRE_USUARIO = reader["NOMBRE_USUARIO"].ToString();
                            usuariosAutenticado.RUT = reader["RUT"].ToString();
                            usuariosAutenticado.EMAIL = reader["EMAIL"].ToString();
                            usuariosAutenticado.SUCURSAL = reader["SUCURSAL"].ToString();
                            usuariosAutenticado.TIPO_IMAGEN = reader["TIPO_IMAGEN"].ToString();
                            //usuariosAutenticado.PASSWORD = reader["PASSWORD"].ToString();



                            if (usuariosAutenticado.TIPO_IMAGEN != "")
                            {


                                string extension = usuariosAutenticado.TIPO_IMAGEN;
                                byte[] img = (byte[])reader["IMAGEN"]; //almacena la imagen de la db en un byte[]

                                

                                string IMG = Base64Encoder(img);

                                usuariosAutenticado.IMAGEN = extension + "," + IMG;

                         }
                            else {
                                usuariosAutenticado.IMAGEN = "/assets/img/avatar_default.jpg";

                            }
                            user = true;
                            pass = true;

                            //if (rutCorregido == usuariosAutenticado.RUT)
                            //{
                            //    user = true;
                            //}
                            //else {
                            //    user = false;
                            //}
                            //if (usuario.PASSWORD == usuariosAutenticado.PASSWORD)
                            //{
                            //    pass = true;
                            //}
                            //else {
                            //    pass = false;
                            //}



                    }

                        if (user == true && pass == true)
                        {

                        Session["ID_USUARIO"] = usuariosAutenticado.ID_USUARIOS;
                        Session["NOMBRE_USUARIO"] = usuariosAutenticado.NOMBRE_USUARIO;
                        Session["IMAGEN"] = usuariosAutenticado.IMAGEN;
                        Session["SUCURSAL"] = usuariosAutenticado.SUCURSAL;



                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(usuariosAutenticado, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else if (user == false)
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "Error de autenticacion";
                        respuestaServicio.Detalle_Error = "";
                    }
                    

                }
                }
                catch (Exception ex)
                {
                    respuestaServicio.Respuesta = "NOK";
                    respuestaServicio.Descripcion = "Error" + ex.Message;
                    respuestaServicio.Detalle_Error = "";
                }

                string respuesta = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
                return respuesta;



        }
    }
}