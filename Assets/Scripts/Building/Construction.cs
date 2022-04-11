using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    private bool canStartConstruction;

    private ShopBuildingDATA buildingData;

    [Header("Hologram color")]
    [SerializeField] private Color _obstacleColor;
    [SerializeField] private Color _noObstacleColor;

    [Header("Socket mesh")]
    [SerializeField] private GameObject _socketMesh;
    [SerializeField] private Transform _socketTransform;

    private Transform _transform;
    private BoxCollider2D _boxCollider;

    private MeshFilter _socketMeshFilter;
    private MeshRenderer _socketMeshRenderer;
    

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _socketMeshFilter = _socketMesh.GetComponent<MeshFilter>();
        _socketMeshRenderer = _socketMesh.GetComponent<MeshRenderer>();
        
    } 

    public void InitializeHologram(ShopBuildingDATA buildingData)
    {
        ComponentsSettings(buildingData.PrefubBuilding);

        this.buildingData = buildingData;

        StopAllCoroutines();
        StartCoroutine(CollissionDetect());

    }

    private IEnumerator ProcessConstruction()
    {
        StopCoroutine(CollissionDetect());

        float timerConstruction = buildingData.TimeConstruction;

        while (true)
        {
            timerConstruction -= Time.deltaTime;
            if (timerConstruction <= 0.0f)
            {
                Instantiate(buildingData.PrefubBuilding, _transform.position, Quaternion.identity);
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator CollissionDetect()
    {
        Collider2D[] _hitCollider = new Collider2D[2];
        while (true)
        {
            Vector3 boxColliderPosition = (Vector2)_transform.position + _boxCollider.size;
            if (Physics2D.OverlapBoxNonAlloc(boxColliderPosition, _boxCollider.size / 2, 0, _hitCollider) >= 2)
            {
                SetColorHologram(_obstacleColor);
                canStartConstruction = false;
            }
            else
            {
                SetColorHologram(_noObstacleColor);
                canStartConstruction = true;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public bool AcceptBuying()
    {
        if (canStartConstruction)
        {
            StartCoroutine(ProcessConstruction());
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetColorHologram(Color color)
    {
        _socketMeshRenderer.material.color = color;
    }


    private void ComponentsSettings(GameObject prefubBuilding)
    {
        _boxCollider.size = prefubBuilding.GetComponent<BoxCollider2D>().size;
        _boxCollider.size = prefubBuilding.GetComponent<BoxCollider2D>().size;

        GameObject buildingObject = prefubBuilding.GetComponentInChildren<MeshFilter>().gameObject;

        _socketMeshFilter.sharedMesh = buildingObject.GetComponent<MeshFilter>().sharedMesh;

        Transform _transformBuilding = buildingObject.GetComponent<Transform>();
        _socketTransform.localPosition = _transformBuilding.localPosition;
        _socketTransform.localRotation = _transformBuilding.localRotation;
        _socketTransform.localScale = _transformBuilding.localScale;
    }
}
