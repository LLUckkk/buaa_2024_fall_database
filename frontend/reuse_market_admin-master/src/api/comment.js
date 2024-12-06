import http from "@/utils/request"
export default {
  getCommentPage(data) {
    return http.get('/admin/comment/page', {params: data})
  },
  delComment(id){
    return http.delete('/admin/comment/'+id )
  }
}