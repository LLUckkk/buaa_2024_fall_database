import axios from 'axios'
import router from "@/router";
import Cookies from "js-cookie";
import {Notification} from 'element-ui';

const Instance = axios.create({
  baseURL: 'http://localhost:5050',

  timeout: 10000
})

// 添加请求拦截器
Instance.interceptors.request.use(config => {
    const token = Cookies.get('admin-token')
    config.headers.set('token', token ?? '')
    return config;
  }
)


Instance.interceptors.response.use(function (response) {
  if(response.data.code === 1){
    //1.操作成功
    return response.data;
  }else{
    //2.操作失败
    Notification({type: 'warning', title: '警告', message: response.data.msg})
    if (response.data.code === 1001007 || response.data.code === 1001008) {
      router.push('/login')
    }else{
      return Promise.reject(response.data)
    }

  }
}, function (error) {
  //状态码超过200
  Notification({type: 'error', title: '错误', message: error.message})
  // router.push('/404')
  return Promise.reject(error)
});

export default Instance
