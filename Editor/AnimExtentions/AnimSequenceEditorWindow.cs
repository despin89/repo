
//#if UNITY_EDITOR

//namespace GD.EditorExtentions
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Reflection;
//    using Game.AnimationSystem;
//    using UnityEditor;
//    using UnityEngine;

//    public class AnimSequenceEditorWindow : EditorWindow
//    {
//        #region Fields

//        private readonly float _visibleDuration = 5;
//        private readonly int _boxHeight = 50;

//        private AnimEventEditorWindow _animEventEditor;

//        private AnimEventsSequence _currentSequence;

//        private float _vScrollPosition;
//        private float _hScrollPosition;

//        private GenericMenu _addEventMenu;

//        private Type[] _animEventTypes;

//        #endregion

//        public static AnimSequenceEditorWindow ShowWindow()
//        {
//            AnimSequenceEditorWindow window =
//                GetWindow(typeof(AnimSequenceEditorWindow), false, "Scenario Editor Window") as
//                    AnimSequenceEditorWindow;
//            return window;
//        }

//        public void SetSequence(AnimEventsSequence sequence)
//        {
//            this._currentSequence = sequence;
//        }

//        private void OnGUI()
//        {
//            if (this._currentSequence != null)
//            {
//                if (this._animEventTypes == null)
//                {
//                    this._animEventTypes =
//                        Assembly.GetAssembly(typeof(AnimEventBase))
//                            .GetTypes()
//                            .Where(e => e.IsSubclassOf(typeof(AnimEventBase)))
//                            .Select(e => e)
//                            .ToArray();
//                }


//                List<AnimEventBase> events = this._currentSequence.Events;

//                Rect toolbatRect = new Rect(0, 0, this.position.width, 20);

//                this._currentSequence.MaximumTracks = EditorGUI.IntField(new Rect(0, 0, 200, 20), "Max tracks",
//                                                                         this._currentSequence.MaximumTracks);

//                if (toolbatRect.yMax <= 1f)
//                {
//                    return;
//                }

//                int trackOffset = Mathf.FloorToInt(this._vScrollPosition / this._boxHeight);
//                this.DrawEvents(events, toolbatRect.yMax, this._visibleDuration, trackOffset);
//                DrawTimeline(toolbatRect.yMax);

//                bool vScrollVisible = this._currentSequence.MaximumTracks * this._boxHeight > this.position.height;
//                float tempMaxDuration = vScrollVisible
//                    ? this._currentSequence.MaximumDuration + 0.1f
//                    : this._currentSequence.MaximumDuration;
//                bool hScrollVisible = this._visibleDuration < tempMaxDuration;

//                if (vScrollVisible)
//                {
//                    this._vScrollPosition =
//                        GUI.VerticalScrollbar(
//                                              new Rect(this.position.width - 15f, toolbatRect.yMax, 15f,
//                                                       this.position.height - toolbatRect.yMax -
//                                                       (hScrollVisible ? 15f : 0f)), this._vScrollPosition,
//                                              this.position.height - toolbatRect.yMax - this._boxHeight, 0f,
//                                              this._currentSequence.MaximumTracks * this._boxHeight);
//                }
//                else
//                {
//                    this._vScrollPosition = 0f;
//                }

//                if (hScrollVisible)
//                {
//                    this._hScrollPosition =
//                        GUI.HorizontalScrollbar(
//                                                new Rect(0f, this.position.height - 15f,
//                                                         this.position.width - (vScrollVisible ? 15f : 0f), 15f),
//                                                this._hScrollPosition, this.position.width, 0f,
//                                                tempMaxDuration * this.position.width);
//                }
//                else
//                {
//                    this._hScrollPosition = 0f;
//                }

//                if ((Event.current.type == EventType.MouseDown) && (Event.current.button == 1))
//                {
//                    if (this._addEventMenu == null)
//                    {
//                        this._addEventMenu = new GenericMenu();
//                        foreach (Type t in this._animEventTypes)
//                        {
//                            this._addEventMenu.AddItem(new GUIContent(t.ToString().Split('.').Last()), false,
//                                                       this.CreateContextItem, t);
//                        }
//                    }

//                    this._addEventMenu.ShowAsContext();
//                    Event.current.Use();
//                }
//            }
//            else
//            {
//                GUILayout.Label("Sequence is NULL");
//            }
//        }

