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
	private bool m_dotPressed = false;
	private int m_bracketCount = 0;
	private float m_powerOfTen = 0.1f;

	public void number(int i){
		if(m_dotPressed){
			m_activeNum += m_powerOfTen*(float)i;
			m_powerOfTen*=0.1f;
		}else{
			m_activeNum = m_activeNum*10 + (float)i;
		}
		m_view.updateDisplay(m_activeNum);
		m_lastInput = InputType.number;
	}

	void Start(){
		Debug.Log ("testing math");
		CalculationEvaluator.test();
	}

	public void dot(){
		m_dotPressed = true;
		m_activeNum += 0.0f;
		m_view.updateDisplay(m_activeNum);

		m_lastInput = InputType.number;
		m_view.updateDisplay();
	}

	public void operation(Operation op){

		switch(m_lastInput){
		case InputType.blank:
			//Use answer as first entry
			m_model.appendAnswer();
			m_model.append((object)MathOpFactory.createMathOp(op));
			break;
		case InputType.open:
			if(op == Operation.subtract){
				//add 0, then subtract to create a negative number
				m_model.append(0.0f);
				m_model.append(MathOpFactory.createMathOp(op));
			}
			break;
		case InputType.number:
			m_model.append(m_activeNum);
			m_model.append(MathOpFactory.createMathOp(op));
			break;
		case InputType.operation:
			return;
		case InputType.close:
			m_model.append(MathOpFactory.createMathOp(op));
			break;
		default:
			return;
		}

		m_lastInput = InputType.operation;
		m_view.updateDisplay();
	}

	public void openBracket(){
		m_model.append(new OpenBracket());
		m_bracketCount++;

		m_lastInput = InputType.open;
		m_view.updateDisplay();
	}

	public void closeBracket(){
		if(!(m_bracketCount == 0 || m_lastInput==InputType.operation)){
			m_bracketCount--;
			m_model.append(new CloseBracket());

			m_lastInput = InputType.close;
			m_view.updateDisplay();
		}
	}

	public void equals(){
		if(!(m_bracketCount>0 || m_lastInput==InputType.operation)){
			m_model.setAnswer();
			m_model.clearInput();

			m_lastInput = InputType.blank;
			m_view.updateDisplay();
		}
	}

	public void clear(){
		m_model.clearInput();
		m_model.clearAnswer();
		m_activeNum = 0.0f;
		m_view.updateDisplay(m_activeNum);
	}
}
