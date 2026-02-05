using System.Collections;
using Seminario.Datos;
using Seminario.Datos.Type.ComboTypes;
using Seminario.Datos.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboEstadosViaje : IGetComboData
{
    public IEnumerable<ICombo> GetCombo()
    {
        return Enum.GetValues<EstadosViaje>()
            .Select(e => new ComboIntModel
            {
                Numero = (int)e,
                Descripcion = e.ToString()
            });
    }
}