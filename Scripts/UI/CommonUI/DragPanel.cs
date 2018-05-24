namespace GD.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields

        private bool mouseDown;
        private bool restrictX;
        private bool restrictY;

        private float fakeX;
        private float fakeY;
        private float myHeight;
        private float myWidth;
        public RectTransform MyRect;

        public RectTransform ParentRT;
        private Vector3 startMousePos;
        private Vector3 startPos;

        #endregion

        #region Implementations

        public void OnPointerDown(PointerEventData ped)
        {
            this.mouseDown = true;
            this.startPos = this.MyRect.transform.position;
            this.startMousePos = Input.mousePosition;
        }

        public void OnPointerUp(PointerEventData ped)
        {
            this.mouseDown = false;
        }

        #endregion

        private void Start()
        {
            this.myWidth = (this.MyRect.rect.width + 5) / 2;
            this.myHeight = (this.MyRect.rect.height + 5) / 2;
        }


        private void Update()
        {
            if (this.mouseDown)
            {
                Vector3 currentPos = Input.mousePosition;
                Vector3 diff = currentPos - this.startMousePos;
                Vector3 pos = this.startPos + diff;
                this.MyRect.transform.position = pos;

                if ((this.MyRect.transform.localPosition.x < 0 - (this.ParentRT.rect.width / 2 - this.myWidth)) ||
                    (this.MyRect.transform.localPosition.x > this.ParentRT.rect.width / 2 - this.myWidth))
                {
                    this.restrictX = true;
                }
                else
                {
                    this.restrictX = false;
                }

                if ((this.MyRect.transform.localPosition.y < 0 - (this.ParentRT.rect.height / 2 - this.myHeight)) ||
                    (this.MyRect.transform.localPosition.y > this.ParentRT.rect.height / 2 - this.myHeight))
                {
                    this.restrictY = true;
                }
                else
                {
                    this.restrictY = false;
                }

                if (this.restrictX)
                {
                    if (this.MyRect.transform.localPosition.x < 0)
                    {
                        this.fakeX = 0 - this.ParentRT.rect.width / 2 + this.myWidth;
                    }
                    else
                    {
                        this.fakeX = this.ParentRT.rect.width / 2 - this.myWidth;
                    }

                    Vector3 xpos = new Vector3(this.fakeX, this.MyRect.transform.localPosition.y, 0.0f);
                    this.MyRect.transform.localPosition = xpos;
                }

                if (this.restrictY)
                {
                    if (this.MyRect.transform.localPosition.y < 0)
                    {
                        this.fakeY = 0 - this.ParentRT.rect.height / 2 + this.myHeight;
                    }
                    else
                    {
                        this.fakeY = this.ParentRT.rect.height / 2 - this.myHeight;
                    }

                    Vector3 ypos = new Vector3(this.MyRect.transform.localPosition.x, this.fakeY, 0.0f);
                    this.MyRect.transform.localPosition = ypos;
                }
            }
        }
    }
}