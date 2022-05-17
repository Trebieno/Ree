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

    [SerializeField] private GameObject _allObjects;
    [SerializeField] private Energy energy;
    private void Start()
    {
        energy = gameObject.GetComponent<Energy>();
        _allObjects = GameObject.FindGameObjectWithTag("GameWorld");
        _allObjects.GetComponent<PlayerActions>().EnergyNetworkAnalysis(energy, energy);
    }

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
