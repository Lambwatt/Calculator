using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalculatorView : MonoBehaviour {

	public CalculatorModel model;
	public Text displayText;

	public void updateDisplay(){
		
	}

	public void updateDisplay(float activeNumber, int decimalPlaces = 0){
			displayText.text = ""+activeNumber.ToString("F0"+decimalPlaces);
	}

	public void showAnswer(){

	}
}
