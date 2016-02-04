using UnityEngine;
using System.Collections;

/************************************************
 * Purpose: Library class that handles math operations where operator have been replaced with MathOps
 * Preconditions: none
 * 			
 */
public class MathResolver {
	
	public static float resolve(MathOp op, float a = 0.0f, float b = 0.0f){
		switch(op.getOperation()){
		case Operation.add:
			return add (a,b);
		case Operation.subtract:
			return subtract(a,b);
		case Operation.multiply:
			return multiply(a,b);
		case Operation.divide:
			return divide(a,b);
		default:
			return 0.0f;
		}
	}

	private static float add(float a, float b){
		Debug.Log ("Resolving "+a+" + "+b);
		return a + b;
	}

	private static float subtract(float a, float b){
		Debug.Log ("Resolving "+a+" - "+b);
		return a - b;
	}

	private static float multiply(float a, float b){
		Debug.Log ("Resolving "+a+" * "+b);
		return a * b;
	}

	private static float divide(float a, float b){
		Debug.Log ("Resolving "+a+" / "+b);
		return a/b;
	}

}
