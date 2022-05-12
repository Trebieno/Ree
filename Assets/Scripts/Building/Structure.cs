using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefubObject;

    [SerializeField] protected int _curHealth;
    [SerializeField] protected int _maxHealth;

    [SerializeField] protected bool _making = false;
    [SerializeField] protected bool _spawningObjects;

    [SerializeField] protected List<GameObject> _nearestObjects;

    private void Start()
    {
        
        if (_making && _spawningObjects) StartCoroutine(SpawnUnit());
    }

    private int index = 0;
    protected IEnumerator SpawnUnit()
    {
        yield return new WaitForSeconds(1.0f);
        index++;

        if (index > 3 && _making)
        {
            Instantiate(_prefubObject, transform.position, transform.rotation);
            index = 0;
        }
        StartCoroutine(SpawnUnit());
    }

    protected virtual void SetState()
    {
        _making = !_making;
    }
}
