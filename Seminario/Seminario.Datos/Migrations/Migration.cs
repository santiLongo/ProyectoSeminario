using Seminario.Core.Dapper;
using Seminario.Core.Migrations.BaseMigrations;

namespace Seminario.Datos.Migrations;

public class Migration : BaseMigrations
{
    protected override int Version => 7;
    protected override string Modulo => "SeminarioMain";
    protected override string ResourcePath => "Seminario.Datos.Migrations.Migrations";
    
    public Migration(IDbExecutor dbExecutor, IDbSession dbSession) : base(dbExecutor, dbSession)
    {
    }
}