using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace homeFinanceMVC.Models
{

    public class MovimientoDAO
    {
        public int Insertar(Movimiento mov)
        {
            string f = String.Format("{0}-{1}-{2}", mov.Fecha.Substring(6), mov.Fecha.Substring(3,2), mov.Fecha.Substring(0,2));

            string vl = mov.Valor.ToString();

            decimal v1 = Convert.ToDecimal(vl, CultureInfo.CreateSpecificCulture("es-ES"));

            vl = vl.Replace(',', '.');
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("Insert into movimientos (descMov, fecha, valor, idEvento, idCuenta) values ('{0}','{1}','{2}','{3}','{4}')",
                mov.DescMov, f , vl, mov.IdEvento, mov.IdCuenta), conection);

            int retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public int Actualizar(Movimiento mUp)
        {
            string vl = mUp.Valor.ToString();

            decimal v1 = Convert.ToDecimal(vl, CultureInfo.CreateSpecificCulture("es-ES"));

            vl = vl.Replace(',', '.');

            string[] data = mUp.Fecha.Split('/');
            string fecha = mUp.Fecha;

            if (data.Length > 1)
                fecha = String.Format("{0}-{1}-{2}", data[2].Substring(0, 4), data[1], data[0]);

            int retorno = 0;
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("update movimientos set descMov = '{0}', fecha = '{1}', valor = '{2}', idCuenta = '{3}', idEvento = '{4}' where idMov={5}",
                mUp.DescMov, fecha, vl, mUp.IdCuenta, mUp.IdEvento, mUp.IdMov), conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public DataTable ConsultarMisEventos(int id)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlDataAdapter comando = new MySqlDataAdapter(string.Format("select * from movimientos m join evactivopasivos e on(m.idEvento=e.idEvento) join usuarios u on (m.idUsuario=u.idUsuario) where m.idUsuario={0} order by fecha", id), conection);
            comando.Fill(dt);

            comando.Dispose();
            conection.Close();

            return dt;
        }

        public int Borrar(int id)
        {

            int retorno = 0;
            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM movimientos where idMov={0}", id), conection);

                retorno = comando.ExecuteNonQuery();
 comando.Dispose();
            conection.Close();

            }
            catch (Exception)
            {

                
            }

           
            return retorno;
        }

        public List<Movimiento> ListaMovimientosPorPeriodo(string de, string hasta)
        {

            string fDe = String.Format("{0}-{1}-{2}", de.Substring(6), de.Substring(3, 2), de.Substring(0, 2));
            string fHasta = String.Format("{0}-{1}-{2}", hasta.Substring(6), hasta.Substring(3, 2), hasta.Substring(0, 2));

            List<Movimiento> lista = new List<Movimiento>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idMov, fecha, descMov, valor, idEvento, idCuenta from movimientos where fecha between '{0}' and '{1}' order by fecha", fDe, fHasta), conection);
                MySqlDataReader codigos = comando.ExecuteReader();
                while (codigos.Read())
                {
                    Movimiento mov = new Movimiento();
                    mov.IdMov = codigos.GetInt32(0);
                    mov.Fecha = codigos.GetString(1);
                    mov.DescMov = codigos.GetString(2);
                    mov.Valor = codigos.GetDecimal(3);
                    mov.IdEvento = codigos.GetInt32(4);
                    mov.IdCuenta = codigos.GetInt32(5);
                    
                    lista.Add(mov);
                }

            comando.Dispose();
            conection.Close();

            return lista;
        }

        public List<Movimiento> ListaMovimientosPorPeriodoAndId(string de, string hasta, int id)
        {

            string fDe = String.Format("{0}-{1}-{2}", de.Substring(6), de.Substring(3, 2), de.Substring(0, 2));
            string fHasta = String.Format("{0}-{1}-{2}", hasta.Substring(6), hasta.Substring(3, 2), hasta.Substring(0, 2));

            List<Movimiento> lista = new List<Movimiento>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idMov, fecha, descMov, valor, idEvento, m.idCuenta from movimientos m join cuentas c on (m.idCuenta=c.idCuenta)" +
                " where fecha between '{0}' and '{1}' and c.idUsuario = '{2}' order by fecha", fDe, fHasta, id), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Movimiento mov = new Movimiento();
                mov.IdMov = codigos.GetInt32(0);
                mov.Fecha = codigos.GetString(1);
                mov.DescMov = codigos.GetString(2);
                mov.Valor = codigos.GetDecimal(3);
                mov.IdEvento = codigos.GetInt32(4);
                mov.IdCuenta = codigos.GetInt32(5);

                lista.Add(mov);
            }

            comando.Dispose();
            conection.Close();

            return lista;
        }

        public List<Object> MovimientosLibro(string de, string hasta)
        {
            string fDe = String.Format("{0}-{1}-{2}", de.Substring(6), de.Substring(3, 2), de.Substring(0, 2));
            string fHasta = String.Format("{0}-{1}-{2}", hasta.Substring(6), hasta.Substring(3, 2), hasta.Substring(0, 2));

            List<Object> lista = new List<Object>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select fecha, e.descEvento, descMov, c.descCuenta, valor, e.tipoEvento from movimientos m" +
                " join evactivopasivos e on(m.idEvento=e.idEvento)" +
                " join cuentas c on(m.idCuenta=c.idCuenta)  where fecha between '{0}' and '{1}' order by fecha", fDe, fHasta), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Object[] mov = new Object[6];
                mov[0] = codigos.GetString(0);
                mov[1] = codigos.GetString(1);
                mov[2] = codigos.GetString(2);
                mov[3] = codigos.GetString(3);
                mov[4] = codigos.GetDecimal(4);
                mov[5] = codigos.GetString(5);


                lista.Add(mov);
            }

            comando.Dispose();
            conection.Close();

            return lista;
        }

        public int ObtenerUltimoId()
        {
            Movimiento mov = new Movimiento();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand("select max(idMov) from movimientos", conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                mov.IdMov = codigos.GetInt32(0);
            }

            comando.Dispose();
            conection.Close();

            return mov.IdMov;
        }

        public Movimiento ObtenerMovimiento(int id)
        {
            Movimiento mov = new Movimiento();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idMov, descMov, valor, fecha, idCuenta, idEvento from movimientos where idMov='" + id + "'"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                mov.IdMov = codigos.GetInt32(0);
                mov.DescMov = codigos.GetString(1);
                mov.Valor = Convert.ToDecimal(codigos.GetDouble(2));
                mov.Fecha = codigos.GetString(3);
                mov.IdCuenta = codigos.GetInt32(4);
                mov.IdEvento = codigos.GetInt32(5);
            }

            comando.Dispose();
            conection.Close();

            return mov;
        }

        // Valores de los Graficos /////

        public List<Movimiento> ObtenerValoresDelGrafico(string de, string hasta, string tipo)
        {
            string fDe = String.Format("{0}-{1}-{2}", de.Substring(6), de.Substring(3, 2), de.Substring(0, 2));
            string fHasta = String.Format("{0}-{1}-{2}", hasta.Substring(6), hasta.Substring(3, 2), hasta.Substring(0, 2));

            List<Movimiento> lista = new List<Movimiento>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select m.idEvento, e.descEvento, sum(m.valor) as VALOR from evactivopasivos e join movimientos m on (e.idEvento=m.idEvento) where VALOR > 0 and e.tipoEvento = '{0}' and fecha between '{1}' and '{2}'group by e.idEvento", tipo, fDe, fHasta), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Movimiento evs = new Movimiento();
                evs.IdEvento = codigos.GetInt32(0);
                evs.DescMov = codigos.GetString(1);
                evs.Valor = codigos.GetDecimal(2);

                lista.Add(evs);
            }

            comando.Dispose();
            conection.Close();

            return lista;
        }



    }
}
