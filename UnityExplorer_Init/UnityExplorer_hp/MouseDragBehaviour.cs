using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityExplorer_hp
{
    // Token: 0x02000006 RID: 6
    public class MouseDragBehaviour : EventTrigger
    {

        public override void OnBeginDrag(PointerEventData eventData)
        {
            this._lastMousePosition = eventData.position;
        }


        public override void OnDrag(PointerEventData eventData)
        {
            Vector2 position = eventData.position;
            Vector2 vector = position - this._lastMousePosition;
            RectTransform component = base.GetComponent<RectTransform>();
            Vector3 vector2 = component.position;
            Vector3 vector3 = vector2 + new Vector3(vector.x, vector.y, base.transform.position.z);
            Vector3 position2 = vector2;
            vector2 = vector3;
            component.position = vector2;
            if (!this.IsRectTransformInsideSreen(component))
            {
                component.position = position2;
            }
            this._lastMousePosition = position;
        }


        public override void OnEndDrag(PointerEventData eventData)
        {
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000025C4 File Offset: 0x000007C4
        private bool IsRectTransformInsideSreen(RectTransform rectTransform)
        {
            bool result = false;
            Vector3[] array = new Vector3[4];
            rectTransform.GetWorldCorners(array);
            int num = 0;
            Rect rect = new Rect(0f, 0f, Screen.width, Screen.height);
            foreach (Vector3 point in array)
            {
                if (rect.Contains(point))
                {
                    num++;
                }
            }
            if (num == 4)
            {
                result = true;
            }
            return result;
        }


        private void Update()
        {
            if (this.Enter)
            {
                Vector3 localScale = base.transform.localScale;
                float x = localScale.x;
                float y = localScale.y;
                float axis = Input.GetAxis("Mouse ScrollWheel");
                base.transform.localScale = new Vector2(x + axis, y + axis);
            }
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            this.Enter = true;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002140 File Offset: 0x00000340
        public override void OnPointerExit(PointerEventData eventData)
        {
            this.Enter = false;
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002149 File Offset: 0x00000349
        public MouseDragBehaviour()
        {
        }

        // Token: 0x04000006 RID: 6
        public Vector2 _lastMousePosition;

        // Token: 0x04000007 RID: 7
        public bool Enter;
    }
}
