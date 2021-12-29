using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;


namespace Tests
{
    public class TestGeneticAlgorithm
    {
        // this variable creates a standard genome
        private static float[] _oneThroughFiveGenome = new float[] {
                1, 2, 3, 4, 5,   1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,   1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,   1, 2, 3, 4, 5,
                1, 2, 3, 4, 5
        };

        private static float[] _allOnes = new float[]
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
        };
        
        private static float[] _allNines = new float[]
        {
            9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            9, 9, 9, 9, 9,
        };
        
        private GeneticAlgorithm.GeneticAlgorithm _geneticAlgorithm;

        [OneTimeSetUp]
        public void Setup()
        {
            _geneticAlgorithm = new GeneticAlgorithm.GeneticAlgorithm();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void TestGeneticAlgorithmExists()
        {
            Assert.IsNotNull(_geneticAlgorithm);
        }

        [Test]
        public void TestGeneticAlgorithmCreatesRandomGenome()
        {
            var randomShipGenome = _geneticAlgorithm.GenerateRandomShip();
            Assert.IsNotNull(randomShipGenome);
            Assert.IsTrue(randomShipGenome.Length == 35);
            foreach (var i in randomShipGenome)
            {
                Assert.IsTrue(i != 0, "A gene cannot be null");
            }
        }

        [Test]
        public void TestGeneticAlgorithmParsesGenome()
        {
            var shipGenome = new GeneticAlgorithm.ShipGenome(TestGeneticAlgorithm._oneThroughFiveGenome);
            Assert.IsNotNull(shipGenome.body, "Should have created a body");
            Assert.IsNotNull(shipGenome.bridge, "Should have created a bridge");
            Assert.IsNotNull(shipGenome.laserCannon, "Should have created a laserCannon");
            Assert.IsNotNull(shipGenome.missileLauncher, "Should have created a missileLauncher");
            Assert.IsNotNull(shipGenome.tractor, "Should have created a tractor");
            Assert.IsNotNull(shipGenome.turbine, "Should have created a turbine");
            Assert.IsNotNull(shipGenome.wing, "Should have created a wing");
        }

        [Test]
        public void TestGeneticAlgorithmCrossOver()
        {
            var shipGenomeA = new GeneticAlgorithm.ShipGenome(_allOnes);
            var shipGenomeB = new GeneticAlgorithm.ShipGenome(_allNines);
            var uniformCrossOver = GeneticAlgorithm.GeneticAlgorithm.UniformCrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome());
            Assert.IsTrue(uniformCrossOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(uniformCrossOver.Average() < 9, "Average must be lower than the higher genome.");
            var onePointCrossOver = GeneticAlgorithm.GeneticAlgorithm.OnePointCrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome());
            Assert.IsTrue(onePointCrossOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(onePointCrossOver.Average() < 9, "Average must be lower than the higher genome.");
            Assert.AreApproximatelyEqual(1f, onePointCrossOver[0], "First should be from A");
            Assert.AreApproximatelyEqual(9f, onePointCrossOver[onePointCrossOver.Length], "Last should be from B");
            var twoPointCrossOver = GeneticAlgorithm.GeneticAlgorithm.KPointCrossOver(
                shipGenomeA.GetGenome(),
                shipGenomeB.GetGenome(),
                2);
            Assert.IsTrue(twoPointCrossOver.Average() > 1, "Average must be higher than the lower genome.");
            Assert.IsTrue(twoPointCrossOver.Average() < 9, "Average must be lower than the higher genome.");
            Assert.AreApproximatelyEqual(1f, onePointCrossOver[0], "First must be from A");
            Assert.AreApproximatelyEqual(1f, onePointCrossOver[0], "Last must be from A");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestGeneticAlgorithmWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
