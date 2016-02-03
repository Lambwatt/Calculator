using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalculationEvaluator {
	
	public static float evaluate(List<object> expression){
		if(expression.Count==0) {
			Debug.Log ("Stopped here.");
			return 0.0f;
		}
		else if(expression[0].GetType()==typeof(OpenBracket))
			return evaluate (new List<object>(), expression.GetRange(1, expression.Count-1));
		return evaluate ((float)expression[0], expression.GetRange(1, expression.Count-1));
	}


	private static float evaluate(float f, List<object> tail){
		if(tail.Count==0){
			Debug.Log ("Stopped here.");
			return f;
		}
		if(tail[0].GetType().IsSubclassOf(typeof(ProportionalOp)))
			return evaluate(f, (ProportionalOp)tail[0], tail.GetRange(1, tail.Count-1));
		else 																				//we still expect an operator, so it must be an incremental
			return evaluate(f, (IncrementalOp)tail[0], tail.GetRange(1, tail.Count-1));
	}


	private static float evaluate(float f, ProportionalOp op, List<object> tail){
		if(tail[0].GetType()==typeof(OpenBracket))
			return evaluate(f, op, new List<object>(), tail.GetRange(1, tail.Count-1));

		return evaluate(MathResolver.resolve((MathOp)op, f, (float)tail[0]), tail.GetRange(1, tail.Count-1));
	}


	private static float evaluate(float f, IncrementalOp op, List<object> tail){
		if(tail[0].GetType()==typeof(OpenBracket))
			return evaluate(f, op, new List<object>(), tail.GetRange(1, tail.Count-1));
		
		return evaluate(f, op, (float)tail[0], tail.GetRange(1, tail.Count-1));
	}


	private static float evaluate(float f1, IncrementalOp op, float f2, List<object> tail){
		if(tail.Count==0 || tail[0].GetType()==typeof(CloseBracket)){
			Debug.Log ("Stopped here.");
			return MathResolver.resolve((MathOp)op, f1, f2);
		} 
		else if(tail[0].GetType().IsSubclassOf(typeof(ProportionalOp)))
			return evaluate(f1, op, f2, (ProportionalOp)tail[0], tail.GetRange(1, tail.Count-1));
		else  																									//we still expect an operator, so it must be an incremental
			return evaluate(MathResolver.resolve((MathOp)op, f1, f2), (IncrementalOp)tail[0], tail.GetRange(1, tail.Count-1));
	}


	private static float evaluate(float f1, IncrementalOp op1, float f2, ProportionalOp op2, List<object> tail){
		if(tail[0].GetType()==typeof(OpenBracket))
			return evaluate(f1, op1, f2, op2, new List<object>(), tail.GetRange(1, tail.Count-1));
		
		return evaluate(f1, op1, MathResolver.resolve((MathOp)op2, f2, (float)tail[0]), tail.GetRange(1, tail.Count-1));
	}


	private static float evaluate(List<object> acc, List<object> tail, int brackets = 0){
		if(tail[0].GetType()==typeof(CloseBracket)){
			if(brackets==0)
				return evaluate(evaluate (acc), tail.GetRange(1, tail.Count-1));
			else{
				acc.Add(tail[0]);
				return evaluate(acc, tail.GetRange(1, tail.Count-1), brackets-1);
			}
		}
		else if(tail[0].GetType()==typeof(OpenBracket)){
			acc.Add(tail[0]);
			return evaluate(acc, tail.GetRange(1, tail.Count-1), brackets+1);
		}
		acc.Add(tail[0]);
		return evaluate(acc, tail.GetRange(1, tail.Count-1), brackets);
	}


	public static float evaluate(float f, ProportionalOp op, List<object> acc, List<object> tail, int brackets = 0){
		if(tail[0].GetType()==typeof(CloseBracket)){
			if(brackets==0)
				return evaluate(MathResolver.resolve(op, f, evaluate(acc)), tail.GetRange(1, tail.Count-1));
			else{
				acc.Add(tail[0]);
				return evaluate(f, op, acc, tail.GetRange(1, tail.Count-1), brackets-1);
			}
		}else if(tail[0].GetType()==typeof(OpenBracket)){
			acc.Add(tail[0]);
			return evaluate(f, op, acc, tail.GetRange(1, tail.Count-1), brackets+1);
		}
		acc.Add(tail[0]);
		return evaluate(f, op, acc, tail.GetRange(1, tail.Count-1), brackets);
	}


	public static float evaluate(float f, IncrementalOp op, List<object> acc, List<object> tail, int brackets = 0){
		Debug.Log (tail[0]+" "+tail.Count);
		if(tail[0].GetType()==typeof(CloseBracket)){
			if(brackets==0)
				return evaluate(f, op, evaluate(acc), tail.GetRange(1, tail.Count-1));
			else{
				acc.Add(tail[0]);
				return evaluate(f, op, acc, tail.GetRange(1, tail.Count-1), brackets-1);
			}
		}else if(tail[0].GetType()==typeof(OpenBracket)){
			acc.Add(tail[0]);
			return evaluate(f, op, acc, tail.GetRange(1, tail.Count-1), brackets+1);
		}
		acc.Add(tail[0]);
		return evaluate(f, op, acc, tail.GetRange(1, tail.Count-1), brackets);
	}


	public static float evaluate(float f1, IncrementalOp op1, float f2, ProportionalOp op2, List<object> acc, List<object> tail, int brackets = 0){
		if(tail[0].GetType()==typeof(CloseBracket)){
			if(brackets==0)
				return evaluate(f1, op1, MathResolver.resolve(op2, f2, evaluate(acc)), tail.GetRange(1, tail.Count-1));
			else{
				acc.Add(tail[0]);
				return evaluate(f1, op1, f2, op2, acc, tail.GetRange(1, tail.Count-1), brackets-1);
			}
		}else if(tail[0].GetType()==typeof(OpenBracket)){
			acc.Add(tail[0]);
			return evaluate(f1, op1, f2, op2, acc, tail.GetRange(1, tail.Count-1), brackets+1);
		}
		acc.Add(tail[0]);
		return evaluate(f1, op1, f2, op2, acc, tail.GetRange(1, tail.Count-1), brackets);
	}


	public static void test(){
		List<object> testList = new List<object>();
		Debug.Log("evaluation of empty list = "+evaluate(testList));


		testList.Add(1.0f);
		testList.Add (new AddOp());
		testList.Add(1.0f);
		Debug.Log("1 + 1 = "+evaluate(testList));

		testList.Clear();

		testList.Add(1000.0f);
		testList.Add(new AddOp());
		testList.Add(200.0f);
		testList.Add(new SubtractOp());
		testList.Add(3.0f);
		testList.Add(new MultiplyOp());
		testList.Add(10.0f);
		testList.Add(new AddOp());
		testList.Add(new OpenBracket());
		testList.Add(16.0f);
		testList.Add(new SubtractOp());
		testList.Add(new OpenBracket());
		testList.Add(2.0f);
		testList.Add(new MultiplyOp());
		testList.Add(2.0f);
		testList.Add(new DivideOp());
		testList.Add(0.5f);
		testList.Add(new CloseBracket());
		testList.Add(new CloseBracket());
		testList.Add(new AddOp());
		testList.Add(1.0f);
		testList.Add(new DivideOp());
		testList.Add(2.0f);
		testList.Add(new AddOp());
		testList.Add(1.0f);

		Debug.Log("real test = "+evaluate(testList));

	}
	
}
