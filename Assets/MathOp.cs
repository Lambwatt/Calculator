using UnityEngine;
using System.Collections;

public class MathOp : Symbol {

	private Operation operation;

	protected MathOp(Operation op, char c):base(c){
		operation = op;
	}

	public Operation getOperation(){
		return operation;
	}
}
