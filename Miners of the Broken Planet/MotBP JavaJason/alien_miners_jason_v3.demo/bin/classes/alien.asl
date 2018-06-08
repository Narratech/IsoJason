/* CREENCIAS */

last_dir(null). // El último movimiento que hice (al principio es un null, claro)
free. // Al principio estoy desocupado, sin nada que hacer
//main(quadrant(0,0,35,35)).
//quadrant(0,0,35,35).

/* REGLAS */

+dead : .my_name(N) <- .kill_agent(N).

calc_new_y(AgY,QuadY2,QuadY2) :- AgY+2 > QuadY2.
calc_new_y(AgY,_,Y) :- Y = AgY+2.

	 	 
/* PLANES PARA MERODEAR POR MI CUADRANTE CUANDO ESTOY DESOCUPADO */


+free : last_checked(X,Y)     <- !prep_around(X,Y).

+free : quadrant(X1,Y1,X2,Y2) <- !prep_around(X1,Y1).

+free : true                  <- !wait_for_quad.

+main(Q) <- +Q.

+quadrant(X1,Y1,X2,Y2)[source(leader)]: main(Q)
	<- .abolish(quadrant(X1,Y1,X2,Y2));
		+Q.

@pwfq[atomic]
+!wait_for_quad : free & quadrant(_,_,_,_) 
   <- -+free.

+!wait_for_quad : free     
   <- .wait("+quadrant(X1,Y1,X2,Y2)", 500); 
      !!wait_for_quad.

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
      do(D).

+!next_step(X,Y) : not pos(_,_)  
   <- !next_step(X,Y).

-!next_step(X,Y) : true
   <- .print("Failed next_step to ", X,"x",Y," fixing and trying again!");
      -+last_dir(null);
      !next_step(X,Y).
      
+cell(X,Y,ally) 
  :  free
  <- -free;
     +ally(X,Y);
     .print("Enemy perceived: ",ally(X,Y));
     !init_kill(ally(X,Y)).
+cell(X,Y,ally)
  :  not free & class(soldier) & .desire(kill(ally(OldX,OldY))) &
     pos(AgX,AgY) &
     jia.dist(X,   Y,   AgX,AgY,DNewA) &
     jia.dist(OldX,OldY,AgX,AgY,DOldA) &
     DNewA < DOldA
  <- +ally(X,Y);
     .drop_desire(kill(ally(OldX,OldY)));
     .print("Giving up current ally ",ally(OldX,OldY),
            " to handle ",ally(X,Y)," which I am seeing!");
     .print("Announcing ",ally(OldX,OldY)," to others");
     !init_kill(ally(X,Y)).

@pik1[atomic]
+!init_kill(Ally) 
  :  .desire(around(_,_))
  <- .print("Dropping around(_,_) desires and intentions to kill ",Ally);
     .drop_desire(around(_,_));
     !init_kill(Ally).

@pik2[atomic]
+!init_kill(Ally)
  :  pos(X,Y)
  <- .print("Going for ",Ally);
     -+last_checked(X,Y);
     !!kill(Ally).
     
+!kill(ally(X,Y)) 
  :  not free
  <- .print("I'm going to kill ",ally(X,Y)," now.");
     !pos(X,Y);
     !ensure(kill,ally(X,Y));
     .abolish(ally(_,_)); 
     .print("Finish killing ",ally(X,Y)); 
	 -+free.
	 
-!kill(ally(X,Y)) : ally(X,Y) & pos(X,Y) & not cell(ally(X,Y))
  <- .print("ally disappeared ",ally(X,Y));
     .abolish(ally(X,Y));
     -+free.	 
-!kill(A) : A
  <- .print("failed to kill ally ",A);
     .abolish(A);
     .drop_desire(kill(A));
     -+free.
-!kill(A) : true
  <- .print("failed to handle ",A,", it isn't in the BB anyway");
     .drop_desire(kill(A));
     -+free.
     
+!pos(X,Y) : pos(X,Y) <- .print("I've reached ",X,"x",Y).

+!pos(X,Y) : not pos(X,Y)
  <- !next_step(X,Y);
     !pos(X,Y).
     
+!ensure(kill,_) : pos(X,Y) & cell(X,Y,alien)
  <- do(kill,ally,X,Y).

/* FINAL DE LA SIMULACIÓN */

+end_of_simulation(S,_) : true 
  <- .drop_all_desires; 
     .abolish(quadrant(_,_,_,_));
     -+free;
     .print("-- END ",S," --").

