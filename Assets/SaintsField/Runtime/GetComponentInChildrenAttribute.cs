﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SaintsField.SaintsXPathParser.Optimization;
using SaintsField.Utils;
#if UNITY_EDITOR
using SaintsField.SaintsXPathParser;
#endif

namespace SaintsField
{
    [Conditional("UNITY_EDITOR")]
    public class GetComponentInChildrenAttribute: GetByXPathAttribute
    {
        public override string GroupBy { get; }

        public readonly Type CompType;
        public readonly bool IncludeInactive;
        public readonly bool ExcludeSelf;

        public GetComponentInChildrenAttribute(bool includeInactive = false, Type compType = null, bool excludeSelf = false, string groupBy = "")
        {
            ParseOptions(SaintsFieldConfigUtil.GetComponentInChildrenExp(EXP.NoPicker | EXP.NoAutoResignToNull));
            ParseArguments(includeInactive, compType, excludeSelf);
            GroupBy = groupBy;

            CompType = compType;
            IncludeInactive = includeInactive;
            ExcludeSelf = excludeSelf;

            OptimizationPayload = new GetComponentInChildrenPayload(compType, includeInactive, excludeSelf);
        }

        public GetComponentInChildrenAttribute(EXP config, bool includeInactive = false, Type compType = null, bool excludeSelf = false, string groupBy = "")
        {
            ParseOptions(config);
            ParseArguments(includeInactive, compType, excludeSelf);
            GroupBy = groupBy;

            OptimizationPayload = new GetComponentInChildrenPayload(compType, includeInactive, excludeSelf);
        }

        private void ParseArguments(bool includeInactive, Type compType, bool excludeSelf)
        {
            string compFilter = GetComponentFilter(compType);
            string activeFilter = includeInactive ? "" : "[@{gameObject.activeInHierarchy}]";

            IEnumerable<string> andXPaths = (excludeSelf? new[]{"//*"}: new[]{"", "//*"})
                .Select(each => $"{each}{activeFilter}{compFilter}");

            XPathInfoAndList = andXPaths.Select(ePath =>
            {
                // UnityEngine.Debug.Log($"ePath={ePath}");
                return new[]
                {
                    new XPathInfo
                    {
                        IsCallback = false,
                        Callback = "",
#if UNITY_EDITOR
                        XPathSteps = XPathParser.Parse(ePath).ToArray(),
#endif
                    },
                };
            }).ToArray();
        }
    }
}
