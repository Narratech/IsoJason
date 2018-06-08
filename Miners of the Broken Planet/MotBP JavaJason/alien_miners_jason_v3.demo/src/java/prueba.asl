/* CREENCIAS */

last_dir(null). // El último movimiento que hice (al principio es un null, claro)
free. // Al principio estoy desocupado, sin nada que hacer

/* REGLAS */

+dead : .my_name(N) <- .kill_agent(N).

calc_new_y(AgY,QuadY2,QuadY2) :- AgY+2 > QuadY2.
calc_new_y(AgY,_,Y) :- Y = AgY+2.

@setnewq[atomic]
+set_new_quadrant(Q): true 
	<- .drop_all_events;
		.abolish(quadrant(_,_,_,_));
	   -set_new_quadrant(Q);
	   +Q.
	   //-+free.
	   
/*-quadrant(_,_,_,_): true
	<- .abolish(around(_,_)).*/

/* PLANES PARA ENVIAR MI POSICIÓN INICIAL AL LÍDER */

+gsize(S,_,_) : true // S es como dijimos el numerillo ese, el ID de simulación 
  <- !send_init_pos(S).
+!send_init_pos(S) : pos(X,Y)
  <- .send(leader,tell,init_pos(S,X,Y)).
+!send_init_pos(S) : not pos(_,_) // Todavía no sé mi posición...
  <- .wait("+pos(X,Y)", 500);     // Esperar un rato antes de reintentarlo (500 ms)
     !!send_init_pos(S).

	 	 
/* PLANES PARA MERODEAR POR MI CUADRANTE CUANDO ESTOY DESOCUPADO */

+free : last_checked(X,Y)     <- !prep_around(X,Y).
+free : quadrant(X1,Y1,X2,Y2) <- !prep_around(X1,Y1).
+free : true                  <- !wait_for_quad.


@pwfq[atomic]
+!wait_for_quad : free & quadrant(_,_,_,_) 
   <- -+free.
+!wait_for_quad : free     
   <- .wait("+quadrant(X1,Y1,X2,Y2)", 500); 
      !!wait_for_quad.
+!wait_for_quad : not free 
   <- .print("No longer free while waiting for quadrant.").
-!wait_for_quad  
   <- !!wait_for_quad.

+around(X1,Y1) : quadrant(X1,Y1,X2,Y2) & free
  <- .print("in Q1 to ",X2,"x",Y1); 
     !prep_around(X2,Y1).


+around(X2,Y2) : quadrant(X1,Y1,X2,Y2) & free 
  <- .print("in Q4 to ",X1,"x",Y1); 
     !prep_around(X1,Y1).

+around(X2,Y) : quadrant(X1,Y1,X2,Y2) & free  
  <- ?calc_new_y(Y,Y2,YF);
     .print("in Q2 to ",X1,"x",YF);
     !prep_around(X1,YF).

+around(X1,Y) : quadrant(X1,Y1,X2,Y2) & free  
  <- ?calc_new_y(Y,Y2,YF);
     .print("in Q3 to ", X2, "x", YF); 
     !prep_around(X2,YF).

+around(X,Y) : quadrant(X1,Y1,X2,Y2) & free & Y <= Y2 & Y >= Y1  
  <- .print("in no Q, going to X1");
     !prep_around(X1,Y).

+around(X,Y) : quadrant(X1,Y1,X2,Y2) & free & X <= X2 & X >= X1  
  <- .print("in no Q, going to Y1");
     !prep_around(X,Y1).

+around(X,Y) : quadrant(X1,Y1,X2,Y2)
  <- .print("It should never happen!!!!!! - go home");
     !prep_around(X1,Y1).

+!prep_around(X,Y) : free
  <- -around(_,_); -last_dir(_); !around(X,Y).
 
+!around(X,Y) :
      (pos(AgX,AgY) & jia.neighbour(AgX,AgY,X,Y)) | last_dir(skip) 
   <- +around(X,Y).

+!around(X,Y) : not around(X,Y)
   <- !next_step(X,Y);
      !!around(X,Y).
 
+!around(X,Y) : true 
   <- !!around(X,Y).

 +!next_step(X,Y)
   :  pos(AgX,AgY)
   <- jia.get_direction(AgX, AgY, X, Y, D);
      -+last_dir(D);
      //.print("Moving to ", D)
      do(D).     

+!next_step(X,Y) : not pos(_,_)  
   <- !next_step(X,Y).


-!next_step(X,Y) : true  // gestión del fallo --> volver a repetir!
   <- .print("Failed next_step to ", X,"x",Y," fixing and trying again!");
      -+last_dir(null);
      !next_step(X,Y).


/* PLANES PARA BUSCAR ORO */

