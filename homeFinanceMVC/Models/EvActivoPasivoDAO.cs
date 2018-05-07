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

            MySqlCommand comando = new MySqlCommand(string.Format("Insert into evactivopasivos (descEvento, tipoEvento) values ('{0}','{1}')",
                evento.DescEvento, evento.TipoEvento), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();

            if (retorno == 0)
            {

                return 3;
            }

            Conexion.CerrarConexion();

            return retorno;
        }
        public int Actualizar(EvActivoPasivo ev)
        {
            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("update evactivopasivos set descEvento = '{0}', tipoEvento = '{1}' where idEvento={2}",
                ev.DescEvento, ev.TipoEvento, ev.IdEvento), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();

            Conexion.CerrarConexion();

            return retorno;
        }

        public int BorarRegistro(int codEvento)
        {
            int retorno = 0;
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM evactivopasivos where idEvento={0}", codEvento), Conexion.ObtenerConexion());

                retorno = comando.ExecuteNonQuery();
            }
            catch (Exception)
            {

                
            }

            Conexion.CerrarConexion();

            return retorno;

        }

        public  EvActivoPasivo ObtenerEvento(int id)
        {
            EvActivoPasivo ev = new EvActivoPasivo();

            MySqlCommand comando = new MySqlCommand(String.Format("select idEvento, descEvento, tipoEvento from evactivopasivos where idEvento='" + id + "'"), Conexion.ObtenerConexion());
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                ev.IdEvento = codigos.GetInt32(0);
                ev.DescEvento = codigos.GetString(1);
                ev.TipoEvento = codigos.GetString(2);
            }

            Conexion.CerrarConexion();

            return ev;

        }

        public List<EvActivoPasivo> ListarTodosEventos()
        {
            List<EvActivoPasivo> listaEventos = new List<EvActivoPasivo>();

            MySqlCommand comando = new MySqlCommand(String.Format("SELECT * FROM evactivopasivos order by descEvento"), Conexion.ObtenerConexion());
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                EvActivoPasivo ev1 = new EvActivoPasivo();
                ev1.IdEvento = codigos.GetInt32(0);
                ev1.DescEvento = codigos.GetString(1);
                ev1.TipoEvento = codigos.GetString(2);


                listaEventos.Add(ev1);
            }

            Conexion.CerrarConexion();

            return listaEventos;
        }

    }
}
