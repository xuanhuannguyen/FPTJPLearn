using JPLearn.Core.Orders;
using JPLearn.Core.Orders.Entities;
using Microsoft.Extensions.Configuration;

namespace JPLearn.Infrastructure.Services.Payments;

public class SePayProvider : IPaymentProvider
{
    private readonly IConfiguration _configuration;
    public string ProviderName => "SePay";

    public SePayProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<PaymentLinkResult> CreatePaymentLinkAsync(Order order)
    {
        // SePay dùng VietQR chuẩn, có thể tạo URL QR trực tiếp từ thông tin ngân hàng
        // Định dạng: https://qr.sepay.vn/img?acc=[STK]&bank=[NGAN_HANG]&amount=[TIEN]&des=[NOI_DUNG]
        
        var merchantId = _configuration["PaymentSettings:SePay:MerchantId"];
        
        // Tạo nội dung chuyển khoản duy nhất để khớp lệnh: JP [ShortGuid]
        var shortId = order.Id.ToString().Split('-')[0].ToUpper();
        var description = $"JP {shortId}";
        
        // Giả sử bạn dùng MB Bank (STK và Tên sẽ cấu hình ở SePay Dashboard)
        // SePay cho phép tạo link QR nhanh:
        var qrUrl = $"https://qr.sepay.vn/img?acc={merchantId}&amount={order.Amount}&des={description}";

        return Task.FromResult(new PaymentLinkResult(
            Success: true,
            PaymentUrl: qrUrl,
            ExternalId: description // Dùng nội dung chuyển khoản làm ID đối soát
        ));
    }
}
