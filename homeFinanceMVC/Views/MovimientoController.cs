using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using homeFinanceMVC.Models;
using PagedList;

namespace homeFinanceMVC.Views
{
    public class MovimientoController : Controller
    {
        private EvActivoPasivoDAO evDAO = new EvActivoPasivoDAO();
        private EvActivoPasivo ev = new EvActivoPasivo();

        private CuentaDAO ccDAO = new CuentaDAO();
        private Cuenta cc = new Cuenta();

        private MovimientoDAO mDAO = new MovimientoDAO();
        private Movimiento mov = new Movimiento();

        private CajaGeneralDAO cDAO = new CajaGeneralDAO();
        private CajaGeneral caja = new CajaGeneral();

        decimal saldo = 0;

        public decimal Saldo
        {
            get
            {
                return saldo;
            }

            set
            {
                saldo = value;
            }
        }
        public ActionResult Create()
        {
            Movimiento nuevoMov = new Movimiento();

            List<EvActivoPasivo> lEventos = new List<EvActivoPasivo>();
            lEventos = evDAO.ListarEvActivoPasivos();
            ev = new EvActivoPasivo("Select un Evento");
            lEventos.Insert(0, ev);
            ViewBag.listaEventos = lEventos;

            List<Cuenta> lCuentas = new List<Cuenta>();
            lCuentas = ccDAO.ListarCuentas();
            cc = new Cuenta("Select una Cuenta");
            lCuentas.Insert(0, cc);
            ViewBag.listaCuentas = lCuentas;


            saldo = cDAO.ConsultarSaldoActual();
            ViewBag.Saldo = saldo;

            DateTime dt = DateTime.Now;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GuardarDatos(Movimiento movimiento, FormCollection form)
        {
            //mov.DescMov = form["desc"].ToString();
            //mov.Fecha = form["fecha"].ToString();
            //mov.Valor = Convert.ToDouble(form["valor"]);
            //mov.IdEvento = Convert.ToInt32(form["evento"]);
            //mov.IdCuenta = Convert.ToInt32(form["cuenta"]);
            //mov.IdUsuario = Convert.ToInt32(Session["idUsu"]);

            if (!ModelState.IsValid)
            {
                List<EvActivoPasivo> lEventos = new List<EvActivoPasivo>();
                lEventos = evDAO.ListarEvActivoPasivos();
                ev = new EvActivoPasivo("Select un Evento");
                lEventos.Insert(0, ev);
                ViewBag.listaEventos = lEventos;

                List<Cuenta> lCuentas = new List<Cuenta>();
                lCuentas = ccDAO.ListarCuentas();
                cc = new Cuenta("Select una Cuenta");
                lCuentas.Insert(0, cc);
                ViewBag.listaCuentas = lCuentas;

                Saldo = cDAO.ConsultarSaldoActual();
                ViewBag.Saldo = Saldo;

                return View("Create", movimiento);
            }

            TempData["mesaje"] = "mensagem Ok";

            mDAO.Insertar(movimiento);

            decimal saldo = Convert.ToDecimal(form["saldo"]);
            decimal nuevoSaldo = 0;

            ev = evDAO.ObtenerEvento(movimiento.IdEvento);
            decimal valorActualizar = 0;

            if (ev.TipoEvento == "C")
            {
                nuevoSaldo = saldo + movimiento.Valor;
                valorActualizar = valorActualizar + movimiento.Valor;
            }
            else
            {
                valorActualizar = valorActualizar - movimiento.Valor;
                nuevoSaldo = saldo - movimiento.Valor;
            }

            caja = new CajaGeneral(movimiento.Fecha, nuevoSaldo);


            CajaGeneral cg = cDAO.LocalizarSaldo(movimiento.Fecha);
            if (cg.FechaSaldo == null)
                cDAO.Insertar(caja);
            else
            {
                caja.Saldo = saldo;

                cDAO.ActualizarSaldo(caja, valorActualizar);
            }

            return RedirectToAction("Create", "Movimiento");
        }

        public ActionResult LibroCaja()
        {
  
            return View();
        }

        [HttpPost]
        public ActionResult MostrarLibro(FormCollection form)
        {

            string deForm = form["de"].ToString();
            string hastaForm = form["hasta"].ToString();

            decimal saldo = cDAO.ConsultarSaldoLibroCaja(deForm);

            ViewBag.Saldo = saldo;

            List<Object> listaLibro = mDAO.MovimientosLibro(deForm, hastaForm);

            return View("MostrarLibro",listaLibro);

        }

        public ActionResult ManipularDatos()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MostrarMovimientos(FormCollection form)
        {

            string deForm = form["de"].ToString();
            string hastaForm = form["hasta"].ToString();



            Session["de"] = deForm;
            Session["hasta"] = hastaForm;

            return RedirectToAction("MostrarMovimientos2","Movimiento");

        }

        public ActionResult MostrarMovimientos2(int? page, string sortOrder)
        {
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha_desc" : "Fecha";

            string deForm = Session["de"].ToString();
            string hastaForm = Session["hasta"].ToString();

            decimal saldo = cDAO.ConsultarSaldoLibroCaja(deForm);

            List<EvActivoPasivo> listaEventos = evDAO.ListarEvActivoPasivos();

            ViewBag.ListaEventos = listaEventos;

            var listaMovimientos = from s in mDAO.ListaMovimientosPorPeriodo(deForm, hastaForm) select s;

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            switch (sortOrder)
            {
                case "fecha_desc":
                    listaMovimientos = listaMovimientos.OrderByDescending(s => s.Fecha);
                    break;
                default:
                    listaMovimientos = listaMovimientos.OrderByDescending(s => s.Fecha);
                    break;
            }

            return View(listaMovimientos.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult DeleteMovimiento(string id)
        {
            mov = mDAO.ObtenerMovimiento(Convert.ToInt32(id));

            return View(mov);
        }

            public ActionResult DelMovimiento(string id)
        {
            mov = mDAO.ObtenerMovimiento(Convert.ToInt32(id));

            mDAO.Borrar(Convert.ToInt32(id));

            ev = evDAO.ObtenerEvento(mov.IdEvento);
            decimal valorActualizar = 0;

            if (ev.TipoEvento == "C")
            {
                valorActualizar = valorActualizar - mov.Valor;
            }
            else
            {
                valorActualizar = valorActualizar + mov.Valor;
            }

            caja = new CajaGeneral(mov.Fecha, valorActualizar);

            CajaGeneral cg = cDAO.LocalizarSaldo(mov.Fecha);

            if (cg.FechaSaldo != null)
            {
                caja.Saldo = saldo;

                cDAO.ActualizarSaldo(caja, valorActualizar);
            }

            TempData["mesajeDelete"] = "mensagem Ok";

            return RedirectToAction("MostrarMovimientos2", "Movimiento");

        }

        public ActionResult EditMovimiento(string id)
        {
            mov = mDAO.ObtenerMovimiento(Convert.ToInt32(id));

            List<EvActivoPasivo> lEventos = new List<EvActivoPasivo>();
            lEventos = evDAO.ListarEvActivoPasivos();
            ViewBag.listaEventos = lEventos;

            List<Cuenta> lCuentas = new List<Cuenta>();
            lCuentas = ccDAO.ListarCuentas();
            ViewBag.listaCuentas = lCuentas;

            return View(mov);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditMovimiento(FormCollection form, Movimiento mov)
        {
            if (!ModelState.IsValid)
            {
                List<EvActivoPasivo> lEventos = new List<EvActivoPasivo>();
                lEventos = evDAO.ListarEvActivoPasivos();
                ViewBag.listaEventos = lEventos;

                List<Cuenta> lCuentas = new List<Cuenta>();
                lCuentas = ccDAO.ListarCuentas();
                ViewBag.listaCuentas = lCuentas;

                return View("EditMovimiento", mov);
            }

            TempData["mesaje"] = "mensagem Ok";

            mov = mDAO.ObtenerMovimiento(Convert.ToInt32(form["IdMov"]));

            decimal valorAnterior = mov.Valor;
            decimal valorActualizar = 0;

            string fechaAnterior = mov.Fecha;

            mov.DescMov = form["descMov"].ToString();
            mov.Fecha = form["fecha"].ToString();
            mov.Valor = Convert.ToDecimal(form["valor"]);
            mov.IdEvento = Convert.ToInt32(form["idEvento"]);
            mov.IdCuenta = Convert.ToInt32(form["idCuenta"]);

            mDAO.Actualizar(mov);

            ev = evDAO.ObtenerEvento(mov.IdEvento);

            if(valorAnterior - mov.Valor != 0) // se hay modificación el el valor, actualiza el saldo
            {
                if(ev.TipoEvento == "C")  // si es credito, se hace el calculo invertido 
                    valorActualizar = (mov.Valor - valorAnterior);
               else
                    valorActualizar = (valorAnterior - mov.Valor); // si es debito el calculo es normal

                caja = new CajaGeneral(mov.Fecha, valorActualizar);

                CajaGeneral cg = cDAO.LocalizarSaldo(mov.Fecha);

                cDAO.ActualizarSaldo(caja, valorActualizar);

                Log lg = new Log(String .Format("Se ha modificado el valor del movimiento nº {0}", mov.IdMov));

                LogDAO lDAO = new LogDAO();

                lDAO.Insertar(lg);

                //if (Convert.ToDateTime(this.txtStartDate.Text) > Convert.ToDateTime(this.txtEndDate.Text)) // comparar fechas en string

            }
            //else if(fechaAnterior != mov.Fecha)
            //{
            //    if (ev.TipoEvento == "C")  // si es credito, se hace el calculo invertido 
            //        valorActualizar = (mov.Valor - valorAnterior);
            //    else
            //        valorActualizar = (valorAnterior - mov.Valor); // si es debito el calculo es normal

            //    caja = new CajaGeneral(mov.Fecha, valorActualizar);

            //    CajaGeneral cg = cDAO.LocalizarSaldo(mov.Fecha);

            //    if (cg.FechaSaldo == null)
            //    {
            //        CajaGeneral cgCompara = cDAO.LocalizarNuevoSaldo(mov.Fecha); // see saldo no existe, coge el saldo seguinte

            //        cg = cDAO.LocalizarSaldo(cgCompara.FechaSaldo);

            //        if (cg.IdCaja == 1)
            //        {
            //            if (ev.TipoEvento == "C")
            //                caja.Saldo = (saldo + mov.Valor);
            //            else
            //                caja.Saldo = (saldo - mov.Valor);

            //            caja.FechaSaldo = mov.Fecha;

            //            cDAO.Insertar(caja);
            //        }

            //    }


            //    cDAO.ActualizarSaldo(caja, valorActualizar);

            //}



            // SE DEVE ACTUALIZAR O SALDO, VERIFICAR AS POSSIBLILIDADES DE MODIFICAR O VALOR  E A DATA ///
            // vERIFICAR SE A DATA JA EXISTE E ACTUALIZAR OU VERIF SE É NOVA Y INSERTAR, E CLARO, ACTUALIZAR O SALDO //

            return RedirectToAction("MostrarMovimientos2", "Movimiento");

        }




    }
}