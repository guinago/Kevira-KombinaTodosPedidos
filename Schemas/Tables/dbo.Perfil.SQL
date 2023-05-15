CREATE TABLE Perfil (
    PerfilID INT NOT NULL IDENTITY(1, 1),
	Descricao VARCHAR(1000) NOT NULL,
	Codigo VARCHAR(1000) NOT NULL,
	SouTodoPoderoso INT NOT NULL,
	SouComprador INT NOT NULL,
	SouTransportador INT NOT NULL,
	DataCriacao DATETIME NOT NULL,
	DataModif DATETIME,
	PRIMARY KEY (PerfilID)
);