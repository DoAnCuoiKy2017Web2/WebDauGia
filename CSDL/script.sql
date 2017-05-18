USE [QuanLyDauGia]
GO
/****** Object:  Table [dbo].[AuctionHistorys]    Script Date: 5/18/2017 11:39:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuctionHistorys](
	[AucID] [int] IDENTITY(1,1) NOT NULL,
	[ProID] [int] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[AucPrice] [float] NOT NULL,
	[Time] [datetime] NOT NULL,
 CONSTRAINT [PK_AuctionHistorys] PRIMARY KEY CLUSTERED 
(
	[AucID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/18/2017 11:39:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CatID] [int] IDENTITY(1,1) NOT NULL,
	[CatName] [nvarchar](50) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 5/18/2017 11:39:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CmtID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[ProID] [int] NOT NULL,
	[Comment] [ntext] NOT NULL,
	[TimeCmt] [nchar](10) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[CmtID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 5/18/2017 11:39:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProID] [int] IDENTITY(1,1) NOT NULL,
	[ProName] [nvarchar](50) NOT NULL,
	[CatID] [int] NOT NULL,
	[FullDes] [ntext] NULL,
	[TinyDes] [nvarchar](80) NULL,
	[Salesman] [nvarchar](50) NOT NULL,
	[Price] [float] NOT NULL,
	[Quantity] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[StepPrice] [int] NOT NULL,
	[AucPrice] [float] NULL,
	[Owner] [nvarchar](50) NULL,
	[OwnerPrice] [float] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/18/2017 11:39:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](4) NULL,
	[DateOfBirth] [date] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Phone] [nchar](14) NULL,
	[Address] [nvarchar](255) NOT NULL,
	[Credit] [float] NOT NULL,
	[DateCreate] [date] NOT NULL,
	[AllowAuction] [bit] NOT NULL,
	[AllowSales] [bit] NOT NULL,
	[Reliability] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CatID], [CatName], [Status]) VALUES (1, N'Khác', NULL)
INSERT [dbo].[Categories] ([CatID], [CatName], [Status]) VALUES (2, N'Điện thoại', NULL)
INSERT [dbo].[Categories] ([CatID], [CatName], [Status]) VALUES (3, N'Máy tính', NULL)
INSERT [dbo].[Categories] ([CatID], [CatName], [Status]) VALUES (4, N'Nhà', NULL)
INSERT [dbo].[Categories] ([CatID], [CatName], [Status]) VALUES (5, N'Xe', NULL)
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([CmtID], [UserName], [ProID], [Comment], [TimeCmt]) VALUES (1, N'User01', 1, N'Sản phẩm rất tốt', N'12-12-2000')
SET IDENTITY_INSERT [dbo].[Comments] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice], [Status]) VALUES (2, N'Sản phẩm 1', 1, N'Sản phẩm để test', N'Test', N'1', 12000, 1, CAST(N'2000-12-12 00:00:00.000' AS DateTime), CAST(N'2000-12-19 00:00:00.000' AS DateTime), 2000, 15000, N'1', 30000, NULL)
INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice], [Status]) VALUES (9, N'Sản phẩm 1', 1, N'Sản phẩm để test', N'Test', N'1', 12000, 1, CAST(N'2000-12-12 00:00:00.000' AS DateTime), CAST(N'2000-12-19 00:00:00.000' AS DateTime), 2000, 15000, N'1', 30000, NULL)
INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice], [Status]) VALUES (10, N'Sản phẩm mới thêm', 4, N'<p><span style="color:#e74c3c">Nh&agrave; rất đẹp, rộng r&atilde;i , tho&aacute;ng m&aacute;t</span></p>
', N'Nhà đẹp', N'Admin', 1000000, 1, CAST(N'2017-05-25 00:00:00.000' AS DateTime), CAST(N'2017-05-27 00:00:00.000' AS DateTime), 100000, 0, NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice], [Status]) VALUES (11, N'Sản phẩm mới thêm', 4, N'<p><span style="color:#e74c3c">Nh&agrave; rất đẹp, rộng r&atilde;i , tho&aacute;ng m&aacute;t</span></p>
', N'Nhà đẹp', N'Admin', 1000000, 1, CAST(N'2017-05-25 00:00:00.000' AS DateTime), CAST(N'2017-05-27 00:00:00.000' AS DateTime), 100000, 0, NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice], [Status]) VALUES (12, N'Sản phẩm mới thêm', 4, N'<p><span style="color:#e74c3c">Nh&agrave; rất đẹp, rộng r&atilde;i , tho&aacute;ng m&aacute;t</span></p>
', N'Nhà đẹp', N'Admin', 1000000, 1, CAST(N'2017-05-25 00:00:00.000' AS DateTime), CAST(N'2017-05-27 00:00:00.000' AS DateTime), 100000, 0, NULL, NULL, NULL)
INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice], [Status]) VALUES (13, N'Sản phẩm mới thêm', 4, N'<p><span style="color:#e74c3c">Nh&agrave; rất đẹp, rộng r&atilde;i , tho&aacute;ng m&aacute;t</span></p>
', N'Nhà đẹp', N'Admin', 1000000, 1, CAST(N'2017-05-25 00:00:00.000' AS DateTime), CAST(N'2017-05-27 00:00:00.000' AS DateTime), 100000, 0, N'admin', 100, NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
INSERT [dbo].[Users] ([UserName], [Password], [Name], [Gender], [DateOfBirth], [Email], [Phone], [Address], [Credit], [DateCreate], [AllowAuction], [AllowSales], [Reliability]) VALUES (N'abc', N'E10ADC3949BA59ABBE56E057F20F883E', N'abc def', NULL, CAST(N'2017-12-05' AS Date), N'abc@gmail.com', NULL, N'HN', 0, CAST(N'2017-05-12' AS Date), 0, 0, N'10/10     ')
INSERT [dbo].[Users] ([UserName], [Password], [Name], [Gender], [DateOfBirth], [Email], [Phone], [Address], [Credit], [DateCreate], [AllowAuction], [AllowSales], [Reliability]) VALUES (N'Admin', N'123123', N'Admin', N'Nam', CAST(N'2000-12-30' AS Date), N'Admin@gmail.com', N'012345678     ', N'HCM', 2000000, CAST(N'2010-01-01' AS Date), 1, 1, N'10/10     ')
INSERT [dbo].[Users] ([UserName], [Password], [Name], [Gender], [DateOfBirth], [Email], [Phone], [Address], [Credit], [DateCreate], [AllowAuction], [AllowSales], [Reliability]) VALUES (N'duynguyen', N'E10ADC3949BA59ABBE56E057F20F883E', N'Duy nguyễn', N'Nam', CAST(N'2017-12-06' AS Date), N'nguyenduylf@gmail.com', NULL, N'HCM', 0, CAST(N'2017-05-12' AS Date), 0, 0, N'10/10     ')
INSERT [dbo].[Users] ([UserName], [Password], [Name], [Gender], [DateOfBirth], [Email], [Phone], [Address], [Credit], [DateCreate], [AllowAuction], [AllowSales], [Reliability]) VALUES (N'duynguyen2', N'E10ADC3949BA59ABBE56E057F20F883E', N'duy nguyen', N'Khác', CAST(N'2017-12-05' AS Date), N'nguyenduylf@gmail.com', N'123           ', N'HCM', 0, CAST(N'2017-05-12' AS Date), 0, 0, N'10/10     ')
