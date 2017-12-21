// Este es el c�digo AgentSpeak de los mineros (los 4 son iguales)

/* CREENCIAS */

last_dir(null). // El �ltimo movimiento que hice (al principio es un null, claro)
free. // Al principio estoy desocupado, sin nada que hacer

/* REGLAS */

// Una regla es que puedo cambiar la coordenada Y de un minero por otra
//nueva que sea exactamente el l�mite inferior del cuadrante, siempre que la nueva 
//coordenada Y del minero COINCIDA con dicho l�mite inferior
//
// Me parece que es que el minero procurar pegar barridos en el eje de las X, 
//bajando de 2 en 2 filas por el de la Y...
calc_new_y(AgY,QuadY2,QuadY2) :- AgY+2 > QuadY2.
// si no, la siguiente l�nea es 2 l�neas m�s abajo (por eso de barrer de 2 en 2)
calc_new_y(AgY,_,Y) :- Y = AgY+2.


/* PLANES PARA ENVIAR MI POSICI�N INICIAL AL AMADO L�DER */

// Si percibo/creo que hay un tablero de cierto tama�o, directamente paso a 
//desear el enviar mi posici�n
+gsize(S,_,_) : true // S es como dijimos el numerillo ese, el ID de simulaci�n 
  <- !send_init_pos(S).
 // Y si deseo enviar mi posici�n y se cumple la condici�n de que creo que 
 //tengo una posici�n, pues se la digo al l�der
+!send_init_pos(S) : pos(X,Y)
  <- .send(leader,tell,init_pos(S,X,Y)).
// Si deseo lo mismo, pero no tengo ni idea de mi posici�n, pues nada... 
  //ya lo volver� a intentar m�s tarde
+!send_init_pos(S) : not pos(_,_) // Todav�a no s� mi posici�n...
  <- .wait("+pos(X,Y)", 500);     // Esperar un rato antes de reintentarlo (500 ms)
  	 //Creo que la doble exclamaci�n significa que no quieres hacer una llamada 
	 //recursiva, sino volver a ponerme el deseo DESDE EL PRINCIPIO
     !!send_init_pos(S).

	 	 
/* PLANES PARA MERODEAR POR MI CUADRANTE CUANDO ESTOY DESOCUPADO */

//No estoy seguro, pero creo que esto significa: cuando quedo libre y se cumple 
//que ten�a una �ltima casilla visitada en mi rastreo, entonces me preparo 
//para volver a dar una vuelta (merodear/rastrear) por donde me qued� all�
+free : last_checked(X,Y)     <- !prep_around(X,Y).
// Si no tengo destino, pero tengo al menos un cuadrante asignado, pues me
//preparao para darme una vuelta empeazando por su esquina superior izquierda
//(que viene a ser como el "principio" del cuadrante)
+free : quadrant(X1,Y1,X2,Y2) <- !prep_around(X1,Y1).
// Y si no tengo ni cuadrante, pues a esperar a que me lo asignen...
+free : true                  <- !wait_for_quad.

// Este es de hecho el plan para esperar a que me asignen cuadrante, que adem�s
//es at�mico, no puede interrumpirse hasta que sepamos si fallo o tiene �xito
@pwfq[atomic]
// Si deseo esperar cuadrante, y se cumple que estoy desocupado y YA TENGO 
//un cuadrante (ha debido asign�rmelo el l�der ahora mismo) pues estoy haciendo 
//un poco el tonto, as� que "reafirmo" (creo que -+ es quitar y volver a poner) 
//mi creencia de que estoy desocupado, a ver si as� me pongo a dar
//una vuelta (objetivo prep_around) gracias al plan anterior (free), le�e!!!
+!wait_for_quad : free & quadrant(_,_,_,_) 
   <- -+free.
