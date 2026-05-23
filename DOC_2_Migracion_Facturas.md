# Plan de Migración — Sistema de Facturas y Recibos

**Fecha:** 2026-05-22  
**Objetivo:** Reemplazar `Cobro` y `Pago` por un sistema unificado de `Factura` + `Recibo` que soporte facturas internas de viajes, facturas externas de proveedores/talleres, y que sirva de base natural para una cuenta corriente futura.

---

## 1. Principios del Diseño

### Una sola entidad `Factura`
Una única tabla de facturas con un campo `TipoFactura` (Emitida / Recibida). La factura conoce directamente a su contraparte (cliente, proveedor o taller) y se conecta a los documentos de origen (viajes, mantenimientos, compras) mediante tablas intermedias.

### `FacturaDetalle` para los conceptos
Una factura puede tener múltiples ítems: servicio de transporte, estadía, flete adicional, mano de obra, repuestos, etc. Cada ítem es una fila en `FacturaDetalle`.

### `Recibo` como documento de pago unificado
Un `Recibo` reemplaza tanto a `Cobro` (dinero que entra) como a `Pago` (dinero que sale). La diferencia está en el `TipoRecibo`. Un recibo puede:
- Cubrir **una o más facturas** (imputación parcial o total).
- Pagarse con **múltiples formas de pago** (parte efectivo, parte cheque, parte transferencia).
- Existir **sin una factura** (anticipo o pago a cuenta).

### Cuenta corriente sin tabla extra
Con este modelo, el saldo de cuenta corriente de un cliente o proveedor es simplemente:

```
Saldo = sum(Facturas no anuladas) − sum(Importes imputados en Recibos)
```

No requiere ninguna tabla adicional. Cuando en el futuro se quiera exponer una pantalla de cuenta corriente, alcanza con una query sobre las tablas ya existentes.

---

## 2. Diagrama General

```
EMITIDAS (a clientes)                   RECIBIDAS (de proveedores/talleres)
─────────────────────────────────────   ─────────────────────────────────────
Viaje ──┐                               Mantenimiento ──┐
         ├── FacturaViaje ──┐            CompraRepuesto ─┤── FacturaMantenimiento
                             ├─ Factura ─┤               └── FacturaCompraRepuesto
         Cliente ───────────┘    │      Proveedor / Taller (FK directo en Factura)
                                 │
                           FacturaDetalle (ítems/conceptos)
                                 │
                           ReciboFactura (imputación)
                                 │
                              Recibo
                                 │
                         ReciboFormaPago (medios de pago)
                                 │
                           PagoCheque (si aplica)
```

---

## 3. Nuevas Entidades

### 3.1 `Factura`

```csharp
[Table("factura")]
public class Factura : IAuditable
{
    [Key]
    public int IdFactura { get; set; }

    public TipoFactura TipoFactura { get; set; }   // Emitida = 1, Recibida = 2

    [StringLength(30)]
    public string NroFactura { get; set; }          // "0001-00012345" (propio o del emisor)

    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }

    public decimal Subtotal { get; set; }
    public decimal PorcentajeIva { get; set; }      // 0, 10.5, 21, etc.
    public decimal Total { get; set; }              // Subtotal + IVA calculado

    public int IdMoneda { get; set; }
    public double? TipoCambio { get; set; }

    public EstadoFactura Estado { get; set; }       // Pendiente, PagoParcial, Cancelada, Anulada

    public string? Observaciones { get; set; }
    public bool Anulada { get; set; }

    // Contraparte (solo uno de los tres aplica según TipoFactura)
    public int? IdCliente { get; set; }             // Para Emitidas
    public int? IdProveedor { get; set; }           // Para Recibidas de proveedor
    public int? IdTaller { get; set; }              // Para Recibidas de taller

    // Auditoría
    public string? UserName { get; set; }
    public DateTime? UserDateTime { get; set; }

    // Navegación
    public virtual Moneda Moneda { get; set; }
    public virtual Cliente? Cliente { get; set; }
    public virtual Proveedor? Proveedor { get; set; }
    public virtual Taller? Taller { get; set; }

    public virtual ICollection<FacturaDetalle> Detalles { get; set; }
    public virtual ICollection<FacturaViaje> FacturasViaje { get; set; }
    public virtual ICollection<FacturaMantenimiento> FacturasMantenimiento { get; set; }
    public virtual ICollection<FacturaCompraRepuesto> FacturasCompraRepuesto { get; set; }
    public virtual ICollection<ReciboFactura> ReciboFacturas { get; set; }

    // Calcula si el total imputado en recibos cubre la factura
    public void RecalcularEstado()
    {
        var imputado = ReciboFacturas
            .Where(rf => !rf.Recibo.Anulado)
            .Sum(rf => rf.ImporteAplicado);

        if (imputado <= 0)
            Estado = EstadoFactura.Pendiente;
        else if (imputado >= Total)
            Estado = EstadoFactura.Cancelada;
        else
            Estado = EstadoFactura.PagoParcial;
    }
}

public enum TipoFactura  { Emitida = 1, Recibida = 2 }
public enum EstadoFactura { Pendiente = 1, PagoParcial = 2, Cancelada = 3, Anulada = 4 }
```

