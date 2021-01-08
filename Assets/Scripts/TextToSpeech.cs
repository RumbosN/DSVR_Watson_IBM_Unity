using System;
using System.Collections;
using System.Collections.Generic;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.TextToSpeech.V1;
using UnityEngine;

public class TextToSpeech : Singleton<TextToSpeech>
{
    [SerializeField] private string apiKey;
    [SerializeField] private string serviceUrl;
    public IBM_voices voice = IBM_voices.US_MichaelV3Voice;
    
    private TextToSpeechService _textToSpeechService;
    private IamAuthenticator _textToSpeechAuthenticator;
    
    //Keep track of when the processing of text to speech is complete.
    private EProcessingStatus _audioStatus;
    private AudioSource _outputAudioSource;

    // A queue for storing the entered texts for conversion to speech audio files
    private Queue<string> textQueue = new Queue<string>();
    // A queue for storing the speech AudioClips for playing
    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    
    void Start()
    {
        _audioStatus = EProcessingStatus.Idle;
        LogSystem.InstallDefaultReactors(); // Watson turn on loggin

        Runnable.Run(CreateAuthenticateServices());
        _outputAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // If no AudioClip is playing, convert the next text phrase to
        // audio audio if there is any left in the text queue.
        // The new audio clip is placed into the audio queue.
        if (textQueue.Count > 0 && _audioStatus == EProcessingStatus.Idle)
        {
            Debug.Log("Run ProcessText");                
            Runnable.Run(ProcessText());
        }

        // If no AudioClip is playing, remove the next clip from the
        // queue and play it.
        if (audioQueue.Count > 0 && !_outputAudioSource.isPlaying)
        {
            PlayClip(audioQueue.Dequeue());
        }
    }

    private IEnumerator CreateAuthenticateServices()
    {
        _textToSpeechAuthenticator = new IamAuthenticator(apiKey);

        while (!_textToSpeechAuthenticator.CanAuthenticate())
        {
            yield return null;
        }
        
        _textToSpeechService = new TextToSpeechService(_textToSpeechAuthenticator);
        
        if (!string.IsNullOrEmpty(serviceUrl))
        {
            _textToSpeechService.SetServiceUrl(serviceUrl);
        }
        else
        {
            Debug.LogError("Speech to text Service URL is null or empty");
        }
    }
    
    public void AddTextToQueue(string text)
    {
        Debug.Log("AddTextToQueue: " + text);
        if (!string.IsNullOrEmpty(text))
        {
            textQueue.Enqueue(text);
        }
    }

    private IEnumerator ProcessText()
    {
        Debug.Log("ProcessText");
        _audioStatus = EProcessingStatus.Processing;
        
        if (_outputAudioSource.isPlaying)
        {
            yield return null;
        }
        
        string nextText = String.Empty;
        if (textQueue.Count < 1)
        {
            yield return null;
        }
        else
        {
            nextText = textQueue.Dequeue();
            Debug.Log(nextText);

            if (String.IsNullOrEmpty(nextText))
            {
                yield return null;
            }
        }
        
        // The method accepts a maximum of 5 KB of input text in the body of the request, and 8 KB for the URL and headers
        // Doc: https://cloud.ibm.com/apidocs/text-to-speech?code=unity#synthesize
        byte[] synthesizeResponse = null;
        AudioClip clip = null;
        _textToSpeechService.Synthesize(
            callback: (DetailedResponse<byte[]> response, IBMError error) =>
            {
                synthesizeResponse = response.Result;
                clip = WaveFile.ParseWAV("message.wav", synthesizeResponse);
                
                //Place the new clip into the audio queue.
                audioQueue.Enqueue(clip);
            },
            text: nextText,
            voice: $"en-{voice}",
            accept: "audio/wav"
        );

        while (synthesizeResponse == null)
        {
            yield return null;
        }
        
        // Set status to indicate text to speech processing task completed
        _audioStatus = EProcessingStatus.Idle;
    }
    
    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            _outputAudioSource.spatialBlend = 0.0f;
            _outputAudioSource.loop = false;
            _outputAudioSource.clip = clip;
            _outputAudioSource.Play();
        }
    }
}
