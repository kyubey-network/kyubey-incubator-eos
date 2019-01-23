<template>
  <div class="top-slider">
    <div class="container header">
    <h1>发现项目</h1>
    <h2>共{{ total }}个项目</h2>
    <div class="header-float">
      <select v-model="status">
        <option value="all">全部</option>
        <option value="not_started">预热</option>
        <option value="in_progress">众筹中</option>
        <option value="over">众筹结束</option>
      </select>
      <select v-model="ranking">
        <option value="latest">最新上线</option>
        <option value="money">金额最高</option>
      </select>
    </div>
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
        total: 0,
      }
    },
    methods: {
      async readData() {
        this.list = [];
        if (this.$root.lang == "zh_tw") {
          this.lang = "zh-Hant";
        } else {
          this.lang = this.$root.lang;
        }
        this.myDate = new Date();
        var self = this;
        await this.$http.get(`/api/v1/lang/${this.lang}/Incubator/list/total?ranking=${this.ranking}&status=${this.status}`)
          .then(x => {
            this.total = x.data.data.total;
          });

      }
    },
    watch: {
      status: function () {
        this.readData();
      },
    },

    async created() {
      this.readData();
    }
  }
</script>

<style>
  .header {
    padding-top: 51px;
    margin-bottom: 32px;
  }
    .header h1 {
      display: inline;
      font-size: 24px;
      font-weight: 500;
      line-height: 33px;
    }
    .header h2 {
      display: inline;
      font-size: 14px;
      font-weight: 400;
      color: #bdbdbd;
      line-height: 20px;
    }
    .header select {
      padding-bottom: 3px;
      margin-right: 20px;
      border-radius: 5px;
      font-size: 14px;
      font-weight: 400;
      line-height: 20px;
      text-align: right;
    }
    .header option {
      margin-top: 10px;
      border-radius: 5px;
    }
  .header-float {
    padding-top: 10px;
    float: right;
  }
</style>
