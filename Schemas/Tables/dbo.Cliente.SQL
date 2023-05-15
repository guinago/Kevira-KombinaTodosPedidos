CREATE TABLE Cliente (
        ClienteID INT NOT NULL IDENTITY(1, 1),
        Nome VARCHAR (MAX) NOT NULL,
        CPF VARCHAR(20) NOT NULL,
        Telefone VARCHAR(20) NOT NULL,
        Email VARCHAR(100),
        EnderecoCEP VARCHAR(10) NOT NULL,
        EnderecoCompleto VARCHAR(1000) NOT NULL,
        Estado VARCHAR(1000) NOT NULL,
        Municipio VARCHAR(1000) NOT NULL,
        EnderecoBairo VARCHAR(1000) NOT NULL,
        DataCriacao VARCHAR(1000) NOT NULL,
        DataModif VARCHAR(1000),	
    PRIMARY KEY (ClienteID)
);