/* CREENCIAS */

last_dir(null).
free.

/* REGLAS */

+dead : .my_name(N) <- .kill_agent(N).

calc_new_y(AgY,QuadY2,QuadY2) :- AgY+2 > QuadY2.
calc_new_y(AgY,_,Y) :- Y = AgY+2.

+set_new_quadrant(Q): class(collector) & (carrying_gold[source(percept)])
	<- .send(leader, tell, log_busy_aleph);
		-set_new_quadrant(Q).

@setnewq[atomic]
+set_new_quadrant(Q): true 
	<- .abolish(quadrant(_,_,_,_));
	   .drop_intention(around(X,Y));
	   .drop_event(around(X,Y));
	   .drop_intention(next_step(_,_));
	   .drop_event(next_step(_,_));
	   .drop_intention(pos(_,_));
	   .drop_event(pos(_,_));
	   !ignore_deletion(Q);
	   -set_new_quadrant(Q);
	   +Q;
	   -+free.
+!ignore_deletion(quadrant(X1,Y1,_,_)):
		class(soldier) &
	 	num_quads(N) &
     	gsize(_,H,W) &
     	jia.calculate_quadrant(N,H,W,X1,Y1,Q) 
	<-	-ignore(alienInQuadrant(Q)).
+!ignore_deletion(Q): not class(soldier)
	<- true.

+go_to_pos(X,Y) : class(collector) & (carrying_gold[source(percept)])
	<- .send(leader, tell, log_busy_aleph);
		-go_to_pos(X,Y).

@gotopos[atomic]
+go_to_pos(X,Y) : class(collector)
	<- /*.drop_intention(around(_,_));
	   .drop_event(around(_,_));
	   .drop_intention(next_step(_,_));
	   .drop_event(next_step(_,_));
	   .drop_intention(pos(_,_));
	   .drop_event(pos(_,_));*/
	   .drop_desire(next_step(_,_));
	   -go_to_pos(X,Y);
	   .send(leader, tell, log_going_to_pos);
	   //!!pos(X,Y).
	   -free;
	   +gold(X,Y);
	   !init_handle(gold(X,Y)).
/*-quadrant(_,_,_,_): true
	<- .abolish(around(_,_)).*/

/* PLANES PARA ENVIAR MI POSICIÓN INICIAL AL LÍDER */


+gsize(S,_,_) : true
  <- !send_init_pos(S).

+!send_init_pos(S) : pos(X,Y)
  <- .send(leader,tell,init_pos(S,X,Y)).
+!send_init_pos(S) : not pos(_,_)
  <- .wait("+pos(X,Y)", 500);    
     !!send_init_pos(S).

	 	 
/* PLANES PARA MERODEAR POR MI CUADRANTE CUANDO ESTOY DESOCUPADO */


//+free : last_checked(X,Y)     <- !prep_around(X,Y).
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
  <- //.print("in Q1 to ",X2,"x",Y1); 
     !prep_around(X2,Y1).
+around(X2,Y2) : quadrant(X1,Y1,X2,Y2) & free 
  <- //.print("in Q4 to ",X1,"x",Y1); 
     !prep_around(X1,Y1).
+around(X2,Y) : quadrant(X1,Y1,X2,Y2) & free  
  <- ?calc_new_y(Y,Y2,YF);
     //.print("in Q2 to ",X1,"x",YF);
     !prep_around(X1,YF).
+around(X1,Y) : quadrant(X1,Y1,X2,Y2) & free  
  <- ?calc_new_y(Y,Y2,YF);
     //.print("in Q3 to ", X2, "x", YF); 
     !prep_around(X2,YF).
+around(X,Y) : quadrant(X1,Y1,X2,Y2) & free & Y <= Y2 & Y >= Y1  
  <- //.print("in no Q, going to X1");
     !prep_around(X1,Y).
+around(X,Y) : quadrant(X1,Y1,X2,Y2) & free & X <= X2 & X >= X1  
  <- //.print("in no Q, going to Y1");
     !prep_around(X,Y1).
+around(X,Y) : quadrant(X1,Y1,X2,Y2)
  <- //.print("It should never happen!!!!!! - go home");
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

