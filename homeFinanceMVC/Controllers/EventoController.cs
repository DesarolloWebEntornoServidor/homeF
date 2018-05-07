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
        public ActionResult GuardarDatosEvento(EvActivoPasivo evento)
        {
            if (!ModelState.IsValid)
            {
                return View("InsertarEvento", evento);
            }

            TempData["mesaje"] = "mensagem Ok";

            int retorno = evDAO.Insertar(evento);

            if (retorno == 3)
            {
                TempData["noConecta"] = "BBDD no Conectada !!!";
            }
            
              return RedirectToAction("InsertarEvento", "Evento");
        }

        public ActionResult ManupularDatosEvento(int? page)
        {
            List<EvActivoPasivo> listaEventos = evDAO.ListarTodosEventos();

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
            evDAO.BorarRegistro(Convert.ToInt32(id));

            TempData["mesajeDelete"] = "mensagem Ok";

            return RedirectToAction("ManupularDatosEvento", "Evento");

        }

        public ActionResult EditEvento(string id)
        {
            ev = evDAO.ObtenerEvento(Convert.ToInt32(id));

            return View(ev);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditEvento(EvActivoPasivo evento)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEvento", evento);
            }

            EvActivoPasivo ev2 = evDAO.ObtenerEvento(evento.IdEvento);
          //  EvActivoPasivo ev2 = evDAO.ObtenerEvento(Convert.ToInt32(form["IdEvento"]));

            ev2.DescEvento = evento.DescEvento;
            ev2.TipoEvento = evento.TipoEvento;

            evDAO.Actualizar(ev2);

            Log lg = new Log(String.Format("Se ha modificado el Evento de nº {0}", ev2.IdEvento));

            LogDAO lDAO = new LogDAO();

            lDAO.Insertar(lg);

            TempData["mesaje"] = "mensagem Ok";

            return RedirectToAction("ManupularDatosEvento", "Evento");

        }
    }
}