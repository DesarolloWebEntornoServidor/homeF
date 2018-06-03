using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using homeFinanceMVC.Models;
//using Datos.Entidades;

namespace homeFinanceMVC.Controllers
{
    public class LoginController : Controller
    {

        private UsuarioDAO uDAO = new UsuarioDAO();
        private Usuario usu = new Usuario();

        // GET: Login
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {

            int logeado = uDAO.verificaUsuario(login,password);

            if (logeado == 3)
            {
                TempData["noConecta"] = "BBDD no Conectada !!!";
                return RedirectToAction("Login", "Login");
            } else if (logeado == 0)
            {
                TempData["mesaje"] = "Usuario no Logeado !!!";
                return RedirectToAction("Login", "Login");
            }
            else { 

            usu = uDAO.ObtenerUsuario(login);


                Session["idUsu"] = usu.IdUsuario.ToString();
                Session["nombreUsu"] = usu.Nombre;
                Session["tipoUsu"] = usu.Tipo;
                Session["ruta"] = usu.Ruta;

                int usuNivelAcceso = usu.Tipo;

                if (usu.Situacion == 0)
                {
                    TempData["inactivo"] = "Usuario Inactivo !!!";
                    return RedirectToAction("Login", "Login");
                }

                if (usuNivelAcceso == 2)
                {
                    return RedirectToAction("IndexUsuario", "Home");
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Registrar(Usuario usuario)
        {

            if (!ModelState.IsValid)
            {
                return View("Registrar", usuario);
            }

            TempData["mesaje"] = "mensagem Ok";

            usuario.Tipo = 2;
            usuario.Situacion = 0;

            int retorno = uDAO.Insertar(usuario);

            if (retorno == 0)
            {
                TempData["noReg"] = "Usuario No Registrado !!!";
                return RedirectToAction("Login", "Login");
            }

            TempData["Reg"] = "Usuario Registrado !!!";

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Logout()
        {

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

    }
}