using UnityEngine;
using System.Collections;
using System;

/************************************************
 * Purpose: Inputs data to CalculatorModel and calls display function in the CalculatorView
 * Preconditions: m_model and m_view must be populated
 * 
 * NOTE: As this is the only function with links to the evaluate() function, it is responsible 
 * 			for ensuring the data stored model is valid when the '=' button is pressed.
 * 			
 */
public class CalculatorController : MonoBehaviour {

	private enum InputType{
		blank, 
		operation, 
		number, 
		open, 
		close, 
		answer
	};

	public CalculatorModel m_model;
	public CalculatorView m_view;

	private float m_activeNum = 0.0f;
	private InputType m_lastInput = InputType.blank;
	private bool m_dotPressed = false;
	private int m_bracketCount = 0;
	private float m_powerOfTen = 0.1f;
	private int m_decimalPlaces = 0;

	private void endNumber(){
		m_model.append(m_activeNum);
		m_activeNum = 0.0f;
		m_dotPressed = false;
		m_powerOfTen = 0.1f;
		m_decimalPlaces = 0;
	}

	public void number(int i){
		if(m_dotPressed){
			m_activeNum += m_powerOfTen*(float)i;
			m_decimalPlaces++;
			m_powerOfTen*=0.1f;
		}else{
			m_activeNum = m_activeNum*10 + (float)i;
		}
		m_view.updateDisplay(m_activeNum, m_decimalPlaces);
		m_lastInput = InputType.number;
	}

	public void dot(){
		m_dotPressed = true;
		m_activeNum += 0.0f;
		m_view.updateDisplay(m_activeNum);

		m_lastInput = InputType.number;
	}

	public void operation(string opString){

		Operation op  = (Operation)System.Enum.Parse( typeof( Operation ), opString );

		switch(m_lastInput){
		case InputType.answer:										
			m_model.appendAnswer();									//Use answer as first entry
			m_model.append((object)MathOpFactory.createMathOp(op));
			break;
		case InputType.blank:
		case InputType.open:
			if(op == Operation.subtract){						
				m_model.append(0.0f);							//add 0, then subtract to create a negative number
				m_model.append(MathOpFactory.createMathOp(op));
			}else{
				return;
			}
			break;
		case InputType.number:
			endNumber();
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
		if(!(m_lastInput==InputType.number)){
			m_model.append(new OpenBracket());
			m_bracketCount++;

			m_lastInput = InputType.open;
			m_view.updateDisplay();
		}
	}

	public void closeBracket(){
		if(!(m_bracketCount == 0 || m_lastInput==InputType.operation)){
			if(m_lastInput==InputType.number){
				endNumber();
			}

			m_bracketCount--;
			m_model.append(new CloseBracket());

			m_lastInput = InputType.close;
			m_view.updateDisplay();
		}
	}

	public void equals(){
		if(!(m_bracketCount>0 || m_lastInput==InputType.operation)){
			if(m_lastInput==InputType.number){
				endNumber();
			}
			m_model.setAnswer(CalculationEvaluator.evaluate(m_model.getInput()));
			m_model.clearInput();

			m_lastInput = InputType.answer;
			m_view.showAnswer();
		}
	}

	public void clear(){
		m_model.clearInput();
		m_model.clearAnswer();
		m_activeNum = 0.0f;
		m_view.updateDisplay(0.0f);
		m_lastInput = InputType.blank;
	}
}