// Si estoy desocupado, pero no tengo cuadrante... me pongo a esperar a ver si 
//el l�der me asigna uno (me bloqueo en esta intenci�n un m�ximo de medio segundo 
//(500 ms)). Una vea tengo cuadrante, repito deseo de esperar por cuadrante 
+!wait_for_quad : free     
   <- .wait("+quadrant(X1,Y1,X2,Y2)", 500); 
      !!wait_for_quad.
// Y si lo que pasa es que no estoy desocupado, pues nada... estar� ocupado
//haciendo mis cosicas...
+!wait_for_quad : not free 
   <- .print("No longer free while waiting for quadrant.").
// Si fracaso en mi objetivo/deseo de esperar por un cuadrante (porque el 
//anterior .wait podr�a fallar... pues me vuelvo a poner el objetivo de esperar
//por un cuadrante!
-!wait_for_quad  
   <- !!wait_for_quad.

// Si estoy cerca del X1, Y1, siendo estas las coordenadas de la esquina 
//superior izquierda de mi cuadrante, y desocupado, pues voy a darme un garbeo 
//hasta la esquina superior derecha
+around(X1,Y1) : quadrant(X1,Y1,X2,Y2) & free
  <- .print("in Q1 to ",X2,"x",Y1); 
     !prep_around(X2,Y1).

// Si estoy cerca del X2, Y2, siendo estas las coordenadas de la esquina 
//inferior derecha de mi cuadrante, y desocupado, pues voy a darme un garbeo 
//hasta la esquina superior izquierda
+around(X2,Y2) : quadrant(X1,Y1,X2,Y2) & free 
  <- .print("in Q4 to ",X1,"x",Y1); 
     !prep_around(X1,Y1).

// Si estoy cerca del X2 siendo este el l�mite derecho de mi cuadrante, y 
//desocupado, pues voy a darme un garbeo hacia la izquierda, pero bajando 
//dos l�neas (este es un poco el estilo del rastreo para peinar la zona 
//que hace, como a rayas: l�nea s� l�nea no, linea s� l�nea no, etc.)
+around(X2,Y) : quadrant(X1,Y1,X2,Y2) & free  
  <- ?calc_new_y(Y,Y2,YF);
     .print("in Q2 to ",X1,"x",YF);
     !prep_around(X1,YF).

// Si estoy cerca del X1 siendo este el l�mite izquierdo de mi cuadrante, y 
//desocupado, pues voy a darme un garbeo hacia la derecha, pero bajando 
//dos l�neas (mismo estilo de rastreo)
+around(X1,Y) : quadrant(X1,Y1,X2,Y2) & free  
  <- ?calc_new_y(Y,Y2,YF);
     .print("in Q3 to ", X2, "x", YF); 
     !prep_around(X2,YF).

// Si de lo que estoy cerca es de unas coordenadas que cumplen que la Y es 
//v�lida, est� dentro del cuadrante, pero la X no parece "estar cerca"
//entonces voy a tratar de volver a mi cuadrante prepar�ndome para regresar
//caminand hasta llegar cerca al l�mite izquierdo de MI cuadrante
+around(X,Y) : quadrant(X1,Y1,X2,Y2) & free & Y <= Y2 & Y >= Y1  
  <- .print("in no Q, going to X1");
     !prep_around(X1,Y).
// Lo mismo de antes pero para cuando es el eje Y el parece que se me ha
//ido de madre y me hace estar "cerca"/metido en otro cuadrante
+around(X,Y) : quadrant(X1,Y1,X2,Y2) & free & X <= X2 & X >= X1  
  <- .print("in no Q, going to Y1");
     !prep_around(X,Y1).

//Estoy no deber�a pasar nunca, pero si pasara que no estoy cerca ni con la X
//ni con la Y de mi cuadrante, pues voy adarme un garbeo hasta la esquina
//superior izquierda, que es por donde empieza uno a patearse su cuadrante
+around(X,Y) : quadrant(X1,Y1,X2,Y2)
  <- .print("It should never happen!!!!!! - go home");
     !prep_around(X1,Y1).

