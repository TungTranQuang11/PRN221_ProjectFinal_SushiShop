CREATE DATABASE [FoodShop]
GO

USE [FoodShop]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/11/2024 10:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 11/11/2024 10:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](15) NULL,
	[Address] [varchar](255) NULL,
	[Password] [varchar](50) NULL,
	[Role] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/11/2024 10:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](10, 2) NULL,
	[Status] [varchar](15) NULL,
	[Ship_Address] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/11/2024 10:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/11/2024 10:20:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](100) NOT NULL,
	[Description] [varchar](255) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[QuantityInStock] [int] NULL,
	[CategoryID] [int] NULL,
	[Image] [varchar](255) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (1, N'Sushi')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (2, N'Pizza')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (3, N'Burger')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (4, N'Steak')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (5, N'Chicken')
INSERT [dbo].[Category] ([CategoryID], [CategoryName]) VALUES (6, N'Cake')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerID], [FullName], [Email], [Phone], [Address], [Password], [Role]) VALUES (1, N'John Doe', N'admin@gmail.com', N'1234567890', N'123 Main St', N'123', 1)
INSERT [dbo].[Customer] ([CustomerID], [FullName], [Email], [Phone], [Address], [Password], [Role]) VALUES (2, N'Jane Smith', N'janesmith@example.com', N'0987654321', N'456 Elm St', N'mypassword', 0)
INSERT [dbo].[Customer] ([CustomerID], [FullName], [Email], [Phone], [Address], [Password], [Role]) VALUES (3, N'Alex Johnson', N'alexj@example.com', N'1122334455', N'789 Oak St', N'secretpass', 0)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (1, N'Rainbow Roll', N'A vibrant medley of fresh fish atop seasoned rice, creating stunning and delicious Rainbow Sushi for all to enjoy together.', CAST(13.99 AS Decimal(10, 2)), 50, 1, N'rainbow_roll.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (2, N'Tuna Nigiri', N'Fresh, succulent tuna placed delicately over seasoned rice, delivering a perfect bite of tradition and delicious flavors in each.', CAST(8.99 AS Decimal(10, 2)), 100, 1, N'tuna_nigiri.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (3, N'California Roll', N'A delightful combination of crab, avocado, and cucumber rolled with vinegared rice and seaweed, creating a classic sushi treat.', CAST(7.99 AS Decimal(10, 2)), 80, 1, N'california_roll.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (4, N'Octopus Sushi', N'Tender octopus laid atop a bed of seasoned sushi rice, offering a unique and oceanic taste that delights the senses with each bite.', CAST(9.99 AS Decimal(10, 2)), 60, 1, N'octopus_sushi.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (5, N'Spicy Tuna Sushi', N'Spicy tuna blended with flavorful spices atop soft rice, bringing a kick to your sushi experience that will keep you coming back.', CAST(10.99 AS Decimal(10, 2)), 70, 1, N'spicy_tuna_sushi.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (6, N'Salmon Nigiri', N'Melt-in-your-mouth fresh salmon paired with seasoned rice for a classic, savory bite that leaves you wanting more every time.', CAST(8.99 AS Decimal(10, 2)), 90, 1, N'salmon_nigiri.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (7, N'Pepperoni Pizza', N'A classic pizza layered with savory pepperoni slices and melted cheese on a crispy crust, satisfying pizza cravings anytime.', CAST(15.99 AS Decimal(10, 2)), 30, 2, N'pepperoni_pizza.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (8, N'Mushroom Pizza', N'Earthy mushrooms spread across a cheesy, golden crust, creating a perfect harmony of flavors that will please any pizza lover.', CAST(14.99 AS Decimal(10, 2)), 25, 2, N'mushroom_pizza.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (9, N'BBQ Chicken Pizza', N'Grilled chicken drizzled with rich BBQ sauce atop a bed of cheese and crispy crust, bringing a delicious twist to classic pizza.', CAST(16.99 AS Decimal(10, 2)), 20, 2, N'bbq_chicken_pizza.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (10, N'BBQ Burger', N'Juicy beef patty smothered in smoky BBQ sauce, stacked high with fresh toppings in a soft bun, making for a delightful meal.', CAST(10.99 AS Decimal(10, 2)), 50, 3, N'bbq_burger.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (11, N'Lamb Burger', N'Tender lamb patty infused with herbs, served in a bun with a burst of Mediterranean flavors that will excite your taste buds.', CAST(12.99 AS Decimal(10, 2)), 40, 3, N'lamb_burger.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (12, N'Chicken Burger', N'Grilled chicken breast paired with crisp lettuce and fresh tomato in a soft, toasted bun, delivering a delightful taste experience.', CAST(9.99 AS Decimal(10, 2)), 60, 3, N'chicken_burger.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (13, N'T-Bone Steak', N'A perfectly grilled T-bone steak, delivering rich, meaty flavors with every bite that satisfies the hunger of meat lovers.', CAST(22.99 AS Decimal(10, 2)), 20, 4, N't_bone_steak.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (14, N'Ribeye Steak', N'Ribeye steak grilled to perfection, offering a tender texture and rich marbling that will make your dining experience unforgettable.', CAST(24.99 AS Decimal(10, 2)), 25, 4, N'ribeye_steak.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (15, N'Brisket Steak', N'Slow-cooked brisket steak, tender and flavorful, melting in your mouth and providing a savory satisfaction for every bite.', CAST(19.99 AS Decimal(10, 2)), 15, 4, N'brisket_steak.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (16, N'Chicken Stir-Fry', N'Crispy chicken stir-fried with fresh vegetables in a savory sauce, perfect for a quick bite that satisfies your cravings.', CAST(11.99 AS Decimal(10, 2)), 50, 5, N'chicken_stir_fry.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (17, N'Grilled Chicken Breast', N'Succulent grilled chicken breast seasoned with herbs, served with your choice of sides for a hearty and nutritious meal.', CAST(12.99 AS Decimal(10, 2)), 60, 5, N'grilled_chicken_breast.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (18, N'Spicy Chicken Fingers', N'Crispy chicken fingers coated in a spicy batter, perfect for dipping in your favorite sauce and satisfying your snack cravings.', CAST(9.99 AS Decimal(10, 2)), 70, 5, N'spicy_chicken_fingers.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (19, N'Whoopie Pie Cake', N'A fun and indulgent cake made from soft, chocolatey whoopie pies, sandwiched with cream.', CAST(6.99 AS Decimal(10, 2)), 30, 6, N'whoopie_pie_cake.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (20, N'Funfetti Cake', N'Bright and colorful funfetti cake filled with sprinkles, perfect for celebrations and bringing smiles to everyone who enjoys it.', CAST(7.99 AS Decimal(10, 2)), 20, 6, N'funfetti_cake.jpg', 0)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [Image], [Status]) VALUES (21, N'Strawberry Shortcake', N'A light and fluffy cake layered with fresh strawberries and whipped cream, offering a delightful dessert experience every time.', CAST(8.99 AS Decimal(10, 2)), 40, 6, N'strawberry_shortcake.jpg', 0)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
