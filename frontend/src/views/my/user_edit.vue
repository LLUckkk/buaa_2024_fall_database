<template>
  <div style="background-color: #f7f7f7;height: 100%;width: 100%; padding: 15px;">
    <div class="user-card">

      <el-form ref="form" :model="form">
        <el-upload
            style="display: flex;align-items: center;justify-content: center;padding: 10px"
            class="avatar-uploader"
            action="http://127.0.0.1:5050/upload/image"
            :on-success="handleUploadSuccess"
            :show-file-list="false"
            :limit="1"
        >
          <img v-if="form.avatar" :src="form.avatar" class="avatar">
          <i v-else class="el-icon-plus avatar-uploader-icon"></i>
        </el-upload>
        <div class="description">
          <el-descriptions :column="1" :colon="false" direction="vertical">
            <el-descriptions-item label="闲宝号">{{ userInfo.number }}</el-descriptions-item>
            <el-descriptions-item label="手机号">{{ userInfo.phone }}</el-descriptions-item>
            <el-descriptions-item label="IP地址">{{ userInfo.province }} {{ userInfo.city }}</el-descriptions-item>
            <el-descriptions-item label="昵称">
              <input class="input" type="text" :placeholder="form.nickName" v-model="form.nickName"/>
            </el-descriptions-item>
            <el-descriptions-item label="我的密码">
             <el-link type="primary" @click="dialog = true">修改密码</el-link>
            </el-descriptions-item>
            <el-descriptions-item label="个人简介">
              <textarea class="textarea" :placeholder="form.intro" v-model="form.intro"></textarea>
            </el-descriptions-item>
          </el-descriptions>
        </div>
        <el-form-item>
          <el-button type="primary" @click="onSubmit" size="mini">确认修改</el-button>
          <el-button size="mini" @click="cancel">取消</el-button>
        </el-form-item>
      </el-form>
    </div>
    <el-dialog title="设置密码" :visible.sync="dialog" append-to-body>
      <el-form :model="form">
        <el-form-item label="设置密码" :label-width="'100px'">
          <el-input type="password" v-model="form.password"/>
        </el-form-item>
        <el-form-item label="重新输入密码" :label-width="'100px'">
          <el-input  type="password" v-model="form.passwordCheck"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialog = false">取 消</el-button>
        <el-button type="primary" @click="postPass()">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import {Notification} from "element-plus";

export default {
  created() {
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
        Notification({type: 'success', title: '修改个人信息', message: '请等待审核通过'})
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
.user-card {
  height: 100%;
  border-radius: 10px;
  background-color: white;
  padding: 10px;

  .description {


    .input {
      padding: 12px 16px 12px 16px;
      width: 60%;
      height: 25px;
      line-height: 25px;
      background: rgba(0, 0, 0, 0.03);
      caret-color: rgba(51, 51, 51, 0.3);
      border-radius: 10px;
      border: none;
      outline: none;
      resize: none;
      color: #333;
    }

    .textarea {
      padding: 12px 16px 12px 16px;
      width: 100%;
      height: 70px;
      line-height: 16px;
      background: rgba(0, 0, 0, 0.03);
      caret-color: rgba(51, 51, 51, 0.3);
      border-radius: 10px;
      border: none;
      outline: none;
      resize: none;
      color: #333;
    }
  }
}

/deep/ .avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 100%;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}

/deep/ .avatar-uploader .el-upload:hover {
  border-color: #409EFF;
}

/deep/ .avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 100px;
  height: 100px;
  line-height: 100px;
  text-align: center;
}

/deep/ .avatar {
  width: 100px;
  height: 100px;
  display: block;
}
</style>