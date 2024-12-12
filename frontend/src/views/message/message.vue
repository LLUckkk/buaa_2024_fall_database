<!-- eslint-disable -->

<template>
  <div>
  
    <div class="container">
      <div style="height: 72px">
        <div class="reds-sticky">
          <div class="reds-tabs-list">
            <div class="reds-tab-item active tab-item">
              <div class="badge-container">
                <el-badge v-if="messageCount > 0" :value="messageCount" :max="99" class="item">
                  <span>我的消息</span>
                </el-badge>
                <span v-else>我的消息</span>
              </div>
            </div>
            <!--          <div class="reds-tab-item tab-item">-->
            <!--            <div class="badge-container">-->
            <!--              <span>评论和@</span>-->
            <!--            </div>-->
            <!--          </div>-->
            <!--          <div class="reds-tab-item tab-item">-->
            <!--            <div class="badge-container">-->
            <!--              <span>赞和收藏</span>-->
            <!--            </div>-->
            <!--          </div>-->
            <!--          <div class="reds-tab-item tab-item">-->
            <!--            <div class="badge-container">-->
            <!--              <span>新增关注</span>-->
            <!--            </div>-->
            <!--          </div>-->
          </div>
          <div class="divider" style="margin: 16px  0"></div>
        </div>
      </div>
  
      <Message_list @setNoReadCount="setMessageCount"></Message_list>
    </div>
  </div>
  </template>
  <script>
  import Message_list from "@/views/message/children/message-list.vue";
  import api from "@/api";
  
  export default {
    components: {Message_list},
    data(){
      return {
        messageCount: 0,
      }
    },
    created() {
      this.$api = api;
    },
    methods: {
      setMessageCount(val){
        this.messageCount = val
        this.$eventBus.$emit('noReadMessage', val);
      },
    }
  }
  </script>
  <style lang="less" scoped>
  .container {
    .header {
      padding: 15px 0;
      line-height: 16px;
      font-size: 16px;
      font-weight: 400;
  
      .header-icon {
        position: relative;
        top: 2px;
        display: inline-block;
        width: 6px;
        height: 19px;
        background: #ff2442;
        border-radius: 3px;
        margin-right: 2px;
      }
  
      .header-title {
        line-height: 20px;
        font-size: 20px;
      }
    }
  
    padding-top: 72px;
    width: 67%;
    margin: 0 auto;
    .reds-sticky {
      top: 72px;
      position: fixed;
      z-index: 12;
      width: 40rem;
      box-sizing: border-box;
      height: 72px;
      padding-top: 16px;
      justify-content: center;
      flex-direction: column;
      background: #fff;
      .reds-tabs-list {
        justify-content: flex-start;
        display: flex;
        flex-wrap: nowrap;
        position: relative;
        font-size: 16px;
        .active {
          font-weight: 600;
          color: #333;
          background-color: rgba(0, 0, 0, 0.03);
          border-radius: 20px;
        }
  
        .reds-tab-item {
          padding: 0px 16px;
          margin-right: 0px;
          font-size: 16px;
          display: flex;
          align-items: center;
          box-sizing: border-box;
          height: 40px;
          cursor: pointer;
          color: rgba(51, 51, 51, 0.8);
          white-space: nowrap;
          transition: transform 0.3s cubic-bezier(0.2, 0, 0.25, 1);
          z-index: 1;
  
          .badge-container {
            position: relative;
          }
        }
      }
      .divider {
        margin: 4px 8px;
        list-style: none;
        height: 0;
        border: solid rgba(0, 0, 0, 0.08);
        border-width: 1px 0 0;
      }
      .back-top {
        width: 40px;
        height: 40px;
        background: #fff;
        border: 1px solid rgba(0, 0, 0, 0.08);
        border-radius: 100px;
        color: rgba(51, 51, 51, 0.8);
        display: flex;
        align-items: center;
        justify-content: center;
        transition: background 0.2s;
        cursor: pointer;
      }
    }
  }
  </style>  