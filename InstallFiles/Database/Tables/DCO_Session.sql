SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DCO_Session](
	[SessionID] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[SessionData] [varbinary](max) NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[SessionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[DCO_Session] ADD  CONSTRAINT [DF_DCO_Session_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DCO_Session] ADD  CONSTRAINT [DF_DCO_Session_LastUpdateDate]  DEFAULT (getdate()) FOR [LastUpdateDate]
GO

