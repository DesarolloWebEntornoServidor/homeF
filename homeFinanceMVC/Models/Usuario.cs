using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace homeFinanceMVC.Models
{
    public class Usuario
    {
        int idUsuario;
        string nombre;
        string login;
        string password;
        int tipo;
        int situacion;
        string ruta;
        byte[] foto;

        public Usuario()
        {
        }

        public Usuario(string nombre, string login, string password, int tipo, int situacion, string ruta, byte[] foto)
        {
            this.nombre = nombre;
            this.login = login;
            this.password = password;
            this.tipo = tipo;
            this.situacion = situacion;
            this.ruta = ruta;
            this.foto = foto;
        }

        public Usuario(string nombre)
        {
            this.nombre = nombre;
        }
        public Usuario(int idUsuario, string nombre)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
        }
        #region Propiedades
        public int IdUsuario
        {
            get
            {
                return idUsuario;
            }

            set
            {
                idUsuario = value;
            }
        }

        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        [RegularExpression(@"[A-Z]{1}[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,. '-]{2,40}", ErrorMessage = "El Nombre debe Empiezar con Carácteres Maiusculo y no debe ser mayor que 40 ni menor que 3.")]
        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        [Required(ErrorMessage = "El Login es Obligatorio")]
        [RegularExpression(@"[a-zA-Z0-9ñÑ]{1,15}", ErrorMessage = "El Login NO debe ser mayor que 15 Carácteres y no puede ser compuesta.")]
        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        [Required(ErrorMessage = "La Contraseña es Obligatoria")]
        [RegularExpression(@"[a-zA-Z0-9ñÑ]{1,15}", ErrorMessage = "La Contraseña NO debe ser mayor que 15 Carácteres y no puede ser compuesta")]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        [NotMapped]
        [Required(ErrorMessage = "La Confirmación de Contraseña es Obligatoria")]
        [CompareAttribute("Password", ErrorMessage = "Las Contraseñas NO son Iguales.")]
        public string ConfirmPassowrd { get; set; }

        public int Tipo
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;
            }
        }

        public int Situacion
        {
            get
            {
                return situacion;
            }

            set
            {
                situacion = value;
            }
        }

        public string Ruta
        {
            get
            {
                return ruta;
            }

            set
            {
                ruta = value;
            }
        }

        public byte[] Foto
        {
            get
            {
                return foto;
            }

            set
            {
                foto = value;
            }
        }


        #endregion
    }
}
