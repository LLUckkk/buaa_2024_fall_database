import http from "@/utils/request"
export default {
  uploadImage(file){
    return http.post('/upload/image',file,{headers:{"Content-type": "multipart/form-data"}})
  }
}