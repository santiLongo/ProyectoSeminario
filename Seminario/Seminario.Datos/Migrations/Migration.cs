using Seminario.Datos.Dapper;

namespace Seminario.Datos.Migrations;

public class Migration : BaseMigrations.BaseMigrations
{
    protected override int Version => 4;
    protected override string Modulo => "SeminarioMain";
    protected override string ResourcePath => "Seminario.Datos.Migrations.Migrations";
    
    public Migration(IDbExecutor dbExecutor, IDbSession dbSession) : base(dbExecutor, dbSession)
    {
    }
}