using Seminario.Core.Type.ComboTypes.Interface;

namespace Seminario.Core.Type.ComboTypes;

public class ComboStringModel : ICombo
{
    public string Numero { get; set; }
    public string Descripcion { get; set; }
    
    object ICombo.Numero => Numero;
}