**SQL:**
```sql
CREATE TABLE factura (
    IdFactura        INT AUTO_INCREMENT PRIMARY KEY,
    TipoFactura      TINYINT NOT NULL,
    NroFactura       VARCHAR(30) NOT NULL,
    FechaEmision     DATE NOT NULL,
    FechaVencimiento DATE NULL,
    Subtotal         DECIMAL(18,2) NOT NULL,
    PorcentajeIva    DECIMAL(5,2) NOT NULL DEFAULT 0,
    Total            DECIMAL(18,2) NOT NULL,
    IdMoneda         INT NOT NULL,
    TipoCambio       DECIMAL(18,4) NULL,
    Estado           TINYINT NOT NULL DEFAULT 1,
    Observaciones    TEXT NULL,
    Anulada          TINYINT(1) NOT NULL DEFAULT 0,
    IdCliente        INT NULL,
    IdProveedor      INT NULL,
    IdTaller         INT NULL,
    UserName         VARCHAR(100) NULL,
    UserDateTime     DATETIME NULL,
    FOREIGN KEY (IdMoneda)    REFERENCES moneda(idMoneda),
    FOREIGN KEY (IdCliente)   REFERENCES cliente(idCliente),
    FOREIGN KEY (IdProveedor) REFERENCES proveedor(idProveedor),
    FOREIGN KEY (IdTaller)    REFERENCES taller(idTaller)
);
```

---

### 3.2 `FacturaDetalle`

Representa un ítem o concepto dentro de la factura.

```csharp
[Table("facturadetalle")]
public class FacturaDetalle
{
    [Key]
    public int IdFacturaDetalle { get; set; }

    public int IdFactura { get; set; }
    public int Orden { get; set; }                  // Posición en la factura

    [StringLength(200)]
    public string Descripcion { get; set; }

    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PorcentajeIva { get; set; }      // Puede variar por ítem
    public decimal Subtotal { get; set; }           // Cantidad * PrecioUnitario
    public decimal Total { get; set; }              // Subtotal + IVA del ítem

    public virtual Factura Factura { get; set; }
}
```

**SQL:**
```sql
CREATE TABLE facturadetalle (
    IdFacturaDetalle INT AUTO_INCREMENT PRIMARY KEY,
    IdFactura        INT NOT NULL,
    Orden            TINYINT NOT NULL DEFAULT 1,
    Descripcion      VARCHAR(200) NOT NULL,
    Cantidad         DECIMAL(10,2) NOT NULL DEFAULT 1,
    PrecioUnitario   DECIMAL(18,2) NOT NULL,
    PorcentajeIva    DECIMAL(5,2) NOT NULL DEFAULT 0,
    Subtotal         DECIMAL(18,2) NOT NULL,
    Total            DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (IdFactura) REFERENCES factura(IdFactura)
);
```