/* ------- PERSONALITY PLAN ------- */
/* COWARD COLLECTOR */
// If the collector knows about an alien in the quad it's going through, it's so scared it thinks slowly
 +!next_step(X,Y)
   :  pos(AgX,AgY) & class(collector) & personality(coward) & 
   	  num_quads(N) &
      gsize(_,H,W) &
      jia.calculate_quadrant(N,H,W,AgX,AgY,Q) & 
      alienInQuadrant(Q)
   <- jia.get_direction(AgX, AgY, X, Y, D);
   	  .wait(300);
      -+last_dir(D);
      do(D).
/* ------- IF OUT OF CHANCE, DO DEFAULT (Next listed plan below) ------- */

 +!next_step(X,Y)
   :  pos(AgX,AgY)
   <- jia.get_direction(AgX, AgY, X, Y, D);
      -+last_dir(D);
      do(D).
+!next_step(X,Y) : not pos(_,_)  
   <- !next_step(X,Y).
-!next_step(X,Y) : true
   <- .print("Failed next_step to ", X,"x",Y," fixing and trying again!");
      -+last_dir(null);
      !next_step(X,Y).


/* PLANES PARA BUSCAR ORO */


/* +gold(X,Y): class(collector)
	<- .abolish(gold(X,Y)).*/

/* ------- PERSONALITY PLAN ------- */
/* RENEGADE COLLECTOR */
// COLLECTOR PLAN
@pcellrencol[atomic]          
+cell(X,Y,gold) 
  :  not gold(X,Y) & class(collector) & personality(renegade) & not carrying_gold & free & 
   	 .random(R) & .print(R) & R > 0.8
  <- -free;
     +gold(X,Y);
     +steal(gold(X,Y));
     !init_handle(gold(X,Y)).
	
// COLLECTOR PLAN
@pcell[atomic]          
+cell(X,Y,gold) 
  :  class(collector) & not carrying_gold & free
  <- -free;
     +gold(X,Y);
     .print("Gold perceived: ",gold(X,Y));
     !init_handle(gold(X,Y)).
// COLLECTOR PLAN   
@pcell2[atomic]
+cell(X,Y,gold)
  :  not carrying_gold & not free & class(collector) &
     .desire(handle(gold(OldX,OldY))) &   // Tengo el deseo de pillar otro oro
     pos(AgX,AgY) &
     jia.dist(X,   Y,   AgX,AgY,DNewG) &
     jia.dist(OldX,OldY,AgX,AgY,DOldG) &
     DNewG < DOldG        // pero está más lejos que el que acabo de percibir
  <- +gold(X,Y);
     .drop_desire(handle(gold(OldX,OldY)));
     .print("Giving up current gold ",gold(OldX,OldY),
            " to handle ",gold(X,Y)," which I am seeing!");
     .print("Announcing ",gold(OldX,OldY)," to others");
     .broadcast(tell,gold(OldX,OldY));
     .broadcast(untell, committed_to(gold(OldX,OldY)));
     !init_handle(gold(X,Y)).
     
/* ------- PERSONALITY PLAN ------- */
/* DISSIDENT EXPLORER */
// EXPLORER PLAN
// If it perceives gold(X,Y), the collector keeps it for itself, and wont advice about that Aleph anymore
+cell(X,Y,gold) 
  :  not gold(X,Y) & class(explorer) & personality(dissident) &
  	 .random(R) & R > 0.7
  <- +gold(X,Y).
/* ------- IF OUT OF CHANCE, DO DEFAULT (Next listed plan below) ------- */

// EXPLORER PLAN    
+cell(X,Y,gold) 
  :  not gold(X,Y) & class(explorer)
  <- +gold(X,Y);
     .print("Announcing ",gold(X,Y)," to others");
     .broadcast(tell,gold(X,Y)). 

/* ------- PERSONALITY PLAN ------- */
/* DISSIDENT SOLDIER */
// SOLDIER PLAN
// If it perceives gold(X,Y), the collector keeps it for itself, 
// and it wont broadcast goldInQuadrant for that gold
+cell(X,Y,gold)
	: class(soldier) & personality(dissident) &
	  .random(R) & R > 0.6
	<-	!dissident_soldier(gold(X,Y)).
	
+!dissident_soldier(gold(X,Y)): class(soldier) & personality(dissident)
	<- +gold(X,Y);
	   .print("*Whistles*").
