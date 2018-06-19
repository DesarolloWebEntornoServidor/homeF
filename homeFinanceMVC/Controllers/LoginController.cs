using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
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

            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("soporte@homefinanceweb.es");
                correo.To.Add("info@railtoncardoso.com");
                correo.Subject = "Nuevo usuario Registrado";
                correo.Body = "Se ha Registrado en la Aplicación, el Usuario: " + usuario.Nombre + " Correo: " + usuario.Email;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.1and1.es";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                string cuentaCorreo = "soporte@homefinanceweb.es";
                string passCorreo = "homefinance";

                smtp.Credentials = new System.Net.NetworkCredential(cuentaCorreo, passCorreo);

                smtp.Send(correo);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Email no Enviado" + ex);
                TempData["errorCorreo"] = "Email error";
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Logout()
        {

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

    }
}