---

### 3.3 Tablas Intermedias (Factura ↔ Documentos de Origen)

**`FacturaViaje`** — viajes incluidos en una factura emitida. Un viaje puede estar en una sola factura activa.

```sql
CREATE TABLE facturaviaje (
    IdFacturaViaje INT AUTO_INCREMENT PRIMARY KEY,
    IdFactura      INT NOT NULL,
    IdViaje        INT NOT NULL,
    MontoViaje     DECIMAL(18,2) NOT NULL,   -- Monto de ese viaje en esta factura
    UNIQUE (IdViaje),                         -- Un viaje → una factura activa
    FOREIGN KEY (IdFactura) REFERENCES factura(IdFactura),
    FOREIGN KEY (IdViaje)   REFERENCES viaje(idViaje)
);
```

**`FacturaMantenimiento`** — mantenimientos cubiertos por una factura recibida de taller.

```sql
CREATE TABLE facturamantenimieno (
    IdFacturaMantenimiento INT AUTO_INCREMENT PRIMARY KEY,
    IdFactura              INT NOT NULL,
    IdMantenimiento        INT NOT NULL,
    ImporteMantenimiento   DECIMAL(18,2) NOT NULL,
    UNIQUE (IdFactura, IdMantenimiento),
    FOREIGN KEY (IdFactura)      REFERENCES factura(IdFactura),
    FOREIGN KEY (IdMantenimiento) REFERENCES mantenimiento(idMantenimiento)
);
```

**`FacturaCompraRepuesto`** — compras de repuestos cubiertas por una factura recibida de proveedor.

```sql
CREATE TABLE facturacomprarepuesto (
    IdFacturaCompraRepuesto INT AUTO_INCREMENT PRIMARY KEY,
    IdFactura               INT NOT NULL,
    IdCompraRepuesto        INT NOT NULL,
    ImporteCompra           DECIMAL(18,2) NOT NULL,
    UNIQUE (IdFactura, IdCompraRepuesto),
    FOREIGN KEY (IdFactura)        REFERENCES factura(IdFactura),
    FOREIGN KEY (IdCompraRepuesto) REFERENCES comprarepuesto(idCompraRepuesto)
);
```

---

### 3.4 `Recibo`

Reemplaza a `Cobro` (dinero que entra) y a `Pago` (dinero que sale). El `TipoRecibo` diferencia ambos casos.

```csharp
[Table("recibo")]
public class Recibo : IAuditable
{
    [Key]
    public int IdRecibo { get; set; }

    public TipoRecibo TipoRecibo { get; set; }      // Cobro = 1, Pago = 2

    public DateTime FechaRecibo { get; set; }

    public decimal MontoTotal { get; set; }          // Suma de todos los ReciboFormaPago

    public int IdMoneda { get; set; }
    public double? TipoCambio { get; set; }

    // Contraparte (solo uno aplica según TipoRecibo)
    public int? IdCliente { get; set; }             // Para TipoRecibo = Cobro
    public int? IdProveedor { get; set; }           // Para TipoRecibo = Pago (proveedor)
    public int? IdTaller { get; set; }              // Para TipoRecibo = Pago (taller)

    public string? Observaciones { get; set; }
    public bool Anulado { get; set; }

    // Auditoría
    public string? UserName { get; set; }
    public DateTime? UserDateTime { get; set; }

    // Navegación
    public virtual Moneda Moneda { get; set; }
    public virtual Cliente? Cliente { get; set; }
    public virtual Proveedor? Proveedor { get; set; }
    public virtual Taller? Taller { get; set; }

    public virtual ICollection<ReciboFormaPago> FormasDePago { get; set; }
    public virtual ICollection<ReciboFactura> ReciboFacturas { get; set; }
}

public enum TipoRecibo { Cobro = 1, Pago = 2 }
```

