using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    [SerializeField] private List<GameObject> _otherObjects;
    [SerializeField] private List<GameObject> _nearestObjects;

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
}
