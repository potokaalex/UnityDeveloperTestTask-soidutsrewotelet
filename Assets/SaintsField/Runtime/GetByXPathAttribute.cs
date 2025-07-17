﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SaintsField.Interfaces;
using SaintsField.Playa;
using SaintsField.SaintsXPathParser.Optimization;
using SaintsField.Utils;
#if UNITY_EDITOR
using SaintsField.SaintsXPathParser;
#endif
using UnityEngine;

namespace SaintsField
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class GetByXPathAttribute: PropertyAttribute, ISaintsAttribute, IPlayaAttribute, IPlayaArraySizeAttribute
    {
        public SaintsAttributeType AttributeType => SaintsAttributeType.Other;
        public virtual string GroupBy => "";

        public bool InitSign;
        public bool AutoResignToValue;
        public bool AutoResignToNull;
        public bool UseResignButton;
        public bool UsePickerButton;
        public bool UseErrorMessage;
        public bool KeepOriginalPicker;
        public bool ForceReOrder;

        public OptimizationPayload OptimizationPayload { get; protected set; }

        public struct XPathInfo
        {
            public bool IsCallback;
            public string Callback;
#if UNITY_EDITOR
            public IReadOnlyList<XPathStep> XPathSteps;

            public override string ToString()
            {
                return XPathSteps == null? Callback: string.Join("/", XPathSteps);
            }
#endif
        }

        // outer and inner or
        public IReadOnlyList<IReadOnlyList<XPathInfo>> XPathInfoAndList;

        protected void ParseOptions(EXP config)
        {
            InitSign = !config.HasFlagFast(EXP.NoInitSign);
            UsePickerButton = !config.HasFlagFast(EXP.NoPicker);
            KeepOriginalPicker = !UsePickerButton || config.HasFlagFast(EXP.KeepOriginalPicker);
            AutoResignToValue = !config.HasFlagFast(EXP.NoAutoResignToValue);
            AutoResignToNull = !config.HasFlagFast(EXP.NoAutoResignToNull);
            if (AutoResignToValue && AutoResignToNull)
            {
                UseResignButton = false;
            }
            else
            {
                UseResignButton = !config.HasFlagFast(EXP.NoResignButton);
            }

            if (config.HasFlagFast(EXP.NoMessage))
            {
                UseErrorMessage = false;
            }
            else
            {
                UseErrorMessage = !UseResignButton;
            }

            ForceReOrder = config.HasFlagFast(EXP.ForceReOrder);
        }

        public GetByXPathAttribute(EXP config, params string[] ePaths)
        {
            ParseOptions(config);
            ParseXPaths(ePaths);
        }

        protected void ParseXPaths(params string[] ePaths)
        {
            XPathInfo[] xPathInfoOrList = ePaths.Length == 0
                ? new[]
                {
                    new XPathInfo
                    {
                        IsCallback = false,
                        Callback = "",
#if UNITY_EDITOR
                        XPathSteps = XPathParser.Parse(".").ToArray(),
#endif
                    },
                }
                : ePaths
                    .Select(ePath =>
                    {
                        (string callback, bool actualIsCallback) = RuntimeUtil.ParseCallback(ePath, false);

                        return new XPathInfo
                        {
                            IsCallback = actualIsCallback,
                            Callback = callback,
#if UNITY_EDITOR
                            XPathSteps = XPathParser.Parse(ePath).ToArray(),
#endif
                        };
                    })
                    .ToArray();

            XPathInfoAndList = new[] { xPathInfoOrList };
        }

        public GetByXPathAttribute(params string[] ePaths) : this(SaintsFieldConfigUtil.GetByXPathExp(EXP.None), ePaths)
        {
        }

        protected static string GetComponentFilter(Type compType)
        {
            if (compType == null)
            {
                return "";
            }

            string nameSpace = compType.Namespace;
            string typeName = compType.Name;
            return $"[@{{GetComponents({nameSpace}.{typeName})}}]";
        }
    }
}
