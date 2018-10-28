int led = D7;
int pushButton = D1;

void setup(){
    pinMode(led, OUTPUT);
    pinMode(pushButton, INPUT_PULLUP);
}

void loop(){
    int pushButtonState;
    pushButtonState = digitalRead(pushButton);
    //String Button_Status = pushButtonState;
   
   
 // Particle.variable("Button_Status", pushButtonState);
   
    
   if(pushButtonState == LOW){
        digitalWrite(led, HIGH);
        Spark.publish("Button_Status","ON",60,PRIVATE);
//        Particle.variable("Button_Status", pushButtonState);
        
        delay(2000);
   }
    else{
        digitalWrite(led, LOW);
        Spark.publish("Button_Status","OFF",60,PRIVATE);
  }




}