**SQL:**
```sql
CREATE TABLE recibo (
    IdRecibo     INT AUTO_INCREMENT PRIMARY KEY,
    TipoRecibo   TINYINT NOT NULL,
    FechaRecibo  DATE NOT NULL,
    MontoTotal   DECIMAL(18,2) NOT NULL,
    IdMoneda     INT NOT NULL,
    TipoCambio   DECIMAL(18,4) NULL,
    IdCliente    INT NULL,
    IdProveedor  INT NULL,
    IdTaller     INT NULL,
    Observaciones TEXT NULL,
    Anulado      TINYINT(1) NOT NULL DEFAULT 0,
    UserName     VARCHAR(100) NULL,
    UserDateTime DATETIME NULL,
    FOREIGN KEY (IdMoneda)    REFERENCES moneda(idMoneda),
    FOREIGN KEY (IdCliente)   REFERENCES cliente(idCliente),
    FOREIGN KEY (IdProveedor) REFERENCES proveedor(idProveedor),
    FOREIGN KEY (IdTaller)    REFERENCES taller(idTaller)
);
```

---

### 3.5 `ReciboFormaPago`

Detalla los medios de pago usados en un recibo. Un recibo puede tener múltiples filas: efectivo + cheque, transferencia + cheque, etc.

```csharp
[Table("reciboformapago")]
public class ReciboFormaPago
{
    [Key]
    public int IdReciboFormaPago { get; set; }

    public int IdRecibo { get; set; }
    public int IdFormaPago { get; set; }
    public decimal Monto { get; set; }
    public int? IdPagoCheque { get; set; }          // Solo si IdFormaPago = Cheque

    public virtual Recibo Recibo { get; set; }
    public virtual FormaPago FormaPago { get; set; }
    public virtual PagoCheque? PagoCheque { get; set; }
}
```

**SQL:**
```sql
CREATE TABLE reciboformapago (
    IdReciboFormaPago INT AUTO_INCREMENT PRIMARY KEY,
    IdRecibo          INT NOT NULL,
    IdFormaPago       INT NOT NULL,
    Monto             DECIMAL(18,2) NOT NULL,
    IdPagoCheque      INT NULL,
    FOREIGN KEY (IdRecibo)     REFERENCES recibo(IdRecibo),
    FOREIGN KEY (IdFormaPago)  REFERENCES formapago(idFormaPago),
    FOREIGN KEY (IdPagoCheque) REFERENCES pagocheque(idPagoCheque)
);
```

---

### 3.6 `ReciboFactura`

Imputación: indica a qué factura(s) se aplica el recibo y por cuánto. Es la tabla que conecta recibos con facturas y permite calcular el saldo.

```csharp
[Table("recibofactura")]
public class ReciboFactura
{
    [Key]
    public int IdReciboFactura { get; set; }

    public int IdRecibo { get; set; }
    public int IdFactura { get; set; }
    public decimal ImporteAplicado { get; set; }

    public virtual Recibo Recibo { get; set; }
    public virtual Factura Factura { get; set; }
}
```

**SQL:**
```sql
CREATE TABLE recibofactura (
    IdReciboFactura INT AUTO_INCREMENT PRIMARY KEY,
    IdRecibo        INT NOT NULL,
    IdFactura       INT NOT NULL,
    ImporteAplicado DECIMAL(18,2) NOT NULL,
    UNIQUE (IdRecibo, IdFactura),
    FOREIGN KEY (IdRecibo)  REFERENCES recibo(IdRecibo),
    FOREIGN KEY (IdFactura) REFERENCES factura(IdFactura)
);
```

---

## 4. Cambios en Entidades Existentes

### 4.1 `PagoCheque` — nuevo FK a `ReciboFormaPago`

`PagoCheque` se reutiliza tal cual. Solo se agrega la columna `IdReciboFormaPago`. Las columnas antiguas (`IdCobro` implícito vía `Cobro.IdPagoCheque` e `IdPago` implícito vía `Pago.IdPagoCheque`) quedan para los datos históricos.

