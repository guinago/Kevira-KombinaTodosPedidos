namespace KeViraKombinaTodos.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriarBase : DbMigration
    {
        public override void Up()
        {           
            CreateTableCondicaoPagamento();
            CreateTablePerfil();
            CreateTableProduto();
            CreateTableTransportadora();
            CreateTableCliente();
            CreateTablePedido();
        }
        public override void Down()
        {
           
        }        
        private void CreateTableCondicaoPagamento()
        {
            CreateTable(
               "dbo.CondicaoPagamento",
          c => new
          {
              CondicaoPagamentoID = c.Int(nullable: false, identity: true),
              Descricao = c.String(nullable: false, maxLength: 1000),
              CondicaoPagamentoPagamentoDesc = c.String(nullable: true, maxLength: 1000),
              TipoCondicaoPagamento = c.Int(nullable: true),
              DataCriacao = c.DateTime(nullable: false),
              DataModif = c.DateTime(nullable: false),
          })
          .PrimaryKey(p => new { p.CondicaoPagamentoID });
        }
        private void CreateTablePerfil()
        {
            CreateTable(
               "dbo.Perfil",
          c => new
          {
              PerfilID = c.Int(nullable: false, identity: true),
              Descricao = c.String(nullable: false, maxLength: 1000),
              Codigo = c.String(nullable: false, maxLength: 1000),
              SouTodoPoderoso = c.Int(nullable: false),
              SouComprador = c.Int(nullable: false),
              SouTransportador = c.Int(nullable: false),
              DataCriacao = c.DateTime(nullable: false),
              DataModif = c.DateTime(nullable: false),
          })
          .PrimaryKey(p => new { p.PerfilID });
        }
        private void CreateTableProduto()
        {
            CreateTable(
               "dbo.Produto",
          c => new
          {
              ProdutoID = c.Int(nullable: false, identity: true),
              Descricao = c.String(nullable: false, maxLength: 1000),
              Codigo = c.String(nullable: false, maxLength: 1000),
              Valor = c.Decimal(nullable: false),
              Quantidade = c.Decimal(nullable: false),
              Ativo = c.Int(nullable: false, defaultValue: 1),
              DataCriacao = c.DateTime(nullable: false),
              DataModif = c.DateTime(nullable: false),
          })
          .PrimaryKey(p => new { p.ProdutoID });
        }
        private void CreateTableTransportadora()
        {
            CreateTable(
                 "dbo.Transportadora",
            c => new
            {
                TransportadoraID = c.Int(nullable: false, identity: true),
                Descricao = c.String(nullable: false, maxLength: 1000),
                Codigo = c.String(nullable: false, maxLength: 1000),
                Complemento = c.String(nullable: true, maxLength: 1000),
                DataCriacao = c.DateTime(nullable: false),
                DataModif = c.DateTime(nullable: false),
            })
            .PrimaryKey(p => new { p.TransportadoraID });
        }
        private void CreateTableCliente()
        {
            CreateTable(
                 "dbo.Cliente",
            c => new
            {
                ClienteID = c.Int(nullable: false, identity: true),
                Nome = c.String(nullable: false, maxLength: 1000),
                CPF = c.String(nullable: false, maxLength: 20),
                Telefone = c.String(nullable: false, maxLength: 20),
                Email = c.String(nullable: true, maxLength: 100),
                EnderecoCEP = c.String(nullable: false, maxLength: 10),
                EnderecoCompleto = c.String(nullable: false, maxLength: 1000),
                Estado = c.String(nullable: false, maxLength: 1000),
                Municipio = c.String(nullable: false, maxLength: 1000),
                EnderecoBairo = c.String(nullable: false, maxLength: 1000),
                DataCriacao = c.DateTime(nullable: false),
                DataModif = c.DateTime(nullable: false),
            })
            .PrimaryKey(p => new { p.ClienteID });
        }
        private void CreateTablePedido()
        {
            CreateTable(
                   "dbo.Pedido",
              c => new
              {
                  PedidoID = c.Int(nullable: false, identity: true),
                  ClienteID = c.Int(nullable: false),
                  CompradorID = c.Int(nullable: false),
                  TransportadoraID = c.Int(nullable: false),
                  CondicaoPagamentoID = c.Int(nullable: false),
                  Telefone = c.String(nullable: false, maxLength: 20),
                  Email = c.String(nullable: true, maxLength: 100),
                  CEP = c.String(nullable: false, maxLength: 10),
                  EnderecoCEP = c.String(nullable: false, maxLength: 10),
                  EnderecoCompleto = c.String(nullable: false, maxLength: 1000),
                  Estado = c.String(nullable: false, maxLength: 1000),
                  Municipio = c.String(nullable: false, maxLength: 1000),
                  EnderecoBairo = c.String(nullable: false, maxLength: 1000),
                  DataEntregaInicio = c.DateTime(nullable: false),
                  DataEntregaFim = c.DateTime(nullable: false),
                  Observacao = c.String(nullable: true, maxLength: 1000),
                  Restricao = c.String(nullable: true, maxLength: 1000),
                  NotaFiscal = c.String(nullable: true, maxLength: 20),
                  Status = c.Int(nullable: false, defaultValue: 0),
                  DataCriacao = c.DateTime(nullable: false),
                  DataModif = c.DateTime(nullable: false),
              })
              .PrimaryKey(p => new { p.PedidoID })
              .ForeignKey("dbo.Cliente", t => t.ClienteID, cascadeDelete: true, name: "ClienteID")
              .ForeignKey("dbo.AspNetUsers", t => t.CompradorID, cascadeDelete: true, name: "Id")
              .ForeignKey("dbo.Transportadora", t => t.TransportadoraID, cascadeDelete: true, name: "TransportadoraID")
              .ForeignKey("dbo.CondicaoPagamento", t => t.CondicaoPagamentoID, cascadeDelete: true, name: "CondicaoPagamentoID");
        }
    }
}
