using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject GuideSheet;
    [SerializeField] private GameObject MenuList;

    public void Play()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void GuideOpen()
    {
        GuideSheet.SetActive(!GuideSheet.activeSelf);
        MenuList.SetActive(!MenuList.activeSelf);
    }

    public void GuideClose()
    {
        GuideSheet.SetActive(false);
        MenuList.SetActive(true);
    }
}
