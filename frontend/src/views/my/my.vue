<template>
  <div class="user-page">
    <!-- 个人信息卡片部分 -->
    <div class="user-profile">
      <div class="user-info">
        <!-- 头像部分 -->
        <div class="avatar-section">
          <div class="avatar-wrapper animate__animated animate__pulse">
            <img
              :src="userInfo.avatar"
              class="user-avatar"
              alt="用户头像"
            />
          </div>
        </div>

        <!-- 用户信息部分 -->
        <div class="info-section">
          <div class="info-content">
            <!-- 基本信息 -->
            <div class="basic-info">
              <div class="user-name">{{ userInfo.nickname }}</div>
              <div class="user-meta">
                <span class="meta-item">
                  <el-icon><User /></el-icon>
                  ID：{{ userInfo.studentId }}
                </span>
                <span class="meta-item">
                  <el-icon><Location /></el-icon>
                  位置：{{ userInfo.address }}
                </span>
              </div>
            </div>

            <!-- 个人简介 -->
            <div class="user-bio">
              <p v-if="userInfo.intro">{{ userInfo.intro }}</p>
              <p v-else class="bio-placeholder">这家伙很神秘，没有写个人简介。</p>
            </div>

            <!-- 数据统计 -->
            <div class="user-stats">
              <div class="stat-item">
                <span class="stat-label">发布</span>
                <span class="stat-value">{{userStat.publish}}</span>
              </div>
              <div class="stat-item">
                <span class="stat-label">卖出</span>
                <span class="stat-value">{{userStat.sell}}</span>
              </div>
              <div class="stat-item">
                <span class="stat-label">买入</span>
                <span class="stat-value">{{userStat.buy}}</span>
              </div>
              <div class="stat-item">
                <span class="stat-label">收藏</span>
                <span class="stat-value">{{userStat.collect}}</span>
              </div>
            </div>
          </div>

          <!-- 操作按钮 -->
          <div class="action-buttons">
            <el-button 
              type="primary" 
              class="action-btn" 
              @click="editUser"
              icon="Edit"
            >
              修改资料
            </el-button>
            <el-button 
              class="action-btn" 
              @click="editAddress"
              icon="Location"
            >
              我的地址
            </el-button>
          </div>
        </div>
      </div>
    </div>

    <!-- 标签页导航 -->
    <div class="tabs-section">
      <div class="tabs-container">
        <div class="tab-list">
          <div 
            v-for="(tab, index) in tabs" 
            :key="index"
            class="tab-item"
            :class="{ active: activeTab === tab.key }"
            @click="switchTab(tab.key)"
          >
            {{ tab.label }}
          </div>
        </div>
      </div>
    </div>

    <!-- 内容区域 -->
    <div class="content-section">
      <user-product :activeTab="activeTab"></user-product>
    </div>

    <!-- 抽屉组件 -->
    <el-drawer 
      v-model="drawer" 
      :size="400"
      :show-close="false"
      direction="rtl"
      destroy-on-close
    >
      <user_edit v-if="type === 'user'" @close-drawer="closeDrawer"></user_edit>
      <user_address v-if="type === 'address'" @close-drawer="closeDrawer"></user_address>
    </el-drawer>
  </div>
</template>

<script>
import UserProduct from "@/components/UserProduct.vue"
import User_edit from "@/views/my/user_edit.vue"
import User_address from "@/views/my/user_address.vue"
import api from "@/api"

export default {
  components: {
    User_address,
    User_edit,
    UserProduct
  },

  data() {
    return {
      drawer: false,
      activeTab: 'publish',
      type: '',
      userInfo: {},
      userStat: {},
      tabs: [
        { key: 'publish', label: '发布的' },
        { key: 'sell', label: '卖出的' },
        { key: 'purchase', label: '买到的' },
        { key: 'collect', label: '收藏的' }
      ]
    }
  },

  created() {
    this.$api = api
    this.getUserInfo()
    this.getUserStat()
  },

  methods: {
    async getUserInfo() {
      try {
        const res = await this.$api.user.getUserInfo()
        this.userInfo = res.data
      } catch (error) {
        ElMessage.error('获取用户信息失败')
      }
    },

    async getUserStat() {
      try {
        const res = await this.$api.productOrder.getUserOrderStat()
        this.userStat = res.data
      } catch (error) {
        ElMessage.error('获取统计信息失败')
      }
    },

    switchTab(tab) {
      this.activeTab = tab
    },

    closeDrawer() {
      this.getUserInfo()
      this.drawer = false
    },

    editUser() {
      this.type = 'user'
      this.drawer = true
    },

    editAddress() {
      this.type = 'address'
      this.drawer = true
    }
  }
}
</script>

<style lang="less" scoped>
.user-page {
  min-height: 100vh;
  background-color: var(--el-bg-color);
  padding: 24px;
}

.user-profile {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px;
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04);
}

.user-info {
  display: flex;
  gap: 48px;
}

.avatar-section {
  flex-shrink: 0;
  
  .avatar-wrapper {
    width: 180px;
    height: 180px;
    border-radius: 50%;
    overflow: hidden;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
    border: 4px solid #fff;
    
    .user-avatar {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
  }
}

.info-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.info-content {
  .basic-info {
    margin-bottom: 24px;

    .user-name {
      font-size: 28px;
      font-weight: 600;
      color: var(--el-text-color-primary);
      margin-bottom: 12px;
    }

    .user-meta {
      display: flex;
      gap: 24px;
      color: var(--el-text-color-secondary);
      font-size: 14px;

      .meta-item {
        display: flex;
        align-items: center;
        gap: 8px;

        .el-icon {
          font-size: 16px;
        }
      }
    }
  }

  .user-bio {
    margin-bottom: 32px;
    color: var(--el-text-color-regular);
    font-size: 15px;
    line-height: 1.6;

    .bio-placeholder {
      color: var(--el-text-color-secondary);
      font-style: italic;
    }
  }

  .user-stats {
    display: flex;
    gap: 48px;

    .stat-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;

      .stat-label {
        font-size: 14px;
        color: var(--el-text-color-secondary);
      }

      .stat-value {
        font-size: 20px;
        font-weight: 600;
        color: var(--el-text-color-primary);
      }
    }
  }
}

.action-buttons {
  display: flex;
  gap: 16px;
  margin-top: 24px;

  .action-btn {
    min-width: 120px;
    height: 40px;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    font-size: 14px;
    border-radius: 20px;
    transition: all 0.3s ease;

    &:hover {
      transform: translateY(-2px);
    }
  }
}

.tabs-section {
  max-width: 1200px;
  margin: 24px auto;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04);

  .tabs-container {
    padding: 16px;

    .tab-list {
      display: flex;
      justify-content: center;
      gap: 8px;

      .tab-item {
        padding: 12px 24px;
        font-size: 15px;
        color: var(--el-text-color-regular);
        cursor: pointer;
        border-radius: 20px;
        transition: all 0.3s ease;

        &:hover {
          color: var(--el-color-primary);
          background: var(--el-color-primary-light-9);
        }

        &.active {
          color: var(--el-color-primary);
          background: var(--el-color-primary-light-8);
          font-weight: 500;
        }
      }
    }
  }
}

.content-section {
  max-width: 1200px;
  margin: 0 auto;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04);
  padding: 24px;
}

// 响应式设计
@media screen and (max-width: 768px) {
  .user-info {
    flex-direction: column;
    align-items: center;
    text-align: center;
    gap: 24px;
  }

  .user-meta {
    justify-content: center;
  }

  .user-stats {
    justify-content: center;
  }

  .action-buttons {
    justify-content: center;
  }
}
</style>
