using UnityEngine;
using System.Collections;

public class Symbol {

	char sign;

	protected Symbol(char c){
		sign = c;
	}

	public override string ToString(){
		return ""+sign;
	}
}