/* ------- IF OUT OF CHANCE, DO DEFAULT (Next listed plan below) ------- */
     
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
     .drop_desire(handle(G)); 
     //!!choose_gold.
     !!return_to_base.
+picked(gold(X,Y))
  <- -gold(X,Y)[source(_)].
  
+picked(goldInQuadrant(Q))
	: not gold(X,Y) & num_quads(N) & gsize(_,H,W) //& jia.calculate_quadrant(N,H,W,X,Y,Q)
	<- .broadcast(tell,clear(goldInQuadrant(Q))).


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
  :  not free &
     num_quads(N) & gsize(_,H,W) & jia.calculate_quadrant(N,H,W,X,Y,Q)
  <- .print("Handling ",gold(X,Y)," now.");
  	 if( not steal(gold(X,Y)) ){
  		.broadcast(tell, committed_to(gold(X,Y)));
  	 }
     !pos(X,Y);
     !ensure(pick,gold(X,Y));
     .broadcast(tell,picked(gold(X,Y)));
     //.broadcast(tell,picked(goldInQuadrant(Q)));
     +picked(goldInQuadrant(Q));
     ?depot(_,DX,DY);
     !pos(DX,DY);
     !ensure(drop, 0);
     -gold(X,Y)[source(_)]; 
     .print("Finish handling ",gold(X,Y));
     !return_to_base. 
     
// It could happen if some other miner lied about that Aleph location OR if a renegade collector stole the Aleph
-!handle(gold(X,Y)) : gold(X,Y)[source(A)] & A \== self & not cell(X,Y,gold)
  <- .print("there isn't any", gold(X,Y));
  	 .send(leader, tell, log_nothing_here);
  	 .send(leader, tell, picked(gold(X,Y)))
  	 !!return_to_base.
-!handle(G) : G
  <- .print("failed to catch gold ",G);
     .abolish(G);
     !!return_to_base. 
-!handle(G) : true
  <- //.print("failed to handle ",G,", it isn't in the BB anyway");
     !!return_to_base.


+!choose_gold 
  :  not gold(_,_)
  <- -+free.
+!choose_gold 
  :  gold(_,_)
  <- .findall(gold(X,Y),gold(X,Y),LG);
     !calc_gold_distance(LG,LD);
     .length(LD,LLD); LLD > 0;
     .print("Uncommitted gold distances: ",LD,LLD);
     .min(LD,d(_,NewG));
     .print("Next gold is ",NewG);
     !!handle(NewG).
-!choose_gold <- -+free.

+!calc_gold_distance([],[]).

+!calc_gold_distance([gold(GX,GY)|R],[d(D,gold(GX,GY))|RD]) 
  :  pos(IX,IY) & not committed_to(gold(GX,GY))
  <- jia.dist(IX,IY,GX,GY,D);
     !calc_gold_distance(R,RD).
     
+!calc_gold_distance([_|R],RD) 
  <- !calc_gold_distance(R,RD).

// UN SECRETO, OJO!!!
// !pos se usa cuando siempres es posible ir a esa posicón   
// por lo que este plan no debería ser usado: 
// +!pos(X,Y) : last_dir(skip) <- .print("It is not possible to go to ",X,"x",Y).
// En el futuro lo que habría que hacer es:
// +last_dir(skip) <- .drop_goal(pos) 
// Pero bueno, la cosa es que si quiero ir a una posición X,Y y ya estoy en ella: hecho!
+!pos(X,Y) : pos(X,Y) <- .print("I've reached ",X,"x",Y).
+!pos(X,Y) : not pos(X,Y)
  <- !next_step(X,Y);
     !pos(X,Y).



/* ------- PERSONALITY PLAN ------- */
/* RENEGADE COLLECTOR */
// Picks piece of Aleph, and deletes carrying_gold from BB (as if it have failed the action)
@ensreneg[atomic]
+!ensure(pick,_) : pos(X,Y) & cell(X,Y,gold) & class(collector) & personality(renegade) &
	(steal(gold(X,Y)) | (.random(R) & .print(R) & R > 0.8))
  <- do(steal);
  	 .fail. 
/* ------- IF OUT OF CHANCE, DO DEFAULT (Next listed plan below) ------- */

