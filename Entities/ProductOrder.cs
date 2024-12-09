using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities{
[Table("product_order")]
public class ProductOrder
{
    [Key]
    [Column("id")]
    public string Id { get; set; }

    [Column("order_number")]
    public string OrderNumber { get; set; }

    [Column("product_user_id")]
    public string ProductUserId { get; set; }

    [Column("product_id")]
    public string ProductId { get; set; }

    [Column("user_id")]
    public string UserId { get; set; }

    [Column("product_title")]
    public string ProductTitle { get; set; }

    [Column("product_img")]
    public string ProductImg { get; set; }

    [Column("product_price")]
    public long ProductPrice { get; set; }

    [Column("product_type")]
    public string ProductType { get; set; }

    [Column("product_type_name")]
    public string ProductTypeName { get; set; }

    [Column("product_sell_price")]
    public long ProductSellPrice { get; set; }

    [Column("product_num")]
    public int ProductNum { get; set; }

    [Column("product_post")]
    public long ProductPost { get; set; }

    [Column("product_post_status")]
    public int ProductPostStatus { get; set; }

    [Column("product_money")]
    public long ProductMoney { get; set; }

    [Column("product_info")]
    public string ProductInfo { get; set; }

    [Column("buy_money_all")]
    public long BuyMoneyAll { get; set; }

    [Column("buy_money")]
    public long Price { get; set; }

    [Column("buy_info")]
    public string BuyInfo { get; set; }

    [Column("post_mode")]
    public string PostMode { get; set; }

    [Column("post_self_code")]
    public string PostSelfCode { get; set; }

    [Column("post_username")]
    public string PostUsername { get; set; }

    [Column("post_phone")]
    public string PostPhone { get; set; }

    [Column("post_address")]
    public string PostAddress { get; set; }

    [Column("ship_username")]
    public string ShipUsername { get; set; }

    [Column("ship_phone")]
    public string ShipPhone { get; set; }

    [Column("ship_address")]
    public string ShipAddress { get; set; }

    [Column("ship_num")]
    public string ShipNum { get; set; }

    [Column("ship_company")]
    public string ShipCompany { get; set; }

    [Column("ship_time")]
    public DateTime ShipTime { get; set; }

    [Column("pay_status")]
    public int PayStatus { get; set; }

    [Column("pay_order_id")]
    public string PayOrderId { get; set; }

    [Column("deal_status")]
    public int DealStatus { get; set; } = 0;

    [Column("eva_score")]
    public int EvaScore { get; set; }

    [Column("eva_content")]
    public string EvaContent { get; set; }

    [Column("create_time")]
    public DateTime CreateTime { get; set; }

    [Column("update_time")]
    public DateTime UpdateTime { get; set; }
}
}