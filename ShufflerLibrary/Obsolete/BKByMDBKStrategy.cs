namespace ShufflerLibrary.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Decorator;
    using Model;

    public class BKByMDBKStrategy : IStrategy
    {
        private BKBySentenceDecorator
            _bKBySentenceDecorator;

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsMDBK()|| text.pe_tag_revised_by_Shuffler == UnitTypes.MDBK))
                return sentence;

            _bKBySentenceDecorator =
                new BKBySentenceDecorator(sentence);

            if (sentence.Texts.Count(text => text.IsMDBK()) == 1)
            {
                ShuffleMDUnitsFromMDBKToBeforeFirstNNAfterBKBy();

                ShuffleMDUnitsFromAfterMDBKToImmediatelyAfterMDBK();

                ShufflePAST_DE_UnitToBeforeMDBKUnit();

                Shuffle_De_NNUnitAfterMDBKUnit();

                DeleteModifiers();

                ReplaceMDBKWithYouguan();
            }
            else
            {
                Shuffle_De_NNUnitAfterMDBKUnit();

                CreateSentenceOptions();

                sentence.SentenceHasMultipleOptions = true;
            }

            return sentence;
        }
 
        private void ShuffleMDUnitsFromMDBKToBeforeFirstNNAfterBKBy()
        {
            int bkByPosition = _bKBySentenceDecorator.BKByIndexPosition;
            int mdbkposition = _bKBySentenceDecorator.GetMDBKPosition(
                _bKBySentenceDecorator.Texts); 
            int mdUnitAfterByAndBeforeMDBKStartPosition =
                _bKBySentenceDecorator
                .Texts
                .Skip(bkByPosition)
                .Take(mdbkposition - bkByPosition)
                .ToList()
                .FindIndex(text => text.IsModifier) + bkByPosition;

            int lastMDPosition = mdbkposition - 1;

            if (mdUnitAfterByAndBeforeMDBKStartPosition > -1 && lastMDPosition > -1
                    && lastMDPosition > mdUnitAfterByAndBeforeMDBKStartPosition)
            {
                if (_bKBySentenceDecorator.ThereIsAnNNUnitBetweenBKByAndMDBK(
                    _bKBySentenceDecorator, bkByPosition, mdbkposition))
                {
                    int NNPosition = _bKBySentenceDecorator
                        .Texts
                        .Skip(bkByPosition)
                        .Take(mdbkposition - bkByPosition)
                        .ToList().FindIndex(text => text.IsNN)
                        + bkByPosition;

                    // remove MD unit
                    var modifierUnitToShuffle = _bKBySentenceDecorator.Texts.GetRange(mdUnitAfterByAndBeforeMDBKStartPosition,
                        lastMDPosition - mdUnitAfterByAndBeforeMDBKStartPosition + 1);

                    _bKBySentenceDecorator.Texts.RemoveRange(
                        mdUnitAfterByAndBeforeMDBKStartPosition, lastMDPosition - mdUnitAfterByAndBeforeMDBKStartPosition + 1);

                    //Insert before the NN
                    _bKBySentenceDecorator.Texts.InsertRange(
                        NNPosition, modifierUnitToShuffle);
                }
            }
        }

        private void ShuffleMDUnitsFromAfterMDBKToImmediatelyAfterMDBK()
        {
            int mdbkPosition = 
                _bKBySentenceDecorator.GetMDBKPosition(_bKBySentenceDecorator.Texts);  

            int modifierStartPosition = _bKBySentenceDecorator.Texts
                .Skip(mdbkPosition).ToList()
                .FindIndex(
                    text => text.IsModifier && text.pe_tag_revised_by_Shuffler != "MDBK") + mdbkPosition;

            int VBVBAPASTPRESPositionFollowingMDBK = _bKBySentenceDecorator.Texts
                .Skip(mdbkPosition)
                .ToList()
                .FindIndex(text => text.IsVbPastPres || text.IsVbVbaPast) + mdbkPosition;

            int modifierEndPosition = VBVBAPASTPRESPositionFollowingMDBK;

            if (modifierEndPosition > modifierStartPosition)
            {
                var mdUnit = _bKBySentenceDecorator.Texts.GetRange(modifierStartPosition,
                    modifierEndPosition - modifierStartPosition);

                _bKBySentenceDecorator.Texts.RemoveRange(modifierStartPosition,
                    modifierEndPosition - modifierStartPosition);

                _bKBySentenceDecorator.Texts.InsertRange(mdbkPosition + 1, mdUnit);
            }
        }

        private void ShufflePAST_DE_UnitToBeforeMDBKUnit()
        {
            if (_bKBySentenceDecorator.NNUnitBeforeBkBy(
                _bKBySentenceDecorator.TextsBeforeBy))
            {
                int nnPosition = _bKBySentenceDecorator.NNPosition;

                if (_bKBySentenceDecorator.IsPASTUnitBetweenNNandBKBy(
                    _bKBySentenceDecorator.TextsBeforeBy, nnPosition))
                {
                    // remove PAST+de
                    int pastPosition = _bKBySentenceDecorator
                        .GetFirstPASTUnitPositionAfterNN(
                            _bKBySentenceDecorator, nnPosition) + nnPosition;

                    var PAST_deUnit = _bKBySentenceDecorator.Texts.GetRange(
                        pastPosition,
                        _bKBySentenceDecorator.Texts[pastPosition].pe_merge_ahead + 1);

                    _bKBySentenceDecorator.Texts.RemoveRange(
                        pastPosition,
                        _bKBySentenceDecorator.Texts[pastPosition].pe_merge_ahead + 1);

                    // insert PAST+de before mdbk
                    _bKBySentenceDecorator.Texts.InsertRange(
                        _bKBySentenceDecorator.GetMDBKPosition(_bKBySentenceDecorator.Texts),
                        PAST_deUnit);
                }
            }
        }

        private static void ShufflePAST_DE_UnitToBeforeYouguanUnit(List<Text> newSentenceTexts)
        {
            var textsBeforeBy = newSentenceTexts.Take(newSentenceTexts.FindIndex(text => text.IsBKBy)).ToList();
            
            if (textsBeforeBy.Any(text => text.IsType(UnitTypes.PAST_Participle)))
            {
                int pastPosition =
                    textsBeforeBy.FindIndex(text => text.IsType(UnitTypes.PAST_Participle));

                if(newSentenceTexts[pastPosition + 1].pe_text_revised != " de ") return;

                const int PAST_deUnitSize = 2;

                var PAST_deUnit = newSentenceTexts.GetRange(
                        pastPosition,
                        PAST_deUnitSize);

                RemovePAST_De_UnitFromCurrentPosition(
                    newSentenceTexts, pastPosition, PAST_deUnitSize);

                InsertPAST_De_UnitBeforeYouguan(
                    newSentenceTexts, PAST_deUnit);
            }
        }

        private static void RemovePAST_De_UnitFromCurrentPosition(List<Text> newSentenceTexts, int pastPosition, int PAST_deUnitSize)
        {
            newSentenceTexts.RemoveRange(
                pastPosition,
                PAST_deUnitSize);
        }

        private static void InsertPAST_De_UnitBeforeYouguan(
            List<Text> newSentenceTexts, List<Text> PAST_deUnit)
        {
            newSentenceTexts.InsertRange(
                newSentenceTexts.FindIndex(
                    text => text.pe_tag_revised_by_Shuffler == "youguan"),
                PAST_deUnit);
        }

        private void Shuffle_De_NNUnitAfterMDBKUnit()
        {
            /*
             Move de +NN1 unit to after MDBK unit or the last MD unit after MDBK
            if there are any such MD units after MDBK.             
             * */
             
            if (_bKBySentenceDecorator.TextsBeforeBy.Any(
                text => text.pe_text_revised == " de " 
                && text.pe_merge_ahead == 1))
            {
                int dePosition = _bKBySentenceDecorator
                    .TextsBeforeBy
                    .FindIndex(text => text.pe_text_revised == " de "
                                       && text.pe_merge_ahead == 1);

                if (DeUnitIsNotFollowedByNNUnit(dePosition))
                    return;

                int unitSize = _bKBySentenceDecorator.Texts[dePosition].pe_merge_ahead + 1;

                var deNNUnit = _bKBySentenceDecorator.Texts.GetRange(
                    dePosition, unitSize);

                _bKBySentenceDecorator.Texts.RemoveRange(
                    dePosition, unitSize);

                //mdbkPosition
                //if modifiers move after them
                //otherwise move straight after 
                if (_bKBySentenceDecorator.FirstModifierAfterMDBK != -1)
                {
                    int firstModifierAfterMDBKPosition =
                        _bKBySentenceDecorator.FirstModifierAfterMDBK;

                    int modifierEndPosition =
                        _bKBySentenceDecorator.Texts.Skip(firstModifierAfterMDBKPosition)
                            .ToList()
                            .FindIndex(text => text.IsVbPastPres || text.IsVbVbaPast)
                            + firstModifierAfterMDBKPosition;

                    // insert deNN after Modifiers
                    _bKBySentenceDecorator.Texts.InsertRange(
                        modifierEndPosition, deNNUnit);
                }
                else
                {
                    _bKBySentenceDecorator.Texts.InsertRange(
                        _bKBySentenceDecorator
                            .GetMDBKPosition(_bKBySentenceDecorator.Texts) + 1,
                        deNNUnit);
                }
            }
        }

        private bool DeUnitIsNotFollowedByNNUnit(int dePosition)
        {
            return !_bKBySentenceDecorator.Texts[dePosition + 1].IsType(UnitTypes.NN);
        }

        private void DeleteModifiers()
        {
            for (int index = 0; index < _bKBySentenceDecorator.Texts.Count; index++)
            {
                if (_bKBySentenceDecorator.Texts[index].IsModifier &&
                        _bKBySentenceDecorator.Texts[index].pe_tag_revised_by_Shuffler != UnitTypes.MDBK)
                {
                    _bKBySentenceDecorator.Texts.Remove(
                        _bKBySentenceDecorator.Texts[index]);

                    index--;
                }
            }
        }

        private void ReplaceMDBKWithYouguan()
        {
            var mdbkText = 
                _bKBySentenceDecorator.Texts.Find(text => text.IsMDBK());

            mdbkText.pe_text_revised = " youguan ";
            mdbkText.pe_tag_revised_by_Shuffler = "youguan";
        }

        private void CreateSentenceOptions()
        {
            int modifierCount = _bKBySentenceDecorator.Texts.Count(text => text.IsModifier);

            for (int i = 0; i < modifierCount; i++)
            {
                List<Text> newSentenceTexts = new List<Text>();

                MapExistingSentenceTextsToNewSentence(newSentenceTexts, i + 1);
                
                var modifiers = newSentenceTexts.Where(
                    text => text.IsModifier).ToList();

                modifierCount = modifiers.Count;

                modifiers[i].pe_tag_revised_by_Shuffler = "youguan";
                modifiers[i].pe_text_revised = " youguan ";

                ShufflePAST_DE_UnitToBeforeYouguanUnit(newSentenceTexts);
                
                DeleteAllMDsExceptYouguan(newSentenceTexts);

                if (i == 0)
                    _bKBySentenceDecorator.Texts = newSentenceTexts;
                else
                    _bKBySentenceDecorator.Texts.AddRange(newSentenceTexts);
            }
        }

        private static void DeleteAllMDsExceptYouguan(List<Text> newSentenceTexts)
        {
            for (int j = 0; j < newSentenceTexts.Count; j++)
            {
                if (newSentenceTexts[j].IsModifier
                    && newSentenceTexts[j].pe_tag_revised_by_Shuffler != "youguan")
                {
                    newSentenceTexts.Remove(newSentenceTexts[j]);
                    j--;
                }
            }
        }

        private List<Text> sentenceTexts;

        private void MapExistingSentenceTextsToNewSentence(
            List<Text> newSentenceTexts, int sentenceOption)
        {
            if (sentenceTexts == null)
                sentenceTexts = _bKBySentenceDecorator.Texts;

            foreach (var text in sentenceTexts)  
            {
                newSentenceTexts.Add(new Text()
                {
                    pe_tag_revised = text.pe_tag_revised,
                    pe_tag = text.pe_tag,
                    pe_text = text.pe_text,
                    pe_para_no = text.pe_para_no,
                    pe_text_revised = text.pe_text_revised,
                    pe_tag_revised_by_Shuffler = text.pe_tag_revised_by_Shuffler,
                    pe_order = text.pe_order,
                    pe_merge_ahead = text.pe_merge_ahead,
                    pe_user_id = text.pe_user_id,
                    pe_C_num = text.pe_C_num,
                    pe_phrase_id = text.pe_phrase_id,
                    pe_rule_applied = text.pe_rule_applied,
                    pe_word_id = text.pe_word_id,
                    Sentence_Option = sentenceOption
                });
            }
        }
    }
}
