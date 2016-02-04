using UnityEngine;
using System.Collections;

/************************************************
 * Purpose: Symbol and its subclasses represent the different types of 
 * 				non-numerical parts of an expression. They are used to 
 * 				define types for the evaluation function.
 * Preconditions: none
 * 
 */
public class Symbol {

	char sign;

	protected Symbol(char c){
		sign = c;
	}

	public override string ToString(){
		return ""+sign;
	}
}
