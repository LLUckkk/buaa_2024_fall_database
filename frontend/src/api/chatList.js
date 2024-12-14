import http from "@/utils/request"
export default {
  createChat(data){
    return http.post('/chat/list',data)
  },
  getChatList(){
    return http.get('/chat/list/all')
  },
  getChatListById(chatListId){
    return http.get('/chat/list/one',{params: chatListId})
  },
  getNoReadCount(){
    return http.get('/chat/list/unread/total')
  },
}