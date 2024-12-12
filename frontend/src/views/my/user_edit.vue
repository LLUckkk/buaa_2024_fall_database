<template>
  <div class="user-card">
    <!-- 添加标题区域 -->
    <div class="title-section">
      <h2 class="page-title">修改资料</h2>
      <div class="title-divider"></div>
    </div>

    <el-form ref="form" :model="form">
      <!-- 头像上传区域 -->
      <div class="avatar-section">
        <el-upload
          class="avatar-uploader"
          action="/api/upload/image"
          :on-success="handleUploadSuccess"
          :show-file-list="false"
          :limit="1"
        >
          <img v-if="form.avatar" :src="form.avatar" class="avatar">
          <i v-else class="el-icon-plus avatar-uploader-icon"></i>
        </el-upload>
        <div class="avatar-tip">点击更换头像</div>
      </div>

      <!-- 用户信息区域 -->
      <div class="user-info">
        <el-descriptions :column="1" :colon="false" direction="vertical">
          <el-descriptions-item label="学号">
            <span class="info-text">{{ userInfo.studentId }}</span>
          </el-descriptions-item>
          <el-descriptions-item label="邮箱">
            <span class="info-text">{{ userInfo.email }}</span>
          </el-descriptions-item>
          <el-descriptions-item label="地址">
            <span class="info-text">{{ userInfo.address }}</span>
          </el-descriptions-item>
          <el-descriptions-item label="昵称">
            <input class="custom-input" type="text" :placeholder="form.nickName" v-model="form.nickName"/>
          </el-descriptions-item>
          <el-descriptions-item label="我的密码">
            <el-link type="primary" @click="dialog = true" class="password-link">修改密码</el-link>
          </el-descriptions-item>
          <el-descriptions-item label="个人简介">
            <textarea class="custom-textarea" :placeholder="form.intro" v-model="form.intro"></textarea>
          </el-descriptions-item>
        </el-descriptions>
      </div>

      <!-- 按钮区域 -->
      <div class="button-group">
        <el-button type="primary" @click="onSubmit">确认修改</el-button>
        <el-button @click="cancel">取消</el-button>
      </div>
    </el-form>
  </div>

  <!-- 修改密码对话框 -->
  <el-dialog title="设置密码" :visible.sync="dialog" width="400px">
    <el-form :model="form" class="password-form">
      <el-form-item label="设置密码">
        <el-input type="password" v-model="form.password"/>
      </el-form-item>
      <el-form-item label="重新输入密码">
        <el-input type="password" v-model="form.passwordCheck"/>
      </el-form-item>
    </el-form>
    <div slot="footer" class="dialog-footer">
      <el-button @click="dialog = false">取 消</el-button>
      <el-button type="primary" @click="postPass()">确 定</el-button>
    </div>
  </el-dialog>
</template>

<script>
import {Notification} from "element-plus";
import api from "@/api";

export default {
  created() {
    this.$api = api
    this.getUserInfo()
  },
  data() {
    return {
      form: {
        nickName: '',
        intro: "",
        avatar: "",
        password: '',
        passwordCheck: '',
      },
      dialog: false,
      userInfo: {},
    }
  },
  methods: {
    onSubmit() {
      this.$api.user.updateUserInfo(this.form).then(res => {
        this.cancel();
        Notification({type: 'success', title: '修改个人信息'})
        this.$store.dispatch('getUserInfo')
      })
    },
    getUserInfo() {
      this.$api.user.getUserInfo().then(res => {
        this.userInfo = res.data
        this.form.nickName = res.data.nickName
        this.form.intro = res.data.intro
        this.form.avatar = res.data.avatar
      })
    },
    postPass() {
      if(!this.form.password) {
        Notification({type: 'warning', title: '异常', message: '请输入密码'})
        return
      }
      if (this.form.password !== this.form.passwordCheck) {
        Notification({type: 'warning', title: '异常', message: '输入密码不一致'})
        return
      }
      this.$api.user.updateUserPassword(this.form).then(res => {
        this.cancel();
        this.dialog = false;
        Notification({type: 'success', title: '修改个人信息', message: '修改成功'})
        this.$store.dispatch('getUserInfo')
      })
    },
    cancel() {
      this.$emit('close-drawer')
    },
    handleUploadSuccess(res, file) {
      this.form.avatar = res.data.url
    }
  }
}
</script>

<style scoped lang="less">
.title-section {
  margin-bottom: 20px;
  text-align: center;
  
  .page-title {
    font-size: 20px;
    font-weight: 600;
    color: #2c3e50;
    margin: 0;
    padding: 0 0 12px 0;
    font-family: "PingFang SC", "Microsoft YaHei", "Helvetica Neue", sans-serif;
    margin-top: -30px;
  }

  .title-divider {
    height: 2px;
    background: linear-gradient(to right, transparent, var(--primary-color), transparent);
    width: 100px;
    margin: 0 auto;
  }
}

.user-card {
  max-width: 560px;
  margin: 20px auto;
  padding: 0 20px;
}

.avatar-section {
  text-align: center;
  margin-bottom: 30px;

  .avatar-tip {
    margin-top: 8px;
    color: #909399;
    font-size: 13px;
  }
}

.user-info {
  :deep(.el-descriptions) {
    .el-descriptions-item__label {
      width: 80px;
      color: #606266;
      font-weight: normal;
      padding: 16px 0;
    }

    .el-descriptions-item__content {
      padding: 16px 0;
    }
  }
}

.custom-input {
  width: 100%;
  max-width: 300px;
  height: 32px;
  padding: 0 12px;
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  transition: all 0.2s;
  
  &:focus {
    border-color: #409EFF;
    outline: none;
  }
}

.custom-textarea {
  width: 100%;
  padding: 8px 12px;
  height: 80px;
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  resize: vertical;
  transition: all 0.2s;
  
  &:focus {
    border-color: #409EFF;
    outline: none;
  }
}

.button-group {
  margin-top: 30px;
  text-align: center;
  
  .el-button {
    padding: 9px 20px;
    margin: 0 10px;
  }
}

:deep(.avatar-uploader) {
  .el-upload {
    border: 1px dashed #d9d9d9;
    border-radius: 50%;
    cursor: pointer;
    
    &:hover {
      border-color: #409EFF;
    }
  }

  .avatar-uploader-icon {
    font-size: 24px;
    color: #8c939d;
    width: 100px;
    height: 100px;
    line-height: 100px;
    text-align: center;
  }

  .avatar {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    object-fit: cover;
  }
}

.password-form {
  padding: 0 20px;
  
  .el-form-item__label {
    width: 100px;
  }
  
  .el-input {
    width: 100%;
  }
}

.dialog-footer {
  text-align: right;
  padding-top: 20px;
}
</style>