<template>
  <div>
    <div id="homeSlider" class="carousel slide" data-ride="carousel">
      <ol class="carousel-indicators">
        <li data-target="#homeSlider" data-slide-to="0" class="active"></li>
        <li data-target="#homeSlider" data-slide-to="1"></li>
        <li data-target="#homeSlider" data-slide-to="2"></li>
        <li data-target="#homeSlider" data-slide-to="3"></li>
      </ol>
      <div class="carousel-inner">
        <div class="carousel-item active">
          <img class="d-block w-100" src="/img/banners/cryptomeetup/bg.png" alt="First slide">
          <img class="carousel-content" :src="'/img/banners/cryptomeetup/front.' + lang + '.png'" alt="First slide">
        </div>
        <div class="carousel-item">
          <img class="d-block w-100" src="/img/banners/dex/bg.png" alt="Second slide">
          <img class="carousel-content" :src="'/img/banners/dex/front.' + lang + '.png'" alt="First slide">
        </div>
        <div class="carousel-item">
          <img class="d-block w-100" src="/img/banners/incubator/bg.png" alt="Third slide">
          <img class="carousel-content" :src="'/img/banners/incubator/front.' + lang + '.png'" alt="First slide">
        </div>
        <div class="carousel-item">
          <img class="d-block w-100" src="/img/banners/mic/bg.png" alt="Third slide">
          <img class="carousel-content" :src="'/img/banners/mic/front.' + lang + '.png'" alt="First slide">
        </div>
      </div>
      <a class="carousel-control-prev" href="#homeSlider" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
      </a>
      <a class="carousel-control-next" href="#homeSlider" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
      </a>
    </div>
    <div class="container home-title">
      <h1>发现项目</h1>
      <router-link :to="'/list'">
        查看更多 >>
      </router-link>
    </div>
    <incubatorList skip="0" take="12" ranking="money" status="all"></incubatorList>
    <toTop speed="80" position="50"></toTop>
  </div>
</template>

<script>
  import incubatorList from './incubator-list.vue'
  import toTop from './to-top.vue'
  export default {
    components: {
      incubatorList,
      toTop
    },
    data() {
      return {
        lang: 'zh'
      }
    },
    watch: {
      '$root.lang': function () {
        if (this.$root.lang == "zh_tw") {
          this.lang = "zh-Hant";
        } else {
          this.lang = this.$root.lang;
        }
      }
    }
  }
</script>

<style>
  .home-title {
    margin-top: 48px;
    margin-bottom: 32px;
  }

    .home-title h1 {
      display: inline;
      font-size: 24px;
      font-weight: 500;
    }

    .home-title a {
      font-size: 14px;
      float: right;
      padding-top: 10px;
      font-weight: 400;
      margin-right: 36px;
    }

  .carousel-inner {
    width: 100%;
    max-height: 350px !important;
  }
  .carousel img {
    max-height: 350px;
    margin: 0 auto;
  }
  .carousel-content {
    z-index: 100;
    position: absolute;
    top: 0;
    right: 15%;
    bottom: 20px;
    left: 15%;
    padding-bottom: 20px;
  }
  .carousel-item {
    text-align: center;
  }
  #homeSlider {
    margin-top: 60px;
  }
</style>
