import http from "@/utils/request"
export default {

  showImage(imageName){
    return http.get('/image', {imageName})
  },

  removeImage(fileName){
    return http.get('/upload/image/delete', {params: {fileName: fileName}})
  }
}