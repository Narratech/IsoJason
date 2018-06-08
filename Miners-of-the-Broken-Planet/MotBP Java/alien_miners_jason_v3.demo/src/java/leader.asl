/* Se encarga de ubicar a los mineros por cuadrantes del escenario */

@quads[atomic]
+gsize(S,W,H) : true
  <- // calculo del árae de cada cuadrante y añadir a mi memoria (creencias +)
     .print("Defining quadrants for ",W,"x",H," simulation ",S);
     +quad(S,0, 0, 0, 11, 11);
     +quad(S,1, 12, 0, 23, 1);
	 +quad(S,2, 24, 0, 35, 11);
     +quad(S,3, 0, 12, 11, 23);
     +quad(S,4, 12, 12, 23, 23);
     +quad(S,5, 24, 12, 35, 23);
     +quad(S,6, 0, 24, 11, 35);
     +quad(S,7, 12, 24, 23, 35);
     +quad(S,8, 24, 24, 35, 35);
     +base(quadrant(12,12,23,23));
     .print("Finished all quadrs for ",S);
     !assign_enemy_quad(alien1, main(quadrant(0, 0, 11, 11)));
     !assign_enemy_quad(alien2, main(quadrant(24, 0, 35, 11)));
     !assign_base_quad(base(quadrant(12,12,23,23))).

+!assign_enemy_quad(E,Q): true
	<- .send(E,tell,Q).	 
+!assign_base_quad(base(Quadrant)): true
	<- .broadcast(tell,Quadrant);
	   .broadcast(tell,base(Quadrant)).


/* Temas de negociación con el oro encontrado (los mineros pujan con la distancia 
a la que están, y al que le pille más cerca, se lo lleva.. es como los taxis) 
Aquí gana el que "la tiene más corta" :-P */
/* 
+bid(gold(X,Y),D,Ag)
  :  .count(bid(gold(X,Y),_,_),2)  
  <- .print("bid from ",Ag," for ",gold(X,Y)," is ",D);
     !allocate_miner(gold(X,Y)); //Asignaré ese oro a quien yo vea mejor
     .abolish(bid(gold(X,Y),_,_)). //A olvidarme de esas pujas
+bid(gold(X,Y),D,Ag)
  <- .print("bid from ",Ag," for ",gold(X,Y)," is ",D).
  
  
+bid(alien(X,Y),D,Ag)
  :  .count(bid(alien(X,Y),_,_),1)  
  <- .print("bid from ",Ag," for ",alien(X,Y)," is ",D);
     !allocate_miner(alien(X,Y)); //Asignaré ese oro a quien yo vea mejor
     .abolish(bid(alien(X,Y),_,_)). //A olvidarme de esas pujas
+bid(alien(X,Y),D,Ag)
  <- .print("bid from ",Ag," for ",alien(X,Y)," is ",D).
 

+!allocate_miner(gold(X,Y)) 
  //op(Dist,A) creo que simplemente es una estructura de pareja distancia-minero
  <- .findall(op(Dist,A),bid(Gold,Dist,A),LD); //saco todas esas parejas
     .min(LD,op(DistCloser,Closer)); // me quedo con la distancia mínima
     DistCloser < 10000; // Estos mineros no me valen, que no pueden recoger más!
     .print("Gold ",gold(X,Y)," was allocated to ",Closer, " options were ",LD);
     .broadcast(tell,allocated(gold(X,Y),Closer)). //broadcast es a todos los mineros!
-!allocate_miner(gold(X,Y))
  <- .print("could not allocate gold ",gold(X,Y)).
     
     
+!allocate_miner(alien(X,Y)) 
  //op(Dist,A) creo que simplemente es una estructura de pareja distancia-minero
  <- .findall(op(Dist,A),bid(Gold,Dist,A),LD); //saco todas esas parejas
     .min(LD,op(DistCloser,Closer)); // me quedo con la distancia mínima
     DistCloser < 10000; // Estos mineros no me valen, que no pueden recoger más!
     .print("Alien ",alien(X,Y)," was allocated to ",Closer, " options were ",LD);
     .broadcast(tell,allocated(alien(X,Y),Closer)). //broadcast es a todos los mineros!
-!allocate_miner(alien(X,Y))
  <- .print("could not allocate alien ",alien(X,Y)).

*/
/* El plan para cuando se termina la simulación */     
@end[atomic]
+end_of_simulation(S,_) : true 
  <- .print("-- END ",S," --");
     .abolish(init_pos(S,_,_)). 

