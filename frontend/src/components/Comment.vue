<template>
  <div class="comments-container">
    <div class="total">共{{ commentCount }}条评论</div>
    <div class="list-container">
      <div class="parent-comment" v-for="(oneComment, oneIndex) in dataList" :key="oneIndex">
        <div class="comment-item">
          <div class="comment-inner-container">
            <div class="avatar">
              <img class="avatar-item" :src="oneComment.pubAvatar"/>
            </div>
            <div class="right">
              <div class="author-wrapper">
                <div class="author">
                  <a class="name">{{ oneComment.pubNickname }}</a>
                </div>
              </div>
              <div class="content">{{ oneComment.content }}</div>
              <div class="info">
                <div class="date">
                  <span>{{ $utils.convert.formatTime(oneComment.createTime) }}</span> <span>{{ oneComment.pubProvince }}</span>
                  <span class="reply" @click="reply(oneComment)" v-if="productStatus === 9">回复</span>
                  <span class="reply" v-if="delshow && productStatus === 9" style="color: #3a64ff;margin-left: 10px" @click="del(oneComment)">删除</span>
                </div>

              </div>
            </div>
          </div>
        </div>
        <div class="reply-container">
          <div class="list-container">
            <div class="comment-item" v-for="(twoComment, twoIndex) in oneComment.commentChildren" :key="twoIndex">
              <div class="comment-inner-container">
                <div class="avatar">
                  <img class="avatar-item" :src="twoComment.pubAvatar"/>
                </div>
                <div class="right">
                  <div class="author-wrapper">
                    <div class="author">
                      <a class="name">{{ twoComment.pubNickname }}</a>
                    </div>
                  </div>
                  <div class="content">
                    回复 <span style="color: rgba(61, 61, 61, 0.8)"
                  >{{ twoComment.parentUserNickname }}: </span
                  >{{ twoComment.content }}
                  </div>
                  <div class="info">
                    <div class="date">
                      <span>{{ twoComment.time }}</span>
                    </div>
                    <div class="interactions">
                      <div
                          class="reply"
                          @click="saveComment(twoComment, oneIndex, twoIndex)"
                      >
                        <span class="date">
                            <span>{{ $utils.convert.formatTime(twoComment.createTime) }}</span> <span>{{ twoComment.pubProvince }}</span>
                          <!--                          <ChatRound style="width: 1.2em; height: 1.2em" />-->
                          <span class="reply" @click="reply(twoComment)" v-if="productStatus === 9">回复</span>
                          <span class="reply" style="color: #3a64ff;margin-left: 10px" v-if="delshow && productStatus === 9" @click="del(twoComment)" >删除</span>

                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div style="padding-bottom: 100px"></div>
  </div>
</template>
<script>
import {Notification} from "element-plus";

export default {
  props: {
    dataList: {
      default: [],
    },
    commentCount: {
      default: 0,
    },
    delshow:{
      default: false
    },
    productStatus:{
      default:0
    }
  },
  data() {
    return {}
  },
  methods: {
    likeComment() {
    },
    del(item){
      console.log(item)
      this.$confirm('确认删除此评论?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.comment.delComment(item.id).then(res => {
          Notification({type: 'success', title: '闲宝交易平台', message: '删除评论成功'})
          this.$emit("del", item)
        })
      })
    },
    reply(item) {
      this.$emit("reply", item)
    },
    saveComment() {
    },
  }
}
</script>
<style lang="less" scoped>
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
              word-break: break-all;
              width: 90%;
            }

            .info {
              display: flex;
              flex-direction: column;
              justify-content: space-between;
              font-size: 12px;
              line-height: 16px;
              color: rgba(51, 51, 51, 0.6);

              .date {
                margin: 5px 0;

                .reply {
                  margin-left: 2px;
                  font-weight: 500;
                  cursor: pointer;
                  color: rgba(51, 51, 51, 0.8);
                }
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
</style>