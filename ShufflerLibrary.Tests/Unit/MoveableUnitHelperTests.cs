namespace ShufflerLibrary.Tests.Unit
{
    using System.Collections.Generic;
    using Helper;
    using NUnit.Framework;

    [TestFixture]
    public class MoveableUnitHelperTests
    {
        [Test]
        public void WhenTwoUnitsReturnsPositions()
        {
            var texts = new List<Model.Text>
            {
                new Model.Text() {pe_text = "So far", pe_tag = "TM1"},
                new Model.Text() {pe_text = "This year", pe_tag = "TM2"},
                new Model.Text() {pe_text = ",", pe_tag = "BKP"},
                new Model.Text() {pe_text = "it", pe_tag = ""},
                new Model.Text() {pe_text = "has", pe_tag = ""},
                new Model.Text() {pe_text = "been", pe_tag = ""},
                new Model.Text() {pe_text = "bad", pe_tag = ""},
            };

            var returnedValue = MoveableUnitHelper.GetMoveableUnitPositions(
                texts, MoveableUnitHelper.NumberableUnitType.Timer, 2);

            Assert.That(returnedValue[0].StartPosition, Is.EqualTo(0));
            Assert.That(returnedValue[0].EndPosition, Is.EqualTo(0));

            Assert.That(returnedValue[1].StartPosition, Is.EqualTo(1));
            Assert.That(returnedValue[1].EndPosition, Is.EqualTo(1));
        }

        [Test]
        public void WhenTwoLargeTMUnitsReturnsTheirPositions()
        {
            var texts = new List<Model.Text>
            {
                new Model.Text() {pe_text = "So far", pe_tag = "TM1"},
                new Model.Text() {pe_text = "This time", pe_tag = "TM2"},
                new Model.Text() {pe_text = "last year", pe_tag = "TM3"},
                new Model.Text() {pe_text = " , ", pe_tag = "BKP"},
                new Model.Text() {pe_text = "the", pe_tag = ""},
                new Model.Text() {pe_text = "fourth quarter", pe_tag = "TM4"},
                new Model.Text() {pe_text = ",", pe_tag = "BKP"},
                new Model.Text() {pe_text = "it", pe_tag = ""},
                new Model.Text() {pe_text = "has", pe_tag = ""},
                new Model.Text() {pe_text = "been", pe_tag = ""},
                new Model.Text() {pe_text = "bad", pe_tag = ""},
            };

            var returnedValue = MoveableUnitHelper.GetMoveableUnitPositions(
                texts, MoveableUnitHelper.NumberableUnitType.Timer, 4);

            Assert.That(returnedValue[0].StartPosition, Is.EqualTo(0));
            Assert.That(returnedValue[0].EndPosition, Is.EqualTo(0));

            Assert.That(returnedValue[1].StartPosition, Is.EqualTo(1));
            Assert.That(returnedValue[1].EndPosition, Is.EqualTo(1));

            Assert.That(returnedValue[2].StartPosition, Is.EqualTo(2));
            Assert.That(returnedValue[2].EndPosition, Is.EqualTo(4));

            Assert.That(returnedValue[3].StartPosition, Is.EqualTo(5));
            Assert.That(returnedValue[3].EndPosition, Is.EqualTo(5));
        }

        [Test]
        public void TwoTimerUnitsSpreadApartReturnsPositions()
        {
            var texts =
                new List<Model.Text>()
                {
                    new Model.Text() {pe_text = " Real "},
                    new Model.Text() {pe_text = " GDP ",},
                    new Model.Text() {pe_text = " rose "},
                    new Model.Text() {pe_text = " this ", pe_tag_revised = "TM1"},
                    new Model.Text() {pe_text = " time "},
                    new Model.Text() {pe_text = " last year ", pe_tag_revised = "TM2"},
                    new Model.Text() {pe_text = " . ", pe_tag = "BKP"}
                };
            
            var returnedValue = MoveableUnitHelper.GetMoveableUnitPositions(
                texts, MoveableUnitHelper.NumberableUnitType.Timer, 2);

            Assert.That(returnedValue[0].StartPosition, Is.EqualTo(3));
            Assert.That(returnedValue[0].EndPosition, Is.EqualTo(4));

            Assert.That(returnedValue[1].StartPosition, Is.EqualTo(5));
            Assert.That(returnedValue[1].EndPosition, Is.EqualTo(5));
        }

        [Test]
        public void HandlesMultipleMDAndTMUnits()
        {
            var texts = new List<Model.Text>
            {
               new Model.Text() { pe_tag = "", pe_text = "real" },
               new Model.Text() { pe_tag = "", pe_text = "gdp" },
               new Model.Text() { pe_tag = "", pe_text = "rose" },
               new Model.Text() { pe_tag = "MD1", pe_text = "at" },
               new Model.Text() { pe_tag = "", pe_text = "an" },
               new Model.Text() { pe_tag = "", pe_text = "annual rate" },
               new Model.Text() { pe_tag = "MD2", pe_text = " of " },
               new Model.Text() { pe_tag = "", pe_text = "about" },
               new Model.Text() { pe_tag = "", pe_text = "2" },
               new Model.Text() { pe_tag = "", pe_text = "percent" },
               new Model.Text() { pe_tag = "MD3", pe_text = "in" },
               new Model.Text() { pe_tag = "", pe_text = "the" },
               new Model.Text() { pe_tag = "TM1", pe_text = "first quarter" },
               new Model.Text() { pe_tag = "", pe_text = "after" },
               new Model.Text() { pe_tag = "", pe_text = "increasing" },
               new Model.Text() { pe_tag = "MD4", pe_text = "at" },
               new Model.Text() { pe_tag = "", pe_text = "a" },
               new Model.Text() { pe_tag = "", pe_text = "3" },
               new Model.Text() { pe_tag = "", pe_text = "percent" },
               new Model.Text() { pe_tag = "", pe_text = "pace" },
               new Model.Text() { pe_tag = "MD5", pe_text = "in" },
               new Model.Text() { pe_tag = "", pe_text = "the" },
               new Model.Text() { pe_tag = "TM2", pe_text = "4th qtr" },
               new Model.Text() { pe_tag = "MD6", pe_text = " of " },
               new Model.Text() { pe_tag = "TMY", pe_text = "2011" },
               new Model.Text() { pe_tag = "BKP", pe_text = " . " }
            };

            var returnedValue = MoveableUnitHelper.GetMoveableUnitPositions(
                texts, MoveableUnitHelper.NumberableUnitType.Modifier, 6);

            Assert.That(returnedValue[0].StartPosition, Is.EqualTo(3)); //MD1
            Assert.That(returnedValue[0].EndPosition, Is.EqualTo(5));

            Assert.That(returnedValue[1].StartPosition, Is.EqualTo(6)); //MD2
            Assert.That(returnedValue[1].EndPosition, Is.EqualTo(9));

            Assert.That(returnedValue[2].StartPosition, Is.EqualTo(10)); //MD3
            Assert.That(returnedValue[2].EndPosition, Is.EqualTo(14));

            Assert.That(returnedValue[3].StartPosition, Is.EqualTo(15)); //MD4
            Assert.That(returnedValue[3].EndPosition, Is.EqualTo(19));
            
            Assert.That(returnedValue[4].StartPosition, Is.EqualTo(20)); //MD5
            Assert.That(returnedValue[4].EndPosition, Is.EqualTo(22));

            Assert.That(returnedValue[5].StartPosition, Is.EqualTo(23)); //MD6
            Assert.That(returnedValue[5].EndPosition, Is.EqualTo(24));

            var returnedValue2 = MoveableUnitHelper.GetMoveableUnitPositions(
                texts, MoveableUnitHelper.NumberableUnitType.Timer, 2);

            Assert.That(returnedValue2[0].StartPosition, Is.EqualTo(12)); //TM1
            Assert.That(returnedValue2[0].EndPosition, Is.EqualTo(21));

            Assert.That(returnedValue2[1].StartPosition, Is.EqualTo(22)); //TM2
            Assert.That(returnedValue2[1].EndPosition, Is.EqualTo(24));
            
            Assert.That(returnedValue2.Length, Is.EqualTo(2));
        }

        [Test]
        public void HeavilyShuffledDocumentStillReturnsPositions()
        {
            /*
             "CS after | 
             increasing |
             MD4 at |
             PREN4 a |
             3| 
             percent | 
             pace |
             MD5 in |
             PREN5 the |
             TM2 fourth quarter |
             MD6 of |
             TMY2011|
             BKP , | 
             real | 
             gross domestic product | 
             ( | 
             (gdp), | 
             ), | 
             rose |
             MD1 at |
             PREN1 an | 
             annual rate |
             MD2 of |
             PREN2 about |2| percent |MD3 in |PREN3 the |TM1 first quarter | . |" 
             * */

            var texts = new List<Model.Text>
            {
               new Model.Text() { pe_tag = "CS", pe_text = "after" },
               new Model.Text() { pe_tag = "", pe_text = "increasing" },
               new Model.Text() { pe_tag = "MD4", pe_text = "at" },
               new Model.Text() { pe_tag = "", pe_text = "a" },
               new Model.Text() { pe_tag = "", pe_text = "3" },
               new Model.Text() { pe_tag = "", pe_text = "percent" },
               new Model.Text() { pe_tag = "", pe_text = "pace" },
               new Model.Text() { pe_tag = "MD5", pe_text = "in" },
               new Model.Text() { pe_tag = "", pe_text = "the" },
               new Model.Text() { pe_tag = "TM2", pe_text = "4th qtr" },
               new Model.Text() { pe_tag = "MD6", pe_text = " of " },
               new Model.Text() { pe_tag = "TMY", pe_text = "2011" },
               new Model.Text() { pe_tag = "BKP", pe_text = " , " },
               new Model.Text() { pe_tag = "", pe_text = "real (" },
               new Model.Text() { pe_tag = "", pe_text = "(gdp))," },
               new Model.Text() { pe_tag = "", pe_text = "rose" },
               new Model.Text() { pe_tag = "MD1", pe_text = "at" },
               new Model.Text() { pe_tag = "", pe_text = "an" },
               new Model.Text() { pe_tag = "", pe_text = "annual rate" },
               new Model.Text() { pe_tag = "MD2", pe_text = " of " },
               new Model.Text() { pe_tag = "", pe_text = "about" },
               new Model.Text() { pe_tag = "", pe_text = "2" },
               new Model.Text() { pe_tag = "", pe_text = "percent" },
               new Model.Text() { pe_tag = "MD3", pe_text = "in" },
               new Model.Text() { pe_tag = "", pe_text = "the" },
               new Model.Text() { pe_tag = "TM1", pe_text = "first quarter" },
               new Model.Text() { pe_tag = "BKP", pe_text = " . " }
            };

            var returnedValue = MoveableUnitHelper.GetMoveableUnitPositions(
                texts, MoveableUnitHelper.NumberableUnitType.Timer, 2);

            Assert.That(returnedValue[0].StartPosition, Is.EqualTo(9)); //TM1
            Assert.That(returnedValue[0].EndPosition, Is.EqualTo(24));

            Assert.That(returnedValue[1].StartPosition, Is.EqualTo(25)); //TM2
            Assert.That(returnedValue[1].EndPosition, Is.EqualTo(25));
        }
    }
}
