import http from "@/utils/request"
export default {
  getProductTypePage(data){
    return http.get('/admin/product/type/page', {params: data})
  },
  createProdcutType(data){
    return http.post('/admin/product/type',  data)
  },
  editProductType(data){
    return http.put('/admin/product/type',data)
  }
}