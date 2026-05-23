namespace Seminario.Datos.Entidades;

public class FacturaMantenimiento
{
    public int IdFacturaMantenimiento { get; set; }

    public int IdFactura { get; set; }

    public int IdMantenimiento { get; set; }

    public decimal ImporteMantenimiento { get; set; }


    // Navigation Properties
    public virtual Factura Factura { get; set; }

    public virtual Mantenimiento Mantenimiento { get; set; }
}