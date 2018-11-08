using System;
using System.Runtime.InteropServices;
using System.Text;
using PilotNtSharp.Interop;

namespace PilotNtSharp
{
    /// <summary>
    /// Класс для работы с пин-падом сбербанка через библиотеку pilot_nt.dll
    /// </summary>
    public class PilotNt
    {
        /// <summary>
        /// Проведение оплаты. 
        /// Для завершения транзакции необходимо вызвать метод CommiTransaction
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="suspendTransaction"></param>
        /// <returns></returns>
        public PaymentResponse Payment(decimal amount, bool suspendTransaction=true)
        {
            uint iAmount = (uint) (amount * 100);
            var ans = new AuthAnswer7
            {
                TType = PilotNtTransType.Payment,
                Amount = iAmount,
            };
            PilotNtInterop.CardAuthorize7(null, ref ans);

            var check = ReadCheck(ans.Check);
            var encoding = Encoding.GetEncoding(1251);
            var code = int.Parse(encoding.GetString(ans.Rcode).TrimEnd('\0'));
            var message = encoding.GetString(ans.AMessage).TrimEnd('\0');
            var authCode = encoding.GetString(ans.AuthCode).TrimEnd('\0');

            var response = new PaymentResponse
            {                
                ResponseCode = code,
                ErrorMessage = message,
                AuthCode = authCode,
                Checks = check,
            };

            if (response.IsSuccess && suspendTransaction)
            {
                PilotNtInterop.SuspendTrx(iAmount, ans.AuthCode);
            }

            return response;
        }

        /// <summary>
        /// Возврат оплаты. 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public PaymentResponse Return(decimal amount)
        {
            uint iAmount = (uint)(amount * 100);
            var ans = new AuthAnswer7
            {
                TType = PilotNtTransType.Return,
                Amount = iAmount,
            };
            PilotNtInterop.CardAuthorize7(null, ref ans);

            var check = ReadCheck(ans.Check);
            var encoding = Encoding.GetEncoding(1251);
            var code = int.Parse(encoding.GetString(ans.Rcode).TrimEnd('\0'));
            var message = encoding.GetString(ans.AMessage).TrimEnd('\0');
            var authCode = encoding.GetString(ans.AuthCode).TrimEnd('\0');

            return new PaymentResponse
            {
                Checks = check,
                ResponseCode = code,
                ErrorMessage = message,
                AuthCode = authCode,
            };
        }

        /// <summary>
        /// Завершение транзакции
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public bool CommitTransaction(decimal amount, string authCode)
        {
            uint iAmount = (uint)(amount * 100);
            var encoding = Encoding.GetEncoding(1251);
            var bAuthCode = encoding.GetBytes(authCode);
            int resp = PilotNtInterop.CommitTrx(iAmount, bAuthCode);
            return resp == 0;
        }

        public CloseShiftResponse CloseShift()
        {
            var ans = new AuthAnswer
            {
                TType = PilotNtTransType.CloseDay,
            };
            int res = PilotNtInterop.CloseDay(ref ans);
            var encoding = Encoding.GetEncoding(1251);
            var code = int.Parse(encoding.GetString(ans.Rcode).TrimEnd('\0'));
            var message = encoding.GetString(ans.AMessage).TrimEnd('\0');
            return new CloseShiftResponse
            {
                Checks = ReadCheck(ans.Check),
            };
        }

        private static string[] ReadCheck(IntPtr checkPtr)
        {
            if (checkPtr == IntPtr.Zero)
                return null;

            var check = Marshal.PtrToStringAnsi(checkPtr);           
            PilotNtInterop.GlobalFree(checkPtr);

            if (string.IsNullOrEmpty(check))
                return null;

            const string separator = "~S\x01\r\n";

            return check.Split(new [] {separator}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
