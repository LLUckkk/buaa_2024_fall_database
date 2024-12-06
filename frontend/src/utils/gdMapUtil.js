import AMapLoader from "@amap/amap-jsapi-loader";

export default {
  initAMap() {
    return AMapLoader.load({
      key: "4109193117e002b59e3158567dc68992",
      version: "2.0",
      plugins: [],
    })
  },
  getCity() {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin('AMap.CitySearch', () => {
          var citySearch = new AMap.CitySearch({
            enableHighAccuracy: true, // 是否使用高精度定位，默认：true
            timeout: 10000, // 设置定位超时时间，默认：无穷大
            offset: [10, 20],  // 定位按钮的停靠位置的偏移量
            zoomToAccuracy: true,  //  定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
            position: 'RB' //  定位按钮的排放位置,  RB表示右下
          })
          citySearch.getLocalCity((status, result) => {
            if (status === 'complete') {
              resolve(result)
            } else {
              reject(new Error('get city error'));
            }
          });
        })
      }).catch(error => reject(error))
    })
  },
  getLocationCity() {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin('AMap.Geolocation', () => {
          let geolocation = new AMap.Geolocation({
            enableHighAccuracy: true, // 是否使用高精度定位，默认：true
            timeout: 10000, // 设置定位超时时间，默认：无穷大
            offset: [10, 20],  // 定位按钮的停靠位置的偏移量
            zoomToAccuracy: true,  //  定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
            position: 'RB' //  定位按钮的排放位置,  RB表示右下
          })
          geolocation.getCityInfo((status, result) => {
            if (status === 'complete') {
              resolve(result)
            } else {
              reject(new Error('get location error'));
            }
          });
        })
      }).catch(error => reject(error))
    })
  },
  getSearchOptions(query) {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin(["AMap.AutoComplete"], () => {
          let autocomplete = new AMap.AutoComplete();
          autocomplete.search(query, (status, result) => {
            if (status === 'complete') {
              resolve(result)
            } else {
              reject(new Error('get search error'));
            }
          })
        });
      }).catch(error => reject(error))
    })
  },
  setMarker(lng, lat) {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin(["AMap.Marker"], () => {
          const marker = new AMap.Marker({
            position: new AMap.LngLat(lng, lat), //经纬度对象，也可以是经纬度构成的一维数组[116.39, 39.9]
          });
          resolve(marker)
        })
      }).catch(error => reject(error))
    })
  },
  getPlaceSearch(keyword) {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin(["AMap.PlaceSearch"], () => {
          let placeSearch = new AMap.PlaceSearch();
          placeSearch.search(keyword, (status, result) => {
            if (status === 'complete') {
              resolve(result)
            } else {
              reject(new Error('get place search error'));
            }
          })
        })
      }).catch(error => reject(error))
    })
  },
  getPlaceSearchDetail(poiid) {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin(["AMap.PlaceSearch"], () => {
          let placeSearch = new AMap.PlaceSearch();
          placeSearch.getDetails(poiid, (status, result) => {
            if (status === 'complete') {
              resolve(result)
            } else {
              reject(new Error('get place search error'));
            }
          })
        })
      }).catch(error => reject(error))
    })
  },
  GeocoderGetAddress(lng, lat) {
    return new Promise((resolve, reject) => {
      this.initAMap().then(AMap => {
        AMap.plugin(['AMap.Geocoder'], () => {
          let geocoder = new AMap.Geocoder({})
          geocoder.getAddress([lng, lat], function (status, result) {
            if (status === 'complete' && result.info === 'OK') {
              resolve(result)
            } else {
              reject(new Error('get geocoder address error'));
            }
          })
        })
      }).catch(error => reject(error))
    })
  },

  destroyMap() {
    this.map?.destroy();
  },
}