using System;
using UnityEngine;

public class RestaurantAssistant : WatsonAssistant
{
    private TextToSpeech _textToSpeech;
    public GameObject donut;
    public GameObject hamburger;
    public GameObject hamEgg;
    public GameObject iceCream;
    
    void Awake()
    {
        _textToSpeech = TextToSpeech.instance;
        donut.SetActive(false);
        hamburger.SetActive(false);
        hamEgg.SetActive(false);
        iceCream.SetActive(false);
    }

    protected override void SendResponse(string text)
    {
        targetResponse.text = text;
        _textToSpeech.AddTextToQueue(text);

        if (FoodConstants.DONUT.IsMatch(text))
        {
            donut.SetActive(true);
        } 
        else if (FoodConstants.HAMBURGER.IsMatch(text))
        {
            hamburger.SetActive(true);
        }
        else if (FoodConstants.HAM_EGG.IsMatch(text))
        {
            hamEgg.SetActive(true);
        }
        else if (FoodConstants.ICE_CREAM.IsMatch(text))
        {
            iceCream.SetActive(true);
        }
        
    }
}
