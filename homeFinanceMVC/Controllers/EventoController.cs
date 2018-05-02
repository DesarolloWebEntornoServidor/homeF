using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using homeFinanceMVC.Models;
using PagedList;

namespace homeFinanceMVC.Controllers
{
    public class EventoController : Controller
    {
        private EvActivoPasivoDAO evDAO = new EvActivoPasivoDAO();
        private EvActivoPasivo ev = new EvActivoPasivo();

        // GET: Evento
        public ActionResult InsertarEvento()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GuardarDatos(EvActivoPasivo evento)
        {
            if (!ModelState.IsValid)
            {
                return View("InsertarEvento", evento);
            }

            TempData["mesaje"] = "mensagem Ok";


            if (evento.TipoEvento == "deb")
                evento.TipoEvento = "D";
            else
                evento.TipoEvento = "C";

            evDAO.Insertar(evento);

            return RedirectToAction("InsertarEvento", "Evento");
        }

        public ActionResult ManupularDatosEvento(int? page)
        {
            List<EvActivoPasivo> listaEventos = evDAO.ListarEvActivoPasivos();

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(listaEventos.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult DeleteEvento(string id)
        {
            ev = evDAO.ObtenerEvento(Convert.ToInt32(id));

            return View(ev);
        }

        public ActionResult DelEvento(string id)
        {
            ev = evDAO.ObtenerEvento(Convert.ToInt32(id));

            evDAO.Borrar(Convert.ToInt32(id));

            TempData["mesajeDelete"] = "mensagem Ok";

            return RedirectToAction("ManupularDatosEvento", "Evento");

        }

        public ActionResult EditEvento(string id)
        {
            ev = evDAO.ObtenerEvento(Convert.ToInt32(id));

            return View(ev);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditEvento(FormCollection form, EvActivoPasivo evento)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEvento", ev);
            }

            TempData["mesaje"] = "mensagem Ok";

            ev = evDAO.ObtenerEvento(Convert.ToInt32(form["IdEvento"]));

            ev.DescEvento = evento.DescEvento;
            ev.TipoEvento = evento.TipoEvento;

            evDAO.Actualizar(ev);

            Log lg = new Log(String.Format("Se ha modificado el Evento de nº {0}", ev.IdEvento));

            LogDAO lDAO = new LogDAO();

            lDAO.Insertar(lg);

            return RedirectToAction("ManupularDatosEvento", "Evento");

        }
    }
}