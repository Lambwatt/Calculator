using UnityEngine;
using System.Collections;

public class CalculatorController : MonoBehaviour {

//	private enum CalculatorButton{
//		zero,one,two,three,four,five,six,seven,eight,nine,dot,add,subtract,multiply,divide,equals,clear,open,close
//	};

	private enum InputType{
		blank,operation,number,open,close
	};

	public CalculatorModel m_model;
	public CalculatorView m_view;

	private float m_activeNum = 0.0f;
	private InputType m_lastInput = InputType.blank;
	private bool dotPressed = false;
	private int bracketCount = 0;
	private float powerOfTen = 0.1f;

	public void number(int i){
		if(dotPressed){
			m_activeNum += powerOfTen*(float)i;
			powerOfTen*=0.1f;
		}else{
			m_activeNum = m_activeNum*10 + (float)i;
		}
		m_view.updateDisplay(m_activeNum);
	}

	void Start(){
		Debug.Log ("testing math");
		MathResolver.test();
		CalculationEvaluator.test();
	}

	public void dot(){
		dotPressed = true;
		m_activeNum += 0.0f;
		m_view.updateDisplay(m_activeNum);
	}

	public void operation(Operation o){

	}

	public void openBracket(){

	}

	public void closeBracket(){

	}

	public void equals(){

	}

	public void clear(){
		m_model.clearInput();
		m_model.clearAnswer();
		m_activeNum = 0.0f;
		m_view.updateDisplay(m_activeNum);
	}
}
