using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace GeneticAlgorithm
{

    public class ShipGenome
    {
        public ShipPartGenome body;
        public ShipPartGenome bridge;
        public ShipPartGenome laserCannon;
        public ShipPartGenome missileLauncher;
        public ShipPartGenome tractor;
        public ShipPartGenome turbine;
        public ShipPartGenome wing;
        
        public ShipGenome (float[] data)
        {
            body = new ShipPartGenome(data.SubArray(0, 5));
            bridge = new ShipPartGenome(data.SubArray(5, 5));
            laserCannon = new ShipPartGenome(data.SubArray(10, 5));
            missileLauncher = new ShipPartGenome(data.SubArray(15, 5));
            tractor = new ShipPartGenome(data.SubArray(20, 5));
            turbine = new ShipPartGenome(data.SubArray(25, 5));
            wing = new ShipPartGenome(data.SubArray(30, 5));
        }

        /// <summary>
        /// Convert from Genome to Array.
        /// </summary>
        /// <returns></returns>
        public float[] GetGenome()
        {
            return body.GetGene()
                .Concat(bridge.GetGene())
                .Concat(laserCannon.GetGene())
                .Concat(missileLauncher.GetGene())
                .Concat(tractor.GetGene())
                .Concat(turbine.GetGene())
                .Concat(wing.GetGene()).ToArray();
        }
    }

}