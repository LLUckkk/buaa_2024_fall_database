import http from "@/utils/request"
export default {
  createChat(data){
    return http.post('/chat/list',data)
  },
  getChatList(){
    return http.get('/chat/list/all')
  },
  getChatListById(chaListId){
    return http.get('/chat/list/one',{params: chaListId})
  },
  getNoReadCount(){
    return http.get('/chat/list/noRead/total')
  },
}