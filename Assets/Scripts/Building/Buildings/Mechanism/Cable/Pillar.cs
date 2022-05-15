using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pillar : Structure
{
    [Header("Pillar")]
    [SerializeField] private GameObject _prefubCable;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private List<LineController> _cables;

    [SerializeField] private GameObject _allObjects;
    [SerializeField] private Energy energy;
    private void Start()
    {
        energy = gameObject.GetComponent<Energy>();
        _allObjects = GameObject.FindGameObjectWithTag("GameWorld");
        _allObjects.GetComponent<PlayerActions>().EnergyNetworkAnalysis(energy, energy);
    }

    private void FixedUpdate()
    {
        _nearestObjects = _allObjects.GetComponent<PlayerActions>().ConnectingToDataBase(gameObject);

        if (_nearestObjects.Count > 0)
        {
            foreach (GameObject item in _nearestObjects)
            {
                ConnectionCable(item.transform);
            }
        }
        
        if (_points.Count > 0)
        {
            foreach (Transform item in _points.ToList())
            {
                if (Vector3.Distance(gameObject.transform.position, item.position) > 8f)
                {
                    DisconnectionCable(item.transform);
                }
            }
        }
    }


    private void ConnectionCable(Transform point)
    {
        Energy energyPoint = point.GetComponent<Energy>();

        if (energyPoint == null)                                            { return; }
        if (_cables.Any(line => line.point == point))                       { return; }
        if (point == gameObject.transform)                                  { return; }
        if (energyPoint.ItemConnect.Count >= energyPoint.MaxItemConnect)    { return; }
        if (energy.ItemConnect.Count >= energy.MaxItemConnect)              { return; }

        if (_allObjects.GetComponent<PlayerActions>().SearchGameNetwork(energy, energyPoint)) { return; }
        if (!_allObjects.GetComponent<PlayerActions>().SearchGameNetwork(energy, energyPoint))
        {
            _allObjects.GetComponent<PlayerActions>().EnergyNetworkAnalysis(energyPoint, energy);
            _allObjects.GetComponent<PlayerActions>().MergerNetworks(energy, energyPoint);
        }


        LineController cable = Instantiate(_prefubCable, transform.position, transform.rotation).GetComponent<LineController>();
        cable.point = point;

        _cables.Add(cable);
        _points.Add(point);

        energy.ItemConnect.Add(cable);
        cable.Connection();

        _allObjects.GetComponent<PlayerActions>().EnergyNetworkAnalysis(energyPoint, energy);
        //_allObjects.GetComponent<PlayerActions>().MergerNetworks(energy, energyPoint);
        StartCoroutine(energyPoint.Charging(energyPoint, energy));
    }

    private void DisconnectionCable(Transform point)
    {
        Energy energyPoint = point.GetComponent<Energy>();

        if (_cables.Any(line => line.point == point))
        {
            int index = _points.IndexOf(point);
            energy.ItemConnect.RemoveAt(index);
            _cables[index].DestroyLine();
            _cables.RemoveAt(index);
            _points.RemoveAt(index);
            energyPoint.StopCharging();
            _allObjects.GetComponent<PlayerActions>().EnergyNetworkExit(energy, energyPoint);
        }
    }
}
