namespace ShufflerLibrary.Strategy
{
    using System.Linq;
    using Decorator;
    using Model;

    public class BKByMDBKStrategy : IStrategy
    {
        private BKBySentenceDecorator
            _bKBySentenceDecorator;

        public Sentence ShuffleSentence(Sentence sentence)
        {
            if (!sentence.Texts.Any(text => text.IsMDBK()))
                return sentence;

            _bKBySentenceDecorator =
                new BKBySentenceDecorator(sentence);

            ShuffleMDUnitsFromMDBKToBeforeFirstNNAfterBKBy();

            ShuffleMDUnitsFromAfterMDBKToImmediatelyAfterMDBK();

            ShufflePAST_DE_UnitToBeforeMDBKUnit();

            Shuffle_De_NNUnitAfterMDBKUnit();

            DeleteModifiers();

            ReplaceMDBKWithYouguan();

            return sentence;
        }

        private void ShuffleMDUnitsFromMDBKToBeforeFirstNNAfterBKBy(
            )
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
                if (ThereIsAnNNUnitBetweenBKByAndMDBK(
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

            var mdUnit = _bKBySentenceDecorator.Texts.GetRange(modifierStartPosition,
                modifierEndPosition - modifierStartPosition );

            _bKBySentenceDecorator.Texts.RemoveRange(modifierStartPosition,
                modifierEndPosition - modifierStartPosition );

            _bKBySentenceDecorator.Texts.InsertRange(mdbkPosition+1, mdUnit);
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

                if (!_bKBySentenceDecorator.Texts[dePosition + 1].IsType(UnitTypes.NN))
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
                    // TODO: need a test for this path
                    // if no modifiers put striaght after MDBK
                    // move straight after MDBK
                    _bKBySentenceDecorator.Texts.InsertRange(
                        _bKBySentenceDecorator
                            .GetMDBKPosition(_bKBySentenceDecorator.Texts) + 1,
                        deNNUnit);
                }
            }
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

        private static bool ThereIsAnNNUnitBetweenBKByAndMDBK(
            BKBySentenceDecorator bKBySentenceDecorator, int bkByPosition, int mdbkposition)
        {
            return bKBySentenceDecorator
                .Texts
                .Skip(bkByPosition)
                .Take(mdbkposition - bkByPosition)
                .ToList()
                .Any(text => text.IsNN);
        }
    }
}
