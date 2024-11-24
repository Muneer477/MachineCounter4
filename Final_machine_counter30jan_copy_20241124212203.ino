#include <Ethernet.h>
#include <SPI.h>

// Network settings
byte mac[] = {
 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED
};
IPAddress ip(192, 168, 2, 209);  // Device IP address
IPAddress server(192, 168, 2, 96); // Server IP address

// Ethernet client
EthernetClient client;

// Pin settings
const int outputPin = 2;
const int inputPin = 3;

// Variables for tracking status
int wireStatus = 2;
int lastTouchState = HIGH; // Initialized to HIGH because of the pull-up resistor

// Device and serial number
const char* deviceIpAddress = "192.168.2.209"; // Device IP address
const char* serialNumber = "PIOT-019";       // Serial number

// Timing for HTTP requests
unsigned long previousMillis = 0;            
const long interval = 6000;                  // Interval for sending HTTP request (6 seconds)

void setup() {
  Serial.begin(9600); // Initialize Serial communication
  pinMode(outputPin, OUTPUT);
  pinMode(inputPin, INPUT_PULLUP); // Set the pin as input with internal pull-up resistor
  Ethernet.begin(mac, ip);
  delay(1000);
}

void loop() {
  int currentTouchState = digitalRead(inputPin);
  if (currentTouchState != lastTouchState) { 
    if (currentTouchState == LOW) {
      wireStatus = 1;
      Serial.println("Wires are touching!");
      updateDatabase();
    }
    lastTouchState = currentTouchState;
  }

  // Send periodic running signal
  unsigned long currentMillis = millis();
  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;
    sendRunningSignal();
  }

  delay(100); // A small delay for debouncing
}

void updateDatabase() {
  if (client.connect(server, 8080)) {
    String url = "GET /api/piotCounter/counter?serialNo=" + String(serialNumber) + "&ipAddress=" + String(deviceIpAddress) + " HTTP/1.1";
    Serial.println("Sending request: " + url);
    client.println(url);
    client.println("Connection: close");
    client.println();
    client.stop();
  } else {
    Serial.println("Connection to database failed");
  }
}

void sendRunningSignal() {
  if (client.connect(server, 8080)) {
    String url = "GET /api/piotRunning/running?serialNo=" + String(serialNumber) + "&ipAddress=" + String(deviceIpAddress) + " HTTP/1.1";
    Serial.println("Sending running signal: " + url);
    client.println(url);
    client.println("Connection: close");
    client.println();
    client.stop();
  } else {
    Serial.println("Connection for running signal failed");
  }
}
