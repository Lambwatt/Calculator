using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalculatorModel : MonoBehaviour {

	private float answer;
	private List<Object> input;

	// Use this for initialization
	void Start () {
		input = new List<Object>();
		answer = 0.0f;
	}
	
	public void append(Object o){}

	public void appendAnswer(){}

	public List<Object> getInput(){
		return input;
	}

	public float getAnswer(){
		return answer;
	}

	public void setAnswer(){}

	public void clearAnswer(){}

	public void clearInput(){}
}
