import http from "@/utils/request"
export default {
  getTypeList(){
    return http.get('/product/type')
  }
}