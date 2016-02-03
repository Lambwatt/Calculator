using UnityEngine;
using System.Collections;

public class MathOp : Symbol {

	private Operation operation;

	protected MathOp(/*int i, */Operation op)/*:base(i)*/{
		operation = op;
	}

	public Operation getOperation(){
		return operation;
	}
}
