using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTManager : MonoBehaviour
{
    public static MQTTManager Instance { get; private set; }

    [Header("Configurações do Broker")]
    public string brokerAddress = "127.0.0.1";
    public int brokerPort = 1883;

    private MqttClient client;
    private readonly Queue<Action> executionQueue = new Queue<Action>();
    public event Action<string, string> OnMessageArrived;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        try
        {
            client = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);

            string clientId = "UnityClient_" + Guid.NewGuid().ToString().Substring(0, 5);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Connect(clientId);

            Debug.Log($"[MQTT] Conectado com sucesso ao Broker: {brokerAddress}");
            client.Subscribe(new string[] { "escola/sala/sensores" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MQTT] Falha ao conectar: {ex.Message}");
        }
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0) executionQueue.Dequeue().Invoke();
        }
    }

    public void Publicar(string topico, string payload)
    {
        if (client != null && client.IsConnected)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(payload);
            client.Publish(topico, bytes, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Debug.Log($"[MQTT Enviado] Tópico: {topico} | Payload: {payload}");
        }
        else
        {
            Debug.LogWarning("[MQTT] Cliente desconectado. Mensagem não enviada.");
        }
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string mensagem = Encoding.UTF8.GetString(e.Message);
        string topico = e.Topic;
        lock (executionQueue) { executionQueue.Enqueue(() => OnMessageArrived?.Invoke(topico, mensagem)); }
    }

    private void OnDestroy()
    {
        if (client != null && client.IsConnected) client.Disconnect();
    }
}