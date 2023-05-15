CREATE TABLE CondicaoPagamento (
    CondicaoPagamentoID INT NOT NULL IDENTITY(1, 1),
    Descricao VARCHAR(1000) NOT NULL,
	CondicaoPagamentoDesc VARCHAR(1000),		
	TipoCondicaoPagamento INT,
	Ativo INT,
    DataCriacao DATETIME NOT NULL,    
    DataModif DATETIME, 
    PRIMARY KEY (CondicaoPagamentoID)
);