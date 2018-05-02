using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class Cuenta
    {
        int idCuenta;
        string descCuenta;
        int idUsuario;

        public int IdCuenta
        {
            get
            {
                return idCuenta;
            }

            set
            {
                idCuenta = value;
            }
        }

        [Required(ErrorMessage = "La Descripción de la Cuenta es Obligatoria")]
        [RegularExpression(@"[A-Za-z]{1}[a-zA-ZñÑ\s]{2,30}", ErrorMessage = "La Descripción debe  Iniciar con Carácter y no puede ser mayor que 30 ni menor que 3.")]
        public string DescCuenta
        {
            get
            {
                return descCuenta;
            }

            set
            {
                descCuenta = value;
            }
        }
        [Range(1, 1000, ErrorMessage = "Debes Seleccionar un Usuario")]
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

        public Cuenta() { }

        public Cuenta(string descCuenta, int idUsuario)
        {
            this.descCuenta = descCuenta;
            this.idUsuario = idUsuario;
        }

        public Cuenta(string descCuenta)
        {
            this.descCuenta = descCuenta;
        }
    }
}
