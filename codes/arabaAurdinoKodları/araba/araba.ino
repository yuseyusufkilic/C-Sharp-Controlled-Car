#include <SoftwareSerial.h>
#include <dht11.h>
#define sagmotorhiz 11
#define solmotorhiz 6
#define sagmotoron 12
#define solmotoron 8
#define sagmotorarka 10
#define solmotorarka 9
SoftwareSerial bt(0,1);

 const int trigPin = 4; //mesafe sensörü pin tanımlaması
 const int echoPin = 3;
 const int suSensorPin=A0;  //yağmur sensörü pin ve eşik değeri tanımlaması
 const int sesSensorPin=13;
 dht11 DHT11; //sıcaklık sensörümüz için dht11 nesnemizi oluşturduk 

 const int buzzerPin = 5;  //buzzer pin tanımlaması 
  const int ledPin = 2;
 double uzaklik=0;
 double sure=0;
 int suDeger=0;
 
void setup(){
  DHT11.attach(7); // digital pinlerimizde 2 numaralı olanı dht11 sensörümüze bağladığımızı belirtiyoruz
  bt.begin(9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(buzzerPin, OUTPUT); 
  pinMode(ledPin, OUTPUT);
   pinMode(sesSensorPin,INPUT);
  pinMode(sagmotorhiz,OUTPUT);
  pinMode(solmotorhiz,OUTPUT);
  pinMode(sagmotoron,OUTPUT);
  pinMode(sagmotorarka,OUTPUT);
  pinMode(solmotoron,OUTPUT);
  pinMode(solmotorarka,OUTPUT);
  
}

char okunanDeger; //formdan gönderilecek karakterlere göre işlem yapacağımızdan char tipinde değişken tanımladık

void loop(){     
if(bt.available()>0){
  okunanDeger=bt.read();
}
if(okunanDeger=='w'){  //eger ki bilgisayardan gönderilen deger 'w' ise araba ileri gider.
  ileri(); 
  }
  if(okunanDeger=='a'){ //eger ki bilgisayardan gönderilen deger 'a' ise araba sola gider.
  sola(); 
  }
  if(okunanDeger=='d'){ //eger ki bilgisayardan gönderilen deger 'd' ise araba saga gider.
  saga(); 
  }
  if(okunanDeger=='x'){ //eger ki bilgisayardan gönderilen deger 'x' ise araba geri gider.
  geri(); 
  }
  if(okunanDeger=='s'){ //eger ki bilgisayardan gönderilen deger 's' ise araba durur.
  dur(); 
  }  
 if(okunanDeger=='m')
 {
  mesafeHesapla();
 }
 if(okunanDeger=='v')
 {
  sesAlgila();
 
 }
 if(okunanDeger=='r')
 {
 suAlgila();
 }
  if(okunanDeger=='c')
 {
  sicaklikHesapla();
 }
  if(okunanDeger=='b')
 {
  buzzerDurdur();
 }
  if(okunanDeger=='g')
 {
  buzzerCal();
 }
 if(okunanDeger=='y')
 {
  ledYak();
 }
 if(okunanDeger=='l')
 {
  ledSondur();
 }

    delay(400);
}


void mesafeHesapla(){

   digitalWrite(trigPin, LOW);
   delayMicroseconds(5);
   digitalWrite(trigPin, HIGH);
   delayMicroseconds(10);
   digitalWrite(trigPin, LOW);   
   sure = pulseIn(echoPin, HIGH);
   uzaklik = sure /29.1/2;  
   bt.println(uzaklik,2);
  
}
void sesAlgila(){
  while(true){
  int deger=digitalRead(sesSensorPin);
  if(deger==HIGH){
    bt.print("111");  
    break;
  }  
  }
}

void suAlgila(){  
    suDeger=analogRead(suSensorPin);    
    bt.println(suDeger);    
 
}
void sicaklikHesapla(){
  int chk = DHT11.read();
   bt.println((float)DHT11.temperature, 2);
}
void buzzerCal(){
 digitalWrite(buzzerPin, HIGH);  
}
void buzzerDurdur()
{
  digitalWrite(buzzerPin, LOW);
}
void ledYak(){
   digitalWrite(ledPin, HIGH);  
}
void ledSondur(){
  digitalWrite(ledPin, LOW);  
}
void ileri()
 {
 analogWrite(sagmotorhiz,100); //motorhız
 digitalWrite(sagmotoron,1);  //motor ön
 digitalWrite(sagmotorarka,0); //arkaya dönmesine engel
  
 analogWrite(solmotorhiz,100); //motorhız
 digitalWrite(solmotoron,1);  //motor ön
 digitalWrite(solmotorarka,0); //arkaya dönmesine engel

 }

 void geri()
 {
 analogWrite(sagmotorhiz,100); //motorhız
 digitalWrite(sagmotoron,0);  //motor ön
 digitalWrite(sagmotorarka,1); //arkaya dönmesine engel
  
 analogWrite(solmotorhiz,100); //motorhız
 digitalWrite(solmotoron,0);  //motor ön
 digitalWrite(solmotorarka,1); //arkaya dönmesine engel

 }
  void saga()
 {
 analogWrite(sagmotorhiz,100); //motorhız
 digitalWrite(sagmotoron,0);  //motor ön
 digitalWrite(sagmotorarka,1); //arkaya dönmesine engel
  
 analogWrite(solmotorhiz,100); //motorhız
 digitalWrite(solmotoron,1);  //motor ön
 digitalWrite(solmotorarka,0); //arkaya dönmesine engel 

 }

  void sola()
 {
 analogWrite(sagmotorhiz,100); //motorhız
 digitalWrite(sagmotoron,1);  //motor ön
 digitalWrite(sagmotorarka,0); //arkaya dönmesine engel
  
 analogWrite(solmotorhiz,100); //motorhız
 digitalWrite(solmotoron,0);  //motor ön
 digitalWrite(solmotorarka,1); //arkaya dönmesine engel 

 }
void dur()
 {
 analogWrite(sagmotorhiz,0); //motorhız
 digitalWrite(sagmotoron,0);  //motor ön
 digitalWrite(sagmotorarka,0); //arkaya dönmesine engel
  
 analogWrite(solmotorhiz,0); //motorhız
 digitalWrite(solmotoron,0);  //motor ön
 digitalWrite(solmotorarka,0); //arkaya dönmesine engel 


 }
