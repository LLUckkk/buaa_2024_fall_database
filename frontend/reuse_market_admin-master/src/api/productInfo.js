import http from "@/utils/request"

export default {
  getProductInfoPage(data) {
    return http.get('/admin/product/info/page', {params: data})
  },
  getProductInfoDetail(id) {
    return http.get('/admin/product/info/detail/' + id)
  },
  passProduct(id) {
    return http.put('/admin/product/info/pass/' + id)
  },
failProduct(id) {
    return http.put('/admin/product/info/fail/' + id)
  },
  downProduct(id) {
    return http.put('/admin/product/info/down/' + id)
  },
  addVoucher(data) {
    return http.post('/admin/product/voucher', data)
  },
  delVoucher(id) {
    return http.delete('/admin/product/voucher/' + id)
  }
}