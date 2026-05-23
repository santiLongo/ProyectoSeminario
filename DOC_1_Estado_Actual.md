# Sistema de Gestión de Flota — Estado Actual

**Fecha de análisis:** 2026-05-22  
**Tecnologías:** .NET 8, ASP.NET Core, Entity Framework Core, Dapper, MySQL 9.3, JWT

---

## 1. Visión General

El sistema es una aplicación de gestión de flota y logística para una empresa de transporte de carga. Permite administrar viajes, vehículos, choferes, clientes, mantenimientos y proveedores, con un módulo de cobros y pagos integrado.

### Arquitectura en capas

```
Seminario.Api         →  Controladores REST + Middleware + JWT
Seminario.Services    →  Lógica de negocio (patrón CQRS: Commands + Handlers)
Seminario.Datos       →  Repositorios + Entidades EF Core + Contexto MySQL
```

---

## 2. Entidades del Sistema

### 2.1 Entidades de Operación (Viajes)

| Entidad | Descripción |
|---|---|
| `Viaje` | Viaje/flete. Entidad central del sistema. Genera la deuda del cliente. |
| `Cliente` | Empresa que contrata los viajes. Tiene CUIT, razón social, contacto. |
| `Chofer` | Conductor asignado al viaje. |
| `Camion` | Vehículo tractor. Identificado por patente, chasis, motor. |
| `TipoCamion` | Categoría del vehículo (camión, tractor, semirremolque). |
| `Destino` | Punto(s) de entrega del viaje. |
| `Procedencia` | Punto(s) de origen del viaje. |
| `ViajeObservacion` | Registro de notas y cambios en el viaje (trazabilidad). |

**Campos clave de `Viaje`:**
- `MontoTotal`: Lo que debe pagar el cliente.
- `Kilometros` + `PrecioKm`: PrecioKm se calcula como `MontoTotal / Kilometros`.
- `Estado`: 1=En Viaje, 2=Finalizado, 3=Suspendido, 4=Cobrado.
- `NroViaje`: Número secuencial autogenerado.
- `IdMoneda` + `TipoCambio`: Soporte multi-moneda.

**Transiciones de estado del Viaje:**

```
Creado → 1 (En Viaje)
       → 2 (Finalizado)  cuando FechaDescarga se registra
       → 3 (Suspendido)  cambio manual
       → 4 (Cobrado)     automático cuando sum(Cobros.Monto) >= MontoTotal
```

La transición a "Cobrado" es automática mediante el método `Modified()` que se dispara al guardar cambios en la entidad Viaje.

---

### 2.2 Entidades de Cobro (Ingresos)

| Entidad | Descripción |
|---|---|
| `Cobro` | Registro de pago recibido de un cliente por un viaje. |
| `FormaPago` | Tipo de pago: efectivo, cheque, transferencia, etc. |
| `Moneda` | Moneda del cobro: pesos, dólares, etc. |
| `PagoCheque` | Datos del cheque si la forma de pago es cheque. |
| `Banco` | Banco emisor del cheque. |

**Campos clave de `Cobro`:**
- `IdViaje`: El viaje que se está cobrando.
- `Monto`: Importe recibido.
- `FechaRecibo`: Fecha en que se recibió el pago.
- `IdFormaPago`, `IdMoneda`, `TipoCambio`: Detalles del pago.
- `CobroAnulado`: Si no es NULL, este cobro es la anulación de otro cobro.
- `UserName` / `UserDateTime`: Auditoría de quién y cuándo.

**Ciclo de vida de un Cobro:**

```
1. Se crea el Cobro (referencia a un Viaje, monto, forma de pago)
2. Si la forma de pago es cheque → se crea/vincula un PagoCheque
3. El trigger Modified() del Viaje recalcula si está totalmente cobrado
4. Si se anula → se crea un Cobro negativo; el cheque se marca como Rechazado
```

**Estados del Cheque (`PagoCheque`):**

