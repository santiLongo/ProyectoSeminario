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
            case "ComboProvincia":
                return new ComboProvincia();
            case "ComboPais":
                return new ComboPais();
            case "ComboTipoCamion":
                return new ComboTipoCamion();
            case "ComboMarcaCamion":
                return new ComboMarcaCamion();
            case "ComboModeloCamion":
                return new ComboModeloCamion();
            case "ComboCamionesDisponibles":
                return new ComboCamionesDisponibles();
            case "ComboFormaPago":
                return new ComboFormaPago();
            case "ComboBanco":
                return new ComboBanco();
            default:
                throw new NotImplementedException($"El combo {type}, no se encuentra implementado");
        }
        
    }
}