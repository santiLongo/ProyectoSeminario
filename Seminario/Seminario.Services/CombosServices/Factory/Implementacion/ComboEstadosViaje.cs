using System.Collections;
using Seminario.Datos;
using Seminario.Datos.Enums;
using Seminario.Core.Type.ComboTypes;
using Seminario.Core.Type.ComboTypes.Interface;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Implementacion;

public class ComboEstadosViaje : IGetComboData
{
    public IEnumerable<ICombo> GetCombo()
    {
        return Enum.GetValues<EstadosViaje>()
            .Where(e => e != EstadosViaje.Facturado && e != EstadosViaje.Cobrado)
            .Select(e => new ComboIntModel
            {
                Numero = (int)e,
                Descripcion = e.ToString()
            });
    }
}