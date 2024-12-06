import http from "@/utils/request"
export default {
  getCountData(){
    return http.get('/admin/stat/countData')
  },
  getUserData(){
    return http.get('/admin/stat/userData')
  },
  getTableData(){
    return http.get('/admin/stat/tableData')
  },
  getVideoData(){
    return http.get('/admin/stat/videoData')
  },
  getOrderData(){
    return http.get('/admin/stat/orderData')
  }
}