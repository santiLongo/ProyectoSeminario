using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminario.Services.Login.Command
{
    public class AuthCommand
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        [StringLength(100,ErrorMessage = "El usuario no debe superar los 100 caracteres")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(30,ErrorMessage = "La contraseña no debe superar los 30 caracteres")]
        public string Password { get; set; }
    }
}
