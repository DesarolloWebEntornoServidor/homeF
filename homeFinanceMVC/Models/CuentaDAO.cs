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
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("Insert into cuentas (descCuenta, idUsuario) values ('{0}','{1}')",
                c.DescCuenta, c.IdUsuario), conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public Cuenta ObtenerCuenta(int id)
        {
            Cuenta cc = new Cuenta();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idCuenta, descCuenta, idUsuario from cuentas where idCuenta='" + id + "'"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                cc.IdCuenta = codigos.GetInt32(0);
                cc.DescCuenta = codigos.GetString(1);
                cc.IdUsuario = codigos.GetInt32(2);
            }

            comando.Dispose();
            conection.Close();

            return cc;
        }

        public int Borrar(int id)
        {
            int retorno = 0;
            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM cuentas where idCuenta={0}", id), conection);

                retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            }
            catch (Exception)
            {
                
            }

           
            return retorno;
        }

        public int Actualizar(Cuenta cc)
        {
            int retorno = 0;
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("update cuentas set descCuenta = '{0}', idUsuario = '{1}' where idCuenta={2}",
                cc.DescCuenta, cc.IdUsuario, cc.IdCuenta), conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }


        public List<Cuenta> ListarCuentas()
        {
            List<Cuenta> lista = new List<Cuenta>();

            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = null;
            try
            {
                comando = new MySqlCommand(String.Format("select * from cuentas where idUsuario > 0 order by descCuenta"), conection);
                MySqlDataReader codigos = comando.ExecuteReader();
                while (codigos.Read())
                {
                    Cuenta evs = new Cuenta();
                    evs.IdCuenta = codigos.GetInt32(0);
                    evs.DescCuenta = codigos.GetString(1);
                    evs.IdUsuario = codigos.GetInt32(2);

                    lista.Add(evs);
                }
            }
            catch (Exception)
            {

                throw;
            }
          

            comando.Dispose();
            conection.Close();

            return lista;
        }
 
        public List<Cuenta> ListarCuentasPorId(int id)
        {
            List<Cuenta> lista = new List<Cuenta>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            try
            {
                MySqlCommand comando = new MySqlCommand(String.Format("select * from cuentas where idUsuario={0}", id), conection);
                MySqlDataReader codigos = comando.ExecuteReader();
                while (codigos.Read())
                {
                    Cuenta evs = new Cuenta();
                    evs.IdCuenta = codigos.GetInt32(0);
                    evs.DescCuenta = codigos.GetString(1);
                    evs.IdUsuario = codigos.GetInt32(2);

                    lista.Add(evs);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
               // comando.Dispose();
                conection.Close();
            }
           


            return lista;
        }
    }
}
