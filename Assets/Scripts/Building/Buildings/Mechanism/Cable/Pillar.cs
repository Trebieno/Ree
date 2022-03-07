using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pillar : Structure
{
    [Header("Pillar")]
    [SerializeField] private GameObject prefubCable;

    [SerializeField] private List<Transform> connection; // Подключённые к этому объекту объекты
    [SerializeField] private List<Transform> points;
    [SerializeField] private List<LineController> cables;

    [SerializeField] private List<GameObject> nearestObjects;
    [SerializeField] private GameObject allObjects;

    private Energy energy;


    private void Start()
    {
        allObjects = GameObject.FindGameObjectWithTag("GameWorld");
        energy = GetComponent<Energy>();
    }

    private void FixedUpdate()
    {
        nearestObjects = allObjects.GetComponent<PlayerActions>().ConnectingToDataBase(gameObject);

        if (nearestObjects.Count > 0)
        {
            foreach (GameObject item in nearestObjects)
            {
                ConnectionCable(item.transform);
                
            }
        }
        
        if (points.Count > 0)
        {
            foreach (Transform item in points.ToList())
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
        if (energy == null || energy.ItemConnect.Count >= energy.MaxItemConnect || cables.Count >= energy.MaxItemConnect)
        {
            return;
        }
        foreach (LineController item in cables) {  if (item.point == point) return;  }
        foreach (Transform item in connection)  {  if (item == point) return;        }

        if (point != gameObject.transform)
        {
            Debug.Log("Подключаю кабель");
            LineController cable = Instantiate(prefubCable, transform.position, transform.rotation).GetComponent<LineController>();
            energy.ItemConnect.Add(cable);

            cable.point = point;
            cable.pillar = gameObject.transform;
            cables.Add(cable);
            points.Add(point);

            cable.Connection();
            if (point.GetComponent<Pillar>() != null) point.GetComponent<Pillar>().Connection(gameObject.transform);

            StartCoroutine(energy.Charging(energy, this.energy));
        }
    }

    private void DisconnectionCable(Transform point)
    {
        Energy energy = point.GetComponent<Energy>();
        foreach (LineController item in cables)
        {
            if (item.point == point)
            {
                point.GetComponent<Energy>().ItemConnect.Remove(item);
                int index = points.IndexOf(point);
                cables[index].DestroyLine();
                points.RemoveAt(index);
                cables.RemoveAt(index);

                if (point.GetComponent<Pillar>() != null) point.GetComponent<Pillar>().Disconnection(gameObject.transform);

                energy.StopCharging();

                return;
            }
        }
    }


    // Взаимодействие с подключённым проводом

    public void Connection(Transform point)
    {
        connection.Add(point);
    }

    public void Disconnection(Transform point)
    {
        connection.Remove(point);
    }

    public List<Transform> ReturnConnectCable()
    {
        return connection;
    }
}
