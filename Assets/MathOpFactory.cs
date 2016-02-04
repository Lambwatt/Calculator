using UnityEngine;
using System.Collections;

/************************************************
 * Purpose: Library class for converting the Operations used in the controller 
 * 				into MathOps used in the CalculatorEvaluator
 * 
 */
public class MathOpFactory{

	public static MathOp createMathOp(Operation op){

		switch(op){
		case Operation.add:
			return new AddOp();
		case Operation.subtract:
			return new SubtractOp();
		case Operation.multiply:
			return new MultiplyOp();
		case Operation.divide:
			return new DivideOp();
		default:
			return null;
		}
	}

}
