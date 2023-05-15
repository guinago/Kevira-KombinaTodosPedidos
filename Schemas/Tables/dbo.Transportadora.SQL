CREATE TABLE Transportadora (
    TransportadoraID INT NOT NULL IDENTITY(1,1),    
	Descricao VARCHAR(1000) NOT NULL,	
	Codigo VARCHAR(1000) NOT NULL,
	Complemento VARCHAR(1000),		
    DataCriacao DATETIME NOT NULL,
    DataModif DATETIME, 	
    PRIMARY KEY (TransportadoraID)
);