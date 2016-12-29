SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDocumentById]') AND type in (N'P', N'PC'))
DROP PROCEDURE dbo.GetDocumentById
GO
-- =============================================
-- Author:		Joseph Egglestone
-- Create date: 28/16/2016
-- Description:	Gets all paragraphs, sentences
--				and texts for a document by pe_pmd_id

--	EXEC dbo.GetDocumentById 2012
-- =============================================
CREATE PROCEDURE [dbo].[GetDocumentById]

	@pe_pmd_id int
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT * 
	FROM [dbo].[v3_Phrase_Element]
    WHERE pe_pmd_id = @pe_pmd_id
    ORDER BY pe_order
END
GO

-- GRANT EXECUTE ON dbo.GetDocumentById TO [userName] AS [dbo]
--GO
