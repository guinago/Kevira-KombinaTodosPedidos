CREATE TABLE Usuario (
        UsuarioID INT NOT NULL,
        Nome VARCHAR (MAX) NOT NULL,
        CPF VARCHAR(20) NOT NULL,
        Telefone VARCHAR(20) NOT NULL,
        Email VARCHAR(100),
        DataCriacao VARCHAR(1000) NOT NULL,
        DataModif VARCHAR(1000),	
    PRIMARY KEY (ClienteID)
);