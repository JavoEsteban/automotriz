using Automotriz.connection;
using Automotriz.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Automotriz.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        public ActionResult IndexSucursal()
        {
            return View();
        }


        public string ObtienePreviewSucursal()
        {
            RespuestaServicio Respuesta = new RespuestaServicio();
            Sucursal Obj_Sucursal = new Sucursal();

            List<Sucursal> Lista_Sucursales = new List<Sucursal>();

            try
            {

                Lista_Sucursales = Obj_Sucursal.Consultar_sucursales_preview();

                Respuesta.Respuesta = "OK";
                Respuesta.Descripcion = JsonConvert.SerializeObject(Lista_Sucursales, Formatting.Indented);
                Respuesta.Detalle_Error = "";

            }
            catch (Exception Err)
            {

                Respuesta.Respuesta = "NOK";
                Respuesta.Descripcion = "";
                Respuesta.Detalle_Error = Err.Message.ToString();
            }

            return JsonConvert.SerializeObject(Respuesta, Formatting.Indented); ;
        }
        public static string  Base64Encoder(byte[] plainText)
        {
            string strIMG = System.Convert.ToBase64String(plainText);
            return strIMG;
        }

        public static byte[] Base64Decoder(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }
       

        public RespuestaServicio cargarImagen(string DESCRIPCION, string DIRECCION, string TELEFONO, string IMAGEN, string VIGENCIA, string TIPO_ARCHIVO, string EXTENSION)
        {
                RespuestaServicio respuesta = new RespuestaServicio();
                SqlConnection conn = null;
                SQLconn sql = new SQLconn();

            try
            {
                
                string [] IMAGENarr = IMAGEN.Split(',');
                string arreglado = IMAGENarr[1];
                byte[] img = Base64Decoder(arreglado);

                TIPO_ARCHIVO = IMAGENarr[0];
                
                
           
                conn = sql.AbrirConnection(sql.ObtenerConnection());

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        SqlCommand command = new SqlCommand("INGRESAR_SUCURSAL_SUCURSAL", conn);

                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DESCRIPCION", DESCRIPCION);
                        command.Parameters.AddWithValue("@DIRECCION", DIRECCION);
                        command.Parameters.AddWithValue("@TELEFONO", TELEFONO);
                        command.Parameters.AddWithValue("@IMAGEN", img);
                        command.Parameters.AddWithValue("@VIGENCIA", VIGENCIA);
                        command.Parameters.AddWithValue("@TIPO_ARCHIVO", TIPO_ARCHIVO);
                        command.Parameters.AddWithValue("@EXTENSION", EXTENSION);

                        //guardarEnCarpeta(img, DESCRIPCION, EXTENSION);

                        command.ExecuteNonQuery();

            
                    }

                    sql.CerrarConnection(conn);

                    respuesta.Respuesta = "OK";
                    respuesta.Descripcion = "";
                    respuesta.Detalle_Error = "Se agrego correctamente la Marca";

                    return respuesta;
                }
                catch (Exception ex)
                {
                    sql.CerrarConnection(conn);

                    respuesta.Respuesta = "NOK";
                    respuesta.Descripcion = "";
                    respuesta.Detalle_Error = "ups, no se logro agregar la Marca"+ex.Message;

                    return respuesta;
                }
            }


        

        public string TraerSucursal()
        {
            //string extension;
            List<Sucursal> listaSucursal = new List<Sucursal>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_SUCURSALES_SUCURSAL", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorSuc = 0;
                    while (reader.Read())
                    {
                        Sucursal sucursal = new Sucursal();
                        sucursal.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"].ToString());
                        sucursal.NOMBRE_SUCURSAL = reader["NOMBRE_SUCURSAL"].ToString();
                        sucursal.DIRECCION = reader["DIRECCION"].ToString();
                        sucursal.TELEFONO = reader["TELEFONO"].ToString();
                        //sucursal.TIPO_ARCHIVO = reader["TIPO_ARCHIVO"].ToString();

                        //extension = sucursal.TIPO_ARCHIVO;
                        //byte[] img = (byte[])reader["IMAGEN"];
                        //string IMG= Base64Encoder(img);
                        //sucursal.IMAGEN = extension + ","+IMG;

                        
                        
                        listaSucursal.Add(sucursal);
                        contadorSuc++;
                    }

                    if (contadorSuc > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaSucursal, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron marcas";
                    }

                }
            }
            catch (Exception ex)
            {
                respuestaServicio.Respuesta = "NOK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = ex.Message;
            }

            string respuesta = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
            return respuesta;

        }

        public string guardarEnCarpeta(byte[] imagen, string nombreSuc, string extension)
        {
            string url = Server.MapPath("~/imagenesGuardar/imagenesSucursal");

            bool saberExistenciaCarpeta;

            saberExistenciaCarpeta = Directory.Exists(url + "/" + nombreSuc);
            if (!saberExistenciaCarpeta)
            {
                Directory.CreateDirectory(url + "/" + nombreSuc);
            }
            else
            {
                Directory.Delete(url + "/" + nombreSuc, true);
                Directory.CreateDirectory(url + "/" + nombreSuc);
            }

            using (Image image = Image.FromStream(new MemoryStream(imagen)))
            {
                image.Save(url + "/" + nombreSuc + "/" + nombreSuc + ".jpg", ImageFormat.Jpeg);
            }
            

            return "";
        }
    }

    
}
        

