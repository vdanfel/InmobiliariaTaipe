using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace InmobiliariaWeb.Servicios
{
    public class LoginService : ILoginService
    {
        private readonly SqlConnection _connection;
        public LoginService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<LoginResult> ValidarLogin(string Usuario, string Clave)
        {
            var claveEncriptada = EncriptarClave(Clave);
            LoginResult loginResult = new LoginResult();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_LOGIN", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISUsuario", Usuario);
                    command.Parameters.AddWithValue("@ISClave", claveEncriptada);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        loginResult.IdentUsuario = Int32.Parse(reader["IDENT_USUARIO"].ToString());
                        loginResult.Ident005TipoUsuario = Int32.Parse(reader["IDENT005TIPOUSUARIO"].ToString());
                        loginResult.Usuario = reader["USUARIO"].ToString();
                        loginResult.NombreCompleto = reader["NOMBRE"].ToString();
                        loginResult.Mensaje = reader["MENSAJE"].ToString();
                    }
                    return loginResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        private string EncriptarClave(string password)
        {
            if (password == null)
            {
                return password;
            }
            else
            {
                using (var md5 = MD5.Create())
                {
                    byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }

                    // Retorna un string hexadecimal
                    return sBuilder.ToString();
                }
            }

        }
    }
}
