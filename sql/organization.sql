USE [DNN_MVC]
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 3/11/2020 4:49:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[ImagePath] [nvarchar](2000) NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Sp_DeleteOrganization]    Script Date: 3/11/2020 4:49:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_DeleteOrganization]
@OrganizationId int
AS
BEGIN
	DECLARE @Message NVARCHAR(200) = 'Something went wrong, please try again'
	BEGIN TRY
		UPDATE [dbo].[Organizations]
		   SET [IsDelete] = 1
		 WHERE OrganizationId = @OrganizationId
		SET @Message = 'Organization has been deleted successfully!'
		SELECT @OrganizationId AS OrganizationId, @Message AS [Message]

	END TRY
	BEGIN CATCH
		SELECT @OrganizationId AS OrganizationId, @Message AS [Message]
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetOrganization]    Script Date: 3/11/2020 4:49:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_GetOrganization]
AS
BEGIN
SELECT [OrganizationId]
      ,[Name]
      ,[Code]
      ,[ImagePath]
	  ,[IsDelete]
  FROM [dbo].[Organizations]
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetOrganizationById]    Script Date: 3/11/2020 4:49:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_GetOrganizationById] 
@OrganizationId int
AS
BEGIN
SELECT [OrganizationId]
      ,[Name]
      ,[Code]
      ,[ImagePath]
  FROM [dbo].[Organizations]
  where OrganizationId = @OrganizationId and IsDelete = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_SaveOrganization]    Script Date: 3/11/2020 4:49:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_SaveOrganization]
(@OrganizationId INT,
 @Name NVARCHAR(500),
 @Code NVARCHAR(50),
 @ImagePath NVARCHAR(2000))
AS
BEGIN
	DECLARE @Message NVARCHAR(200) = 'Something went wrong, please try again'
	IF(@OrganizationId IS NULL OR @OrganizationId = 0)
	BEGIN
		INSERT INTO [dbo].[Organizations]
				   ([Name]
				   ,[Code]
				   ,[ImagePath]
				   ,[IsDelete])
			 VALUES
				   (@Name
				   ,@Code
				   ,@ImagePath
				   ,0)
		SET @Message = 'Organization has been created successfully!'
	END
	ELSE 
	BEGIN
		UPDATE [dbo].[Organizations]
		   SET [Name] = @Name
			  ,[Code] = @Code
			  ,[ImagePath] = @ImagePath
			  ,[IsDelete] = 0
		 WHERE OrganizationId = @OrganizationId
		SET @Message = 'Organization has been updated successfully!'
	END
END
GO
