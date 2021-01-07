public class SpeechToTextLabel : SpeechToText
{
    protected override void SendResponse(string text)
    {
        OutputText.text = text;
    }
}
