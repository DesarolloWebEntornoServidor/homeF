using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Web;
using Rotativa;
using System.Web.Mvc;
using homeFinanceMVC.Models;
using PagedList;

namespace homeFinanceMVC.Views
{
    public class UsuarioController : Controller
    {
        private UsuarioDAO uDAO = new UsuarioDAO();
        private Usuario usu = new Usuario();

        public ActionResult InsertarUsuario()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GuardarDatos(HttpPostedFileBase file, Usuario usuario)
        {

            if (!ModelState.IsValid)
            {
                return View("InsertarUsuario", usuario);
            }

            string rutaFinal = "";
            TempData["mesaje"] = "mensagem Ok";

            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    TempData["formatoImagen"] = null;
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    usu.Foto = array;

                    string ruta = Server.MapPath(("~") + "//imagenes");
                    string strFilename = Path.GetFileName(file.FileName);

                    string archivo = "";
                    string extension = "";

                    string nex = strFilename.Substring(strFilename.Length - 3);

                    if (strFilename != "")
                    {

                        if (!Directory.Exists(ruta))
                            Directory.CreateDirectory(ruta);

                        archivo = String.Format("{0}//{1}", ruta, strFilename);

                        file.SaveAs(archivo);

                        extension = Path.GetExtension(strFilename);

                        if (nex != "jpg" && nex != "JPG" && nex != "png" && nex != "PNG")
                        {
                            TempData["formatoImagen"] = "No Soportado";

                            return View("InsertarUsuario", usuario);
                        }

                        rutaFinal = "/imagenes/" + strFilename;

                    }
                }
            }

            usuario.Ruta = rutaFinal;
            uDAO.Insertar(usuario);

            return RedirectToAction("InsertarUsuario", "Usuario");
        }

        public ActionResult ManipularDatos(int? page)
        {

            List<Usuario> usu = uDAO.ListarUsuarios();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(uDAO.ListarUsuarios().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DelUsuario(string id)
        {
            int verificaId = Convert.ToInt32(Session["idUsu"]);

            if (verificaId == Convert.ToInt32(id))
            {
                TempData["mesajeDeleteCancelado"] = "mensagem";
            }

            usu = uDAO.ObtenerUsuarioPorId(Convert.ToInt32(id));


            return View(usu);       
        }

        public ActionResult DelUsuarioConfirma(string id)
        {
            uDAO.BorarRegistro(Convert.ToInt32(id));

            TempData["mesajeDelete"] = "mensagem Ok";

           return RedirectToAction("ManipularDatos", "Usuario");
        }

        public ActionResult EditUsuario(string id)
        {
            usu = uDAO.ObtenerUsuarioPorId(Convert.ToInt32(id));

            return View(usu);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditUsuario(HttpPostedFileBase file1, Usuario usu, string ruta, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return View("EditUsuario", usu);
            }

            string rutaActual = Session["ruta"].ToString();

            usu = uDAO.ObtenerUsuarioPorId(Convert.ToInt32(form["IdUsuario"]));

            int sitAnterior = usu.Situacion;
            string situaDesc = "";

            usu.Nombre = form["Nombre"].ToString();
            usu.Login = form["Login"].ToString();
            usu.Email = form["Email"].ToString();
            usu.Password = form["Password"].ToString();
            string tp = form["Tipo"].ToString();
            string sit = form["Situacion"].ToString();

            if (tp == "2")
                usu.Tipo = 2;
            else
                usu.Tipo = 1;

            if (sit == "1")
            {
                usu.Situacion = 1;
                situaDesc = "Activado";
            }
            else
            {
                usu.Situacion = 0;
                situaDesc = "Desactivado";
            }

            if (ruta == null)
                ruta = usu.Ruta;

            string rutaNew = ruta;
            
            if (file1 != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    TempData["formatoImagen"] = null;
                    try
                    {
                        file1.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();

                        usu.Foto = array;

                        rutaNew = Server.MapPath(("~") + "//imagenes");
                        string strFilename = Path.GetFileName(file1.FileName);

                        string archivo = "";
                        string extension = "";

                        string nex = strFilename.Substring(strFilename.Length - 3);

                        if (strFilename != "")
                        {

                            if (!Directory.Exists(rutaNew))
                                Directory.CreateDirectory(rutaNew);

                            archivo = String.Format("{0}//{1}", rutaNew, strFilename);

                            file1.SaveAs(archivo);

                            extension = Path.GetExtension(strFilename);

                            if (nex != "jpg" && nex != "JPG" && nex != "png" && nex != "PNG")
                            {
                                TempData["formatoImagen"] = "No Soportado";

                                return View("EditUsuario", usu);
                            }

                            usu.Ruta = "/imagenes/" + strFilename;

                        }
                    }
                    catch (Exception)
                    {

                        TempData["formatoImagen"] = "No Soportado";

                        return View("EditUsuario", usu);
                    }
                  
                }
            }

            uDAO.ModificarUsuario(usu);

            if (Convert.ToInt32(Session["idUsu"]) == usu.IdUsuario)
            {
                Session["ruta"] = usu.Ruta;
            }
            else
            {
                Session["ruta"] = rutaActual;
            }

            // Enviar email informando la situación actual del Usuario
            if(sitAnterior != usu.Situacion)
            {
                try
                {
                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress("soporte@homefinanceweb.es");
                    correo.To.Add(usu.Email);
                    correo.Subject = "Cambio de situacion de acceso";
                    correo.Body = "Su cuenta en Home Finance, ha sido " + situaDesc;
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
            }


            if (Convert.ToInt32(Session["tipoUsu"]) == 2)
                return RedirectToAction("IndexUsuario", "Home");

            return RedirectToAction("ManipularDatos", "Usuario");

        }

        public ActionResult Imprimir(int? page)
        {

            List<Usuario> usu = uDAO.ListarUsuarios();

            int pageSize = 50000;
            int pageNumber = (page ?? 1);

            return View(uDAO.ListarUsuarios().ToPagedList(pageNumber, pageSize));
        }

        private RelatorioDuplicata getRelatorio()
        {
            var rpt = new RelatorioDuplicata();
            rpt.BasePath = Server.MapPath("/");

            rpt.PageTitle = "Lista de Usuarios";
            rpt.PageTitle = "Lista de Usuarios";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = true;

            return rpt;
        }
        public ActionResult Preview()
        {
            var rpt = getRelatorio();

            return File(rpt.GetOutputU().GetBuffer(), "application/pdf");
        }


    }
}