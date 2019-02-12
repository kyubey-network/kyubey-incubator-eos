<template>
  <footer class="page-footer">
    <div class="container  footer-top">
      <div class="d-flex">
        <div class=" footer-leftbox ">
          <p class="footer-header">{{$t('About')}}</p>
          <router-link tag="a" :to="'/about-us'" class="footer-subheader bottom-hyperlink">{{$t('About us')}}</router-link>
          <p class="footer-subheader">{{$t('Join us')}}</p>
        </div>
        <div class=" footer-leftbox ">
          <p class="footer-header">{{$t('Help')}}</p>
          <p class="footer-subheader">{{$t('Common problems')}}</p>
          <p class="footer-subheader">{{$t('How to set up a project?')}}</p>
        </div>
        <div class="ml-auto footer-rightbox">
          <p class="footer-header">订阅我们</p>
          <p class="footer-subheader">留下您的电子邮件地址，订阅我们。将会收到我们的最新动态。感谢您的关注；）</p>
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
    methods: {

    },
    created() {
    }
  }
</script>

<style scoped>
  .bottom-hyperlink {
    color: black;
  }
  .bottom-hyperlink:hover {
    text-decoration: underline;
  }
</style>
