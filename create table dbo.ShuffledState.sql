USE [Shuffler]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (
SELECT * FROM SYS.TABLES T WHERE T.NAME = 'Shuffled_State' 
)
	DROP TABLE [dbo].[Shuffled_State]
	GO

CREATE TABLE [dbo].[Shuffled_State](
	[sentence_identifier] [uniqueidentifier],
	[sentence_structure] [text] NULL,
	[strategy_applied] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


