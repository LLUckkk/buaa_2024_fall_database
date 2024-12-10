import http from "@/utils/request"
export default {
  login(data){
    return http.post('/public/login', data)
  },
  register(data){
    return http.post('/public/register', data)
  },
  updateUserInfo(data){
    return http.put('/user', data)
  },
  updateUserPassword(data) {
    return http.put('/user/password', data)
  },
  logout(){
    return http.get('/user/logout')
  },
  getUserInfo(){
    return http.get('/user/getUserInfo');
  },
  getUserInfoById(userId) {
    return http.get('/user/getUserInfo/byId', {params: {userId: userId}});
  },
  getCode(data){
    return http.get('/public/getCheckCode', {params: {phone: data}});
  }

}

