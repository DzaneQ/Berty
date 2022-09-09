using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject GuideSheet;
    [SerializeField] private GameObject MenuList;
    [SerializeField] private GameObject ExitButton;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ExitButton.SetActive(true);
        }
    }

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

    public void ExitGame()
    {
        Application.Quit();
    }
}
