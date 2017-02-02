USE [Shuffler]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ShuffledState](
	[pe_pmd_id] [int] NOT NULL,
	[pe_para_no] [int] NULL,
	[sentence_identifier] [uniqueidentifier] NULL,
	[SentenceStructure] [text] NULL,
	[Strategy Applied] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