// Si deseo darme un garbeo por X,Y, y estoy desocupado, ELIMINO 
//(dejo de creer) que estoy cerca de donde sea que estoy cerca, dejo de creer
//en mi �ltima direcci�n en la que me mov�, y simplemente me pongo el objetivo
//de estar cerca de X,Y
+!prep_around(X,Y) : free
  <- -around(_,_); -last_dir(_); !around(X,Y).
 
// Si deseo estar cerca de X, Y, y se cumple que mi posici�n es "vecina"
//de la de X, Y... O (|) que mi �ltimo movimiento fue en ninguna direcci�n (lo 
//que significar�a que no hay forma de llegar a donde voy, por eso lo de skip),
//pues en ese caso puedo pasar a tener la creencia de que S�: estoy cerca de X, Y
+!around(X,Y) :
      (pos(AgX,AgY) & jia.neighbour(AgX,AgY,X,Y)) | last_dir(skip) 
   <- +around(X,Y).
 // Si deseo lo mismo, pero no estoy cerca, entonces es cuando deseo dar un paso
 //(next step) y despu�s volver a tratar de estar cerca de X, Y (!! se relanza
 //el objetivo/deseo)
+!around(X,Y) : not around(X,Y)
   <- !next_step(X,Y);
      !!around(X,Y).
//Y si las otras reglas no funcionaron, pues nada, a relanzar el objetivo otra vez!	  
+!around(X,Y) : true 
   <- !!around(X,Y).

 // Si deseo dar un paso, gracias a jia averiguro la direcci�n a la que moverme
 //y "reestablezco" la creencia de que mi �ltima direcci�n es D, justo la acci�n
 //que hago, que es D.
+!next_step(X,Y)
   :  pos(AgX,AgY)
   <- jia.get_direction(AgX, AgY, X, Y, D);
      -+last_dir(D);
      do(D).
// Si pasa lo mismo, y adem�s ocurre que todav�a no s� cual es mi posici�n,
//vuelvo a desear dar ese siguiente paso...
+!next_step(X,Y) : not pos(_,_)  
   <- !next_step(X,Y).
// Si pasa lo mismo y no ha valido nada de los dem�s, "reestalezco" que mi 
// �ltima direcci�n fue nula y vuelvo a intentar el objetivo next_step
-!next_step(X,Y) : true  // gesti�n del fallo --> volver a repetir!
   <- .print("Failed next_step to ", X,"x",Y," fixing and trying again!");
      -+last_dir(null);
      !next_step(X,Y).


/* PLANES PARA BUSCAR ORO */

//Ya sab�is, un plan at�mico es aquel que se ejecuta sin hacer ni caso
//a cualquier evento que ocurra hasta que se termine por completo (en este caso
//hasta ponerme el objetivo de hacerme con ese oro)
@pcell[atomic]          
// Si percibo una casilla con oro, y coincide que yo no estoy
//llevando ning�n oro a cuestas y estoy desocupado... paso a estar ocupado,
//marco el oro como percibido y me pongo el objetivo de hacerme con �l
+cell(X,Y,gold) 
  :  not carrying_gold & free
  <- -free;
     +gold(X,Y);
     .print("Gold perceived: ",gold(X,Y));
     !init_handle(gold(X,Y)).
     