```sql
ALTER TABLE pagocheque ADD COLUMN IdReciboFormaPago INT NULL;
ALTER TABLE pagocheque ADD FOREIGN KEY (IdReciboFormaPago) 
    REFERENCES reciboformapago(IdReciboFormaPago);
```

### 4.2 `Viaje` — nuevos estados

Se agrega el estado 4 (Facturado) con significado ajustado. El estado "Cobrado" pasa a vivir en `Factura.Estado`, no en `Viaje`.

| Código | Nombre | Condición |
|---|---|---|
| 1 | En Viaje | Recién creado, en curso |
| 2 | Finalizado | `FechaDescarga` registrada |
| 3 | Suspendido | Cambio manual |
| 4 | Facturado | Existe una `FacturaViaje` activa |

La lógica del método `Modified()` se simplifica:

```csharp
public void Modified()
{
    this.PrecioKm = (float)(MontoTotal / Kilometros);

    if (FechaDescarga <= DateTime.Today.AddDays(1)
        && Estado == EstadosViaje.EnViaje.ToInt()
        && FechaDescarga is not null)
        Estado = EstadosViaje.Finalizado.ToInt();

    // Ya no calcula "Cobrado" aquí — eso lo maneja Factura.RecalcularEstado()
}
```

---

## 5. Qué se Reemplaza, Qué se Conserva

| Entidad actual | Reemplazada por | Acción |
|---|---|---|
| `Cobro` | `Recibo` (TipoRecibo=Cobro) + `ReciboFormaPago` + `ReciboFactura` | Conservar tabla, renombrar a `cobro_hist` después de migrar |
| `Pago` | `Recibo` (TipoRecibo=Pago) + `ReciboFormaPago` + `ReciboFactura` | Conservar tabla, renombrar a `pago_hist` después de migrar |
| `PagoMantenimiento` | `FacturaMantenimiento` + `ReciboFactura` | Conservar como historial |
| `PagoCompraRepuesto` | `FacturaCompraRepuesto` + `ReciboFactura` | Conservar como historial |
| `PagoCheque` | Se reutiliza, vinculado a `ReciboFormaPago` | Agregar columna `IdReciboFormaPago` |
| `FormaPago` | Sin cambio | — |
| `Moneda` | Sin cambio | — |
| `Banco` | Sin cambio | — |
| `Viaje` | Sin cambio estructural, solo lógica de estado | — |
| `Mantenimiento` | Sin cambio | — |
| `CompraRepuesto` | Sin cambio | — |
| `Proveedor` | Sin cambio | — |
| `Taller` | Sin cambio | — |
| `Cliente` | Sin cambio | — |

---

## 6. Nuevos Servicios CQRS

### `FacturasServices`

```
AddFacturaEmitidaHandler
  - Recibe: IdCliente, NroFactura, FechaEmision, FechaVencimiento, PorcentajeIva, IdMoneda, TipoCambio
  - Recibe: Detalles[] (descripcion, cantidad, precioUnitario, porcentajeIva)
  - Recibe: ViajesIds[] con MontoViaje para cada uno
  - Valida: viajes en estado Finalizado (2) y sin FacturaViaje activa
  - Crea Factura + FacturaDetalle[] + FacturaViaje[]
  - Actualiza estado de cada Viaje a 4 (Facturado)
  - Calcula Subtotal y Total desde los Detalles

AddFacturaRecibidaHandler
  - Recibe: TipoOrigen (proveedor/taller), IdProveedor o IdTaller
  - Recibe: NroFactura (del emisor), FechaEmision, FechaVencimiento, PorcentajeIva
  - Recibe: Detalles[] (conceptos, importes)
  - Recibe: IdMantenimiento[] o IdCompraRepuesto[] vinculados (opcionales)
  - Crea Factura + FacturaDetalle[] + tablas intermedias correspondientes

GetFacturaHandler
  - Devuelve factura completa: detalles, documentos vinculados, recibos imputados, saldo pendiente

GetAllFacturasHandler
  - Filtros: TipoFactura, IdCliente, IdProveedor, IdTaller, Estado, FechaEmision desde/hasta
  - Incluye saldo pendiente por factura

AnularFacturaHandler
  - Valida que no tenga recibos imputados
  - Marca Anulada = true, Estado = Anulada
  - Revierte estado de Viajes vinculados a 2 (Finalizado)
```

