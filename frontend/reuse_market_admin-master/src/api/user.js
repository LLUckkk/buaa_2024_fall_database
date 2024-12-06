import http from "@/utils/request"

export default {
  login(data) {
    return http.post('/public/admin/user/login', data)
  },
  getUserInfo(){
    return http.get('/admin/user/getUserInfo')
  },
  logout(){
    return http.post('/admin/user/logout')
  },
  getUserList(data){
    return http.get('/admin/user/page',{params:data})
  },
  createUser(data){
    return http.post('/admin/user',data)
  },
  delUser(data) {
    return http.delete('/admin/user', {params: data})
  },
  editUser(data) {
    return http.put('/admin/user', data)
  },
}

