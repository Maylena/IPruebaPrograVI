using IPruebaPrograVI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IPruebaPrograVI.Repositorios
{
    public class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> InsertarClienteAsync(Cliente cliente, string usuarioCreacion)
        {
            int nuevoId = 0;

            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                using (SqlCommand comando = new SqlCommand("usp_InsertarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    comando.Parameters.AddWithValue("@Correo", cliente.Correo);

                    // Manejo del valor nulo para el teléfono
                    if (string.IsNullOrEmpty(cliente.Telefono))
                    {
                        comando.Parameters.AddWithValue("@Telefono", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    }

                    comando.Parameters.AddWithValue("@usuario_creacion", usuarioCreacion);

                    await conexion.OpenAsync();

                    //Asegura que se devuelva el ultimo ID
                    object resultado = await comando.ExecuteScalarAsync();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        nuevoId = Convert.ToInt32(resultado);
                    }
                }
            }

            return nuevoId;
        }
    }
}
