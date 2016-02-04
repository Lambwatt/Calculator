using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/************************************************
 * Purpose: Evaluates a valid list of floats and Symbols as an arithmetic expression
 * Preconditions: expression must be valid and complete: 
 * 					-	all operators have right and left hand terms
 * 					-	all brackets come in matching pairs
 * 			
 * NOTE: These functions are written in the style of functional programming, in particular PROLOG
 * 
 * ADDITIONAL NOTE: I couldn't find classifications of operators that grouped addition with subtraction and multiplication with division,
 * 						so I made some up. Addition and subtraction are called "incremental operations". Multiplication and division
 * 						are called "proportional operations."
 * 
 */
public class CalculationEvaluator {

	//Public entry function. All evaluations start here. If the expression is empty, return zero.
	//If the new operand is the start of a bracketed term, continue evaluating with an accumulator to collect the terms inside the brackets.
	//Otherwise, peel off the first number and continue evaluating.
	public static float evaluate(List<object> expression){
		if(expression.Count==0)
			return 0.0f;
		else if(expression[0].GetType()==typeof(OpenBracket))
			return evaluate (new List<object>(), expression.GetRange(1, expression.Count-1));
		return evaluate ((float)expression[0], expression.GetRange(1, expression.Count-1));
	}


	//Return the float if the expression has been completely evaluated. Otherwise continue evaluating based on the next operator
	private static float evaluate(float f, List<object> tail){
		if(tail.Count==0)
			return f;
		if(tail[0].GetType().IsSubclassOf(typeof(ProportionalOp)))
			return evaluate(f, (ProportionalOp)tail[0], tail.GetRange(1, tail.Count-1));
		else 																				//we still expect an operator, so it must be an incremental
			return evaluate(f, (IncrementalOp)tail[0], tail.GetRange(1, tail.Count-1));
	}


	//If the new operand is the start of a bracketed term, continue evaluating with an accumulator to collect the terms inside the brackets.
	//Otherwise, apply the multiplication/division operation now to ensure left evaluation.
	private static float evaluate(float f, ProportionalOp op, List<object> tail){
		if(tail[0].GetType()==typeof(OpenBracket))
			return evaluate(f, op, new List<object>(), tail.GetRange(1, tail.Count-1));
		return evaluate(MathResolver.resolve((MathOp)op, f, (float)tail[0]), tail.GetRange(1, tail.Count-1));
	}


	//If the new operand is the start of a bracketed term, continue evaluating with an accumulator to collect the terms inside the brackets.
	//Otherwise, peel of the second operand for the addition/subtraction operator and continue evaluating.
	private static float evaluate(float f, IncrementalOp op, List<object> tail){
		if(tail[0].GetType()==typeof(OpenBracket))
			return evaluate(f, op, new List<object>(), tail.GetRange(1, tail.Count-1));
		return evaluate(f, op, (float)tail[0], tail.GetRange(1, tail.Count-1));
	}


	//If the end of an expression has been reached reolve the current addition/subtraction and return the result.
	//If not resolve the current addition and subtraction if the next operation is of equal precedence.
	//Otherwise continue evaluating.
	private static float evaluate(float f1, IncrementalOp op, float f2, List<object> tail){
		if(tail.Count==0)
			return MathResolver.resolve((MathOp)op, f1, f2);
		else if(tail[0].GetType().IsSubclassOf(typeof(ProportionalOp)))
			return evaluate(f1, op, f2, (ProportionalOp)tail[0], tail.GetRange(1, tail.Count-1));
		else  																						//we still expect an operator, so it must be an incremental
			return evaluate(MathResolver.resolve((MathOp)op, f1, f2), (IncrementalOp)tail[0], tail.GetRange(1, tail.Count-1));
	}


	//If the new operand is the start of a bracketed term, continue evaluating with an accumulator to collect the terms inside the brackets.
	//Otherwise, apply the multiplication/division operation now to ensure left evaluation and continue evaluation with the incomplete addition/subtraction in tow.
	private static float evaluate(float f1, IncrementalOp op1, float f2, ProportionalOp op2, List<object> tail){
		if(tail[0].GetType()==typeof(OpenBracket))
			return evaluate(f1, op1, f2, op2, new List<object>(), tail.GetRange(1, tail.Count-1));
		
		return evaluate(f1, op1, MathResolver.resolve((MathOp)op2, f2, (float)tail[0]), tail.GetRange(1, tail.Count-1));
	}


	//Accumulate all terms in acc unless they are the matching bracket from the start of the accumulator.
	//If the matching bracket is found, evaluate the bracketed term now fully contained in acc.
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


	//Accumulate all terms in acc unless they are the matching bracket from the start of the accumulator.
	//If the matching bracket is found, resolve the multiplication/division using the evaluation of the bracketed term now fully contained in acc as the right operand.
	//Resolving this now ensures left evaluation is maintained.
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


	//Accumulate all terms in acc unless they are the matching bracket from the start of the accumulator.
	//If the matching bracket is found, continue the evaluation using the evaluation of the bracketed term now fully contained in acc 
	//as the right operand of the open addition/subtraction. Resolving this now ensures left evaluation is maintained.
	public static float evaluate(float f, IncrementalOp op, List<object> acc, List<object> tail, int brackets = 0){
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



	//Accumulate all terms in acc unless they are the matching bracket from the start of the accumulator.
	//If the matching bracket is found, continue the evaluation using the evaluation of the bracketed term now fully contained in acc 
	//as the right operand of the open multiplcation/division. Resolving this now ensures left evaluation is maintained.
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
}
