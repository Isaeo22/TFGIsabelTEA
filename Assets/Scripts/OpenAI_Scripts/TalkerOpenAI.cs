using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkerOpenAI : MonoBehaviour
{

    public bool hasAnswered;
    public string rol;
    [SerializeField] public TMP_Text message;
    [SerializeField] public TMP_Text rolText;
    public string firstMessage;
    private OpenAIApi openai;
    public List<ChatMessage> messages = new List<ChatMessage>();
    public string prompt;

    [HideInInspector]
    public string openAIMessage;

    public event Action<string> OnNpcResponse;

    private void Start()
    {
        openai = new OpenAIApi(ConfigLoader.Instance.ApiKey, ConfigLoader.Instance.OrganizationId);
    }
    public void StartTalker()
    {
        message.text = firstMessage;
        ChatMessage newMessage = new ChatMessage();
        rolText.text = rol;
      
        if (messages.Count == 0)
        {
            newMessage.Role = "user";
            newMessage.Content = prompt + "\n";

            var newAssistantMessage = new ChatMessage();

            newAssistantMessage.Role = "assistant";
            newAssistantMessage.Content = firstMessage;
            messages.Add(newMessage);
            messages.Add(newAssistantMessage);
        }
        
    }

    public async void AskChatGPT(string newText)
    {
      
        ChatMessage newMessage = new ChatMessage();

        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        //request.Model = "gpt-3.5-turbo";
        request.Model = "gpt-4o";
        var response = await openai.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            message.text =  chatResponse.Content;          
            openAIMessage = chatResponse.Content;
            OnNpcResponse?.Invoke(openAIMessage);


        }
    }
}
