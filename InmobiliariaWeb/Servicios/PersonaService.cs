using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models;
using InmobiliariaWeb.Models.Persona;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Persona;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace InmobiliariaWeb.Servicios
{
    public class PersonaService : IPersonaService
    {
        private readonly SqlConnection _connection;
        public PersonaService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<PersonaResult> PersonaRegistrar(PersonaCrearViewModel personaCrearViewModel, LoginResult loginResult)
        {
            PersonaResult personaResult = new PersonaResult();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Persona_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_001_TipoDocumento", personaCrearViewModel.Ident001TipoDocumento);
                    command.Parameters.AddWithValue("@ISDocumento", personaCrearViewModel.Documento);
                    command.Parameters.AddWithValue("@ISPaterno", personaCrearViewModel.Paterno ?? "");
                    command.Parameters.AddWithValue("@ISMaterno", personaCrearViewModel.Materno ?? "");
                    command.Parameters.AddWithValue("@ISNombre", personaCrearViewModel.Nombres ?? "");
                    command.Parameters.AddWithValue("@ISIdent_006_EstadoCivil", personaCrearViewModel.Ident006TipoEstadoCivil);
                    command.Parameters.AddWithValue("@ISFechaNacimiento", personaCrearViewModel.FechaNacimiento);
                    command.Parameters.AddWithValue("@ISRazonSocial", personaCrearViewModel.RazonSocial ?? "");
                    command.Parameters.AddWithValue("@ISCelular", personaCrearViewModel.Celular ?? "");
                    command.Parameters.AddWithValue("@ISTelefono", personaCrearViewModel.Telefono ?? "");
                    command.Parameters.AddWithValue("@ISCorreo", personaCrearViewModel.Correo ?? "");
                    command.Parameters.AddWithValue("@ISContacto", personaCrearViewModel.Contacto ?? "");
                    command.Parameters.AddWithValue("@ISTelefonoContacto", personaCrearViewModel.TelefonoContacto ?? "");
                    command.Parameters.AddWithValue("@ISCorreoContacto", personaCrearViewModel.CorreoContacto ?? "");
                    command.Parameters.AddWithValue("@ISDepartamento", personaCrearViewModel.Departamento);
                    command.Parameters.AddWithValue("@ISProvincia", personaCrearViewModel.Provincia);
                    command.Parameters.AddWithValue("@ISDistrito", personaCrearViewModel.Distrito);
                    command.Parameters.AddWithValue("@ISDireccion", personaCrearViewModel.Direccion ?? "");
                    command.Parameters.AddWithValue("@ISReferencia", personaCrearViewModel.Referencia ?? "");
                    command.Parameters.AddWithValue("@ISUsuarioCreacion", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        personaResult.IdentPersona = Int32.Parse(reader["IDENT_PERSONA"].ToString());

                    }
                    if (personaResult.IdentPersona > 0)
                    {
                        personaResult.Mensaje = "Se registró de manera satisfactoria";
                    }
                    else
                    {
                        personaResult.Mensaje = "No se pudo realizar el registro";
                    }
                    return personaResult;
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
        public async Task<int> PersonaExiste(PersonaCrearViewModel personaCrearViewModel)
        {
            int existe = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Persona_Existe", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_001_TipoDocumento", personaCrearViewModel.Ident001TipoDocumento);
                    command.Parameters.AddWithValue("@ISDocumento", personaCrearViewModel.Documento);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        existe = Int32.Parse(reader["EXISTE"].ToString());

                    }
                    return existe;
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
        public async Task<List<PersonaList>> PersonaBandeja(string buscar)
        {
            var personas = new List<PersonaList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Persona_Bandeja", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISBuscar", buscar ?? "");
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var personaList = new PersonaList();
                        personaList.Indice = Int32.Parse(reader["INDICE"].ToString());
                        personaList.Ident_Persona = Int32.Parse(reader["IDENT_PERSONA"].ToString());
                        personaList.Ident_001_TipoDocumento = Int32.Parse(reader["IDENT_001_TIPODOCUMENTO"].ToString());
                        personaList.TipoDocumento = reader["TIPO_DOCUMENTO"].ToString();
                        personaList.Documento = reader["DOCUMENTO"].ToString();
                        personaList.Paterno = reader["PATERNO"].ToString();
                        personaList.Materno = reader["MATERNO"].ToString();
                        personaList.Nombre = reader["NOMBRES"].ToString();
                        personaList.Ident006TipoEstadoCivil = Int32.Parse(reader["IDENT_006_ESTADOCIVIL"].ToString());
                        personaList.EstadoCivil = reader["ESTADO_CIVIL"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("FECHA_NACIMIENTO")))
                        {
                            personaList.FechaNacimiento = (DateTime)reader["FECHA_NACIMIENTO"];
                        }
                        personaList.RazonSocial = reader["RAZON_SOCIAL"].ToString();
                        personaList.NombreCompleto = reader["NOMBRE_COMPLETO"].ToString();
                        personaList.Celular = reader["CELULAR"].ToString();
                        personaList.Telefono = reader["TELEFONO"].ToString();
                        personaList.Correo = reader["CORREO"].ToString();
                        personaList.Contacto = reader["CONTACTO"].ToString();
                        personaList.TelefonoContacto = reader["TELEFONO_CONTACTO"].ToString();
                        personaList.CorreoContacto = reader["CORREO_CONTACTO"].ToString(); 
                        personaList.Ident_Ubigeo = Int32.Parse(reader["IDENT_UBIGEO"].ToString());
                        personaList.CodigoDepartamento = reader["DEPARTAMENTO"].ToString();
                        personaList.CodigoProvincia = reader["PROVINCIA"].ToString();
                        personaList.CodigoDistrito = reader["DISTRITO"].ToString();
                        personaList.Direccion = reader["DIRECCION"].ToString();
                        personaList.Referencia = reader["REFERENCIA"].ToString();
                        personaList.Ident004Estado = Int32.Parse(reader["IDENT_004_ESTADO"].ToString());
                        personaList.Estado = reader["ESTADO"].ToString();
                        personas.Add(personaList);
                    }
                    return personas;
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
        public async Task<PersonaList> Persona_XIdentPersona(int identPersona)
        {
            var personaList = new PersonaList();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Persona_XIdentPersona", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdentPersona", identPersona);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        personaList.Ident_Persona = Int32.Parse(reader["IDENT_PERSONA"].ToString());
                        personaList.Ident_001_TipoDocumento = Int32.Parse(reader["IDENT_001_TIPODOCUMENTO"].ToString());
                        personaList.TipoDocumento = reader["TIPO_DOCUMENTO"].ToString();
                        personaList.Documento = reader["DOCUMENTO"].ToString();
                        personaList.Paterno = reader["PATERNO"].ToString();
                        personaList.Materno = reader["MATERNO"].ToString();
                        personaList.Nombre = reader["NOMBRES"].ToString();
                        personaList.Ident006TipoEstadoCivil = Int32.Parse(reader["IDENT_006_ESTADOCIVIL"].ToString());
                        if (!reader.IsDBNull(reader.GetOrdinal("FECHA_NACIMIENTO")))
                        {
                            personaList.FechaNacimiento = (DateTime)reader["FECHA_NACIMIENTO"];
                        }
                        personaList.EstadoCivil = reader["ESTADO_CIVIL"].ToString();
                        personaList.RazonSocial = reader["RAZON_SOCIAL"].ToString();
                        personaList.NombreCompleto = reader["NOMBRE_COMPLETO"].ToString();
                        personaList.Celular = reader["CELULAR"].ToString();
                        personaList.Telefono = reader["TELEFONO"].ToString();
                        personaList.Correo = reader["CORREO"].ToString();
                        personaList.Contacto = reader["CONTACTO"].ToString();
                        personaList.TelefonoContacto = reader["TELEFONO_CONTACTO"].ToString();
                        personaList.CorreoContacto = reader["CORREO_CONTACTO"].ToString();
                        personaList.Ident_Ubigeo = Int32.Parse(reader["IDENT_UBIGEO"].ToString());
                        personaList.CodigoDepartamento = reader["DEPARTAMENTO"].ToString();
                        personaList.CodigoProvincia = reader["PROVINCIA"].ToString();
                        personaList.CodigoDistrito = reader["DISTRITO"].ToString();
                        personaList.Direccion = reader["DIRECCION"].ToString();
                        personaList.Referencia = reader["REFERENCIA"].ToString();
                        personaList.Ident004Estado = Int32.Parse(reader["IDENT_004_ESTADO"].ToString());
                        personaList.Estado = reader["ESTADO"].ToString();
                    }
                    return personaList;
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
        public async Task<PersonaList> PersonaActualizar(PersonaList personaList, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Persona_Actualizar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Persona", personaList.Ident_Persona);
                    command.Parameters.AddWithValue("@ISPaterno", personaList.Paterno ?? "");
                    command.Parameters.AddWithValue("@ISMaterno", personaList.Materno ?? "");
                    command.Parameters.AddWithValue("@ISNombre", personaList.Nombre ?? "");
                    command.Parameters.AddWithValue("@ISIdent_006_EstadoCivil", personaList.Ident006TipoEstadoCivil);
                    command.Parameters.AddWithValue("@ISFechaNacimiento", personaList.FechaNacimiento);
                    command.Parameters.AddWithValue("@ISRazonSocial", personaList.RazonSocial ?? "");
                    command.Parameters.AddWithValue("@ISCelular", personaList.Celular ?? "");
                    command.Parameters.AddWithValue("@ISTelefono", personaList.Telefono ?? "");
                    command.Parameters.AddWithValue("@ISCorreo", personaList.Correo ?? "");
                    command.Parameters.AddWithValue("@ISContacto", personaList.Contacto ?? "");
                    command.Parameters.AddWithValue("@ISTelefonoContacto", personaList.TelefonoContacto ?? "");
                    command.Parameters.AddWithValue("@ISCorreoContacto", personaList.CorreoContacto ?? "");
                    command.Parameters.AddWithValue("@ISDepartamento", personaList.CodigoDepartamento);
                    command.Parameters.AddWithValue("@ISProvincia", personaList.CodigoProvincia);
                    command.Parameters.AddWithValue("@ISDistrito", personaList.CodigoDistrito);
                    command.Parameters.AddWithValue("@ISDireccion", personaList.Direccion ?? "");
                    command.Parameters.AddWithValue("@ISReferencia", personaList.Referencia ?? "");
                    command.Parameters.AddWithValue("@ISUsuarioModificacion", loginResult.IdentUsuario);
                    command.Parameters.AddWithValue("@ISAnular", 0);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        personaList.Mensaje= reader["MENSAJE"].ToString();

                    }
                    
                    return personaList;
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
        public async Task<string> PersonaAnular(int ident_Persona, LoginResult loginResult)
        {
            string mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Persona_Anular", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Persona", ident_Persona);
                    command.Parameters.AddWithValue("@ISIdent_UsuarioModificador", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        mensaje = reader["MENSAJE"].ToString();
                    }
                    return mensaje;
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
