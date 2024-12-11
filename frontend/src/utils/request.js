import axios from "axios";
import router from '../router'
import { ElNotification } from 'element-plus';
import "../style.css";
import { API_URL } from "@/config";

const Instance = axios.create({
  baseURL: API_URL,
  timeout: 5000,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  }
})

Instance.interceptors.request.use(function (config) {
  return config;
});

// 添加响应拦截器
Instance.interceptors.response.use(function (response) {
  if (response.data.code === 1) {
    //1.操作成功
    return response.data;
  } else {
    //2.操作失败
    if (response.data.Code === 1001007 || response.data.Code === 1001008) {
      router.push('/login')
    } else {
      ElNotification({type: 'warning', title: '异常', message: response.data.Message, customClass: 'http-message'})
      return Promise.reject(response.data)
    }
  }
}, function (error) {
  //状态码超过200
  ElNotification({type: 'error', title: '错误', message: error.response.data.Message, customClass: 'http-message'})
  // router.push('/404')
  return Promise.reject(error)
});

export default Instance