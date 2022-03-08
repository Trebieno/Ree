using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefubUnit;

    [SerializeField] private int _curHealth;
    [SerializeField] private int _maxHealth;

    [SerializeField] private bool _making = false;
    [SerializeField] private bool _spawningUnits;

    private void Start()
    {
        if (_making && _spawningUnits) StartCoroutine(SpawnUnit());
    }

    private int index = 0;
    protected IEnumerator SpawnUnit()
    {
        yield return new WaitForSeconds(1.0f);
        index++;

        if (index > 3 && _making)
        {
            Instantiate(_prefubUnit, transform.position, transform.rotation);
            index = 0;
        }
        StartCoroutine(SpawnUnit());
    }

    protected virtual void SetState()
    {
        _making = !_making;
    }
}
