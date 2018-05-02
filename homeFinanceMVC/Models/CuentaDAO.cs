using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class CuentaDAO
    {
        public int Insertar(Cuenta c)
        {
           int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("Insert into cuentas (descCuenta, idUsuario) values ('{0}','{1}')",
                c.DescCuenta, c.IdUsuario), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();
            return retorno;
        }

        public DataTable Consultar()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            MySqlDataAdapter comando = new MySqlDataAdapter("select * from cuentas c join usuarios u on (c.idUsuario=u.idUsuario) order by descCuenta", Conexion.ObtenerConexion());
            comando.Fill(dt);
            return dt;
        }

        public Cuenta ObtenerCuenta(int id)
        {
            Cuenta cc = new Cuenta();

            MySqlCommand comando = new MySqlCommand(String.Format("select idCuenta, descCuenta, idUsuario from cuentas where idCuenta='" + id + "'"), Conexion.ObtenerConexion());
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                cc.IdCuenta = codigos.GetInt32(0);
                cc.DescCuenta = codigos.GetString(1);
                cc.IdUsuario = codigos.GetInt32(2);
            }
            Conexion.CerrarConexion();

            return cc;
        }

        public int Borrar(int id)
        {
            int retorno = 0;
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM cuentas where idCuenta={0}", id), Conexion.ObtenerConexion());

                retorno = comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                Conexion.CerrarConexion();
            }

           return retorno;
        }

        public int Actualizar(Cuenta cc)
        {
            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("update cuentas set descCuenta = '{0}', idUsuario = '{1}' where idCuenta={2}",
                cc.DescCuenta, cc.IdUsuario, cc.IdCuenta), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();
            return retorno;
        }


        public List<Cuenta> ListarCuentas()
        {
            List<Cuenta> lista = new List<Cuenta>();

            MySqlCommand comando = new MySqlCommand(String.Format("select * from cuentas where idUsuario > 0 order by descCuenta"), Conexion.ObtenerConexion());
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Cuenta evs = new Cuenta();
                evs.IdCuenta = codigos.GetInt32(0);
                evs.DescCuenta = codigos.GetString(1);
                evs.IdUsuario = codigos.GetInt32(2);

                lista.Add(evs);
            }

            return lista;
        }


    }
}
