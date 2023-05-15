CREATE TABLE ItemPedido (
    PedidoID INT NOT NULL,	
    ProdutoID INT NOT NULL,
    Preco FLOAT NOT NULL,
    Quantidade FLOAT NOT NULL,
    ValorTotal FLOAT NOT NULL,
    Datacriacao DATETIME NOT NULL,
    DataModif DATETIME
    PRIMARY KEY (PedidoID, ProdutoID),
    FOREIGN KEY (PedidoID) REFERENCES Pedido(PedidoID),
    FOREIGN KEY (ProdutoID) REFERENCES Produto(ProdutoID)
);