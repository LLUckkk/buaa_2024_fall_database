<template>
  <div class="login-container">
    <!-- 登录表单 -->
    <el-form ref="loginForm" :model="loginForm" class="login-form" auto-complete="on" label-position="left">

      <!-- 标题区域 -->
      <div class="title-container">
        <h3 class="title">交易集市后台管理系统</h3>
      </div>

      <!-- 用户名输入框 -->
      <el-form-item prop="username">
        <!-- 用户名图标 -->
        <span class="svg-container">
          <i class="el-icon-user-solid"></i>
        </span>
        <!-- 用户名输入框 -->
        <el-input
            ref="username"
            v-model="loginForm.username"
            placeholder="Username"
            name="username"
            type="text"
            tabindex="1"
            auto-complete="on"
        />
      </el-form-item>

      <!-- 密码输入框 -->
      <el-form-item prop="password">
        <!-- 密码图标 -->
        <span class="svg-container">
          <i class="el-icon-lock"></i>
        </span>
        <!-- 密码输入框 -->
        <el-input
            :key="passwordType"  
            ref="password"
            v-model="loginForm.password"
            :type="passwordType" 
            placeholder="Password"
            name="password"
            tabindex="2"
            auto-complete="on"  
        />
        <!-- 显示/隐藏密码按钮 -->
        <span class="show-pwd" @click="showPwd">
          <i :class="passwordType === 'password'?'el-icon-lock':'el-icon-view'"></i>
        </span>
      </el-form-item>

      <!-- 登录按钮 -->
      <el-button :loading="loading" type="primary" style="width:100%;margin-bottom:30px;">登录</el-button>

      <!-- 提示区域，提供系统管理员和超级管理员的默认账号信息 -->
      <div class="tips" style="color: #DCDFE6">
        系统管理员：
        <span style="margin-right:20px;color: #409EFF">账号 system</span><span style="color:#409EFF ">密码 system</span>
      </div>
      <div class="tips" style="color: #DCDFE6">
        超级管理员：
        <span style="margin-right:20px;color: #409EFF">账号 admin</span><span style="color:#409EFF ">密码 admin</span>
      </div>
    </el-form>
  </div>
</template>

<script>
import Cookies from 'js-cookie'

export default {
  name: 'Login',
  data() {
    return {
      // 登录表单的初始值，包含用户名和密码
      loginForm: {
        username: '',
        password: ''
      },
      // 是否正在加载
      loading: false,
      // 密码显示类型，默认为隐藏密码
      passwordType: 'password',
      // 登录后是否需要重定向的路径
      redirect: undefined
    }
  },
  methods: {
    // 切换显示/隐藏密码
    showPwd() {
      if (this.passwordType === 'password') {
        this.passwordType = ''  // 显示密码
      } else {
        this.passwordType = 'password'  // 隐藏密码
      }
      // 更新密码框焦点
      this.$nextTick(() => {
        this.$refs.password.focus()
      })
    },
    // 登录方法
    Login() {
      this.loading = true  // 设置加载状态
      this.$api.user.login(this.loginForm).then(res => {
        // 登录成功后保存 token 并跳转到首页
        Cookies.set('admin-token', res.result.tokenValue)
        this.loading = false  // 取消加载状态
        this.$router.push('/')  // 跳转到首页
      })
    }
  }
}
</script>

<style lang="scss">
$bg: #283443;  // 背景颜色
$light_gray: #fff;  // 浅灰色
$cursor: #fff;  // 光标颜色

/* 支持 Webkit 浏览器的输入框样式调整 */
@supports (-webkit-mask: none) and (not (cater-color: $cursor)) {
  .login-container .el-input input {
    color: $cursor;
  }
}

/* 自定义 Element UI 样式 */
.login-container {
  .el-input {
    display: inline-block;
    height: 47px;
    width: 85%;  // 输入框宽度
  
    input {
      background: #fff;  // 设置背景透明
      border: 1px solid rgba(255,255,255,0.2);  // 去掉边框
      
      border-radius: 5px;  // 去掉圆角
      padding: 12px 5px 12px 15px;  // 内边距设置
      color: $light_gray;  // 输入框文字颜色
      height: 47px;  // 高度
      caret-color: $cursor;  // 光标颜色
  
      &:-webkit-autofill {
        box-shadow: 0 0 0px 1000px $bg inset !important;  // 设置自动填充的背景
        -webkit-text-fill-color: $cursor !important;  // 自动填充文字颜色
      }
    }
  }

  .el-form-item {
    border: 1px solid rgba(255, 255, 255, 0.1);  // 设置边框
    background: rgba(0, 0, 0, 0.1);  // 设置背景色
    border-radius: 5px;  // 设置圆角
    color: #454545;  // 设置文字颜色
  }
}
</style>

<style lang="scss" scoped>
$bg: #2d3a4b;  // 背景颜色
$dark_gray: #889aa4;  // 深灰色
$light_gray: #eee;  // 浅灰色

.login-container {
  min-height: 100%;  // 最小高度为 100%
  width: 100%;  // 宽度为 100%
  background-color: $bg;  // 设置背景颜色
  overflow: hidden;  // 隐藏溢出内容

  .login-form {
    position: relative;
    width: 520px;  // 设置登录表单宽度
    max-width: 100%;  // 最大宽度 100%
    padding: 160px 35px 0;  // 上、右、下、左内边距
    margin: 0 auto;  // 水平居中
    overflow: hidden;  // 隐藏溢出内容
  }

  .tips {
    font-size: 14px;  // 设置字体大小
    color: #fff;  // 设置文字颜色
    margin-bottom: 10px;  // 设置底部外边距

    span {
      &:first-of-type {
        margin-right: 16px;  // 第一个 span 设置右边距
      }
    }
  }

  .svg-container {
    padding: 6px 5px 6px 15px;  // 设置内边距
    color: $dark_gray;  // 设置颜色
    vertical-align: middle;  // 垂直居中
    width: 30px;  // 设置宽度
    display: inline-block;  // 设置为内联块级元素
  }

  .title-container {
    position: relative;

    .title {
      font-size: 26px;  // 设置字体大小
      color: $light_gray;  // 设置颜色
      margin: 0px auto 40px auto;  // 设置外边距
      text-align: center;  // 文本居中
      font-weight: bold;  // 设置加粗字体
    }
  }

  .show-pwd {
    position: absolute;
    right: 10px;  // 设置右边距
    top: 7px;  // 设置上边距
    font-size: 16px;  // 设置字体大小
    color: $dark_gray;  // 设置颜色
    cursor: pointer;  // 设置鼠标为指针样式
    user-select: none;  // 禁止用户选择
  }
}
</style>
