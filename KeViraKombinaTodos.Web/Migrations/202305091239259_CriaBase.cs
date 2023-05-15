namespace KeViraKombinaTodos.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriaBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CPF = c.String(),
                        Nome = c.String(),
                        SobreNome = c.String(),
                        IsEnabled = c.Boolean(),
                        DataCriacao = c.DateTime(nullable: false),
                        DataModif = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTableCondicaoPagamento();
            CreateTablePerfil();
            CreateTableProduto();
            CreateTableTransportadora();
            CreateTableCliente();
            CreateTablePedido();
            CreateTableItemPedido();
            CreateTableUsuario();

        }



        private void CreateTableCondicaoPagamento()
        {
            CreateTable(
               "dbo.CondicaoPagamento",
          c => new
          {
              CondicaoPagamentoID = c.Int(nullable: false, identity: true),
              Descricao = c.String(nullable: false, maxLength: 1000),
              DataCriacao = c.DateTime(nullable: false),
              DataModif = c.DateTime(nullable: true),
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
              DataModif = c.DateTime(nullable: true),
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
              Valor = c.Double(nullable: false),
              Quantidade = c.Double(nullable: false),
              Ativo = c.Int(nullable: false, defaultValue: 1),
              DataCriacao = c.DateTime(nullable: false),
              DataModif = c.DateTime(nullable: true),
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
                DataCriacao = c.DateTime(nullable: false),
                DataModif = c.DateTime(nullable: true),
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
                DataModif = c.DateTime(nullable: true),
            })
            .PrimaryKey(p => new { p.ClienteID });
        }

        private void CreateTableUsuario()
        {
            CreateTable(
                 "dbo.Usuario",
            c => new
            {
                UsuarioID = c.Int(nullable: false, identity: false),
                Nome = c.String(nullable: false, maxLength: 1000),
                CPF = c.String(nullable: false, maxLength: 20),
                Telefone = c.String(nullable: false, maxLength: 20),
                Email = c.String(nullable: false, maxLength: 100),
                DataCriacao = c.DateTime(nullable: false),
                DataModif = c.DateTime(nullable: true),
            })
            .PrimaryKey(p => new { p.UsuarioID });
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
                  DataModif = c.DateTime(nullable: true),
              })
              .PrimaryKey(p => new { p.PedidoID })
              .ForeignKey("dbo.Cliente", t => t.ClienteID, cascadeDelete: true, name: "ClienteID")
              .ForeignKey("dbo.Usuario", t => t.CompradorID, cascadeDelete: true, name: "UsuarioID")
              .ForeignKey("dbo.Transportadora", t => t.TransportadoraID, cascadeDelete: true, name: "TransportadoraID")
              .ForeignKey("dbo.CondicaoPagamento", t => t.CondicaoPagamentoID, cascadeDelete: true, name: "CondicaoPagamentoID");
        }
        private void CreateTableItemPedido()
        {
            CreateTable(
                   "dbo.ItemPedido",
              c => new
              {
                  PedidoID = c.Int(nullable: false),
                  ProdutoID = c.Int(nullable: false),
                  Preco = c.Double(nullable: false),
                  Quantidade = c.Double(nullable: false),
                  ValorTotal = c.Double(nullable: false),
                  DataCriacao = c.DateTime(nullable: false),
                  DataModif = c.DateTime(nullable: true),
              })
              .PrimaryKey(p => new { p.PedidoID, p.ProdutoID })
              .ForeignKey("dbo.Pedido", t => t.PedidoID, cascadeDelete: true, name: "PedidoID")
              .ForeignKey("dbo.Produto", t => t.ProdutoID, cascadeDelete: true, name: "ProdutoID");
        }



        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
