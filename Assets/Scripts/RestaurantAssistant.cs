using System;
using UnityEngine;

public class RestaurantAssistant : WatsonAssistant
{
    public GameObject donut;
    public GameObject hamburger;
    public GameObject hamEgg;
    public GameObject iceCream;
    public GameObject background;

    private TextToSpeech _textToSpeech;
    private AudioSource _audioSuccess;

    void Awake()
    {
        _textToSpeech = TextToSpeech.instance;
        _audioSuccess = GetComponent<AudioSource>();
        donut.SetActive(false);
        hamburger.SetActive(false);
        hamEgg.SetActive(false);
        iceCream.SetActive(false);
        background.SetActive(false);
    }

    protected override void SendResponse(string text)
    {
        targetResponse.text = text;
        _textToSpeech.AddTextToQueue(text);

        if (FoodConstants.DONUT.IsMatch(text))
        {
            ActiveObject(donut);
        } 
        else if (FoodConstants.HAMBURGER.IsMatch(text))
        {
            ActiveObject(hamburger);
        }
        else if (FoodConstants.HAM_EGG.IsMatch(text))
        {
            ActiveObject(hamEgg);
        }
        else if (FoodConstants.ICE_CREAM.IsMatch(text))
        {
            ActiveObject(iceCream);
        }
    }
    
    private void ActiveObject(GameObject obj)
    {
        obj.SetActive(true);
        background.SetActive(true);
        _audioSuccess.Play();
    }
}
