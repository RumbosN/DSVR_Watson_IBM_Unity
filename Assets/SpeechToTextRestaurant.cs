using IBM.Cloud.SDK.Utilities;

public class SpeechToTextRestaurant : SpeechToText
{
    private RestaurantAssistant _assistant;
    
    private void Awake()
    {
        _assistant = FindObjectOfType<RestaurantAssistant>();
    }

    protected override void SendResponse(string text)
    {
        if (_assistant != null)
        { 
            Runnable.Run(_assistant.ProcessChat(text));
        }

        if (OutputText != null)
        {
            OutputText.text = text;
        }
    }
}
