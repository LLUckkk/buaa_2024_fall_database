export default {
// 获取当前可视范围的高度
  getClientHeight() {
    var clientHeight = 0;
    var container = document.querySelector('.feeds-container');
    if (container) {
      clientHeight = container.clientHeight;
    }
    return clientHeight;
  },

  // 获取文档完整的高度
  getScrollHeight() {
    var container = document.querySelector('.feeds-container');
    return container ? container.scrollHeight : 0;
  },

  // 获取当前滚动条的位置
  getScrollTop() {
    var scrollTop = 0;
    var container = document.querySelector('.feeds-container');
    if (container) {
      scrollTop = container.scrollTop;
    }
    return scrollTop;
  }
}