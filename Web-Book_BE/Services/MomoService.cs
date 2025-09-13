using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Web_Book_BE.Services.Interfaces;
using Web_Book_BE.Utils;

namespace Web_Book_BE.Services
{
    public class MomoService : IMomoService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public MomoService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<string> CreatePaymentAsync(decimal amount)
        {
            var config = _configuration.GetSection("MomoAPI");

            string endpoint = config["MomoApiUrl"];
            string partnerCode = config["PartnerCode"];
            string accessKey = config["AccessKey"];
            string secretKey = config["SecretKey"];
            string returnUrl = config["ReturnUrl"];
            string notifyUrl = config["NotifyUrl"];

            // ✅ Thêm validation
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentException("ReturnUrl không được để trống");
            }
            if (string.IsNullOrEmpty(notifyUrl))
            {
                throw new ArgumentException("NotifyUrl không được để trống");
            }

            Console.WriteLine($"ReturnUrl: {returnUrl}");
            Console.WriteLine($"NotifyUrl: {notifyUrl}");

            string orderId = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string orderInfo = "Thanh toán đơn hàng #" + orderId;
            string extraData = "";

            // rawHash để tạo signature
            string rawHash =
                "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + notifyUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + returnUrl +
                "&requestId=" + requestId +
                "&requestType=captureMoMoWallet";

            string signature = MomoUtil.CreateSignature(rawHash, secretKey);

            var paymentRequest = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount = amount.ToString(),
                orderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                extraData,
                requestType = "captureMoMoWallet",
                signature,
                lang = "vi"
            };

            // ✅ Log request để debug
            var json = JsonConvert.SerializeObject(paymentRequest, Formatting.Indented);
            Console.WriteLine($"MoMo Request: {json}");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"MoMo Response: {responseString}");

            return responseString;
        }
    }
    }
