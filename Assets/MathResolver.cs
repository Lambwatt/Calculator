using UnityEngine;
using System.Collections;

//effectively a library to handle math operations
public class MathResolver {

	//applies Operation op to r and l such that the result will be "l <op> r". 
	//NOTE: l and r are swapped in the paramaters of this function so that they can be pulled of a stack as they are entered
	public static float resolve(Operation op, float r = 0.0f, float l = 0.0f){
		switch(op){
		case Operation.add:
			return add (l,r);
		case Operation.subtract:
			return subtract(l,r);
		case Operation.multiply:
			return multiply(l,r);
		case Operation.divide:
			return divide(l,r);
		default:
			return 0.0f;
		}
	}

	private static float add(float a, float b){
		return a + b;
	}

	private static float subtract(float a, float b){
		return a - b;
	}

	private static float multiply(float a, float b){
		return a * b;
	}

	private static float divide(float a, float b){
		return a/b;
	}

	public static void test(){
		Debug.Log (resolve(Operation.add,3.0f, 4.0f));
		Debug.Log (resolve(Operation.subtract,3.0f, 4.0f));
		Debug.Log (resolve(Operation.multiply,3.0f, 4.0f));
		Debug.Log (resolve(Operation.divide,3.0f, 4.0f));
	}
}
