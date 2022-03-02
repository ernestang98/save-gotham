using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutionManager : MonoBehaviour
{
    [Range(0.0f, 3.0f)]
    public float pollutionIndex;
    private float maxPollutionIndex = 3.0f;

    public Image cameraPollutionFilter;

    public RainScript rainController;

    public List<GameObject> smokeObjects;

    // Start is called before the first frame update
    void Start()
    {
        rainController = GameObject.Find("Rain").GetComponent<RainScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pointsToColorDensity();
        pointsToRainIntensity();
        pointsToSmokeDensity();
    }

    void pointsToColorDensity()
    {
        float maxAlphaValue = 0.5f;
        float alphaValue = pollutionIndex / maxPollutionIndex * maxAlphaValue;
        cameraPollutionFilter.color = new Color(cameraPollutionFilter.color.r, cameraPollutionFilter.color.g, cameraPollutionFilter.color.b, alphaValue);

        /*if (pollutionIndex >= 0f && pollutionIndex < 1f)
        {
            cameraPollutionFilter.color = new Color(cameraPollutionFilter.color.r, cameraPollutionFilter.color.g, cameraPollutionFilter.color.b, 0f);
        }
        else if (pollutionIndex >= 1f && pollutionIndex < 2f)
        {
            cameraPollutionFilter.color = new Color(cameraPollutionFilter.color.r, cameraPollutionFilter.color.g, cameraPollutionFilter.color.b, 0.25f);
        }
        else
        {
            cameraPollutionFilter.color = new Color(cameraPollutionFilter.color.r, cameraPollutionFilter.color.g, cameraPollutionFilter.color.b, 0.5f);
        }*/
    }

    void pointsToRainIntensity()
    {
        float maxRainIntensity = 0.4f;
        float rainIntensity = pollutionIndex / maxPollutionIndex * maxRainIntensity;
        if (rainIntensity < 0.05f)
            rainIntensity = 0f;
        rainController.RainIntensity = rainIntensity;

        /*if (pollutionIndex >= 0f && pollutionIndex < 1f)
        {
            rainController.RainIntensity = 0;
        }
        else if (pollutionIndex >= 1f && pollutionIndex < 2f)
        {
            rainController.RainIntensity = 0.15f;
        }
        else
        {
            rainController.RainIntensity = 0.3f;
        }*/
    }

    void pointsToSmokeDensity()
    {
        if (pollutionIndex >= 0f && pollutionIndex < 1f)
        {
            foreach (GameObject smoke in smokeObjects)
            {
                smoke.SetActive(false);
            }
        }
        else if (pollutionIndex >= 1f && pollutionIndex < 2f)
        {
            for (int i=0;i<2;i++)
            {
                smokeObjects[i].SetActive(true);
            }
        }
        else
        {
            foreach (GameObject smoke in smokeObjects)
            {
                smoke.SetActive(true);
            }
        }
    }
}
