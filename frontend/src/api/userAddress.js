import http from "@/utils/request"
export default {
  saveAddress(data) {
    return http.post('/user/address', data)
  },
  updateAddress(data) {
    return http.put('/user/address', data)
  },
  getAddressList() {
  return http.get('/user/address/list')
  },
  deleteAddress(id){
    return http.delete('/user/address/'+id)
  },
}