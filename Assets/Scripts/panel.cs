using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel : MonoBehaviour
{
    private Canvas canvas = null;
    private MenuManager menuManager = null;
    // Start is called before the first frame update

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Setup(MenuManager menuManager)
    {
        this.menuManager = menuManager;
        Hide();
    }

    public void Show()
    {
        //Time.timeScale(0);
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
}
