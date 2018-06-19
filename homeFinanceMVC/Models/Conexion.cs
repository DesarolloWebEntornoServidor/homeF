using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace homeFinanceMVC.Models
{
    public class Conexion
    {
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conectar = null;
            try
            {
                // conectar = new MySqlConnection("server=127.0.0.1; database=homefinance; Uid=root; pwd=altair;");

                conectar = new MySqlConnection("server=db735710035.db.1and1.com; database=db735710035; Uid=dbo735710035; pwd=Ray130270");

                conectar.Open();
            }
            catch (Exception)
            {
               
                return null;
            }

            return conectar;
        }



    }
}