// Si percibo una casilla con oro, y no estoy llevando ning�n oro a cuestas 
//pero estoy ocupado (porque voy de camino a por otro oro, que adem�s est� m�s
//lejos, umm)... pues marco el nuevo oro como percibido, abandono el deseo de 
//pillar el otro oro lejano y aviso a los dem�s de que me "desdigo" sobre lo de
//buscar ese oro antiguo y les digo donde est� por si alguien quieren ir...
//ah, y tambi�n me pongo a mi mismo el objetivo de hacerme con el nuevo oro
//m�s cercano!
@pcell2[atomic]
+cell(X,Y,gold)
  :  not carrying_gold & not free &
     .desire(handle(gold(OldX,OldY))) &   // Tengo el deseo de pillar otro oro
     pos(AgX,AgY) &
     jia.dist(X,   Y,   AgX,AgY,DNewG) &
     jia.dist(OldX,OldY,AgX,AgY,DOldG) &
     DNewG < DOldG        // pero est� m�s lejos que el que acabo de percibir
  <- +gold(X,Y);
     .drop_desire(handle(gold(OldX,OldY)));
     .print("Giving up current gold ",gold(OldX,OldY),
            " to handle ",gold(X,Y)," which I am seeing!");
     .print("Announcing ",gold(OldX,OldY)," to others");
     .broadcast(tell,gold(OldX,OldY));
     .broadcast(untell, committed_to(gold(OldX,OldY)));
     !init_handle(gold(X,Y)).
    
//Si no se cumple lo anterior (vamos, que estoy ocupado), pero s� es verdad que 
//ese oro no estaba descubierto ni yo estaba a por �l... simplemente me lo 
//a�ado como creencia y anuncio su existencia a los dem�s mineros
+cell(X,Y,gold) 
  :  not gold(X,Y) & not committed_to(gold(X,Y))
  <- +gold(X,Y);
     .print("Announcing ",gold(X,Y)," to others");
     .broadcast(tell,gold(X,Y)). 
     
// Si recibo la informaci�n de que hay oro en X,Y y se cumple la condici�n
//de que dicha creencia no surge de mi, ni hay nadie adjudicado para cogerlo,
//ni estoy llevando yo oro ahora mismo, y estoy desocupado y tal...
//entonces hago una puja por ese oro y se la mando al l�der
//(a ver si me lo asigna con un poco de suerte, jeje)
+gold(X1,Y1)[source(A)]
  :  A \== self &
     not allocated(gold(X1,Y1),_) & // El oro no est� asignado a nadie
     not carrying_gold &            // Yo no estoy llevando ning�n oro
     free &                         // Estoy desocupado, adem�s
     pos(X2,Y2) & // Esto es para sacar mi posici�n
     .my_name(Me) // Esto es para meter mi ID de agente en la variable Me
  <- jia.dist(X1,Y1,X2,Y2,D);       // bid
     .send(leader,tell,bid(gold(X1,Y1),D,Me)). //Yo pujo, se�or l�der!!!

// Si no se cumple lo de antes (b�sicamente porque no estoy desocupado)
//entonces pujo pero con 10000 que significa "mejor no me lo asignes ahora, l�der, 
//que estoy liado" 
+gold(X1,Y1)[source(A)]
  :  A \== self & .my_name(Me)
  <- .send(leader,tell,bid(gold(X1,Y1),10000,Me)).

// El plan at�mico de cuando el l�der me asigna un oro, �qu� felicidad!
// Dejo de estar libre y me pongo a tratar de conseguir ese oro :-)
@palloc1[atomic]
+allocated(Gold,Ag)[source(leader)] 
  :  .my_name(Ag) & free // Sigo estando desocupado (si no, no podr�a aceptarlo)
  <- -free;
     .print("Gold ",Gold," allocated to ",Ag);
     !init_handle(Gold).

// Otro plan at�mico para cuando el �der me lo asigna, pero justo ahora resulta
//que ya no lo puedo gestionar porque ya no estoy libre... proceder� a 
//re-anunciar que ese oro est� ah�, disponible! 
@palloc2[atomic]
+allocated(Gold,Ag)[source(leader)] 
  :  .my_name(Ag) & not free // Lo siento, pero ya no estoy libre...
  <- .print("I can not handle ",Gold," anymore!");
     .print("(Re)announcing ",Gold," to others");
     .broadcast(tell,Gold). 
     
