CREATE TABLE Clients
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    DataCreated DATETIME NOT NULL,
    NameClient NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Logo NVARCHAR(255)

);
CREATE TABLE Addresses
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    DataCreated DATETIME NOT NULL,
    Country NVARCHAR(255),
    States NVARCHAR(255),
    City NVARCHAR(255),
    Neighborhood NVARCHAR(255),
    Road NVARCHAR(255),
    Number NVARCHAR(50),
    Complement NVARCHAR(255),
AddressClientId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Client(Id)
);

ALTER TABLE Addresses
ADD ClientId UNIQUEIDENTIFIER NULL;

ALTER TABLE Clients
DROP COLUMN AddressClientId;
---Procedures--------

DROP PROCEDURE dbo.UpdateClient;
DELETE FROM Clients;

CREATE PROCEDURE InsertClient
	@Id   UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @Email NVARCHAR(255),
    @Logo NVARCHAR(255)
AS
BEGIN
    INSERT INTO Clients (Id, NameClient, Email, Logo)
    VALUES (@Id, @Name, @Email, @Logo);

    SELECT SCOPE_IDENTITY(); 
END;

CREATE PROCEDURE UpdateClient
    @ClientId UNIQUEIDENTIFIER,
    @NewName NVARCHAR(255),
    @NewEmail NVARCHAR(255),
	@NewLogo NVARCHAR(255)
AS
BEGIN
    UPDATE Clients
    SET
        NameClient = @NewName,
        Email = @NewEmail,
		Logo = @NewLogo
    WHERE
        Id = @ClientId;
END;

CREATE PROCEDURE DeleteClient
    @ClientId UNIQUEIDENTIFIER,
    @DeleteAddresses BIT
AS
BEGIN
    IF @DeleteAddresses = 1
    BEGIN
        -- Excluir o cliente e seus endereços
        DELETE FROM Clients
        WHERE Id = @ClientId;

        DELETE FROM Addresses
        WHERE ClientId = @ClientId;
    END
    ELSE
    BEGIN
        -- Excluir apenas o cliente
        DELETE FROM Clients
        WHERE Id = @ClientId;
    END;
END;

-------Procedures para endereço------------

CREATE PROCEDURE InsertAddress
    @Id UNIQUEIDENTIFIER,
    @Country NVARCHAR(255),
    @State NVARCHAR(255),
    @City NVARCHAR(255),
    @Neighborhood NVARCHAR(255),
    @Road NVARCHAR(255),
    @Number NVARCHAR(50),
    @Complement NVARCHAR(255),
	@ClientId UNIQUEIDENTIFIER
AS
BEGIN
    INSERT INTO Addresses (Id, Country, State, City, Neighborhood, Road, Number, Complement, ClientId)
    VALUES (@Id, @Country, @State, @City, @Neighborhood, @Road, @Number, @Complement, @ClientId);
END;

CREATE PROCEDURE UpdateAddress
    @ClientId UNIQUEIDENTIFIER,
    @Country NVARCHAR(255),
    @State NVARCHAR(255),
    @City NVARCHAR(255),
    @Neighborhood NVARCHAR(255),
    @Road NVARCHAR(255),
    @Number NVARCHAR(50),
    @Complement NVARCHAR(255)
AS
BEGIN
    -- Verifica se o endereço já existe para o cliente
    IF EXISTS (SELECT 1 FROM Addresses WHERE ClientId = @ClientId)
    BEGIN
        -- Atualiza o endereço existente
        UPDATE Addresses
        SET
            Country = @Country,
            State = @State,
            City = @City,
            Neighborhood = @Neighborhood,
            Road = @Road,
            Number = @Number,
            Complement = @Complement
        WHERE
            ClientId = @ClientId;
    END
    ELSE
    BEGIN
        -- Insere um novo endereço para o cliente
        INSERT INTO Addresses (Id, ClientId, Country, State, City, Neighborhood, Road, Number, Complement)
        VALUES (NEWID(), @ClientId, @Country, @State, @City, @Neighborhood, @Road, @Number, @Complement);
    END;
END;

CREATE PROCEDURE DeleteAddress
    @AddressId UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM Addresses
    WHERE Id = @AddressId;
END;

