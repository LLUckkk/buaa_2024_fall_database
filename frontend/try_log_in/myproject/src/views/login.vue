<template>
    <div class="login-container">
        <div class="form">
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
        <div class="form"></div>
    </div>
</template>

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
.form {
    width: 400px;
    height: 500px;
    background: rgba(30, 61, 212, 0.75);
    //设置模糊效果
    backdrop-filter: blur(79px) saturate(0);
    // 圆角
    border-radius:10px;
    //描边
    border:1px solid rgba(255,255,255,0.15);
    padding: 40px 60px;
    box-shadow: rgba(50,50,93,0.25) 50px 50px 100px -20px,rgba(0,0,0,0.5) 30px 30px 60px -30px,rgba(212,217,222,0.35) 2px -2px 6px 0px inset;
    //居中设置
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: flex-start;
    .title {
        font-size: 18px;
        color: rgb(246,240,255);
        letter-spacing: 1px;
        font-weight: 300;
    }
    .sub-title {
        font-size: 11px;
        color: rgb(246,240,255);
        letter-spacing: 1px;
        margin-bottom: 50px;
    }
    .inputf {
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
}
}
</style>