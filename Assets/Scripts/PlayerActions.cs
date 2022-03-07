using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    [SerializeField] private List<GameObject> otherObjects;
    [SerializeField] private List<GameObject> nearestObjects;

    private ShopBuilding shopBuilding;

    private void Start()
    {
        shopBuilding = GetComponent<ShopBuilding>();
    }
    
    private void Update()
    {
        if (shopBuilding.StatusBuy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                shopBuilding.PurchaseBuilding();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                shopBuilding.CancelBuy();
            }

            shopBuilding.MoveHologram();
        }

        otherObjects = GameObject.FindGameObjectsWithTag("Unit").ToList();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Building").ToList())
        {
            otherObjects.Add(item);
        }
    }

    public List<GameObject> ConnectingToDataBase(GameObject Object)
    {
        //Debug.Log("Поделючаюсь к базе");
        nearestObjects = otherObjects.Where(x => Vector3.Distance(Object.transform.position, x.transform.position) < 8f).ToList();
        nearestObjects = nearestObjects.OrderBy(x => Vector3.Distance(Object.transform.position, x.transform.position)).ToList();

        return nearestObjects;
    }
}
