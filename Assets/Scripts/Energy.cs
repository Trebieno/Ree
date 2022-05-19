using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [Header("Энергия")]
    public int MinEnergyToActive = 0; // Минимальное количество энергии для работы 
    public int CurEnergy = 0;
    public int MaxEnergy = 0;

    public bool Charg = false; //Зарядка

    [Header("Настройки объекта")]
    [Range(1, 1000)] public int Conductivity = 0; //Электропроводимость-прирост
    public int Voltage = 0;   //Напряжение
    public int MaxVoltage = 0;   //Максимальное Напряжение
    public int Discharge = 0;   //Саморазряд

    [Header("Подключения")]
    public List<Transform> ItemConnect;
    public int MaxCableConnect = 0; //Максимальное кол-во подкл. к другим объектам 
    public int Id = -1;
    public bool Connect = false; // Может ли объект к что-то подключать к себе создавая новую сеть
    

    private void Start()
    { 
        //StartCoroutine(Discharging(gameObject.GetComponent<Energy>()));
    }

    private void FixedUpdate()
    {
        
    }

    public void TransferEnergy(Energy StartObject, Energy EndObject = null)
    {
        if (StartObject.CurEnergy > 0)
        {
            if (StartObject.Conductivity >= EndObject.Conductivity)
            {
                StartObject.CurEnergy -= EndObject.Conductivity;
                EndObject.CurEnergy += EndObject.Conductivity; 
            }

            else if (StartObject.Conductivity <= EndObject.Conductivity)
            {
                StartObject.CurEnergy -= StartObject.Conductivity;
                EndObject.CurEnergy += StartObject.Conductivity;
            }

            if (EndObject.CurEnergy > EndObject.MaxEnergy) 
            { 
                EndObject.CurEnergy = EndObject.MaxEnergy; 
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
    //public IEnumerator Charging(Energy lowEnergy, Pillar accumulator)
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (accumulator._.Find(line => line.point == lowEnergy.GetComponent<Transform>()) && CurEnergy < MaxEnergy)
    //    {
    //        TransferEnergy(accumulator, lowEnergy);
    //        StartCoroutine(Charging(lowEnergy, accumulator));
    //    }
    //}

    public void StopCharging()
    {
        StopAllCoroutines();
    }
}
