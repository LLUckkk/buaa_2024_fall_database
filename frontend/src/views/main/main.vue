<!--商品详情页-->
<template>
  <div v-if="productInfo">
    <div class="note-detail-mask animate__animated animate__zoomIn  animate__delay-0.5s">
      <div class="note-container">
        <div class="media-container">
          <el-carousel height="90vh">
            <el-carousel-item v-for="item in productInfo.image" :key="item">
              <el-image
                  style="width: 100%; height: 100%"
                  :src=item
                  fit="cover"
              />
            </el-carousel-item>
          </el-carousel>
        </div>

        <div class="interaction-container">
          <div class="author-container">
            <div class="author-me">
              <div class="info">
                <img
                    class="avatar-item"
                    style="width: 40px; height: 40px"
                    :src="this.productInfo.userInfo.avatar"
                    alt=""/>
                <span class="name">{{ this.productInfo.userInfo.nickname }}</span>
                <!-- <span class="name">user</span> -->
              </div>
            </div>

            <div class="note-scroller">
              <div class="note-content">
                <div class="title">
                  {{ this.productInfo.title }}
                    <!-- 麦当劳 -->
                </div>

                <div class="desc">
                  <span>{{ this.productInfo.intro }} <br/></span>
                  <!-- <span>肯德基疯狂星期四v我50 <br/></span> -->
                  <!--                  <a class="tag tag-search">#海贼王</a>-->
                </div>
                <!-- <div class="post-container">
                  <span class="post" v-if="productInfo.postType ===0 ">发货方式：邮寄 </span>
                  <span class="post" v-if="productInfo.postType ===1 ">发货方式：自提 </span>
                </div> -->
                <div class="post-container">
                  <div class="post">
                    <!-- <span style="font-weight: bold;color: red;font-size: 11px">￥<span style="font-size: 16px">99</span></span> -->
                    <!-- <span style="font-weight: bold;color: red;font-size: 11px">￥<span style="font-size: 16px">{{ $utils.convert.to_price(productInfo.price) }}</span></span> -->
                    <span style="font-weight: bold;color: red;font-size: 11px">￥<span style="font-size: 16px">{{this.$utils.convert.to_price(productInfo.price) }}</span></span>
                  </div>
                </div>

                <div class="bottom-container" style="margin-top: 5px">
                    <span class="date">{{ $utils.convert.parseTime(productInfo.createTime) }}</span>

                  <el-button type="danger" round @click="chatUser" v-if="canChat" >我想要</el-button>

                </div>
              </div>

              <div class="divider interaction-divider"></div>
              <!-- 评论 -->
              <div class="comments-el">
                <Comment :data-list="commentList" :commentCount="commentCount" :productStatus="productInfo.status" :delshow="productInfo.userId === currentUserId" @reply="handleReply" @del="handleDel"></Comment>
              </div>
            </div>
            <div class="interactions-footer" v-if="productInfo.status === 9">
              <div class="buttons">
                <div class="left">
                  <div class="like-wrapper">
                    <span v-if="productInfo.userId != currentUserId ">收藏</span>
                    <div class="like-lottie" style="margin-left: 10px">
                      <div v-if="productInfo.userId != currentUserId">
                        <img src="@/assets/image/collect.png" style="width: 15px;height: 15px;cursor: pointer" alt="" v-if="collect" @click="cancelCollect">
                        <img src="@/assets/image/notCollect.png" style="width: 15px;height: 15px;cursor: pointer" alt="" @click="setCollect" v-else>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="share-wrapper"></div>
              </div>
              <div class="comment-wrapper active comment-comp">
                <div class="input-wrapper">
                  <input class="comment-input" v-model="commentForm.content" type="text" :placeholder="placeholder" @keyup.enter="submitComment"/>
                  <div class="input-buttons" @click="removeContent">
                    <i class="el-icon-circle-close"></i>
                  </div>
                </div>
                <button class="submit" @click="submitComment">发送</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="close-cricle" @click="close()">
        <div class="close close-mask-white">
          <i class="el-icon-close" style="color: rgba(51, 51, 51, 0.8)">
            <el-icon size="large"><Close /></el-icon>
          </i>
        </div>
      </div>
      <div class="back-desk" @click="close"></div>
    </div>
    <!--抽屉 聊天侧边栏-->
    <div>
      <el-drawer @close="closeChat" :show-close="false" destroy-on-close :visible.sync="chatVisiable" direction="rtl">
        <Chat :chat-list-item="chatListItem" @close="closeChat" :product-id="productInfo.id"></Chat>
      </el-drawer>
    </div>
  </div>

