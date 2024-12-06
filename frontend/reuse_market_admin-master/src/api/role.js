import http from "@/utils/request"
export default {
  getRolePage(data) {
    return http.get('/admin/role/page',{params:data})
  },
  getRoleList(){
    return http.get('/admin/role/list')
  },
  createRole(data){
    return http.post('/admin/role',data)
  },
  editRole(data){
    return http.put('/admin/role', data)
  },
  delRole(data) {
    return http.delete('/admin/role', {params: data})
  }
}