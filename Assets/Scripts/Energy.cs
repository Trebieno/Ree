using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [Header("Энергия")]
    public int MinEnergyToActive = 0; //Минимальное количество энергии для работы
    public int CurEnergy = 0;
    public int MaxEnergy = 0;

    public bool Charg = false; //Зарядка
    //public bool LowEnergy;

    [Header("Настройки объекта")]
    [Range(1, 1000)] public int Conductivity = 0; //Электропроводимость
    public int Voltage = 0;   //Напряжение
    public int MaxVoltage = 0;   //Максимальное Напряжение
    public int Discharge = 0;   //Саморазряд

    [Header("Подключения")]
    public List<Transform> connection; // Подключённые к этому объекту объекты
    public List<LineController> ItemConnect;
    public int MaxItemConnect = 0;


    private void Start()
    {
        StartCoroutine(Discharging(gameObject.GetComponent<Energy>()));
    }

    public void TransferEnergy(Energy StartObject, Energy EndObject)
    {
        if (StartObject.CurEnergy > 0)
        {
            int addition = StartObject.SearchOfConnection(StartObject.connection).Count;
            if (StartObject.Conductivity >= EndObject.Conductivity)
            {
                int difference = StartObject.Conductivity - EndObject.Conductivity;
                StartObject.CurEnergy -= StartObject.Conductivity - difference;
                EndObject.CurEnergy += StartObject.Conductivity - difference + addition; 
            }

            else if (StartObject.Conductivity <= EndObject.Conductivity)
            {
                int difference = StartObject.Conductivity - EndObject.Conductivity;
                StartObject.CurEnergy -= StartObject.Conductivity + difference;
                EndObject.CurEnergy += StartObject.Conductivity + difference + addition;
            }
        }
    }
    //Возвращает время работы аккумулятора
    public int TimeToDischargeInSecond(int curEnergy, int voltage)
    {
        //Время работы аккумулятора = (10 * емкость батареи) / напряжение   --- (В ЧАСАХ)
        return (10 * curEnergy) / voltage * 60 * 60;
    }

    //Саморазрадка со временем
    public IEnumerator Discharging(Energy gameObject)
    {
        yield return new WaitForSeconds(1f);
        gameObject.CurEnergy -= Discharge;
        StartCoroutine(Discharging(gameObject));
    }
    
    //Зарядка объекта
    public IEnumerator Charging(Energy lowEnergy, Energy accumulator)
    {
        yield return new WaitForSeconds(1f);
        
        if (CurEnergy < MinEnergyToActive && CurEnergy < MaxEnergy)
        {
            TransferEnergy(accumulator, lowEnergy);
            StartCoroutine(Charging(lowEnergy, accumulator));
        }
    }

    public void StopCharging()
    {
        StopAllCoroutines();
    }
    
    public List<Transform> SearchOfConnection(List<Transform> connection)
    {
        if (1 < connection.Count)
        {
            
            foreach (Transform i1 in connection)
            {
                foreach (Transform i2 in this.connection)
                {
                    bool same = false;
                    if (i1 == i2) same = true;
                    if (!same)
                    {
                        //this.connection.Add(i1);
                        SearchOfConnection(this.connection);
                    }
                }
            }
        }
        else if(0 < connection.Count)
        {
            //this.connection.Add(connection[0]);
            SearchOfConnection(this.connection);
        }
        return this.connection;
    }

    // Взаимодействие с подключённым проводом

    public void Connection(Transform point)
    {
        gameObject.GetComponent<Energy>().connection.Add(point);
    }

    public void Disconnection(Transform point)
    {
        gameObject.GetComponent<Energy>().connection.Remove(point);
    }

    public List<Transform> ReturnConnectCable()
    {
        return connection;
    }
}
