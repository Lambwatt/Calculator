using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CalculatorPDA {

	public static float evaluate(List<object> inputList){
		Stack<Symbol> oStack = new Stack<Symbol>();
		Stack<float> nStack = new Stack<float>();

		return evaluate(inputList.GetEnumerator(), oStack, nStack);
	}

	private static float evaluate(IEnumerator<object> iE, Stack<Symbol> oStack, Stack<float> nStack){
		if(iE.MoveNext()){

			if(iE.Current.GetType() == typeof(float))
				return evaluateNumber(iE, oStack, nStack);

			else if(iE.Current.GetType() == typeof(Operation))
			    return evaluateOperation(iE, oStack, nStack);

			else if(iE.Current.GetType() == typeof(OpenBracket))
				return evaluateOpenBracket(iE, oStack, nStack);

			else if(iE.Current.GetType() == typeof(CloseBracket))
				return evaluateCloseBracket(iE, oStack, nStack);

			else{
				Debug.Log ("Throw an exception because this is not a valid data type for this class.");
				return 0.0f;
			}

		}else{

			return evaluateEnd(oStack, nStack);

		}
	}

	private static float evaluateEnd(Stack<Symbol> oStack, Stack<float> nStack){
		return 0.0f;
	}

	private static float evaluateOpenBracket(IEnumerator<object> iE, Stack<Symbol> oStack, Stack<float> nStack){
		return 0.0f;
	}

	private static float evaluateCloseBracket(IEnumerator<object> iE, Stack<Symbol> oStack, Stack<float> nStack){
		return 0.0f;
	}

	private static float evaluateNumber(IEnumerator<object> iE, Stack<Symbol> oStack, Stack<float> nStack){
		nStack.Push(Convert.ToSingle(iE.Current));
		return 0.0f;
	}

	private static float evaluateOperation(IEnumerator<object> iE, Stack<Symbol> oStack, Stack<float> nStack){
		return 0.0f;
	}

}
