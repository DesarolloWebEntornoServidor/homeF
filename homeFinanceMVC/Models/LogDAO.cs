using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace homeFinanceMVC.Models
{
    public class LogDAO
    {
        public int Insertar(Log l)
        {
            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("Insert into logs (logDesc) values ('{0}')",
                l.LogDesc), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();

            Conexion.CerrarConexion();

            return retorno;
        }

        public int Borrar(int id)
        {
            int retorno = 0;
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM logs where idLog={0}", id), Conexion.ObtenerConexion());

                retorno = comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
            }

            Conexion.CerrarConexion();

            return retorno;
        }
        public List<Log> ListarLogs()
        {
            List<Log> lista = new List<Log>();

            MySqlCommand comando = new MySqlCommand(String.Format("select idLog, logDesc, fechaLog from logs order by fechaLog"), Conexion.ObtenerConexion());
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Log l = new Log();
                l.IdLog = codigos.GetInt32(0);
                l.LogDesc = codigos.GetString(1);
                l.FechaLog = Convert.ToDateTime(codigos.GetString(2));

                lista.Add(l);
            }

            Conexion.CerrarConexion();

            return lista;
        }


    }
}