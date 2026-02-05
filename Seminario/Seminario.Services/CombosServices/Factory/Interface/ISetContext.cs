using Seminario.Datos.Contextos.AppDbContext;

namespace Seminario.Services.CombosServices.Factory.Interface;

public interface ISetContext
{
    void SetContext(IAppDbContext context);
}