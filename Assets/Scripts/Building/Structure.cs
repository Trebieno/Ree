using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [Header("Description")]
    public string Name;
    public GameObject PrefubUnit;

    public int CurHealth;
    public int MaxHealth;

    public bool Making = false;
    public bool SpawningUnits;

    private void Start()
    {
        if (Making && SpawningUnits) StartCoroutine(SpawnUnit());
    }

    private int index = 0;
    protected IEnumerator SpawnUnit()
    {
        yield return new WaitForSeconds(1.0f);
        index++;

        if (index > 3 && Making)
        {
            Instantiate(PrefubUnit, transform.position, transform.rotation);
            index = 0;
        }
        StartCoroutine(SpawnUnit());
    }

    protected virtual void SetState()
    {
        Making = !Making;
    }
}
