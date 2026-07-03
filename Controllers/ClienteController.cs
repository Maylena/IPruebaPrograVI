using IPruebaPrograVI.Models;
using Microsoft.AspNetCore.Mvc;
using IPruebaPrograVI.Repositorios;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IPruebaPrograVI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly string _cadenaConexion = string.Empty;

        public ClienteController(IConfiguration configuracion)
        {
            _cadenaConexion = configuracion.GetConnectionString("DefaultConnection")!;
        }

        // POST: api/cliente
        [HttpPost]
        public IActionResult Post([FromBody] Cliente cliente)
        {
            if (cliente == null || string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrEmpty(cliente.Correo))
                return BadRequest(new { mensaje = "El Nombre y el Correo son obligatorios." });

            try
            {
                using (SqlConnection con = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_InsertarCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", string.IsNullOrEmpty(cliente.Telefono) ? (object)DBNull.Value : cliente.Telefono);
                    cmd.Parameters.AddWithValue("@usuario_creacion", "AdminAPI");

                    con.Open();
                    var idGenerado = cmd.ExecuteScalar();
                    cliente.Id = Convert.ToInt32(idGenerado);
                }
                return Ok(new { mensaje = "Cliente insertado correctamente", id = cliente.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }
    }

}
