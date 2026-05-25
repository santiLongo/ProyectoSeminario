namespace EasyAfip.Core.Modelos;

public class RespuestaAfip
{
    /// <summary>A=Aprobado, R=Rechazado, P=Aprobado con observaciones</summary>
    public string Resultado { get; set; }

    /// <summary>Código de Autorización Electrónica (14 dígitos)</summary>
    public string CAE { get; set; }

    /// <summary>Vencimiento del CAE formato YYYYMMDD</summary>
    public string CAEFchVto { get; set; }

    public bool Reproceso { get; set; }

    public List<string> Observaciones { get; set; } = new();
    public List<string> Errores { get; set; } = new();

    public bool Aprobado => Resultado == "A" || Resultado == "P";
}

public class DummyResponse
{
    public string AppServer { get; set; }
    public string DbServer { get; set; }
    public string AuthServer { get; set; }
    public bool Ok => AppServer == "OK" && DbServer == "OK" && AuthServer == "OK";
}
