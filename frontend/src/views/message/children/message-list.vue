<template>
  <div>
    <div class="message-container">
      <div class="message-item" @click="toChat(item)" v-for="item in chatList">
        <div class="user-avatar">
          <el-image class="avatar-item" :src="userInfo.id === item.fromUserId?item.toUserAvatar:item.fromUserAvatar"></el-image>
        </div>
        <div class="main">
          <div class="info">

            <div class="user-info">
              <!-- <a class>{{ item.fromUserNick }}</a> -->
              <a class>{{ userInfo.id === item.fromUserId ? item.toUserNick : item.fromUserNick }}</a>
            </div>

            <div class="interaction-content" style="height: 20px">
              <span>{{ item.chatMessage.content }}</span>

              <div class="msg-count" v-show="item.noReadCount > 0">{{ item.noReadCount }}</div>
            </div>
            <div class="user-info">
              <!-- <div class="interaction-hint"><span>{{ item.updateTime }}</span></div> -->
              <div class="interaction-hint"><span>{{ $utils.convert.formatTime(item.updateTime) }}</span></div>
            </div>
          </div>
          <div class="product-image">
            <el-image style=" width: 100%;height: 100%;border-radius: 5px;" :src="item.productImage"></el-image>
          </div>
        </div>
      </div>
    </div>

    <!--抽屉 聊天侧边栏-->
    <div>
      <el-drawer @onClose="closeChat" @open_main="toMain" :size="'450px'" :show-close="false"
         direction="rtl" v-model="chatVisiable">
        <Chat :chat-list-item="chatListItem" @close="closeChat" @open_main="toMain"
          :product-id="chatListItem.productId"></Chat>
      </el-drawer>
    </div>
    <Main @main_close="closeMain" v-if="mainShow" :productId="productId"></Main>
  </div>
</template>

<script setup>
const testChat = [
  {
    chatMessage: {
      content: "this is a message",
    },
    noReadCount: 5,
    updateTime: "2024-12-07",
    productImage: "@/assets/logo.jpg",
    fromUserNick: "hhh",
    productId: 1,
  },
  {
    chatMessage: {
      content: "this is a message",
    },
    noReadCount: 0,
    updateTime: "2024-12-07",
    productImage: "@/assets/logo.jpg",
    fromUserNick: "hhh",
    productId: 2,
  },
]
</script>

<script>
import Chat from '@/components/Chat.vue'
//import Address_edit from "@/views/publish/address_edit.vue";
import websocket from "@/utils/websocket";
import Main from "@/views/main/main.vue";
import utils from "@/utils";
import api from "@/api";

export default {
  components: {Main, Chat},

  data() {
    return {
      productId: '',
      mainShow: false,
      chatListItem: {},
      chatList: [],
      chatVisiable: false,
      userInfo: {},
      timer: null,
    }
  },
  created() {
    this.$api = api;
    this.$utils = utils;
  },
  mounted() {
    //this.userInfo = this.$store.state.user.userInfo
    this.getUserInfo()
    this.getChatList()
    this.getChatListFlush()
  },
  methods: {
    getUserInfo() {
      this.$api.user.getUserInfo().then(res => {
        this.userInfo = res.data
      })
    },
    getChatList() {
      this.$api.chatList.getChatList().then(res => {
        this.chatList = res.data
        let noReadCount = 0
        this.chatList.forEach(item => {
          noReadCount += item.noReadCount
        })
        this.$emit('setNoReadCount', noReadCount)
      })
    },
    getChatListFlush() {
      this.timer = setInterval(() => {
        this.getChatList()
      }, 1000)
    },
    toChat(item) {
      this.$api.chatMessage.updateChatMessageIsRead(item.id).then(res => {
        item.noReadCount = 0
        this.chatListItem = item
        this.chatVisiable = true;
      })
      item.noReadCount = 0;
      this.chatListItem = item;
      this.chatVisiable = true;
    },
    toMain(productId) {
      this.productId = productId
      this.mainShow = true
    },
    closeMain() {
      this.mainShow = false
    },
    closeChat() {
      this.getChatList()
      this.chatVisiable = false;
    }
  },
  destroyed() {
    clearInterval(this.timer)
  }
}
</script>
<style lang="less" scoped>
/deep/ .el-drawer__header {
  display: none;
}

.animate__animated {
  animation-duration: 0.5s; //动画持续时间
  animation-delay: 0s !important; //动画延迟时间
  //animation-iteration-count: 2 !important;
}

.message-container {
  width: 40rem;

  .message-item {
    display: flex;
    flex-direction: row;
    margin-top: 15px;
    cursor: pointer;

    .user-avatar {
      margin-right: 24px;
      flex-shrink: 0;
      display: flex;
      align-items: center;
      justify-content: center;

      .avatar-item {
        width: 58px;
        height: 58px;
        border-radius: 100%;
        border: 1px solid rgba(0, 0, 0, 0.08);
        object-fit: cover;
      }
    }

    .main {
      flex-grow: 1;
      flex-shrink: 1;
      display: flex;
      flex-direction: row;
      padding-bottom: 10px;
      border-bottom: 1px solid rgba(0, 0, 0, 0.08);

      .info {
        flex-grow: 1;
        flex-shrink: 1;
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: space-around;

        .user-info {
          display: flex;

          a {
            color: #333;
            font-size: 16px;
            font-weight: 600;
          }

          .interaction-hint {
            font-size: 12px;
            color: rgba(51, 51, 51, 0.6);
          }
        }

        .interaction-content {
          display: flex;
          font-size: 14px;
          color: #333;
          line-height: 140%;
          cursor: pointer;
          justify-content: space-between;
          align-items: center;

          .msg-count {
            width: 20px;
            height: 20px;
            line-height: 20px;
            font-size: 13px;
            color: #fff;
            background-color: red;
            text-align: center;
            border-radius: 100%;
            margin-right: 20px;
          }
        }
      }

      .product-image {
        width: 80px;
        height: 80px;
      }
    }
  }
}
</style>