//        private void DrawEvents(List<AnimEventBase> events, float offset, float duration, int _trackOffset)
//        {
//            foreach (AnimEventBase action in this._currentSequence.Events)
//            {
//                List<AnimEventBase> newList = events.ToList();
//                newList.Remove(action);

//                foreach (AnimEventBase otherAction in newList)
//                {
//                    if ((otherAction.EditingTrack == action.EditingTrack) &&
//                        (((action.StartTime >= otherAction.StartTime) && (action.StartTime <= otherAction.EndTime)) ||
//                         ((action.EndTime >= otherAction.StartTime) && (action.EndTime <= otherAction.EndTime))))
//                    {
//                        otherAction.EditingTrack++;
//                    }
//                }
//            }

//            float maxVisibleTracks = this.position.height / this._boxHeight + 1;
//            int maxTracks = this._currentSequence.MaximumTracks;
//            for (int i = 0; i < (maxTracks < maxVisibleTracks ? maxTracks : maxVisibleTracks); i++)
//            {
//                GUIStyle trackStyle = new GUIStyle(GUI.skin.box)
//                {
//                    normal = {background = AnimEventBase.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.1f))},
//                    name = "Track Style"
//                };
//                GUI.Box(new Rect(0, offset + this._boxHeight * i, this.position.width, this._boxHeight + 1),
//                        string.Empty, trackStyle);
//            }

//            foreach (AnimEventBase animEvent in events)
//            {
//                if ((animEvent.EditingTrack < _trackOffset) ||
//                    (animEvent.EditingTrack >= _trackOffset + maxVisibleTracks))
//                {
//                    continue;
//                }

//                float horizontalPosStart = this.position.width * (animEvent.StartTime / duration);
//                float horizontalPosEnd = this.position.width * (animEvent.EndTime / duration);
//                float width = horizontalPosEnd - horizontalPosStart;
//                Rect boxRect = new Rect(horizontalPosStart,
//                                        offset + this._boxHeight * (animEvent.EditingTrack - _trackOffset),
//                                        width, this._boxHeight);
//                EditorGUIUtility.AddCursorRect(boxRect, MouseCursor.Pan);

//                animEvent.DrawTimelineGui(boxRect);
//                bool mainHandle = boxRect.Contains(Event.current.mousePosition);

//                if (((Event.current.type == EventType.MouseDown) || (Event.current.type == EventType.ContextClick)) &&
//                    mainHandle)
//                {
//                    if (Event.current.button == 0)
//                    {
//                        switch (Event.current.clickCount)
//                        {
//                            case 2:
//                                this._animEventEditor =
//                                    GetWindow(typeof(AnimEventEditorWindow), true, "Event editor") as
//                                        AnimEventEditorWindow;
//                                this._animEventEditor.SetCurrentEvent(animEvent);
//                                break;
//                        }
//                    }
//                    else if ((Event.current.button == 1) || (Event.current.type == EventType.ContextClick))
//                    {
//                        GenericMenu eventMenu = new GenericMenu();
//                        eventMenu.AddItem(new GUIContent("Remove"), false, this.RemoveContextItem, animEvent);
//                        eventMenu.ShowAsContext();
//                        Event.current.Use();
//                    }
//                }
//            }
//        }


//        private void DrawTimeline(float yMax)
//        {
//            if (this._currentSequence.InProgress)
//            {
//                var horizontalPosStart = position.width * _currentSequence.CurrentTime;
//                var timeRect = new Rect(horizontalPosStart, yMax, 1f, position.height - yMax);
//                GUI.Box(timeRect, string.Empty);
//                this.Repaint();
//            }
//        }

//        private void RemoveContextItem(object obj)
//        {
//            AnimEventBase animEvent = obj as AnimEventBase;
//            if (animEvent == null) return;

//            this._currentSequence.RemoveEvent(animEvent);
//            DestroyImmediate(animEvent, true);

//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();

//            this.Repaint();
//        }

//        private void CreateContextItem(object obj)
//        {
//            Type animEvent = obj as Type;
//            if (animEvent == null) return;

//            AnimEventBase animEventSO = CreateInstance(animEvent) as AnimEventBase;
//            this._currentSequence.AddEvent(animEventSO);

//            AssetDatabase.AddObjectToAsset(animEventSO, this._currentSequence);
//            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animEventSO));

//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();

//            this.Repaint();
//        }

//        private static float RoundToPoint1(float value)
//        {
//            return (float) Math.Round(value * 100, MidpointRounding.AwayFromZero) / 100;
//        }
//    }
//}

//#endif