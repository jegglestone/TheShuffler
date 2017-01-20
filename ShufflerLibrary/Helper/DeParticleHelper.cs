namespace ShufflerLibrary.Helper
{
    using Decorator;
    using Model;

    public static class DeParticleHelper
    {
        public static void InsertDeParticleBeforeAndUnderline(
            SentenceDecorator bkBySentenceDecorator, int i)
        {
            var previousOrderNumber = i == 0
                ? bkBySentenceDecorator.Texts[i].pe_order - 10
                : bkBySentenceDecorator.Texts[i - 1].pe_order;

            bkBySentenceDecorator.Texts.Insert(
                i,
                CreateNewDeParticle(
                    previousOrderNumber, 1));
        }

        public static void InsertDeParticleAfterAndUnderline(
            SentenceDecorator bKBySentenceDecorator, int i)
        {
            bKBySentenceDecorator.Texts.Insert(
                i + 1,
                CreateNewDeParticle(
                    bKBySentenceDecorator.Texts[i].pe_order, 0));

            bKBySentenceDecorator.Texts[i].pe_merge_ahead = 1;
        }

        public static Text CreateNewDeParticle(int previous_pe_order, int pe_merge_ahead)
        {
            return new Text()
            {
                pe_text = " de ",
                pe_text_revised = " de ",
                pe_tag = "PY",
                pe_tag_revised = "PY",
                pe_order = previous_pe_order + 5,
                pe_merge_ahead = pe_merge_ahead
            };
        }
    }
}
