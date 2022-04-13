using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class DataBaseNetwork
{
    public int Id;
    public List<Transform> Network = new List<Transform>();
}

public class PlayerActions : MonoBehaviour
{

    [SerializeField] private List<GameObject> _otherObjects;
    [SerializeField] private List<GameObject> _nearestObjects;
    
    [SerializeField] private List<DataBaseNetwork> _energyNetwork;



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


    public void EnergyNetworkAnalysis(Energy gObject)
    {
        if (gObject.ItemConnect.Count == 0 && gObject.Connects.Count == 0)
        {
            int index = 0;
            while (index < _energyNetwork.Count)
            {
                if (_energyNetwork.Any(item => item.Id == gObject.Id))
                    gObject.Id++;
                else return;
                index++;
            }
            DataBaseNetwork data = new DataBaseNetwork();
            data.Id = gObject.Id;
            data.Network.Add(gObject.GetComponent<Transform>());

            _energyNetwork.Add(data);
        }

        if (gObject.ItemConnect.Count > 0)
        {
            gObject.Id = gObject.ItemConnect[0].point.GetComponent<Energy>().Id;

            DataBaseNetwork data = new DataBaseNetwork();

            if (_energyNetwork.Any(item => item.Id == gObject.Id))
            {
                data = _energyNetwork.Find(item => item.Id == gObject.Id);
                int index = _energyNetwork.IndexOf(data);
                _energyNetwork[index].Network.Add(gObject.GetComponent<Transform>());
                _energyNetwork[index].Network = _energyNetwork[index].Network.Distinct().ToList();
            }
        }

        else if (gObject.Connects.Count > 0)
        {
            gObject.Id = gObject.Connects[0].GetComponent<Energy>().Id;

            DataBaseNetwork data = new DataBaseNetwork();

            if (_energyNetwork.Any(item => item.Id == gObject.Id))
            {
                data = _energyNetwork.Find(item => item.Id == gObject.Id);
                int index = _energyNetwork.IndexOf(data);
                _energyNetwork[index].Network.Add(gObject.GetComponent<Transform>());
                _energyNetwork[index].Network = _energyNetwork[index].Network.Distinct().ToList();
            }
        }

        for (int i1 = 0; i1 < _energyNetwork.Count; i1++)
        {
            for (int i2 = 0; i2 < _energyNetwork[i1].Network.Count; i2++)
            {
                for (int i3 = 0; i3 < _energyNetwork.Count; i3++)
                {
                    for (int i4 = 0; i4 < _energyNetwork[i3].Network.Count; i4++)
                    {

                        if (_energyNetwork[i1] != _energyNetwork[i3])
                        {
                            if (_energyNetwork[i1].Network[i2] == _energyNetwork[i3].Network[i4])
                            {
                                _energyNetwork.RemoveAt(i3);
                            }
                        }
                    }
                }
            }
        }


    }
}
