using UnityEngine;

public enum ColorCommand { blue, red }
public class UnitRTS : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private string _name = "None";
    [SerializeField] private ColorCommand _colorCommand;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _curHealth;
    [SerializeField] private float _speed;

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
