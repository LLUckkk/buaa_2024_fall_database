import http from "@/utils/request"
export default {
  getUserOrderStat(){
    return http.get('/product/order/stat')
  },
  getProductOrderInfo(productOrderId){
    return http.get('/product/order',{params:{productOrderId}})
  },
  createProductOrder(data){
    return http.post('/product/order',data)
  },
  getMySellProductOrder(){
    return http.get('/product/order/my/sell')
  },
  getMyBuyProductOrder() {
    return http.get('/product/order/my/buy')
  },
  userSelfProduct(productOrderId){
    return http.put('/product/order/user/self/'+ productOrderId)
  },
  postProductOrder(data){
    return http.put('/product/order/post',data)
  },
  postSelfProductOrder(data) {
    return http.put('/product/order/selfPost', data)
  },
  confirmPost(productOrderId){
    return http.put('/product/order/post/confirm/' + productOrderId)
  },
  getProductOrderDetail(productOrderId){
    return http.get('/product/order/detail', {params: {productOrderId}})
  },
  evaluateOrder(data){
    return http.put('/product/order/evaluate',data)
  }
}