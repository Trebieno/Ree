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
    [SerializeField] private List<GameObject> _nearestObjects;

    private void Start()
    {
        Energy._allObjects = GameObject.FindGameObjectWithTag("GameWorld");
        Energy._allObjects.GetComponent<PlayerActions>().EnergyNetworkAnalysis(Energy);
    }

    private void FixedUpdate()
    {
        _nearestObjects = Energy._allObjects.GetComponent<PlayerActions>().ConnectingToDataBase(gameObject);

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
        if (Energy.ItemConnect.Count >= Energy.MaxItemConnect)            { return; }

        if (Energy._allObjects.GetComponent<PlayerActions>().SearchGameNetwork(Energy, energyPoint)) { return; }

        if (!Energy._allObjects.GetComponent<PlayerActions>().SearchGameNetwork(Energy, energyPoint))
            Energy._allObjects.GetComponent<PlayerActions>().MergerNetworks(Energy, energyPoint);



        LineController cable = Instantiate(_prefubCable, transform.position, transform.rotation).GetComponent<LineController>();
        cable.point = point;

        _cables.Add(cable);
        _points.Add(point);

        Energy.ItemConnect.Add(cable);
        cable.Connection();

        Energy.GetComponent<PlayerActions>().EnergyNetworkAnalysis(energyPoint);
        StartCoroutine(energyPoint.Charging(energyPoint, Energy));
    }

    private void DisconnectionCable(Transform point)
    {
        Energy energyPoint = point.GetComponent<Energy>();

        if (_cables.Any(line => line.point == point))
        {
            int index = _points.IndexOf(point);
            Energy.ItemConnect.RemoveAt(index);
            _cables[index].DestroyLine();
            _cables.RemoveAt(index);
            
            _points.RemoveAt(index);

            
            energyPoint.StopCharging();
            Energy._allObjects.GetComponent<PlayerActions>().EnergyNetworkExit(Energy, energyPoint);
        }
    }
}
