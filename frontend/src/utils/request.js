import axios from "axios";
import router from '../router'
import {Notification} from 'element-ui';
import Cookies from "js-cookie";
import "../style.css";

const Instance = axios.create({
  baseURL: 'http://localhost:5050',

  timeout: 5000
})

// 添加请求拦截器
Instance.interceptors.request.use(function (config) {
  // 在发送请求之前做些什么

  const token = Cookies.get('web-token')
  config.headers.set('token', token ?? '')
  return config;
});

// 添加响应拦截器
Instance.interceptors.response.use(function (response) {
  if (response.data.code === 1) {
    //1.操作成功
    return response.data;
  } else {
    //2.操作失败
    if (response.data.code === 1001007 || response.data.code === 1001008) {
      router.push('/login')
    } else {
      Notification({type: 'warning', title: '异常', message: response.data.msg, customClass: 'http-message'})
      return Promise.reject(response.data)
    }
  }
}, function (error) {
  //状态码超过200
  Notification({type: 'error', title: '错误', message: error.response.data.error, customClass: 'http-message'})
  // router.push('/404')
  return Promise.reject(error)
});

export default Instance