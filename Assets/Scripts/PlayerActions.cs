using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class DataBaseNetwork
{
    public int Id;
    public List<Transform> Network = new List<Transform>();

    public DataBaseNetwork(int id, List<Transform> network)
    {
        Id = id;
        Network = network;
    }
}

public class PlayerActions : MonoBehaviour
{

    [SerializeField] private List<GameObject> _otherObjects;
    [SerializeField] private List<GameObject> _nearestObjects;

    [SerializeField] private List<DataBaseNetwork> _energyNetwork = new List<DataBaseNetwork>();



    private ShopBuilding _shopBuilding;

    private void Start()
    {
        _shopBuilding = GetComponent<ShopBuilding>();
    }

    private void Update()
    {
        if (_shopBuilding.StatusBuy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _shopBuilding.PurchaseBuilding();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                _shopBuilding.CancelBuy();
            }

            _shopBuilding.MoveHologram();
        }

        _otherObjects = GameObject.FindGameObjectsWithTag("Unit").ToList();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Building").ToList())
        {
            _otherObjects.Add(item);
        }
    }

    public List<GameObject> ConnectingToDataBase(GameObject Object)
    {
        //Debug.Log("Поделючаюсь к базе");
        _nearestObjects = _otherObjects.Where(x => Vector3.Distance(Object.transform.position, x.transform.position) < 8f).ToList();
        _nearestObjects = _nearestObjects.OrderBy(x => Vector3.Distance(Object.transform.position, x.transform.position)).ToList();

        return _nearestObjects;
    }


    public void CreateNetwork(Energy gObject)
    {
        if (SearchingInNetwork(gObject))
            return;
        gObject.Id = 1;
        for (int i = 0; i < _energyNetwork.Count; i++)
        {
            if (_energyNetwork.Any(i => i.Id == gObject.Id))
                gObject.Id++;
            else break;
        }
        var network = new List<Transform>() { gObject.transform };
        var data = new DataBaseNetwork(gObject.Id, network);
        _energyNetwork.Add(data);
    }

    public List<Transform> ObjectsNetwork(Energy gObject)
    {
        if (SearchingInNetwork(gObject))
        {
            int index = _energyNetwork.FindIndex(i => i.Id == gObject.Id);
            return _energyNetwork[index].Network;
        }
        return null;
    }

    public bool SearchingInNetwork(Energy gObject)
    {
        if (_energyNetwork.Any(i => i.Id == gObject.Id))
        {
            return true;
        }
        return false;
    }

    public bool SearchingInNetwork(Energy gObject1, Energy gObject2)
    {
        if (SearchingInNetwork(gObject2))
        {
            int index = _energyNetwork.FindIndex(i => i.Id == gObject1.Id);
            if(_energyNetwork[index].Network.Any(i => i == gObject2.transform))
                return true;
        }
        return false;
    }

    public void MergingNetworks(Energy gObject1, Energy gObject2)
    {
        List<Transform> network1 = ObjectsNetwork(gObject1);
        List<Transform> network2 = ObjectsNetwork(gObject2);

        for (int i = 0; i < network2.Count; i++)
        {
            network2[i].GetComponent<Energy>().Id = gObject1.Id;
            network1.Add(network2[i]);
        }

        int index1 = _energyNetwork.FindIndex(i => i.Id == gObject1.Id);
        _energyNetwork[index1].Network = network1;
        

        //int index2 = _energyNetwork.FindIndex(i => i.Id == gObject2.Id);
        //_energyNetwork.RemoveAt(index2);

        Sorting();
    }

    private void Sorting()
    {
        for (int i1 = 0; i1 < _energyNetwork.Count; i1++)
        {
            _energyNetwork[i1].Network = _energyNetwork[i1].Network.Distinct().ToList();

            for (int i2 = 0; i2 < _energyNetwork[i1].Network.Count; i2++)
            {
                for (int i3 = 0; i3 < _energyNetwork.Count; i3++)
                {
                    for (int i4 = 0; i4 < _energyNetwork[i3].Network.Count; i4++)
                    {   
                        if (_energyNetwork[i1].Network[i2] == _energyNetwork[i3].Network[i4])
                        {
                            _energyNetwork[i3].Network.RemoveAt(i4);
                            if (_energyNetwork[i3].Network.Count == 0)
                                _energyNetwork.RemoveAt(i3);
                        }


                        break;
                    }
                }
            }
        }
    }
}
