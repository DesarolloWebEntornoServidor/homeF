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
        public int Insertar(EvActivoPasivo ev)
        {

            int retorno = 0; 

            MySqlCommand comando = new MySqlCommand(string.Format("Insert into EvActivoPasivos (descEvento, tipoEvento) values ('{0}','{1}')",
                ev.DescEvento, ev.TipoEvento), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();
            return retorno;
        }
        public int Actualizar(EvActivoPasivo ev)
        {
            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(string.Format("update evactivopasivos set descEvento = '{0}', tipoEvento = '{1}' where idEvento={2}",
                ev.DescEvento, ev.TipoEvento, ev.IdEvento), Conexion.ObtenerConexion());

            retorno = comando.ExecuteNonQuery();
            return retorno;
        }

        public DataTable Consultar()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            MySqlDataAdapter comando = new MySqlDataAdapter("select idEvento,descEvento, tipoEvento,case tipoEvento" +
                "  when 'C' then 'Credito'  when 'D' then 'Debito'" +
                "  end as tipEvent from evactivopasivos where descEvento != '' order by descEvento", Conexion.ObtenerConexion());
            comando.Fill(dt);
            return dt;
        }

        public int Borrar(int id)
        {

            int retorno = 0;
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM EvActivoPasivos where idEvento={0}", id), Conexion.ObtenerConexion());

                retorno = comando.ExecuteNonQuery();

                //MySqlCommand comando1 = new MySqlCommand(string.Format("delete FROM cajageneral where idMov={0}", id));

                //comando1.ExecuteNonQuery();

            }
            catch (Exception)
            {

                Conexion.CerrarConexion();
            }

            return retorno;
        }
        public  List<EvActivoPasivo> ListarEvActivoPasivos()
        {
            List<EvActivoPasivo> lista = new List<EvActivoPasivo>();

            MySqlCommand comando = new MySqlCommand(String.Format("select idEvento, descEvento, tipoEvento from EvActivoPasivos order by descEvento"), Conexion.ObtenerConexion());
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                EvActivoPasivo evs = new EvActivoPasivo();
                evs.IdEvento = codigos.GetInt32(0);
                evs.DescEvento = codigos.GetString(1);
                evs.TipoEvento = codigos.GetString(2);

                lista.Add(evs);
            }


            return lista;
        }
        public object ConsultarTipo()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            MySqlDataAdapter comando = new MySqlDataAdapter("select distinct case (tipoEvento)" +
                "  when 'C' then 'Credito'  when 'D' then 'Debito'" +
                "  end as tipEvent, tipoEvento from evactivopasivos where descEvento != '' order by descEvento", Conexion.ObtenerConexion());
            comando.Fill(dt);
            return dt;
        }

        public static List<EvActivoPasivo> ListarEvActivoPasivosDS()
        {
            List<EvActivoPasivo> lista = new List<EvActivoPasivo>();
            try
            {
              //  MySqlDataAdapter adap = new MySqlDataAdapter("SELECT * FROM tabla_bd", BDConnection.ObtenerConexion());

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
               // adap.Fill(dt);
                //cboSexo.DataTextField = "Valor_que se quiero consultar";
                //cboSexo.DataValueField = "identificador de la tabla";
                //cboSexo.DataSource = dt;
                //dropdownlist.DataBind();
            }
            catch (Exception _e)
            {
                throw;
            }

            return lista;
        }



        public  EvActivoPasivo ObtenerEvento(int id)
        {
            EvActivoPasivo ev = new EvActivoPasivo();

            MySqlConnection cmd = Conexion.ObtenerConexion();

            MySqlCommand comando = new MySqlCommand(String.Format("select * FROM EvActivoPasivos where idEvento={0}", id), cmd);
            MySqlDataReader cods = comando.ExecuteReader();
            while (cods.Read())
            {
                ev.IdEvento = cods.GetInt32(0);
                ev.DescEvento = cods.GetString(1);
                ev.TipoEvento = cods.GetString(2);

            }

            cmd.Close();

            return ev;

        }

   

    }
}
