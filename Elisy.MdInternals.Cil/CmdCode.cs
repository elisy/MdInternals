using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.Cil
{
    public enum CmdCode : int
    {
        _IfSimple = -22,
        _IfSimpleEndIf = -21,
        _ContinueLabel = -20,
        _Continue = -19,
        _BreakLabel = -18,
        _Break = -17,
        _While = -16,
        _WhileEndDo = -15,
        _If = -14,
        _ElsIf = -13,
        _Else = -12,
        _EndIf = -11,
        _For = -10,
        _ForEndDo = -9,
        _ForEach = -8,
        _ForEachIn = -7,
        _ForEachEndDo = -6,
        _Catch = -5,
        _ReturnProc = -4,
        _ReturnFunc = -3,
        _InlineIfThen = -2,
        _NOPInternal = -1,

        Nop = 0,
        LineNo = 1,
        PushStatic = 2,
        PushLocal = 3,
        PushConst = 4,
        PushReturn = 5,
        PushFalse = 6,
        PushTrue = 7,
        PushUndefined = 8,
        PushNull = 9,
        PushEmpty = 10,//???
        Pop = 11,
        GetObjectProperty = 12,
        GetIndexed = 13,
        SelEnum = 14,
        NextEnum = 15,
        Assign = 16, //Pop = Pop

        AssignReturnValue = 17, //Pop = Pop
        SetParamsCount = 18, //Parameter number
        Call = 19,
        CallObjectProcedure = 20,
        CallObjectFunction = 21,
        Ret = 22,

        Neg = 23,
        Add = 24,
        Sub = 25,
        Mul = 26,
        Div = 27,
        Mod = 28,

        Not = 29,
        AndJmp = 30,
        OrJmp = 31,
        LogVal = 32, //???
        EQ = 33,
        NE = 34,
        GT = 35,
        LT = 36,
        GE = 37,
        LE = 38,

        Jmp = 39,
        JZ = 40, //Else go to op1
        JNZ = 41,
        JmpLbl = 42,

        Inc = 43,

        PushTry = 44,
        PopTry = 45,
        EndTry = 46,
        Raise = 47,

        PushFor = 48,
        PushForVar = 49,
        PopFor = 50,

        New = 51, //Constants[op1]

        Execute = 52,
        StrLen = 53,
        TrimL = 54,
        TrimR = 55,
        TrimAll = 56,
        Left = 57,
        Right = 58,
        Mid = 59,
        Find = 60,
        Upper = 61,
        Lower = 62,
        Char = 63,
        CharCode = 64,
        IsBlankStr = 65,
        Int = 66,
        RoundDefault = 67,
        Round = 68,

        Boolean = 69,
        Numeric = 70,
        String = 71,

        Date = 72,
        Date3 = 73,
        Date6 = 74,
        AddMonth = 75,
        BegOfMonth = 76,
        EndOfMonth = 77,
        BegOfQuart = 78,
        EndOfQuart = 79,
        BegOfYear = 80,
        EndOfYear = 81,
        Year = 82,
        Month = 83,
        Day = 84,
        Hour = 85,
        Minute = 86,
        Second = 87,
        DayOfYear = 88,
        WeekOfYear = 89,
        WeekDay = 90,
        BegOfWeek = 91,
        EndOfWeek = 92,
        BegOfDay = 93,
        EndOfDay = 94,
        BegOfHour = 95,
        EndOfHour = 96,
        BegOfMinute = 97,
        EndOfMinute = 98,
        CurDate = 99,

        StrReplace = 100,
        LinesCount = 101,
        LineGet = 102,

        Min = 103,
        Max = 104,
        StrCountOccur = 105,
        ErrorDescr = 106,
        TypeOf = 107,
        Type = 108,
        Eval = 109,
        Format = 110,
        NewObject = 111,

        ACos = 112,
        ASin = 113,
        ATan = 114,
        Cos = 115,
        Exp = 116,
        Log = 117,
        Log10 = 118,
        Pow = 119,
        Sin = 120,
        Sqrt = 121,
        Tan = 122,

        AddHndlr = 123,
        AddObjHndlr = 124,
        RmvHndlr = 125,
        RmvObjHndlr = 126,
        Title = 127,
        ErrorInfo = 128
    }
}
