USE AdventureWorks2012
GO;

CREATE PROCEDURE dbo.UpdateDLPoints
	@DLNumber varchar(8)
	,@Points int
AS
BEGIN
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	UNION SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	UNION SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	UNION SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	UNION SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	UNION SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	UNION SELECT @DLNumber as DLNumber, (ABS(CHECKSUM(NewId())) % 1000) as PersonID, (ABS(CHECKSUM(NewId())) % 10)+@Points as Points
	
END
GO

--EXEC dbo.UpdateDLPoints '00000000',3
--DROP PROCEDURE dbo.UpdateDLPoints
