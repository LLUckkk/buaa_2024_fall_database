import http from "@/utils/request"
export default {
  getMessageList(chatListId){
   return http.get('/chat/message',{params:chatListId})
  },
  updateChatMessageIsRead(chatListId){
    return http.put('/chat/message/'+ chatListId)
  }
}