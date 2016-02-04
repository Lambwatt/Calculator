using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalculatorModel : MonoBehaviour {

	private float m_answer;
	private List<object> m_input;

	// Use this for initialization
	void Start () {
		m_input = new List<object>();
		m_answer = 0.0f;
	}
	
	public void append(object o){
		m_input.Add(o);
	}

	public void appendAnswer(){
		m_input.Add(m_answer);
	}

	public List<object> getInput(){
		return m_input;
	}

	public float getAnswer(){
		return m_answer;
	}

	public void setAnswer(){
		m_answer = CalculationEvaluator.evaluate(m_input);
	}

	public void clearAnswer(){
		m_answer = 0.0f;
	}

	public void clearInput(){
		m_input.Clear();
	}
}
