public class AssistantWithText : WatsonAssistant
{    
    private TextToSpeech _textToSpeech;

    void Awake()
    {
        _textToSpeech = TextToSpeech.instance;
    }

    protected override void SendResponse(string text)
    {
        targetResponse.text = text;
        _textToSpeech.AddTextToQueue(text);
    }
}