+!ensure(pick,_) : pos(X,Y) & cell(X,Y,gold) & class(collector)
  <- do(pick); ?carrying_gold. 

+!ensure(drop, _) : pos(X,Y) & depot(_,X,Y) & class(collector)
  <- do(drop).





/* PLANS FOR KILLING ENEMIES (SOLDIER) */

/* ------- PERSONALITY PLANS ------- */
/* COWARD SOLDIER */
@pcellacowsol[atomic]
+cell(X,Y,alien)
	: free & class(soldier) & personality(coward) &
    	not ignore(alienInQuadrant(Q)) &
	 	.random(R) & R > 0.6
	<-	!coward_soldier(X,Y).
	
// It tells everybody he killed the enemy, and it starts ignoring the enemy, like it doesn't exist anymore
+!coward_soldier(X,Y)
	: class(soldier) & personality(coward) &
	 num_quads(N) &
     gsize(_,H,W) &
     jia.calculate_quadrant(N,H,W,X,Y,Q)
	<-	.print("I'm going to kill ",alien(X,Y)," now.");
		.wait(200);
		+ignore(alienInQuadrant(Q));
		.broadcast(tell,killed(alien(X,Y)));
		//.broadcast(untell,alienInQuadrant(Q));
		.broadcast(tell,killed(alienInQuadrant(Q)));
    	.print("Finish killing ",alien(X,Y)); 
     	!return_to_base.

//If another miner notifies alienInQuadrant(Q), soldier stops ignoring it, otherwise SOLDIER COULD BE UNCOVERED!
+alienInQuadrant(Q)[source(A)]
  	:	A \== self & ignored(alienInQuadrant(Q)) & class(soldier) & personality(coward)
	<-	-ignored(alienInQuadrant(Q)).
/* ------- IF OUT OF CHANCE, DO DEFAULT (Next listed plan below) ------- */

// If the agent is a soldier and it isn't chasing any enemy
@pcellal[atomic]      
+cell(X,Y,alien) 
  :  free & class(soldier) &
 	 num_quads(N) &
     gsize(_,H,W) &
     jia.calculate_quadrant(N,H,W,X,Y,Q) & 
     not ignore(alienInQuadrant(Q))
  <- -free;
     +alien(X,Y);
     .print("Alien perceived: ",alien(X,Y));
     .broadcast(tell,alienInQuadrant(Q));
     !init_kill(alien(X,Y)).
     
     
// If the alien it's chasing has moved or it founds a closer one
@pcella2[atomic]
+cell(X,Y,alien)
  :  not free & class(soldier) & .desire(kill(alien(OldX,OldY))) &
     pos(AgX,AgY) &
     jia.dist(X,   Y,   AgX,AgY,DNewA) &
     jia.dist(OldX,OldY,AgX,AgY,DOldA) &
     DNewA < DOldA
  <- +alien(X,Y);
     .drop_desire(kill(alien(OldX,OldY)));
     .print("Giving up current alien ",alien(OldX,OldY),
            " to handle ",alien(X,Y)," which I am seeing!");
     .print("Announcing ",alien(OldX,OldY)," to others");
     //.broadcast(tell,alien(OldX,OldY));
     //.broadcast(untell, committed_to(alien(OldX,OldY)));
     !init_kill(alien(X,Y)).
     
/* ------- PERSONALITY PLANS ------- */
/* RENEGADE EXPLORER */
@pcellarenexp[atomic]
+cell(X,Y,alien)
	: class(explorer) & personality(renegade) &
	 	.random(R) & R > 0.7
	<-	!renegade_explorer(X,Y).
	
// It lies and tells everybody is a piece of Aleph instead of alert up about enemies	
+!renegade_explorer(X,Y)
	: class(explorer) & personality(renegade) &
	    num_quads(N) &
    	gsize(_,H,W) &
    	jia.calculate_quadrant(N,H,W,X,Y,Q) &
    	not lied_about(Q)
	<- 	.broadcast(tell, gold(X,Y));
		+alienInQuadrant(Q);
	   	+lied_about(Q).
+!renegade_explorer(X,Y): lied_about(Q) <- true.
/* ------- IF OUT OF CHANCE, DO DEFAULT (Next listed plan below) ------- */
	
