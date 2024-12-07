<template>
  <div class="user-page">
    <div class="user">
      <div class="user-info">
        <div class="avatar ">
          <div class="avatar-wrapper animate__animated animate__pulse ">
            <img
                :src="userInfo.avatar"
                class="user-image"
                style="border: 1px solid rgba(0, 0, 0, 0.08)"
            />
          </div>
        </div>
        <div class="info-part">
          <div class="info">
            <div class="basic-info">
              <div class="user-basic">
                <div class="user-nickname ">
                  <div class="user-name">{{ userInfo.nickName }}</div>
                </div>
                <div class="user-content">
                  <span class="user-redId">ID：{{ userInfo.number }}</span>
                  <span class="user-IP"> 位置：{{ userInfo.province }} {{ userInfo.city }}</span>
                </div>
              </div>
            </div>
            <div class="user-desc">
              <span v-if="userInfo.intro">{{ userInfo.intro }}</span>
              <span v-else>这家伙很神秘，没有写个人简介。</span>
            </div>
            <div class="user-tags">
              <!--              <div class="tag-item">-->
              <!--                <div>双子座</div>-->
              <!--              </div>-->
              <!--              <div class="tag-item">-->
              <!--                <div>广东广州</div>-->
              <!--              </div>-->
            </div>
            <div class="data-info">
              <div class="user-interactions">
                <div><span class="shows">发布</span><span class="count">{{userStat.publish}}</span></div>
                <div><span class="shows">卖出</span><span class="count">{{userStat.sell}}</span></div>
                <div><span class="shows">买入</span><span class="count">{{userStat.buy}}</span></div>
                <div><span class="shows">收藏</span><span class="count">{{userStat.collect}}</span></div>
              </div>
            </div>
            <!---->
          </div>
          <div class="follow" @click="editUser">修改资料</div>
          <div class="follow1" @click="editAddress">我的地址</div>
        </div>
      </div>
    </div>
    <div class="reds-sticky-box user-page-sticky" style="--1ee3a37c: all 0.4s cubic-bezier(0.2, 0, 0.25, 1) 0s">
      <div class="reds-sticky" style="">
        <div class="tertiary center reds-tabs-list" style="padding: 0 12px">
          <div class="reds-tab-item" @click="toPublish" :class="activeTab==='publish'?'active':''">
            <span>发布的</span>
          </div>
          <div class="reds-tab-item" @click="toSell" :class="activeTab==='sell'?'active':''">
            <span>卖出的</span>
          </div>
          <div class="reds-tab-item" @click="toPurchase" :class="activeTab==='purchase'?'active':''">
            <span>买到的</span>
          </div>
          <div class="reds-tab-item" @click="toCollect" :class="activeTab==='collect'?'active':''">
            <span>收藏的</span>
          </div>
          <!---->
          <div class="active-tag" style="width: 64px; left: 627px"></div>
        </div>
      </div>
    </div>
    <div class="feeds-tab-container" >
      <user-product  :activeTab="activeTab"></user-product>
    </div>
    <!--抽屉 用户编辑-->
    <div>
      <el-drawer destroy-on-close @close-drawer="closeDrawer" :show-close="false" :size="'400px'" v-model="drawer" direction="rtl">
        <user_edit v-if="type === 'user'" @close-drawer="closeDrawer"></user_edit>
        <user_address v-if="type === 'address'" @close-drawer="closeDrawer"></user_address>
      </el-drawer>
    </div>
  </div>
</template>
<script>
import UserProduct from "@/components/UserProduct.vue";
//import Address_edit from "@/views//address_edit.vue";
import User_edit from "@/views/my/user_edit.vue";
//import User_address from "@/views/user/user_address.vue";

export default {
  //components: {User_address, User_edit, Address_edit, UserProduct},
  components: {User_edit, UserProduct},
  data() {
    return {
      drawer: false,
      activeTab: 'publish',
      type:'',
      userInfo: {},
      userStat: {},
    }
  },
  created() {
    //this.getUserInfo()
    //this.getUserStat()
  },
  methods: {
    getUserInfo() {
      this.$api.user.getUserInfo().then(res => {
        this.userInfo = res.result
      })
    },
    getUserStat() {
      this.$api.productOrder.getUserOrderStat().then(res=>{
        this.userStat = res.result
      })
    },
    closeDrawer() {
      this.getUserInfo()
      this.drawer = false
    },
    toPublish() {
      this.activeTab = 'publish'
    },
    toSell() {
      this.activeTab = 'sell'
      // this.$router.push("/agree")
    },
    editUser() {
      this.drawer = true;
      this.type = 'user'
    },
    editAddress(){
      this.drawer = true;
      this.type = 'address'
    },
    toPurchase() {
      this.activeTab = 'purchase'
      // this.$router.push("/agree")
    },
    toCollect(){
      this.activeTab = 'collect'
    },
  }
}
</script>
<style lang="less" scoped>
/deep/ .el-drawer__header {
  display: none;
}

