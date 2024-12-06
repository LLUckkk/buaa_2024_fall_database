import http from "@/utils/request"
export default {
  removeImage(fileName){
    return http.get('/upload/image/delete', {params: {fileName: fileName}})
  }
}