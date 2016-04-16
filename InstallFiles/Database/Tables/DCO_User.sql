
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DCO_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[UserPassword] [varchar](255) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[IsHidden] [bit] NOT NULL,
	[ChangePassword] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DCO_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[DCO_User] ADD  CONSTRAINT [DF_DCO_User_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[DCO_User] ADD  CONSTRAINT [DF_DCO_User_IsHidden]  DEFAULT ((0)) FOR [IsHidden]
GO

ALTER TABLE [dbo].[DCO_User] ADD  CONSTRAINT [DF_DCO_User_ChangePassword]  DEFAULT ((1)) FOR [ChangePassword]
GO

ALTER TABLE [dbo].[DCO_User] ADD  CONSTRAINT [DF_DCO_User_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DCO_User] ADD  CONSTRAINT [DF_DCO_User_LastUpdateDate]  DEFAULT (getdate()) FOR [LastUpdateDate]
GO

