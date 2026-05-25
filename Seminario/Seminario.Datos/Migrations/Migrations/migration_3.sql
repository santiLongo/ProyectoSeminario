-- Migration 3: Campos AFIP en tabla factura

ALTER TABLE `factura`
    ADD COLUMN `TipoComprobante` INT NULL AFTER `Anulada`,
    ADD COLUMN `CAE`             VARCHAR(14) NULL AFTER `TipoComprobante`,
    ADD COLUMN `CAEFchVto`       VARCHAR(8)  NULL AFTER `CAE`;
