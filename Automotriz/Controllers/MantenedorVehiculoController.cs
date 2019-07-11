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
    public class MantenedorVehiculoController : Controller
    {
        // GET: MantenedorVehiculo
        public ActionResult Index()
        {
            
            return View();
        }

        public RespuestaServicio Editar()
        {
            RespuestaServicio respuestaServicio = new RespuestaServicio();


            return respuestaServicio;
        }
        public static string Base64Encoder(byte[] plainText)//PASA BASE 64 A STR
        {
            string strIMG = System.Convert.ToBase64String(plainText);
            return strIMG;
        }

        public static byte[] Base64Decoder(string base64EncodedData) //PASA BASE64 A BYTES[]
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }
        [HttpPost]
        public string GuardarVehiculo(Vehiculos vehiculo,List<ImagenVehiculo> imagenes)
        {
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            SqlConnection conn = null;
            SQLconn sql = new SQLconn();
            int ID_AUTO_INGRESADO = 0;

            try
            {

                conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand command = new SqlCommand("INSERTAR_VEHICULO_VEHICULOS", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID_SUCURSAL", vehiculo.ID_SUCURSAL);
                    command.Parameters.AddWithValue("@ID_USUARIOS", vehiculo.ID_USUARIOS);
                    command.Parameters.AddWithValue("@ID_DISPONIBILIDAD", vehiculo.ID_DISPONIBILIDAD);
                    command.Parameters.AddWithValue("@ID_MARCA", vehiculo.ID_MARCA);
                    command.Parameters.AddWithValue("@ID_TIPOTRACCION", vehiculo.ID_TIPOTRACCION);
                    command.Parameters.AddWithValue("@ID_TIPO_CONSIGNACION", vehiculo.ID_TIPO_CONSIGNACION);
                    command.Parameters.AddWithValue("@ID_COLOR", vehiculo.ID_COLOR);
                    command.Parameters.AddWithValue("@ID_TIPO_TRANSMICION", vehiculo.ID_TIPO_TRANSMICION);
                    command.Parameters.AddWithValue("@ID_TIPO_VEHICULO", vehiculo.ID_TIPO_VEHICULO);
                    command.Parameters.AddWithValue("@ID_TIPO_COMBUSTIBLE", vehiculo.ID_TIPO_COMBUSTIBLE);
                    command.Parameters.AddWithValue("@ID_ESTADO", vehiculo.ID_ESTADO);
                    command.Parameters.AddWithValue("@ID_MODELO", vehiculo.ID_MODELO);
                    command.Parameters.AddWithValue("@PATENTE", vehiculo.PATENTE);
                    command.Parameters.AddWithValue("@FECHA_INGRESO", vehiculo.FECHA_INGRESO);
                    command.Parameters.AddWithValue("@VERSION", vehiculo.VERSION);
                    command.Parameters.AddWithValue("@MOTOR", vehiculo.MOTOR);
                    command.Parameters.AddWithValue("@ANO", vehiculo.ANO);
                    command.Parameters.AddWithValue("@CILINDRADA", vehiculo.CILINDRADA);
                    command.Parameters.AddWithValue("@PRECIO_VENTA", vehiculo.PRECIO_VENTA);
                    command.Parameters.AddWithValue("@PRECIO_COMPRA", vehiculo.PRECIO_COMPRA);
                    command.Parameters.AddWithValue("@PRECIO_CONSIGNACION", vehiculo.PRECIO_CONSIGNACION);
                    command.Parameters.AddWithValue("@PRECIO_MINIMO_VENTA", vehiculo.PRECIO_MINIMO_VENTA);
                    command.Parameters.AddWithValue("@KILOMETRAJE", vehiculo.KILOMETRAJE);
                    command.Parameters.AddWithValue("@CHASIS", vehiculo.CHASIS);
                    command.Parameters.AddWithValue("@CANTIDAD_DUENIOS", vehiculo.CANTIDAD_DUENIOS);
                    //command.Parameters.AddWithValue("@IMAGEN", img);


                    //Metodo incesario xd
                    //traerIdVehiculo(vehiculo.PATENTE,imagen);


                    //se ejecuta el metodo y toma la respuesta 
                    SqlDataReader reader = command.ExecuteReader();

                    //lee la respuesta que envia 
                    while (reader.Read())
                    {
                        //tomo el valor de de la bd (ve el procedimiento almacenado INSERTAR_VEHICULO_VEHICULOS )
                        ID_AUTO_INGRESADO = Int32.Parse(reader["ID_AUTO"].ToString());
                    }
                    
                    //verificio que la lista de imagenes no venga vacia (si no viene vacia ingresara imagenes)
                    if (imagenes.Count != 0)
                    {
                        int ordenImagenes = 1;
                        foreach (var imagen in imagenes)
                        {

                            InsertarImagenVehiculo(imagen, ID_AUTO_INGRESADO, ordenImagenes);
                            ordenImagenes++;
                        }

                    }
                    sql.CerrarConnection(conn);
                    respuestaServicio.Respuesta = "OK";
                    respuestaServicio.Descripcion = "";
                    respuestaServicio.Detalle_Error = "Se agrego correctamente el Vehiculo";

                }
            }
            catch (Exception ex)
            {
                sql.CerrarConnection(conn);
                respuestaServicio.Respuesta = "NOK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = "ups, no se logro agregar el Vehiculo" + ex.Message;

            }
            string respuesta = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
            return respuesta;
        }

        public string traerVehiculoPorID(Vehiculos vehiculos)
        {

            Vehiculos nuevoVehiculo = new Vehiculos();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_VEHICULO_POR_ID", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_VEHICULO", vehiculos.ID_VEHICULO);

                    SqlDataReader reader = command.ExecuteReader();
                    int contadorVehiculo = 0;
                    while (reader.Read())
                    {

                        nuevoVehiculo.ID_VEHICULO = Convert.ToInt32(reader["ID_VEHICULO"]);
                        nuevoVehiculo.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"]);
                        nuevoVehiculo.ID_DISPONIBILIDAD = Convert.ToInt32(reader["ID_DISPONIBILIDAD"]);
                        nuevoVehiculo.ID_MARCA = Convert.ToInt32(reader["ID_MARCA"]);
                        nuevoVehiculo.ID_TIPOTRACCION = Convert.ToInt32(reader["ID_TIPOTRACCION"]);
                        nuevoVehiculo.ID_TIPO_CONSIGNACION = Convert.ToInt32(reader["ID_TIPO_CONSIGNACION"]);
                        nuevoVehiculo.ID_COLOR = Convert.ToInt32(reader["ID_COLOR"]);
                        nuevoVehiculo.ID_TIPO_TRANSMICION = Convert.ToInt32(reader["ID_TIPO_TRANSMICION"]);
                        nuevoVehiculo.ID_TIPO_COMBUSTIBLE = Convert.ToInt32(reader["ID_TIPO_COMBUSTIBLE"]);
                        nuevoVehiculo.ID_ESTADO = Convert.ToInt32(reader["ID_ESTADO"]);
                        nuevoVehiculo.ID_MODELO = Convert.ToInt32(reader["ID_MODELO"]);
                        nuevoVehiculo.ID_USUARIOS =  Convert.ToInt32(reader["ID_USUARIOS"]);
                        nuevoVehiculo.ID_TIPO_VEHICULO = Convert.ToInt32(reader["ID_TIPO_VEHICULO"]);
                        nuevoVehiculo.PATENTE = reader["PATENTE"].ToString();
                        nuevoVehiculo.FECHA_INGRESO =(DateTime)reader["FECHA_INGRESO"];
                        nuevoVehiculo.VERSION = reader["VERSION"].ToString();
                        nuevoVehiculo.MOTOR = reader["MOTOR"].ToString();
                        nuevoVehiculo.ANO = Convert.ToInt32(reader["ANO"]);
                        nuevoVehiculo.CILINDRADA = reader["CILINDRADA"].ToString();
                        nuevoVehiculo.PRECIO_VENTA = Convert.ToInt32(reader["PRECIO_VENTA"]);
                        nuevoVehiculo.PRECIO_COMPRA = Convert.ToInt32(reader["PRECIO_COMPRA"]);
                        nuevoVehiculo.PRECIO_CONSIGNACION = Convert.ToInt32(reader["PRECIO_CONSIGNACION"]);
                        nuevoVehiculo.PRECIO_MINIMO_VENTA = Convert.ToInt32(reader["PRECIO_MINIMO_VENTA"]);
                        nuevoVehiculo.KILOMETRAJE = Convert.ToInt32(reader["KILOMETRAJE"]);
                        nuevoVehiculo.CHASIS = reader["CHASIS"].ToString();
                        nuevoVehiculo.CANTIDAD_DUENIOS = Convert.ToInt32(reader["KILOMETRAJE"]);

                        contadorVehiculo++;
                    }

                    if (contadorVehiculo > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(nuevoVehiculo, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información del Vehiculo";
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

        public string traerVehiculoPorPatente(string PATENTE)
        {

            List<Vehiculos> listaVehiculo = new List<Vehiculos>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            string jsonRespuestaServicio = "";
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_VEHICULO_POR_PATENTE", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PATENTE", PATENTE);

                    SqlDataReader reader = command.ExecuteReader();
                    Vehiculos vehiculo = null;
                    while (reader.Read())
                    {
                        vehiculo = new Vehiculos();
                        vehiculo.ID_VEHICULO = Convert.ToInt32(reader["ID_VEHICULO"]);

                    }
                    if (vehiculo != null)
                    {
                        string JsonVehiculo = JsonConvert.SerializeObject(vehiculo, Formatting.Indented);
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonVehiculo;
                        respuestaServicio.Detalle_Error = "Se encontro vehiculo Correctamente";
                        jsonRespuestaServicio = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "No se encontro ningun Vehiculo con la patente: " + PATENTE;
                        jsonRespuestaServicio = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
                    }


                }
            }
            catch (Exception ex)
            {
                respuestaServicio.Respuesta = "NOK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = "Uuups, no se logro buscar vehiculo con patente, Error: " + ex.Message;
                jsonRespuestaServicio = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
            }


            return jsonRespuestaServicio;

        }


        public string InsertarImagenVehiculo(ImagenVehiculo imagen, int idVehiculo, int orden)
        {
            List<ImagenVehiculo> listaImagen = new List<ImagenVehiculo>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            SqlConnection conn = null;
            SQLconn sql = new SQLconn();

            try
            {
                string imagenCompleta = imagen.IMAGEN;
                string[] IMAGENarr = imagenCompleta.Split(',');
                string imagenSinExtension = IMAGENarr[1];
                imagen.TIPO_IMAGEN = IMAGENarr[0];
                imagen.EXTENSION = IMAGENarr[0];

                byte[] imgBaseDatos = Base64Decoder(imagenSinExtension);

                conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("INSERTAR_IMAGEN_IMAGENES_VEHICULO", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID_VEHICULO", idVehiculo);
                    command.Parameters.AddWithValue("@IMAGEN", imgBaseDatos);
                    command.Parameters.AddWithValue("@TIPO_IMAGEN", imagen.TIPO_IMAGEN);
                    command.Parameters.AddWithValue("@VIGENCIA", imagen.VIGENCIA);
                    command.Parameters.AddWithValue("@EXTENSION", imagen.EXTENSION);
                    command.Parameters.AddWithValue("@ORDEN_IMAGEN", orden);


                    //guardarEnCarpeta(img, ID_MARCA);



                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    }

                }
                sql.CerrarConnection(conn);
                respuestaServicio.Respuesta = "OK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = "Se agrego correctamente El autito";

            }
            catch (Exception ex)
            {
                sql.CerrarConnection(conn);
                respuestaServicio.Respuesta = "NOK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = "ups, no se logro agregar el autito" + ex.Message;

            }
            string respuesta = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
            return respuesta;
        }


        public void obtenerImagenPrincipalVehiculo(Vehiculos vehiculos)
        {
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_IMAGEN_PRINCIPAL", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_VEHICULO", vehiculos.ID_VEHICULO);

                    SqlDataReader reader = command.ExecuteReader();
                    ImagenVehiculo imagenVehiculo = null;
                    while (reader.Read())
                    {
                        byte[] imagenArray;
                        string tipoArchivo = "";
                        string base64 = "";
                        imagenVehiculo = new ImagenVehiculo();



                        imagenVehiculo.TIPO_IMAGEN = reader["TIPO_IMAGEN"].ToString();
                        imagenArray = (byte[])reader["IMAGEN"];

                        tipoArchivo = imagenVehiculo.TIPO_IMAGEN;
                        base64 = Convert.ToBase64String(imagenArray);
                        imagenVehiculo.IMAGEN = tipoArchivo + "," + base64;

                    }
                    if (imagenVehiculo != null)
                    {
                        vehiculos.IMAGEN_PRINCIPAL = imagenVehiculo.IMAGEN;
                    }
                    else
                    {
                        vehiculos.IMAGEN_PRINCIPAL = "~/img/autito.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                vehiculos.IMAGEN_PRINCIPAL = "~/img/autito.jpg";
            }
        }

        public void obetenerTodasLasImagenes(Vehiculos vehiculos)
        {
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_IMAGENES_VEHICULO", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_VEHICULO", vehiculos.ID_VEHICULO);

                    SqlDataReader reader = command.ExecuteReader();


                    List<ImagenVehiculo> imagenesVehiculos = new List<ImagenVehiculo>();


                    while (reader.Read())
                    {
                        byte[] imagenArray;
                        string tipoArchivo = "";
                        string base64 = "";
                        ImagenVehiculo imagenVehiculo = new ImagenVehiculo();



                        imagenVehiculo.TIPO_IMAGEN = reader["TIPO_IMAGEN"].ToString();
                        imagenArray = (byte[])reader["IMAGEN"];

                        tipoArchivo = imagenVehiculo.TIPO_IMAGEN;
                        base64 = Convert.ToBase64String(imagenArray);
                        imagenVehiculo.IMAGEN = tipoArchivo + "," + base64;

                        imagenesVehiculos.Add(imagenVehiculo);
                    }

                    vehiculos.IMAGENES_VEHICULO = imagenesVehiculos;


                }
            }
            catch (Exception ex)
            {
                List<ImagenVehiculo> imagenesVehiculos = new List<ImagenVehiculo>();
                vehiculos.IMAGENES_VEHICULO = imagenesVehiculos;

            }
        }


      public string TraerVehiculo()
        {
            List<Vehiculos> listaVehiculo = new List<Vehiculos>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();

            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_PREVIEW_VEHICULOS", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();


                    int contadorVehiculo = 0;

                    while (reader.Read())
                    {
                        Vehiculos vehiculo = new Vehiculos();
                        vehiculo.ID_VEHICULO = Convert.ToInt32(reader["ID_VEHICULO"]);
                        vehiculo.NOMBRE_MODELO= reader["NOMBRE_MODELO"].ToString();
                        vehiculo.PATENTE = reader["PATENTE"].ToString();
                        vehiculo.NOMBRE_TRANSMISION = reader["TRANSMISION"].ToString();
                        vehiculo.PRECIO_COMPRA = Convert.ToInt32(reader["PRECIO_COMPRA"]);

                        //obtenerImagenPrincipalVehiculo(vehiculo);


                        listaVehiculo.Add(vehiculo);
                        contadorVehiculo++;
                    }

                    
                    if (contadorVehiculo > 0)
                    {
                        sql.CerrarConnection(conn);

                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaVehiculo, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        sql.CerrarConnection(conn);

                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información del Vehiculo";
                    }

                    sql.CerrarConnection(conn);


                }

                sql.CerrarConnection(conn);

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

        public string FiltrarVehiculo(Vehiculos vehiculo)
        {
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            SqlConnection conn = null;
            SQLconn sql = new SQLconn();
            int ID_AUTO_INGRESADO = 0;
            string jsonRespuestaServicio = "";

            try
            {

                conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {

                    SqlCommand command = new SqlCommand("TRAER_VEHICULOS_FILTRADOS", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ID_MARCA", vehiculo.ID_MARCA);
                    command.Parameters.AddWithValue("@ID_SUCURSAL", vehiculo.ID_SUCURSAL);
                    command.Parameters.AddWithValue("@ID_TRANSMISION", vehiculo.ID_TIPO_TRANSMICION);
                    command.Parameters.AddWithValue("@ID_TRACCION", vehiculo.ID_TIPOTRACCION);
                    command.Parameters.AddWithValue("@ID_TIPO_COMBUSTIBLE", vehiculo.ID_TIPO_COMBUSTIBLE);
                    command.Parameters.AddWithValue("@PRECIO_MINIMO", vehiculo.PRECIO_MINIMO_VENTA);

                    SqlDataReader reader = command.ExecuteReader();

                    List<Vehiculos> ListaDeVehiculos = new List<Vehiculos>();
                    //lee la respuesta que envia 
                    while (reader.Read())
                    {
                        Vehiculos VueltaVehiculo = new Vehiculos();

                        VueltaVehiculo.ID_VEHICULO = Int32.Parse(reader["ID_VEHICULO"].ToString());
                        VueltaVehiculo.PATENTE = reader["PATENTE"].ToString();
                        VueltaVehiculo.ANO = Int32.Parse(reader["ANO"].ToString());
                        VueltaVehiculo.PRECIO_COMPRA = Int32.Parse(reader["PRECIO_COMPRA"].ToString());
                        VueltaVehiculo.NOMBRE_MODELO = reader["NOMBRE_MODELO"].ToString();
                        VueltaVehiculo.NOMBRE_TRANSMISION = reader["NOMBRE_TRANSMISION"].ToString();

                        ListaDeVehiculos.Add(VueltaVehiculo);
                    }

                    sql.CerrarConnection(conn);


                    if (ListaDeVehiculos.Count > 0)
                    {
                        string jsonListaVehiculos = JsonConvert.SerializeObject(ListaDeVehiculos, Formatting.Indented);

                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = jsonListaVehiculos;
                        respuestaServicio.Detalle_Error = "Se encontraton vehiculos Perfectamente";
                        jsonRespuestaServicio = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "No se encontraron Vehiculos con los filtros seleccionados";

                        jsonRespuestaServicio = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
                    }

                }
            }
            catch (Exception ex)
            {
                sql.CerrarConnection(conn);
                respuestaServicio.Respuesta = "NOK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = "ups, no se lograron buscar vehiculos" + ex.Message;
                jsonRespuestaServicio = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
            }

            return jsonRespuestaServicio;
        }

        public string BusquedaVehiculoMantenedor(Vehiculos vehiculo)
        {
            string JsonRespuesta = "";


            if (vehiculo.PATENTE != "" || vehiculo.PATENTE != null)
            {
                JsonRespuesta = traerVehiculoPorPatente(vehiculo.PATENTE);
            }
            else
            {

                JsonRespuesta = FiltrarVehiculo(vehiculo);
            }

            return JsonRespuesta;
        }


        //public RespuestaServicio EliminarVehiculo(int ID_VEHICULO)
        //{

        //    RespuestaServicio respuestaServicio = new RespuestaServicio();
        //    try
        //    {
        //        SQLconn sql = new SQLconn();
        //        SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

        //        if (conn.State == System.Data.ConnectionState.Open)
        //        {
        //            SqlCommand command = new SqlCommand("ELIMINAR_VEHICULO_VEHICULOS", conn);

        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@ID_VEHICULO", ID_VEHICULO);
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //            }


        //        }
        //        sql.CerrarConnection(conn);
        //        respuestaServicio.Respuesta = "OK";
        //        respuestaServicio.Descripcion = "Se elimino correctamente el Vehículo";
        //        respuestaServicio.Detalle_Error = "";
        //        return respuestaServicio;
        //    }
        //    catch (Exception ex)
        //    {
        //        respuestaServicio.Respuesta = "NOK";
        //        respuestaServicio.Descripcion = "";
        //        respuestaServicio.Detalle_Error = ex.Message;
        //        return respuestaServicio;
        //    }


        //}

        public string EditarVehiculo(Vehiculos vehiculo)
        {
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            SqlConnection conn = null;
            SQLconn sql = new SQLconn();


            try
            {
                //string[] IMAGENarr = IMAGEN.Split(',');
                //string arreglado = IMAGENarr[1];
                //byte[] img = Base64Decoder(arreglado);
                conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("EDITAR_VEHICULO_VEHICULOS", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_VEHICULO", vehiculo.ID_VEHICULO);
                    command.Parameters.AddWithValue("@ID_SUCURSAL", vehiculo.ID_SUCURSAL);
                    command.Parameters.AddWithValue("@ID_DISPONIBILIDAD", vehiculo.ID_DISPONIBILIDAD);
                    command.Parameters.AddWithValue("@ID_MARCA", vehiculo.ID_MARCA);
                    command.Parameters.AddWithValue("@ID_TIPOTRACCION", vehiculo.ID_TIPOTRACCION);
                    command.Parameters.AddWithValue("@ID_TIPO_CONSIGNACION", vehiculo.ID_TIPO_CONSIGNACION);
                    command.Parameters.AddWithValue("@ID_COLOR", vehiculo.ID_COLOR);
                    command.Parameters.AddWithValue("@ID_TIPO_TRANSMICION", vehiculo.ID_TIPO_TRANSMICION);
                    command.Parameters.AddWithValue("@ID_TIPO_COMBUSTIBLE", vehiculo.ID_TIPO_COMBUSTIBLE);
                    command.Parameters.AddWithValue("@ID_ESTADO", vehiculo.ID_ESTADO);
                    command.Parameters.AddWithValue("@ID_MODELO", vehiculo.ID_MODELO);
                    command.Parameters.AddWithValue("@ID_USUARIOS", vehiculo.ID_USUARIOS);
                    command.Parameters.AddWithValue("@ID_TIPO_VEHICULO", vehiculo.ID_TIPO_VEHICULO);
                    command.Parameters.AddWithValue("@PATENTE", vehiculo.PATENTE);
                    command.Parameters.AddWithValue("@FECHA_INGRESO", vehiculo.FECHA_INGRESO);
                    command.Parameters.AddWithValue("@VERSION", vehiculo.VERSION);
                    command.Parameters.AddWithValue("@MOTOR", vehiculo.MOTOR);
                    command.Parameters.AddWithValue("@ANO", vehiculo.ANO);
                    command.Parameters.AddWithValue("@CILINDRADA", vehiculo.CILINDRADA);
                    command.Parameters.AddWithValue("@PRECIO_VENTA", vehiculo.PRECIO_VENTA);
                    command.Parameters.AddWithValue("@PRECIO_COMPRA", vehiculo.PRECIO_COMPRA);
                    command.Parameters.AddWithValue("@PRECIO_CONSIGNACION", vehiculo.PRECIO_CONSIGNACION);
                    command.Parameters.AddWithValue("@PRECIO_MINIMO_VENTA", vehiculo.PRECIO_MINIMO_VENTA);
                    command.Parameters.AddWithValue("@KILOMETRAJE", vehiculo.KILOMETRAJE);
                    command.Parameters.AddWithValue("@CHASIS", vehiculo.CHASIS);
                    command.Parameters.AddWithValue("@CANTIDAD_DUENIOS", vehiculo.CANTIDAD_DUENIOS);
                    //command.Parameters.AddWithValue("@IMAGEN", img);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    }

                }
                sql.CerrarConnection(conn);
                respuestaServicio.Respuesta = "OK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = "Se editaron correctamente los datos del Vehiculo";
                
            }
            catch (Exception ex)
            {
                sql.CerrarConnection(conn);
                respuestaServicio.Respuesta = "NOK";
                respuestaServicio.Descripcion = "";
                respuestaServicio.Detalle_Error = " NO Se editaron correctamente los datos del Vehiculoe" + ex.Message;
                
            }
            string respuesta = JsonConvert.SerializeObject(respuestaServicio, Formatting.Indented);
            return respuesta;
        }


        public string TraerMarcas() { //traer todas las marcas de la base de datos presentes en la tabla Marca de forma serializada
            List<Marca> listaMarcas = new List<Marca>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_MARCAS_MARCA", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorMarcas = 0;
                    while (reader.Read())
                    {
                        Marca marcas = new Marca();
                        marcas.ID_MARCA = Convert.ToInt32(reader["ID_MARCA"].ToString());
                        marcas.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        listaMarcas.Add(marcas);
                        contadorMarcas++;
                    }

                    if (contadorMarcas > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaMarcas, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else {
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

        public string TraerCombustibles() //trae todos los tipos de combustibles de la base de datos presentes en la combustibles Marca de forma serializada
        {
            List<TipoCombustible> listaCombustibles = new List<TipoCombustible>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_COMBUSTIBLES_TIPO_COMBUSTIBLES", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorCombustibles = 0;
                    while (reader.Read())
                    {
                        TipoCombustible combustible = new TipoCombustible();
                        combustible.ID_TIPO_COMBUSTIBLE = Convert.ToInt32(reader["ID_TIPO_COMBUSTIBLE"].ToString());
                        combustible.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        listaCombustibles.Add(combustible);
                        contadorCombustibles++;
                    }
                    if (contadorCombustibles > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaCombustibles, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerTraccion() //trae todas las tracciones de la base de datos presentes en la tabla TIPO_TRACCION de forma serializada
        {
            List<TipoTraccion> listaTracciones = new List<TipoTraccion>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_TRACCIONES_TIPO_TRACCION", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorTraccion  = 0;
                    while (reader.Read())
                    {
                        TipoTraccion traccion = new TipoTraccion();
                        traccion.ID_TIPOTRACCION = Convert.ToInt32(reader["ID_TIPOTRACCION"].ToString());
                        traccion.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        listaTracciones.Add(traccion);
                        contadorTraccion++;
                    }
                    if (contadorTraccion > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaTracciones, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerTransmision() //trae todas las tracciones de la base de datos presentes en la tabla TIPO_TRACCION de forma serializada
        {
            List<TipoTransmicion> listaTransmision = new List<TipoTransmicion>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_TRANSMISION_TIPO_TRANSMICION", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorTransmision = 0;
                    while (reader.Read())
                    {
                        TipoTransmicion tipoTransmision = new TipoTransmicion();
                        tipoTransmision.ID_TIPO_TRANSMICION = Convert.ToInt32(reader["ID_TIPO_TRANSMICION"].ToString());
                        tipoTransmision.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        listaTransmision.Add(tipoTransmision);
                        contadorTransmision++;
                    }
                    if (contadorTransmision > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaTransmision, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerConsignacion() //trae todas las tracciones de la base de datos presentes en la tabla TIPO_TRACCION de forma serializada
        {
            List<TipoConsigna> listaConsignacion = new List<TipoConsigna>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_TIPO_CONSIGNACION_TIPO_CONSIGNACION", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        TipoConsigna tipoConsignacion = new TipoConsigna();
                        tipoConsignacion.ID_TIPO_CONSIGNACION = Convert.ToInt32(reader["ID_TIPO_CONSIGNACION"].ToString());
                        tipoConsignacion.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        listaConsignacion.Add(tipoConsignacion);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaConsignacion, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerDisponibilidad() //trae la disponibilidad de un vehiculo
        {
            List<Disponibilidad> listaDisponibilidad = new List<Disponibilidad>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_DISPONIBILIDAD_DISPONIBILIDAD", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        Disponibilidad disponibilidad = new Disponibilidad();
                        disponibilidad.ID_DISPONIBILIDAD = Convert.ToInt32(reader["ID_DISPONIBILIDAD"].ToString());
                        disponibilidad.ESTADO_DISPONIBILIDAD = reader["ESTADO_DISPONIBILIDAD"].ToString();


                        listaDisponibilidad.Add(disponibilidad);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaDisponibilidad, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontro disponibilidad";
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

        public string TraerEstadoVehiculo() //trae el estado de los vehiculos presentes en la tabla estado_vehiculo
        {
            List<EstadoVehiculo> ListaEstadoVehiculos = new List<EstadoVehiculo>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_ESTADO_ESTADO_VEHICULO", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        EstadoVehiculo estado = new EstadoVehiculo();
                        estado.ID_ESTADO = Convert.ToInt32(reader["ID_ESTADO"].ToString());
                        estado.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        ListaEstadoVehiculos.Add(estado);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(ListaEstadoVehiculos, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerColor() //trae el estado de los vehiculos presentes en la tabla estado_vehiculo
        {
            List<Color> listaColor = new List<Color>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_COLOR_COLOR", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        Color colorVehiculo = new Color();
                        colorVehiculo.ID_COLOR = Convert.ToInt32(reader["ID_COLOR"].ToString());
                        colorVehiculo.DESCRIPCION = reader["DESCRIPCION"].ToString();


                        listaColor.Add(colorVehiculo);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaColor, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron COLORES";
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

        public string TraerTipoVehiculo() //trae el tipo de vehiculo de los vehiculos presentes en la tabla tipo de vehiculo
        {
            List<TipoVehiculo> listaTipos = new List<TipoVehiculo>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_TIPO_VEHICULO_TIPO_VEHICULO", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        TipoVehiculo tipoVehiculo = new TipoVehiculo();
                        tipoVehiculo.ID_TIPO_VEHICULO = Convert.ToInt32(reader["ID_TIPO_VEHICULO"].ToString());
                        tipoVehiculo.TIPO_VEHICULO = reader["TIPO_VEHICULO"].ToString();


                        listaTipos.Add(tipoVehiculo);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaTipos, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerModeloVehiculo() //trae los modelos  de vehiculo de los vehiculos presentes en la tabla modelos vehiculos
        {
            List<ModeloVehiculo> listaModelos = new List<ModeloVehiculo>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_MODELOS_MODELOS_VEHICULO", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        ModeloVehiculo modelos = new ModeloVehiculo();
                        modelos.ID_MODELO = Convert.ToInt32(reader["ID_MODELO"].ToString());
                        modelos.NOMBRE_MODELO = reader["NOMBRE_MODELO"].ToString();
                        modelos.ID_MARCA = Convert.ToInt32(reader["ID_MARCA"].ToString());

                        listaModelos.Add(modelos);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaModelos, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron combustibles";
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

        public string TraerUsuarios() //trae el tipo de vehiculo de los vehiculos presentes en la tabla tipo de vehiculo
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                SQLconn sql = new SQLconn();
                SqlConnection conn = sql.AbrirConnection(sql.ObtenerConnection());

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("TRAER_USUARIOS_USUARIO", conn);

                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = command.ExecuteReader();
                    int contadorConsigna = 0;
                    while (reader.Read())
                    {
                        Usuarios usuarios = new Usuarios();
                        usuarios.ID_USUARIOS = Convert.ToInt32(reader["ID_USUARIOS"].ToString());
                        usuarios.ID_ROL = Convert.ToInt32(reader["ID_ROL"].ToString());
                        usuarios.ID_SUCURSAL = Convert.ToInt32(reader["ID_SUCURSAL"].ToString());
                        usuarios.RUT = reader["RUT"].ToString();
                        usuarios.EMAIL = reader["EMAIL"].ToString();
                        usuarios.PASSWORD = reader["PASSWORD"].ToString();
                       usuarios.NOMBRE_USUARIO = reader["NOMBRE_USUARIO"].ToString();


                        listaUsuarios.Add(usuarios);
                        contadorConsigna++;
                    }
                    if (contadorConsigna > 0)
                    {
                        respuestaServicio.Respuesta = "OK";
                        respuestaServicio.Descripcion = JsonConvert.SerializeObject(listaUsuarios, Formatting.Indented);
                        respuestaServicio.Detalle_Error = "";
                    }
                    else
                    {
                        respuestaServicio.Respuesta = "NOK";
                        respuestaServicio.Descripcion = "";
                        respuestaServicio.Detalle_Error = "Error al obtener información, no se encontraron Usuarios";
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

    }

    
}

