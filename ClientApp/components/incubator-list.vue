<template>
    <div class="container">
      <div class="project-list">
        <router-link v-for="x in list" class="project-card" tag="div" :to="x.url">
          <img class="project-cover" :src="x.cover" />
          <div class="project-description">
            <h1>{{ x.id }}</h1>
            <img class="project-Avatar" :src="x.avatar" />
            <p class="project-Introduction">{{x.introduction}}</p>
            <p v-if="x.status" class="project-blue">{{ x.targetAmount }} EOS<span class="project-percentage"> {{ x.percentage.toFixed(1) }}%</span></p>
            <p v-else class="project-blue">{{ x.startTime }} 开始</p>
            <progress v-if="x.status" class="project-Progress" :value="x.percentage" max="100"> </progress>
            <hr v-else />
            <p class="project-gray">{{ x.numberOfSupporters }} 人已看好</p>
          </div>
        </router-link>
      </div>
    </div>
</template>

<script>
  export default {
    computed: {
    },
    props: [
      "skip",
      "take",
      "ranking",
      "status",
    ],

    data() {
      return {
        list: [],
        //skip: 0,
        //take: 12,
        lang: 'zh',
        //ranking: 'money',
        myDate: null,
      }
    },

    methods: {
      getTime: function (t) {
        var _time = new Date(t);
        var year = _time.getFullYear();
        var month = _time.getMonth() + 1;
        var date = _time.getDate();
        return year + "/" + month + "/" + date;
      },

      async readData() {
        this.list = [];
        if (this.$root.lang == "zh_tw") {
          this.lang = "zh-Hant";
        } else {
          this.lang = this.$root.lang;
        }
        this.myDate = new Date();
        var self = this;
        await this.$http.get(`/api/v1/lang/${this.lang}/Incubator/list?ranking=${this.ranking}&status=${this.status}&skip=${this.skip}&take=${this.take}`)
          .then(x => {

            for (var i = 0; i < x.data.data.length; i++) {
              var item = x.data.data[i];

              item.startTime = new Date(item.startTime);

              item.deadLine = new Date(item.deadLine);
              item.cover = "/token_assets/" + item.id + "/slides/1." + self.lang + ".png";
              item.avatar = "/token_assets/" + item.id + "/icon.png";
              item.status = item.startTime < self.myDate;
              item.url = "/detail/" + item.id;
              if (item.targetAmount != 0) {
                item.percentage = (item.targetAmount / item.targetCredits) * 100;
              } else {
                item.percentage = 0;
              }
              item.startTime = self.getTime(item.startTime);
              self.list.push(item);
            }

          });

      }
    },
    watch: {
      '$root.lang': function () {
        this.readData();
      },
      status: function() {
        this.readData();
      },
      ranking: function () {
        this.readData();
      },
      take: function () {
        this.readData();
      },
      skip: function () {
        this.readData();
      }
    },

    async created() {
      this.readData();
    }
  }
</script>

<style>
  .project-list:after {
    content: " ";
    display: block;
    height: 0;
    clear: left;
    margin-bottom: 106px;
  }

  .project-card {
    float: left;
    width: 256px;
    height: 393px;
    background: #FFFFFF;
    box-shadow: 0px 1px 10px 2px #ABABAB;
    margin-right: 21px;
    margin-bottom: 22px;
  }
    .project-card:hover {
      background: #d8d8d8;
    }
    .project-card h1 {
      display: inline;
      font-size: 16px;
      font-weight: 500;
    }
  .project-Introduction {
    width: 100%;
    height: 51px;
    font-size: 12px;
    font-family: Helvetica;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
  }
  .project-Avatar {
    float: right;
    width: 28px;
    height: 28px;
    border-radius: 100px;
  }
  .project-description {
    margin: 16px 16px 23px 16px;
  }
  .project-blue {
    height: 17px;
    font-size: 12px;
    font-family: PingFangSC-Medium;
    font-weight: 500;
    color: #55B0C3;
    margin-bottom: 0px;
  }
  .project-gray {
    font-size: 12px;
    font-family: PingFangSC-Regular;
    font-weight: 400;
    color: #bdbdbd;
    line-height: 17px;
  }
  .project-Progress {
    width: 100%;
    height: 6px;
    margin: 13px 0px 9px 0px;
    background: #FFFFFF;
  }
    .project-Progress::-webkit-progress-bar {
      background-color: #e2e2e2;
    }

    .project-Progress::-webkit-progress-value {
      background: linear-gradient(to right,#2a82b7, #3ce3ff);
    }
  .project-percentage {
    float: right;
    color: #bdbdbd;
  }
  .project-cover {
    width: 256px;
    height: 191px;
  }
</style>
