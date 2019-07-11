using Automotriz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Automotriz.Controllers
{
    public class TipoCombustibleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public string insertTipoCombustible(string descripcion, string vigencia)
        {
            TipoCombustible combustible = new TipoCombustible();

            combustible.DESCRIPCION = descripcion;
            combustible.VIGENCIA = Int32.Parse(vigencia);
            combustible.AgregarCombustible();

            return "";
        }
    }
}