| Estado | Condición |
|---|---|
| Para Cobrar | Hoy está entre `FechaCobro` y `FechaVencimiento`, no rechazado, sin depósito |
| Por Cobrar | `FechaCobro` dentro del rango consultado, no rechazado, sin depósito |
| Cobrados | `FechaDeposito` registrada, no rechazado |
| Rechazados | `Rechazado = true` |

La fecha de vencimiento se calcula automáticamente como `FechaCobro + 30 días`.

---

### 2.3 Entidades de Mantenimiento y Gastos

| Entidad | Descripción |
|---|---|
| `Mantenimiento` | Orden de mantenimiento de un vehículo en un taller. |
| `Taller` | Taller mecánico que realiza el mantenimiento. |
| `CompraRepuesto` | Compra de repuestos a un proveedor para un mantenimiento. |
| `CompraRepuestoDetalle` | Ítem individual dentro de una compra de repuestos. |
| `Proveedor` | Proveedor de repuestos o materiales. |
| `MantenimientoObservacion` | Notas sobre el mantenimiento. |
| `MantenimientoTarea` | Tareas/trabajos dentro de un mantenimiento. |
| `Especialidad` | Categoría de servicio (mecánica, electricidad, etc.). |
| `TallerEspecialidad` | Especialidades de un taller (many-to-many). |
| `ProveedorEspecialidad` | Especialidades de un proveedor (many-to-many). |

**Campos clave de `Mantenimiento`:**
- `IdVehiculo`: Vehículo al que pertenece.
- `IdTaller`: Taller que lo ejecuta.
- `FechaEntrada` / `FechaSalida`: Período del trabajo.
- `Importe`: Costo total del servicio del taller.
- `Suspendido`: Si fue cancelado.

---

### 2.4 Entidades de Pago (Egresos)

| Entidad | Descripción |
|---|---|
| `Pago` | Registro de pago realizado por la empresa (a talleres o proveedores). |
| `PagoMantenimiento` | Vínculo entre un Pago y un Mantenimiento (con importe parcial). |
| `PagoCompraRepuesto` | Vínculo entre un Pago y una CompraRepuesto (con importe parcial). |

Un `Pago` puede aplicarse parcialmente a múltiples mantenimientos o compras mediante `ImporteAplicado`. Esto permite, por ejemplo, pagar con un solo cheque varias facturas del mismo taller.

---

### 2.5 Entidades de Referencia

| Entidad | Descripción |
|---|---|
| `Localidad` / `Provincia` / `Pais` | Geografía para domicilios. |
| `Evento` / `TipoEvento` | Sistema de log de eventos del sistema. |
| `Usuario` | Cuentas de usuario para autenticación JWT. |

---

## 3. API REST — Endpoints Principales

### Cobros (`/api/v1/cobros`)

| Método | Ruta | Acción |
|---|---|---|
| POST | `/cobros/add` | Registrar un cobro nuevo |
| GET | `/cobros/getAll` | Listar cobros con filtros y paginación |
| POST | `/cobros/update` | Modificar un cobro existente |
| GET | `/cobros/get` | Obtener detalle de un cobro |
| POST | `/cobros/anular` | Anular un cobro (crea cobro negativo) |

### Cheques (`/api/v1/cheques`)

| Método | Ruta | Acción |
|---|---|---|
| GET | `/cheques/getAll` | Listar cheques con filtro por estado |

### Viajes (`/api/v1/viaje`)

| Método | Ruta | Acción |
|---|---|---|
| POST | `/viaje/add` | Crear un viaje nuevo |
| POST | `/viaje/update` | Actualizar datos de un viaje |
| GET | `/viaje/get` | Obtener detalle (incluye cobros relacionados) |
| GET | `/viaje/getAll` | Listar viajes con filtros |
| POST | `/viaje/cargar-descarga` | Registrar carga o descarga |
| POST | `/viaje/forzar-estado` | Cambiar estado manualmente |
| GET | `/viaje/get-obs` | Ver historial de observaciones |

