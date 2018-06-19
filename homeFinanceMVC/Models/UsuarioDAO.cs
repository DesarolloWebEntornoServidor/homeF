using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeFinanceMVC.Models
{
    public class UsuarioDAO
    {
        public int Insertar(Usuario u)
        {
            int retorno = 0;

            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("Insert into usuarios (login, password, nombre, tipo, situacion, ruta, foto, email) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                  u.Login, Utilitarios.Encriptar(u.Password), u.Nombre, u.Tipo, u.Situacion, u.Ruta, u.Foto, u.Email), conection);

                retorno = comando.ExecuteNonQuery();
            comando.Dispose();
            conection.Close();
            }
            catch (Exception)
            {

                return 0;
            }

           

            return retorno;
        }

        public int ModificarUsuario(Usuario u) // Modificar Usuario // 
        {
            int retorno = 0;
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(string.Format("update usuarios set login = '{0}', password = '{1}', nombre = '{2}', tipo = '{3}', situacion = '{4}', ruta = '{5}', foto = '{6}', email = '{7}' where idUsuario = " +
                "{8}", u.Login, Utilitarios.Encriptar(u.Password), u.Nombre, u.Tipo, u.Situacion, u.Ruta, u.Foto,u.Email, u.IdUsuario), conection);

            retorno = comando.ExecuteNonQuery();

            comando.Dispose();
            conection.Close();

            return retorno;
        }

        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idUsuario,nombre,login,tipo, situacion, ruta, email from usuarios where tipo < 5 order by nombre"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                Usuario usus = new Usuario();
                usus.IdUsuario = codigos.GetInt32(0);
                usus.Nombre = codigos.GetString(1);
                usus.Login = codigos.GetString(2);
                usus.Tipo = codigos.GetInt32(3);
                usus.Situacion = codigos.GetInt32(4);
                try
                {
                    usus.Ruta = codigos.GetString(5);
                }
                catch (Exception)
                {

                    usus.Ruta = "";
                }

                usus.Email = codigos.GetString(6);


                lista.Add(usus);
            }
            comando.Dispose();
            conection.Close();
            return lista;
        }

        public Usuario ObtenerUsuario(string login)
        {
            Usuario usus = new Usuario();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idUsuario, nombre, login, password, tipo, situacion, ruta, email  from usuarios where login='" + login + "'"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                usus.IdUsuario = codigos.GetInt32(0);
                usus.Nombre = codigos.GetString(1);
                usus.Login = codigos.GetString(2);
                usus.Password = codigos.GetString(3);
                usus.Tipo = codigos.GetInt32(4);
                usus.Situacion = codigos.GetInt32(5);
                usus.Ruta = codigos.GetString(6);
                usus.Email = codigos.GetString(7);

            }

            comando.Dispose();
            conection.Close();

            return usus;
        }

        public int verificaUsuario(string usuario, string pass) // Logear //
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conection = Conexion.ObtenerConexion();
            cmd.Connection = conection;

            if(cmd.Connection == null)
            {
                
                return 3;
            }
            
            try
            {
                cmd.CommandText = string.Format("select count(*) from usuarios where login = '{0}' and password = '{1}'", usuario, Utilitarios.Encriptar(pass));

                int valor = int.Parse(cmd.ExecuteScalar().ToString());

                if (valor == 1)
                    return 1;
            }
            catch (Exception)
            {
                Console.WriteLine("Conexion no Aceptada !!!!!!!");
                return 0;
            }

            cmd.Dispose();
            conection.Close();

            return 0;
        }

        public int Borrar(int id)
        {
            int retorno = 0;
            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM usuarios where idUsuario={0}", id), conection);

                retorno = comando.ExecuteNonQuery();  comando.Dispose();
            conection.Close();

            }
            catch (Exception)
            {
               
            }

          
            return retorno;
        }

        public List<Usuario> YaExiste(string usuario, string alias)
        {
            List<Usuario> lista = new List<Usuario>();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idUsuario, nombre from usuarios where login = '{0}' or alias = '{1}'", usuario, alias), conection);
            MySqlDataReader codigos = comando.ExecuteReader();

            Usuario usus = new Usuario();

            while (codigos.Read())
            {
                usus.IdUsuario = codigos.GetInt32(0);
                usus.Nombre = codigos.GetString(1);


                lista.Add(usus);
            }

            comando.Dispose();
            conection.Close();

            return lista;
        }


        public int BorarRegistro(int codUsuario)
        {
            int retorno = 0;
            try
            {
                MySqlConnection conection = Conexion.ObtenerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("delete FROM usuarios where idUsuario={0}", codUsuario), conection);

                retorno = comando.ExecuteNonQuery();
                comando.Dispose();
            conection.Close();
            }
            catch (Exception)
            {

                
            }

         

            return retorno;

        }

        public Usuario ObtenerUsuarioPorId(int idUsu)
        {
            Usuario usus = new Usuario();
            MySqlConnection conection = Conexion.ObtenerConexion();
            MySqlCommand comando = new MySqlCommand(String.Format("select idUsuario, nombre, login, password, tipo, situacion, ruta, email from usuarios where idUsuario='" + idUsu + "'"), conection);
            MySqlDataReader codigos = comando.ExecuteReader();
            while (codigos.Read())
            {
                usus.IdUsuario = codigos.GetInt32(0);
                usus.Nombre = codigos.GetString(1);
                usus.Login = codigos.GetString(2);
                usus.Password = codigos.GetString(3);
                usus.Tipo = codigos.GetInt32(4);
                usus.Situacion = codigos.GetInt32(5);
                usus.Ruta = codigos.GetString(6);
                usus.Email = codigos.GetString(7);

            }

            comando.Dispose();
            conection.Close();

            return usus;
        }

    }
}

