using UnityEngine;
using System.Collections;
using System;

public class CalculationEvaluator {
	
	public static float evaluate(object[] expression){

		if(expression.Length==0) 
			return 0.0f;

		else if(expression[0].GetType()==typeof(OpenBracket))
			return evaluate (new object[0], new ArraySegment<object>(expression, 1, expression.Length-1));

		return evaluate (expression[0], new ArraySegment<object>(expression, 1, expression.Length-1));
	}

	private static float evaluate(float f, object[] tail){

		if(expression[0].GetType()==typeof(ProportionalOp))
			return evaluate(f, tail[0], new ArraySegment<object>(tail, 1, tail.Length-1));
		
		return MathResolver.resolve(tail[0], f, evaluate(new ArraySegment<object>(tail, 1, tail.Length-1)));
	}

	private static float evaluate(float f, ProportionalOp op, object[] tail){

		if(expression[0].GetType()==typeof(OpenBracket))
			return MathResolver.resolve(op, f, evaluate(new ArraySegment<object>(tail, 1, tail.Length-1)));

		return evaluate(MathResolver.resolve(op, f, tail[0]), new ArraySegment<object>(tail, 1, tail.Length-1));
	}

	private static float evaluate(object[] acc, object[] tail){
		if(expression[0].GetType()==typeof(CloseBracket))
			return evaluate(evaluate (acc), new ArraySegment<object>(tail, 1, tail.Length-1));
		return evaluate(arr

	}

	public static void test(){
		object[] testArray = new object[]{};
		testArray[0] = 0.0f;
		testArray[1] = new ProportionalOp(Operation.multiply);
		testArray[2] = new OpenBracket();
		testArray[3] = new CloseBracket();

		for(int i = 0; i<testArray.Length; i++){
			Debug.Log("element "+i+" is a "+testArray[i].GetType());
		}

		Debug.Log(typeof(float));
	}
}
