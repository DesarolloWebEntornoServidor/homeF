using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class EvActivoPasivoDAO
    {
        public int Insertar(EvActivoPasivo evento)
        {
            int retorno = 0;
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("Insert into evactivopasivos (descEvento, tipoEvento) values ('{0}','{1}')",
                evento.DescEvento, evento.TipoEvento), conection);

            retorno = comando.ExecuteNonQuery();
 comando.Dispose();
            conection.Close();
            if (retorno == 0)
            {

                return 3;
            }

           

            return retorno;
        }
        public int Actualizar(EvActivoPasivo ev)
        {
            int retorno = 0;
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("update evactivopasivos set descEvento = '{0}', tipoEvento = '{1}' where idEvento={2}",
                ev.DescEvento, ev.TipoEvento, ev.IdEvento), conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public int BorarRegistro(int codEvento)
        {
            int retorno = 0;
            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM evactivopasivos where idEvento={0}", codEvento), conection);


                retorno = comando.ExecuteNonQuery();
            comando.Dispose();
            conection.Close();

            }
            catch (Exception)
            {

                
            }

            return retorno;

        }

        public  EvActivoPasivo ObtenerEvento(int id)
        {
            EvActivoPasivo ev = new EvActivoPasivo();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idEvento, descEvento, tipoEvento from evactivopasivos where idEvento='" + id + "'"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                ev.IdEvento = codigos.GetInt32(0);
                ev.DescEvento = codigos.GetString(1);
                ev.TipoEvento = codigos.GetString(2);
            }

            comando.Dispose();
            conection.Close();

            return ev;

        }

        public List<EvActivoPasivo> ListarTodosEventos()
        {
            List<EvActivoPasivo> listaEventos = new List<EvActivoPasivo>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT * FROM evactivopasivos order by descEvento"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                EvActivoPasivo ev1 = new EvActivoPasivo();
                ev1.IdEvento = codigos.GetInt32(0);
                ev1.DescEvento = codigos.GetString(1);
                ev1.TipoEvento = codigos.GetString(2);


                listaEventos.Add(ev1);
            }

            comando.Dispose();
            conection.Close();

            return listaEventos;
        }

    }
}
