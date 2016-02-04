using UnityEngine;
using System.Collections;

public class AddOp : IncrementalOp {
	public AddOp():base(Operation.add, '+'){}
}
