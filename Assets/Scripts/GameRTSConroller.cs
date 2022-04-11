using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameRTSConroller : MonoBehaviour
{
    
    [SerializeField] private LayerMask _maskMoveCursor;
    [SerializeField] private Transform _selectionAreaTransform;

    private Vector3 _startPosition;
    private List<UnitRTS> _selectedUnitRTSList;

    private Camera _camera;

    private void Awake()
    {
        _selectedUnitRTSList = new List<UnitRTS>();
        _selectionAreaTransform.gameObject.SetActive(false);
    }
    
    
    private void Start()
    {
        _camera = Camera.main;
    }
    
    

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _maskMoveCursor))
        {
            Vector3 rayPos = new Vector3(hitInfo.point.x, hitInfo.point.y, -0.1f);
            
            if (Input.GetMouseButtonDown(0))
            {
                _selectionAreaTransform.gameObject.SetActive(true);
                _startPosition = rayPos;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 currentMousePosition = rayPos;
                Vector3 lowerLeft = new Vector3(Mathf.Min(_startPosition.x, currentMousePosition.x),
                                                Mathf.Min(_startPosition.y, currentMousePosition.y));

                Vector3 upperRight = new Vector3(Mathf.Max(_startPosition.x, currentMousePosition.x),
                                                Mathf.Max(_startPosition.y, currentMousePosition.y));
                _selectionAreaTransform.position = lowerLeft;
                _selectionAreaTransform.localScale = upperRight - lowerLeft;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectionAreaTransform.gameObject.SetActive(false);
                Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(_startPosition, rayPos);

                foreach (UnitRTS unitRTS in _selectedUnitRTSList)
                {
                    unitRTS.SetSelectedVisible(false);
                }
                _selectedUnitRTSList.Clear();

                foreach (Collider2D collider2D in collider2DArray)
                {
                    UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
                    if (unitRTS != null)
                    {
                        unitRTS.SetSelectedVisible(true);
                        _selectedUnitRTSList.Add(unitRTS);
                    }
                    //Debug.Log(selectedUnitRTSList.Count);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 moveToPosition = rayPos;


                List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 2f, 2*2, 2*3}, new int[] {5, 10, 20});

                int targetPositionListIndex = 0;

                foreach (UnitRTS unitRTS in _selectedUnitRTSList)
                {
                    unitRTS.MoveTo(targetPositionList[targetPositionListIndex]);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
            }
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDisntanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDisntanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDisntanceArray[i], ringPositionCountArray[i]));
        }

        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}
