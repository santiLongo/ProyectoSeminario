SELECT
    cheque.idPagoCheque AS Id,
    cheque.nroCheque AS NroCheque,
    IFNULL(cheque.esPropio, 0) AS EsPropio,
    banco.descripcion AS Banco,
    cheque.cuitEmisor AS CuitEmisor,
    cheque.fechaCobro AS FechaDeCobro,
    cheque.fechaDeposito AS FechaDeposito,
    cheque.fechaVencimiento AS FechaVencimiento,
    mant.idMantenimiento AS IdMantenimiento,
    mant.titulo AS MantenimientoDesc,
    repu.idCompraRepuesto AS CompraRepuesto

FROM pagocheque cheque
LEFT JOIN pago ON pago.idPagoCheque = cheque.idPagoCheque
LEFT JOIN LATERAL (
    SELECT * FROM `pago/mantenimiento` mant WHERE mant.idPago = pago.idPago LIMIT 1
    ) pagmant ON TRUE
LEFT JOIN mantenimiento mant ON mant.idMantenimiento = pagmant.idMantenimiento
LEFT JOIN LATERAL (
    SELECT * FROM `pago/comprarepuesto` repues WHERE repues.idPago = pago.idPago LIMIT 1
    ) pagrepues ON TRUE
LEFT JOIN `compra/repuesto` repu ON repu.idCompraRepuesto = pagrepues.idCompraRepuesto
LEFT JOIN banco ON banco.idBanco = cheque.idBanco
WHERE
    (
        (@estado = 1 AND (
            (@hoy between cheque.fechaCobro AND cheque.fechaVencimiento)
            AND (cheque.rechazado <> 1)
            AND (cheque.fechaDeposito IS NULL)
            )) OR
        (@estado = 2 AND (
            (cheque.fechaCobro between @cobroDesde AND @cobroHasta)
            AND (cheque.rechazado <> 1)
            AND (cheque.fechaDeposito IS NULL)
            )) OR
        (@estado = 3 AND (
                (cheque.rechazado <> 1)
                AND (cheque.fechaDeposito IS NOT NULL)
            )) OR
        (@estado = 4 AND (
            (cheque.rechazado = 1)
            )) OR
        (@estado = 5)
    ) AND
    (@soloPropios IS NULL OR cheque.esPropio = 1)