// Plan at�mico para cuando alguien anuncia que ha cogido un oro y se cumple
//la condici�n de que era el oro que yo esta cogiendo o estaba tratando de coger
//.. asi que dejo de querer coger ese oro y voy a ponerme el objetivo de elegir otro
@ppgd[atomic]
+picked(G)[source(A)] 
  :  .desire(handle(G)) | .desire(init_handle(G))
  <- .print(A," has taken ",G," that I am pursuing! Dropping my intention.");
     .abolish(G);
	 //Debe ser que el �nico imprescindible de abandonar es el deseo de coger el oro
     .drop_desire(handle(G)); 
     !!choose_gold.

// Si simplemente es alguien que ha cogido un oro del que yo sab�a que exist�a
//(aunque ni iba tras �l por ahora ni nada), pues lo quito de mis creencias y fuera 
+picked(gold(X,Y))
  <- -gold(X,Y)[source(_)].

// Plan at�mico de cuando me voy a poner a tratar de conseguir un oro y se 
//cumple que estaba yo con ganas de merodear... pues mando a la porra mis
//ganas de merodear y paso al siguiente plan para tratar de coseguir el oro
@pih1[atomic]
+!init_handle(Gold) 
  :  .desire(around(_,_)) 
  <- .print("Dropping around(_,_) desires and intentions to handle ",Gold);
     .drop_desire(around(_,_));
     !init_handle(Gold).
//Segundo plan at�mico para tratar de conseguir un oro, estando en una cierta
//posici�n.... reafirmar� mi creencia de que esa era mi �ltima posici�n
//visitadan haciendo mi ronda (para luego volver m�s tarde) y me lanzo a coger
//el oro!
@pih2[atomic]
+!init_handle(Gold)
  :  pos(X,Y)
  <- .print("Going for ",Gold);
     -+last_checked(X,Y);
	 // Ojo, aqu� se usa !! en vez de simplemente ! para que "handle" se ejecute
	 //como un plan NORMAL, no at�mico (recuerda que lo estamos llamado 
	 //desde aqu�, que estamos dentro de un plan at�mico)
     !!handle(Gold). 

//Cuando quiero coger un oro y se cumple que no estoy desocupado (claro, nada 
//de merodeos, yo estoy a por oro), entonces le digo a todo el mundo que estoy
//comprometido con pillar ese oro, trato de moverme hasta su posici�n, me
//aseguro de cogerlo, de dec�rselo a todo el mundo, luego averiguo donde est�
//el dep�sito de oro, me muevo hasta all� y me aseguro de soltarlo. Dejo de
//creer en ese oro y me pongo a elegir otro oro
+!handle(gold(X,Y)) 
  :  not free 
  <- .print("Handling ",gold(X,Y)," now.");
     .broadcast(tell, committed_to(gold(X,Y)));
     !pos(X,Y);
     !ensure(pick,gold(X,Y));
     // Digo a todos que tengo el gold(X,Y), para evitar que alguien se ponga tras �l
     .broadcast(tell,picked(gold(X,Y)));
     ?depot(_,DX,DY); //Este tipo de objetivos (?) es s�lo responder una pregunta
     !pos(DX,DY);
     !ensure(drop, 0);
     -gold(X,Y)[source(_)]; 
     .print("Finish handling ",gold(X,Y));
	 //Creo que lo de !! es para no considerar choose_gold como un subgoal de handle, 
	 //sino que es un deseo totalmente nuevo y fresco que arrancamos
     !!choose_gold. 

//Si falla lo de antes, seguramente porque falla lo de ensure(pick/drop) 
//entonces ignoro la creencia de ese oro, y me pongo a perseguir otro oro 
-!handle(G) : G
  <- .print("failed to catch gold ",G);
     .abolish(G); // Ignoramos la fuente de la creencia (de donde haya venido)
     !!choose_gold.
//Como antes, pero es que adem�s ni siquiera ten�a la creencia de ese oro...
-!handle(G) : true
  <- .print("failed to handle ",G,", it isn't in the BB anyway");
     !!choose_gold.

