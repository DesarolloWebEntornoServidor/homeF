using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using homeFinanceMVC.Models;

namespace homeFinanceMVC.Controllers
{
    public class HomeController : Controller
    {
        private EvActivoPasivoDAO evDAO = new EvActivoPasivoDAO();
        private EvActivoPasivo ev = new EvActivoPasivo();

        private CuentaDAO ccDAO = new CuentaDAO();
        private Cuenta cc = new Cuenta();

        private LogDAO lDAO = new LogDAO();
        private Log lg = new Log();

        private MovimientoDAO mDAO = new MovimientoDAO();
        private Movimiento mov = new Movimiento();

        private CajaGeneralDAO cDAO = new CajaGeneralDAO();
        private CajaGeneral caja = new CajaGeneral();

        public ActionResult Index()
        {

            if (Session["idUsu"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<Log> listaLogs = lDAO.ListarLogs();

            ViewBag.ListaLogs = listaLogs;
            ViewBag.controlLista = 1;


            return View(listaLogs);
        }

        public ActionResult IndexUsuario()
        {

            if (Session["idUsu"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        public ActionResult Grafico()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeneraGrafico(FormCollection form)
        {
            string tp = form["rbTipo"].ToString();

            if (tp == "desp")
                Session["tipo"] = "desp";
            else
                Session["tipo"] = "rece";

            string deForm = form["de"].ToString();
            string hastaForm = form["hasta"].ToString();

            if (deForm == "" || hastaForm == "")
            {
                Session["de"] = "";
                Session["hasta"] = "";

                TempData["NoGenera"] = "Sin Valores para esa Fecha";

                return View();
            }       

            Session["de"] = deForm;
            Session["hasta"] = hastaForm;

            string tipo = "D";

            if (tp == "rece")
                tipo = "C";

            var data = mDAO.ObtenerValoresDelGrafico(deForm, hastaForm, tipo);

            if (data.Count == 0)
            {
                TempData["NoGenera"] = "Sin Valores para esa Fecha";
            }

            return View();

        }

        public ActionResult GetCharData()
        {
            string deForm = Session["de"].ToString();
            string hastaForm = Session["hasta"].ToString();
            string tp = Session["tipo"].ToString();

            string tipo = "D";

            if (tp == "rece")
                tipo = "C";

            var data = mDAO.ObtenerValoresDelGrafico(deForm, hastaForm, tipo);           

            //if (data.Count == 0)
            //{
            //    TempData["mesajeGrafico"] = "Sin Valores para esa Fecha";
            //    return RedirectToAction("Grafico", "Home");
            //}

            var dataforchart = data.Select(x => new { name = x.DescMov, y = x.Valor });

            return Json(dataforchart, JsonRequestBehavior.AllowGet);
        }

  
    }
}