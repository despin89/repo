namespace GD.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeColorByScale : MonoBehaviour
    {
        #region Fields

        public Color maxColor = Color.green;
        public Color minColor = Color.red;
        public float maxValue = 1.0f;

        // Parameters
        public float minValue = 0.0f;

        // Target
        public Image image;
        public SelectedAxis selectedAxis = SelectedAxis.xAxis;

        #endregion

        // The default image is the one in the gameObject
        private void Start()
        {
            if (this.image == null)
            {
                this.image = this.GetComponent<Image>();
            }
        }

        private void Update()
        {
            switch (this.selectedAxis)
            {
                case SelectedAxis.xAxis:
                    // Lerp color depending on the scale factor
                    this.image.color = Color.Lerp(this.minColor, this.maxColor,
                                                  Mathf.Lerp(this.minValue, this.maxValue, this.transform.localScale.x));
                    break;
                case SelectedAxis.yAxis:
                    // Lerp color depending on the scale factor
                    this.image.color = Color.Lerp(this.minColor, this.maxColor,
                                                  Mathf.Lerp(this.minValue, this.maxValue, this.transform.localScale.y));
                    break;
                case SelectedAxis.zAxis:
                    // Lerp color depending on the scale factor
                    this.image.color = Color.Lerp(this.minColor, this.maxColor,
                                                  Mathf.Lerp(this.minValue, this.maxValue, this.transform.localScale.z));
                    break;
            }
        }

        public enum SelectedAxis
        {
            xAxis,
            yAxis,
            zAxis
        }
    }
}