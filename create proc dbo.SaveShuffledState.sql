SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (
	SELECT * 
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[dbo].[SaveShuffledState]') 
	AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveShuffledState]
GO
-- =============================================
-- Author:		Joseph Egglestone
-- Create date: 28/16/2016
-- Description:	Save state of a shuffled document 
-- throughout shuffling cycle.

-- =============================================
CREATE PROCEDURE [dbo].[SaveShuffledState]

	@sentenceIdentifier [uniqueidentifier]
    ,@sentenceStructure NVARCHAR(MAX)
	,@ruleApplied VARCHAR(20)

	
AS
BEGIN
	
	SET NOCOUNT OFF;	

	INSERT INTO [dbo].[Shuffled_State]
           ([sentence_identifier]
           ,[sentence_structure]
           ,[strategy_applied])
     VALUES
           (@sentenceIdentifier
           ,@sentenceStructure
           ,@ruleApplied)
END
GO
