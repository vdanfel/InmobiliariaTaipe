using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Tablas;
using InmobiliariaWeb.Result;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

namespace InmobiliariaWeb.Servicios
{
    public class TablasService:ITablasService
    {
        private readonly SqlConnection _connection;

        public TablasService(SqlConnection connection) 
        {
            _connection = connection;
        }
        public async Task<List<TipoDocumento>> ListarTipoDocumento()
        {
            int parametro = 1;
            var tipoDocumentoList = new List<TipoDocumento>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoDocumento = new TipoDocumento();
                        tipoDocumento.Ident_001_TipoDocumento= Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoDocumento.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoDocumento.Valor = reader["VALOR"].ToString();
                        tipoDocumento.Abreviatura = reader["ABREVIATURA"].ToString();
                        tipoDocumentoList.Add(tipoDocumento);
                    }
                    return tipoDocumentoList;
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
        public async Task<List<TipoEstadoCivil>> ListarTipoEstadoCivil()
        {
            int parametro = 6;
            var tipoEstadoCivilList = new List<TipoEstadoCivil>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Parametros", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("ISIdent_ParametroTipo", parametro);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var tipoEstadoCivil = new TipoEstadoCivil();
                        tipoEstadoCivil.Ident_006_TipoEstadoCivil = Int32.Parse(reader["IDENT_PARAMETRO"].ToString());
                        tipoEstadoCivil.Descripcion = reader["DESCRIPCION"].ToString();
                        tipoEstadoCivil.Valor = reader["VALOR"].ToString();
                        tipoEstadoCivilList.Add(tipoEstadoCivil);
                    }
                    return tipoEstadoCivilList;
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
        public async Task<List<Departamento>> ListarDepartamento()
        { 
            var departamentosList = new List<Departamento>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_DepartamentosList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var departamentos = new Departamento();
                        departamentos.CodigoDepartamento= reader["CODIGO"].ToString();
                        departamentos.DescripcionDepartamento= reader["DESCRIPCION"].ToString();
                        departamentosList.Add(departamentos);
                    }
                    return departamentosList;
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
        public async Task<List<Provincia>> ListarProvincia(string codigoDepartamento)
        {
            var provinciaList = new List<Provincia>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_ProvinciaList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCodigoDepartamento", codigoDepartamento);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var provincias = new Provincia();
                        provincias.CodigoProvincia = reader["CODIGO"].ToString();
                        provincias.DescripcionProvincia= reader["DESCRIPCION"].ToString();
                        provinciaList.Add(provincias);
                    }
                    return provinciaList;
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
        public async Task<List<Distrito>> ListarDistrito(string codigoDepartamento,string codigoProvincia)
        {
            var distritoList = new List<Distrito>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_DistritoList", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISCodigoDepartamento", codigoDepartamento);
                    command.Parameters.AddWithValue("@ISCodigoProvincia", codigoProvincia);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var distrito = new Distrito();
                        distrito.CodigoDistrito = reader["CODIGO"].ToString();
                        distrito.DescripcionDistrito = reader["DESCRIPCION"].ToString();
                        distrito.IdentUbigeo = Int32.Parse(reader["IDENT_UBIGEO"].ToString());
                        distritoList.Add(distrito);
                    }
                    return distritoList;
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
