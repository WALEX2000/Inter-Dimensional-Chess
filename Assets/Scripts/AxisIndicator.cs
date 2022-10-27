namespace Chess.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Chess.Board;
    using Chess.Game;
    using TMPro;

    public class AxisIndicator : MonoBehaviour
    {
        public TextMeshProUGUI axisText;
        public void SwitchAxis(GameObject clickedAxisObj)
        {
            // Change the 4th dimension axis on BoardInterface
            BoardAxis clickedAxis = clickedAxisObj.GetComponent<AxisSelector>().axis;
            GameManager.Instance.boardInterface.SwitchFourthDimensionAxis(clickedAxis);
            // Change the visual representation of the Axis GUI
        }

        private void Update() {
            axisText.text = GameManager.Instance.boardInterface.fourthDimensionValue.ToString();
            // TODO Optimize this so it only updates when the value changes
        }
    }
}