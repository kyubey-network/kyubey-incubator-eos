<template>
  <footer class="page-footer">
    <div class="container  footer-top">
      <div class="d-flex">
        <div class=" footer-leftbox ">
          <p class="footer-header">{{$t('About')}}</p>
          <p class="footer-subheader"><router-link :to="'/about-us'" class="bottom-hyperlink">{{$t('About us')}}</router-link></p>
          <p class="footer-subheader"> <router-link :to="'/join-us'" class="bottom-hyperlink">{{$t('Join us')}}</router-link></p>
        </div>
        <div class=" footer-leftbox ">
          <p class="footer-header">{{$t('Help')}}</p>
          <p class="footer-subheader"><a href="#" class="bottom-hyperlink">{{$t('Common problems')}}</a></p>
          <p class="footer-subheader"><a href="#" class="bottom-hyperlink">{{$t('How to set up a project?')}}</a></p>
        </div>
        <div class="ml-auto footer-rightbox">
          <p class="footer-header">{{$t('Website subscription')}}</p>
          <p class="footer-subheader">{{$t('Subscribe.Text')}}</p>
          <p class="footer-subheader"><input type="text" class="email-input" v-model="inputVal" v-bind:placeholder="$t('Email Address')" /></p>
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
        var _this = this;
        if (this.inputVal == null) {
          this.$message({
            type: 'error',
            message: _this.$t('Please enter your email address')
          });
          return;
        }
        this.$message({
          type: 'success',
          message: _this.$t('Subscribe succeed')
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
  .bottom-hyperlink { color: black; }
    .bottom-hyperlink:hover { text-decoration: underline; }
</style>
