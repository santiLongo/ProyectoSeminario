-- Migration 5: Agrego numero real y punto de venta real

ALTER TABLE `factura`
    ADD COLUMN `PuntoVentaReal` int NULL,
    ADD COLUMN `NumeroReal` int NULL;
    
