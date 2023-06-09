USE [master]
GO

/****** Object:  Database [KeViraKombinaTodos]    Script Date: 17/10/2019 19:55:38 ******/
CREATE DATABASE [KeViraKombinaTodos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KeViraKombinaTodos', FILENAME = N'/var/opt/mssql/data/KeViraKombinaTodos.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'KeViraKombinaTodos_log', FILENAME = N'/var/opt/mssql/data/KeViraKombinaTodos_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KeViraKombinaTodos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [KeViraKombinaTodos] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET ARITHABORT OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [KeViraKombinaTodos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [KeViraKombinaTodos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET  DISABLE_BROKER 
GO

ALTER DATABASE [KeViraKombinaTodos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [KeViraKombinaTodos] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET RECOVERY FULL 
GO

ALTER DATABASE [KeViraKombinaTodos] SET  MULTI_USER 
GO

ALTER DATABASE [KeViraKombinaTodos] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [KeViraKombinaTodos] SET DB_CHAINING OFF 
GO

ALTER DATABASE [KeViraKombinaTodos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [KeViraKombinaTodos] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [KeViraKombinaTodos] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [KeViraKombinaTodos] SET QUERY_STORE = OFF
GO

ALTER DATABASE [KeViraKombinaTodos] SET  READ_WRITE 
GO

