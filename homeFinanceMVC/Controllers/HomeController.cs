﻿using System;
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Grafico()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeneraGrafico(FormCollection form)
        {

            string deForm = form["de"].ToString();
            string hastaForm = form["hasta"].ToString();

            string tp = form["rbTipo"].ToString();

            if (tp == "desp")
                Session["tipo"] = "desp";
            else
                Session["tipo"] = "rece";

            Session["de"] = deForm;
            Session["hasta"] = hastaForm;

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

            var dataforchart = data.Select(x => new { name = x.DescMov, y = x.Valor });

            return Json(dataforchart, JsonRequestBehavior.AllowGet);
        }

  
    }
}