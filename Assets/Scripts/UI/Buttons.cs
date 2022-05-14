using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    // Functions for buttons //
    public void PlayB() 
    {
        Debug.Log("Button play clicked!!!");
        
        
        panel1.SetActive(false);
        panel1.SetActive(true);


    }
    public void NetworkB() 
    {
        Debug.Log("Button network clicked!!!");
        // Start anim
    }
    public void SettingsB() 
    {
        Debug.Log("Button settings clicked!!!");
        // Start anim
    }
    public void AvtorsB() 
    {
        Debug.Log("Button avtors clicked!!!");
        // Start anim
    }
    public void ExitB() 
    {
        // Exit
    }
    public void NewPlayB() 
    {
        
    }
    public void DownloadGameB() 
    {
        
    }
    public void BackB() 
    {
        panel1.SetActive(true);
        panel1.SetActive(false);
    }
}
