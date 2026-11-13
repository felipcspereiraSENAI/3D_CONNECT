#include <ESP8266WiFi.h>
#include <PubSubClient.h>

// --- Configurações de Rede ---
const char* ssid = "3D";
const char* password = "abc123456";

// IP do computador onde o Mosquitto está rodando
const char* mqtt_server = "10.220.99.36"; 
const int mqtt_port = 1883;

WiFiClient espClient;
PubSubClient client(espClient);

void setup_wifi() {
  delay(10);
  Serial.print("Conectando em ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("\nWiFi conectado!");
  Serial.print("IP do ESP8266: ");
  Serial.println(WiFi.localIP());
}

// Executado quando a Unity envia uma mensagem
void callback(char* topic, byte* message, unsigned int length) {
  String payload = "";
  for (int i = 0; i < length; i++) {
    payload += (char)message[i];
  }

  Serial.print("Mensagem recebida [");
  Serial.print(topic);
  Serial.print("]: ");
  Serial.println(payload);

  if (String(topic) == "escola/sala/led") {
    if (payload == "LIGAR") {
      digitalWrite(LED_BUILTIN, LOW);  // No ESP8266, LOW acende o LED onboard
      Serial.println(">> LED ONBOARD ACESO");
    } 
    else if (payload == "DESLIGAR") {
      digitalWrite(LED_BUILTIN, HIGH); // No ESP8266, HIGH apaga o LED onboard
      Serial.println(">> LED ONBOARD APAGADO");
    }
  }
}

void reconnect() {
  while (!client.connected()) {
    Serial.print("Conectando ao Broker MQTT...");
    String clientId = "ESP8266_BuiltinLED_" + String(random(0xffff), HEX);
    
    if (client.connect(clientId.c_str())) {
      Serial.println(" Conectado!");
      client.subscribe("escola/sala/led");
    } else {
      Serial.print(" Falhou, rc=");
      Serial.print(client.state());
      Serial.println(" Tentando em 3s...");
      delay(3000);
    }
  }
}

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, HIGH); // Começa apagado (lógica invertida)

  Serial.begin(115200);
  setup_wifi();
  
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();
}
