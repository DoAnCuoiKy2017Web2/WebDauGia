USE [QuanLyDauGia]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CatID], [CatName]) VALUES (1, N'Điện thoại')
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([CmtID], [UserName], [ProID], [Comment], [TimeCmt]) VALUES (1, N'User01', 1, N'Sản phẩm rất tốt', N'12-12-2000')
SET IDENTITY_INSERT [dbo].[Comments] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProID], [ProName], [CatID], [FullDes], [TinyDes], [Salesman], [Price], [Quantity], [StartTime], [EndTime], [StepPrice], [AucPrice], [Owner], [OwnerPrice]) VALUES (2, N'Sản phẩm 1', 1, N'Sản phẩm để test', N'Test', N'1', 12000, 1, CAST(N'2000-12-12 00:00:00.000' AS DateTime), CAST(N'2000-12-19 00:00:00.000' AS DateTime), 2000, 15000, N'1', 30000)
SET IDENTITY_INSERT [dbo].[Products] OFF
INSERT [dbo].[Users] ([UserName], [Password], [Name], [Gender], [DateOfBirth], [Email], [Phone], [Address], [Credit], [DateCreate], [AllowAuction], [AllowSales], [Reliability]) VALUES (N'Admin', N'123123', N'Admin', N'Nam', CAST(N'2000-12-30' AS Date), N'Admin@gmail.com', N'012345678     ', N'HCM', 2000000, CAST(N'2010-01-01' AS Date), 1, 1, N'10/10     ')