// For agents who aren't soldier
+cell(X,Y,alien) 
  :  not alien(X,Y) & not committed_to(alien(X,Y)) &
   	 num_quads(N) &
     gsize(_,H,W) &
     jia.calculate_quadrant(N,H,W,X,Y,Q) &
     not alienInQuadrant(Q) & not ignore(alienInQuadrant(Q))
  <- +alienInQuadrant(Q);
     .print("Announcing ",alienInQuadrant(Q)," to others");
     .broadcast(tell,alienInQuadrant(Q)).

+killed(A)
  <- -A.
+killed(alien(X,Y))
  <- -alien(X,Y)[source(_)].
+killed(alienInQuadrant(Q))
  <- -alienInQuadrant(Q).

 
// First part of init_kill
@pik1[atomic]
+!init_kill(Alien) 
  :  .desire(around(_,_))
  <- .print("Dropping around(_,_) desires and intentions to kill ",Alien);
     .drop_desire(around(_,_));
     !init_kill(Alien).
// Second part of init_kill
@pik2[atomic]
+!init_kill(Alien)
  :  pos(X,Y)
  <- .print("Going for ",Alien);
     -+last_checked(X,Y);
	 // Kill is executed as non-atomic
     !!kill(Alien). 

+!kill(alien(X,Y)) 
  :  not free & alien(X,Y) &
     num_quads(N) & gsize(_,H,W) & jia.calculate_quadrant(N,H,W,X,Y,Q)
  <- .print("I'm going to kill ",alien(X,Y)," now.");
     !pos(X,Y);
     !ensure(kill,alien(X,Y));
     // Broadcasts alien in X Y has been killed
     .broadcast(tell,killed(alien(X,Y)));
     .broadcast(tell,killed(alienInQuadrant(Q)));
     .broadcast(untell,alienInQuadrant(Q));
     -alien(X,Y)[source(_)]; 
     ?base(quadrant(DX,DY,_,_));
     .print("Finish killing ",alien(X,Y)); 
	 !return_to_base.
	 
// If it's failed because enemy have left pos(X,Y)	 
-!kill(alien(X,Y)) : alien(X,Y) & pos(X,Y) & not cell(alien(X,Y))
  <- .print("alien disappeared ",alien(X,Y));
     .abolish(alien(X,Y));
     //+free.
     !!choose_alien.
// If kill action is failed 
-!kill(A) : A
  <- .print("failed to kill alien ",A);
     .abolish(A);
     //+free.
     !!choose_alien.
// If it already forgot about alien(X,Y), because that enemy became not perceptible
-!kill(A) : true
  <- .print("failed to handle ",A,", it isn't in the BB anyway");// +free.
     !!choose_alien.

+!choose_alien 
  :  not alien(_,_)
  <- -+free.
// Chooses between all known enemy positions
+!choose_alien 
  :  alien(_,_)
  <- .findall(alien(X,Y),alien(X,Y),LA);
     !calc_alien_distance(LA,LD);
     .length(LD,LLD); LLD > 0;
     .print("Uncommitted alien distances: ",LD,LLD);
     .min(LD,d(_,NewA));
     .print("Next alien is ",NewA);
     !!kill(NewA).
// If there aren't perceived aliens
-!choose_alien <- -+free.

+!calc_alien_distance([],[]).
+!calc_alien_distance([alien(AX,AY)|R],[d(D,alien(AX,AY))|RD]) 
  :  pos(IX,IY) & not committed_to(alien(AX,AY))
  <- jia.dist(IX,IY,AX,AY,D);
     !calc_alien_distance(R,RD). 
+!calc_alien_distance([_|R],RD) 
  <- !calc_alien_distance(R,RD).

+!ensure(kill,_) : pos(X,Y) & class(soldier) //& cell(X,Y,alien) 
  <- .print("KILLING");
  	do(kill,alien,X,Y).
  
@returnToBase[atomic]
+!return_to_base: quadrant(X1,Y1,X2,Y2) & base(Quadrant)
	<- 	.drop_desire(next_step(_,_));
		.drop_desire(around(_,_));
		-quadrant(X1,Y1,X2,Y2); // It forgets its assigned quad is the current one
	   	+Quadrant; // It believes its quadrant is the base quadrant
	   	-+free. // It already is free and has a quad, so it will prep_around()


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

