namespace ShufflerLibrary.Helper
{
    using System;
    using Model;

    public class SentenceOrderReSetter
    {
        public static Sentence SetPeOrderAsc(Sentence sentence)
        {
            int[] pe_orders = new int[sentence.Texts.Count];

            for (int index = 0; index < sentence.Texts.Count; index++)
                pe_orders[index] =
                    sentence.Texts[index].pe_order;

            Array.Sort(pe_orders);

            for (int index = 0; index < sentence.Texts.Count; index++)
                sentence.Texts[index].pe_order =
                    pe_orders[index];

            return sentence;
        }
    }
}