</template>

<script>
import Comment from "@/components/Comment.vue";
import Chat from '@/components/Chat.vue'
import websocket from "@/utils/websocket";
import {Notification} from "element-plus";
import api from "@/api";
import utils from "@/utils";
import { ElMessage } from 'element-plus'

export default {
  components: {Comment, Chat},
  emits: ['main_close'],
  props: {
    productId: {
      default: ''
    },
    ableVoucher: {
      default: true
    },
  },
  data() {
    return {
      chatVisiable: false,
      collect: false,
      voucherVisable: false,
      placeholder: '回复内容',
      chatListItem: {},
      productInfo: {
        userInfo: {
          nickName: ''
        },
        productVoucher: {}
      },
      commentList: [],
      commentCount: 0,
      commentForm: {
        productId: "",
        content: "",
        parentId: "",
      },
      currentUserId: null,
      someData: null,
      loading: false
    }
  },
  created() {
    this.$api = api
    this.$utils = utils
    this.getCurrentUserId()
    this.getProductInfo()
    this.getCommentList()
    this.getCollectList()
  },
  computed: {
    canChat() {
      return this.productInfo.userId !== this.currentUserId
        && this.productInfo.status === 1;
    }
  },
  methods: {
    getProductInfo() {
      //alert(this.productId);
      this.$api.product.getProductInfo({id: this.productId})
        .then(res => {
          this.productInfo = res.data;
        })
        .catch(error => {
          console.error('API 请求失败', error);
        });
    },
    handleDel(){
      this.getCommentList()
    },
    getCollectList() {
      this.$api.productCollect.getCollectList().then(res => {
        let collectList = res.data
        let collect = collectList.find(item => item === this.productId)
        if (collect) {
          this.collect = true
        }
      })
    },
    sekill() {
      this.$api.product.seckillVoucher(this.productInfo.productVoucher.id).then(res => {
        Notification({type: 'success', title: '航游集市', message: '抢购成功'})
        this.getProductInfo()
      })
    },
    cancelCollect() {
      this.$api.productCollect.deleteCollect(this.productId).then(res => {
        this.collect = false
        Notification({type: 'success', title: '航游集市', message: '取消收藏'})
      })
    },
    async setCollect() {
      try {
        const response = await this.$api.user.getUserInfo()
        const userId = response.data.id
        
        if (!userId) {
          Notification({
            type: 'warning',
            title: '航游集市',
            message: '请先登录'
          })
          return
        }

        const collectData = {
          UserId: userId,
          Id: this.productId,
          productId: this.productId
        }
        
        await this.$api.productCollect.saveCollect(collectData)
        this.collect = true
        Notification({type: 'success', title: '航游集市', message: '收藏成功'})
        
      } catch (error) {
        console.error('收藏失败:', error)
        Notification({
          type: 'error',
          title: '航游集市',
          message: '收藏失败，请重试'
        })
      }
    },
    async chatUser() {
      this.$api.product.addProductLike(this.productId)
      let data = {
        fromUserId: this.currentUserId,
        toUserId: this.productInfo.userId,
        productId: this.productInfo.id,
        productImage: this.productInfo.image[0],
      }
      try {
        const chatListId = await this.$api.chatList.createChat(data)
        const chatListData = await this.$api.chatList.getChatListById({chatListId: chatListId.data})
        this.chatListItem = chatListData.data
        this.chatVisiable = true
      } catch (error) {
        console.error('创建聊天失败:', error)
      }
    },
    closeChat() {
      this.chatVisiable = false;

    },
    submitComment() {
      if (!this.commentForm.content) {
        ElMessage.warning('请输入评论内容');
        return;
      }
      
      this.commentForm.productId = this.productId;
      
      console.log('当前商品ID:', this.productId);
      console.log('评论表单数据:', this.commentForm);
      
      this.$api.comment.saveComment(this.commentForm)
        .then(res => {
          if (res.code === 200) {
            this.commentForm.content = '';
            this.commentForm.parentId = '';
            this.placeholder = '回复内容';
            this.getCommentList();
            ElMessage.success('评论成功');
          } else {
            ElMessage.error(res.message || '评论失败');
          }
        })
        .catch(error => {
          console.error('评论失败:', error);
          ElMessage.error('评论失败，请重试');
        });
    },
    handleReply(item) {
      this.commentForm.parentId = item.id;
      this.placeholder = '回复：' + item.pubNickname;
    },
    removeContent() {
      this.commentForm.content = ''
      this.commentForm.parentId = ''
      this.placeholder = '回复内容'
    },
    getCommentList() {
      this.$api.comment.getCommentList({productId: this.productId}).then(res => {
        this.commentList = res.data.commentList
        this.commentCount = res.data.commentCount
      })
    },
    close() {
      this.$emit('main_close')
    },
    async getCurrentUserId() {
      try {
        const response = await this.$api.user.getUserInfo()
        this.currentUserId = response.data.id
      } catch (error) {
        console.error('获取用户信息失败:', error)
      }
    },
    async someAsyncMethod() {
      try {
        this.loading = true
        // 异步操作
      } catch (error) {
        console.error(error)
      } finally {
        if (this.$el) {  // 检查组件是否还存在
          this.loading = false
        }
      }
    }
  },
  beforeUnmount() {
    // 清理所有可能的副作用
    // 比如定时器、事件监听器等
  }
}
</script>

