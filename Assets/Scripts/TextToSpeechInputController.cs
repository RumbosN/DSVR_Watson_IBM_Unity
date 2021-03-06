﻿public class TextToSpeechInputController : InputFieldController
{
    private TextToSpeech _textToSpeech;
    
    void Awake()
    {
        _textToSpeech = TextToSpeech.instance;
    }
    
    protected override void SendMessage(string text)
    {
        _textToSpeech.AddTextToQueue(text);
    }
}
