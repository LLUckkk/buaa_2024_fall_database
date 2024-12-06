import http from "@/utils/request"
export default {
  getProductOrderPage(data) {
    return http.get('/admin/product/order/page', {params: data})
  },
  getProductOrderDetail(id) {
    return http.get('/admin/product/order/detail/' + id)
  },
  getProductOrderEvaPage(data) {
    return http.get('/admin/product/order/page/evaluate', {params: data})
  },
}