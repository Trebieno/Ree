using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorCommand { blue, red }
public class UnitRTS : MonoBehaviour
{
    [Header("Description")]
    public string Name = "None";
    public ColorCommand ColorCommand;
    public int MaxHealth;
    public int CurHealth;
    public float Speed;

    private GameObject selectedGameObject;
    private IMovePosition movePosition;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<MovePositionDirect>();
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        movePosition.SetMovePosition(targetPosition);
    }
}
