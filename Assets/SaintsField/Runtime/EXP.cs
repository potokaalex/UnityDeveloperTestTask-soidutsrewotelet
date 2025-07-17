﻿using System;

namespace SaintsField
{
    [Flags]
    // ReSharper disable once InconsistentNaming
    public enum EXP
    {
        None = 0,

        NoInitSign = 1 << 1,
        NoAutoResignToValue = 1 << 2,
        NoAutoResignToNull = 1 << 3,
        NoResignButton = 1 << 4,
        NoMessage = 1 << 5,
        NoPicker = 1 << 6,
        KeepOriginalPicker = 1 << 7,
        ForceReOrder = 1 << 8,

        NoAutoResign = NoAutoResignToValue | NoAutoResignToNull,

        Silent = NoAutoResign | NoMessage,
        JustPicker = NoInitSign | NoAutoResign | NoResignButton | NoMessage,
        Message = NoAutoResign | NoResignButton,
    }

    public static class EXPExtensions
    {
        public static bool HasFlagFast(this EXP lhs, EXP rhs) => (lhs & rhs) != 0;
    }
}
