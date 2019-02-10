import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
  counter: 1
}

const loginStateModule = {
  namespaced: true,
  state: {
    eosLoginState: {}
  },
  getters: {
    isEosLogin: state => {
      if (typeof state.eosLoginState.account !== 'undefined' && state.eosLoginState.account !== null && state.eosLoginState.account.name != null) {
        return true
      }
      return false
    },
    eosUsername: (state, getters) => {
      if (getters.isEosLogin) {
        return state.eosLoginState.account.name
      }
      return null
    }
  },
  mutations: {},
  actions: {
    eosLogin: function (context, { account, loginMode, eos, requiredFields, eosScatter }) {
      context.state.eosLoginState = {
        account,
        loginMode,
        eos,
        requiredFields,
        eosScatter
      }
    },
    eosLogout: function (context) {
      context.state.eosLoginState.eosScatter.forgetIdentity()
      context.state.eosLoginState.account = null
      context.state.eosLoginState.loginMode = null
      context.state.eosLoginState.eos = null
      context.state.eosLoginState.requiredFields = null
    }
  }
}

// MUTATIONS
const mutations = {
  [MAIN_SET_COUNTER](state, obj) {
    state.counter = obj.counter
  }
}

// ACTIONS
const actions = ({
  setCounter({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj)
  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions,
  modules: {
    loginState: loginStateModule
  }
})
