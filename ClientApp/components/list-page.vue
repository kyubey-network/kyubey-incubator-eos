<template>
  <div class="top-slider">
    <incubatorList skip="0" take="12" ranking="money" status="all"></incubatorList>
  </div>
</template>

<script>
  import incubatorList from './incubator-list.vue'
  export default {
    components: {
      incubatorList
    },
    data() {
      return {}
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
    }

  .carousel-inner {
    width: 100%;
    max-height: 350px !important;
  }
  .carousel img {
    max-height: 350px;
    margin: 0 auto;
  }

  /*.carousel {
    margin: 0 auto;
    border: 1px solid #000;
    max-width: 1440px;
  }*/
  #homeSlider { margin-top: 60px; }
</style>