### `RecibosServices`

```
AddReciboHandler
  - Recibe: TipoRecibo, FechaRecibo, IdCliente/IdProveedor/IdTaller, IdMoneda, TipoCambio
  - Recibe: FormasDePago[] (IdFormaPago, Monto, datos de cheque si aplica)
  - Recibe: Imputaciones[] (IdFactura, ImporteAplicado) — puede estar vacío (anticipo)
  - Valida: sum(FormasDePago.Monto) == sum(Imputaciones.ImporteAplicado) si hay imputaciones
  - Valida: sum(Imputaciones) <= Factura.Total para cada factura imputada
  - Crea Recibo + ReciboFormaPago[] + ReciboFactura[]
  - Si forma de pago es cheque → crea PagoCheque + vincula a ReciboFormaPago
  - Recalcula estado de cada Factura imputada

ImputarReciboHandler
  - Para anticipos: imputar un Recibo existente (sin facturas) a una Factura nueva
  - Crea ReciboFactura con ImporteAplicado
  - Recalcula estado de la Factura

AnularReciboHandler
  - Marca Anulado = true
  - Si tenía cheques → marca PagoCheque.Rechazado = true
  - Recalcula estado de todas las Facturas afectadas
  - Revierte el pago parcial o total de cada factura

GetReciboHandler
  - Devuelve recibo con formas de pago y facturas imputadas

GetAllRecibosHandler
  - Filtros: TipoRecibo, IdCliente/IdProveedor/IdTaller, Estado, Fecha desde/hasta
```

---

## 7. Nuevos Endpoints API

### `/api/v1/facturas`

| Método | Ruta | Acción |
|---|---|---|
| POST | `/add-emitida` | Crear factura al cliente (con viajes) |
| POST | `/add-recibida` | Cargar factura de proveedor/taller |
| GET | `/get` | Obtener factura completa |
| GET | `/getAll` | Listar con filtros y saldo |
| POST | `/anular` | Anular factura |
| GET | `/viajes-disponibles` | Viajes en estado Finalizado disponibles para facturar |

### `/api/v1/recibos`

| Método | Ruta | Acción |
|---|---|---|
| POST | `/add` | Registrar recibo (cobro o pago) |
| POST | `/imputar` | Imputar recibo existente a una factura |
| GET | `/get` | Obtener recibo completo |
| GET | `/getAll` | Listar con filtros |
| POST | `/anular` | Anular recibo |

### `/api/v1/cheques` — sin cambios en interfaz, con datos extendidos

| Método | Ruta | Acción |
|---|---|---|
| GET | `/getAll` | Lista cheques por estado (igual que antes) |

---

## 8. Flujos del Nuevo Sistema

### 8.1 Factura Emitida → Recibo de Cobro

```
Cliente cierra uno o varios viajes
             ↓
Operador crea la Factura Emitida
  - Selecciona los viajes a incluir
  - Carga los conceptos en FacturaDetalle
  - El sistema calcula Subtotal + IVA = Total
  - Viajes pasan a Estado 4 (Facturado)
             ↓
Vence el plazo → el cliente paga
             ↓
Operador registra el Recibo (TipoRecibo = Cobro)
  - Elige el cliente
  - Carga las formas de pago: efectivo $X, cheque $Y
  - Si cheque → ingresa NroCheque, Banco, FechaCobro (vencimiento = +30 días)
  - Imputa el recibo a la factura (ReciboFactura con ImporteAplicado)
             ↓
El sistema recalcula el estado de la Factura:
  - sum(Imputados) >= Total → Estado: Cancelada
  - sum(Imputados) > 0     → Estado: PagoParcial
```

