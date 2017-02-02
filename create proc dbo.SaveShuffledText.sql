SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (
	SELECT * 
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[dbo].[SaveShuffledText]') 
	AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveShuffledText]
GO
-- =============================================
-- Author:		Joseph Egglestone
-- Create date: 28/16/2016
-- Description:	Insert a text item of a shuffled document

-- =============================================
CREATE PROCEDURE [dbo].[SaveShuffledText]

	@pe_pmd_id int
    ,@pe_user_id int
    ,@pe_para_no int
    ,@pe_phrase_id int = null
    ,@pe_word_id int = null
    ,@pe_tag nvarchar(10) = null
    ,@pe_text nvarchar(2000)
    ,@pe_tag_revised nvarchar(10) 
    ,@pe_merge_ahead int
    ,@pe_text_revised nvarchar(2000)
    ,@pe_rule_applied nvarchar(500) = null
    ,@pe_order int = null
    ,@pe_C_num int = null
	,@sentence_no int
	,@sentence_option int
	,@sentence_option_selected int
	,@sentence_identifier [uniqueidentifier]
	
AS
BEGIN
	
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[Shuffled_Phrase_Element]
			   ([pe_pmd_id]
			   ,[pe_user_id]
			   ,[pe_para_no]
			   ,[pe_phrase_id]
			   ,[pe_word_id]
			   ,[pe_tag]
			   ,[pe_text]
			   ,[pe_tag_revised]
			   ,[pe_merge_ahead]
			   ,[pe_text_revised]
			   ,[pe_rule_applied]
			   ,[pe_order]
			   ,[pe_C_num]
			   ,[Sentence_Identifier]
			   ,[sentence_no]
			   ,[sentence_option]
			   ,[sentence_option_selected])
		 VALUES
			   (@pe_pmd_id
			   ,@pe_user_id
			   ,@pe_para_no
			   ,@pe_phrase_id
			   ,@pe_word_id
			   ,@pe_tag
			   ,@pe_text
			   ,@pe_tag_revised
			   ,@pe_merge_ahead
			   ,@pe_text_revised
			   ,@pe_rule_applied
			   ,@pe_order
			   ,@pe_C_num
			   ,@sentence_identifier
			   ,@sentence_no
			   ,@sentence_option
			   ,@sentence_option_selected)
END
GO

-- GRANT EXECUTE ON dbo.[SaveShuffledText] TO [userName] AS [dbo]
--GO
