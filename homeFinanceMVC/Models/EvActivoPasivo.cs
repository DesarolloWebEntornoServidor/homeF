using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class EvActivoPasivo
    {
        int idEvento;
        string descEvento;
        string tipoEvento;

        #region Propiedades
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

        [Required(ErrorMessage = "El Nombre del Evento es Obligatorio")]
        [RegularExpression(@"[a-zA-ZñÑ\s]{5,30}", ErrorMessage = "La Descripción debe ser caracteres e no mayor que 30 ni menor que 5.")]
        public string DescEvento
        {
            get
            {
                return descEvento;
            }

            set
            {
                descEvento = value;
            }
        }

        public string TipoEvento 
        {
            get
            {
                return tipoEvento;
            }

            set
            {
                tipoEvento = value;
            }
        }
        #endregion
        public EvActivoPasivo() { }

        public EvActivoPasivo(string descEvento, string tipoEvento)
        {
            this.descEvento = descEvento;
            this.tipoEvento = tipoEvento;
        }

        public EvActivoPasivo(int idEvento, string descEvento, string tipoEvento)
        {
            this.idEvento = idEvento;
            this.descEvento = descEvento;
            this.tipoEvento = tipoEvento;
        }

        public EvActivoPasivo(string descEvento)
        {
            this.descEvento = descEvento;
        }

    }
}
