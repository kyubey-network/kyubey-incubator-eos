<template>
    <div class="container top-slider">
      <h1>{{$t('Join us')}}</h1>
      <div class="join-us-text">
        <div class="container-div1">
          <h2>
            前端工程师
          </h2>
          <p>
            负责前端页面的实现与区块链合约交互；有扎实的HTML、CSS和Javascript基础，熟悉各大主流前端框架。
          </p>
          <h2>
             后端工程师
          </h2>
          <p>
            负责开发和完善项目的后端，组织系统架构与升级优化。 熟悉一门高性能静态语言，如C++、rust。对linux、网络、分布式系统、加密算法有项目经验和深入的理解者优先。
          </p>
          <h2>
            算法工程师
          </h2>
          <p>
            研究实现区块链底层、跨链技术、金融建模、加密算法；熟练开发主流公链合约并与后端对接。 熟悉C++等主流编程语言，有以上相关算法的研究实现经历。 有相关密码学论文和金融从业经验者优先。 有ACM/ICPC和CCPC区域现场赛银牌以上者优先。
          </p>
        </div>

        <div class="container-div2">
          <h2>
            产品经理
          </h2>
          <p>
            2年以上工作经验，负责产品的构思及原型设计，资源协调与监督技术团队的进度；需要对区块链及行业生态有深入的认识与研究，沟通能力满点。
          </p>
          <h2>
            市场运营
          </h2>
          <p>
            负责项目的营销与推广，连接各路资源，将市场反馈给技术，把技术推向市场；2年及以上市场运营经验，并独立完成过大型项目的运营推广。
          </p>
          <h2>
            UI/UX设计师
          </h2>
          <p>
            项目的“颜值担当”。负责产品的视觉设计/交互设计方案；有良好的设计sense和个人的风格，精通Sketch、AI等视觉设计工具。简历里请附上作品集。
          </p>
        </div>
      </div>
      <div>
        <p class="contact-details">
          简历请发送至 <a href="mailto:hiring@andoromeda.io">hiring@andoromeda.io</a>
        </p>
      </div>
    </div>
</template>

<script>
</script>

<style  scoped>
  .container h1 {
    font-size: 28px;
    font-weight: 600;
    line-height: 40px;
    padding-top: 58px;
    margin-bottom: 54px;
  }
  .join-us-text h2 {
    font-size: 16px;
    font-weight: 600;
    line-height: 22px;
    margin-bottom: 9px;
  }
  .join-us-text p {
    font-size: 12px;
    font-weight: 400;
    line-height: 17px;
    padding-bottom: 13px;
    color: #676767;
  }
  .container-div1 {
    width: 464px;
    float: left;
    padding-right: 61px;
  }
  .container-div2 {
    width: 403px;
    float: left;
  }
  .container {
    min-height: 792px;
  }
  .join-us-text:after {
    content: " ";
    display: block;
    height: 0;
    clear: left;
    margin-bottom: 37px;
  }
  .contact-details {
    font-size: 14px;
    font-weight: 400;
  }
  .contact-details a{
    font-weight: 600;
  }
</style>
