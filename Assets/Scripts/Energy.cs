using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [Header("�������")]
    public int MinEnergyToActive = 0; //����������� ���������� ������� ��� ������
    public int CurEnergy = 0;
    public int MaxEnergy = 0;

    public bool Charg = false; //�������
    //public bool LowEnergy;

    [Header("��������� �������")]
    [Range(1, 1000)] public int Conductivity = 0; //�������������������
    public int Voltage = 0;   //����������
    public int MaxVoltage = 0;   //������������ ����������
    public int Discharge = 0;   //����������

    [Header("�����������")]
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
    //���������� ����� ������ ������������
    public int TimeToDischargeInSecond(int curEnergy, int voltage)
    {
        //����� ������ ������������ = (10 * ������� �������) / ����������   --- (� �����)
        return (10 * curEnergy) / voltage * 60 * 60;
    }

    //������������ �� ��������
    public IEnumerator Discharging(Energy gameObject)
    {
        yield return new WaitForSeconds(1f);
        gameObject.CurEnergy -= Discharge;
        StartCoroutine(Discharging(gameObject));
    }
    
    //������� �������
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
