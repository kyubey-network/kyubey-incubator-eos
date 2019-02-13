<template>
  <footer class="page-footer">
    <div class="container  footer-top">
      <div class="d-flex">
        <div class=" footer-leftbox ">
          <p class="footer-header">{{$t('About')}}</p>
          <router-link tag="a" :to="'/about-us'" class="footer-subheader bottom-hyperlink">{{$t('About us')}}</router-link>
          <router-link tag="a" :to="'/join-us'" class="footer-subheader bottom-hyperlink">{{$t('Join us')}}</router-link>
        </div>
        <div class=" footer-leftbox ">
          <p class="footer-header">{{$t('Help')}}</p>
          <p class="footer-subheader">{{$t('Common problems')}}</p>
          <p class="footer-subheader">{{$t('How to set up a project?')}}</p>
        </div>
        <div class="ml-auto footer-rightbox">
          <p class="footer-header">{{$t('Website subscription')}}</p>
          <p class="footer-subheader">{{$t('Subscribe.Text')}}</p>
          <p class="footer-subheader"><input type="text" class="email-input" v-model="inputVal" placeholder="Email Address" /></p>
          <p class="footer-subheader"><button class="email-button" @click="subscribe">{{$t('Subscribe')}}</button></p>
        </div>
      </div>
    </div>
    <div class="container-fluid  footer-bottom">
      <div class="row">
        <div class="col-3"></div>
        <div class="col-6">
          <p class="m-0 text-center footer-text-color small">Copyright © Kyubey 2019.All rights reserved.</p>
        </div>
        <div class="col-3"></div>
      </div>
    </div>
  </footer>
</template>
<script>
  export default {
    components: {

    },
    data() {
      return {
        inputVal: null,
      };
    },
    methods: {
      subscribe: function () {
        if (this.inputVal == null) {
          this.$message({
            type: 'error',
            message: '请输入你的Email地址'
          });
          return;
        }
        this.$message({
          type: 'success',
          message: '订阅成功'
        });
      }
    },
    created() {
    }
  }
</script>

<style scoped>
    .email-button { width: 265px; height: 35px; background-color: rgba(77,77,77,1); border: 0; margin-top: 6px; font-weight: 500; color: rgba(231,231,231,1); }
    .email-input { width: 265px; height: 35px; font-size: 14px; padding: 7px; }
    .bottom-hyperlink { color: black; display:block; }
      .bottom-hyperlink:hover { text-decoration: underline; }
</style>
