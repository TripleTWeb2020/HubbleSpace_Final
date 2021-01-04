USE [master]
GO
/****** Object:  Database [HubbleSpace_Final]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE DATABASE [HubbleSpace_Final]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HubbleSpace_Final', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\HubbleSpace_Final.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HubbleSpace_Final_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\HubbleSpace_Final_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HubbleSpace_Final] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HubbleSpace_Final].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HubbleSpace_Final] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET ARITHABORT OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [HubbleSpace_Final] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HubbleSpace_Final] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HubbleSpace_Final] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET  ENABLE_BROKER 
GO
ALTER DATABASE [HubbleSpace_Final] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HubbleSpace_Final] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [HubbleSpace_Final] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HubbleSpace_Final] SET  MULTI_USER 
GO
ALTER DATABASE [HubbleSpace_Final] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HubbleSpace_Final] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HubbleSpace_Final] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HubbleSpace_Final] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [HubbleSpace_Final] SET DELAYED_DURABILITY = DISABLED 
GO
USE [HubbleSpace_Final]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Account]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID_Account] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Avatar] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Level] [int] NOT NULL,
	[Date_Create] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID_Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Banner]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banner](
	[ID_Banner] [int] IDENTITY(1,1) NOT NULL,
	[Banner_Name] [nvarchar](100) NOT NULL,
	[Photo] [nvarchar](max) NOT NULL,
	[Date_Upload] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Banner] PRIMARY KEY CLUSTERED 
(
	[ID_Banner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Brand]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[ID_Brand] [int] IDENTITY(1,1) NOT NULL,
	[Brand_Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[ID_Brand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID_Categorie] [int] IDENTITY(1,1) NOT NULL,
	[Category_Name] [nvarchar](100) NOT NULL,
	[Object] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID_Categorie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Client]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ID_Client] [int] IDENTITY(1,1) NOT NULL,
	[Client_Name] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[DOB] [datetime2](7) NOT NULL,
	[Gender] [int] NOT NULL,
	[CreditCard] [nvarchar](max) NULL,
	[ID_Account] [int] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ID_Client] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Color_Product]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Color_Product](
	[ID_Color_Product] [int] IDENTITY(1,1) NOT NULL,
	[Color_Name] [nvarchar](100) NOT NULL,
	[ID_Product] [int] NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Color_Product] PRIMARY KEY CLUSTERED 
(
	[ID_Color_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Discount]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[ID_Discount] [int] IDENTITY(1,1) NOT NULL,
	[Code_Discount] [nvarchar](100) NOT NULL,
	[Expire] [datetime2](7) NOT NULL,
	[Value] [int] NOT NULL,
	[NumberofTurns] [int] NOT NULL,
 CONSTRAINT [PK_Discount] PRIMARY KEY CLUSTERED 
(
	[ID_Discount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employee]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[ID_Employee] [int] IDENTITY(1,1) NOT NULL,
	[Employee_Name] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[DOB] [datetime2](7) NOT NULL,
	[Gender] [int] NOT NULL,
	[Salary] [float] NOT NULL,
	[ID_Account] [int] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[ID_Employee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Img_Product]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Img_Product](
	[ID_Img_Product] [int] IDENTITY(1,1) NOT NULL,
	[Photo] [nvarchar](max) NOT NULL,
	[ID_Color_Product] [int] NOT NULL,
 CONSTRAINT [PK_Img_Product] PRIMARY KEY CLUSTERED 
(
	[ID_Img_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID_Order] [int] IDENTITY(1,1) NOT NULL,
	[TotalMoney] [float] NOT NULL,
	[Date_Create] [datetime2](7) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Receiver] [nvarchar](max) NOT NULL,
	[SDT] [nvarchar](max) NOT NULL,
	[ID_Account] [int] NOT NULL,
	[Process] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID_Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[ID_OrderDetail] [int] IDENTITY(1,1) NOT NULL,
	[ID_Color_Product] [int] NOT NULL,
	[Size] [nvarchar](max) NULL,
	[Quantity] [int] NOT NULL,
	[ID_Order] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[ID_OrderDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ID_Product] [int] IDENTITY(1,1) NOT NULL,
	[Product_Name] [nvarchar](100) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[Price_Product] [float] NOT NULL,
	[Price_Sale] [float] NOT NULL,
	[ID_Brand] [int] NOT NULL,
	[ID_Categorie] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ID_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Size]    Script Date: 1/4/2021 3:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Size](
	[ID_Size_Product] [int] IDENTITY(1,1) NOT NULL,
	[size] [nvarchar](max) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ID_Color_Product] [int] NOT NULL,
 CONSTRAINT [PK_Size] PRIMARY KEY CLUSTERED 
(
	[ID_Size_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_Client_ID_Account]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Client_ID_Account] ON [dbo].[Client]
(
	[ID_Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Color_Product_ID_Product]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Color_Product_ID_Product] ON [dbo].[Color_Product]
(
	[ID_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Employee_ID_Account]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Employee_ID_Account] ON [dbo].[Employee]
(
	[ID_Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Img_Product_ID_Color_Product]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Img_Product_ID_Color_Product] ON [dbo].[Img_Product]
(
	[ID_Color_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_ID_Account]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_ID_Account] ON [dbo].[Order]
(
	[ID_Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetail_ID_Color_Product]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetail_ID_Color_Product] ON [dbo].[OrderDetail]
(
	[ID_Color_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetail_ID_Order]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetail_ID_Order] ON [dbo].[OrderDetail]
(
	[ID_Order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_ID_Brand]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_ID_Brand] ON [dbo].[Product]
(
	[ID_Brand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_ID_Categorie]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_ID_Categorie] ON [dbo].[Product]
(
	[ID_Categorie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Size_ID_Color_Product]    Script Date: 1/4/2021 3:30:09 PM ******/
