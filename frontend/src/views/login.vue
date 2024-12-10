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
        </div>
        <div :class="active == 2 ? 'form' : 
                    active == 3 ? 'form hidden2':'form hidden'">
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
            <button @click="validateFormR()">注册</button>
        </div>
        <div :class="active == 3 ? 'form' : 'form hidden2'">
            <div class="inputf">
                <input type="text" placeholder="请输入原账户邮箱" v-model="email"/>
                <span class = "label">邮箱</span>
                <div v-if="EMEmail" class="errormessage">{{ EMEmail }}</div>
            </div>
            <div class="inputf">
                <input type="text" placeholder="输入新的密码" v-model="password"/>
                <span class = "label">密码</span>
                <div v-if="EMPassWord" class="errormessage">{{ EMPassWord }}</div>
            </div>
            <div class="input-container">
                <input type="text" placeholder="验证码" v-model="verifyCode"/>
                <span class = "label">验证码</span>
                <div v-if="EMVerifyCode" class="errormessage">{{ EMVerifyCode }}</div>
                <div class = "btn" @click="validateFormForget()">发送验证码</div>
            </div>
            <button @click="validateFormReset()">重置账户</button>
        </div>
        <div :class = "active == 1 ? 'card' : 'card active'">
            <div class = "head">
                <div class = "name">闲置<span>集市</span></div>
                <div class="desc">具体描述</div>
                <div :class="active == 3 ? 'btn hidden' : 'btn'">
                    {{ active == 1 ? '还没有账号?' : '已有账号' }}
                    <button @click = "clickSwitch()">
                        {{ active == 1 ? '去注册' : '去登录' }}</button>
                </div>
                <div :class="active == 2 ? 'btn hidden' : 'btn'">
                    {{ active == 1 ? '忘记密码？' : '已重置？' }}
                    <button @click = "clickSwitch2()">{{ active == 1 ? '重置密码' : '返回登录' }}</button>
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
// 响应式数据,R代表注册部分相关信息
const active = ref(1);
const username = ref('');
const password = ref('');
const usernameR = ref('');
const passwordR = ref('');
const email = ref('');
const studentId = ref('');
const verifyCode = ref('');
// error message
const EMUserName = ref('');
const EMPassWord = ref('');
const EMUserNameR = ref('');
const EMPassWordR = ref('');
const EMEmail = ref('');
const EMStudentId = ref('');
const EMVerifyCode = ref('');
// 表单验证函数Fo
const validateForm = () => {
    //用户名规范
    if (!username.value) {
        EMUserName.value = '用户名不能为空';
        return;
    } else{
        EMUserName.value = '';  // 清空错误信息
    }
    //密码规范
    if (!password.value) {
        EMPassWord.value = '密码不能为空';
        return;
    } else {
        EMPassWord.value = '';  // 清空错误信息
    }
    loginCheck();

};
const loginCheck = async () => {
    try {
        const response = await axios.post(`${API_URL}public/login`, {
            username: username.value,
            password: password.value,
        });
        if (response.data.code != 1) {
            EMPassWord.value = response.data.message;
            alert('登录失败');
        }
        else {
            EMPassWord.value = '';
            alert('欢迎回来');
            window.location.href = '/dashboard';
            //切换页面
        }
    } catch (error) {
    
    }
};
//注册部分规范
const validateFormR = () => {
    //用户名
    if (usernameR.value.length < 4 || usernameR.value.length > 64) {
        EMUserNameR.value = '用户名在4-64个字符';
        return;
    } else {
        EMUserNameR.value = '';  // 清空错误信息
    }
    //密码
    if (!passwordR.value) {
        EMPassWordR.value = '密码不能为空';
        return;
    } else if (passwordR.value.length < 6) {
        EMPassWordR.value = '密码不能少于6个字符';
        return;
    } else {
        EMPassWordR.value = '';
    }
    //邮箱
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/; // 正则表达式匹配邮箱

    if (!email.value) {
        EMEmail.value = '邮箱不能为空';
        return;
    } else if (!emailRegex.test(email.value)) {
        EMEmail.value = '邮箱地址不合法';
        return;
    } else {
        EMEmail.value = '';  // 清空错误信息
    }
    registerUser();
};
const registerUser = async () => {
    try {
        const response = await axios.post(`${API_URL}public/register`, {
            username: usernameR.value,
            password: passwordR.value,
            email: email.value,
            studentId: studentId.value
        });
        if (response.data.code != 1) {
            EMStudentId.value = response.data.message;
            alert('注册失败');
        } else {
            alert('注册成功');
            clickSwitch();
        }
    } catch (error) {
    
    }
};

//忘记密码发送邮箱验证码
const validateFormForget = () => {
    //邮箱
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/; // 正则表达式匹配邮箱
    if (!email.value) {
        EMEmail.value = '邮箱不能为空';
        return;
    } else if (!emailRegex.test(email.value)) {
        EMEmail.value = '邮箱地址不合法';
        return;
    } else {
        EMEmail.value = '';  // 清空错误信息
    }
    sendVerifyCode();
}

const sendVerifyCode = async () => {
    try {
        const response = await axios.post(`${API_URL}auth/forgotPassWord`, {
            email: email.value,
        });
        alert('发送成功');
    } catch (error) {
        EMVerifyCode.value = error.response.data.message;
        alert('发送失败');
    }
};

//重置部分规范
const validateFormReset = () => {
    //密码
    if (!password.value) {
        EMPassWord.value = '密码不能为空';
        return;
    } else if (password.value.length < 6) {
        EMPassWord.value = '密码不能少于 6 个字符';
        return;
    } else {
        EMPassWord.value = '';
    }
    //邮箱
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/; // 正则表达式匹配邮箱

    if (!email.value) {
        EMEmail.value = '邮箱不能为空';
        return;
    } else if (!emailRegex.test(email.value)) {
        EMEmail.value = '邮箱地址不合法';
        return;
    } else {
        EMEmail.value = '';  // 清空错误信息
    }
    resetUser();
};


const resetUser = async () => {
    try {
        const response = await axios.post(`${API_URL}auth/resetPassWord`, {
            email: email.value,
            newPassWord:password.value,
            token:verifyCode.value
        });
        alert('重置成功');
    } catch (error) {
        EMVerifyCode.value = error.response.data.message;
        alert('重置失败');
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
    EMEmail.value = '';
    EMVerifyCode.value = '';
    // 清空文本框
    username.value = '';
    password.value = '';
    usernameR.value = '';
    passwordR.value = '';
    email.value = '';
    verifyCode.value = '';
};

const clickSwitch2 = () => {
    active.value = (active.value == 1)? 3 : 1;
    // 清空报错信息
    EMUserName.value = '';
    EMPassWord.value = '';
    EMUserNameR.value = '';
    EMPassWordR.value = '';
    EMEmail.value = '';
    EMVerifyCode.value = '';
    // 清空文本框
    username.value = '';
    password.value = '';
    usernameR.value = '';
    passwordR.value = '';
    email.value = '';
    verifyCode.value = '';
};

</script>
<style scoped>
@import '../assets/login.css';
/* Your additional styles */
</style>