<template>
  <div class="top-slider">
    <h1></h1>
    <p> </p>
    <div>
      <select onchange="checkField(this.value)">
        <option value="all">全部</option>
        <option value="not_started">预热</option>
        <option value="in_progress">众筹中</option>
        <option value="over">众筹成功</option>
      </select>
      <select>
        <option value="latest">最新上线</option>
        <option value="money">金额最高</option>
      </select>
    </div>
    <incubatorList v-bind:skip="skip" v-bind:take="take" v-bind:ranking="ranking" v-bind:status="status"></incubatorList>
  </div>
</template>

<script>
  import incubatorList from './incubator-list.vue'
  export default {
    components: {
      incubatorList
    },
    data() {
      return {
        skip: 0,
        take: 12,
        ranking: "money",
        status: "all",
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
