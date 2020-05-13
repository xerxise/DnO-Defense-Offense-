using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_baseUi;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!GameManager.isPause)
            CallMenu();
            else
            CloseMenu();
        }
        
    }
    private void CallMenu()
    {
        GameManager.isPause = true;
        go_baseUi.SetActive(true);
        Time.timeScale = 0f;
    }
    private void CloseMenu()
    {
        GameManager.isPause =false;
        go_baseUi.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ClickSave()
    {
        Debug.Log("세이브");
    }
    public void ClickLoad()
    {
        Debug.Log("로어드");
    }
    public void ClickQuit()
    {
        Debug.Log("로그아웃");
        Application.Quit();
    }
}
