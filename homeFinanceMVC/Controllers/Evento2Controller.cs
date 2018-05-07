using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using homeFinanceMVC.Models;
using PagedList;

namespace homeFinanceMVC.Controllers
{
    public class Evento2Controller : Controller
    {
        private EvActivoPasivo e = new EvActivoPasivo();
        private EvActivoPasivoDAO eDAO = new EvActivoPasivoDAO();

        public ActionResult Evento2()
        {
            return View();
        }

        public ActionResult ManupularDatosEvento2(int? page)
        {

            //List<EvActivoPasivo> listaDeEventos = eDAO.ListarEvActivoPasivos();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(eDAO.ListarTodosEventos().ToPagedList(pageNumber, pageSize));
        }
    }
}