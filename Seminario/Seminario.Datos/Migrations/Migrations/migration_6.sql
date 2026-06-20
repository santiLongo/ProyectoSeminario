-- ============================================================
-- Migración: agregar columnas email y phoneNumber a usuario
-- Base de datos: MySQL
-- Seguro de correr varias veces (idempotente)
-- ============================================================

SET @table_name  = 'usuario';

-- ===== Columna email (VARCHAR(50) NULL) =====
SET @col_exists = (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = @schema_name
      AND TABLE_NAME   = @table_name
      AND COLUMN_NAME  = 'email'
);

SET @sql = IF(@col_exists = 0,
    CONCAT('ALTER TABLE `', @table_name, '` ADD COLUMN `email` VARCHAR(50) NULL;'),
    'SELECT "Columna email ya existe, se omite" AS info;'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ===== Columna phoneNumber (VARCHAR(20) NULL) =====
SET @col_exists = (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = @schema_name
      AND TABLE_NAME   = @table_name
      AND COLUMN_NAME  = 'phoneNumber'
);

SET @sql = IF(@col_exists = 0,
    CONCAT('ALTER TABLE `', @table_name, '` ADD COLUMN `phoneNumber` VARCHAR(20) NULL;'),
    'SELECT "Columna phoneNumber ya existe, se omite" AS info;'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEA