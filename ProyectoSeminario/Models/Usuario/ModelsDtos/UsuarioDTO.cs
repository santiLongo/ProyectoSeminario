using System.ComponentModel.DataAnnotations;

namespace ProyectoSeminario.Models.Usuario.ModelsDtos
{

    //Clase destinada a enviar la informacion necesaria para el usuario
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string Mail { get; set; }

        public string Role { get; set; }
    }
}