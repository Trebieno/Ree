using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pillar : Structure
{
    [Header("Pillar")]
    [SerializeField] private GameObject _prefubCable;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private List<LineController> _cables;
    [SerializeField] private List<GameObject> _nearestObjects;
    [SerializeField] private GameObject _allObjects;

    private Energy energy;


    private void Start()
    {
        _allObjects = GameObject.FindGameObjectWithTag("GameWorld");
        energy = GetComponent<Energy>();
    }

    private void FixedUpdate()
    {
        _nearestObjects = _allObjects.GetComponent<PlayerActions>().ConnectingToDataBase(gameObject);

        if (_nearestObjects.Count > 0)
        {
            foreach (GameObject item in _nearestObjects)
            {
                ConnectionCable(item.transform);
                
            }
        }
        
        if (_points.Count > 0)
        {
            foreach (Transform item in _points.ToList())
            {
                if (Vector3.Distance(gameObject.transform.position, item.position) > 8f)
                {
                    DisconnectionCable(item.transform);
                }
            }
        }
    }


    private void ConnectionCable(Transform point)
    {
        Energy energy = point.GetComponent<Energy>();
        if (energy == null || energy.ItemConnect.Count >= energy.MaxItemConnect || _cables.Count >= energy.MaxItemConnect)
        {
            return;
        }
        foreach (LineController item in _cables) {  if (item.point == point) return;  }
        foreach (Transform item in this.energy.connection)  {  if (item == point) return;        }

        if (point != gameObject.transform)
        {
            Debug.Log("Подключаю кабель");
            LineController cable = Instantiate(_prefubCable, transform.position, transform.rotation).GetComponent<LineController>();
            energy.ItemConnect.Add(cable);

            cable.point = point;
            cable.pillar = gameObject.transform;
            _cables.Add(cable);
            _points.Add(point);

            cable.Connection();
            if (energy != null) energy.Connection(gameObject.transform);

            StartCoroutine(energy.Charging(energy, this.energy));
        }
    }

    private void DisconnectionCable(Transform point)
    {
        Energy energy = point.GetComponent<Energy>();
        foreach (LineController item in _cables)
        {
            if (item.point == point)
            {
                point.GetComponent<Energy>().ItemConnect.Remove(item);
                int index = _points.IndexOf(point);
                _cables[index].DestroyLine();
                _points.RemoveAt(index);
                _cables.RemoveAt(index);

                if (energy != null) energy.Disconnection(gameObject.transform);

                energy.StopCharging();

                return;
            }
        }
    }
}
