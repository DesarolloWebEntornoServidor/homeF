using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class Movimiento
    {
        int idMov;
        
        string descMov;

        
        string fecha;

        [Range(1, 5000)]
        [RegularExpression(@"^((\d+(\.\d{0,2})?)|(\.\d{0,2}))$", ErrorMessage ="O valor tiene que ser un numero")]
        decimal valor;

        [Required]
        int idUsuario;

        [Required]
        int idEvento;

        [Required]
        int idCuenta;

        public Movimiento() { }

        public Movimiento(int idMov, string descMov, string fecha, decimal valor, int idUsuario, int idEvento, int idCuenta)
        {
            this.idMov = idMov;
            this.descMov = descMov;
            this.fecha = fecha;
            this.valor = valor;
            this.idUsuario = idUsuario;
            this.idEvento = idEvento;
            this.idCuenta = idCuenta;
        }

        public Movimiento(int idMov, string descMov, string fecha, decimal valor, int idCuenta, int idEvento)
        {
            this.idMov = idMov;
            this.descMov = descMov;
            this.fecha = fecha;
            this.valor = valor;
            this.idEvento = idEvento;
            this.idCuenta = idCuenta;
        }

        public Movimiento(string descMov)
        {
            this.descMov = descMov;
        }
  
        public int IdMov
        {
            get
            {
                return idMov;
            }

            set
            {
                idMov = value;
            }
        }

        [StringLength(30, MinimumLength = 5)]
        [Required(ErrorMessage = "La Descripción es Necesaria")]
        [RegularExpression(@"[A-Za-z]{1}[a-zA-ZñÑ\s]{2,30}", ErrorMessage = "La Descripción debe  Iniciar con Carácter y no puede ser mayor que 30 ni menor que 3.")]
        public string DescMov
        {
            get
            {
                return descMov;
            }

            set
            {
                descMov = value;
            }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A Fecha es Necesaria")]
        public string Fecha
        {
            get
            {
                return fecha;
            }

            set
            {
                fecha = value;
            }
        }

        [Required(ErrorMessage = "El Valor es Necesario")]
        [Range(1, 10000)]
        [DataType(DataType.Currency)]
        public decimal Valor
        {
            get
            {
                return valor;
            }

            set
            {
                valor = value;
            }
        }

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

        [Range(1, 1000, ErrorMessage = "Debes Seleccionar un Evento")]
        public int IdEvento
        {
            get
            {
                return idEvento;
            }

            set
            {
                idEvento = value;
            }
        }

        [Range(1, 1000, ErrorMessage = "Debes Seleccionar una Cuenta")]
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
    }
}
