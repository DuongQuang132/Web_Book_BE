namespace Web_Book_BE.DTO
{
    public class MomoPaymentRequest
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string RequestId { get; set; }
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
        public string ExtraData { get; set; }
        public string RequestType { get; set; }
        public string Signature { get; set; }
        public string Lang { get; set; } = "vi";
    }

    public class MomoPaymentResponse
    {
        public string PayUrl { get; set; }
        public string ErrorCode { get; set; }
        public string LocalMessage { get; set; }
    }

}
