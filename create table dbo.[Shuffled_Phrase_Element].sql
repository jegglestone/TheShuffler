IF EXISTS (
SELECT * FROM SYS.TABLES T WHERE T.NAME = 'Shuffled_Phrase_Element' 
)
	DROP TABLE [dbo].[Shuffled_Phrase_Element]
	GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Shuffled_Phrase_Element](
	[pe_id] [int] IDENTITY(1,1) NOT NULL,
	[pe_pmd_id] [int] NOT NULL,
	[pe_user_id] [int] NOT NULL,
	[pe_para_no] [int] NOT NULL,
	[pe_phrase_id] [int] NULL,
	[pe_word_id] [int] NULL,
	[pe_tag] [nvarchar](10) NULL,
	[pe_text] [nvarchar](2000) NOT NULL,
	[pe_tag_revised] [nvarchar](10) NULL,
	[pe_tag_revised_by_Shuffler] [nvarchar](10) NULL,
	[pe_merge_ahead] [int] NOT NULL DEFAULT ((0)),
	[pe_text_revised] [nvarchar](2000) NULL,
	[pe_rule_applied] [nvarchar](500) NULL,
	[pe_order] [int] NULL,
	[pe_C_num] [int] NULL,
	[sentence_no][int] NULL,
	[sentence_option][int] NULL,
	[sentence_option_selected][int] NULL,
 CONSTRAINT [PK_Shuffled] PRIMARY KEY CLUSTERED 
(
	[pe_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
	ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