### 8.2 Factura Recibida → Recibo de Pago

```
Taller o proveedor emite una factura en papel
             ↓
Operador carga la Factura Recibida
  - Elige TipoOrigen: Taller / Proveedor
  - Vincula al Mantenimiento o CompraRepuesto correspondiente (opcional)
  - Ingresa NroFactura del emisor, montos, fechas
  - Carga los conceptos en FacturaDetalle
             ↓
Llega la fecha de pago
             ↓
Operador registra el Recibo (TipoRecibo = Pago)
  - Elige el proveedor o taller
  - Carga la forma de pago (cheque propio, transferencia, etc.)
  - Imputa a la factura recibida
             ↓
El sistema recalcula el estado de la Factura Recibida
```

### 8.3 Anticipo (Recibo sin Factura)

```
Cliente paga por adelantado antes de que exista una factura
             ↓
Operador registra el Recibo sin imputaciones
  - TipoRecibo = Cobro, IdCliente = X
  - Sin ReciboFactura (ImporteAplicado = 0)
             ↓
Cuando se emite la factura posterior:
  - Operador usa "Imputar Recibo" para aplicar el anticipo a la factura
  - Crea ReciboFactura con el importe del anticipo
  - La factura queda en PagoParcial o Cancelada según el monto
```

---

## 9. Base para Cuenta Corriente (Futura)

Con las tablas del nuevo sistema, la cuenta corriente de un cliente es una query directa. No requiere ninguna entidad nueva:

```sql
-- Saldo de cuenta corriente por cliente
SELECT
    f.IdCliente,
    SUM(f.Total)                                           AS TotalFacturado,
    COALESCE(SUM(rf.ImporteAplicado), 0)                  AS TotalCobrado,
    SUM(f.Total) - COALESCE(SUM(rf.ImporteAplicado), 0)  AS SaldoPendiente
FROM factura f
LEFT JOIN recibofactura rf ON rf.IdFactura = f.IdFactura
LEFT JOIN recibo r ON r.IdRecibo = rf.IdRecibo AND r.Anulado = 0
WHERE f.TipoFactura = 1          -- Solo emitidas
  AND f.Anulada = 0
  AND f.IdCliente = @IdCliente
GROUP BY f.IdCliente;
```

```sql
-- Saldo de cuenta corriente por proveedor
SELECT
    f.IdProveedor,
    SUM(f.Total)                                           AS TotalFacturado,
    COALESCE(SUM(rf.ImporteAplicado), 0)                  AS TotalPagado,
    SUM(f.Total) - COALESCE(SUM(rf.ImporteAplicado), 0)  AS SaldoPendiente
FROM factura f
LEFT JOIN recibofactura rf ON rf.IdFactura = f.IdFactura
LEFT JOIN recibo r ON r.IdRecibo = rf.IdRecibo AND r.Anulado = 0
WHERE f.TipoFactura = 2          -- Solo recibidas
  AND f.Anulada = 0
  AND f.IdProveedor = @IdProveedor
GROUP BY f.IdProveedor;
```

Cuando se quiera exponer una pantalla de cuenta corriente: se crea un endpoint `GET /api/v1/cuenta-corriente/cliente/{id}` que ejecuta la query anterior y devuelve el historial de movimientos (facturas y recibos entremezclados en orden cronológico) más el saldo.

---

## 10. Migración de Datos Históricos

### 10.1 Migrar `Cobro` → `Factura` + `Recibo`

Por cada `Cobro` histórico:

