using System;
using System.Collections.Generic;
using Rotativa;
using System.Web.Mvc;
using homeFinanceMVC.Models;
using PagedList;

namespace homeFinanceMVC.Views
{
    public class CuentaController : Controller
    {
        private CuentaDAO ccDAO = new CuentaDAO();
        private Cuenta cc = new Cuenta();

        private UsuarioDAO uDAO = new UsuarioDAO();
        private Usuario u = new Usuario();

        List<Cuenta> listaCuentas = null;

        public ActionResult InsertarCuenta()
        {
            List<Usuario> lUsuarios = new List<Usuario>();

            if (Convert.ToInt32(Session["tipoUsu"]) == 2)
            {
                u = new Usuario(Convert.ToInt32(Session["idUsu"]), Session["nombreusu"].ToString());
                lUsuarios.Add(u);
            }
            else
            {
                lUsuarios = uDAO.ListarUsuarios();
                u = new Usuario("Select un Usuario");
                lUsuarios.Insert(0, u);
            }

            ViewBag.listaUsuarios = lUsuarios;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GuardarDatos(FormCollection form, Cuenta cuenta)
        {
            List<Cuenta> listaCuentas = ccDAO.ListarCuentasPorId(Convert.ToInt32(Session["idUsu"]));

            if (Convert.ToInt32(Session["tipoUsu"]) == 2)
            {
                cuenta.IdUsuario = Convert.ToInt32(Session["idUsu"]);
            }

            if (!ModelState.IsValid)
            {
                List<Usuario> lUsuarios = new List<Usuario>();
                lUsuarios = uDAO.ListarUsuarios();
                u = new Usuario("Select un Usuario");
                lUsuarios.Insert(0, u);
                ViewBag.listaUsuarios = lUsuarios;

                return View("InsertarCuenta", cuenta);
            }

            TempData["mesaje"] = "mensagem Ok";

            ccDAO.Insertar(cuenta);

            return RedirectToAction("InsertarCuenta", "Cuenta");
        }

        public ActionResult ManupularDatosCuenta(int? page)
        {
            List<Cuenta> listaCuentas = ccDAO.ListarCuentasPorId(Convert.ToInt32(Session["idUsu"]));

            if (Convert.ToInt32(Session["tipoUsu"]) == 1)
            {
                listaCuentas = ccDAO.ListarCuentas();
            }

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(listaCuentas.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult DeleteCuenta(string id)
        {
            cc = ccDAO.ObtenerCuenta(Convert.ToInt32(id));

            return View(cc);
        }

        public ActionResult DelCuenta(string id)
        {
            cc = ccDAO.ObtenerCuenta(Convert.ToInt32(id));

            ccDAO.Borrar(Convert.ToInt32(id));

            TempData["mesajeDelete"] = "mensagem Ok";

            return RedirectToAction("ManupularDatosCuenta", "Cuenta");

        }

        public ActionResult EditCuenta(string id)
        {
            cc = ccDAO.ObtenerCuenta(Convert.ToInt32(id));

            List<Usuario> lUsuarios = new List<Usuario>();
            lUsuarios = uDAO.ListarUsuarios();
            ViewBag.listaUsuarios = lUsuarios;

            return View(cc);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCuenta(FormCollection form, Cuenta cc)
        {
            if (!ModelState.IsValid)
            {
                List<Usuario> lUsuarios = new List<Usuario>();
                lUsuarios = uDAO.ListarUsuarios();
                ViewBag.listaUsuarios = lUsuarios;

                return View("EditCuenta", cc);
            }

            TempData["mesaje"] = "mensagem Ok";

            cc = ccDAO.ObtenerCuenta(Convert.ToInt32(form["IdCuenta"]));

            cc.DescCuenta = form["descCuenta"].ToString();
            cc.IdUsuario = Convert.ToInt32(form["idUsuario"]);

            ccDAO.Actualizar(cc);

                Log lg = new Log(String.Format("Se ha modificado el la Cuenta de nº {0}", cc.IdCuenta));

                LogDAO lDAO = new LogDAO();

                lDAO.Insertar(lg);

            return RedirectToAction("ManupularDatosCuenta", "Cuenta");

        }

        public ActionResult ImprimirCuentas(int? page)
        {

            int pageSize = 50000;
            int pageNumber = (page ?? 1);

            return View(ccDAO.ListarCuentas().ToPagedList(pageNumber, pageSize));
        }

        //public ActionResult GenerarPDF()
        //{
        //    var q = new ActionAsPdf("ImprimirCuentas");

        //    return q;
        //}

        private RelatorioDuplicata getRelatorio()
        {
            var rpt = new RelatorioDuplicata();
            rpt.BasePath = Server.MapPath("/");

            rpt.PageTitle = "Lista de Cuentas";
            rpt.PageTitle = "Lista de Cuentas";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = true;

            return rpt;
        }
        public ActionResult Preview()
        {
            var rpt = getRelatorio();

            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        //public FileResult BaixarPDF()
        //{
        //    var rpt = getRelatorio();

        //    return File(rpt.GetOutput().GetBuffer(), "application/pdf", "Documento.pdf");
        //}
    }
}