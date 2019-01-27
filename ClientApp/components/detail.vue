<template>
  <div class="container page-containner">
    <div class="page-content">
      <div class="row container">
        <div class="detail-left">
          <h1 class="detail-header">{{tokenId}}</h1>
          <div class="detail-slider">
            <el-carousel trigger="click" style="width:100%;height:100%;" height="100%">
              <el-carousel-item v-for="item in wording.sliders" :key="item">
                <img :src="item" style="width:100%; height:100%;" />
              </el-carousel-item>
            </el-carousel>
          </div>
          <div class="token-description">
            {{wording.description}}
          </div>
        </div>
        <div class="detail-right">
          <detail-operator></detail-operator>
        </div>
      </div>
      <div class="row container content-containner">
        <div class="detail-left">
          <ul class="detail-nav">
            <li v-bind:class="{ active: currentNav=='detail' }" @click="currentNav='detail'"><a>项目介绍</a></li>
            <li v-bind:class="{ active: currentNav=='update' }" @click="currentNav='update'"><a>更新记录</a></li>
            <!--<li v-bind:class="{ active: currentNav=='comments' }" @click="currentNav='comments'"><a>用户评论</a></li>-->
          </ul>
          <div v-if="currentNav=='detail'" class="page-bottom-left-box" v-html="marked(wording.detail)">

          </div>
          <div v-if="currentNav=='update'" class="">
            <div v-for="item in wording.updates">
              <h1>{{wording.updates.time}} {{wording.updates.title}}</h1>
              <span  v-html="marked(item.content)"></span>
            </div>
          </div>
          <div v-if="currentNav=='comments'" class="">
            comments
          </div>
        </div>
        <div class="detail-right">
          <div class="warning-tip-title">
            风险提示
          </div>
          <div class="warning-tip-content">
            1. 您参与众筹是支持将创意变为现实的过程，而不是直接的商品交易，因此存在一定风险。请您根据自己的判断选择、支持众筹项目。<br />
            2. 众筹存在于发起人与支持者之间，摩点网作为第三方平台，只提供网络空间、技术支持等服务。众筹的回报产品和承诺由发起人提供和作出，摩点网不参与和担保众筹项目的具体运作。<br />
            3. 众筹项目的回报发放及其他后续服务事项均由发起人负责。如果发生发起人无法发放回报、延迟发放回报、不提供回报后续服务等情形，您需要直接和发起人协商解决，摩点网对此不承担任何责任。<br />
            4. 由于发起人能力和经验不足、市场风险、法律风险等各种因素，众筹可能失败。对于在众筹期限届满前失败的项目，您支持项目的款项会全部原路退还给您；对于众筹成功的项目，支持者不能通过摩点平台申请退款，若此时支持者因任何原因希望退款，需直接与发起者协商，若发 起者同意退款，需直接向支持者退回款项，摩点不会从可结算款项中扣除该部分退款金额。您对项目发起人的无偿支持以及额外打赏，一旦众筹成功将不予退款，但众筹失败的情况除外…
          </div>
        </div>
      </div>
    </div>
  </div>

</template>

<script>
  import marked from 'marked'
  import DetailOperator from './detail-operator'
  export default {
    components: {
      'detail-operator': DetailOperator,
    },
    data() {
      return {
        currentNav: 'detail',
        tokenId: null,
        wording: {

        }
      };
    },
    methods: {
      marked: function (input) {
        return marked(input);
      },
      async getWording() {
        var _this = this;
        await this.$http.get(`/api/v1/lang/${_this.$root.lang}/Incubator/wording/${_this.tokenId}`).then(res => {
          if (res.status == 200) {
            _this.wording = res.data;
          }
        });
      }
    },
    created() {
      this.tokenId = this.$route.params.id;
      this.getWording();
    }
  }

</script>
<style>
  .page-bottom-left-box img { max-width: 100%; }
  .content-containner img { max-width: 100%; }
</style>

<style scoped>
  .page-bottom-left-box { max-width: 100%; }
  .token-description { margin-top: 19px; }
  .detail-right { width: 438px; display: inline-block; margin-left: 58px; }
  .container { max-width: 1440px; }
  .page-containner { margin-top: 60px; min-width: 1440px; }
  .detail-header { font-size: 24px; margin-bottom: 15px; }
  .page-content { padding-top: 50px; }
  .detail-slider { height: 586px; background-color: gray; }
  .detail-left { width: 797px; display: inline-block; }
  .content-containner { margin-top: 38px; margin-bottom: 40px; }
  .detail-nav { font-size: 14px; list-style: none; flex-wrap: wrap; margin: 0; padding: 0; display: flex; margin-bottom: 30px; }
    .detail-nav li { list-style-type: none; display: list-item; margin-right: 30px; }
      .detail-nav li.active { font-weight: bold; border-bottom: 2px solid rgba(66,66,66,1); }
      .detail-nav li:hover { font-weight: bold; }

  .warning-tip-title { font-size: 14px; font-weight: 600; color: rgba(0,0,0,1); line-height: 20px; }
  .warning-tip-content { font-size: 14px; font-weight: 400; color: rgba(0,0,0,1); line-height: 20px; margin-top: 30px; }
</style>