@pcell[atomic]          
+cell(X,Y,gold) 
  :  not carrying_gold & free & class(collector)
  <- -free;
     +gold(X,Y);
     .print("Gold perceived: ",gold(X,Y));
     !init_handle(gold(X,Y)).

+cell(X,Y,gold) 
  :  not gold(X,Y) & class(explorer)
  <- +gold(X,Y);
     .print("Announcing ",gold(X,Y)," to others");
     .broadcast(tell,gold(X,Y)). 
     
// Este será el código para todos, con el cual podrán informar de un cuadrante que contiene un huevo
+cell(X,Y,gold) 
  :  not gold(X,Y) & not committed_to(gold(X,Y)) &
   	 num_quads(N) &
     gsize(_,H,W) &
     jia.calculate_quadrant(N,H,W,X,Y,Q) &
     not goldInQuadrant(Q)
  	 <- +goldInQuadrant(Q);
     .print("Announcing ",goldInQuadrant(Q)," to others");
     .broadcast(tell,goldInQuadrant(Q)).

@ppgd[atomic]
+picked(G)[source(A)] 
  :  ( .desire(handle(G)) | .desire(init_handle(G)) )
  <- .print(A," has taken ",G," that I am pursuing! Dropping my intention.");
     .abolish(G);
	 //Debe ser que el único imprescindible de abandonar es el deseo de coger el oro
     .drop_desire(handle(G)); 
     !!choose_gold.

+picked(gold(X,Y))
  <- -gold(X,Y)[source(_)].

@pih1[atomic]
+!init_handle(Gold) 
  :  .desire(around(_,_))
  <- .print("Dropping around(_,_) desires and intentions to handle ",Gold);
     .drop_desire(around(_,_));
     !init_handle(Gold).

@pih2[atomic]
+!init_handle(Gold)
  :  pos(X,Y)
  <- .print("Going for ",Gold);
     -+last_checked(X,Y);
     !!handle(Gold). 

+!handle(gold(X,Y)) 
  :  not free 
  <- .print("Handling ",gold(X,Y)," now.");
     .broadcast(tell, committed_to(gold(X,Y)));
     !pos(X,Y);
     !ensure(pick,gold(X,Y));
     // Digo a todos que tengo el gold(X,Y), para evitar que alguien se ponga tras él
     .broadcast(tell,picked(gold(X,Y)));
     ?depot(_,DX,DY); //Este tipo de objetivos (?) es sólo responder una pregunta
     !pos(DX,DY);
     !ensure(drop, 0);
     -gold(X,Y)[source(_)]; 
     .print("Finish handling ",gold(X,Y));
     !!choose_gold. 

-!handle(G) : G
  <- .print("failed to catch gold ",G);
     .abolish(G); // Ignoramos la fuente de la creencia (de donde haya venido)
     !!choose_gold.
//Como antes, pero es que además ni siquiera tenía la creencia de ese oro...
-!handle(G) : true
  <- .print("failed to handle ",G,", it isn't in the BB anyway");
     !!choose_gold.

+!choose_gold 
  :  not gold(_,_)
  <- -+free.

+!choose_gold 
  :  gold(_,_)
  <- .findall(gold(X,Y),gold(X,Y),LG);
     !calc_gold_distance(LG,LD);
     .length(LD,LLD); LLD > 0;
     .print("Uncommitted gold distances: ",LD,LLD); //Cómo sabe tan seguro que uncommitted?
     .min(LD,d(_,NewG));
     .print("Next gold is ",NewG);
     !!handle(NewG).

-!choose_gold <- -+free.


+!calc_gold_distance([],[]).

+!calc_gold_distance([gold(GX,GY)|R],[d(D,gold(GX,GY))|RD]) 
  :  pos(IX,IY) & not committed_to(gold(GX,GY))
  <- jia.dist(IX,IY,GX,GY,D);
     !calc_gold_distance(R,RD).
//Y esto es la llamada recursiva, aunque ahora mismo no la entiendo bien...	 
+!calc_gold_distance([_|R],RD) 
  <- !calc_gold_distance(R,RD).

+!pos(X,Y) : pos(X,Y) <- .print("I've reached ",X,"x",Y).


+!pos(X,Y) : not pos(X,Y)
  <- !next_step(X,Y);
     !pos(X,Y).

+!ensure(pick,_) : pos(X,Y) & cell(X,Y,gold) & class(collector)
  <- do(pick).// ?carrying_gold. 

+!ensure(drop, _) : pos(X,Y) & depot(_,X,Y) & class(collector)
  <- do(drop). //TODO: Faltaría preguntar si ya   not ?carrying_gold. 






/* PLANES PARA MATAR ALIENS (SOLDIER) */

