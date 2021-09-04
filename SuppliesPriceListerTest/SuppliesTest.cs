using buildxact_supplies.Readers;
using NUnit.Framework;
using System;
using System.Linq;

namespace SuppliesPriceListerTest
{
    [TestFixture]
    public class SuppliesTest
    {
        [Test]
        public void TestHumphriesConverter()
        {
            // ARRANGE
            var reader = new HumphriesFileReader();
            Guid g = Guid.NewGuid();
            var testData = new HumphriesData() { Description = "Test1", Id = g, Price = 12.35M };

            // ACT
            var output = reader.Convert(testData);

            // ASSERT
            Assert.AreEqual("Test1", output.Description);
            Assert.AreEqual(g.ToString(), output.Id);
            Assert.AreEqual(output.Price, output.Price);
        }

        [Test]
        public void TestGetSuppliesDataMegaCorp()
        {
            // ARRANGE
            var reader = new MegaCorpFileReader();
            
            // ACT
            var output = reader.GetSuppliesDataAsync("TestMegaCorp.json").Result.ToList();

            // ASSERT
            Assert.AreEqual(2, output.Count);
            var o1 = output[0];
            Assert.AreEqual("1", o1.Id);
            Assert.AreEqual("100 x 200 x 20mpa Internal Beam", o1.Description);
            Assert.AreEqual(4000, o1.Price);

            var o2 = output[1];
            Assert.AreEqual("0", o2.Id);
            Assert.AreEqual("100 x 350 Thickened Edge", o2.Description);
            Assert.AreEqual(3500, o2.Price);
        }

        [Test]
        public void TestGetSuppliesHumphriesCorp()
        {
            // ARRANGE
            var reader = new HumphriesFileReader();

            // ACT
            var output = reader.GetSuppliesDataAsync("TestHumphries.csv").Result.ToList();

            // ASSERT
            Assert.AreEqual(2, output.Count);
            var o1 = output[0];
            Assert.AreEqual("586e0bd4-a84c-4c39-a696-1fafdf85e5bb", o1.Id);
            Assert.AreEqual("Suspended Slab Formwork per m2", o1.Description);
            Assert.AreEqual(23.59, o1.Price);

            var o2 = output[1];
            Assert.AreEqual("e1b3e145-782b-43b3-a081-f3634a07db00", o2.Id);
            Assert.AreEqual("TM3 R11 Mesh", o2.Description);
            Assert.AreEqual(83.99, o2.Price);
        }
    }
}
