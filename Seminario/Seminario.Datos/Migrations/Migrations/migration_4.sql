-- Migration 4: Campo Confirmada en tabla factura (emitidas)

ALTER TABLE `factura`
    ADD COLUMN `Confirmada` TINYINT(1) NOT NULL DEFAULT 0 AFTER `CAEFchVto`;
