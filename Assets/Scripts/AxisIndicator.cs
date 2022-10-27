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
        public GameObject xAxis, yAxis, zAxis, wAxis;
        public void SwitchAxis(GameObject clickedAxisObj)
        {
            // Change the 4th dimension axis on BoardInterface
            BoardAxis clickedAxis = clickedAxisObj.GetComponent<AxisSelector>().axis;
            GameManager.Instance.boardInterface.SwitchFourthDimensionAxis(clickedAxis);

            // Update the Axis Indicator
            Color wAxisColorOld = wAxis.GetComponent<MeshRenderer>().material.color;
            BoardAxis wAxisValueOld = wAxis.GetComponent<AxisSelector>().axis;

            Color clickedColor = clickedAxisObj.GetComponent<MeshRenderer>().material.color;
            wAxis.GetComponent<MeshRenderer>().material.color = clickedColor;
            wAxis.GetComponent<AxisSelector>().axis = clickedAxis;
            
            if (clickedAxis == BoardAxis.W || wAxisValueOld == BoardAxis.W) { // Everything is normal
                clickedAxisObj.GetComponent<AxisSelector>().axis = wAxisValueOld;
                clickedAxisObj.GetComponent<MeshRenderer>().material.color = wAxisColorOld; // Whichever color it was before
            } else if (wAxisValueOld == BoardAxis.X) { // W was X, so clickedAxis is Y or Z
                Color wColor = xAxis.GetComponent<MeshRenderer>().material.color;
                clickedAxisObj.GetComponent<AxisSelector>().axis = BoardAxis.W;
                clickedAxisObj.GetComponent<MeshRenderer>().material.color = wColor; // Whichever color it was before
                xAxis.GetComponent<AxisSelector>().axis = wAxisValueOld;
                xAxis.GetComponent<MeshRenderer>().material.color = wAxisColorOld;
            } else if (wAxisValueOld == BoardAxis.Y) {
                Color wColor = yAxis.GetComponent<MeshRenderer>().material.color;
                clickedAxisObj.GetComponent<AxisSelector>().axis = BoardAxis.W;
                clickedAxisObj.GetComponent<MeshRenderer>().material.color = wColor; // Whichever color it was before
                yAxis.GetComponent<AxisSelector>().axis = wAxisValueOld;
                yAxis.GetComponent<MeshRenderer>().material.color = wAxisColorOld;
            } else if (wAxisValueOld == BoardAxis.Z) {
                Color wColor = zAxis.GetComponent<MeshRenderer>().material.color;
                clickedAxisObj.GetComponent<AxisSelector>().axis = BoardAxis.W;
                clickedAxisObj.GetComponent<MeshRenderer>().material.color = wColor; // Whichever color it was before
                zAxis.GetComponent<AxisSelector>().axis = wAxisValueOld;
                zAxis.GetComponent<MeshRenderer>().material.color = wAxisColorOld;
            }
        }

        private void Update() {
            axisText.text = GameManager.Instance.boardInterface.fourthDimensionValue.ToString();
            // TODO Optimize this so it only updates when the value changes
        }
    }
}