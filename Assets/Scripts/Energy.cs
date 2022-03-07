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
    public int MaxItemConnect = 0;
    public List<LineController> ItemConnect;


    private void Start()
    {
        StartCoroutine(Discharging(gameObject.GetComponent<Energy>()));
    }

    public void TransferEnergy(Energy StartObject, Energy EndObject)
    {
        if (StartObject.CurEnergy > 0)
        {
            if (StartObject.Conductivity >= EndObject.Conductivity)
            {
                int difference = StartObject.Conductivity - EndObject.Conductivity;
                StartObject.CurEnergy -= StartObject.Conductivity - difference;
                EndObject.CurEnergy += StartObject.Conductivity - difference;
            }

            else if (StartObject.Conductivity <= EndObject.Conductivity)
            {
                int difference = StartObject.Conductivity - EndObject.Conductivity;
                StartObject.CurEnergy -= StartObject.Conductivity + difference;
                EndObject.CurEnergy += StartObject.Conductivity + difference;
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
}