.user-page {
  background: #fff;
  overflow-y: scroll;
  overflow-x: hidden;

  .user {
    padding-top: 72px;
    display: flex;
    align-items: center;
    justify-content: center;

    .user-info {
      display: flex;
      justify-content: center;
      padding: 48px 0;

      .avatar {
        .avatar-wrapper {
          text-align: center;
          width: 250.66667px;
          height: 175.46667px;

          .user-image {
            border-radius: 50%;
            margin: 0 auto;
            width: 70%;
            height: 100%;
            object-fit: cover;
          }
        }
      }

      .info-part {
        position: relative;
        width: 100%;

        .info {
          width: 500px;
          margin-left: 32px;

          .basic-info {
            display: flex;
            align-items: center;

            .user-basic {
              width: 100%;

              .user-nickname {
                width: 100%;
                display: flex;
                align-items: center;
                max-width: calc(100% - 96px);

                .user-name {
                  font-weight: 600;
                  font-size: 24px;
                  line-height: 120%;
                  color: #333;
                }
              }

              .user-content {
                width: 100%;
                font-size: 12px;
                line-height: 120%;
                color: rgba(51, 51, 51, 0.6);
                display: flex;
                margin-top: 8px;

                .user-redId {
                  padding-right: 12px;
                }
              }
            }
          }

          .user-desc {
            width: 100%;
            font-size: 14px;
            line-height: 140%;
            color: #333;
            margin-top: 16px;
            white-space: pre-line;
          }

          .user-tags {
            height: 24px;
            margin-top: 16px;
            display: flex;
            align-items: center;
            font-size: 12px;
            color: #333;
            text-align: center;
            font-weight: 400;
            line-height: 120%;

            .tag-item :first-child {
              padding: 3px 6px;
            }

            .tag-item {
              display: flex;
              align-items: center;
              justify-content: center;
              padding: 4px 8px;
              grid-gap: 4px;
              gap: 4px;
              height: 18px;
              border-radius: 41px;
              background: rgba(0, 0, 0, 0.03);
              height: 24px;
              line-height: 24px;
              margin-right: 6px;
              color: rgba(51, 51, 51, 0.6);
            }
          }

          .data-info {
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top: 20px;

            .user-interactions {
              width: 100%;
              display: flex;
              align-items: center;

              .count {
                font-weight: 500;
                font-size: 14px;
                margin-left: 5px;
              }

              .shows {
                color: rgba(51, 51, 51, 0.6);
                font-size: 14px;
                line-height: 120%;
              }
            }

            .user-interactions > div {
              height: 100%;
              display: flex;
              align-items: center;
              justify-content: center;
              text-align: center;
              margin-right: 16px;
            }
          }
        }

        .follow {
          cursor: pointer;
          position: absolute;
          margin-left: auto;
          display: block;
          right: 0;
          top: 0;
        }

        .follow1 {
          cursor: pointer;
          position: absolute;
          margin-left: auto;
          display: block;
          right: 80px;
          top: 0;
        }
      }
    }
  }

  .reds-sticky {
    padding: 16px 0;
    z-index: 5 !important;
    background: hsla(0, 0%, 100%, 0.98);

    .reds-tabs-list {
      @media screen and (min-width: 1728px) {
        width: 1445.33333px;
      }
      display: flex;
      flex-wrap: nowrap;
      position: relative;
      font-size: 16px;
      justify-content: center;

      .reds-tab-item {
        padding: 0 16px;
        margin-right: 15px;
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
      }

      .reds-tab-item.active {
        background-color: rgba(0, 0, 0, 0.03);
        border-radius: 20px;
        font-weight: 600;
        color: rgba(51, 51, 51, 0.8);
      }
    }
  }

  .feeds-tab-container {
    padding-left: 2rem;
  }
}
</style>
