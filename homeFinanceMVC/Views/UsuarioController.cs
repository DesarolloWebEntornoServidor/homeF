using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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

            TempData["mesaje"] = "mensagem Ok";

            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    usu.Foto = array;

                    string ruta = Server.MapPath(("~") + "//imagenes");
                    string strFilename = Path.GetFileName(file.FileName);

                    string archivo = "";

                    if (strFilename != "")
                    {

                        if (!Directory.Exists(ruta))
                            Directory.CreateDirectory(ruta);

                        archivo = String.Format("{0}//{1}", ruta, strFilename);

                        // Verificar que el archivo no exista
                        //if (File.Exists(archivo))
                        //MensajeError(String.Format(
                        //  "Ya existe una imagen con nombre\"{0}\".", file.FileName));

                        file.SaveAs(archivo);

                        string extension = Path.GetExtension(strFilename);

                        usu.Ruta = "/imagenes/" + strFilename;

                    }
                }
            }

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

            TempData["mesaje"] = "mensagem Ok";

            usu = uDAO.ObtenerUsuarioPorId(Convert.ToInt32(form["IdUsuario"]));

            usu.Nombre = form["Nombre"].ToString();
            usu.Login = form["Login"].ToString();
            usu.Password = form["Password"].ToString();
            string tp = form["Tipo"].ToString();

            if (tp == "2")
                usu.Tipo = 2;
            else
                usu.Tipo = 1;

            string sit = form["Situacion"].ToString();

            if (sit == "1")
                usu.Situacion = 1;
            else
                usu.Situacion = 0;

            if (ruta == null)
                ruta = usu.Ruta;

            string rutaNew = ruta;
            
            if (file1 != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file1.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    usu.Foto = array;

                    rutaNew = Server.MapPath(("~") + "//imagenes");
                    string strFilename = Path.GetFileName(file1.FileName);

                    string archivo = "";

                    if (strFilename != "")
                    {

                        if (!Directory.Exists(rutaNew))
                            Directory.CreateDirectory(rutaNew);

                        archivo = String.Format("{0}//{1}", rutaNew, strFilename);

                        file1.SaveAs(archivo);

                        string extension = Path.GetExtension(strFilename);

                        usu.Ruta = "/imagenes/" + strFilename;

                    }
                }
            }

            uDAO.ModificarUsuario(usu);

            if(Convert.ToInt32(Session["tipoUsu"]) == 2)
                return RedirectToAction("IndexUsuario", "Home");

            return RedirectToAction("ManipularDatos", "Usuario");

        }


    }
}