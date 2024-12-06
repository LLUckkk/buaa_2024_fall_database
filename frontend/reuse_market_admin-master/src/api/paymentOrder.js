import http from "@/utils/request"
export default {
  getPaymentOrderPage(data) {
    return http.get('/admin/payment/order/page', {params: data})
  },
  getPaymentOrderDetail(id) {
    return http.get('/admin/payment/order/detail/' + id)
  }
}