using Seminario.Core.Type.ComboTypes.Interface;

namespace Seminario.Core.Type.ComboTypes;

public class ComboIntModel : ICombo
{
    public int Numero { get; set; }
    public string Descripcion { get; set; }
    
    object ICombo.Numero => Numero;
}