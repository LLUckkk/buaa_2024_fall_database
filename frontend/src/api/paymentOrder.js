import http from "@/utils/request"
export default {
  createPaymentOrder(data){
    return http.post('/payment/order',data)
  },
  getPaymentOrder(paymentOrderId){
    return http.get('/payment/order',{params:{paymentOrderId: paymentOrderId}})
  }
}