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
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("Insert into logs (logDesc) values ('{0}')",
                l.LogDesc), conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public int Borrar(int id)
        {
            int retorno = 0;
            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM logs where idLog={0}", id), conection);

                retorno = comando.ExecuteNonQuery();
            comando.Dispose();
            conection.Close();
            }
            catch (Exception)
            {
                
            }


            return retorno;
        }
        public List<Log> ListarLogs()
        {
            List<Log> lista = new List<Log>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idLog, logDesc, fechaLog from logs order by fechaLog"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Log l = new Log();
                l.IdLog = codigos.GetInt32(0);
                l.LogDesc = codigos.GetString(1);
                l.FechaLog = Convert.ToDateTime(codigos.GetString(2));

                lista.Add(l);
            }

            comando.Dispose();
            conection.Close();

            return lista;
        }


    }
}