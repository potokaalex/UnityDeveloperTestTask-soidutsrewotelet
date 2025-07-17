﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaintsField.Editor.Playa.SaintsEditorWindowUtils;
using SaintsField.Playa;
using UnityEditor;


namespace SaintsField.Editor
{
    public partial class SaintsEditorWindow: EditorWindow
    {
        #region Inline Editor

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
        public class WindowInlineEditorAttribute : Attribute, IPlayaAttribute
        {
            public readonly Type EditorType;
            public WindowInlineEditorAttribute(Type editorType = null)
            {
                EditorType = editorType;
            }
        }

        #endregion

        // [NonSerialized] private readonly UnityEvent<UnityEngine.Object> _editorChangeTargetEvent = new UnityEvent<UnityEngine.Object>();

        [NonSerialized]
        // ReSharper disable once UnassignedField.Global
        public bool EditorShowMonoScript;

        public virtual Type EditorDrawerType => typeof(SaintsEditorWindowSpecialEditor);

        private UnityEngine.Object EditorGetTargetInternal()
        {
            return EditorInspectingTarget = GetTarget();
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public virtual UnityEngine.Object GetTarget()
        {
            return this;
        }

        [NonSerialized] public UnityEngine.Object EditorInspectingTarget;

        public void EditorRefreshTarget()
        {
            if (_saintsEditorWindowSpecialEditor != null)
            {
                DestroyImmediate(_saintsEditorWindowSpecialEditor);
                _saintsEditorWindowSpecialEditor = null;
            }

#if UNITY_2021_3_OR_NEWER && !SAINTSFIELD_UI_TOOLKIT_DISABLE
            EditorRelinkRootUIToolkit();
#endif
        }

        #region LifeCircle

        // public virtual UnityEngine.Object EditorGetInitTarget(UnityEngine.Object oldTarget) => this;
        // public void EditorChangeTarget(UnityEngine.Object newTarget) => _editorChangeTargetEvent.Invoke(newTarget);

        private void OnDestroy()
        {
#if UNITY_2021_3_OR_NEWER && !SAINTSFIELD_UI_TOOLKIT_DISABLE
            EditorCleanUpUIToolkit();
#endif
            EditorCleanUpIMGUI();
            OnEditorDestroy();
        }

        public virtual void OnEditorDestroy()
        {

        }

        private void OnEnable()
        {
            OnEditorEnable();
        }

        public virtual void OnEditorEnable()
        {

        }

        private void OnDisable()
        {
            OnEditorDisable();
        }

        public virtual void OnEditorDisable()
        {

        }

        #endregion

        #region Update

        private readonly HashSet<IEnumerator> _coroutines = new HashSet<IEnumerator>();

        private void EditorOnUpdateInternal()
         {
             // HashSet<IEnumerator> toRemove = new HashSet<IEnumerator>();
             // Debug.Log(_coroutines.Count);
             // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
             foreach (IEnumerator coroutine in _coroutines.ToArray())
             {
                 // Debug.Log($"coroutine run={coroutine}");
                 if (!coroutine.MoveNext())
                 {
                     // Debug.Log($"coroutine will move={coroutine}");
                     // toRemove.Add(coroutine);
                     _coroutines.Remove(coroutine);
                 }
             }

             // _coroutines.ExceptWith(toRemove);

             OnEditorUpdate();
         }

        #endregion

        // ReSharper disable once MemberCanBeProtected.Global
        public virtual void OnEditorUpdate()
        {
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public void StartEditorCoroutine(IEnumerator routine)
        {
            _coroutines.Add(routine);
            // Debug.Log($"coroutine add={routine}, {_coroutines.Count}");
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public void StopEditorCoroutine(IEnumerator routine)
        {
            _coroutines.Remove(routine);
        }
    }
}
