using UnityEngine;
using System.Collections;

public class MathOp : Symbol {

	private Operation operation {get; set;}

	protected MathOp(/*int i, */Operation op)/*:base(i)*/{
		operation = op;
	}
}
