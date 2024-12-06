import http from "@/utils/request"
export default {
  saveCollect(data){
    return http.post('/user/collect',data)
  },
  getCollectList(){
    return http.get('/user/collect/list')
  },
  deleteCollect(id){
    return http.delete('/user/collect/'+id)
  },
}