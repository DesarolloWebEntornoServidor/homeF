using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class CajaGeneral
    {
        int idCaja;
        string fechaSaldo;
        decimal saldo;
        int idMov;

        public int IdCaja
        {
            get
            {
                return idCaja;
            }

            set
            {
                idCaja = value;
            }
        }

        public string FechaSaldo
        {
            get
            {
                return fechaSaldo;
            }

            set
            {
                fechaSaldo = value;
            }
        }

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

        public CajaGeneral() { }

        public CajaGeneral(string fechaSaldo, decimal saldo, int idMov) 
        {
            this.idMov = idMov;
            this.fechaSaldo = fechaSaldo;
            this.saldo = saldo;
        }

        public CajaGeneral(string fechaSaldo, decimal saldo)
        {
            this.fechaSaldo = fechaSaldo;
            this.saldo = saldo;
        }

        public CajaGeneral(decimal saldo)
        {
            this.saldo = saldo;
        }

    }
}
