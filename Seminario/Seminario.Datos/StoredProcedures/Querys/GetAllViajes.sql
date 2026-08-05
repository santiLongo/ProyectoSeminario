select
    via.idViaje                         idViaje,
    via.nroViaje                        NroViaje,
    cli.razonSocial                     Cliente,
    CONCAT(chf.nombre,' ',chf.apellido) Chofer,
    cam.Patente                         Patente,
    via.Carga                           Carga,
    via.kilometros                      Kilometros,
    via.MontoTotal                      MontoTotal,
    cobros.Cobrado                      Cobrado,
    via.Estado                          Estado,
    mon.Descripcion                     Moneda,
    via.FechaPartida                    FechaPartida,
    via.FechaDescarga                   FechaDescarga,
    via.UserName                        UserName,
    via.UserDateTime                    UserDateTime
from viaje via
         inner join camion cam ON cam.idCamion = via.idCamion
         inner join cliente cli ON cli.idCliente = via.idCliente
         inner join chofer chf ON chf.idChofer = via.idChofer
         LEFT JOIN moneda mon ON mon.idMoneda = via.idMoneda
         LEFT JOIN LATERAL ( SELECT SUM(MONTO) AS Cobrado FROM cobro WHERE cobro.idViaje = via.idViaje) cobros ON TRUE
where
    (@nroViaje is NULL or via.nroViaje = @nroViaje)
  AND     (@idCamion is NULL or cam.idCamion = @idCamion)
  AND     (@idCliente is NULL or cli.idCliente = @idCliente)
  AND     (@idChofer is NULL or chf.idChofer = @idChofer)
  AND     (@fechaAltaDesde is NULL or via.FechaAlta > @fechaAltaDesde)
  AND     (@fechaAltaHasta is NULL or via.FechaAlta < @fechaAltaHasta)
  AND     (@estado is NULL or via.Estado = @estado)
  AND (
    @idLocalidadProc IS NULL
        OR EXISTS (
        SELECT 1
        FROM procedencia proc
        WHERE proc.idViaje = via.idViaje
          AND proc.idLocalidad = @idLocalidadProc
    )
    )
  AND (
    @idLocalidadDest IS NULL
        OR EXISTS (
        SELECT 1
        FROM destino dest
        WHERE dest.idViaje = via.idViaje
          AND dest.idLocalidad = @idLocalidadDest
    )
    )