using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilding : MonoBehaviour
{
    public bool StatusBuy { get; private set; }
    [SerializeField] private ShopBuildingDATA[] _buildingData;

    [Header("Hologram settings")]
    [SerializeField] private GameObject _prefabHologram;
    [SerializeField] private LayerMask _maskMoveHologram;

    private int _indexBuyBuilding;
    private Construction _hologramBuilding;
    private Transform _transformHologram;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void BuyBuilding(int indexInShop)
    {
        if (Static.money > _buildingData[indexInShop].Cost)
        {
            if (!StatusBuy)
            {
                _indexBuyBuilding = indexInShop;
                _hologramBuilding = Instantiate(_prefabHologram, Vector3.zero, Quaternion.identity).GetComponent<Construction>();
                _hologramBuilding.InitializeHologram(_buildingData[_indexBuyBuilding]);

                _transformHologram = _hologramBuilding.transform;
                StatusBuy = true;
            }
        }
    }

    public void MoveHologram()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 50.0f, _maskMoveHologram))
        {
            _transformHologram.position = new Vector3(hitInfo.point.x, hitInfo.point.y, -0.1f);
        }
    }

    public void PurchaseBuilding()
    {
        StartCoroutine(AcceptBuying());
    }

    private IEnumerator AcceptBuying()
    {
        yield return new WaitForFixedUpdate();

        if (_hologramBuilding.AcceptBuying())
        {
            Static.money -= _buildingData[_indexBuyBuilding].Cost;
            StatusBuy = false;
        }
    }

    public void CancelBuy()
    {
        StatusBuy = false;
        Destroy(_hologramBuilding.gameObject);
    }
}
