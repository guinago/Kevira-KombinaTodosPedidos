USE [KeViraKombinaTodos]
GO

/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 29/10/2019 18:48:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[IDMaster] [int] NOT NULL,
	[DataNascimento] [datetime] NULL,
	[DataCadastro] [datetime] NOT NULL,
	[CGC] [nvarchar](max) NOT NULL,
	[Razao] [nvarchar](max) NOT NULL,
	[Contato] [nvarchar](max) NOT NULL,
	[EnderecoNumero] [nvarchar](max) NULL,
	[Endereco] [nvarchar](max) NULL,
	[Estado] [nvarchar](max) NULL,
	[Municipio] [nvarchar](max) NULL,
	[Bairro] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[TipoCGC] [int] NOT NULL,
	[Nome] [nvarchar](max) NULL,
	[IsEnabled] [bit] NULL,
	[CEP] [nvarchar](max) NOT NULL,
	[PerfilID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [IDMaster]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [DataCadastro]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('') FOR [CGC]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('') FOR [Razao]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('') FOR [Contato]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [TipoCGC]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('') FOR [CEP]
GO

ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [PerfilID]
GO