// Si deseo elegir un oro que perseguir o no s� de ninguno, entonces
//voy a REAFIRMAR mi creencia de que estoy libre, para volver a comportarme como tal 
//(para merodear en busca de oro)
+!choose_gold 
  :  not gold(_,_)
  <- -+free.

// Si deseo elegir un oro y s� de algunos otros que quedan sin pillar,
// entonces tendr� que encontrarlos todos y calcular las distancias a las que 
//estoy de ellos, y con el que tenga la m�nima, a por ese oro que voy!
+!choose_gold 
  :  gold(_,_)
  <- .findall(gold(X,Y),gold(X,Y),LG);
     !calc_gold_distance(LG,LD);
     .length(LD,LLD); LLD > 0;
     .print("Uncommitted gold distances: ",LD,LLD); //C�mo sabe tan seguro que uncommitted?
     .min(LD,d(_,NewG));
     .print("Next gold is ",NewG);
     !!handle(NewG).
// Y cuando pierda el deseo de elegir un oro, reiterarme en que estoy libre para merodear
-!choose_gold <- -+free.

//Si no hay lista de cosas, no hay distancias entre ellas :-)
+!calc_gold_distance([],[]).
//Calculo de la distancia de mi posici�n a la de una de los oros, y lo anoto
+!calc_gold_distance([gold(GX,GY)|R],[d(D,gold(GX,GY))|RD]) 
  :  pos(IX,IY) & not committed_to(gold(GX,GY))
  <- jia.dist(IX,IY,GX,GY,D);
     !calc_gold_distance(R,RD).
//Y esto es la llamada recursiva, aunque ahora mismo no la entiendo bien...	 
+!calc_gold_distance([_|R],RD) 
  <- !calc_gold_distance(R,RD).

// UN SECRETO, OJO!!!
// !pos se usa cuando siempres es posible ir a esa posic�n   
// por lo que este plan no deber�a ser usado: 
// +!pos(X,Y) : last_dir(skip) <- .print("It is not possible to go to ",X,"x",Y).
// En el futuro lo que habr�a que hacer es:
// +last_dir(skip) <- .drop_goal(pos) 
// Pero bueno, la cosa es que si quiero ir a una posici�n X,Y y ya estoy en ella: hecho!
+!pos(X,Y) : pos(X,Y) <- .print("I've reached ",X,"x",Y).
//Y si no estoy en ella, doy un paso y vuelvo a hacer la llamada recursiva 
//de que quiero estar en dicha posici�n X,Y 
+!pos(X,Y) : not pos(X,Y)
  <- !next_step(X,Y);
     !pos(X,Y).

//Asegurarse coger algo, cuando se cumple que estoy en X,Y y que all� hay oro
//es b�sicamente lanzar la acci�n PICK y luego preguntar si estoy llevando oro
+!ensure(pick,_) : pos(X,Y) & cell(X,Y,gold)
  <- do(pick); ?carrying_gold. 
// Falla si no hubiese oro all�, o si tras hacer el pick no estoy cogiendo el oro por lo que sea   
// ... Este fallo de alguna manera es "capturado y gestionado" por handle(G) 

//Asegurarse de soltar algo, cuando se cumple que estoy en X,Y y que all� hay
//un dep�sito de oro, es b�sicamente lanzar la acci�n DROP
+!ensure(drop, _) : pos(X,Y) & depot(_,X,Y) 
  <- do(drop). //TODO: Faltar�a preguntar si ya   not ?carrying_gold. 


/* FINAL DE LA SIMULACI�N */

// Cuando percibo que se acaba la simulaci�n, directamente mando a la porra
//todos mis deseos, me cargo un buen mont�n de mis creencias, y me reafirmo en ser libre
+end_of_simulation(S,_) : true 
  <- .drop_all_desires; 
     .abolish(quadrant(_,_,_,_));
     .abolish(gold(_,_));
     .abolish(committed_to(_));
     .abolish(picked(_));
     .abolish(last_checked(_,_));
     -+free;
     .print("-- END ",S," --").

