using Seminario.Core.Type.ComboTypes.Interface;

namespace Seminario.Services.CombosServices.Factory.Interface;

public interface IGetComboData
{
    IEnumerable<ICombo> GetCombo();
}