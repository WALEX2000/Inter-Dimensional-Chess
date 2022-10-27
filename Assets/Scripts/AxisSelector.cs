namespace Chess.Interface
{
    using UnityEngine;
    using Chess.Board;

    public class AxisSelector : MonoBehaviour
    {
        public BoardAxis axis;
        private AxisIndicator axisIndicator;
        private void Start()
        {
            axisIndicator = this.GetComponentInParent<AxisIndicator>();
        }
        private void OnMouseDown()
        { // Click on Axis
            axisIndicator.SwitchAxis(gameObject);
        }
    }
}