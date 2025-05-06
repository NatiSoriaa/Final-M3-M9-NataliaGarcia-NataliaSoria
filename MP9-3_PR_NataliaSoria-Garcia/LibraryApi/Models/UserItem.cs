using System.Reflection.Metadata;

namespace UserApi.Models;

public class UserItem
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    // public Blob? Avatar { get; set; } = null;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }= string.Empty;
    
}


//RECUPERAR Y GUARDAR AVATAR EN BLOB REVISAR (EJEMMPLO CHATGPT/CONVERSION DE EJEMPLO BLOG)
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Data.SQLite;
// using System.IO;
// using System.Threading.Tasks;

// public class ImagenesController : Controller
// {
//     [HttpPost]
//     public async Task<IActionResult> SubirImagen(IFormFile archivo)
//     {
//         if (archivo == null || archivo.Length == 0)
//             return BadRequest("No se seleccionó ninguna imagen.");

//         string nombreArchivo = "mi_base_de_datos.db";

//         try
//         {
//             using var conexion = new SQLiteConnection($"Data Source={nombreArchivo};Version=3;");
//             await conexion.OpenAsync();

//             using var ms = new MemoryStream();
//             await archivo.CopyToAsync(ms);
//             byte[] imagenBytes = ms.ToArray();

//             using var comando = new SQLiteCommand("INSERT INTO imagenes (imagen) VALUES (@imagen)", conexion);
//             comando.Parameters.AddWithValue("@imagen", imagenBytes);
//             await comando.ExecuteNonQueryAsync();

//             return Ok("Imagen guardada correctamente.");
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, $"Error al guardar la imagen: {ex.Message}");
//         }
//     }
// }

// <form asp-controller="Imagenes" asp-action="SubirImagen" method="post" enctype="multipart/form-data">
//     <input type="file" name="archivo" />
//     <button type="submit">Subir Imagen</button>
// </form>