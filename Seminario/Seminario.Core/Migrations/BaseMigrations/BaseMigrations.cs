using Dapper;
using Seminario.Core.Dapper;

namespace Seminario.Core.Migrations.BaseMigrations;

public abstract class BaseMigrations
{
    private readonly IDbExecutor _dbExecutor;
    private readonly IDbSession _session;

    protected abstract int Version { get; }
    protected abstract string Modulo { get; }
    protected abstract string ResourcePath { get; }

    protected BaseMigrations(
        IDbExecutor dbExecutor,
        IDbSession dbSession)
    {
        _dbExecutor = dbExecutor;
        _session = dbSession;
    }

    public async Task MigrarAsync()
    {
        var versionActual = await ObtenerUltimaVersionAsync();

        while (versionActual < Version)
        {
            var proximaVersion = versionActual + 1;

            var archivo = $"migration_{proximaVersion}.sql";

            try
            {
                var sql = LeerSqlEmbebido(proximaVersion);

                await _dbExecutor.ExecuteAsync<int>(sql);

                await RegistrarMigracionAsync(
                    proximaVersion,
                    archivo,
                    true,
                    null);
            }
            catch (Exception ex)
            {
                await RegistrarMigracionAsync(
                    proximaVersion,
                    archivo,
                    false,
                    ex.ToString());
            }
            
            versionActual = proximaVersion;
        }
    }

    private async Task<int> ObtenerUltimaVersionAsync()
    {
        const string sql = @"
            SELECT MAX(Version)
            FROM MigrationsHistory
            WHERE Modulo = @modulo;
        ";

        var parametros = new DynamicParameters();

        parametros.Add("modulo", Modulo);

        var version = await _dbExecutor.ExecuteFirstOrDefaultAsync<int?>(sql, parametros);

        return version ?? 0;
    }

    private async Task RegistrarMigracionAsync(
        int version,
        string archivo,
        bool ejecutado,
        string? error)
    {
        const string sql = @"
            INSERT INTO MigrationsHistory
            (
                Modulo,
                Version,
                Archivo,
                Ejecutado,
                Error
            )
            VALUES
            (
                @modulo,
                @version,
                @archivo,
                @ejecutado,
                @error
            )
            ON DUPLICATE KEY UPDATE
                Ejecutado = VALUES(Ejecutado),
                Error = VALUES(Error),
                FechaEjecucion = CURRENT_TIMESTAMP;
        ";

        var parametros = new DynamicParameters();

        parametros.Add("modulo", Modulo);
        parametros.Add("version", version);
        parametros.Add("archivo", archivo);
        parametros.Add("ejecutado", ejecutado);
        parametros.Add("error", error?.Substring(0, 1000));

        await _dbExecutor.ExecuteAsync<int>(sql, parametros);
    }

    private string LeerSqlEmbebido(int version)
    {
        var assembly = GetType().Assembly;

        var resourceName = $"{ResourcePath}.migration_{version}.sql";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
            throw new Exception($"No se encontró el recurso embebido: {resourceName}");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}