### Otros endpoints disponibles

- `/api/v1/cliente` — Alta/baja/modificación de clientes
- `/api/v1/camion` — Gestión de vehículos
- `/api/v1/chofer` — Gestión de choferes
- `/api/v1/mantenimiento` — Ciclo de vida de mantenimientos
- `/api/v1/proveedor` — Gestión de proveedores
- `/api/v1/taller` — Gestión de talleres
- `/api/v1/banco` — Directorio de bancos

---

## 4. Flujo de Ingreso (Viaje → Cobro)

```
Cliente contrata transporte
        ↓
Se crea el Viaje
  - MontoTotal: lo que deberá pagar
  - Estado: 1 (En Viaje)
        ↓
Se completa la entrega
  - FechaDescarga registrada
  - Estado: 2 (Finalizado)
        ↓
Se recibe el pago → se crea un Cobro
  - Referencia al Viaje
  - Monto cobrado (puede ser parcial)
  - Forma de pago + moneda
  - Si cheque → PagoCheque con datos del cheque
        ↓
Viaje recalcula estado automáticamente
  - sum(Cobros) >= MontoTotal → Estado: 4 (Cobrado)
  - sum(Cobros) <  MontoTotal → Estado: 2 (Finalizado)
```

---

## 5. Flujo de Egreso (Mantenimiento → Pago)

```
Vehículo requiere reparación
        ↓
Se crea el Mantenimiento
  - Vehículo, taller, fechas, importe presupuestado
        ↓
Se compran repuestos → CompraRepuesto
  - Proveedor, ítems, IVA
        ↓
Se emite el pago → Pago
  - Monto, forma de pago, moneda
        ↓
Se aplica el pago:
  - PagoMantenimiento → vincula Pago al Mantenimiento
  - PagoCompraRepuesto → vincula Pago a la CompraRepuesto
  - Un pago puede dividirse en ImporteAplicado a múltiples registros
```

---

## 6. Soporte Multi-Moneda

- `Moneda ID 1 = Pesos Argentinos` (moneda base).
- Cualquier otra moneda requiere `TipoCambio` (tipo de cambio al momento de la operación).
- El tipo de cambio se almacena en cada transacción (`Cobro`, `Pago`) para auditoría histórica.
- Validación: Si `IdMoneda != 1` y `TipoCambio` es nulo → error de negocio.

---

## 7. Auditoría

Todas las entidades importantes implementan `IAuditable`:
- `CreatedAt`, `ModifiedAt`: Timestamps automáticos.
- `UserName`, `UserDateTime`: Usuario que realizó la acción.
- El interceptor `AuditSaveChangesInterceptor` aplica estos valores automáticamente al guardar.

---

## 8. Seguridad

- Autenticación con JWT Bearer Tokens.
- Todos los endpoints requieren token, excepto el login.
- El servicio `ICurrentUserService` extrae el usuario del token para auditoría.
- CORS abierto en desarrollo.

---

## 9. Limitaciones del Sistema Actual

| Limitación | Detalle |
|---|---|
| Sin concepto de "factura" | Un Cobro registra el pago, pero no hay documento de factura previa. No hay número de factura, condición de venta, ni datos fiscales. |
| Sin cuenta corriente por cliente | No hay saldo acumulado. Cada cobro está atado a un único viaje. |
| Sin factura de proveedor | Los pagos a talleres y proveedores no tienen una factura de compra registrada. Solo se registra el pago directo. |
| Sin soporte AFIP | No hay tipo de comprobante (A, B, C, M), punto de venta, ni CAE. |
| Sin fechas de vencimiento de facturas | Los cobros no tienen fecha de vencimiento para el seguimiento de deudas. |
| Sin reporte de deuda | No existe una vista de "lo que me deben" ni "lo que le debo a proveedores". |
| Cancelación simplificada | La anulación de un Cobro crea un registro negativo, pero no revierte el estado del viaje correctamente en todos los casos. |
