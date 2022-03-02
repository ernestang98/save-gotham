using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemDescription : MonoBehaviour
{
    public GameObject leftItemDescriptionCanvas;
    public Text leftItemDescriptionText;
    public Text leftItemTriviaText;

    [Space]
    public GameObject rightItemDescriptionCanvas;
    public Text rightItemDescriptionText;
    public Text rightItemTriviaText;

    [Space]
    public XRGrabExtension xRGrabExtension;

    [Space]
    public string itemDescription;
    public string itemType;

    private readonly string cans = "Cans are recycled by only 38% of households in Singapore";
    private readonly string books = "Books are the most recycled items by Singapore households";
    private readonly string milkCartons = "Milk cartons should be rinsed, crushed, then recycled to Paper recycling bin, according to NEA";
    private readonly string plasticBottleCup = "50% of households in Singapore recycle plastic bottles and cups";
    private readonly string toiletPaper = "Toilet tissue paper, like all paper, is recyclable as long as it is free of contaminants such as foil and glitter";
    private readonly string plants = "Plants improve our air quality by filtering harmful dust and pollutants from the air we breathe";


    private readonly string leftInteractor = "Left Interactor";
    private readonly string rightInteractor = "Right Interactor";

    private bool leftPanelShowing;
    private bool rightPanelShowing;

    // Start is called before the first frame update
    void Start()
    {
        leftPanelShowing = false;
        rightPanelShowing = false;
        xRGrabExtension = GetComponent<XRGrabExtension>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showItemDescription()
    {
        if (!leftPanelShowing && !rightPanelShowing)
        {
            if (xRGrabExtension.hoveringInteractor.tag == leftInteractor)
            {
                leftItemDescriptionText.text = itemDescription;

                switch (itemType)
                {
                    case "Cans":
                        leftItemTriviaText.text = cans;
                        break;
                    case "Books":
                        leftItemTriviaText.text = books;
                        break;
                    case "Milk Cartons":
                        leftItemTriviaText.text = milkCartons;
                        break;
                    case "Plastic Bottle Cup":
                        leftItemTriviaText.text = plasticBottleCup;
                        break;
                    case "Toilet Paper":
                        leftItemTriviaText.text = toiletPaper;
                        break;
                    case "Plants":
                        leftItemTriviaText.text = plants;
                        break;
                }

                leftItemDescriptionCanvas.SetActive(true);
                leftPanelShowing = true;
            }
            else if (xRGrabExtension.hoveringInteractor.tag == rightInteractor)
            {
                rightItemDescriptionText.text = itemDescription;

                switch (itemType)
                {
                    case "Cans":
                        rightItemTriviaText.text = cans;
                        break;
                    case "Books":
                        rightItemTriviaText.text = books;
                        break;
                    case "Milk Cartons":
                        rightItemTriviaText.text = milkCartons;
                        break;
                    case "Plastic Bottle Cup":
                        rightItemTriviaText.text = plasticBottleCup;
                        break;
                    case "Toilet Paper":
                        rightItemTriviaText.text = toiletPaper;
                        break;
                    case "Plants":
                        rightItemTriviaText.text = plants;
                        break;
                }

                rightItemDescriptionCanvas.SetActive(true);
                rightPanelShowing = true;
            }
        }
    }

    public void hideItemDescription()
    {
        if (leftPanelShowing)
        {
            leftItemDescriptionText.text = "";
            leftItemTriviaText.text = "";
            leftItemDescriptionCanvas.SetActive(false);
            leftPanelShowing = false;
        }
        else if (rightPanelShowing)
        {
            rightItemDescriptionText.text = "";
            rightItemTriviaText.text = "";
            rightItemDescriptionCanvas.SetActive(false);
            rightPanelShowing = false;
        }
    }
}
