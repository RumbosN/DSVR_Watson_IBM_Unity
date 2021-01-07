using System;
using System.Collections;
using System.Collections.Generic;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;
using UnityEngine;
using UnityEngine.UI;

public class WatsonAssistant : Singleton<WatsonAssistant>
{
    public Text targetResponse;
    
    [SerializeField] private string apiKey;
    [SerializeField] private string serviceUrl;
    [SerializeField] private string assistantId;
    [SerializeField] private string serviceVersionDate = "2020-04-13";
    
    private AssistantService _assistantService;
    private IamAuthenticator _assistantAuthenticator;
    private EProcessingStatus _assistantStatus;
    private bool _createSessionTested = false;
    private string _sessionId;

    
    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateAuthenticateServices());
        _assistantStatus = EProcessingStatus.Idle;
    }

    private IEnumerator CreateAuthenticateServices()
    {
        _assistantAuthenticator = new IamAuthenticator(apiKey);

        while (!_assistantAuthenticator.CanAuthenticate())
        {
            yield return null;
        }
        
        _assistantService = new AssistantService(serviceVersionDate, _assistantAuthenticator);
        
        if (!string.IsNullOrEmpty(serviceUrl))
        {
            _assistantService.SetServiceUrl(serviceUrl);
        }
        else
        {
            Debug.LogError("Assistant Service URL is null or empty");
        }
        
        _assistantService.CreateSession(OnCreateSession, assistantId);

        while (!_createSessionTested)
        {
            yield return null;
        }
    }
    
    private void OnCreateSession(DetailedResponse<SessionResponse> response, IBMError error)
    {
        Log.Debug("SimpleBot.OnCreateSession()", "Session: {0}", response.Result.SessionId);
        _sessionId = response.Result.SessionId;
        _createSessionTested = true;
    }

    public IEnumerator ProcessChat(string chatInput)
    {
        Debug.Log($"Processchat: {chatInput}");
        
        // Set status to show that the chat input is being processed.
        _assistantStatus = EProcessingStatus.Processing;
        
        if (String.IsNullOrEmpty(chatInput) || _assistantService == null)
        {
            yield return null;
        }
        
        MessageResponse messageResponse = null;
        _assistantService.Message(
            callback: OnMessage,
            assistantId: assistantId,
            sessionId: _sessionId,
            input: new MessageInput()
            {
                Text = chatInput,
                Options = new MessageInputOptions()
                {
                    ReturnContext = true
                }
            }
        );

        while (messageResponse == null)
        {
            yield return null;
        }
    }
    
    private void OnMessage(DetailedResponse<MessageResponse> response, IBMError error)
    {
        Debug.Log($"response = {response.Result}");
        
        string textResponse;
        if (response.Result == null || response.Result.Output == null || 
            response.Result.Output.Generic == null || response.Result.Output.Generic.Count < 1)
        {
            textResponse = "I don't know how to respond to that.";
        }
        else
        {
            textResponse = response.Result.Output.Generic[0].Text.ToString();
        }

        SendResponse(textResponse);
    }

    protected void SendResponse(string text)
    {
        targetResponse.text = text;
    }
}
