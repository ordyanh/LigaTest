USE [Liga]
GO

/****** Object:  Table [dbo].[Claims]    Script Date: 3/27/2026 1:41:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Claims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PolicyId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[IncidentDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[RequestedAmount] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Claims] ADD  DEFAULT ('Created') FOR [Status]
GO

ALTER TABLE [dbo].[Claims] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[Claims]  WITH CHECK ADD  CONSTRAINT [FK_Claims_Policies] FOREIGN KEY([PolicyId])
REFERENCES [dbo].[Policies] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Claims] CHECK CONSTRAINT [FK_Claims_Policies]
GO

ALTER TABLE [dbo].[Claims]  WITH CHECK ADD  CONSTRAINT [FK_Claims_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Claims] CHECK CONSTRAINT [FK_Claims_Users]
GO

ALTER TABLE [dbo].[Claims]  WITH CHECK ADD CHECK  (([Status]='Rejected' OR [Status]='Approved' OR [Status]='Created'))
GO


