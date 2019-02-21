<template>
  <div class="container page-containner">
    <div class="page-content">
      <div class="row container">
        <div class="detail-left">
          <h1 class="detail-header">{{tokenId}}</h1>
          <div class="detail-slider">
            <el-carousel trigger="click" style="width:100%;height:100%;" height="100%">
              <el-carousel-item v-for="item in wording.sliders" :key="item">
                <img :src="item" style="width:100%; height:100%;" />
              </el-carousel-item>
            </el-carousel>
          </div>
          <div class="token-description">
            {{wording.description}}
          </div>
        </div>
        <div class="detail-right">
          <detail-operator></detail-operator>
        </div>
      </div>
      <div class="row container content-containner">
        <div class="detail-left">
          <ul class="detail-nav">
            <li v-bind:class="{ active: currentNav=='detail' }" @click="currentNav='detail'"><a>{{$t('Detail')}}</a></li>
            <li v-bind:class="{ active: currentNav=='update' }" @click="currentNav='update'"><a>{{$t('Recent updates')}}</a></li>
            <!--<li v-bind:class="{ active: currentNav=='comments' }" @click="currentNav='comments'"><a>用户评论</a></li>-->
          </ul>
          <div v-if="currentNav=='detail'" class="page-bottom-left-box" v-html="marked(wording.detail)">

          </div>
          <div v-if="currentNav=='update'" class="">
            <div v-for="item in wording.updates">
              <h1>{{item.time| formatDate}} {{item.title}}</h1>
              <span v-html="marked(item.content)"></span>
            </div>
          </div>
          <div v-if="currentNav=='comments'" class="">
            comments
          </div>
        </div>
        <div class="detail-right">
          <div class="warning-tip-title">
            {{$t('Risk warning')}}
          </div>
          <div class="warning-tip-content" v-html="$t('Risk warning content')"></div>
        </div>
      </div>
    </div>
    <toTop speed="160" position="50"></toTop>
  </div>

</template>

<script>
  import marked from 'marked'
  import toTop from './to-top.vue'
  import { formatDate } from '../common/date.js'
  import DetailOperator from './detail-operator'

  marked.setOptions({
    "baseUrl": null,
    "breaks": false,
    "gfm": true,
    "headerIds": true,
    "headerPrefix": "",
    "highlight": null,
    "langPrefix": "language-",
    "mangle": true,
    "pedantic": true,
    "sanitize": false,
    "sanitizer": null,
    "silent": false,
    "smartLists": false,
    "smartypants": false,
    "tables": true,
    "xhtml": false
  });

  export default {
    components: {
      'detail-operator': DetailOperator,
      toTop
    },
    data() {
      return {
        currentNav: 'detail',
        tokenId: null,
        wording: {

        }
      };
    },
    filters: {
      formatDateTime: function (timeStr) {
        if (typeof timeStr == 'undefined') {
          return null;
        }
        let date = new window.Date(timeStr);
        return formatDate(date, 'yyyy-MM-dd hh:mm')
      },
      formatDate: function (timeStr) {
        if (typeof timeStr == 'undefined') {
          return null;
        }
        let date = new window.Date(timeStr);
        return formatDate(date, 'yyyy-MM-dd')
      }
    },
    methods: {
      marked: function (input) {
        if (input == null)
          return null;
        return marked(input);
      },
      async getWording() {
        var _this = this;
        await this.$http.get(`/api/v1/lang/${_this.$root.lang}/Incubator/wording/${_this.tokenId}`).then(res => {
          if (res.status == 200) {
            _this.wording = res.data;
          }
        });
      }
    },
    watch: {
      '$root.lang': function () {
        var _this = this;
        _this.getWording();
      }
    },
    created() {
      this.tokenId = this.$route.params.id;
      this.getWording();
    }
  }

</script>
<style>
  .page-bottom-left-box img { max-width: 100%; }
  .content-containner img { max-width: 100%; }
  .content-containner h1 { font-size: 20px; }
  .content-containner h2 { font-size: 18px; }
  .content-containner h3 { font-size: 16px; }
  .content-containner h4 { font-size: 14px; }
  .content-containner h5 { font-size: 14px; }
  .content-containner p { font-size: 14px; }
</style>

<style scoped>

  .page-bottom-left-box { max-width: 100%; }
  .token-description { margin-top: 19px; }
  .detail-right { width: 438px; display: inline-block; margin-left: 58px; }
  .container { max-width: 1440px; }
  .page-containner { margin-top: 60px; min-width: 1440px; }
  .detail-header { font-size: 24px; margin-bottom: 15px; }
  .page-content { padding-top: 50px; }
  .detail-slider { height: 586px; background-color: gray; }
  .detail-left { width: 797px; display: inline-block; }
  .content-containner { margin-top: 38px; margin-bottom: 40px; }
  .detail-nav { font-size: 14px; list-style: none; flex-wrap: wrap; margin: 0; padding: 0; display: flex; margin-bottom: 30px; }
    .detail-nav li { list-style-type: none; display: list-item; margin-right: 30px; }
      .detail-nav li.active { font-weight: bold; border-bottom: 2px solid rgba(66,66,66,1); }
      .detail-nav li:hover { font-weight: bold; }

  .warning-tip-title { font-size: 14px; font-weight: 600; color: rgba(0,0,0,1); line-height: 20px; }
  .warning-tip-content { font-size: 14px; font-weight: 400; color: rgba(0,0,0,1); line-height: 20px; margin-top: 30px; }
</style>
