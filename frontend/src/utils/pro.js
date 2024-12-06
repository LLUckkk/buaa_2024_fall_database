export default {

  paymentStatus: [
    {value: 0, text: "待生成付款"},
    {value: 1, text: "待付款"},
    {value: 2, text: "已付款"},
    {value: 3, text: "付款失败"},
    {value: 8, text: "关闭订单"},
    {value: 9, text: "完成订单"}
  ],
  orderStatus: [
    {value: 0, text: "临时"},
    {value: 1, text: "交易失败"},
    {value: 2, text: "付款中"},
    {value: 3, text: "待发货"},
    {value: 4, text: "待确认"},
    {value: 7, text: "退货中"},
    {value: 9, text: "交易成功"},
    {value: 11, text: "评价成功"}
  ],
  payStatus: [
    {value: 0, text: "待支付"},
    {value: 1, text: "支付中"},
    {value: 9, text: "支付完成"}
  ]
}