<template>
  <div>
    <nav class="navbar navbar-expand-lg navbar-light bg-light fixed-top navbar-custom">
      <router-link class="navbar-brand" :to="'/'">
        <img src="/img/KYUBEY_logo.png" alt="" style="width: 50px;" />
      </router-link>
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarText">
        <ul class="navbar-nav mr-auto">
          <li class="nav-item ">
            <a class="nav-link" href="#">实验室</a>
          </li>
          <router-link tag="li" class="nav-item" :to="'/list'" exact-active-class="active">
            <a class="nav-link" href="#">发现项目</a>
          </router-link>
          <router-link tag="li" class="nav-item" :to="'/publish'" exact-active-class="active">
            <a class="nav-link" href="#">发布项目</a>
          </router-link>
        </ul>
        <button type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation" class="navbar-toggler"><span class="navbar-toggler-icon"></span></button>
        <div id="navbarResponsive" class="collapse navbar-collapse">
          <ul class="navbar-nav mr-auto">
            <li class="nav-item">
              <a href="/dex" class="nav-link"></a>
            </li>
          </ul>
          <ul class="navbar-nav ">
            <li class="nav-item">
              <a class="nav-link" @click="showLoginModal">登录</a>
            </li>

            <li class="nav-item dropdown">
              <a href="#" id="navbarDropdownMenuLink" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" class="nav-link dropdown-toggle">
                简体中文
              </a>
              <div aria-labelledby="navbarDropdownMenuLink" class="dropdown-menu">
                <a class="dropdown-item">English</a>
                <a class="dropdown-item">简体中文</a>
                <a class="dropdown-item">繁體中文</a>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </nav>
    <loginModal ref="loginModalRef" :isShow="loginModal.isShow"></loginModal>
  </div>
</template>

<script>
  import LoginModal from './login'
  export default {
    components: {
      'loginModal': LoginModal
    },
    methods: {
      showLoginModal() {
        this.$refs.loginModalRef.isShow = true;
      }
    },
    data() {
      return {
        loginModal: {
          isShow: false
        }
      }
    },
    created() {
    }
  }
</script>

<style scoped>
</style>