<style lang="less" scoped>
/deep/ .el-drawer__header {
  display: none;

}

/deep/ .el-card__body {
  padding: 10px;
}


.note-detail-mask {
  position: fixed;
  left: 0;
  top: 0;
  display: flex;
  width: 100vw;
  height: 100vh;
  z-index: 20;
  overflow: auto;


  .back-desk {
    position: fixed;
    background-color: #f4f4f4;
    opacity: 0.5;
    width: 100vw;
    height: 100vh;
    z-index: 30;
  }

  .close-cricle {
    right: 3.3vw;
    top: 1.3vw;
    position: fixed;
    display: flex;
    z-index: 100;
    cursor: pointer;

    .close-mask-white {
      box-shadow: 0 2px 8px 0 rgba(0, 0, 0, 0.04),
      0 1px 2px 0 rgba(0, 0, 0, 0.02);
      border: 1px solid rgba(0, 0, 0, 0.08);
    }

    .close {
      display: flex;
      justify-content: center;
      align-items: center;
      border-radius: 100%;
      width: 40px;
      height: 40px;
      cursor: pointer;
      transition: all 0.3s;
    }
  }

  .note-container {
    width: 86%;
    height: 90%;
    transition: transform 0.4s ease 0s, width 0.4s ease 0s;
    transform: translate(104px, 32px) scale(1);
    overflow: visible;
    display: flex;
    border-radius: 20px;
    background: #f8f8f8;
    transform-origin: left top;
    z-index: 100;

    .media-container {
      width: 68%;
      position: relative;
      background: rgba(0, 0, 0, 0.03);
      flex-shrink: 0;
      flex-grow: 0;
      -webkit-user-select: none;
      user-select: none;
      overflow: hidden;
      border-radius: 20px 0 0 20px;
      transform: translateZ(0);
      height: 100%;
      object-fit: contain;
      min-width: 440px;
    }

    .interaction-container {
      width: 32%;
      flex-shrink: 0;
      border-radius: 0 20px 20px 0;
      position: relative;
      display: flex;
      flex-direction: column;
      flex-grow: 1;
      height: 100%;
      background-color: #fff;
      overflow: hidden;
      border-left: 1px solid rgba(0, 0, 0, 0.08);

      .author-me {
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        padding: 24px;
        border-bottom: 1px solid transparent;

        .info {
          display: flex;
          align-items: center;

          .avatar-item {
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            border-radius: 100%;
            border: 1px solid rgba(0, 0, 0, 0.08);
            object-fit: cover;
          }

          .name {
            padding-left: 12px;
            height: 40px;
            display: flex;
            align-items: center;
            font-size: 16px;
            color: rgba(51, 51, 51, 0.8);
          }
        }
      }

      .note-scroller::-webkit-scrollbar {
        display: none;
      }

      .note-scroller {
        transition: scroll 0.4s;
        overflow-y: scroll;
        flex-grow: 1;
        height: 80vh;

        .note-content {
          padding: 0 24px 24px;
          color: var(--color-primary-label);

          .title {
            margin-bottom: 8px;
            font-weight: 600;
            font-size: 18px;
            line-height: 140%;
          }

          .desc {
            margin: 0;
            font-weight: 400;
            font-size: 16px;
            line-height: 150%;
            color: #333;
            white-space: pre-wrap;
            overflow-wrap: break-word;

            .tag-search {
              cursor: pointer;
            }

            .tag {
              margin-right: 2px;
              color: #13386c;
            }
          }

          .post-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 10px;

            .post {
              font-size: 14px;
              line-height: 120%;
              color: rgba(51, 51, 51, 0.6);
            }
          }

          .bottom-container {
            display: flex;
            justify-content: space-between;
            align-items: center;

            .date {
              font-size: 14px;
              line-height: 120%;
              color: rgba(51, 51, 51, 0.6);
            }

            .contact {
              font-size: 14px;
              line-height: 120%;
              color: rgba(51, 51, 51, 0.6);
            }
          }
        }


        .divider {
          list-style: none;
          height: 0;
          border: solid rgba(0, 0, 0, 0.08);
          border-width: 1px 0 0;
        }

        .comments-el {
          position: relative;

          .comments-container {
            padding: 16px;

            .total {
              font-size: 14px;
              color: rgba(51, 51, 51, 0.6);
              margin-left: 8px;
              margin-bottom: 12px;
            }

            .list-container {
              position: relative;

              .parent-comment {
                margin-bottom: 20px;

                .comment-item {
                  position: relative;
                  display: flex;
                  padding: 8px;

                  .comment-inner-container {
                    position: relative;
                    display: flex;
                    z-index: 1;
                    width: 100%;
                    flex-shrink: 0;

                    .avatar {
                      flex: 0 0 auto;

                      .avatar-item {
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        cursor: pointer;
                        border-radius: 100%;
                        border: 1px solid rgba(0, 0, 0, 0.08);
                        object-fit: cover;
                        width: 40px;
                        height: 40px;
                      }
                    }

                    .right {
                      margin-left: 12px;
                      display: flex;
                      flex-direction: column;
                      font-size: 14px;
                      flex-grow: 1;

                      .author-wrapper {
                        display: flex;
                        justify-content: space-between;
                        align-items: center;

                        .author {
                          display: flex;
                          align-items: center;

                          .name {
                            color: rgba(51, 51, 51, 0.6);
                            line-height: 18px;
                          }
                        }
                      }

                      .content {
                        margin-top: 4px;
                        line-height: 140%;
                        color: #333;
                      }

                      .info {
                        display: flex;
                        flex-direction: column;
                        justify-content: space-between;
                        font-size: 12px;
                        line-height: 16px;
                        color: rgba(51, 51, 51, 0.6);

                        .date {
                          margin: 8px 0;
                        }

                        .interactions {
                          display: flex;
                          margin-left: -2px;

                          .like-wrapper {
                            padding: 0 4px;
                            color: rgba(51, 51, 51, 0.8);
                            font-weight: 500;

                            position: relative;
                            cursor: pointer;
                            display: flex;
                            align-items: center;

                            .like-lottie {
                              width: 16px;
                              height: 16px;
                              left: 4px;
                            }

                            .count {
                              margin-left: 2px;
                              font-weight: 500;
                            }
                          }
                        }
                      }
                    }
                  }
                }

                .reply-container {
                  margin-left: 52px;

                  .show-more {
                    margin-bottom: 100px;
                    margin-left: 44px;
                    height: 32px;
                    line-height: 32px;
                    color: #13386c;
                    cursor: pointer;
                    font-weight: 500;
                    font-size: 14px;
                  }
                }
              }
            }
          }
        }
      }

      .interactions-footer {
        position: absolute;
        bottom: 0;
        background: #fff;
        flex-shrink: 0;
        padding: 12px 24px 24px;
        height: 100px;
        border-top: 1px solid rgba(0, 0, 0, 0.08);
        flex-basis: 130px;
        z-index: 1;
        width: 100%;

        .buttons {
          display: flex;
          justify-content: space-between;

          .count {
            margin-left: 6px;
            margin-right: 12px;
            font-weight: 500;
            font-size: 12px;
          }

          .left {
            display: flex;

            .like-wrapper {
              position: relative;
              cursor: pointer;
              display: flex;
              justify-content: left;
              color: rgba(51, 51, 51, 0.6);
              margin-right: 5px;
              align-items: center;

              .like-lottie {
                transform: scale(1.7);
              }
            }

            .collect-wrapper {
              position: relative;
              cursor: pointer;
              display: flex;
              color: rgba(51, 51, 51, 0.6);
              margin-right: 5px;
              align-items: center;

              .like-lottie {
                transform: scale(1.7);
              }
            }

            .chat-wrapper {
              cursor: pointer;
              color: rgba(51, 51, 51, 0.6);
              display: flex;
              align-items: center;

              .like-lottie {
                transform: scale(1.7);
              }
            }
          }
        }

        .comment-wrapper {
          &.active {
            .input-wrapper {
              flex-shrink: 1;
            }
          }
        }

        .comment-wrapper {
          display: flex;
          font-size: 16px;
          overflow: hidden;

          .input-wrapper {
            display: flex;
            position: relative;
            width: 100%;
            flex-shrink: 0;
            transition: flex 0.3s;

            .comment-input:placeholder-shown {
              background-image: none;
              padding: 12px 92px 12px 16px;
              background-repeat: no-repeat;
              background-size: 16px 16px;
              background-position: 16px 12px;
              color: rgba(51, 51, 51, 0.3);
            }

            .comment-input {
              padding: 12px 92px 12px 16px;
              width: 100%;
              height: 40px;
              line-height: 16px;
              background: rgba(0, 0, 0, 0.03);
              caret-color: rgba(51, 51, 51, 0.3);
              border-radius: 22px;
              border: none;
              outline: none;
              resize: none;
              color: #333;
            }

            .input-buttons {
              cursor: pointer;
              position: absolute;
              right: 0;
              top: 0;
              height: 40px;
              display: flex;
              align-items: center;
              justify-content: center;
              width: 40px;
              color: rgba(51, 51, 51, 0.3);
            }
          }

          .submit {
            margin-left: 8px;
            width: 60px;
            height: 40px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #fff;
            font-weight: 600;
            cursor: pointer;
            flex-shrink: 0;
            background: #3d8af5;
            border-radius: 44px;
            font-size: 16px;
          }
        }

        .comment-comp {
          margin-top: 20px;
        }
      }
    }
  }
}

.main-component {
  // 添加过渡效果
  transition: all 0.05s ease-out;
  
  // 优化初始显示效果
  &.animate__fadeIn {
    animation-duration: 0.05s !important;
  }
  
  &.animate__fadeOut {
    animation-duration: 0.05s !important;
  }
}
</style>