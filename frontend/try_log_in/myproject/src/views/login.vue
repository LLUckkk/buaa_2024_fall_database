<template>
    <div class="login-container">
       <div class = "slider">
        <div :class="active == 1 ? 'form' : 'form hidden'">
            <div class="title">欢迎 <b>回来</b></div>
            <div class="subtitle">登录你的账户</div>
            <div class="inputf">
                <input type="text" placeholder="用户名"/>
                <span class = "label">用户名</span>
            </div>
            <div class="inputf">
                <input type="text" placeholder="密码"/>
                <span class = "label">密码</span>
            </div>
            <button>登录</button>
        </div>
        <div :class="active == 2 ? 'form' : 'form hidden'">
            <div class="title"><b>开始</b></div>
            <div class="subtitle">创建你的账户</div>
            <div class="inputf">
                <input type="text" placeholder="用户名"/>
                <span class = "label">密码</span>
            </div>
            <div class="inputf">
                <input type="text" placeholder="密码"/>
                <span class = "label">密码</span>
            </div>
            <div class="inputf">
                <input type="text" placeholder="邮箱"/>
                <span class = "label">用户名</span>
            </div>
            <div class="inputf">
                <div class="input-container">
                    <input type="text" placeholder="输入验证码"/>
                    <span class="label">验证码</span>
                    <button class="captcha-btn">获取验证码</button>
                </div>
            </div>
            <button>注册</button>
        </div>
        <div :class = "active == 1 ? 'card' : 'card active'">
            <div class = "head">
                <div class = "name">闲置<span>集市</span></div>
                <div class="desc">具体描述</div>
                <div class="btn">
                    还没有账号？
                    <button @click = "active = (active == 1)? 2 : 1">去注册</button>
                </div>
            </div>
        </div>
       </div>
    </div>
</template>
<script setup>
import {ref} from "vue";
const active = ref(1);

</script>
<style lang = "scss">
.login-container {
    width:100%;
    min-height:100vh;
    display:flex;
    justify-content: center;
    align-items: center;
    background: #eee;
    background: url("../assets/background.jpg") no-repeat center center;
    background-size: cover;
    .slider {
        position: relative;
        display: flex;
        justify-content: center;
        align-items: center;
        .form {
    width: 400px;
    height: 500px;
    background: rgba(255,255,255,0.15);
    //设置模糊效果
    backdrop-filter: blur(79px) saturate(0);
    // 圆角
    border-radius:10px;
    //描边
    border:1px solid rgba(255,255,255,0.15);
    padding: 40px 60px;
   /* box-shadow: rgba(50,50,93,0.25) 50px 50px 100px -20px,rgba(0,0,0,0.5) 30px 30px 60px -30px,rgba(212,217,222,0.35) 2px -2px 6px 0px inset;*/
    //居中设置
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: flex-start;
    margin: 0 10px;
    z-index: 3;
    transition: 0.5s ease-in-out;
    &.hidden {
        height:395px;
        box-shadow: none;
        z-index:1;
    }
    .title {
        font-size: 24px;
        color: black;
        letter-spacing: 1px;
        font-weight: 300;
    }
    .subtitle {
        font-size: 15px;
        color: rgb(246,240,255);
        letter-spacing: 1px;
        margin-bottom: 35px;
    }
    .inputf {
        display: block;
        width: 100%;
        position: relative;
        margin-bottom: 35px;
        input {
            width: 100%;
            height: 35px;
            border: none;
            outline: 1.5px solid rgb(200,200,220);
            background: transparent;
            border-radius: 8px;
            font-size: 12px;
            padding: 0 12px;
            color: rgb(246,249,255);
            // 未输入时的文本符
            &::placeholder {
                color: rgb(175,180,190);
            }
            //获取焦点，实现点击文本框出现 label
            &:focus {
                outline: 1.5px solid rgb(250, 252, 255);
                // 隐藏占位符
                &::placeholder {
                    opacity: 0;
                } 
                & + .label {
                    opacity: 1;
                    top:-20px;
                }
            }
            //保证输入内容时 label 不消失
            &:not(:placeholder-shown) + .label {
                    opacity: 1;
                    top: -20px;
                }
        }
        .label {
            position: absolute;
            top:0;
            left: 0;    
            color: rgb(246,249,255);
            font-size: 11px;
            font-weight: bold;
            transition:0.25s ease-out;
            opacity: 0;
        }
    }
    .input-container {
    display: flex;
    width: 100%;
    align-items: center;

    input {
        flex: 1;
        margin-right: 10px;
    }

    .captcha-btn {
        width: 120px;
        height: 35px;
        background: lightskyblue;
        color: white;
        border: none;
        border-radius: 5px;
        font-weight: bold;
        font-size: 12px;
        cursor: pointer;
    }
}
    button {
        width: 100%;
        height: 35px;
        background: rgb(36,217,127);
        color: #ffffff;
        border: none;
        outline: none;
        border-radius: 5px;
        font-weight: bold;
        font-size: 12px;
        cursor: pointer;
    }
        }
        .card {
                position: absolute;
                right:0;
                top:50%;
                transform: translate(0,-50%);
                width: 430px;
                height: 400px;
                background-size: contain;
                padding: 40px;
                border-radius: 0 10px 10px 0;
                background-color: white;
                z-index:2;
                transition: 0.5s ease-in-out;
                &.active {
                    right:calc(100% - 440px);
                    border-radius: 10px 0 0 10px;
                    transition: 0.5s ease-in-out;
                }
                .head {
                    font-size: 34px;
                    margin-bottom: 35px;
                    .name {
                        font-weight: 300;
                        span {
                            color: rgb(36,217,127);
                            font-weight: bold;
                        }
                    }
                    .desc {
                        font-size: 14px;
                        text-align: justify;
                        margin-bottom: 35px;
                    }
                    .btn {
                        font-size: 14px;
                        button {
                            background: rgb(36,217,127);
                            font-size:14px;
                            padding:5px 15px;
                            border:none;
                            outline: none;
                            border-radius: 5px;
                            cursor:pointer;
                            margin-left: 10px;
                        }
                    }
                }
            }
        }
}
</style>