CREATE NONCLUSTERED INDEX [IX_Size_ID_Color_Product] ON [dbo].[Size]
(
	[ID_Color_Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_Account_ID_Account] FOREIGN KEY([ID_Account])
REFERENCES [dbo].[Account] ([ID_Account])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_Account_ID_Account]
GO
ALTER TABLE [dbo].[Color_Product]  WITH CHECK ADD  CONSTRAINT [FK_Color_Product_Product_ID_Product] FOREIGN KEY([ID_Product])
REFERENCES [dbo].[Product] ([ID_Product])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Color_Product] CHECK CONSTRAINT [FK_Color_Product_Product_ID_Product]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Account_ID_Account] FOREIGN KEY([ID_Account])
REFERENCES [dbo].[Account] ([ID_Account])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Account_ID_Account]
GO
ALTER TABLE [dbo].[Img_Product]  WITH CHECK ADD  CONSTRAINT [FK_Img_Product_Color_Product_ID_Color_Product] FOREIGN KEY([ID_Color_Product])
REFERENCES [dbo].[Color_Product] ([ID_Color_Product])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Img_Product] CHECK CONSTRAINT [FK_Img_Product_Color_Product_ID_Color_Product]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Account_ID_Account] FOREIGN KEY([ID_Account])
REFERENCES [dbo].[Account] ([ID_Account])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Account_ID_Account]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Color_Product_ID_Color_Product] FOREIGN KEY([ID_Color_Product])
REFERENCES [dbo].[Color_Product] ([ID_Color_Product])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Color_Product_ID_Color_Product]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order_ID_Order] FOREIGN KEY([ID_Order])
REFERENCES [dbo].[Order] ([ID_Order])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order_ID_Order]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Brand_ID_Brand] FOREIGN KEY([ID_Brand])
REFERENCES [dbo].[Brand] ([ID_Brand])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Brand_ID_Brand]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category_ID_Categorie] FOREIGN KEY([ID_Categorie])
REFERENCES [dbo].[Category] ([ID_Categorie])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category_ID_Categorie]
GO
ALTER TABLE [dbo].[Size]  WITH CHECK ADD  CONSTRAINT [FK_Size_Color_Product_ID_Color_Product] FOREIGN KEY([ID_Color_Product])
REFERENCES [dbo].[Color_Product] ([ID_Color_Product])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Size] CHECK CONSTRAINT [FK_Size_Color_Product_ID_Color_Product]
GO
USE [master]
GO
ALTER DATABASE [HubbleSpace_Final] SET  READ_WRITE 
GO
