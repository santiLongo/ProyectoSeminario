namespace EasyAfip.Core.Modelos;

public class SolicitudFactura
{
    /// <summary>Tipo AFIP: 1=FA, 6=FB, 11=FC</summary>
    public int TipoComprobante { get; set; }

    /// <summary>1=Productos, 2=Servicios, 3=Productos y Servicios</summary>
    public int Concepto { get; set; }

    /// <summary>80=CUIT, 86=CUIL, 96=DNI, 99=Consumidor Final</summary>
    public int DocTipo { get; set; }

    public long DocNro { get; set; }

    /// <summary>Fecha del comprobante YYYYMMDD. Null = AFIP usa fecha del proceso.</summary>
    public string FechaComprobante { get; set; }

    /// <summary>Obligatorio si Concepto = 2 o 3. Formato YYYYMMDD.</summary>
    public string FechaServicioDesde { get; set; }

    public string FechaServicioHasta { get; set; }

    public string FechaVencimientoPago { get; set; }

    public decimal ImporteNeto { get; set; }
    public decimal ImporteIVA { get; set; }
    public decimal ImporteNoGravado { get; set; }
    public decimal ImporteExento { get; set; }
    public decimal ImporteTributos { get; set; }
    public decimal ImporteTotal { get; set; }

    /// <summary>Código de moneda AFIP. PES = Peso Argentino.</summary>
    public string Moneda { get; set; } = "PES";

    public double Cotizacion { get; set; } = 1.0;

    /// <summary>Obligatorio para RG 5616. Código de condición IVA del receptor.</summary>
    public int? CondicionIVAReceptorId { get; set; }

    public List<AlicuotaIva> Ivas { get; set; } = new();
}

public class AlicuotaIva
{
    /// <summary>3=0%, 4=10.5%, 5=21%, 6=27%</summary>
    public int Id { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Importe { get; set; }
}