@pcellal[atomic]          
+cell(X,Y,alien) 
  :  free & class(soldier)
  <- -free;
     +alien(X,Y);
     .print("Alien perceived: ",alien(X,Y));
     !init_kill(alien(X,Y)).
     

/* +cell(X,Y,alien) 
  :  not alien(X,Y)
  <- +alien(X,Y);
     .print("Announcing ",alien(X,Y)," to others");
     .broadcast(tell,alien(X,Y)). */

 
+cell(X,Y,alien) 
  :  not alien(X,Y) &
     not committed_to(alien(X,Y)) &
   	 num_quads(N) &
     gsize(_,H,W) &
     jia.calculate_quadrant(N,H,W,X,Y,Q) &
     not alienInQuadrant(Q)
  <- +alienInQuadrant(Q);
     .print("Announcing ",alienInQuadrant(Q)," to others");
     .broadcast(tell,alienInQuadrant(Q)).

+killed(alien(X,Y)):
   	 num_quads(N) &
     gsize(H,W)
  <- -alien(X,Y)[source(_)];
     jia.calculate_quadrant(N,H,W,X,Y,Q);
     -alienInQuadrant(Q).

-alien(X,Y)
 <- .abolish(alien(_,_)).
 
@pik1[atomic]
+!init_kill(Alien) 
  :  .desire(around(_,_))
  <- .print("Dropping around(_,_) desires and intentions to kill ",Alien);
     .drop_desire(around(_,_));
     !init_kill(Alien).

@pik2[atomic]
+!init_kill(Alien)
  :  pos(X,Y)
  <- .print("Going for ",Alien);
     -+last_checked(X,Y);
     !!kill(Alien). 

/* +!kill(alien(X,Y)) 
  :  not free & not cell(alien(X,Y))
  	<- .print("Enemy have disappeared from ",X,",",Y);
  		.abolish(alien(X,Y));
  		!!choose_alien.*/


+!kill(alien(X,Y)) 
  :  not free
  <- .print("I'm going to kill ",alien(X,Y)," now.");
     .broadcast(tell, committed_to(alien(X,Y)));
     !pos(X,Y);
     !ensure(kill,alien(X,Y));
     .broadcast(tell,killed(alien(X,Y)));
     -alien(X,Y)[source(_)]; 
     ?base(quadrant(DX,DY,_,_));
     //!pos(DX,DY);
     .print("Finish killing ",alien(X,Y));
	 !returnToBase.	 
	 
-!kill(alien(X,Y)) : alien(X,Y) & pos(X,Y) & not cell(alien(X,Y))
  <- .print("alien disappeared ",alien(X,Y));
     .abolish(alien(X,Y));
     !!choose_alien.
-!kill(A) : A
  <- .print("failed to kill alien ",A);
     .abolish(A);
     !!choose_alien.
-!kill(A) : true
  <- .print("failed to handle ",A,", it isn't in the BB anyway");
     !!choose_alien.


+!choose_alien 
  :  not alien(_,_)
  <- -+free.
+!choose_alien 
  :  alien(_,_)
  <- .findall(alien(X,Y),alien(X,Y),LA);
     !calc_alien_distance(LA,LD);
     .length(LD,LLD); LLD > 0;
     .print("Uncommitted alien distances: ",LD,LLD); //Cómo sabe tan seguro que uncommitted?
     .min(LD,d(_,NewA));
     .print("Next alien is ",NewA);
     !!kill(NewA).
-!choose_alien <- -+free.

+!calc_alien_distance([],[]).
+!calc_alien_distance([alien(AX,AY)|R],[d(D,alien(AX,AY))|RD]) 
  :  pos(IX,IY) & not committed_to(alien(AX,AY))
  <- jia.dist(IX,IY,AX,AY,D);
     !calc_alien_distance(R,RD).
+!calc_alien_distance([_|R],RD) 
  <- !calc_alien_distance(R,RD).


+!ensure(kill,_) : pos(X,Y) & cell(X,Y,alien) & class(soldier)
  <- do(kill,alien,X,Y).
  
@returnToBase[atomic]
+!returnToBase: quadrant(X1,Y1,X2,Y2) & base(Quadrant)
	<- -quadrant(X1,Y1,X2,Y2); // It forgots its assigned quad is the current one
	   +Quadrant; // It believes its quadrant is the base quadrant
	   -+free. // It already is free and has a quad, so it will prep_around


/* FINAL DE LA SIMULACIÓN */

+end_of_simulation(S,_) : true 
  <- .drop_all_desires; 
     .abolish(quadrant(_,_,_,_));
     .abolish(gold(_,_));
     .abolish(committed_to(_));
     .abolish(picked(_));
     .abolish(last_checked(_,_));
     -+free;
     .print("-- END ",S," --").