using Seminario.Datos.Type.ComboTypes.Interface;

namespace Seminario.Datos.Type.ComboTypes;

public class ComboStringModel : ICombo
{
    public string Numero { get; set; }
    public string Descripcion { get; set; }
    
    object ICombo.Numero => Numero;
}