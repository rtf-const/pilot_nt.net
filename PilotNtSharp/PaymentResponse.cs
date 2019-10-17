namespace PilotNtSharp
{
    public class PaymentResponse
    {
        public int ResponseCode { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public bool IsSuccess => ResponseCode == 0;

        /// <summary>
        /// Образ чека, который необходимо необходимо отправить на печать 
        /// (в нефискальном режиме) перед завершением транзакции
        /// </summary>
        public string[] Checks { get; internal set; }

        /// <summary>
        /// Код авторизации, используемый для комита или отката транзакции
        /// </summary>
        public string AuthCode { get; internal set; }

        /// <summary>
        /// Хеш SHA1 от номера карты
        /// </summary>
        public string CardHash { get; internal set; }
    }
}
