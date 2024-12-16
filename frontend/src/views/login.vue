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
            <div class="verify-container">
                <div class="inputf" style="flex: 2;">
                    <input type="text" placeholder="输入验证码" v-model="verifyCode"/>
                    <span class="label">验证码</span>
                    <div v-if="EMVerifyCode" class="errormessage">{{ EMVerifyCode }}</div>
                </div>
                <button 
                    class="verify-btn" 
                    @click="validateFormForget()" 
                    :disabled="!canSendCode"
                    style="flex: 1;"
                >
                    {{ canSendCode ? '发送验证码' : `${countdown}秒后重试` }}
                </button>
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
import user from "@/api/user";
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
        const response = await user.login({
            username: username.value,
            password: password.value,
        });
        alert('欢迎回来');
        window.location.href = '/dashboard';
    } catch (error) {
        EMPassWord.value = error.message;
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
        const data = await user.register({
            username: usernameR.value,
            password: passwordR.value,
            email: email.value,
            studentId: studentId.value
        });
            alert('注册成功');
            window.location.href = '/dashboard';
        
    } catch (error) {
        EMPassWord.value = error.message;
        alert('注册失败');
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

// 添加倒计时相关的响应式数据
const countdown = ref(0);
const canSendCode = ref(true);

// 修改发送验证码的方法
const sendVerifyCode = async () => {
    if (!canSendCode.value) return;
    try {
        const response = await axios.post(`${API_URL}public/getValidateCode`, {
            email: email.value,
        });
        
        // 开始倒计时
        canSendCode.value = false;
        countdown.value = 60;
        
        const timer = setInterval(() => {
            countdown.value--;
            if (countdown.value <= 0) {
                clearInterval(timer);
                canSendCode.value = true;
            }
        }, 1000);
        
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
        const response = await axios.post(`${API_URL}public/reset`, {
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
.login-container {
    width: 100%;
    min-height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    background: linear-gradient(120deg, #e0c3fc 0%, #8ec5fc 100%);
}

.slider {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    max-width: 1000px;
    padding: 50px;
}

.form {
    width: 400px;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    border-radius: 20px;
    border: 1px solid rgba(255, 255, 255, 0.2);
    padding: 45px;
    box-shadow: 0 15px 35px rgba(0, 0, 0, 0.1);
    
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin: 0 15px;
    transition: all 0.4s ease;
}

.title {
    font-size: 32px;
    color: #2c3e50;
    margin-bottom: 15px;
    font-weight: 600;
}

.title b {
    color: #4a90e2;
    font-weight: 700;
}

.subtitle {
    font-size: 16px;
    color: #34495e;
    margin-bottom: 35px;
    letter-spacing: 0.5px;
}

.inputf {
    width: 100%;
    margin-bottom: 25px;
    position: relative;
}

.inputf input {
    width: 100%;
    height: 48px;
    background: rgba(255, 255, 255, 0.9);
    border: 2px solid #e0e0e0;
    border-radius: 12px;
    padding: 0 15px;
    font-size: 15px;
    color: #2c3e50;
    transition: all 0.3s ease;
    position: relative;
    z-index: 2;
}

.inputf input:focus {
    border-color: #4a90e2;
    box-shadow: 0 0 10px rgba(74, 144, 226, 0.1);
}

.inputf input::placeholder {
    color: #95a5a6;
    transition: opacity 0.3s ease;
}

.inputf input:focus::placeholder {
    color: transparent;
}

.label {
    position: absolute;
    left: 15px;
    top: 50%;
    transform: translateY(-50%);
    color: #95a5a6;
    font-size: 15px;
    pointer-events: none;
    transition: all 0.3s ease;
    background: white;
    padding: 0 5px;
    z-index: 1;
}

.inputf input:focus + .label,
.inputf input:not(:placeholder-shown) + .label {
    top: 0;
    font-size: 14px;
    color: #4a90e2;
    font-weight: 600;
    transform: translateY(-50%);
    z-index: 3;
}

.inputf input:focus {
    outline: none;
    border-color: #4a90e2;
}

button {
    width: 100%;
    height: 48px;
    background: linear-gradient(45deg, #4a90e2, #67b0ff);
    border: none;
    border-radius: 12px;
    color: white;
    font-size: 16px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
    margin-top: 10px;
}

button:hover {
    transform: translateY(-2px);
    box-shadow: 0 7px 14px rgba(74, 144, 226, 0.2);
}

.card {
    position: absolute;
    width: 380px;
    background: linear-gradient(45deg, #4a90e2, #67b0ff);
    padding: 40px;
    border-radius: 20px;
    margin-left: 450px;
    color: white;
    transition: 0.5s ease-in-out;
}

.card .name {
    font-size: 30px;
    font-weight: 700;
    margin-bottom: 15px;
}

.card .name span {
    color: #fff;
    text-shadow: 0 0 10px rgba(255,255,255,0.3);
}

.card .desc {
    font-size: 16px;
    line-height: 1.6;
    margin-bottom: 30px;
}

.card .btn button {
    background: transparent;
    border: 2px solid white;
    padding: 10px 25px;
    border-radius: 25px;
    color: white;
    font-size: 14px;
    font-weight: 600;
    transition: all 0.3s ease;
}

.card .btn button:hover {
    background: white;
    color: #4a90e2;
}

.errormessage {
    color: #e74c3c;
    font-size: 12px;
    margin-top: 5px;
    padding-left: 5px;
}

/* 动画效果 */
.form.hidden {
    transform: translateX(-30px);
    opacity: 0;
}

.form.hidden2 {
    display: none;
}

.card.active {
    transform: translateX(-460px);
}

/* 响应式调整 */
@media (max-width: 768px) {
    .form {
        width: 320px;
        padding: 30px;
    }
    
    .card {
        width: 320px;
        padding: 30px;
    }
}

.verify-container {
    width: 100%;
    display: flex;
    gap: 10px;
    align-items: flex-start;
    margin-bottom: 25px;
}

.verify-container .inputf {
    margin-bottom: 0;
}

.verify-container .verify-btn {
    height: 48px;
    margin: 0;
    padding: 0 15px;
    white-space: nowrap;
    font-size: 14px;
    transition: all 0.3s ease;
}

.verify-btn:disabled {
    background: #ccc;
    cursor: not-allowed;
    transform: none;
    box-shadow: none;
}

.verify-btn:disabled:hover {
    transform: none;
    box-shadow: none;
}

/* 确保错误消息不会影响布局 */
.verify-container .errormessage {
    position: absolute;
    bottom: -20px;
}
</style>