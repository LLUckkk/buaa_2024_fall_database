import http from "@/utils/request"
export default {
  getTrendProduct(){
    return http.get('/product/info/list/my')
  }
}