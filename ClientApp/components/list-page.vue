<template>
  <div class="top-slider">
    <div class="container header">
      <h1 class="mr-3">发现项目</h1>
      <h2>共{{ total }}个项目</h2>
      <div class="header-float">
        <select v-model="status" class=" form-control d-inline-block">
          <option value="all">全部</option>
          <option value="not_started">尚未开始</option>
          <option value="in_progress">正在进行</option>
          <option value="over">已经结束</option>
        </select>
        <select v-model="ranking" class="header-right form-control d-inline-block">
          <option value="latest">最新上线</option>
          <option value="money">金额最高</option>
        </select>
      </div>
    </div>
    <incubatorList @monitorTotal="getData" v-bind:skip="skip" v-bind:take="take" v-bind:ranking="ranking" v-bind:status="status"></incubatorList>
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
        skip: 0,
        take: 12,
        ranking: "money",
        status: "all",
        total: 0,
      }
    },
    methods: {
      getData(data) {
        this.total = data;
      },
    },
    watch: {
    },
  }
</script>

<style>
  .header { padding-top: 51px; margin-bottom: 32px; }
    .header h1 { display: inline; font-size: 24px; font-weight: 500; line-height: 33px; }
    .header h2 { display: inline; font-size: 14px; font-weight: 400; color: #bdbdbd; line-height: 20px; }
    .header select { width: 140px; margin-right: 20px; font-size: 14px; font-weight: 400; }
    .header option { margin-top: 10px; border-radius: 5px; }
  .header-float { float: right; }
  .header-right { margin-right: 40px !important; }
</style>
