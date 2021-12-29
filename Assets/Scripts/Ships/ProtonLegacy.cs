using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
using UnityEngine;

public class ProtonLegacy : MonoBehaviour
{
    public bool ready = false;
    private GameObject _headquartersGameObject;
    private Headquarters _headquarters;
    private ShipGenome _genome;
    public Transform[] bodies;
    public Transform[] bridges;
    public Transform[] laserCannons;
    public Transform[] missileLaunchers;
    public Transform[] tractors;
    public Transform[] turbines;
    public Transform[] wings;
    
    // Start is called before the first frame update
    void Start()
    {
        _headquartersGameObject = GameObject.Find("EnemyHeadQuarters");
        _headquarters = _headquartersGameObject.GetComponent<Headquarters>();
        if (_headquarters is null) return;
        var newGenome = _headquarters.NewShipGenome();
        if (newGenome != null) _genome = newGenome;
        _IdentifyParts();
        _ApplyGenome();
    }

    void _IdentifyParts()
    {
        bodies = GetParts("ShipBody");
        bridges = GetParts("ShipBridge");
        laserCannons = GetParts("ShipLaserCannon");
        missileLaunchers = GetParts("ShipMissileLauncher");
        tractors = GetParts("ShipTractor");
        turbines = GetParts("ShipTurbines");
        wings = GetParts("ShipWings");
    }

    private IEnumerable<Transform> _parts()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            yield return transform.GetChild(i);
        }
    }

    public Transform[] GetParts(string partType)
    {
        return _parts()
            .Where(r => r.CompareTag(partType))
            .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (_genome != null)
        {
        }
    }

    private void _ApplyGenome()
    {
        if (_genome == null) return;
        static Action<Transform> TranslateOnZ (float v) => 
            (Transform b) => b.Translate(new Vector3(0f, 0f, v), Space.Self);
        static Action<Transform> RotateOnY (float v) =>
            (Transform b) => b.Rotate(new Vector3(0f, v, 0f), Space.Self);
        var value = _genome.body.position;
        Utils.Iterables.SymmetricalApply(bodies, TranslateOnZ(value), TranslateOnZ(-1 * value));
        value = _genome.turbine.position;
        Utils.Iterables.SymmetricalApply(turbines, TranslateOnZ(value), TranslateOnZ(-1 * value));
        value = _genome.laserCannon.position;
        Utils.Iterables.SymmetricalApply(laserCannons, TranslateOnZ(value), TranslateOnZ(-1 * value));
        value = _genome.turbine.rotation;
        Utils.Iterables.SymmetricalApply(turbines, RotateOnY(value), RotateOnY(-1 * value));
        value = _genome.laserCannon.rotation;
        Utils.Iterables.SymmetricalApply(laserCannons, RotateOnY(value), RotateOnY(-1 * value));
    }

}