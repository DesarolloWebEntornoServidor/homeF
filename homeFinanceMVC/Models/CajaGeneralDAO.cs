using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using homeFinanceMVC.Models;
using System.Data;
using System.Globalization;

namespace homeFinanceMVC.Models
{
    public class CajaGeneralDAO
    {
        public int Insertar(CajaGeneral cg)
        {
            string vl = cg.Saldo.ToString("N2");
            string vr = cg.Saldo.ToString();

            decimal v1 = Convert.ToDecimal(vl, CultureInfo.CreateSpecificCulture("es-ES"));

            vl = vl.Replace(',', '.');
            vr = vr.Replace(',', '.');

            string f = String.Format("{0}-{1}-{2}", cg.FechaSaldo.Substring(6), cg.FechaSaldo.Substring(3, 2), cg.FechaSaldo.Substring(0, 2));

            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("Insert into cajageneral (fechaSaldo, saldo) values ('{0}','{1}')",
                f, vr), conection);

            int retorno = comando.ExecuteNonQuery();
            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public void ActualizarSaldo(CajaGeneral caja, decimal valor)
        {

            string vl = valor.ToString("N2");
            string vr = valor.ToString();

            decimal v1 = Convert.ToDecimal(vl, CultureInfo.CreateSpecificCulture("es-ES"));

            vl = vl.Replace(',', '.');
            vr = vr.Replace(',', '.');

            int retorno = 0;

            string[] data = caja.FechaSaldo.Split('/');
            string fecha = caja.FechaSaldo;

            if (data.Length > 1)
                fecha = String.Format("{0}-{1}-{2}", data[2].Substring(0, 4), data[1], data[0]);
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("update cajageneral set saldo = (saldo + '{0}') where fechaSaldo >= '{1}'", vr, fecha),  conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return;
        }

        public decimal ConsultarSaldoActual()
        {
            decimal saldo = 0;

            MySqlConnection cmd = Conexion.ObtenerConexion();

            MySqlCommand comando = new MySqlCommand(String.Format("select saldo from cajageneral order by idCaja desc limit 1"), cmd);
            MySqlDataReader cods = comando.ExecuteReader();
            while (cods.Read())
            {
                //cg.IdCaja = cods.GetInt32(0);
                saldo = cods.GetDecimal(0);

            }

            cmd.Close();

            return saldo;

        }
        public decimal ConsultarSaldoLibroCaja(string de)
        {
            decimal saldo = 0;

            string f = String.Format("{0}-{1}-{2}", de.Substring(6), de.Substring(3, 2), de.Substring(0, 2));


            MySqlConnection cmd = Conexion.ObtenerConexion();

            MySqlCommand comando = new MySqlCommand(String.Format("select saldo from cajageneral where fechaSaldo >= '{0}' limit 1;", f), cmd);
            MySqlDataReader cods = comando.ExecuteReader();
            while (cods.Read())
            {
                saldo = cods.GetDecimal(0);
            }

            cmd.Close();

            return saldo;
        }

        public CajaGeneral LocalizarSaldo(string de)
        {
            string[] data = de.Split('/');
            string fecha = de;

            if (data.Length > 1)
                fecha = String.Format("{0}-{1}-{2}", data[2].Substring(0, 4), data[1], data[0]);

            CajaGeneral cg = new CajaGeneral();

            MySqlConnection cmd = Conexion.ObtenerConexion();

            MySqlCommand comando = new MySqlCommand(String.Format("select * from cajageneral where fechaSaldo = '{0}' limit 1;", fecha), cmd);
            MySqlDataReader cods = comando.ExecuteReader();
            while (cods.Read())
            {
                cg.IdCaja = cods.GetInt32(0);
                cg.FechaSaldo = cods.GetString(1);
                cg.Saldo = cods.GetDecimal(2);
            }

            cmd.Close();

            return cg;

        }

        public CajaGeneral LocalizarNuevoSaldo(string de)
        {
            string[] data = de.Split('/');
            string fecha = de;

            if (data.Length > 1)
                fecha = String.Format("{0}-{1}-{2}", data[2].Substring(0, 4), data[1], data[0]);

            CajaGeneral cg = new CajaGeneral();

            MySqlConnection cmd = Conexion.ObtenerConexion();

            MySqlCommand comando = new MySqlCommand(String.Format("select * from cajageneral where fechaSaldo >= '{0}' limit 1;", fecha), cmd);
            MySqlDataReader cods = comando.ExecuteReader();
            while (cods.Read())
            {
                cg.IdCaja = cods.GetInt32(0);
                cg.FechaSaldo = cods.GetString(1);
                cg.Saldo = cods.GetDecimal(2);
            }

            cmd.Close();

            return cg;

        }




    }
}
