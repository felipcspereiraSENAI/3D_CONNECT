using UnityEngine;

public class ControleLed : MonoBehaviour
{
    private const string TOPICO_LED = "escola/sala/led";

    public void BotaoLigarLED()
    {
        if (MQTTManager.Instance != null)
            MQTTManager.Instance.Publicar(TOPICO_LED, "LIGAR");
    }

    public void BotaoDesligarLED()
    {
        if (MQTTManager.Instance != null)
            MQTTManager.Instance.Publicar(TOPICO_LED, "DESLIGAR");
    }
}