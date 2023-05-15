CREATE TABLE Produto (
    ProdutoID INT NOT NULL IDENTITY(1, 1),	
    Descricao VARCHAR(1000) NOT NULL,
    Codigo VARCHAR(20) NOT NULL,
    Valor FLOAT NOT NULL,
	Quantidade FLOAT NOT NULL,
	DataCadastro DATETIME,
	Ativo INT NOT NULL DEFAULT 1,
    DataCriacao DATETIME NOT NULL,
    DataModif DATETIME,
    PRIMARY KEY (ProdutoID)
);