package jia;

import jason.asSemantics.DefaultInternalAction;
import jason.asSemantics.TransitionSystem;
import jason.asSemantics.Unifier;
import jason.asSyntax.Atom;
import jason.asSyntax.NumberTerm;
import jason.asSyntax.Term;

public class calculate_quadrant extends DefaultInternalAction{
	 
    @Override
    public Object execute(TransitionSystem ts, Unifier un, Term[] terms) throws Exception {
        int nQuads = (int)((NumberTerm)terms[0]).solve();
        int sqQuads = (int) Math.sqrt(nQuads);
        int h = (int)((NumberTerm)terms[1]).solve();
        int w = (int)((NumberTerm)terms[2]).solve();
        int x = (int)((NumberTerm)terms[3]).solve();
        int y = (int)((NumberTerm)terms[4]).solve();
        
        int xQ = x/(h/sqQuads);
        int yQ = y/(w/sqQuads);
        
        String result;
        result = Integer.toString((yQ*sqQuads)+xQ);
    	return un.unifies(terms[5], new Atom(result));
    }
}
