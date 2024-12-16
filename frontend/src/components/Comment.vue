<template>
  <div class="comments-container">
    <div class="total">共{{ commentCount }}条评论</div>
    <div class="list-container">
      <div class="parent-comment" v-for="(oneComment, oneIndex) in dataList.filter(comment => comment.parentId === null)" :key="oneIndex">
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
import utils from "@/utils";
import api from "@/api";
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
  created() {
    this.$utils = utils
    this.$api = api
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
          Notification({type: 'success', title: '航游集市', message: '删除评论成功'})
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
  max-width: 680px;
  margin: 0 auto;

  .total {
    font-size: 14px;
    color: #666;
    margin: 0 8px 12px 8px;
    text-align: center;
  }

  .list-container {
    position: relative;

    .parent-comment {
      background: #fff;
      border-radius: 8px;
      margin-bottom: 16px;
      padding: 12px;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      width: 100%;
      max-width: 600px;

      .comment-item {
        display: flex;
        align-items: flex-start;
        padding: 8px 0;

        .comment-inner-container {
          display: flex;
          width: 100%;

          .avatar {
            .avatar-item {
              width: 40px;
              height: 40px;
              border-radius: 50%;
              margin-right: 8px;
            }
          }

          .right {
            flex: 1;

            .author-wrapper {
              .author {
                .name {
                  font-size: 14px;
                  font-weight: bold;
                  color: #333;
                  margin-bottom: 4px;
                }
              }
            }

            .content {
              font-size: 15px;
              line-height: 1.6;
              color: #333;
              word-break: break-word;
            }

            .info {
              display: flex;
              align-items: center;
              font-size: 12px;
              color: #999;
              margin-top: 4px;

              .date {
                margin-right: 8px;
              }

              .reply {
                color: #3a64ff;
                cursor: pointer;
                margin-left: 8px;
                transition: color 0.3s ease;

                &:hover {
                  color: #2a50d4;
                }
              }
            }
          }
        }
      }

      .reply-container {
        margin-left: 48px;
        margin-top: 8px;
        background: #f9f9f9;
        border-radius: 4px;
        padding: 8px;

        .comment-item {
          padding: 8px 0;

          .comment-inner-container {
            .avatar {
              .avatar-item {
                width: 36px;
                height: 36px;
              }
            }

            .right {
              margin-left: 8px;

              .content {
                font-size: 14px;
                color: #666;
              }

              .info {
                .date {
                  font-size: 12px;
                  color: #999;
                }
              }
            }
          }
        }
      }
    }
  }
}
</style>