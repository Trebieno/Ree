using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilding : MonoBehaviour
{
    public bool StatusBuy { get; private set; }
    [SerializeField] private ShopBuildingDATA[] buildingData;

    [Header("Hologram settings")]
    [SerializeField] private GameObject prefabHologram;
    [SerializeField] private LayerMask maskMoveHologram;

    private int indexBuyBuilding;
    private Construction hologramBuilding;
    private Transform transformHologram;

    private Camera m_Camera;

    private void Start()
    {
        m_Camera = Camera.main;
    }

    public void BuyBuilding(int indexInShop)
    {
        if (Static.money > buildingData[indexInShop].Cost)
        {
            if (!StatusBuy)
            {
                indexBuyBuilding = indexInShop;
                hologramBuilding = Instantiate(prefabHologram, Vector3.zero, Quaternion.identity).GetComponent<Construction>();
                hologramBuilding.InitializeHologram(buildingData[indexBuyBuilding]);

                transformHologram = hologramBuilding.transform;
                StatusBuy = true;
            }
        }
    }

    public void MoveHologram()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 50.0f, maskMoveHologram))
        {
            transformHologram.position = new Vector3(hitInfo.point.x, hitInfo.point.y, -0.1f);
        }
    }

    public void PurchaseBuilding()
    {
        StartCoroutine(AcceptBuying());
    }

    private IEnumerator AcceptBuying()
    {
        yield return new WaitForFixedUpdate();

        if (hologramBuilding.AcceptBuying())
        {
            Static.money -= buildingData[indexBuyBuilding].Cost;
            StatusBuy = false;
        }
    }

    public void CancelBuy()
    {
        StatusBuy = false;
        Destroy(hologramBuilding.gameObject);
    }
}
