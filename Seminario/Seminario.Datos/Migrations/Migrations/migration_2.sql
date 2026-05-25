-- Migration 2: Tablas de Facturas y Recibos

CREATE TABLE IF NOT EXISTS `factura`
(
    `IdFactura`        INT             NOT NULL AUTO_INCREMENT,
    `Tipo`             TINYINT         NOT NULL,
    `PuntoVenta`       INT             NOT NULL,
    `Numero`           INT             NOT NULL,
    `FechaEmision`     DATE            NOT NULL,
    `FechaVencimiento` DATE            NULL,
    `Subtotal`         DECIMAL(18, 2)  NOT NULL,
    `PorcentajeIva`    DECIMAL(5, 2)   NOT NULL,
    `Total`            DECIMAL(18, 2)  NOT NULL,
    `IdMoneda`         INT             NOT NULL,
    `TipoCambio`       DECIMAL(18, 4)  NULL,
    `Estado`           TINYINT         NOT NULL DEFAULT 1,
    `Observaciones`    TEXT            NULL,
    `Anulada`          TINYINT(1)      NOT NULL DEFAULT 0,
    `IdCliente`        INT             NULL,
    `IdProveedor`      INT             NULL,
    `IdTaller`         INT             NULL,
    `UserName`         VARCHAR(100)    NULL,
    `UserDateTime`     DATETIME        NULL,
    `UserAlta`         VARCHAR(100)    NULL,
    `FechaAlta`        DATETIME        NULL,
    PRIMARY KEY (`IdFactura`),
    UNIQUE KEY `UQ_FACTURA_TIPO_PV_NRO` (`Tipo`, `PuntoVenta`, `Numero`),
    CONSTRAINT `FK_FACTURA_MONEDA`    FOREIGN KEY (`IdMoneda`)    REFERENCES `moneda`    (`idMoneda`),
    CONSTRAINT `FK_FACTURA_CLIENTE`   FOREIGN KEY (`IdCliente`)   REFERENCES `cliente`   (`idCliente`),
    CONSTRAINT `FK_FACTURA_PROVEEDOR` FOREIGN KEY (`IdProveedor`) REFERENCES `proveedor` (`idProveedor`),
    CONSTRAINT `FK_FACTURA_TALLER`    FOREIGN KEY (`IdTaller`)    REFERENCES `taller`    (`idTaller`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `facturadetalle`
(
    `IdFacturaDetalle` INT             NOT NULL AUTO_INCREMENT,
    `IdFactura`        INT             NOT NULL,
    `Orden`            INT             NOT NULL,
    `Descripcion`      VARCHAR(255)    NOT NULL,
    `Cantidad`         DECIMAL(18, 2)  NOT NULL,
    `PrecioUnitario`   DECIMAL(18, 2)  NOT NULL,
    `PorcentajeIva`    DECIMAL(5, 2)   NOT NULL,
    `Subtotal`         DECIMAL(18, 2)  NOT NULL,
    `Total`            DECIMAL(18, 2)  NOT NULL,
    PRIMARY KEY (`IdFacturaDetalle`),
    CONSTRAINT `FK_FACTURADETALLE_FACTURA` FOREIGN KEY (`IdFactura`) REFERENCES `factura` (`IdFactura`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `facturaviaje`
(
    `IdFacturaViaje` INT            NOT NULL AUTO_INCREMENT,
    `IdFactura`      INT            NOT NULL,
    `IdViaje`        INT            NOT NULL,
    `MontoViaje`     DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (`IdFacturaViaje`),
    UNIQUE KEY `UQ_FACTURAVIAJE_IDVIAJE` (`IdViaje`),
    CONSTRAINT `FK_FACTURAVIAJE_FACTURA` FOREIGN KEY (`IdFactura`) REFERENCES `factura` (`IdFactura`),
    CONSTRAINT `FK_FACTURAVIAJE_VIAJE`   FOREIGN KEY (`IdViaje`)   REFERENCES `viaje`   (`idViaje`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `facturamantenimiento`
(
    `IdFacturaMantenimiento` INT            NOT NULL AUTO_INCREMENT,
    `IdFactura`              INT            NOT NULL,
    `IdMantenimiento`        INT            NOT NULL,
    `ImporteMantenimiento`   DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (`IdFacturaMantenimiento`),
    UNIQUE KEY `UQ_FACTURAMANTENIMIENTO` (`IdFactura`, `IdMantenimiento`),
    CONSTRAINT `FK_FACTURAMANTENIMIENTO_FACTURA`      FOREIGN KEY (`IdFactura`)      REFERENCES `factura`      (`IdFactura`),
    CONSTRAINT `FK_FACTURAMANTENIMIENTO_MANTENIMIENTO` FOREIGN KEY (`IdMantenimiento`) REFERENCES `mantenimiento` (`idMantenimiento`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `facturacomprarepuesto`
(
    `IdFacturaCompraRepuesto` INT            NOT NULL AUTO_INCREMENT,
    `IdFactura`               INT            NOT NULL,
    `IdCompraRepuesto`        INT            NOT NULL,
    `ImporteCompra`           DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (`IdFacturaCompraRepuesto`),
    UNIQUE KEY `UQ_FACTURACOMPRAREPUESTO` (`IdFactura`, `IdCompraRepuesto`),
    CONSTRAINT `FK_FACTURACOMPRAREPUESTO_FACTURA`      FOREIGN KEY (`IdFactura`)        REFERENCES `factura`          (`IdFactura`),
    CONSTRAINT `FK_FACTURACOMPRAREPUESTO_COMPRAREPUESTO` FOREIGN KEY (`IdCompraRepuesto`) REFERENCES `compra/repuesto` (`idCompraRepuesto`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `recibo`
(
    `IdRecibo`     INT            NOT NULL AUTO_INCREMENT,
    `Tipo`         TINYINT        NOT NULL,
    `FechaRecibo`  DATE           NOT NULL,
    `MontoTotal`   DECIMAL(18, 2) NOT NULL,
    `IdMoneda`     INT            NOT NULL,
    `TipoCambio`   DECIMAL(18, 4) NULL,
    `IdCliente`    INT            NULL,
    `IdProveedor`  INT            NULL,
    `IdTaller`     INT            NULL,
    `Observaciones` TEXT          NULL,
    `Anulado`      TINYINT(1)     NOT NULL DEFAULT 0,
    `UserName`     VARCHAR(100)   NULL,
    `UserDateTime` DATETIME       NULL,
    `UserAlta`     VARCHAR(100)   NULL,
    `FechaAlta`    DATETIME       NULL,
    PRIMARY KEY (`IdRecibo`),
    CONSTRAINT `FK_RECIBO_MONEDA`    FOREIGN KEY (`IdMoneda`)    REFERENCES `moneda`    (`idMoneda`),
    CONSTRAINT `FK_RECIBO_CLIENTE`   FOREIGN KEY (`IdCliente`)   REFERENCES `cliente`   (`idCliente`),
    CONSTRAINT `FK_RECIBO_PROVEEDOR` FOREIGN KEY (`IdProveedor`) REFERENCES `proveedor` (`idProveedor`),
    CONSTRAINT `FK_RECIBO_TALLER`    FOREIGN KEY (`IdTaller`)    REFERENCES `taller`    (`idTaller`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `reciboformapago`
(
    `IdReciboFormaPago` INT            NOT NULL AUTO_INCREMENT,
    `IdRecibo`          INT            NOT NULL,
    `IdFormaPago`       INT            NOT NULL,
    `Monto`             DECIMAL(18, 2) NOT NULL,
    `IdPagoCheque`      INT            NULL,
    PRIMARY KEY (`IdReciboFormaPago`),
    CONSTRAINT `FK_RECIBOFORMAPAGO_RECIBO`     FOREIGN KEY (`IdRecibo`)     REFERENCES `recibo`     (`IdRecibo`),
    CONSTRAINT `FK_RECIBOFORMAPAGO_FORMAPAGO`  FOREIGN KEY (`IdFormaPago`)  REFERENCES `formapago`  (`idFormaPago`),
    CONSTRAINT `FK_RECIBOFORMAPAGO_PAGOCHEQUE` FOREIGN KEY (`IdPagoCheque`) REFERENCES `pagocheque` (`idPagoCheque`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `recibofactura`
(
    `IdReciboFactura`  INT            NOT NULL AUTO_INCREMENT,
    `IdRecibo`         INT            NOT NULL,
    `IdFactura`        INT            NOT NULL,
    `ImporteAplicado`  DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (`IdReciboFactura`),
    UNIQUE KEY `UQ_RECIBOFACTURA` (`IdRecibo`, `IdFactura`),
    CONSTRAINT `FK_RECIBOFACTURA_RECIBO`  FOREIGN KEY (`IdRecibo`)  REFERENCES `recibo`  (`IdRecibo`),
    CONSTRAINT `FK_RECIBOFACTURA_FACTURA` FOREIGN KEY (`IdFactura`) REFERENCES `factura` (`IdFactura`)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
