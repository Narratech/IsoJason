// Este es el agente l�der, una especie de "sargento" de los mineros
//que les reparte por el mapa, para que as� "abarquen" m�s y mejor todo :)
//Yo creo que no tendr�a equivalente en nuestro ejemplo de los monjes... 
//... podemos pasar ol�mpicamente de esto ;-)

/* Se encarga de ubicar a los mineros por cuadrantes del escenario */

// Creo que esto significa que lo que viene a continuaci�n es un plan llamado
//quads, para establecer las casillas que caen en cada cuadrante al principio
//de la simulaci�n. Lo de atomic es una anotaci�n que se pone para indicar que
//mientras se est� ejcutando esta intenci�n NO SE PUEDE HACER OTRA COSA...
@quads[atomic]
//Cuando se produce el evento y yo, el agente l�der, percibo (es decir, a�ado a 
//mis creencias) el tama�o del tablero (algo que pasa al arraque de simulaci�n)
//directamente (true) me pongo a calcular cuadrantes y los a�ado a mis creencias
+gsize(S,W,H) : true
  <- // calculo del �rae de cada cuadrante y a�adir a mi memoria (creencias +)
     .print("Defining quadrants for ",W,"x",H," simulation ",S);
     +quad(S,1, 0, 0, W div 2 - 1, H div 2 - 1);
     +quad(S,2, W div 2, 0, W-1, H div 2 - 1);
     +quad(S,3, 0, H div 2, W div 2 - 1, H - 1);
     +quad(S,4, W div 2, H div 2, W - 1, H - 1);
     .print("Finished all quadrs for ",S).
	 
	 // S creo que simplemente es el numerito ese que identifica la simulaci�n

// Cuando percibo que alguien empieza en una posici� inicial (ese alguien, el 
//source del aviso, es el agente que sea, por ejemplo A) y se da la condici�n
// de que cuento los sucesos de este tipo y hay 4 (el n�mero de mineros)
//entonces me recuerdo a mi mismo que ninguno tiene cuadrante asignado y me
//apunto el objetivo de asign�rselo a los cuatro :-)
+init_pos(S,X,Y)[source(A)]
  :  // cuando todos los mineros, los 4, tienen posici�n inicial
     .count(init_pos(S,_,_),4)
  <- .print("* InitPos ",A," is ",X,"x",Y);
     // me apunto que los cuatro mineros NO tienen cuadrante asignado 
	 // (estamos hablando del principio de la simulaci�n, recordad)
     +~quad(S,miner1); +~quad(S,miner2);
     +~quad(S,miner3); +~quad(S,miner4);
     !assign_all_quads(S,[1,2,3,4]). //Me propongo asign�rselos
// En caso de que perciba lo mismo de antes pero que no se haya cumplido la 
//condici�n esa de que sepa las posiciones iniciales de los 4 mineros, entonces
//no puedo hacer nada todav�a (salvo mostrar un mensajito). Sigo esperando...
+init_pos(S,X,Y)[source(A)] 
  <- .print("- InitPos ",A," is ",X,"x",Y).

  
//Si tengo el objetivo/deseo de asignar los cuadrantes a todos los mineros...
//... y resulta que ya est�n todos los cuadrantes asignados, �pues he consegido 
//mi objetivo, se acab�!
+!assign_all_quads(_,[]).
// Si no, asigno un cuadrante y repito recursivamente con el resto
+!assign_all_quads(S,[Q|T])
  <- !assign_quad(S,Q);
     !assign_all_quads(S,T).

// Si tengo el objetivo/deseo de asignar un cuadrante, y se cumple que s�
//cual es ese cuadrante y que hay alg�n minero sin cuadrante asignado...
//entonces buscar� esos mineros "desocupados" ver� el minero que m�s cerca
//est� de ese cuadrante y se lo asignar� 
+!assign_quad(S,Q) 
  :  quad(S,Q,X1,Y1,X2,Y2) & 
     ~quad(S,_) // hay un minero sin cuadrante (la virgulilla esa significa NO)
  <- .findall(Ag, ~quad(S,Ag), LAgs); // Encuentrar mineros sin cuadrante
     !calc_ag_dist(S,Q,LAgs,LD); // Ver sus distancias al cuadrante
     .min(LD,d(Dist,Ag)); //Quearme con el minero m�s cercano
     .print(Ag, "'s Quadrant is: ",Q);
     -~quad(S,Ag); // Quit� esa creencia de que ese minero estaba sin cuadrante
     .send(Ag,tell,quadrant(X1,Y1,X2,Y2)). //y le informo del cuadrante asignado! 