```sql
-- 1. Crear la Factura desde el Viaje
INSERT INTO factura (TipoFactura, NroFactura, FechaEmision, Subtotal, PorcentajeIva, Total,
                     IdMoneda, TipoCambio, Estado, IdCliente)
SELECT 1,
       CONCAT('HIST-', v.NroViaje),
       c.FechaRecibo,
       v.MontoTotal, 0, v.MontoTotal,
       COALESCE(v.IdMoneda, 1), v.TipoCambio,
       CASE WHEN v.Estado = 4 THEN 3 ELSE 1 END,
       v.IdCliente
FROM cobro c
JOIN viaje v ON v.idViaje = c.IdViaje;

-- 2. Crear el vínculo FacturaViaje
INSERT INTO facturaviaje (IdFactura, IdViaje, MontoViaje)
SELECT f.IdFactura, c.IdViaje, v.MontoTotal
FROM cobro c
JOIN factura f ON f.NroFactura = CONCAT('HIST-', v.NroViaje)
JOIN viaje v ON v.idViaje = c.IdViaje;

-- 3. Crear el Recibo
INSERT INTO recibo (TipoRecibo, FechaRecibo, MontoTotal, IdMoneda, TipoCambio, IdCliente)
SELECT 1, c.FechaRecibo, c.Monto, c.idMoneda, c.TipoCambio, v.IdCliente
FROM cobro c
JOIN viaje v ON v.idViaje = c.IdViaje;

-- 4. Crear ReciboFormaPago
INSERT INTO reciboformapago (IdRecibo, IdFormaPago, Monto, IdPagoCheque)
SELECT r.IdRecibo, c.idFormaPago, c.Monto, c.IdPagoCheque
FROM cobro c
JOIN recibo r ON r.FechaRecibo = c.FechaRecibo; -- ajustar join real

-- 5. Imputar el Recibo a la Factura
INSERT INTO recibofactura (IdRecibo, IdFactura, ImporteAplicado)
SELECT r.IdRecibo, f.IdFactura, r.MontoTotal
FROM recibo r
JOIN factura f ON ...; -- join por IdCliente + fecha
```

> **Nota:** El script exacto depende de cómo estén los datos. Se recomienda ejecutar primero en ambiente de staging y comparar saldos con los Cobros originales.

### 10.2 Migrar `Pago` + `PagoMantenimiento` → `Factura` + `Recibo`

Mismo patrón: por cada `Mantenimiento` con pagos asociados se crea una `Factura` (TipoFactura=Recibida, IdTaller) con un `FacturaMantenimiento`, y el `Pago` se convierte en un `Recibo` + `ReciboFormaPago` + `ReciboFactura`.

### 10.3 Conservar datos históricos

No eliminar las tablas `cobro`, `pago`, `pagomantenimiento`, `pagocomprarepuesto`. Renombrarlas con prefijo `_hist_` una vez validada la migración, y mantenerlas al menos 12 meses para auditoría.

---

## 11. Orden de Implementación

| Paso | Tarea |
|---|---|
| 1 | Crear las nuevas tablas en MySQL (`factura`, `facturadetalle`, `facturaviaje`, `facturamantenimieno`, `facturacomprarepuesto`, `recibo`, `reciboformapago`, `recibofactura`) |
| 2 | Agregar columna `IdReciboFormaPago` a `pagocheque` |
| 3 | Agregar las nuevas entidades al `AppDbContext` (6 nuevos DbSet) |
| 4 | Crear repositorios: `IFacturasRepo`, `IRecibosRepo` |
| 5 | Crear servicios CQRS para `Facturas` (AddEmitida, AddRecibida, Get, GetAll, Anular) |
| 6 | Crear servicios CQRS para `Recibos` (Add, Imputar, Get, GetAll, Anular) |
| 7 | Crear controladores `/api/v1/facturas` y `/api/v1/recibos` |
| 8 | Actualizar la lógica `Viaje.Modified()` (remover auto-Cobrado, agregar estado Facturado) |
| 9 | Ejecutar script de migración de datos históricos en staging |
| 10 | Validar saldos: sum(Cobros históricos) == sum(Recibos migrados) por cliente |
| 11 | Desactivar (no eliminar) endpoints de `Cobros` y `Pagos` antiguos |
| 12 | Renombrar tablas históricas a `_hist_` |
