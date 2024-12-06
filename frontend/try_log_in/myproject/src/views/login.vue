<template>

    <div class="login-container">
       <div class = "slider">
        <div :class="active == 1 ? 'form' : 'form hidden'">
            <div class="title">欢迎 <b>回来</b></div>
            <div class="subtitle">登录你的账户</div>
            <div class="inputf">
                <input type="text" placeholder="用户名" v-model = "username" />
                <span class = "label">用户名</span>
                <div v-if="EMUserName" class="errormessage">{{ EMUserName }}</div>
            </div>
            <div class="inputf">
                <input type="text" placeholder="密码" v-model = "password"/>
                <span class = "label">密码</span>
                <div v-if="EMPassWord" class="errormessage">{{ EMPassWord }}</div>
            </div>
            <button @click="validateForm()">登录</button>
            <button @click="checkHeartbeat()">测试</button>
        </div>
        <div :class="active == 2 ? 'form' : 'form hidden'">
            <div class="title"><b>开始</b></div>
            <div class="subtitle">创建你的账户</div>
            <div class="inputf">
                <input type="text" placeholder="用户名" v-model = "usernameR"/>
                <span class = "label">用户名</span>
                <div v-if="EMUserNameR" class="errormessage">{{ EMUserNameR }}</div>
            </div>
            <div class="inputf">
                <input type="text" placeholder="密码" v-model="passwordR"/>
                <span class = "label">密码</span>
                <div v-if="EMPassWordR" class="errormessage">{{ EMPassWordR }}</div>
            </div>
            <div class="inputf">
                <input type="text" placeholder="邮箱" v-model="email"/>
                <span class = "label">邮箱</span>
                <div v-if="EMEmail" class="errormessage">{{ EMEmail }}</div>
            </div>
            <div class="inputf">
                <input type="text" placeholder="学号" v-model="studentId"/>
                <span class = "label">学号</span>
                <div v-if="EMStudentId" class="errormessage">{{ EMStudentId }}</div>
            </div>
            <div class="inputf">
                <div class="input-container">
                    <input type="text" placeholder="输入验证码" v-model= "identifyCode"/>
                    <span class="label">验证码</span>
                    <button class="captcha-btn">获取验证码</button>
                </div>
                <div v-if="EMIdentifyCode" class="errormessage">{{ EMIdentifyCode }}</div>
            </div>
            <button @click="validateFormR()">注册</button>
        </div>
        <div :class = "active == 1 ? 'card' : 'card active'">
            <div class = "head">
                <div class = "name">闲置<span>集市</span></div>
                <div class="desc">具体描述</div>
                <div class="btn">
                    {{ active == 1 ? '还没有账号?' : '已有账号' }}
                    <button @click = "clickSwitch()">
                        {{ active == 1 ? '去注册' : '去登录' }}</button>
                </div>
            </div>
        </div>
       </div>
    </div>
</template>

<script setup>
import {ref} from "vue";
import { errorMessages } from "vue/compiler-sfc";
import axios from 'axios';
import { API_URL } from '../config';
// 响应式数据
const active = ref(1);
const username = ref('');
const password = ref('');
// R 代表注册部分相关信息
const usernameR = ref('');
const passwordR = ref('');
const email = ref('');
const identifyCode = ref('');
const studentId = ref('');
// error message
const EMUserName = ref('');
const EMPassWord = ref('');
const EMUserNameR = ref('');
const EMPassWordR = ref('');
const EMEmail = ref('');
const EMIdentifyCode = ref('');
const EMStudentId = ref('');
// 表单验证函数
const validateForm = () => {
    //用户名规范
    if (!username.value) {
        EMUserName.value = '用户名不能为空呦';
    } else{
        EMUserName.value = '';  // 清空错误信息
    }
    //密码规范
    if (!password.value) {
        EMPassWord.value = '密码怎么可以为空';
    } else {
        EMPassWord.value = '';  // 清空错误信息
    }

};
//注册部分规范
const registerUser = async () => {
    try {
        const response = await axios.post(`${API_URL}auth/register`, {
            username: usernameR.value,
            password: passwordR.value,
            email: email.value,
            studentId: studentId.value
        });
        if (response.data.success) {
            alert('注册成功');
            clickSwitch();
        } else {
            alert('注册失败');
        }
    } catch (error) {
        alert('注册失败');
    }
};

const validateFormR = () => {
    //用户名
    if (usernameR.value.length < 3 || usernameR.value.length > 8) {
        EMUserNameR.value = '用户名不能太长也不能太短~';
    } else {
        EMUserNameR.value = '';  // 清空错误信息
    }
    //密码
    if (!passwordR.value) {
        EMPassWordR.value = '密码不能为空';
    } else if (passwordR.value.length <= 8) {
        EMPassWordR.value = '密码不能太短，不然容易被盗号'
    } else {
        EMPassWordR.value = '';
    }
    //邮箱
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/; // 正则表达式匹配邮箱

    if (!email.value) {
        EMEmail.value = '邮箱不能为空';
    } else if (!emailRegex.test(email.value)) {
        EMEmail.value = '邮箱地址不合法，怎么给你发邮件！';
    } else {
        EMEmail.value = '';  // 清空错误信息
    }
    //验证码
    if (!identifyCode.value) {
        EMIdentifyCode.value = '验证码不能为空';
    } else {
        EMIdentifyCode.value = '';  // 清空错误信息
    }
    if (!EMUserNameR.value && !EMPassWordR.value && !EMEmail.value && !EMIdentifyCode.value) {
        registerUser();
    }
};
//点击切换按钮，所有错误信息清空
const clickSwitch = () => {
    active.value = (active.value == 1)? 2 : 1;
    // 清空报错信息
    EMUserName.value = '';
    EMPassWord.value = '';
    EMUserNameR.value = '';
    EMPassWordR.value = '';
    EMIdentifyCode.value = '';
    EMEmail.value = '';
    // 清空文本框
    username.value = '';
    password.value = '';
    usernameR.value = '';
    passwordR.value = '';
    identifyCode.value = '';
    email.value = '';
};

function checkHeartbeat() {
    axios.get(`${API_URL}status`).then((response) => {
        if (response.data.status === "Alive") {
            alert('成功');
        }
    }).catch((error) => {
        alert('失败');
    });
}

</script>
<style scoped>
@import '../assets/login.css';
/* Your additional styles */
</style>