import http from "@/utils/request"
export default {
  getCommentList(productId){
    return http.get('/comment/byProduct', {params: productId})
  },
  saveComment(data){
    return http.post('/comment',data)
  },
  delComment(id){
    return http.delete('/comment/'+id)
  }
}