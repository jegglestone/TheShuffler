INSERT INTO [dbo].[Shuffled_Phrase_Element]  
(
[pe_pmd_id],
[pe_user_id],
[pe_para_no],
[pe_phrase_id],
[pe_word_id],[pe_tag],[pe_text],[pe_tag_revised],[pe_tag_revised_by_Shuffler],[pe_merge_ahead],[pe_text_revised],[pe_rule_applied],[pe_order],[pe_C_num],[sentence_no],[sentence_option],[sentence_option_selected]) VALUES
(2016,4,4,0,4279,'NN', ' He ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,NULL,'NULL', ' shouted ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,NULL,'NULL', ' loudly ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,22,'BKP', ' , ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,4280,'ADV', ' emotionally ','VB',NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,33,'DYN2', ' and ','BK',NULL,0,NULL,'Prime-DYN2-Tag-as-BK',(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,4281,'ADV', ' non-stop ','VB',NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1),
(2016,4,4,0,29,'BKP', ' . ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,1,1)
GO

INSERT INTO [dbo].[Shuffled_Phrase_Element]  
(
[pe_pmd_id],
[pe_user_id],
[pe_para_no],
[pe_phrase_id],
[pe_word_id],[pe_tag],[pe_text],[pe_tag_revised],
[pe_tag_revised_by_Shuffler],[pe_merge_ahead],[pe_text_revised],[pe_rule_applied],[pe_order],[pe_C_num],
[sentence_no],[sentence_option],[sentence_option_selected]) VALUES
(2016,4,4,0,4279,'NN', ' He ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,NULL,'NULL', ' shouted ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,4280,'ADV', ' emotionally ','VB',NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,22,'BKP', ' , ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,NULL,'NULL', ' loudly ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,33,'DYN2', ' and ','BK',NULL,0,NULL,'Prime-DYN2-Tag-as-BK',(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,4281,'ADV', ' non-stop ','VB',NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1),
(2016,4,4,0,29,'BKP', ' . ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,2,1)

GO


INSERT INTO [dbo].[Shuffled_Phrase_Element]  
(
[pe_pmd_id],
[pe_user_id],
[pe_para_no],
[pe_phrase_id],
[pe_word_id],[pe_tag],[pe_text],[pe_tag_revised],
[pe_tag_revised_by_Shuffler],[pe_merge_ahead],[pe_text_revised],[pe_rule_applied],[pe_order],[pe_C_num],
[sentence_no],[sentence_option],[sentence_option_selected]) VALUES
(2016,4,4,0,4279,'NN', ' He ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,NULL,'NULL', ' shouted ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,4281,'ADV', ' non-stop ','VB',NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,22,'BKP', ' , ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,4280,'ADV', ' emotionally ','VB',NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,33,'DYN2', ' and ','BK',NULL,0,NULL,'Prime-DYN2-Tag-as-BK',(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,NULL,'NULL', ' loudly ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1),
(2016,4,4,0,29,'BKP', ' . ',NULL,NULL,0,NULL,NULL,(SELECT MAX(pe_order) + 10 FROM [dbo].[Shuffled_Phrase_Element]),0,1,3,1)

GO


SELECT * FROM  [dbo].[Shuffled_Phrase_Element]  