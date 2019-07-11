using Automotriz.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Automotriz.Controllers.Mantenedores
{
    public class MarcaController : Controller
    {
        // GET: Marca
        public ActionResult Index()
        {

            //***PROCESO DE VALIDACION AUTENTIFICACION + Session de USUARIO  *******************

            string nombreUsuario = "";
            nombreUsuario = (string)Session["NOMBRE_USUARIO"];
            ViewBag.NOMBRE_USUARIO = Session["NOMBRE_USUARIO"];
            ViewBag.IMAGEN = (string)Session["IMAGEN"];
            ViewBag.SUCURSAL = Session["SUCURSAL"];

            if (nombreUsuario == null || nombreUsuario=="")
            {
                return RedirectToAction("Login", "Home");
            }
            
            
            //**FIN PROCESO DE VALIDACION AUTENTIFICACION *******************

            
                       


            return View();
        }

        public string ObtieneListadoMarcas()
        {
            RespuestaServicio Respuesta = new RespuestaServicio();
            List<Marca> Lista_Marca = new List<Marca>();
            Marca Obj_Marca = new Marca();

            try
            {
                
                Lista_Marca = Obj_Marca.Consultar_Marcas();

                Respuesta.Respuesta = "OK";
                Respuesta.Descripcion = JsonConvert.SerializeObject(Lista_Marca, Formatting.Indented);
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

        public string ObtieneMarcaPorId( Marca marca)
        {
            RespuestaServicio Respuesta = new RespuestaServicio();
           

            try
            {

                marca= marca.Consulta_Marca_Por_Id(marca);

                Respuesta.Respuesta = "OK";
                Respuesta.Descripcion = JsonConvert.SerializeObject(marca, Formatting.Indented);
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

        public string Editar_Marca(Marca marca)
        {
            RespuestaServicio respuesta ;

            respuesta = marca.Editar_Marcas(marca);

            string resultado = JsonConvert.SerializeObject(respuesta, Formatting.Indented);
            return resultado;
        }

        public string AgregarMarca(Marca marca)
        {
            RespuestaServicio respuesta;

            respuesta = marca.AgregarMarca(marca);

            string resultado = JsonConvert.SerializeObject(respuesta, Formatting.Indented);
            return resultado;
        }

        public string EliminarMarca(Marca marca)
        {
            RespuestaServicio respuesta;

            respuesta = marca.EliminarMarca(marca);

            string resultado = JsonConvert.SerializeObject(respuesta, Formatting.Indented);
            return resultado;
        }


    }
}