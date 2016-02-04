using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/************************************************
 * Purpose: Displays data to CalculatorModel in a text field
 * Preconditions: m_model and m_displayText must be populated
 * 
 * 			
 */
public class CalculatorView : MonoBehaviour {

	public CalculatorModel m_model;
	public Text m_displayText;

	public void updateDisplay(){
		m_displayText.text = "";

		foreach(object o in m_model.getInput()){
			m_displayText.text+=o.ToString();
		}
	}

	public void updateDisplay(float activeNumber, int decimalPlaces = 0){
		updateDisplay();
		m_displayText.text = m_displayText.text+activeNumber.ToString("F0"+decimalPlaces);
	}

	public void showAnswer(){
		m_displayText.text = ""+m_model.getAnswer();
	}
}
