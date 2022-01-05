using System;
using System.Linq;
using GeneticAlgorithm;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class Headquarters : MonoBehaviour
{

    private GameObject _protonLegacyPrefab;
    private Func<Individual[], float[][]> _protonLegacyGA;
    
    private GeneticAlgorithm.GeneticAlgorithm _ga;

    private GameObject _shipsContainer;
    private GameObject _gameObject;

    // Start is called before the first frame update
    private void Awake()
    {
        _protonLegacyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/Prefabs/ProtonLegacy1.prefab"
        );
        _gameObject = GameObject.Find("SpawnPoints");
        _shipsContainer = GameObject.Find("Ships");
    }

    void Start()
    {
        _ga = new GeneticAlgorithm.GeneticAlgorithm();
        _protonLegacyGA = _ga.GAFactory(
            (achievements) => achievements.Sum(),
            (r) => GeneticAlgorithm.GeneticAlgorithm.SelectionRoulette(r, 0.0f),
            GeneticAlgorithm.GeneticAlgorithm.MatchingLeaderChoice,
            GeneticAlgorithm.GeneticAlgorithm.UniformCrossOver
            );
    }

    void _setGeneticAlgorithm()
    {
        _ga = this._ga == null 
            ? new GeneticAlgorithm.GeneticAlgorithm()
            : _ga;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            var currentGeneration = _shipsContainer.GetComponentsInChildren<ProtonLegacy>();
            var currentGen = currentGeneration.Select(
                r => r.GetIndividual()
            ).ToArray();
            GeneticAlgorithm.GeneticAlgorithm.SetArbitraryAchievements(currentGen);
            SpawnGeneration();
        }
        
    }

    public ShipGenome NewShipGenome()
    {
        _setGeneticAlgorithm();
        var randomShip = _ga.GenerateRandomShip();
        return new ShipGenome(randomShip);
    }

    /// <summary>
    /// Creates a generation of N random genomes of ships.
    /// </summary>
    /// <param name="n">number of different random genomes to create</param>
    /// <returns></returns>
    public ShipGenome[] NewShipGenomeGeneration(int n)
    {
        return (from s in new float[n] select NewShipGenome()).ToArray();
    }

    private GameObject[] SpawnGeneration()
    {
        var currentGeneration = _shipsContainer.GetComponentsInChildren<ProtonLegacy>();
        ShipGenome[] genomes;
        if (currentGeneration.Length == 0)
        {
            genomes = new ShipGenome[7];
            for (var i = 0; i < 7; i++)
            {
                genomes[i] = NewShipGenome();
            }
        }
        else
        {
            var currentGen = currentGeneration.Select(
                r => r.GetIndividual()
            ).ToArray();
            genomes = _protonLegacyGA(currentGen).Select(r => new ShipGenome(r)).ToArray();
        }
        foreach (Transform ship in _shipsContainer.transform) Destroy(ship.gameObject);
        var spawnPoints = ( from Transform i in _gameObject.transform select i).ToArray();
        var result = new GameObject[genomes.Length];
        for (var i = 0; i < result.Length; i++)
        {
            var ship = Object.Instantiate(
                                           _protonLegacyPrefab,
                                           spawnPoints[i].position,
                                           spawnPoints[i].rotation,
                                           _shipsContainer.transform
                                       );
            ship.GetComponent<ProtonLegacy>().SetGenome(genomes[i]);
            result[i] = ship;
        }
        return result;
    }

}
