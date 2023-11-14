using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Persona;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace InmobiliariaWeb.Servicios
{
    public class ProgramaService: IProgramaService
    {
        private readonly SqlConnection _connection;
        public ProgramaService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> RegistrarPrograma(ViewPrograma viewPrograma, LoginResult loginResult)
        {
            var IdentPrograma = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_PROGRAMA_INSERT", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISNOMBRE", viewPrograma.NombrePrograma);
                    command.Parameters.AddWithValue("@ISNUMERO_PARTIDA", viewPrograma.NumeroPartida);
                    command.Parameters.AddWithValue("@ISDIRECCION", viewPrograma.Direccion);
                    command.Parameters.AddWithValue("@ISREFERENCIA", viewPrograma.Referencia ?? "");
                    command.Parameters.AddWithValue("@ISAREA_TOTAL", viewPrograma.AreaTotal);
                    command.Parameters.AddWithValue("@ISAREA_LOTIZADA", viewPrograma.AreaLotizada);
                    command.Parameters.AddWithValue("@ISCANTIDAD_MANZANAS", viewPrograma.CantidadManzanas);
                    command.Parameters.AddWithValue("@ISSUMINISTRO", viewPrograma.Suministro ?? "");
                    command.Parameters.AddWithValue("@ISUSUARIO", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        IdentPrograma = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                    }
                    return IdentPrograma;
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
        public async Task<string> RegistrarManzanas(ViewPrograma viewPrograma, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Manzana_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", viewPrograma.IdentPrograma);
                    command.Parameters.AddWithValue("@ISCantidad", viewPrograma.CantidadManzanas);
                    command.Parameters.AddWithValue("@ISUSUARIO", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<ViewPrograma> BuscarProgramaIdentPrograma(int identPrograma)
        {
            ViewPrograma viewPrograma = new ViewPrograma();
            viewPrograma.IdentPrograma = identPrograma;
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_Programa_BIdentPrograma", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", viewPrograma.IdentPrograma);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        viewPrograma.NombrePrograma = reader["NOMBRE"].ToString();
                        viewPrograma.NumeroPartida = reader["NUMERO_PARTIDA"].ToString();
                        viewPrograma.Codigo = reader["CODIGO"].ToString();
                        viewPrograma.Direccion = reader["DIRECCION"].ToString();
                        viewPrograma.Referencia = reader["REFERENCIA"].ToString();
                        viewPrograma.AreaTotal = decimal.Parse(reader["AREA_TOTAL"].ToString());
                        viewPrograma.AreaLotizada = decimal.Parse(reader["AREA_LOTIZADA"].ToString());
                        viewPrograma.CantidadManzanas = Int32.Parse(reader["CANTIDAD_MANZANAS"].ToString());
                        viewPrograma.Suministro = reader["SUMINISTRO"].ToString();
                    }
                    return viewPrograma;
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
        public async Task<List<ProgramaList>> BandejaPrograma(string buscar)
        { 
            var programas = new List<ProgramaList>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_PROGRAMAS_BANDEJA", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISBuscar", buscar ?? "");
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var programaList = new ProgramaList();
                        programaList.Indice = Int32.Parse(reader["INDICE"].ToString());
                        programaList.IdentPrograma = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                        programaList.Codigo = reader["CODIGO"].ToString();
                        programaList.NombrePrograma = reader["NOMBRE"].ToString();
                        programaList.LotesUsados = Int32.Parse(reader["UTILIZADO"].ToString());
                        programaList.LotesTotales = Int32.Parse(reader["TOTAL"].ToString());
                        programas.Add(programaList);
                    }
                    return programas;
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
    }
}
