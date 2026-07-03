
CREATE DATABASE IPruebaPrograVI;
GO

-----------------------------------------------------------------------------
--TABLA CLIENTES
-----------------------------------------------------------------------------
CREATE TABLE Clientes (
	id INT PRIMARY KEY IDENTITY (1,1),
	Nombre VARCHAR(100) NOT NULL,
	Correo VARCHAR(150) UNIQUE NOT NULL,
	Telefono VARCHAR(20) NULL,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	Fecha_Creacion DATETIME NOT NULL DEFAULT GETDATE(),
	Usuario_Creacion NVARCHAR(50) NOT NULL,
	Fecha_Modificacion DATETIME NULL,
	Usuario_Modificacion NVARCHAR(50) NULL
);

USE IPruebaPrograVI;
go


-----------------------------------------------------------------------------
--PROCEDIMIENTO ALMACENADO
-----------------------------------------------------------------------------
CREATE PROCEDURE sp_InsertarCliente
	@Nombre VARCHAR(100),
	@Correo VARCHAR(150),
	@Telefono VARCHAR(20) = NULL,
	@Usuario_Creacion VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

    --Para verificar que los espacios de nombre y correo no esten vacios
    IF LTRIM(RTRIM(ISNULL(@Nombre, ''))) = '' OR LTRIM(RTRIM(ISNULL(@Correo, ''))) = ''
    BEGIN
        RAISERROR('Los campos Nombre y Correo deben contener datos.', 16, 1);
        RETURN;
    END

    -- Para verificar que el correo no se duplique
    IF EXISTS (SELECT 1 FROM Clientes WHERE Correo = @Correo)
    BEGIN
        RAISERROR('El correo ya se encuentra registrado.', 16, 1);
        RETURN;
    END

    -- Manejo de errores
    BEGIN TRY
        INSERT INTO Clientes (Nombre, Correo, Telefono, usuario_creacion)
        VALUES (@Nombre, @Correo, @Telefono, @usuario_creacion);
        
        SELECT SCOPE_IDENTITY() AS NuevoID;
    END TRY
    BEGIN CATCH
        -- Detección de errores 
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
