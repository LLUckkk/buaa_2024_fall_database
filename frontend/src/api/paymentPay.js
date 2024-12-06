import http from "@/utils/request"
export default {
  payOrder(paymentPayId){
    return http.put('/payment/pay/finish/'+ paymentPayId)
  },
  createPayOrder(data){
    return http.post('/payment/pay',data)
  }
}