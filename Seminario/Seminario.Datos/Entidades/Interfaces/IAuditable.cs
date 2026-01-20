namespace Seminario.Datos.Entidades.Interfaces;

public interface IAuditable
{
    void CreatedAt(DateTime date, string user);
    void ModifiedAt(DateTime date, string user);
}