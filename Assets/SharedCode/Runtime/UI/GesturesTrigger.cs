using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.UI
{
    [AddComponentMenu("Event/Gestures Trigger")]
    public class GesturesTrigger : EventTrigger
	{
		//public UnityEvent OnTapDown, OnTapUp;
        public UnityEvent OnSwipeLeft, OnSwipeRight, OnSwipeUp, OnSwipeDown;
        public UnityEvent OnClick;
        public UIClickThrough clickThrough;

        float xMovementThisDrag, yMovementThisDrag;

        bool gestureMadeThisDrag; 

        public override void OnPointerDown(PointerEventData data)
        { 
            gestureMadeThisDrag = false;
            xMovementThisDrag = yMovementThisDrag = 0;
			//OnTapDown.Invoke ();
            //Debug.Log("OnPointerEnter called.");
        }

        public override void OnDrag(PointerEventData data)
        {
            xMovementThisDrag += data.delta.x;
            yMovementThisDrag += data.delta.y;
            DetectGesture();
            //Debug.Log("OnDrag called.");
        }

        public override void OnPointerClick(PointerEventData data)
        {
            if (!gestureMadeThisDrag)
            {
                OnClick.Invoke();
                //Debug.Log("OnPointerClick called.");
                if (clickThrough != null) clickThrough.SendClick(data);
            }
        }

//		public override void OnPointerUp(PointerEventData data)
//		{
////		    Debug.Log("OnPointerUp called.");
//			OnTapUp.Invoke ();
//		}

        #region unused calls 

        //public override void OnBeginDrag(PointerEventData data)
        //{
        //    Debug.Log("OnBeginDrag called.");
        //}

        //public override void OnEndDrag(PointerEventData data)
        //{
        //    Debug.Log("OnEndDrag called.");
        //}

        //public override void OnCancel(BaseEventData data)
        //{
        //    Debug.Log("OnCancel called.");
        //}

        //public override void OnDeselect(BaseEventData data)
        //{
        //    Debug.Log("OnDeselect called.");
        //}

        //public override void OnDrop(PointerEventData data)
        //{
        //    Debug.Log("OnDrop called.");
        //}

        //public override void OnInitializePotentialDrag(PointerEventData data)
        //{
        //    Debug.Log("OnInitializePotentialDrag called.");
        //}

        //public override void OnMove(AxisEventData data)
        //{
        //    Debug.Log("OnMove called.");
        //}

        //public override void OnPointerEnter(PointerEventData data)
        //{
        //    Debug.Log("OnPointerDown called.");
        //} 

        //public override void OnPointerExit(PointerEventData data)
        //{
        //    Debug.Log("OnPointerExit called.");
        //} 
 

        //public override void OnScroll(PointerEventData data)
        //{
        //    Debug.Log("OnScroll called.");
        //}

        //public override void OnSelect(BaseEventData data)
        //{
        //    Debug.Log("OnSelect called.");
        //}

        //public override void OnSubmit(BaseEventData data)
        //{
        //    Debug.Log("OnSubmit called.");
        //}

        //public override void OnUpdateSelected(BaseEventData data)
        //{
        //    Debug.Log("OnUpdateSelected called.");
        //}
        #endregion

        float thresholdInPixels = 0;
        public float thresholdInInches = .4f;
        void DetectGesture()
        {
            if (gestureMadeThisDrag) return;

            thresholdInPixels = Screen.dpi * thresholdInInches;

            if (xMovementThisDrag > thresholdInPixels) {
                OnSwipeRight.Invoke();
                gestureMadeThisDrag = true;
                //print("OnSwipeRight");
            }
            else if (xMovementThisDrag < -thresholdInPixels) {
                OnSwipeLeft.Invoke();
                gestureMadeThisDrag = true;
                //print("OnSwipeLeft");
            }

            if (yMovementThisDrag > thresholdInPixels) {
                OnSwipeUp.Invoke();
                gestureMadeThisDrag = true;
                //print("OnSwipeUp");
            }
            else if (yMovementThisDrag < -thresholdInPixels) {
                OnSwipeDown.Invoke();
                gestureMadeThisDrag = true;
                //print("OnSwipeDown");
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GesturesTrigger))]
    public class GesturesTriggerEditor : Editor
    {
        GesturesTrigger script;
        SerializedObject so;
        SerializedProperty sp;
        void OnEnable()
        {
            script = target as GesturesTrigger; 
        }

        public override void OnInspectorGUI()
        {
            so = new SerializedObject(target); 

            GUILayout.Space(10);
            GUILayout.Label("Click: ");
            sp = so.FindProperty("OnClick");
            EditorGUILayout.PropertyField(sp);
            script.clickThrough = (UIClickThrough)EditorGUILayout.ObjectField("clickThrough", script.clickThrough, typeof(UIClickThrough), true);

            GUILayout.Space(10);
            GUILayout.Label("Swipes: ");

            script.thresholdInInches = EditorGUILayout.FloatField("Threshold (Inches)", script.thresholdInInches);

            sp = so.FindProperty("OnSwipeLeft"); 
            EditorGUILayout.PropertyField(sp);

            sp = so.FindProperty("OnSwipeRight");
            EditorGUILayout.PropertyField(sp);

            sp = so.FindProperty("OnSwipeUp");
            EditorGUILayout.PropertyField(sp);

            sp = so.FindProperty("OnSwipeDown");
            EditorGUILayout.PropertyField(sp);
        }
    }
#endif
}