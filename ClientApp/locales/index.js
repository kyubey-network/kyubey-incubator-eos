import Vue from 'vue'
import VueI18n from 'vue-i18n'

Vue.use(VueI18n)

const locales = {
  zh: require('./zh.json'),
  en: require('./en.json'),
  zh_tw: require('./zh_tw.json')
}

const i18n = new VueI18n({
  locale: 'zh', // set locale
  fallbackLocale: 'en',
  messages: locales // set locale messages
})

export default i18n
