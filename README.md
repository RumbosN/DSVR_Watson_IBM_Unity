
# IBM Watson - Unity Restaurant

**Using:**
Unity 2020.1.17f1

Watson Unity SDK [5.0.1](https://github.com/watson-developer-cloud/unity-sdk/releases/tag/v5.0.1 "5.0.1")

Watson IBM SDK core [1.2.1](https://github.com/IBM/unity-sdk-core/releases/tag/v1.2.1 "1.2.1")

# Youtube

In [youtube](https://youtu.be/-oej2rPA8QE) I leave a playlist to explain a little of what the project is about, some key points of the code and a little of the cloud configuration

# Settings
Each component (assistant, speech to text and text to speech) has its own ApiKey and ServiceUrl Settings. Before run please set it to the scenes can run.

To Restaurant Scena can work well in the youtube video you can found my assistant credentials. Be careful if you're going to use it to other people can use it too.

## Assistant Id Settings
In the youtube video I didn't explain how get the assistant id. To get it:
1. Go to your assistant service.
2. Click in "Start Watson Assistant"
3. Before click in your assistant, click in the three dots menu and select "Settings"
4. Ready! just copy the assistant ID

## Useful Documentation
- [TextToSpeech API](https://cloud.ibm.com/apidocs/speech-to-text?code=unity#authentication)
- [SpeechToText API](https://cloud.ibm.com/apidocs/speech-to-text?code=unity#authentication)
- [Assistant API](https://cloud.ibm.com/apidocs/assistant/assistant-v2?code=unity#message)
- [Assistant Setup](https://cloud.ibm.com/docs/assistant?topic=assistant-tool-overview)