// Si tengo que calcular las distancias de una lista de mineros al cuadrante Q
// y esa lista est� vac�a, pues la lista de distancias tambi�n lo estar�.Fin :)
+!calc_ag_dist(S,Q,[],[]).
// Pero si no, saco el primer minero de la lista y voy a ver su distancia,
// si se cumple la condici�n de que exista el cuadrante Q y el minero tenga su
// posici�n inicial, calculo la distancia y sigo con los dem�s mineros
+!calc_ag_dist(S,Q,[Ag|RAg],[d(Dist,Ag)|RDist]) 
  :  quad(S,Q,X1,Y1,X2,Y2) & init_pos(S,AgX,AgY)[source(Ag)]
  <- // conseguir la distancia entre X1,Y1 y AgX,AgY
     jia.dist(X1,Y1,AgX,AgY,Dist); // Esto es como Jason calcula la distancia??
     !calc_ag_dist(S,Q,RAg,RDist). //Recursivamente sigo con el resto de la lista de mineros


/* Temas de negociaci�n con el oro encontrado (los mineros pujan con la distancia 
a la que est�n, y al que le pille m�s cerca, se lo lleva.. es como los taxis) 
Aqu� gana el que "la tiene m�s corta" :-P */

//Si percibo/creo que me toca resolver una puja, y se da la condici�n de que
//tengo tres pujas (???) pues voy a asignar ese oro a alg�n minero y a borrar/
//abolir todas las pujas sobre ese oro.
+bid(Gold,D,Ag)
	// he recibido 3 pujas (ser� que m�s la que acaba de llegar, 4, no???)
  :  .count(bid(Gold,_,_),3)  
  <- .print("bid from ",Ag," for ",Gold," is ",D);
     !allocate_miner(Gold); //Asignar� ese oro a quien yo vea mejor
     .abolish(bid(Gold,_,_)). //A olvidarme de esas pujas
// Si toca resolver puja, y no hab�a esas 3 pujas, simplemente saco el mensajito
//pero no toca hacer nada todav�a
+bid(Gold,D,Ag)
  <- .print("bid from ",Ag," for ",Gold," is ",D).
 
 //Si tengo que asignar un oro a alguien, busco todo los pares distancia-minero
 //de entre toda la lista de pujas que haya, encuentro la distancia m�nima
 // y si es menor que 10000 (que significa "co�o, ya voy cargao, no quiero ir 
 //a por ese puto oro!" voy a decir a todo el mundo que le asigno ese oro
 //a ese minero que ten�a la "puja" menor
+!allocate_miner(Gold) 
  //op(Dist,A) creo que simplemente es una estructura de pareja distancia-minero
  <- .findall(op(Dist,A),bid(Gold,Dist,A),LD); //saco todas esas parejas
  	 !publish_received(LD);
     .min(LD,op(DistCloser,Closer)); // me quedo con la distancia m�nima
     DistCloser < 10000; // Estos mineros no me valen, que no pueden recoger m�s!
     .print("Gold ",Gold," was allocated to ",Closer, " options were ",LD);
     .broadcast(tell,allocated(Gold,Closer)); //broadcast es a todos los mineros!
     !publish_sent(allocated(Gold,Closer)).
// Si no se han cumplido las condiciones anteriores, porque no hab�a pujas
//que no sean de 10000, b�sicamente... lo siento pero no puedo asignar a nadie
//a que recoja ese oro... otra vez ser�! Vuelva usted a llamar m�s tarde :-[
-!allocate_miner(Gold)
  <- .print("could not allocate gold ",Gold).

+!publish_received(Message)
  <- jia.publish(Message,received). 
-!publish_received(Message)
	<- .print("Publishing received info failed.").
	
+!publish_sent(Message)
  <- jia.publish(Message,sent).
-!publish_sent(Message)
	<- .print("Publishing sent info failed.").

/* El plan para cuando se termina la simulaci�n */     
// El plan se llama end y es at�mico (vamos, que no se interrumpe hasta 
//que no se haya ejecutado entero)
@end[atomic]
+end_of_simulation(S,_) : true 
  <- .print("-- END ",S," --");
  	 // Abolir es quitar TODAS las creencias que siguen este patr�n, todas las
	 //posiciones iniciales
     .abolish(init_pos(S,_,_)). 

