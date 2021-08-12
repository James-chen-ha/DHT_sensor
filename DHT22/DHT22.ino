#include "DHT.h"

#define DHTPIN 2     // Digital pin connected to the DHT sensor
#define DHTTYPE DHT22   // DHT 22  (AM2302), AM2321
DHT dht(DHTPIN, DHTTYPE);

void setup() 
{
  Serial.begin(9600);
 // Serial.println(F("DHTxx test!"));

  dht.begin();
}




void loop() {
  // Wait a few seconds between measurements.
 // delay(2000);
  float t = dht.readTemperature();
  float h = dht.readHumidity();
  

  if (isnan(h) || isnan(t)) {return;}

  // Compute heat index in Fahrenheit (the default)
  Serial.print("@");
  Serial.print(t); Serial.print("A");
  Serial.print(h); Serial.print("B");
  Serial.println("\n");

  delay(500);
}
