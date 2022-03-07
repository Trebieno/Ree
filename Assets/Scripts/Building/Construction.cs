using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    private bool canStartConstruction;

    private ShopBuildingDATA buildingData;

    [Header("Hologram color")]
    [SerializeField] private Color obstacleColor;
    [SerializeField] private Color noObstacleColor;

    [Header("Socket mesh")]
    [SerializeField] private GameObject socketMesh;
    [SerializeField] private Transform socketTransform;

    private Transform m_Transform;
    private BoxCollider2D m_BoxCollider;

    private MeshFilter socketMeshFilter;
    private MeshRenderer socketMeshRenderer;
    

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_BoxCollider = GetComponent<BoxCollider2D>();

        socketMeshFilter = socketMesh.GetComponent<MeshFilter>();
        socketMeshRenderer = socketMesh.GetComponent<MeshRenderer>();
        
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
                Instantiate(buildingData.PrefubBuilding, m_Transform.position, Quaternion.identity);
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
            Vector3 boxColliderPosition = (Vector2)m_Transform.position + m_BoxCollider.offset;
            if (Physics2D.OverlapBoxNonAlloc(boxColliderPosition, m_BoxCollider.size / 2, 0, _hitCollider) >= 2)
            {
                SetColorHologram(obstacleColor);
                canStartConstruction = false;
            }
            else
            {
                SetColorHologram(noObstacleColor);
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
        socketMeshRenderer.material.color = color;
    }


    private void ComponentsSettings(GameObject prefubBuilding)
    {
        m_BoxCollider.size = prefubBuilding.GetComponent<BoxCollider2D>().size;
        m_BoxCollider.offset = prefubBuilding.GetComponent<BoxCollider2D>().offset;

        GameObject buildingObject = prefubBuilding.GetComponentInChildren<MeshFilter>().gameObject;

        socketMeshFilter.sharedMesh = buildingObject.GetComponent<MeshFilter>().sharedMesh;

        Transform _transformBuilding = buildingObject.GetComponent<Transform>();
        socketTransform.localPosition = _transformBuilding.localPosition;
        socketTransform.localRotation = _transformBuilding.localRotation;
        socketTransform.localScale = _transformBuilding.localScale;
    }
}
