using System;
using System.Runtime.InteropServices;

namespace PilotNtSharp.Interop
{
    internal enum PilotNtTransType
    {
        Payment = 1,
        Return = 3,
        CloseDay = 7,

        StatShort = 1,
        StatDetailed = 0,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct AuthAnswer
    {
        public PilotNtTransType TType;                   //вход: тип транзакции

        public uint Amount;        //вход: сумма в копейках

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 + 1)]
        public byte[] Rcode; //выход: код результата авторизации

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] AMessage; //выход: словесное пояснение результата

        public int CType;         //выход: тип карты 

        public IntPtr Check;         //выход: образ чека, должен освобождаться
        //       GlobalFree в вызывающей программе        
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct AuthAnswer9
    {
        public PilotNtTransType TType;                   //вход: тип транзакции

        public uint Amount;        //вход: сумма в копейках

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 + 1)]
        public byte[] Rcode; //выход: код результата авторизации

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] AMessage; //выход: словесное пояснение результата

        public int CType;         //выход: тип карты 

        public IntPtr Check;         //выход: образ чека, должен освобождаться
                                     //       GlobalFree в вызывающей программе

        // Конец структуры AuthAnswer

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] AuthCode; //выход: код авторизации

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        public byte[] CardID;   //выход: номер карты        

        public int SberOwnCard;   //выход: флаг принадлежности карты Сбербанку 

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Hash;
    }

    internal static class PilotNtInterop
    {
        [DllImport("pilot_nt.dll", EntryPoint = "_card_authorize9", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int CardAuthorize9(byte[] rack2, ref AuthAnswer9 ans);

        [DllImport("pilot_nt.dll", EntryPoint = "_SuspendTrx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int SuspendTrx(uint dwAmount, byte[] pAuthCode);

        [DllImport("pilot_nt.dll", EntryPoint = "_CommitTrx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int CommitTrx(uint dwAmount, byte[] pAuthCode);

        [DllImport("pilot_nt.dll", EntryPoint = "_RollbackTrx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int RollbackTrx(uint dwAmount, byte[] pAuthCode);

        [DllImport("pilot_nt.dll", EntryPoint = "_close_day", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CloseDay(ref AuthAnswer ans);

        [DllImport("pilot_nt.dll", EntryPoint = "_get_statistics", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetStatistics(ref AuthAnswer ans);

        [DllImport("pilot_nt.dll", EntryPoint = "_ServiceMenu", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ServiceMenu(ref AuthAnswer ans);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GlobalFree(IntPtr handle);
    }
}
