import http from "@/utils/request"
export default {
  getWebUserPage(data) {
    return http.get('/admin/web/user/page', {params: data})
  },
  ableUser(id){
    return http.put('/admin/web/user/able/'+id )
  },
  disableUser(id) {
    return http.put('/admin/web/user/disable/' + id)
  },
  successInfo(id) {
    return http.put('/admin/web/user/check/success/' + id)
  },
  failInfo(id){
    return http.put('/admin/web/user/check/fail/' + id)
  }
}