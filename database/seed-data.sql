-- Datos de ejemplo para Embrujo Cerveza
-- Ejecutar este script en la base de datos PostgreSQL luego de aplicar las migraciones.

BEGIN;

INSERT INTO "BeerStyles" ("Id", "Name", "Description", "Abv", "Ibu", "ImageFileName") VALUES
    (1, 'IPA', 'India Pale Ale con notas cítricas y amargor medio-alto.', 6.2, 55, NULL),
    (2, 'Stout de avena', 'Cerveza oscura y cremosa con sabores a café y chocolate.', 5.8, 35, NULL)
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "BottleTypes" ("Id", "Material", "CapacityMl", "Description") VALUES
    (1, 'Vidrio ámbar', 330, 'Botella estándar ideal para estilos lupulados.'),
    (2, 'Lata de aluminio', 473, 'Formato práctico para lotes pequeños.'),
    (3, 'Vidrio ámbar', 500, 'Presentación clásica para stouts y cervezas especiales.')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "BeerLots" ("Id", "BeerStyleId", "BottleTypeId", "BottleCount", "BottledOn", "Notes") VALUES
    (1, 1, 1, 120, CURRENT_DATE - INTERVAL '10 days', 'Lote principal de temporada.'),
    (2, 2, 2, 60, CURRENT_DATE - INTERVAL '3 days', 'Producción limitada.')
ON CONFLICT ("Id") DO NOTHING;

COMMIT;
