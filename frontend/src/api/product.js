import http from "@/utils/request"
export default {
  createProductInfo(data){
    return http.post('/product/info',data)
  },
  getProductList(data){
    return http.get('/product/info/page', {params:data})
  },
  getProductInfo(data){
    return http.get('/product/info/detail', {params: data})
  },
  addProductLike(productId){
    return http.put('/product/info/like/count/'+productId)
  },
  getMyProductList(){
    return http.get('/product/info/my')
  },
  delProductInfo(id){
    return http.delete('product/info/'+id)
  },
  getCollectProduct(){
    return http.get('/product/info/my/collect')
  },
  seckillVoucher(id){
    return http.post('/voucher/order/seckill/'+id)
  },
  disableProduct(id){
    return http.put('/product/info/disable/'+id)
  }
}