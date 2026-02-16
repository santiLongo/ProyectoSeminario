using Seminario.Services.CombosServices.Factory.Implementacion;
using Seminario.Services.CombosServices.Factory.Interface;

namespace Seminario.Services.CombosServices.Factory.Resolver;

public static class ComboResolver
{
    public static IGetComboData Resolve(string type)
    {
        switch (type)
        {
            case "ComboChoferes":
                return new ComboChofer();
            case "ComboCamiones":
                return new ComboCamion();
            case "ComboClientes":
                return new ComboCliente();
            case "ComboDestinoProcedencias":
                return new ComboDestinoProcedencia();
            case "ComboEstadosViaje":
                return new ComboEstadosViaje();
            case "ComboMoneda":
                return new ComboMoneda();
            default:
                throw new NotImplementedException($"El combo {type}, no se encuentra implementado");
        }
        
    }
}