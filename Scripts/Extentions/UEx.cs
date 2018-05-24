namespace GD.Extentions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Game;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using Object = UnityEngine.Object;

    public static class UEx
    {
        #region Публичные методы

        public static List<GameObject> GetAllChild(this GameObject go)
        {
            return (from Transform t in go.transform select t.gameObject).ToList();
        }

        public static void StaticPixelSnap(this GameObject go)
        {
            //Debug.Log("on will" + Camera.current);
            Camera cam = Camera.main;

            var actualPosition = go.transform.position;

            float cameraPPU = (float)cam.pixelHeight / (2f * cam.orthographicSize);
            float cameraUPP = 1.0f / cameraPPU;

            Vector2 camPos = cam.transform.position.xy();
            Vector2 pos = actualPosition.xy();
            Vector2 relPos = pos - camPos;

            Vector2 offset = new Vector2(0, 0)
            {
                x = (cam.pixelWidth % 2 == 0) ? 0 : 0.5f,
                y = (cam.pixelHeight % 2 == 0) ? 0 : 0.5f
            };
            // offset for screen pixel edge if screen size is odd
            // offset for pivot in Sprites
            Vector2 pivotOffset = new Vector2(0, 0);

            var sprite = go.GetComponent<Image>()?.sprite;

            if (sprite != null)
            {
                pivotOffset = sprite.pivot - new Vector2(Mathf.Floor(sprite.pivot.x), Mathf.Floor(sprite.pivot.y)); // the fractional part in texture pixels           
                float camPixelsPerAssetPixel = cameraPPU / sprite.pixelsPerUnit;
                pivotOffset *= camPixelsPerAssetPixel; // convert to screen pixels
            }
            relPos.x = (Mathf.Round(relPos.x / cameraUPP - offset.x) + offset.x + pivotOffset.x) * cameraUPP;
            relPos.y = (Mathf.Round(relPos.y / cameraUPP - offset.y) + offset.y + pivotOffset.y) * cameraUPP;

            pos = relPos + camPos;

            go.transform.position = new Vector3(pos.x, pos.y, actualPosition.z);
        }

        public static bool V3Eq_0001(this Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.0001;
        }

        public static void SetLayer(this GameObject parent, string layer, bool includeChildren = true)
        {
            int layerNumber = LayerMask.NameToLayer(layer);
            parent.layer = layerNumber;
            if (includeChildren)
            {
                foreach (Transform trans in parent.transform.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = layerNumber;
                }
            }
        }

        public static void SetLayer(this MonoBehaviour parent, string layer, bool includeChildren = true)
        {
            int layerNumber = LayerMask.NameToLayer(layer);
            parent.gameObject.layer = layerNumber;
            if (includeChildren)
            {
                foreach (Transform trans in parent.gameObject.transform.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = layerNumber;
                }
            }
        }

        public static T GetComponentInChild<T>(this MonoBehaviour o, string childName)
        {
            return o.transform.Find(childName).GetComponent<T>();
        }

        public static GameObject GetChild(this MonoBehaviour o, string childName)
        {
            return o.transform.Find(childName).gameObject;
        }

        public static void DestroyAllChildren(this MonoBehaviour o)
        {
            foreach (Transform child in o.gameObject.transform)
            {
                child.gameObject.Destroy();
            }
        }

        public static void DestroyAllChildren(this GameObject o)
        {
            foreach (Transform child in o.transform)
            {
                child.gameObject.Destroy();
            }
        }

        public static void DestroyAllChildren(this Transform t)
        {
            foreach (Transform child in t)
            {
                child.gameObject.Destroy();
            }
        }

        public static void DelayedAction(float waitTime, Action action, Func<bool> allower = null)
        {
            GameManager.Instance.StartCoroutine(DelayedActionCoroutine(waitTime, action, allower));
        }

        private static IEnumerator DelayedActionCoroutine(float waitTime, Action action, Func<bool> allower)
        {
            yield return new WaitForSeconds(waitTime);

            if (allower != null)
            {
                if (allower())
                    action?.Invoke();
            }
            else
                action?.Invoke();
        }

        public static IEnumerator Wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }

        public static void ActivateSelectable(this Selectable selectable)
        {
            selectable.interactable = true;
        }

        public static void DeactivateSelectable(this Selectable selectable)
        {
            selectable.interactable = false;
        }

        public static void ActivateGO(this GameObject gObj)
        {
            gObj.SetActive(true);
        }

        public static void SetLastAndActivate(this GameObject gObj)
        {
            gObj.ActivateGO();
            gObj.transform.SetAsLastSibling();
        }

        public static void DeactivateGO(this GameObject gObj)
        {
            gObj.SetActive(false);
        }

        public static void Destroy(this GameObject gObj)
        {
            Object.Destroy(gObj);
        }

        public static void Destroy(this MonoBehaviour mb)
        {
            mb.gameObject.Destroy();
        }

        public static void AddAction(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public static Text Text(this Button button)
        {
            return button.transform.GetChild(0).GetComponent<Text>();
        }

        public static T GetChildComponent<T>(this Transform transform)
        {
            T result = transform.GetComponentInChildren<T>();
            if (result == null)
            {
                throw new Exception("Cannot find a component of type: {0}".F(typeof(T)));
            }
            return result;
        }

        public static Text GetTextComponent(this Transform transform)
        {
            Text result = transform.GetComponentInChildren<Text>();
            if (result == null)
            {
                throw new Exception("Cannot find a component with a Name: Text");
            }
            return result;
        }

        public static Vector3 WorldToScreen(this MonoBehaviour mb, Vector3 pos)
        {
            return Camera.main.WorldToScreenPoint(pos);
        }

        #endregion
    }
}